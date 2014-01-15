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
using SuperPag.Agents.VBV3;

namespace SuperPag.Agents.VisaMoset3
{
    class SystemInterface
    {
        public static responseOrdersOrderPaymentInstallment ProcessPayment(DOrder order, DOrderInstallment orderInstallment, DPaymentAttempt attempt, SuperPag.Helper.Xml.Request.genericPaymentFormDetailCreditCardInformation paymentInfo, InstallmentType installmentType)
        {
            Ensure.IsNotNull(order, "O pedido deve ser informado");
            Ensure.IsNotNull(attempt, "A tentativa de pagamento deve ser informada");
            Ensure.IsNotNull(orderInstallment, "A parcela relativa deve ser informada");
            Ensure.IsNotNull(paymentInfo, "Os dados do cartão devem ser informados");
            
            DPaymentAgentSetupVBV agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
            Ensure.IsNotNull(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");

            //Inicializo as classes para retorno
            responseOrdersOrderPaymentInstallment installment = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallment();
            responseOrdersOrderPaymentInstallmentPaymentFormDetail pfDetail = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetail();
            responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation ccInfo = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation();

            DateTime paymentDate = DateTime.MinValue;
            
            string urlComponent = ConfigurationManager.AppSettings["VBV3ComponentUrl"];           
            
            string tid = "";
            DateTime tidDate = DateTime.Now;

            bool autoCapture = (paymentInfo.captureSpecified && paymentInfo.capture == SuperPag.Helper.Xml.Request.capture.auto ? true : false);

            tid = agentsetup.businessNumber.ToString().PadLeft(10, '0').Substring(4, 5);
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
            attemptMoset.merchantId = agentsetup.businessNumber.ToString();
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
            bool result = moset.ProcessPayment(urlComponent, order.orderId, order.storeReferenceOrder, attemptMoset.tid, attempt.price,
                paymentInfo.cardNumber.ToString(), GenericHelper.ParseDate(paymentInfo.expireDate + "-01", "yyyy-MM-dd"), agentsetup.businessNumber.ToString(), "cfg" + agentsetup.paymentAgentSetupId.ToString(), attempt.paymentAttemptId.ToString(), paymentInfo.securityCode.ToString().PadRight(3, Char.Parse("0")));

            if (result)
            {
                attemptMoset.free = moset.Free;
                attemptMoset.lr = moset.Lr;

                // verifico se o TID retornado é mesmo que está gravado no banco de dados
                if (moset.Tid != attemptMoset.tid)
                    Ensure.IsNotNull(null, "TID inconsistente");

                attempt.returnMessage = Moset.GetPaymentProcessResponseDescription(attemptMoset.lr);
                attemptMoset.message = attempt.returnMessage;
                ccInfo.acquirerReturnCode = attemptMoset.lr.ToString();

                if (attemptMoset.lr == 0 || attemptMoset.lr == 11)
                {
                    ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item3; //aprovada e pendente de captura
                    
                    if (autoCapture)
                    {
                        attemptMoset.mosetStatus = (int)PaymentAttemptVisaMosetStatus.Capture;
                        attemptMoset.TruncateStringFields();
                        DataFactory.PaymentAttemptMoset().Update(attemptMoset);

                        VBV3.Messages.VBV3CaptureReturn captureReturn = VBV3.VBV3.Capture(attemptMoset.tid, attempt.paymentAttemptId.ToString(), "cfg" + agentsetup.paymentAgentSetupId.ToString());
                        if (captureReturn != null && captureReturn.tid != null)
                        {                            
                            attemptMoset.capturedTid = captureReturn.tid;
                            attemptMoset.capturedArs = captureReturn.ars;
                            attemptMoset.capturedCod = (int)captureReturn.lr;
                            attemptMoset.capturedCap = captureReturn.cap;
                            attemptMoset.capturedCurrency = Moset.GetCapturedCurrency(captureReturn.cap);
                            attemptMoset.capturedValue = Moset.GetCapturedValue(captureReturn.cap);

                            if (attemptMoset.capturedCod != int.MinValue)
                            {
                                attempt.returnMessage = attemptMoset.capturedArs;
                                attemptMoset.message = attemptMoset.capturedArs;
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
                                attempt.returnMessage = "Retorno da captura não reconhecido";
                                attemptMoset.message = attempt.returnMessage;
                                ccInfo.acquirerReturnCode = attemptMoset.capturedCod.ToString();
                            }
                        }
                        else
                        {
                            attemptMoset.capturedCod = -1;
                            attempt.returnMessage = "Problemas na captura";
                            attemptMoset.message = attempt.returnMessage;
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
                if (moset.Lr == -1)
                {
                    attemptMoset.lr = -1;
                    attemptMoset.message = moset.Msgret;
                    attempt.returnMessage = "Problemas na transferência das informações";
                }
                else
                {
                    attemptMoset.lr = -2;
                    attemptMoset.message = "retorno de autorização não reconhecido: " + moset.Msgret;
                    attempt.returnMessage = "Retorno de autorização não reconhecido";
                }
                
                ccInfo.acquirerReturnCode = attemptMoset.lr.ToString();
            }

            ccInfo.transactionId = tid;
            ccInfo.acquirerReturnMessage = attempt.returnMessage;
            //TODO: verificar como obter o campo arp para o visa moset
            ccInfo.authorizationId = "";

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