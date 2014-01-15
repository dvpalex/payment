using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper.Xml.Update;
using Resp = SuperPag.Helper.Xml.Response;
using SuperPag.Helper;
using System.Reflection;
using SuperPag.Helper.Xml;

namespace SuperPag.Handshake.Service
{
    public class Update
    {
        public Resp.response ProcessUpdate(DStore store, update update)
        {
            Resp.response response = new Resp.response();
            response.orders = new Resp.responseOrders();

            if (update.orders.Length > 0)
                response.orders.order = new Resp.responseOrdersOrder[update.orders.Length];
            
            int count = 0;
            foreach (updateOrder order in update.orders)
            {
                //Checar se o Id passado corresponde a uma order
                DOrder dOrder = DataFactory.Order().Locate((long)order.id);
                if (dOrder == null)
                    Ensure.IsNotNull(null, "Pedido inválido");

                HelperService.CheckOrderByStore(store, dOrder);
                
                DOrderInstallment[] installments = DataFactory.OrderInstallment().List(dOrder.orderId);
                if (installments == null)
                    Ensure.IsNotNull(null, "O pedido não possui parcelamento válido");

                Resp.responseOrdersOrder responseOrder = null;
                Resp.responseOrdersOrderPayment responsePayment = null;

                if (order.Item == null && !order.totalSpecified)
                {
                    responseOrder = ProcessConsult(store, dOrder);
                    responsePayment = null;
                }
                else if (order.Item == null && order.totalSpecified)
                {
                    responseOrder = ProcessOrderAmount(store, dOrder, installments, GenericHelper.ParseDecimal(order.total, 2));
                    responsePayment = null;
                }
                else if (order.Item.GetType().Equals(typeof(updateOrderPayments)))
                {
                    //Regra de negocio: se o atributo 'total' da tag order estiver
                    //especificado, então o atributo 'amount' da tag payment também
                    //deverá ser especificado e vice versa, além disso, o valor do
                    //atributo 'total' da tag order deve ser igual ao valor do
                    //atributo 'amount' da tag payment
                    if((order.totalSpecified && !((updateOrderPayments)order.Item).payment.amountSpecified) ||
                       (!order.totalSpecified && ((updateOrderPayments)order.Item).payment.amountSpecified))
                        Ensure.IsNotNull(null, "Para alterar o valor do pedido, os atributos 'total' e 'amount' das tags 'order' e 'payment' devem ser especificados");
                    else if ((order.totalSpecified && ((updateOrderPayments)order.Item).payment.amountSpecified) && (order.total != ((updateOrderPayments)order.Item).payment.amount))
                        Ensure.IsNotNull(null, "Os atributos 'total' e 'amount' das tags 'order' e 'payment' estão inconsistentes");
                    
                    responsePayment = ProcessPayment(dOrder, installments, ((updateOrderPayments)order.Item).payment);
                    responseOrder = null;
                }
                else
                {
                    //Regra de negocio: se o atributo 'total' da tag order estiver
                    //especificado, então o atributo 'amount' da tag recurrence também
                    //deverá ser especificado e vice versa, além disso, o valor do
                    //atributo 'total' da tag order deve ser igual ao valor do
                    //atributo 'amount' da tag recurrence
                    if ((order.totalSpecified && !((updateOrderRecurrences)order.Item).recurrence.amountSpecified) ||
                       (!order.totalSpecified && ((updateOrderRecurrences)order.Item).recurrence.amountSpecified))
                        Ensure.IsNotNull(null, "Para alterar o valor do pedido, os atributos 'total' e 'amount' das tags 'order' e 'recurrence' devem ser especificados");
                    else if ((order.totalSpecified && ((updateOrderRecurrences)order.Item).recurrence.amountSpecified) && (order.total != ((updateOrderRecurrences)order.Item).recurrence.amount))
                        Ensure.IsNotNull(null, "Os atributos 'total' e 'amount' das tags 'order' e 'recurrence' estão inconsistentes");
                    
                    responsePayment = ProcessRecurrence(dOrder, installments, ((updateOrderRecurrences)order.Item).recurrence);
                    responseOrder = null;
                }

                if (responseOrder == null)
                {
                    responseOrder = new Resp.responseOrdersOrder();
                    responseOrder.id = (ulong)dOrder.orderId;
                    responseOrder.reference = dOrder.storeReferenceOrder;
                    responseOrder.total = GenericHelper.ParseLong(dOrder.finalAmount);
                    responseOrder.status = responsePayment.status;
                    responseOrder.payments = new Resp.responseOrdersOrderPayment[1];
                    responseOrder.payments[0] = responsePayment;
                }

                response.orders.order[count] = responseOrder;
                
                count++;
            }

            return response;
        }

