using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using SuperPag.Helper.Xml.Request;
using Resp = SuperPag.Helper.Xml.Response;
using Up = SuperPag.Helper.Xml.Update;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using SuperPag;

namespace SuperPag.Handshake.Service
{
    public class HelperService
    {
        public static Resp.responseOrdersOrderPaymentInstallment ScheduleTransaction(DOrder order, DOrderInstallment installment, InstallmentType installmentType, DateTime date, genericPaymentFormDetail paymentFormDetail, DRecurrence recurrence)
        {
            DSchedule schedule = new DSchedule();
            schedule.orderId = (int)order.orderId;
            schedule.recurrenceId = recurrence == null ? int.MinValue : recurrence.recurrenceId;
            schedule.installmentNumber = installment.installmentNumber;
            schedule.installmentType = (int)installmentType;
            schedule.date = date;
            schedule.paymentFormId = installment.paymentFormId;
            schedule.paymentFormDetail = SuperPag.Helper.Xml.XmlHelper.GetXml(typeof(genericPaymentFormDetail), paymentFormDetail);
            schedule.status = (int)ScheduleStatus.Scheduled;
            DataFactory.Schedule().Insert(schedule);

            Resp.responseOrdersOrderPaymentInstallment responseInstallment = new Resp.responseOrdersOrderPaymentInstallment();
            responseInstallment.dateSpecified = (date != DateTime.MinValue);
            responseInstallment.date = date;
            responseInstallment.paymentDateSpecified = false;
            responseInstallment.paymentFormDetail = HelperService.CreateResponsePaymentFormDetail(installment.paymentFormId);
            responseInstallment.number = (ulong)installment.installmentNumber;
            responseInstallment.status = (byte)OrderInstallmentStatus.PendingPaid;

            installment.status = (int)OrderInstallmentStatus.PendingPaid;
            DataFactory.OrderInstallment().Update(installment);

            return responseInstallment;
        }
        public static Resp.responseOrdersOrderPayment ScheduleTransaction(DOrder order, DOrderInstallment[] installments, InstallmentType installmentType, DateTime date, genericPaymentFormDetail paymentFormDetail, DRecurrence recurrence)
        {
            DSchedule schedule = new DSchedule();
            schedule.orderId = (int)order.orderId;
            schedule.recurrenceId = recurrence == null ? int.MinValue : recurrence.recurrenceId;
            schedule.installmentNumber = int.MinValue;
            schedule.installmentType = (int)installmentType;
            schedule.date = date;
            schedule.paymentFormId = installments[0].paymentFormId;
            schedule.paymentFormDetail = SuperPag.Helper.Xml.XmlHelper.GetXml(typeof(genericPaymentFormDetail), paymentFormDetail); ;
            schedule.status = (int)ScheduleStatus.Scheduled;
            DataFactory.Schedule().Insert(schedule);

            Resp.responseOrdersOrderPayment responsePayment = HelperService.CreateResponsePayment(installments.Length, installments[0].paymentFormId);

            responsePayment.installments[0].dateSpecified = (date != DateTime.MinValue);
            responsePayment.installments[0].date = date;
            responsePayment.installments[0].paymentDateSpecified = false;
            responsePayment.installments[0].number = (ulong)installments[0].installmentNumber;
            responsePayment.installments[0].status = (byte)OrderInstallmentStatus.PendingPaid;

            installments[0].status = (int)OrderInstallmentStatus.PendingPaid;
            DataFactory.OrderInstallment().Update(installments[0]);

            for (int i = 1; i < installments.Length; i++)
            {
                responsePayment.installments[i] = responsePayment.installments[0];
                responsePayment.installments[i].number = (ulong)installments[i].installmentNumber;

                installments[i].status = (int)OrderInstallmentStatus.PendingPaid;
                DataFactory.OrderInstallment().Update(installments[i]);
            }

            return responsePayment;
        }
        public static Resp.responseOrdersOrderPaymentInstallment ScheduleTransaction(DOrder order, DOrderInstallment installment, InstallmentType installmentType, DateTime date, Up.genericPaymentFormDetail paymentFormDetail, DRecurrence recurrence)
        {
            genericPaymentFormDetail pay = new genericPaymentFormDetail();
            if (paymentFormDetail.Item.GetType().Equals(typeof(Up.genericPaymentFormDetailBoletoInformation)))
            {
                pay.Item = new genericPaymentFormDetailBoletoInformation();
                ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((Up.genericPaymentFormDetailBoletoInformation)paymentFormDetail.Item).dueDate;
                ((genericPaymentFormDetailBoletoInformation)pay.Item).instructions = ((Up.genericPaymentFormDetailBoletoInformation)paymentFormDetail.Item).instructions;
            }
            else if (paymentFormDetail.Item.GetType().Equals(typeof(Up.genericPaymentFormDetailCreditCardInformation)))
            {
                pay.Item = new genericPaymentFormDetailCreditCardInformation();
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).cardHolder = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).cardHolder;
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).cardNumber = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).cardNumber;
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).expireDate = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).expireDate;
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).securityCode = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).securityCode;
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).captureSpecified = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).captureSpecified;
                if (((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).captureSpecified)
                    ((genericPaymentFormDetailCreditCardInformation)pay.Item).capture = (capture)((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).capture;
            }

            return ScheduleTransaction(order, installment, installmentType, date, (genericPaymentFormDetail)pay, recurrence);
        }
        public static Resp.responseOrdersOrderPayment ScheduleTransaction(DOrder order, DOrderInstallment[] installments, InstallmentType installmentType, DateTime date, Up.genericPaymentFormDetail paymentFormDetail, DRecurrence recurrence)
        {
            genericPaymentFormDetail pay = new genericPaymentFormDetail();
            if (paymentFormDetail.Item.GetType().Equals(typeof(Up.genericPaymentFormDetailBoletoInformation)))
            {
                pay.Item = new genericPaymentFormDetailBoletoInformation();
                ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate = ((Up.genericPaymentFormDetailBoletoInformation)paymentFormDetail.Item).dueDate;
                ((genericPaymentFormDetailBoletoInformation)pay.Item).instructions = ((Up.genericPaymentFormDetailBoletoInformation)paymentFormDetail.Item).instructions;
            }
            else if (paymentFormDetail.Item.GetType().Equals(typeof(Up.genericPaymentFormDetailCreditCardInformation)))
            {
                pay.Item = new genericPaymentFormDetailCreditCardInformation();
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).cardHolder = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).cardHolder;
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).cardNumber = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).cardNumber;
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).expireDate = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).expireDate;
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).securityCode = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).securityCode;
                ((genericPaymentFormDetailCreditCardInformation)pay.Item).captureSpecified = ((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).captureSpecified;
                if (((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).captureSpecified)
                    ((genericPaymentFormDetailCreditCardInformation)pay.Item).capture = (capture)((Up.genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item).capture;
            }

            return ScheduleTransaction(order, installments, installmentType, date, (genericPaymentFormDetail)pay, recurrence);
        }

        public static Resp.responseOrdersOrderPayment CreateResponsePayment(int installmentQuantity, int paymentFormId)
        {
            Resp.responseOrdersOrderPayment payment = new Resp.responseOrdersOrderPayment();
            payment.form = (ushort)paymentFormId;
            payment.status = (byte)PaymentAttemptStatus.PendingPaid;

            payment.installments = new Resp.responseOrdersOrderPaymentInstallment[installmentQuantity];
            for (int i = 0; i < installmentQuantity; i++)
            {
                payment.installments[i] = new Resp.responseOrdersOrderPaymentInstallment();
                payment.installments[i].dateSpecified = false;
                payment.installments[i].number = (ulong)i;
                payment.installments[i].paymentDateSpecified = false;
                payment.installments[i].status = (byte)PaymentAttemptStatus.PendingPaid;
                payment.installments[i].paymentFormDetail = new Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail();
                payment.installments[i].paymentFormDetail = CreateResponsePaymentFormDetail(payment.form);
            }

            return payment;
        }
        public static Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail CreateResponsePaymentFormDetail(int paymentFormId)
        {
            Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail paymentformDetail = new Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail();

            switch ((PaymentForms)paymentFormId)
            {
                case PaymentForms.Amex2Party:
                case PaymentForms.Amex3Party:
                case PaymentForms.AmexSitef:
                case PaymentForms.DinersKomerci:
                case PaymentForms.DinersKomerciInBox:
                case PaymentForms.DinersSitef:
                case PaymentForms.DinersWebService:
                case PaymentForms.HipercardSitef:
                case PaymentForms.MasterKomerci:
                case PaymentForms.MasterKomerciInBox:
                case PaymentForms.MasterSitef:
                case PaymentForms.MasterWebService:
                case PaymentForms.VisaMoset:
                case PaymentForms.VisaMoset3:
                case PaymentForms.VisaSitef:
                case PaymentForms.VisaVBV:
                case PaymentForms.VisaVBVInBox:
                case PaymentForms.VisaVBV3:
                    Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation ccard = new Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation();
                    ccard.acquirerReturnCode = "";
                    ccard.acquirerReturnMessage = "";
                    ccard.authorizationId = "";
                    ccard.returnCode = Resp.returnCode.Item;
                    ccard.transactionId = "";
                    paymentformDetail.Item = ccard;
                    break;
                case PaymentForms.BoletoBancoDoBrasil:
                case PaymentForms.BoletoBradesco:
                case PaymentForms.BoletoItau:
                case PaymentForms.BoletoHSBC:
                    Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation bol = new Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation();
                    bol.number = "";
                    bol.typingLine = "";
                    bol.url = "";
                    bol.paidValueSpecified = false;
                    paymentformDetail.Item = bol;
                    break;
            }

            return paymentformDetail;
        }

        public static Resp.responseOrdersOrderPaymentInstallment StartTransaction(DOrder order, DOrderInstallment installment, InstallmentType installmentType, SuperPag.Helper.Xml.Request.genericPaymentFormDetail paymentFormDetail, out DPaymentAttempt attempt, requestOrderPaymentsPayment payment)
        {
            DPaymentAttempt paymentAttempt = new DPaymentAttempt();
            paymentAttempt.paymentAttemptId = Guid.NewGuid();
            paymentAttempt.price = installment.installmentValue;
            paymentAttempt.orderId = order.orderId;
            paymentAttempt.paymentFormId = installment.paymentFormId;
            paymentAttempt.paymentAgentSetupId = GenericHelper.GetPaymentAgentSetupId(order.storeId, installment.paymentFormId);
            paymentAttempt.startTime = DateTime.Now;
            paymentAttempt.lastUpdate = DateTime.Now;
            paymentAttempt.step = 0;
            paymentAttempt.installmentNumber = installment.installmentNumber;
            paymentAttempt.status = (int)PaymentAttemptStatus.Pending;
            paymentAttempt.billingScheduleId = int.MinValue;
            paymentAttempt.isSimulation = false;
            DataFactory.PaymentAttempt().Insert(paymentAttempt);

            attempt = paymentAttempt;

            Resp.responseOrdersOrderPaymentInstallment responseInstallment = new Resp.responseOrdersOrderPaymentInstallment();

            DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(installment.paymentFormId);
            switch ((PaymentAgents)paymentForm.paymentAgentId)
            {
                case PaymentAgents.Boleto:
                    responseInstallment = SuperPag.Agents.Boleto.SystemInterface.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailBoletoInformation)paymentFormDetail.Item);
                    break;
                case PaymentAgents.PaymentClientVirtual2Party:
                    responseInstallment = SuperPag.Agents.PaymentClientVirtual.SystemInterface.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.VisaMoset:
                    responseInstallment = SuperPag.Agents.VisaMoset.SystemInterface.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.VisaMoset3:
                    responseInstallment = SuperPag.Agents.VisaMoset3.SystemInterface.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.KomerciWS:
                    responseInstallment = SuperPag.Agents.KomerciWS.KomerciWS.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.DebitoContaCorrente:
                    responseInstallment = SuperPag.Agents.ContaCorrente.ContaCorrente.ProcessPayment(order, installment, paymentAttempt, (genericPaymentContaCorrenteInformation)paymentFormDetail.Item, payment);
                    break;
            }

            responseInstallment.number = (ulong)installment.installmentNumber;

            DStore dStore = DataFactory.Store().Locate(order.storeId);
            if (dStore != null && dStore.handshakeConfigurationId != int.MinValue)
            {
                DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(dStore.handshakeConfigurationId);
                if (dHandshakeConfiguration != null && dHandshakeConfiguration.sendEmailConsumer)
                    SuperPag.Handshake.Helper.SendFinalizationConsumerEmail(paymentAttempt.paymentAttemptId, null, "", "");
            }

            return responseInstallment;
        }
        public static Resp.responseOrdersOrderPaymentInstallment StartTransaction(DOrder order, DOrderInstallment installment, InstallmentType installmentType, SuperPag.Helper.Xml.Request.genericPaymentFormDetail paymentFormDetail, out DPaymentAttempt attempt, requestOrderPaymentsPayment payment, Guid Userid)
        {
            DPaymentAttempt paymentAttempt = new DPaymentAttempt();
            paymentAttempt.paymentAttemptId = Guid.NewGuid();
            paymentAttempt.price = installment.installmentValue;
            paymentAttempt.orderId = order.orderId;
            paymentAttempt.paymentFormId = installment.paymentFormId;
            paymentAttempt.paymentAgentSetupId = GenericHelper.GetPaymentAgentSetupId(order.storeId, installment.paymentFormId);
            paymentAttempt.startTime = DateTime.Now;
            paymentAttempt.lastUpdate = DateTime.Now;
            paymentAttempt.step = 0;
            paymentAttempt.installmentNumber = installment.installmentNumber;
            paymentAttempt.status = (int)PaymentAttemptStatus.Pending;
            paymentAttempt.billingScheduleId = int.MinValue;
            paymentAttempt.isSimulation = false;
            DataFactory.PaymentAttempt().Insert(paymentAttempt);

            attempt = paymentAttempt;

            Resp.responseOrdersOrderPaymentInstallment responseInstallment = new Resp.responseOrdersOrderPaymentInstallment();

            DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(installment.paymentFormId);
            switch ((PaymentAgents)paymentForm.paymentAgentId)
            {
                case PaymentAgents.Boleto:
                    //TUDO:modificação para ponto cred
                    if (payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                    {
                        responseInstallment = SuperPag.Agents.Boleto.SystemInterface.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailBoletoInformation)paymentFormDetail.Item);
                    }
                    else
                    {
                        responseInstallment = SuperPag.Agents.Boleto.SystemInterface.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailBoletoInformationIPTE)paymentFormDetail.Item, Userid);
                    }
                    break;
                case PaymentAgents.PaymentClientVirtual2Party:
                    responseInstallment = SuperPag.Agents.PaymentClientVirtual.SystemInterface.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.VisaMoset:
                    responseInstallment = SuperPag.Agents.VisaMoset.SystemInterface.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.VisaMoset3:
                    responseInstallment = SuperPag.Agents.VisaMoset3.SystemInterface.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.KomerciWS:
                    responseInstallment = SuperPag.Agents.KomerciWS.KomerciWS.ProcessPayment(order, installment, paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.DebitoContaCorrente:
                    responseInstallment = SuperPag.Agents.ContaCorrente.ContaCorrente.ProcessPayment(order, installment, paymentAttempt, (genericPaymentContaCorrenteInformation)paymentFormDetail.Item, payment);
                    break;
            }

            responseInstallment.number = (ulong)installment.installmentNumber;

            DStore dStore = DataFactory.Store().Locate(order.storeId);
            if (dStore != null && dStore.handshakeConfigurationId != int.MinValue)
            {
                DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(dStore.handshakeConfigurationId);
                if (dHandshakeConfiguration != null && dHandshakeConfiguration.sendEmailConsumer)
                    SuperPag.Handshake.Helper.SendFinalizationConsumerEmail(paymentAttempt.paymentAttemptId, null, "", "");
            }

            return responseInstallment;
        }
        public static Resp.responseOrdersOrderPayment StartTransaction(DOrder order, DOrderInstallment[] installments, InstallmentType installmentType, SuperPag.Helper.Xml.Request.genericPaymentFormDetail paymentFormDetail, out DPaymentAttempt attempt, requestOrderPaymentsPayment payment)
        {
            DPaymentAttempt paymentAttempt = new DPaymentAttempt();
            paymentAttempt.paymentAttemptId = Guid.NewGuid();
            paymentAttempt.price = order.finalAmount;
            paymentAttempt.orderId = order.orderId;
            paymentAttempt.paymentFormId = installments[0].paymentFormId;
            paymentAttempt.paymentAgentSetupId = GenericHelper.GetPaymentAgentSetupId(order.storeId, installments[0].paymentFormId);
            paymentAttempt.startTime = DateTime.Now;
            paymentAttempt.lastUpdate = DateTime.Now;
            paymentAttempt.step = 0;
            paymentAttempt.installmentNumber = int.MinValue;
            paymentAttempt.status = (int)PaymentAttemptStatus.Pending;
            paymentAttempt.billingScheduleId = int.MinValue;
            paymentAttempt.isSimulation = false;
            DataFactory.PaymentAttempt().Insert(paymentAttempt);

            attempt = paymentAttempt;

            Resp.responseOrdersOrderPayment responsePayment = HelperService.CreateResponsePayment(installments.Length, installments[0].paymentFormId);

            DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(installments[0].paymentFormId);
            switch ((PaymentAgents)paymentForm.paymentAgentId)
            {
                case PaymentAgents.Boleto:
                    responsePayment.installments[0] = SuperPag.Agents.Boleto.SystemInterface.ProcessPayment(order, installments[0], paymentAttempt, (genericPaymentFormDetailBoletoInformation)paymentFormDetail.Item);
                    break;
                case PaymentAgents.PaymentClientVirtual2Party:
                    responsePayment.installments[0] = SuperPag.Agents.PaymentClientVirtual.SystemInterface.ProcessPayment(order, installments[0], paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.VisaMoset:
                    responsePayment.installments[0] = SuperPag.Agents.VisaMoset.SystemInterface.ProcessPayment(order, installments[0], paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.VisaMoset3:
                    responsePayment.installments[0] = SuperPag.Agents.VisaMoset3.SystemInterface.ProcessPayment(order, installments[0], paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.KomerciWS:
                    responsePayment.installments[0] = SuperPag.Agents.KomerciWS.KomerciWS.ProcessPayment(order, installments[0], paymentAttempt, (genericPaymentFormDetailCreditCardInformation)paymentFormDetail.Item, installmentType);
                    break;
                case PaymentAgents.DebitoContaCorrente:
                    if (payment.paymentFormDetail.Item.GetType().Equals(typeof(genericPaymentContaCorrenteInformationCSU)))
                    {
                        responsePayment.installments[0] = SuperPag.Agents.ContaCorrente.ContaCorrente.ProcessPayment(order, installments[0], paymentAttempt, (genericPaymentContaCorrenteInformationCSU)paymentFormDetail.Item, payment);
                    }
                    else
                    {
                        responsePayment.installments[0] = SuperPag.Agents.ContaCorrente.ContaCorrente.ProcessPayment(order, installments[0], paymentAttempt, (genericPaymentContaCorrenteInformation)paymentFormDetail.Item, payment);
                    }
                    break;
            }

            for (int i = 1; i < installments.Length; i++)
            {
                responsePayment.installments[i] = responsePayment.installments[0];
                responsePayment.installments[i].number = (ulong)installments[i].installmentNumber;

                installments[i].status = installments[0].status;
                DataFactory.OrderInstallment().Update(installments[i]);
            }

            DStore dStore = DataFactory.Store().Locate(order.storeId);
            if (dStore != null && dStore.handshakeConfigurationId != int.MinValue)
            {
                DHandshakeConfiguration dHandshakeConfiguration = DataFactory.HandshakeConfiguration().Locate(dStore.handshakeConfigurationId);
                if (dHandshakeConfiguration != null && dHandshakeConfiguration.sendEmailConsumer)
                    SuperPag.Handshake.Helper.SendFinalizationConsumerEmail(paymentAttempt.paymentAttemptId, null, "", "");
            }

            return responsePayment;
        }

        public static PaymentAttemptStatus GetOrderStatus(Resp.responseOrdersOrderPayment[] payments)
        {
            if (payments == null || payments.Length < 1)
                return PaymentAttemptStatus.Pending;

            int? definitive = null, actual = null;
            for (int i = 0; i < payments.Length; i++)
            {
                actual = (int)payments[i].status;
                definitive = (definitive == null ? actual : definitive);

                if (actual != definitive)
                    if ((actual == (int)PaymentAttemptStatus.Pending || actual == (int)PaymentAttemptStatus.NotPaid) &&
                       (definitive == (int)PaymentAttemptStatus.Pending || definitive == (int)PaymentAttemptStatus.NotPaid))
                        definitive = (int)PaymentAttemptStatus.NotPaid;
                    else
                        definitive = (int)PaymentAttemptStatus.PendingPaid;
            }

            return (PaymentAttemptStatus)(definitive == null ? (int)PaymentAttemptStatus.Pending : definitive);
        }
        public static PaymentAttemptStatus GetPaymentStatus(Resp.responseOrdersOrderPaymentInstallment[] installments)
        {
            if (installments == null || installments.Length < 1)
                return PaymentAttemptStatus.Pending;

            int? definitive = null, actual = null;
            for (int i = 0; i < installments.Length; i++)
            {
                actual = (int)installments[i].status;
                definitive = (definitive == null ? actual : definitive);

                if (actual != definitive)
                    if ((actual == (int)PaymentAttemptStatus.Pending || actual == (int)PaymentAttemptStatus.NotPaid) &&
                       (definitive == (int)PaymentAttemptStatus.Pending || definitive == (int)PaymentAttemptStatus.NotPaid))
                        definitive = (int)PaymentAttemptStatus.NotPaid;
                    else
                        definitive = (int)PaymentAttemptStatus.PendingPaid;
            }

            return (PaymentAttemptStatus)(definitive == null ? (int)PaymentAttemptStatus.Pending : definitive);
        }
        public static void CheckOrderByStore(DStore store, DOrder order)
        {
            if (order.storeId != store.storeId)
                Ensure.IsNotNull(null, "O pedido enviado não pertence a loja que requisitou a operação");
        }
        public static bool CheckScheduleInstallmentNumbers(DSchedule[] schedules)
        {
            if (schedules == null)
                return false;

            if (schedules.Length < 1)
                return false;

            if (schedules[0].installmentNumber != 1 && schedules[0].installmentNumber != 2)
                return false;

            for (int i = 0; i < schedules.Length; i++)
            {
                if (schedules[i].installmentNumber == int.MinValue)
                    return false;

                if (i > 0 && schedules[i].installmentNumber != (schedules[i - 1].installmentNumber + 1))
                    return false;
            }

            return true;
        }
        public static Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail GetResponsePaymentForm(DPaymentAttempt attempt, out bool paymentDateSpecified, out DateTime paymentDate)
        {
            paymentDate = attempt.lastUpdate;
            paymentDateSpecified = (attempt.status == (int)PaymentAttemptStatus.Paid && paymentDate != DateTime.MinValue);
            Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail pay = CreateResponsePaymentFormDetail(attempt.paymentFormId);

            DPaymentForm paymentForm = DataFactory.PaymentForm().Locate(attempt.paymentFormId);
            if (paymentForm == null)
                return pay;

            switch (paymentForm.paymentAgentId)
            {
                case (int)PaymentAgents.Boleto:
                    string serverUrl = "";
                    if (HttpContext.Current != null)
                    {
                        string http = (HttpContext.Current.Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
                        string server = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                        serverUrl = String.Format("{0}://{1}", http, server);
                    }
                    else if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["ServerUrl"] != null)
                        serverUrl = ConfigurationManager.AppSettings["ServerUrl"];

                    DPaymentAttemptBoleto boleto = DataFactory.PaymentAttemptBoleto().Locate(attempt.paymentAttemptId);
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation)pay.Item).number = boleto.ourNumber;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation)pay.Item).typingLine = boleto.oct;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation)pay.Item).url = String.Format("{0}/Agents/Boleto/showboleto.aspx?id={1}", serverUrl, attempt.paymentAttemptId);

                    paymentDate = DateTime.MinValue;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation)pay.Item).paidValueSpecified = false;

                    //Valor Pago e Data de Pagamento efetivos do retorno do boleto
                    DPaymentAttemptBoletoReturn boletoReturn = DataFactory.PaymentAttemptBoletoReturn().Locate(boleto.agentOrderReference.ToString());
                    if (boletoReturn != null)
                    {
                        paymentDate = boletoReturn.dataLiquidacao;
                        ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation)pay.Item).paidValueSpecified = (attempt.status == (int)PaymentAttemptStatus.Paid && boletoReturn.valorRecebido != decimal.MinValue);
                        ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailBoletoInformation)pay.Item).paidValue = (ulong)(GenericHelper.ConvertDecimal(boletoReturn.valorRecebido, 2) * 100);
                    }

                    break;
                case (int)PaymentAgents.VBV:
                case (int)PaymentAgents.VBVInBox:
                case (int)PaymentAgents.VBV3:
                    DPaymentAttemptVBV vbv = DataFactory.PaymentAttemptVBV().Locate(attempt.paymentAttemptId);
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).authorizationId = (vbv.arp != int.MinValue ? vbv.arp.ToString() : "");
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).transactionId = vbv.tid;
                    //TODO: verificar como determinar o returnCode da transacao
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).returnCode = Resp.returnCode.Item;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnCode = (vbv.lr != decimal.MinValue ? vbv.lr.ToString() : "");
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnMessage = vbv.ars;
                    break;
                case (int)PaymentAgents.VisaMoset:
                case (int)PaymentAgents.VisaMoset3:
                    DPaymentAttemptMoset moset = DataFactory.PaymentAttemptMoset().Locate(attempt.paymentAttemptId);
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).authorizationId = "";
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).transactionId = moset.tid;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).returnCode = Resp.returnCode.Item;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnCode = (moset.capturedCod != int.MinValue ? moset.capturedCod.ToString() : (moset.lr != int.MinValue ? moset.lr.ToString() : ""));
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnMessage = (moset.capturedCod != int.MinValue ? moset.capturedArs : moset.message);
                    break;
                case (int)PaymentAgents.Komerci:
                case (int)PaymentAgents.KomerciInBox:
                    DPaymentAttemptKomerci komerci = DataFactory.PaymentAttemptKomerci().Locate(attempt.paymentAttemptId);
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).authorizationId = komerci.numautor;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).transactionId = komerci.numcv;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).returnCode = Resp.returnCode.Item;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnCode = komerci.codret;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnMessage = komerci.msgret;
                    break;
                case (int)PaymentAgents.KomerciWS:
                    DPaymentAttemptKomerciWS komerciWS = DataFactory.PaymentAttemptKomerciWS().Locate(attempt.paymentAttemptId);
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).authorizationId = komerciWS.numautor;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).transactionId = komerciWS.numcv;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).returnCode = Resp.returnCode.Item;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnCode = (!String.IsNullOrEmpty(komerciWS.capcodret) ? komerciWS.capcodret : komerciWS.codret);
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnMessage = (!String.IsNullOrEmpty(komerciWS.capmsgret) ? komerciWS.capmsgret : komerciWS.msgret);
                    break;
                case (int)PaymentAgents.PaymentClientVirtual2Party:
                case (int)PaymentAgents.PaymentClientVirtual3Party:
                    DPaymentAttemptPaymentClientVirtual payclient = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(attempt.paymentAttemptId);
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).authorizationId = payclient.vpc_AuthorizeId.ToString();
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).transactionId = payclient.vpc_TransactionNo.ToString();
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).returnCode = Resp.returnCode.Item;
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnCode = (!String.IsNullOrEmpty(payclient.vpc_CapTxnResponseCode) ? payclient.vpc_CapTxnResponseCode : payclient.vpc_TxnResponseCode);
                    ((Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation)pay.Item).acquirerReturnMessage = (!String.IsNullOrEmpty(payclient.vpc_CaptureMessage) ? payclient.vpc_CaptureMessage : payclient.vpc_Message);
                    break;
                case (int)PaymentAgents.DebitoContaCorrente:                    
                    Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailDebitoContaCorrenteInformation ObjCC = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetailDebitoContaCorrenteInformation();
                    SuperPag.Business.Messages.MPaymentAttemptContaCorrente ObjMPaymentAttemptContaCorrente = SuperPag.Business.PaymentAttemptContaCorrente.GetInstance().Locate(attempt.paymentAttemptId);

                    ObjCC.Status = ObjMPaymentAttemptContaCorrente.Status;
                    ObjCC.Status = ObjMPaymentAttemptContaCorrente.Status;
                    ObjCC.DataVencimento = ObjMPaymentAttemptContaCorrente.DataVencimento;
                    ObjCC.ValorAgendado = ObjMPaymentAttemptContaCorrente.ValorAgendado;
                    ObjCC.NumInstituicao = Convert.ToInt32(ObjMPaymentAttemptContaCorrente.NumInstituicao);
                    ObjCC.Ocorrencia = ObjMPaymentAttemptContaCorrente.Ocorrencia;
                    ObjCC.DataProcessamento = Convert.ToDateTime(ObjMPaymentAttemptContaCorrente.DataProcessamento);

                    pay.Item = ObjCC;
                    break;

            }

            paymentDateSpecified = (attempt.status == (int)PaymentAttemptStatus.Paid && paymentDate != DateTime.MinValue);

            return pay;
        }

        public static Resp.responseOrdersOrderPayment GetResponseByPaymentNotInSchedule(DPaymentAttempt[] attempts, DSchedule[] schedules)
        {
            if (attempts == null || attempts.Length < 1)
                return null;

            List<DPaymentAttempt> attemptsNotInSchedule = new List<DPaymentAttempt>();

            for (int i = 0; i < attempts.Length; i++)
            {
                bool notInSchedule = true;
                foreach (DSchedule schedule in schedules)
                    if (schedule.paymentAttemptId != null && schedule.paymentAttemptId != Guid.Empty && attempts[i].paymentAttemptId == attempts[i].paymentAttemptId)
                    {
                        notInSchedule = false;
                        break;
                    }

                if (notInSchedule)
                    attemptsNotInSchedule.Add(attempts[i]);
            }

            if (attemptsNotInSchedule.Count < 1)
                return null;

            DPaymentAttempt attempt = GenericHelper.ChooseAttemptByStatus(attemptsNotInSchedule.ToArray());
            return HelperService.GetResponseByPayment(attempt);
        }

        public static Resp.responseOrdersOrderPayment GetResponseByPayment(DPaymentAttempt attempt)
        {
            DateTime paymentDate;
            bool paymentDateSpecified;

            Resp.responseOrdersOrderPayment ret = HelperService.CreateResponsePayment(1, attempt.paymentFormId);
            ret.status = (byte)attempt.status;
            ret.installments[0].status = (byte)attempt.status;
            ret.installments[0].dateSpecified = (attempt.lastUpdate != DateTime.MinValue);
            ret.installments[0].date = attempt.lastUpdate;
            ret.installments[0].number = 1;
            ret.installments[0].paymentFormDetail = GetResponsePaymentForm(attempt, out paymentDateSpecified, out paymentDate);
            ret.installments[0].paymentDateSpecified = paymentDateSpecified;
            ret.installments[0].paymentDate = paymentDate;

            return ret;
        }
        public static Resp.responseOrdersOrderPayment GetResponseByPayment(DOrderInstallment[] installments, Dictionary<int, DPaymentAttempt> attempts)
        {
            Resp.responseOrdersOrderPayment ret = HelperService.CreateResponsePayment(installments.Length, installments[0].paymentFormId);

            DateTime paymentDate = DateTime.MinValue;
            bool paymentDateSpecified = false;
            int form = installments[0].paymentFormId;

            for (int i = 0; i < installments.Length; i++)
            {
                Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail pay = null;
                DPaymentAttempt attempt = null;
                int status = (int)PaymentAttemptStatus.Pending;
                DateTime lastUpdate = DateTime.MinValue;

                if (attempts.TryGetValue(installments[i].installmentNumber, out attempt))
                {
                    status = attempt.status;
                    lastUpdate = attempt.lastUpdate;
                    form = attempt.paymentFormId;
                    pay = GetResponsePaymentForm(attempt, out paymentDateSpecified, out paymentDate);
                }

                ret.status = (byte)status;
                ret.installments[i].status = (byte)status;
                ret.installments[i].dateSpecified = (lastUpdate != DateTime.MinValue);
                ret.installments[i].date = lastUpdate;
                ret.installments[i].number = (ulong)installments[i].installmentNumber;
                ret.installments[i].paymentDateSpecified = paymentDateSpecified;
                ret.installments[i].paymentDate = paymentDate;
                ret.installments[i].paymentFormDetail.Item = (pay != null && pay.Item != null ? pay.Item : ret.installments[0].paymentFormDetail.Item);
            }

            ret.form = (ushort)form;
            ret.status = (byte)HelperService.GetPaymentStatus(ret.installments);

            return ret;
        }
        public static Resp.responseOrdersOrderPayment GetResponseBySchedule(DSchedule schedule)
        {
            if (schedule.paymentAttemptId != null && schedule.paymentAttemptId != Guid.Empty)
            {
                DPaymentAttempt attemtp = DataFactory.PaymentAttempt().Locate(schedule.paymentAttemptId);
                if (attemtp != null)
                    return GetResponseByPayment(attemtp);
            }

            Resp.responseOrdersOrderPayment ret = HelperService.CreateResponsePayment(1, schedule.paymentFormId);
            ret.status = (byte)PaymentAttemptStatus.PendingPaid;
            ret.installments[0].status = (byte)PaymentAttemptStatus.PendingPaid;
            ret.installments[0].dateSpecified = (schedule.date != DateTime.MinValue);
            ret.installments[0].date = schedule.date;
            ret.installments[0].number = (ulong)schedule.installmentNumber;
            ret.installments[0].paymentDateSpecified = false;

            return ret;
        }
        public static Resp.responseOrdersOrderPayment GetResponseBySchedule(DOrderInstallment[] installments, DSchedule schedule)
        {
            Resp.responseOrdersOrderPayment ret = HelperService.CreateResponsePayment(installments.Length, schedule.paymentFormId);

            if (schedule.status == (int)ScheduleStatus.Processed)
            {
                Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail pay = null;
                DateTime paymentDate = DateTime.MinValue;
                bool paymentDateSpecified = false;
                int status = (int)PaymentAttemptStatus.Pending;

                if (schedule.paymentAttemptId != null)
                {
                    DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(schedule.paymentAttemptId);
                    if (attempt != null)
                    {
                        status = attempt.status;
                        pay = GetResponsePaymentForm(attempt, out paymentDateSpecified, out paymentDate);
                    }
                }

                for (int i = 0; i < installments.Length; i++)
                {
                    ret.status = (byte)status;
                    ret.installments[i].status = (byte)status;
                    ret.installments[i].dateSpecified = (schedule.date != DateTime.MinValue);
                    ret.installments[i].date = schedule.date;
                    ret.installments[i].number = (ulong)installments[i].installmentNumber;
                    ret.installments[i].paymentDateSpecified = paymentDateSpecified;
                    ret.installments[i].paymentDate = paymentDate;
                    ret.installments[i].paymentFormDetail.Item = (pay != null && pay.Item != null ? pay.Item : ret.installments[0].paymentFormDetail.Item);
                }
            }
            else
            {
                for (int i = 0; i < installments.Length; i++)
                {
                    ret.status = (schedule.status == (int)ScheduleStatus.Scheduled ? (byte)PaymentAttemptStatus.PendingPaid : (byte)PaymentAttemptStatus.Canceled);
                    ret.installments[i].status = (schedule.status == (int)ScheduleStatus.Scheduled ? (byte)PaymentAttemptStatus.PendingPaid : (byte)PaymentAttemptStatus.Canceled);
                    ret.installments[i].dateSpecified = (schedule.date != DateTime.MinValue);
                    ret.installments[i].date = schedule.date;
                    ret.installments[i].number = (ulong)installments[i].installmentNumber;
                    ret.installments[i].paymentDateSpecified = false;
                }
            }

            return ret;
        }
    }
}
