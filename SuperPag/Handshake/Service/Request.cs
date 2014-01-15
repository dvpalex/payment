using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper.Xml.Request;
using Resp = SuperPag.Helper.Xml.Response;
using SuperPag.Helper;
using System.Reflection;
using SuperPag.Helper.Xml;

namespace SuperPag.Handshake.Service
{
    public class Request
    {
        public Resp.response ProcessRequest(DStore store, request request)
        {
            Resp.response response = new Resp.response();
            response.orders = new Resp.responseOrders();

            if (request.orders.Length > 0)
                response.orders.order = new Resp.responseOrdersOrder[request.orders.Length];

            int count = 0;
            foreach (requestOrder order in request.orders)
            {
                DOrder dOrder = SaveOrder(store, order);
                Resp.responseOrdersOrderPayment responsePayment;

                if (order.Item.GetType().Equals(typeof(requestOrderPayments)))
                {
                    DOrderInstallment[] installments = SaveOrderInstallmentPayment(dOrder, ((requestOrderPayments)order.Item).payment);
                    responsePayment = ProcessPayment(dOrder, installments, ((requestOrderPayments)order.Item).payment);
                }
                else
                {
                    DOrderInstallment[] installments = SaveOrderInstallmentRecurrence(dOrder, ((requestOrderRecurrences)order.Item).recurrence);
                    DRecurrence dRecurrence = SaveRecurrence(dOrder, ((requestOrderRecurrences)order.Item).recurrence);
                    responsePayment = ProcessRecurrence(dOrder, installments, dRecurrence, ((requestOrderRecurrences)order.Item).recurrence);
                }

                Resp.responseOrdersOrder responseOrder = new Resp.responseOrdersOrder();
                responseOrder.id = (ulong)dOrder.orderId;
                responseOrder.reference = order.reference;
                responseOrder.total = GenericHelper.ParseLong(dOrder.finalAmount);
                responseOrder.status = responsePayment.status;
                responseOrder.payments = new Resp.responseOrdersOrderPayment[1];
                responseOrder.payments[0] = responsePayment;
                response.orders.order[count] = responseOrder;

                count++;
            }

            return response;
        }
        public Resp.response ProcessRequest(DStore store, request request, Guid Userid)
        {
            Resp.response response = new Resp.response();
            response.orders = new Resp.responseOrders();

            if (request.orders.Length > 0)
                response.orders.order = new Resp.responseOrdersOrder[request.orders.Length];

            int count = 0;
            foreach (requestOrder order in request.orders)
            {
                DOrder dOrder = SaveOrder(store, order);
                Resp.responseOrdersOrderPayment responsePayment;

                if (order.Item.GetType().Equals(typeof(requestOrderPayments)))
                {
                    DOrderInstallment[] installments = SaveOrderInstallmentPayment(dOrder, ((requestOrderPayments)order.Item).payment);
                    responsePayment = ProcessPayment(dOrder, installments, ((requestOrderPayments)order.Item).payment, Userid);
                }
                else
                {
                    DOrderInstallment[] installments = SaveOrderInstallmentRecurrence(dOrder, ((requestOrderRecurrences)order.Item).recurrence);
                    DRecurrence dRecurrence = SaveRecurrence(dOrder, ((requestOrderRecurrences)order.Item).recurrence);
                    responsePayment = ProcessRecurrence(dOrder, installments, dRecurrence, ((requestOrderRecurrences)order.Item).recurrence);
                }

                Resp.responseOrdersOrder responseOrder = new Resp.responseOrdersOrder();
                responseOrder.id = (ulong)dOrder.orderId;
                responseOrder.reference = order.reference;
                responseOrder.total = GenericHelper.ParseLong(dOrder.finalAmount);
                responseOrder.status = responsePayment.status;
                responseOrder.payments = new Resp.responseOrdersOrderPayment[1];
                responseOrder.payments[0] = responsePayment;
                response.orders.order[count] = responseOrder;

                count++;
            }

            return response;
        }
        public Resp.responseOrdersOrderPayment ProcessPayment(DOrder order, DOrderInstallment[] installments, requestOrderPaymentsPayment payment)
        {
            Resp.responseOrdersOrderPayment responsePayment = HelperService.CreateResponsePayment(installments.Length, (int)payment.form);

            //Regra de negocio: se a data contida no atributo date da tag payment
            //for inferior a data atual, então o atributo status da tag payment
            //deve estar definido. Isso significa que o cliente esta enviando
            //um pedido já concluido (pago ou cancelado) com data retroativa
            //Se o atributo status da tag payment não estiver definido a data de
            //agendamento deverá ser maior ou igual a data atual
            if (payment.dateSpecified && payment.date < DateTime.Today && !payment.statusSpecified)
                Ensure.IsNotNull(null, "A data de processamento do pagamento deve ser maior ou igual a data atual");

            //Regra de negocio: checar se o conteudo do paymentFormDetail é condizente
            //com a forma de pagamento enviada no atributo 'form'
            //TUDO:modificação para a ponto cred
            if (
                 (GenericHelper.IsBoleto((int)payment.form) && (!payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)) && !payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformationIPTE)))) ||
                 (!GenericHelper.IsBoleto((int)payment.form) && !payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailCreditCardInformation)) && !payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentContaCorrenteInformation)) && !payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentContaCorrenteInformationCSU)))
                )
                Ensure.IsNotNull(null, "Os detalhes da forma de pagamento estão inconsistentes com a forma enviada");

            if (payment.statusSpecified)
            {
                OrderInstallmentStatus inst = OrderInstallmentStatus.Paid;
                switch (payment.status)
                {
                    case status.paid:
                        order.status = (int)OrderStatus.Analysing;
                        responsePayment.status = (int)PaymentAttemptStatus.Paid;
                        inst = OrderInstallmentStatus.Paid;
                        break;
                    case status.cancelled:
                        order.status = (int)OrderStatus.Cancelled;
                        responsePayment.status = (int)PaymentAttemptStatus.Canceled;
                        inst = OrderInstallmentStatus.Canceled;
                        break;
                }
                order.lastUpdateDate = DateTime.Now;
                DataFactory.Order().Update(order);

                for (int i = 1; i < installments.Length; i++)
                {
                    responsePayment.installments[i].status = (byte)inst;
                    installments[i].status = (int)inst;
                    DataFactory.OrderInstallment().Update(installments[i]);
                }

                return responsePayment;
            }

            DPaymentAttempt attempt = null;

            generate generate = generate.integral;
            type iType = type.merchant;
            if (payment.installments != null)
            {
                //Regra de negocio: se a quantidade de parcelas for igual a 1 assumiremos
                //que o atributo 'generate' será integral
                if (payment.installments.quantity == 1)
                    generate = generate.integral;
                else
                    generate = payment.installments.generateSpecified ? payment.installments.generate : generate;
                iType = payment.installments.typeSpecified ? payment.installments.type : iType;
            }
            InstallmentType installmentType = iType == type.merchant ? InstallmentType.Merchant : InstallmentType.Emissor;

            if (generate == generate.parcial)
            {
                if ((payment.dateSpecified && payment.date > DateTime.Today) || (payment.batchSpecified && payment.batch == batch.@true))
                {
                    for (int i = 0; i < installments.Length; i++)
                    {
                        genericPaymentFormDetail pay = payment.paymentFormDetail;
                        if (pay.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                            ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                        responsePayment.installments[i] = HelperService.ScheduleTransaction(order, installments[i], installmentType, payment.date.AddMonths(i), pay, null);
                    }
                }
                else
                {
                    responsePayment.installments[0] = HelperService.StartTransaction(order, installments[0], installmentType, payment.paymentFormDetail, out attempt, payment);

                    //Regra de negocio: Se a primeira transação for negada ou não concluída, não agendamos as próximas.
                    if (responsePayment.installments[0].status == (byte)OrderInstallmentStatus.NotPaid || responsePayment.installments[0].status == (byte)OrderInstallmentStatus.Pending)
                    {
                        for (int i = 1; i < installments.Length; i++)
                            responsePayment.installments[i].status = (byte)installments[0].status;
                    }
                    else
                    {
                        for (int i = 1; i < installments.Length; i++)
                        {
                            genericPaymentFormDetail pay = payment.paymentFormDetail;
                            if (pay.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                                ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                            responsePayment.installments[i] = HelperService.ScheduleTransaction(order, installments[i], installmentType, DateTime.Today.AddMonths(i), pay, null);
                        }
                    }
                }
            }
            else if (GenericHelper.IsBoleto((int)payment.form))
            {
                if ((payment.dateSpecified && payment.date > DateTime.Today) || (payment.batchSpecified && payment.batch == batch.@true))
                {
                    for (int i = 0; i < installments.Length; i++)
                    {
                        genericPaymentFormDetail pay = payment.paymentFormDetail;
                        if (!payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformationIPTE)))
                        {
                            ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                        }
                        responsePayment.installments[i] = HelperService.ScheduleTransaction(order, installments[i], installmentType, payment.date, pay, null);
                    }
                }
                else
                {
                    for (int i = 0; i < installments.Length; i++)
                    {
                        genericPaymentFormDetail pay = payment.paymentFormDetail;
                        if (!payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformationIPTE)))
                        {
                            ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                        }
                        responsePayment.installments[i] = HelperService.StartTransaction(order, installments[i], installmentType, pay, out attempt, payment);
                    }
                }
            }
            else
            {
                if ((payment.dateSpecified && payment.date > DateTime.Today) || (payment.batchSpecified && payment.batch == batch.@true))
                    responsePayment = HelperService.ScheduleTransaction(order, installments, installmentType, payment.date, payment.paymentFormDetail, null);
                else
                    responsePayment = HelperService.StartTransaction(order, installments, installmentType, payment.paymentFormDetail, out attempt, payment);
            }

            responsePayment.status = (byte)HelperService.GetPaymentStatus(responsePayment.installments);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, (int)responsePayment.status);

            return responsePayment;
        }
        public Resp.responseOrdersOrderPayment ProcessPayment(DOrder order, DOrderInstallment[] installments, requestOrderPaymentsPayment payment, Guid Userid)
        {
            Resp.responseOrdersOrderPayment responsePayment = HelperService.CreateResponsePayment(installments.Length, (int)payment.form);

            //Regra de negocio: se a data contida no atributo date da tag payment
            //for inferior a data atual, então o atributo status da tag payment
            //deve estar definido. Isso significa que o cliente esta enviando
            //um pedido já concluido (pago ou cancelado) com data retroativa
            //Se o atributo status da tag payment não estiver definido a data de
            //agendamento deverá ser maior ou igual a data atual
            if (payment.dateSpecified && payment.date < DateTime.Today && !payment.statusSpecified)
                Ensure.IsNotNull(null, "A data de processamento do pagamento deve ser maior ou igual a data atual");

            //Regra de negocio: checar se o conteudo do paymentFormDetail é condizente
            //com a forma de pagamento enviada no atributo 'form'
            //TUDO:modificação para a ponto cred
            if (
                 (GenericHelper.IsBoleto((int)payment.form) && (!payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)) && !payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformationIPTE)))) ||
                 (!GenericHelper.IsBoleto((int)payment.form) && !payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailCreditCardInformation)))
                )
                Ensure.IsNotNull(null, "Os detalhes da forma de pagamento estão inconsistentes com a forma enviada");

            if (payment.statusSpecified)
            {
                OrderInstallmentStatus inst = OrderInstallmentStatus.Paid;
                switch (payment.status)
                {
                    case status.paid:
                        order.status = (int)OrderStatus.Analysing;
                        responsePayment.status = (int)PaymentAttemptStatus.Paid;
                        inst = OrderInstallmentStatus.Paid;
                        break;
                    case status.cancelled:
                        order.status = (int)OrderStatus.Cancelled;
                        responsePayment.status = (int)PaymentAttemptStatus.Canceled;
                        inst = OrderInstallmentStatus.Canceled;
                        break;
                }
                order.lastUpdateDate = DateTime.Now;
                DataFactory.Order().Update(order);

                for (int i = 1; i < installments.Length; i++)
                {
                    responsePayment.installments[i].status = (byte)inst;
                    installments[i].status = (int)inst;
                    DataFactory.OrderInstallment().Update(installments[i]);
                }

                return responsePayment;
            }

            DPaymentAttempt attempt = null;

            generate generate = generate.integral;
            type iType = type.merchant;
            if (payment.installments != null)
            {
                //Regra de negocio: se a quantidade de parcelas for igual a 1 assumiremos
                //que o atributo 'generate' será integral
                if (payment.installments.quantity == 1)
                    generate = generate.integral;
                else
                    generate = payment.installments.generateSpecified ? payment.installments.generate : generate;
                iType = payment.installments.typeSpecified ? payment.installments.type : iType;
            }
            InstallmentType installmentType = iType == type.merchant ? InstallmentType.Merchant : InstallmentType.Emissor;

            if (generate == generate.parcial)
            {
                if ((payment.dateSpecified && payment.date > DateTime.Today) || (payment.batchSpecified && payment.batch == batch.@true))
                {
                    for (int i = 0; i < installments.Length; i++)
                    {
                        genericPaymentFormDetail pay = payment.paymentFormDetail;
                        if (pay.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                            ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                        responsePayment.installments[i] = HelperService.ScheduleTransaction(order, installments[i], installmentType, payment.date.AddMonths(i), pay, null);
                    }
                }
                else
                {
                    responsePayment.installments[0] = HelperService.StartTransaction(order, installments[0], installmentType, payment.paymentFormDetail, out attempt, payment);

                    //Regra de negocio: Se a primeira transação for negada ou não concluída, não agendamos as próximas.
                    if (responsePayment.installments[0].status == (byte)OrderInstallmentStatus.NotPaid || responsePayment.installments[0].status == (byte)OrderInstallmentStatus.Pending)
                    {
                        for (int i = 1; i < installments.Length; i++)
                            responsePayment.installments[i].status = (byte)installments[0].status;
                    }
                    else
                    {
                        for (int i = 1; i < installments.Length; i++)
                        {
                            genericPaymentFormDetail pay = payment.paymentFormDetail;
                            if (pay.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                                ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                            responsePayment.installments[i] = HelperService.ScheduleTransaction(order, installments[i], installmentType, DateTime.Today.AddMonths(i), pay, null);
                        }
                    }
                }
            }
            else if (GenericHelper.IsBoleto((int)payment.form))
            {
                if ((payment.dateSpecified && payment.date > DateTime.Today) || (payment.batchSpecified && payment.batch == batch.@true))
                {
                    for (int i = 0; i < installments.Length; i++)
                    {
                        genericPaymentFormDetail pay = payment.paymentFormDetail;
                        if (!payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformationIPTE)))
                        {
                            ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                        }
                        responsePayment.installments[i] = HelperService.ScheduleTransaction(order, installments[i], installmentType, payment.date, pay, null);
                    }
                }
                else
                {
                    for (int i = 0; i < installments.Length; i++)
                    {
                        genericPaymentFormDetail pay = payment.paymentFormDetail;
                        if (!payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformationIPTE)))
                        {
                            ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(i);
                        }
                        responsePayment.installments[i] = HelperService.StartTransaction(order, installments[i], installmentType, pay, out attempt, payment, Userid);
                    }
                }
            }
            else
            {
                if ((payment.dateSpecified && payment.date > DateTime.Today) || (payment.batchSpecified && payment.batch == batch.@true))
                    responsePayment = HelperService.ScheduleTransaction(order, installments, installmentType, payment.date, payment.paymentFormDetail, null);
                else
                    responsePayment = HelperService.StartTransaction(order, installments, installmentType, payment.paymentFormDetail, out attempt, payment);
            }

            responsePayment.status = (byte)HelperService.GetPaymentStatus(responsePayment.installments);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, (int)responsePayment.status);

            return responsePayment;
        }

        public Resp.responseOrdersOrderPayment ProcessRecurrence(DOrder order, DOrderInstallment[] installments, DRecurrence dRecurrence, requestOrderRecurrencesRecurrence recurrence)
        {
            Resp.responseOrdersOrderPayment responsePayment = HelperService.CreateResponsePayment(1, (int)recurrence.form);

            if (recurrence.startDateSpecified && recurrence.startDate < DateTime.Today)
                Ensure.IsNotNull(null, "A data de processamento da recorrência deve ser maior ou igual a data atual");

            //Regra de negocio: checar se o conteudo do paymentFormDetail é condizente
            //com a forma de pagamento enviada no atributo 'form'
            if ((GenericHelper.IsBoleto((int)recurrence.form) && !recurrence.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation))) ||
                 (!GenericHelper.IsBoleto((int)recurrence.form) && !recurrence.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailCreditCardInformation)) && !recurrence.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentContaCorrenteInformation)))
               )
                Ensure.IsNotNull(null, "Os detalhes da forma de pagamento estão inconsistentes com a forma enviada");

            int monthInterval = recurrence.intervalSpecified ? recurrence.interval / 30 : 1;

            responsePayment.installments = new Resp.responseOrdersOrderPaymentInstallment[1];

            DPaymentAttempt attempt = null;

            if (recurrence.startDateSpecified && recurrence.startDate > DateTime.Today)
                responsePayment.installments[0] = HelperService.ScheduleTransaction(order, installments[0], InstallmentType.Merchant, recurrence.startDate, recurrence.paymentFormDetail, dRecurrence);
            else
            {
                responsePayment.installments[0] = HelperService.StartTransaction(order, installments[0], InstallmentType.Merchant, recurrence.paymentFormDetail, out attempt, null);

                //Sempre agendar a próxima transação
                genericPaymentFormDetail rec = recurrence.paymentFormDetail;
                if (rec.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                    ((genericPaymentFormDetailBoletoInformation)rec.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)rec.Item).dueDate.AddMonths(monthInterval);
                HelperService.ScheduleTransaction(order, installments[0], InstallmentType.Merchant, dRecurrence.startDate.AddMonths(monthInterval), rec, dRecurrence);
            }

            if ((int)responsePayment.installments[0].status == (int)OrderInstallmentStatus.NotPaid)
            {
                responsePayment.status = (byte)OrderInstallmentStatus.NotPaid;
            }
            else if (recurrence.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentContaCorrenteInformation)))
            {
                responsePayment.status = (byte)responsePayment.installments[0].status;
            }

            GenericHelper.UpdateOrderStatusByAttemptStatus(order, (int)responsePayment.status);

            return responsePayment;
        }

        public DRecurrence SaveRecurrence(DOrder order, requestOrderRecurrencesRecurrence recurrence)
        {
            //Salvo as informações da recorrência
            DRecurrence dRecurrence = new DRecurrence();
            dRecurrence.orderId = order.orderId;
            dRecurrence.interval = recurrence.intervalSpecified ? (int)recurrence.interval : 30;
            dRecurrence.paymentFormId = recurrence.form;
            dRecurrence.paymentFormDetail = SuperPag.Helper.Xml.XmlHelper.GetXml(typeof(genericPaymentFormDetail), recurrence.paymentFormDetail);
            dRecurrence.startDate = recurrence.startDateSpecified ? recurrence.startDate : DateTime.Now.Date;

            dRecurrence.status = (int)RecurrenceStatus.Active;
            DataFactory.Recurrence().Insert(dRecurrence);

            return dRecurrence;
        }
        public DOrderInstallment[] SaveOrderInstallmentPayment(DOrder order, requestOrderPaymentsPayment payment)
        {
            //Regra de negocio: o atributo total da tag order (campo totalAmount
            //da tabela order) deve ser igual ao atributo amount da tag payment
            if (order.totalAmount != GenericHelper.ParseDecimal(payment.amount, 2))
                Ensure.IsNotNull(null, "Os atributos 'total' e 'amount' das tags 'order' e 'payment' estão inconsistentes");

            int paymentFormId = payment.form;
            int installmentNumber = (payment.installments != null ? (int)payment.installments.quantity : 1);
            decimal interestPercentage = (payment.installments != null && payment.installments.interestSpecified ? payment.installments.interest : 0);

            //calculo valor da parcela e finalAmout da order
            decimal installmentValue = GenericHelper.GetInstallmentValue(installmentNumber, order.totalAmount, interestPercentage);
            decimal finalAmount = installmentValue * installmentNumber;

            //atualizo a order
            order.lastUpdateDate = DateTime.Now;
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
        public DOrderInstallment[] SaveOrderInstallmentPayment(DOrder order, requestOrderPaymentsPayment payment, Guid Userid)
        {
            //Regra de negocio: o atributo total da tag order (campo totalAmount
            //da tabela order) deve ser igual ao atributo amount da tag payment
            if (order.totalAmount != GenericHelper.ParseDecimal(payment.amount, 2))
                Ensure.IsNotNull(null, "Os atributos 'total' e 'amount' das tags 'order' e 'payment' estão inconsistentes");

            int paymentFormId = payment.form;
            int installmentNumber = (payment.installments != null ? (int)payment.installments.quantity : 1);
            decimal interestPercentage = (payment.installments != null && payment.installments.interestSpecified ? payment.installments.interest : 0);

            //calculo valor da parcela e finalAmout da order
            decimal installmentValue = GenericHelper.GetInstallmentValue(installmentNumber, order.totalAmount, interestPercentage);
            decimal finalAmount = installmentValue * installmentNumber;

            //atualizo a order
            order.lastUpdateDate = DateTime.Now;
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
        public DOrderInstallment[] SaveOrderInstallmentRecurrence(DOrder order, requestOrderRecurrencesRecurrence recurrence)
        {
            //Regra de negocio: o atributo total da tag order (campo totalAmount
            //da tabela order) deve ser igual ao atributo amount da tag recurrence
            if (order.totalAmount != GenericHelper.ParseDecimal(recurrence.amount, 2))
                Ensure.IsNotNull(null, "Os atributos 'total' e 'amount' das tags 'order' e 'recurrence' estão inconsistentes.");

            //por padrao setamos a quantidade de parcela em recorrencia para 1
            order.lastUpdateDate = DateTime.Now;
            order.finalAmount = order.totalAmount;
            order.installmentQuantity = 1;
            DataFactory.Order().Update(order);

            //Cria OrderInstallment
            DataFactory.OrderInstallment().Delete(order.orderId);
            DOrderInstallment orderInstallment = new DOrderInstallment();
            orderInstallment.orderId = order.orderId;
            orderInstallment.installmentNumber = 1;
            orderInstallment.paymentFormId = recurrence.form;
            orderInstallment.installmentValue = order.totalAmount;
            orderInstallment.interestPercentage = 0;
            orderInstallment.status = (int)OrderInstallmentStatus.Pending;
            DataFactory.OrderInstallment().Insert(orderInstallment);

            DOrderInstallment[] orderInstallments = new DOrderInstallment[1];
            orderInstallments[0] = orderInstallment;

            return orderInstallments;
        }
        public DOrder SaveOrder(DStore store, requestOrder reqorder)
        {
            //crio sessao de handshake
            Guid handshakeSessionId = Guid.NewGuid();
            DHandshakeSession dHandshakeSession = new DHandshakeSession();
            dHandshakeSession.handshakeSessionId = handshakeSessionId;
            dHandshakeSession.storeId = store.storeId;
            dHandshakeSession.orderId = long.MinValue;
            dHandshakeSession.handshakeType = (int)HandshakeType.XmlService;
            dHandshakeSession.createDate = DateTime.Now;
            DataFactory.HandshakeSession().Insert(dHandshakeSession);

            //salva os dados recebidos no log de handshake
            DHandshakeSessionLog dHandshakeLog = new DHandshakeSessionLog();
            dHandshakeLog.createDate = DateTime.Now;
            dHandshakeLog.handshakeSessionId = handshakeSessionId;
            dHandshakeLog.step = 1;
            dHandshakeLog.url = " ";
            dHandshakeLog.xmlData = XmlHelper.GetXml(typeof(requestOrder), reqorder);
            DataFactory.HandshakeSessionLog().Insert(dHandshakeLog);

            //cria a order
            long orderId = CreateOrder(store.storeId, reqorder);

            dHandshakeSession.orderId = orderId;
            DataFactory.HandshakeSession().Update(dHandshakeSession);

            CreateOrderItens(orderId, reqorder);

            long consumerId = CreateConsumer(orderId, reqorder);

            CreateAddress(consumerId, reqorder);

            DOrder order = DataFactory.Order().Locate(orderId);
            Ensure.IsNotNull(order, "Pedido {0} não encontrado", orderId);

            return order;
        }

        public DOrder SaveOrder(DStore store, requestOrder reqorder, Guid Userid)
        {
            //crio sessao de handshake
            Guid handshakeSessionId = Guid.NewGuid();
            DHandshakeSession dHandshakeSession = new DHandshakeSession();
            dHandshakeSession.handshakeSessionId = handshakeSessionId;
            dHandshakeSession.storeId = store.storeId;
            dHandshakeSession.orderId = long.MinValue;
            dHandshakeSession.handshakeType = (int)HandshakeType.XmlService;
            dHandshakeSession.createDate = DateTime.Now;
            DataFactory.HandshakeSession().Insert(dHandshakeSession);

            //salva os dados recebidos no log de handshake
            DHandshakeSessionLog dHandshakeLog = new DHandshakeSessionLog();
            dHandshakeLog.createDate = DateTime.Now;
            dHandshakeLog.handshakeSessionId = handshakeSessionId;
            dHandshakeLog.step = 1;
            dHandshakeLog.url = " ";
            dHandshakeLog.xmlData = XmlHelper.GetXml(typeof(requestOrder), reqorder);
            DataFactory.HandshakeSessionLog().Insert(dHandshakeLog);

            //cria a order
            long orderId = CreateOrder(store.storeId, reqorder);

            dHandshakeSession.orderId = orderId;
            DataFactory.HandshakeSession().Update(dHandshakeSession);

            CreateOrderItens(orderId, reqorder);

            long consumerId = CreateConsumer(orderId, reqorder);

            CreateAddress(consumerId, reqorder);

            DOrder order = DataFactory.Order().Locate(orderId);
            Ensure.IsNotNull(order, "Pedido {0} não encontrado", orderId);

            return order;
        }
        public long CreateOrder(int storeId, requestOrder reqorder)
        {
            DOrder dOrder = new DOrder();
            dOrder.orderId = long.MinValue;
            dOrder.storeId = storeId;
            dOrder.consumerId = long.MinValue;
            dOrder.storeReferenceOrder = reqorder.reference;
            dOrder.totalAmount = GenericHelper.ParseDecimal(reqorder.total, 2);
            dOrder.finalAmount = decimal.MinValue;
            dOrder.installmentQuantity = int.MinValue;
            dOrder.creationDate = DateTime.Now;
            dOrder.lastUpdateDate = DateTime.Now;
            dOrder.status = (int)OrderStatus.Unfinished;
            DataFactory.Order().Insert(dOrder);

            return dOrder.orderId;
        }
        public void CreateOrderItens(long orderId, requestOrder reqorder)
        {
            if (reqorder.detail == null)
                return;

            int count = 0;

            if (reqorder.detail.itens != null)
                foreach (requestOrderDetailItem item in reqorder.detail.itens)
                {
                    DOrderItem orderItem = new DOrderItem();

                    orderItem.itemCode = item.code;
                    orderItem.itemDescription = item.description;
                    orderItem.itemQuantity = (int)item.quantity;
                    orderItem.itemValue = GenericHelper.ParseDecimal(item.value, 2);
                    orderItem.itemNumber = count++;
                    orderItem.orderItemId = long.MinValue;
                    orderItem.orderId = orderId;
                    orderItem.itemType = (int)ItemTypes.Regular;

                    DataFactory.OrderItem().Insert(orderItem);
                }

            if (reqorder.detail.rates != null)
                foreach (requestOrderDetailRate rate in reqorder.detail.rates)
                {
                    DOrderItem orderItem = new DOrderItem();

                    orderItem.itemCode = rate.type.ToString();
                    orderItem.itemDescription = rate.description;
                    orderItem.itemQuantity = 1;
                    orderItem.itemValue = GenericHelper.ParseDecimal(rate.amount, 2);
                    orderItem.itemNumber = count++;
                    orderItem.orderItemId = long.MinValue;
                    orderItem.orderId = orderId;
                    switch (rate.type)
                    {
                        case SuperPag.Helper.Xml.Request.rate.shipping:
                            orderItem.itemType = (int)ItemTypes.ShippingRate;
                            break;
                        case SuperPag.Helper.Xml.Request.rate.discount:
                            orderItem.itemType = (int)ItemTypes.Discount;
                            break;
                        case SuperPag.Helper.Xml.Request.rate.extra:
                            orderItem.itemType = (int)ItemTypes.Extra;
                            break;
                    }

                    DataFactory.OrderItem().Insert(orderItem);
                }
        }
        public long CreateConsumer(long orderId, requestOrder reqorder)
        {
            if (reqorder.consumer == null || reqorder.consumer.Item == null)
                return long.MinValue;

            DConsumer dConsumer = new DConsumer();
            if (reqorder.consumer.Item.GetType().Equals(typeof(requestOrderConsumerPerson)))
            {
                //Pessoa Fisica
                requestOrderConsumerPerson fisica = (requestOrderConsumerPerson)reqorder.consumer.Item;
                dConsumer.name = fisica.name;
                dConsumer.CPF = fisica.cpf.ToString();
                dConsumer.birthDate = fisica.birthDate;
                dConsumer.ger = fisica.gender.ToString();
                dConsumer.civilState = fisica.civilState;
                dConsumer.occupation = fisica.occupation;
                dConsumer.email = (fisica.emails != null ? fisica.emails[0].address : "");
                if (fisica.phones != null)
                    foreach (requestOrderConsumerPersonPhone phone in fisica.phones)
                        switch (phone.type)
                        {
                            case SuperPag.Helper.Xml.Request.phoneType.home:
                                dConsumer.phone = (phone.countryCodeSpecified ? phone.countryCode.ToString() : "") + (phone.dddSpecified ? phone.ddd.ToString() : "") + phone.number.ToString();
                                break;
                            case SuperPag.Helper.Xml.Request.phoneType.cell:
                                dConsumer.celularPhone = (phone.countryCodeSpecified ? phone.countryCode.ToString() : "") + (phone.dddSpecified ? phone.ddd.ToString() : "") + phone.number.ToString();
                                break;
                            case SuperPag.Helper.Xml.Request.phoneType.business:
                                dConsumer.commercialPhone = (phone.countryCodeSpecified ? phone.countryCode.ToString() : "") + (phone.dddSpecified ? phone.ddd.ToString() : "") + phone.number.ToString();
                                break;
                            case SuperPag.Helper.Xml.Request.phoneType.fax:
                                dConsumer.fax = (phone.countryCodeSpecified ? phone.countryCode.ToString() : "") + (phone.dddSpecified ? phone.ddd.ToString() : "") + phone.number.ToString();
                                break;
                        }
            }
            else if (reqorder.consumer.Item.GetType().Equals(typeof(requestOrderConsumerCorporate)))
            {
                //Pessoa Juridica
                requestOrderConsumerCorporate juridica = (requestOrderConsumerCorporate)reqorder.consumer.Item;
                dConsumer.name = juridica.name;
                dConsumer.CNPJ = juridica.cnpj.ToString();
                dConsumer.IE = juridica.ie;
                dConsumer.responsibleCPF = "";
                dConsumer.responsibleName = "";
                dConsumer.email = (juridica.emails != null ? juridica.emails[0].address : "");
                if (juridica.phones != null)
                    foreach (requestOrderConsumerCorporatePhone phone in juridica.phones)
                        switch (phone.type)
                        {
                            case SuperPag.Helper.Xml.Request.phoneType.home:
                                dConsumer.phone = (phone.countryCodeSpecified ? phone.countryCode.ToString() : "") + (phone.dddSpecified ? phone.ddd.ToString() : "") + phone.number.ToString();
                                break;
                            case SuperPag.Helper.Xml.Request.phoneType.cell:
                                dConsumer.celularPhone = (phone.countryCodeSpecified ? phone.countryCode.ToString() : "") + (phone.dddSpecified ? phone.ddd.ToString() : "") + phone.number.ToString();
                                break;
                            case SuperPag.Helper.Xml.Request.phoneType.business:
                                dConsumer.commercialPhone = (phone.countryCodeSpecified ? phone.countryCode.ToString() : "") + (phone.dddSpecified ? phone.ddd.ToString() : "") + phone.number.ToString();
                                break;
                            case SuperPag.Helper.Xml.Request.phoneType.fax:
                                dConsumer.fax = (phone.countryCodeSpecified ? phone.countryCode.ToString() : "") + (phone.dddSpecified ? phone.ddd.ToString() : "") + phone.number.ToString();
                                break;
                        }
            }

            //insere o consumidor
            DataFactory.Consumer().Insert(dConsumer);

            //vincula consumidor com a order
            DataFactory.Order().Update(dConsumer.consumerId, orderId);

            return dConsumer.consumerId;
        }
        public void CreateAddress(long consumerId, requestOrder reqorder)
        {
            if (reqorder.consumer != null && reqorder.consumer.Item != null)
                if (reqorder.consumer.Item.GetType().Equals(typeof(requestOrderConsumerPerson)))
                {
                    requestOrderConsumerPerson fisica = (requestOrderConsumerPerson)reqorder.consumer.Item;

                    if (fisica.billingAddress != null)
                    {
                        //cria o endereco de cobranca
                        DConsumerAddress dConsumerAddress = new DConsumerAddress();
                        dConsumerAddress.logradouro = "";
                        dConsumerAddress.address = fisica.billingAddress.location;
                        dConsumerAddress.addressNumber = fisica.billingAddress.number.ToString();
                        dConsumerAddress.addressType = (int)AddressTypes.Billing;
                        dConsumerAddress.addressComplement = fisica.billingAddress.complement;
                        dConsumerAddress.district = fisica.billingAddress.district;
                        dConsumerAddress.city = fisica.billingAddress.city;
                        dConsumerAddress.cep = fisica.billingAddress.postalCode.ToString();
                        dConsumerAddress.state = fisica.billingAddress.state;
                        dConsumerAddress.country = fisica.billingAddress.country;
                        dConsumerAddress.consumerAddressId = long.MinValue;
                        dConsumerAddress.consumerId = consumerId;
                        //insere o endereco de cobranca
                        DataFactory.ConsumerAddress().Insert(dConsumerAddress);
                    }
                }
                else if (reqorder.consumer.Item.GetType().Equals(typeof(requestOrderConsumerCorporate)))
                {
                    requestOrderConsumerCorporate juridica = (requestOrderConsumerCorporate)reqorder.consumer.Item;

                    if (juridica.billingAddress != null)
                    {
                        //cria o endereco de cobranca
                        DConsumerAddress dConsumerAddress = new DConsumerAddress();
                        dConsumerAddress.logradouro = "";
                        dConsumerAddress.address = juridica.billingAddress.location;
                        dConsumerAddress.addressNumber = juridica.billingAddress.number.ToString();
                        dConsumerAddress.addressType = (int)AddressTypes.Billing;
                        dConsumerAddress.addressComplement = juridica.billingAddress.complement;
                        dConsumerAddress.district = juridica.billingAddress.district;
                        dConsumerAddress.city = juridica.billingAddress.city;
                        dConsumerAddress.cep = juridica.billingAddress.postalCode.ToString();
                        dConsumerAddress.state = juridica.billingAddress.state;
                        dConsumerAddress.country = juridica.billingAddress.country;
                        dConsumerAddress.consumerAddressId = long.MinValue;
                        dConsumerAddress.consumerId = consumerId;
                        //insere o endereco de cobranca
                        DataFactory.ConsumerAddress().Insert(dConsumerAddress);
                    }
                }

            if (reqorder.detail != null && reqorder.detail.deliveryAddress != null)
            {
                //cria o endereco de entrega
                DConsumerAddress dConsumerAddress = new DConsumerAddress();
                dConsumerAddress.logradouro = "";
                dConsumerAddress.address = reqorder.detail.deliveryAddress.location;
                dConsumerAddress.addressNumber = reqorder.detail.deliveryAddress.number.ToString();
                dConsumerAddress.addressType = (int)AddressTypes.Delivery;
                dConsumerAddress.addressComplement = reqorder.detail.deliveryAddress.complement;
                dConsumerAddress.district = reqorder.detail.deliveryAddress.district;
                dConsumerAddress.city = reqorder.detail.deliveryAddress.city;
                dConsumerAddress.cep = reqorder.detail.deliveryAddress.postalCode.ToString();
                dConsumerAddress.state = reqorder.detail.deliveryAddress.state;
                dConsumerAddress.country = reqorder.detail.deliveryAddress.country;
                dConsumerAddress.consumerAddressId = long.MinValue;
                dConsumerAddress.consumerId = consumerId;
                //insere o endereco de entrega
                DataFactory.ConsumerAddress().Insert(dConsumerAddress);
            }
        }
    }
}