        public Resp.responseOrdersOrder ProcessConsult(DStore store, DOrder order)
        {
            Check check = new Check();
            return check.GetResponseOrder(store, order.orderId);
        }
        public Resp.responseOrdersOrder ProcessOrderAmount(DStore store, DOrder order, DOrderInstallment[] installments, decimal newAmount)
        {
            //TODO: Regra de negocio: O cliente só podera alterar o valor do
            //pedido se o valor da soma dos itens for igual ao novo valor enviado
            //if (!OrderTotalEqualsItensTotal())
            //    throw new Exception("O valor da soma dos itens não corresponde ao novo valor total do pedido.");

            //Regra de negocio: O cliente nao podera alterar o valor de um
            //pedido que não seja recorrente caso tenha qualquer attempt
            //com status pago
            DRecurrence dRecurrence = DataFactory.Recurrence().Locate(order.orderId);
            if (dRecurrence == null)
            {
                //o pedido não é recorrente, validar a regra de negocio anterior
                DPaymentAttempt[] paidAttempts = DataFactory.PaymentAttempt().List(order.orderId, (int)PaymentAttemptStatus.Paid);
                if (paidAttempts != null)
                    Ensure.IsNotNull(null, "Não é possivel atualizar o valor de um pedido já pago ou parcialmente pago");
            }

            //TODO: Verificar possibilidade de analisar de há algum agendamento antes de atualizar
            // o valor

            UpdateOrderInstallment(order, installments, newAmount);

            Check check = new Check();
            return check.GetResponseOrder(store, order.orderId);
        }
        public Resp.responseOrdersOrderPayment ProcessPayment(DOrder order, DOrderInstallment[] installments, updateOrderPaymentsPayment payment)
        {
            //Regra de negocio: verificar se o atributo 'form' está presente, se sim
            //é necessário que a tag 'paymentFormDetail' também estaja presente e
            //seja condizente com a forma de pagamento enviada no atributo 'form'
            if (payment.formSpecified && payment.paymentFormDetail.Item == null)
                Ensure.IsNotNull(null, "Os detalhes da forma de pagamento não foram especificados");

            //Regra de negocio: checar se o conteudo do paymentFormDetail é condizente
            //com a forma de pagamento enviada no atributo 'form'
            if (payment.formSpecified &&
                 ((GenericHelper.IsBoleto((int)payment.form) && !payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation))) ||
                  (!GenericHelper.IsBoleto((int)payment.form) && !payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailCreditCardInformation)))))
                Ensure.IsNotNull(null, "Os detalhes da forma de pagamento estão inconsistentes com a forma enviada");

            //Regra de negocio: validar se o pedido enviado não é recorrente, pois
            //foi enviado como um pagamento normal (tag 'payment')
            DRecurrence dRecurrence = DataFactory.Recurrence().Locate(order.orderId);
            if (dRecurrence != null)
                Ensure.IsNotNull(null, "O pedido enviado pertence a uma recorrência, deve ser atualizado utilizando as tags 'payments' e 'payment'");

            //Regra de negocio: se o pedido não possui agendamento não pode
            //ser atualizado
            DSchedule[] schedules = DataFactory.Schedule().List(order.orderId, (int)ScheduleStatus.Scheduled);
            if (schedules == null)
                Ensure.IsNotNull(null, "Não existe agendamento para o pedido enviado, não será possível atualizar os dados");

            //Regra de negocio: O cliente nao podera alterar os dados de um
            //pedido que não seja recorrente caso tenha qualquer attempt
            //com status pago
            DPaymentAttempt[] paidAttempts = DataFactory.PaymentAttempt().List(order.orderId, (int)PaymentAttemptStatus.Paid);
            if (paidAttempts != null)
                Ensure.IsNotNull(null, "Não é possivel atualizar o valor de um pedido já pago ou parcialmente pago");

            //Regra de negocio: a data de agendamento deverá ser maior ou igual
            //a data atual
            if (payment.dateSpecified && payment.date < DateTime.Today)
                Ensure.IsNotNull(null, "A data de processamento do pagamento deve ser maior ou igual a data atual");
            
            //TODO: verificar se é necessário utilizar o atributo 'batch' da tag 'payment'

            Resp.responseOrdersOrderPayment responsePayment = HelperService.CreateResponsePayment(installments.Length, (payment.formSpecified ? (int)payment.form : installments[0].paymentFormId));
            
            installments = UpdateOrderInstallment(order, installments, payment);
            responsePayment = UpdateSchedule(order, installments, schedules, payment);

            return responsePayment;
        }
        public Resp.responseOrdersOrderPayment ProcessRecurrence(DOrder order, DOrderInstallment[] installments, updateOrderRecurrencesRecurrence recurrence)
        {
            //Regra de negocio: verificar se o atributo 'form' está presente, se sim
            //é necessário que a tag 'paymentFormDetail' também estaja presente e
            //seja condizente com a forma de pagamento enviada no atributo 'form'
            if (recurrence.formSpecified && recurrence.paymentFormDetail.Item == null)
                Ensure.IsNotNull(null, "Os detalhes da forma de pagamento não foram especificados");
            
            //Regra de negocio: checar se o conteudo do paymentFormDetail é condizente
            //com a forma de pagamento enviada no atributo 'form'
            if ( recurrence.formSpecified && 
                 ( (GenericHelper.IsBoleto((int)recurrence.form) && !recurrence.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation))) ||
                   (!GenericHelper.IsBoleto((int)recurrence.form) && !recurrence.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailCreditCardInformation))) ) )
                Ensure.IsNotNull(null, "Os detalhes da forma de pagamento estão inconsistentes com a forma enviada");

            //Regra de negocio: validar realmente se o pedido enviado é recorrente
            DRecurrence dRecurrence = DataFactory.Recurrence().Locate(order.orderId);
            if (dRecurrence == null)
                Ensure.IsNotNull(null, "O pedido enviado não pertence a uma recorrência, não pode ser atualizado utilizando as tags 'recurrences' e 'recurrence'");

            if (recurrence.startDateSpecified && recurrence.startDate < DateTime.Today)
                Ensure.IsNotNull(null, "A data de processamento da recorrência deve ser maior ou igual a data atual");
            
            Resp.responseOrdersOrderPayment responsePayment = HelperService.CreateResponsePayment(installments.Length, (recurrence.formSpecified ? (int)recurrence.form : installments[0].paymentFormId));

            UpdateOrderInstallment(order, installments, recurrence);
            UpdateRecurrence(dRecurrence, recurrence);
            
            DSchedule[] schedules = DataFactory.Schedule().List(order.orderId, (int)ScheduleStatus.Scheduled);
            if (schedules != null)
                responsePayment = UpdateSchedule(order, installments, dRecurrence, schedules, recurrence);
            
            return responsePayment;
        }

        public void UpdateOrderInstallment(DOrder order, DOrderInstallment[] installments, decimal newAmount)
        {
            int installmentNumber = order.installmentQuantity;
            decimal interestPercentage = installments[0].interestPercentage;

            //calculo valor da parcela e finalAmout da order
            decimal installmentValue = GenericHelper.GetInstallmentValue(installmentNumber, newAmount, interestPercentage);
            decimal finalAmount = installmentValue * installmentNumber;

            //atualizo a order
            order.lastUpdateDate = DateTime.Now;
            order.totalAmount = newAmount;
            order.finalAmount = finalAmount;
            DataFactory.Order().Update(order);

            //Atualiza OrderInstallment
            for (int i = 0; i < installments.Length; i++)
            {
                installments[i].installmentValue = installmentValue;
                DataFactory.OrderInstallment().Update(installments[i]);
            }
        }
        public void UpdateOrderInstallment(DOrder order, DOrderInstallment[] installments, updateOrderRecurrencesRecurrence recurrence)
        {
            decimal installmentValue = 0;

            //atualizar o valor do pedido se o atributo 'amount' for especificado
            if (recurrence.amountSpecified)
            {
                int installmentNumber = order.installmentQuantity;
                decimal interestPercentage = installments[0].interestPercentage;
                decimal newAmount = GenericHelper.ParseDecimal(recurrence.amount, 2);

                //calculo valor da parcela e finalAmout da order
                installmentValue = GenericHelper.GetInstallmentValue(installmentNumber, newAmount, interestPercentage);
                decimal finalAmount = installmentValue * installmentNumber;

                //atualizo a order
                order.lastUpdateDate = DateTime.Now;
                order.totalAmount = newAmount;
                order.finalAmount = finalAmount;
                DataFactory.Order().Update(order);
            }

            if (recurrence.amountSpecified || recurrence.formSpecified)
                //Atualiza OrderInstallment
                for (int i = 0; i < installments.Length; i++)
                {
                    installments[i].paymentFormId = (recurrence.formSpecified ? recurrence.form : installments[i].paymentFormId);
                    installments[i].installmentValue = (recurrence.amountSpecified ? installmentValue : installments[i].installmentValue);
                    DataFactory.OrderInstallment().Update(installments[i]);
                }
        }
        public DOrderInstallment[] UpdateOrderInstallment(DOrder order, DOrderInstallment[] installments, updateOrderPaymentsPayment payment)
        {
            int paymentFormId = (payment.formSpecified ? payment.form : installments[0].paymentFormId);
            decimal newAmount = (payment.amountSpecified ? GenericHelper.ParseDecimal(payment.amount, 2) : order.totalAmount);
            int installmentNumber = (payment.installments != null ? (int)payment.installments.quantity : order.installmentQuantity);
            decimal interestPercentage = (payment.installments != null && payment.installments.interestSpecified ? payment.installments.interest : installments[0].interestPercentage);

            //calculo valor da parcela e finalAmout da order
            decimal installmentValue = GenericHelper.GetInstallmentValue(installmentNumber, order.totalAmount, interestPercentage);
            decimal finalAmount = installmentValue * installmentNumber;

            //atualizo a order
            order.lastUpdateDate = DateTime.Now;
            order.totalAmount = newAmount;
            order.finalAmount = finalAmount;
            order.installmentQuantity = installmentNumber;
            DataFactory.Order().Update(order);

            //Cria OrderInstallment
            DataFactory.OrderInstallment().Delete(order.orderId);
            DOrderInstallment[] orderInstallments = new DOrderInstallment[installmentNumber];
            for (int i = 0; i < installmentNumber; i++)
            {
                DOrderInstallment orderInstallment = new DOrderInstallment();
                orderInstallment.orderId = order.orderId;
                orderInstallment.installmentNumber = i + 1;
                orderInstallment.paymentFormId = paymentFormId;
                orderInstallment.installmentValue = installmentValue;
                orderInstallment.interestPercentage = interestPercentage;
                orderInstallment.status = (int)OrderInstallmentStatus.Pending;
                DataFactory.OrderInstallment().Insert(orderInstallment);
                orderInstallments[i] = orderInstallment;
            }

            return orderInstallments;
        }
        public Resp.responseOrdersOrderPayment UpdateSchedule(DOrder order, DOrderInstallment[] installments, DSchedule[] schedules, updateOrderPaymentsPayment payment)
        {
            int paymentFormId = (payment.formSpecified ? payment.form : schedules[0].paymentFormId);
            DateTime date = (payment.dateSpecified ? payment.date : schedules[0].date);
            int installmentNumber = (payment.installments != null ? (int)payment.installments.quantity : order.installmentQuantity);
            generate gen = (payment.installments != null && payment.installments.generateSpecified ? payment.installments.generate : GetGenerateOption(schedules));
            InstallmentType installmentType = (payment.installments != null && payment.installments.typeSpecified ? (payment.installments.type == type.merchant ? InstallmentType.Merchant : InstallmentType.Emissor ) : (InstallmentType)schedules[0].installmentType);
            
            genericPaymentFormDetail pay;
            if (payment.paymentFormDetail != null)
            {
                pay = payment.paymentFormDetail;
            }
            else
            {
                string msgerror = "";
                if ((pay = (genericPaymentFormDetail)XmlHelper.GetClass(schedules[0].paymentFormDetail, typeof(genericPaymentFormDetail), out msgerror)) == null)
                    Ensure.IsNotNull(null, msgerror);
            }

            foreach (DSchedule sched in schedules)
                DataFactory.Schedule().Delete(sched.scheduleId);

            Resp.responseOrdersOrderPayment responsePayment = HelperService.CreateResponsePayment(installmentNumber, paymentFormId);
            
            if (gen == generate.parcial)
            {
                for (int i = 0; i < installmentNumber; i++)
                {
                    if (pay.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                        ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                    responsePayment.installments[i] = HelperService.ScheduleTransaction(order, installments[i], installmentType, date.AddMonths(i), pay, null);
                }
            }
            else if (GenericHelper.IsBoleto((int)payment.form))
            {
                for (int i = 0; i < installmentNumber; i++)
                {
                    ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                    responsePayment.installments[i] = HelperService.ScheduleTransaction(order, installments[i], installmentType, date, pay, null);
                }
            }
            else
            {
                responsePayment = HelperService.ScheduleTransaction(order, installments, installmentType, date, pay, null);
            }

            responsePayment.status = (byte)HelperService.GetPaymentStatus(responsePayment.installments);
            
            return responsePayment;
        }
        public Resp.responseOrdersOrderPayment UpdateSchedule(DOrder order, DOrderInstallment[] installments, DRecurrence dRecurrence, DSchedule[] schedules, updateOrderRecurrencesRecurrence recurrence)
        {
            int paymentFormId = (recurrence.formSpecified ? recurrence.form : schedules[0].paymentFormId);
            DateTime date = (recurrence.startDateSpecified ? recurrence.startDate : schedules[0].date);

            genericPaymentFormDetail pay;
            if (recurrence.paymentFormDetail != null)
            {
                pay = recurrence.paymentFormDetail;
            }
            else
            {
                string msgerror = "";
                if ((pay = (genericPaymentFormDetail)XmlHelper.GetClass(schedules[0].paymentFormDetail, typeof(genericPaymentFormDetail), out msgerror)) == null)
                    Ensure.IsNotNull(null, msgerror);
            }

            foreach (DSchedule sched in schedules)
                DataFactory.Schedule().Delete(sched.scheduleId);

            Resp.responseOrdersOrderPayment responsePayment = HelperService.CreateResponsePayment(1, paymentFormId);

            responsePayment.installments[0] = HelperService.ScheduleTransaction(order, installments[0], InstallmentType.Merchant, date, pay, dRecurrence);
            responsePayment.status = (byte)HelperService.GetPaymentStatus(responsePayment.installments);

            return responsePayment;
        }
        public void UpdateRecurrence(DRecurrence dRecurrence, updateOrderRecurrencesRecurrence recurrence)
        {
            if (recurrence.formSpecified || recurrence.intervalSpecified || recurrence.startDateSpecified)
            {
                if (recurrence.formSpecified)
                {
                    dRecurrence.paymentFormId = recurrence.form;
                    dRecurrence.paymentFormDetail = SuperPag.Helper.Xml.XmlHelper.GetXml(typeof(genericPaymentFormDetail), recurrence.paymentFormDetail);
                }
                dRecurrence.interval = recurrence.intervalSpecified ? (int)recurrence.interval : dRecurrence.interval;
                dRecurrence.startDate = recurrence.startDateSpecified ? recurrence.startDate : dRecurrence.startDate;
                DataFactory.Recurrence().Update(dRecurrence);
            }
        }

        public generate GetGenerateOption(DSchedule[] schedules)
        {
            if (schedules.Length <= 1)
                return generate.integral;
            if (schedules[0].date != schedules[1].date)
                return generate.parcial;
            else
                return generate.integral;
        }
        public bool OrderTotalEqualsItensTotal(ulong totalAmount, updateOrderDetail orderDetail)
        {
            //TODO: Checar se o valor total é igual a soma dos itens
            return true;
        }
    }
}
