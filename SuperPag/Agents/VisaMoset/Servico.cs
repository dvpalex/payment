using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Helper;
using SuperPag.Data;
using SuperPag.Helper.Xml.Request;
using SuperPag.Helper.Xml.Response;
using System.Configuration;
using SuperPag;

namespace SuperPag.Agents.VisaMoset
{
    class SystemInterface
    {
        public static responseOrdersOrderPaymentInstallment ProcessPayment(DOrder order, DOrderInstallment orderInstallment, DPaymentAttempt attempt, SuperPag.Helper.Xml.Request.genericPaymentFormDetailCreditCardInformation paymentInfo, InstallmentType installmentType)
        {
            Ensure.IsNotNull(order, "O pedido deve ser informado");
            Ensure.IsNotNull(attempt, "A tentativa de pagamento deve ser informada");
            Ensure.IsNotNull(orderInstallment, "A parcela relativa deve ser informada");
            Ensure.IsNotNull(paymentInfo, "Os dados do cartão devem ser informados");
            
            DPaymentAgentSetupMoset agentsetup = DataFactory.PaymentAgentSetupMoset().Locate(attempt.paymentAgentSetupId);
            Ensure.IsNotNull(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");

            //Inicializo as classes para retorno
            responseOrdersOrderPaymentInstallment installment = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallment();
            responseOrdersOrderPaymentInstallmentPaymentFormDetail pfDetail = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetail();
            responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation ccInfo = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation();

            DateTime paymentDate = DateTime.MinValue;

            string urlComponent = ConfigurationManager.AppSettings["MosetComponentUrl"];
            
            string tid = "";
            DateTime tidDate = DateTime.Now;

            //TODO:alteração marcos
            //bool autoCapture = (paymentInfo.captureSpecified && paymentInfo.capture == SuperPag.Helper.Xml.Request.capture.auto ? true : false);
            bool autoCapture = (paymentInfo.captureSpecified ? (paymentInfo.capture == SuperPag.Helper.Xml.Request.capture.auto ? true : false) : true);

            tid = agentsetup.merchantId.ToString().PadLeft(10, '0').Substring(4, 5);
            tid += tidDate.Year.ToString("0000").Substring(3, 1);
            tid += tidDate.DayOfYear.ToString("000");
            tid += tidDate.ToString("hhmmss");
            tid += tidDate.Millisecond.ToString().Substring(0, 1);
            if (order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue)
                tid += "1001"; // Cartões VISA à vista
            else
                //Enviar tipo de pagamento especifico
                tid += (installmentType == InstallmentType.Emissor ? "3" : "2") + order.installmentQuantity.ToString("000");

            DPaymentAttemptMoset attemptMoset = new DPaymentAttemptMoset();
            attemptMoset.paymentAttemptId = attempt.paymentAttemptId;
            attemptMoset.merchantId = agentsetup.merchantId;
            attemptMoset.cardInformation = GenericHelper.CreateCreditCardXml(paymentInfo.cardHolder, paymentInfo.cardNumber.ToString(), paymentInfo.securityCode.ToString(), GenericHelper.ParseDate(paymentInfo.expireDate + "-01", "yyyy-MM-dd"));
            attemptMoset.tid = tid;
            attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.Send;
            DataFactory.PaymentAttemptMoset().Insert(attemptMoset);

            attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.End;
            attempt.status = (int)PaymentAttemptStatus.NotPaid;
            orderInstallment.status = (int)OrderInstallmentStatus.NotPaid;
            installment.status = (byte)PaymentAttemptStatus.NotPaid;
            ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item2; //nao aprovada

            Moset moset = new Moset();
            bool result = moset.ProcessPayment(urlComponent, order.orderId, attemptMoset.tid, attempt.price,
                paymentInfo.cardNumber.ToString(), GenericHelper.ParseDate(paymentInfo.expireDate + "-01", "yyyy-MM-dd"), agentsetup.merchantId, "cfg" + agentsetup.paymentAgentSetupId.ToString(), attempt.paymentAttemptId.ToString());

            if (result)
            {
                attemptMoset.free = moset.Free;
                attemptMoset.lr = moset.Lr;

                if (attemptMoset.lr != int.MinValue)
                {
                    // verifico se o TID retornado é mesmo que está gravado no banco de dados
                    if (moset.Tid != attemptMoset.tid)
                        Ensure.IsNotNull(null, "TID inconsistente");

                    attempt.returnMessage = moset.GetPaymentProcessResponseDescription();
                    attemptMoset.message = moset.GetPaymentProcessResponseDescription();
                    ccInfo.acquirerReturnCode = attemptMoset.lr.ToString();

                    if (attemptMoset.lr == 0 || attemptMoset.lr == 11)
                    {
                        ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item3; //aprovada e pendente de captura
                        ccInfo.authorizationId = moset.Arp;

                        if (autoCapture)
                        {
                            attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.Capture;
                            attemptMoset.TruncateStringFields();
                            DataFactory.PaymentAttemptMoset().Update(attemptMoset);

                            bool resultCapture = moset.Capture(urlComponent, attemptMoset.tid, "cfg" + agentsetup.paymentAgentSetupId.ToString());

                            if (resultCapture)
                            {
                                attemptMoset.capturedTid = moset.Tid;
                                attemptMoset.capturedArs = moset.Ars;
                                attemptMoset.capturedCod = moset.Cod;
                                attemptMoset.capturedCap = moset.Cap;
                                attemptMoset.capturedCurrency = moset.GetCapturedCurrency();
                                attemptMoset.capturedValue = moset.GetCapturedValue();

                                if (attemptMoset.capturedCod != int.MinValue)
                                {
                                    attempt.returnMessage = moset.GetCaptureResponseDescription();
                                    attemptMoset.message = moset.GetCaptureResponseDescription();
                                    ccInfo.acquirerReturnCode = attemptMoset.capturedCod.ToString();

                                    if (attemptMoset.capturedCod == 0)
                                    {
                                        attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.End;
                                        attempt.status = (int)PaymentAttemptStatus.Paid;
                                        orderInstallment.status = (int)OrderInstallmentStatus.Paid;
                                        installment.status = (byte)PaymentAttemptStatus.Paid;
                                        ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item1; //aprovada
                                    }
                                }
                                else
                                {
                                    attemptMoset.capturedCod = -1;
                                    attemptMoset.message = "retorno da captura não reconhecido: " + moset.MsgretCapture;
                                    attempt.returnMessage = "Retorno da captura não reconhecido";
                                    ccInfo.acquirerReturnCode = attemptMoset.capturedCod.ToString();
                                }
                            }
                            else
                            {
                                attemptMoset.capturedCod = -1;
                                attemptMoset.message = moset.MsgretCapture;
                                attempt.returnMessage = "Problemas na captura";
                                ccInfo.acquirerReturnCode = attemptMoset.capturedCod.ToString();
                            }
                        }
                        else
                        {
                            attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.WaitingCapture;
                            attempt.status = (int)PaymentAttemptStatus.Paid;
                            orderInstallment.status = (int)OrderInstallmentStatus.Paid;
                            installment.status = (byte)PaymentAttemptStatus.Paid;
                        }
                    }
                }
                else
                {
                    attemptMoset.lr = -1;
                    attemptMoset.message = "retorno de autorização não reconhecido: " + moset.Msgret;
                    attempt.returnMessage = "Retorno de autorização não reconhecido";
                    ccInfo.acquirerReturnCode = attemptMoset.lr.ToString();
                }
            }
            else
            {
                attemptMoset.lr = -1;
                attemptMoset.message = moset.Msgret;
                attempt.returnMessage = "Problemas na transferência das informações";
                ccInfo.acquirerReturnCode = attemptMoset.lr.ToString();
            }

            ccInfo.transactionId = tid;
            ccInfo.acquirerReturnMessage = attempt.returnMessage;
            ////TODO: verificar como obter o campo arp para o visa moset
            //ccInfo.authorizationId = "";

            attempt.lastUpdate = DateTime.Now;
            attempt.TruncateStringFields();
            attemptMoset.TruncateStringFields();
            DataFactory.PaymentAttemptMoset().Update(attemptMoset);
            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.OrderInstallment().Update(orderInstallment);

            pfDetail.Item = ccInfo;
            installment.paymentFormDetail = pfDetail;
            installment.number = (ulong)orderInstallment.installmentNumber;
            installment.date = DateTime.Today;
            installment.dateSpecified = true;
            installment.paymentDateSpecified = (attempt.status == (int)PaymentAttemptStatus.Paid);
            installment.paymentDate = DateTime.Today;

            return installment;
        }
    }
}
