using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using SuperPag.Helper;
using SuperPag.Helper.ParseHTML;
using SuperPag.Data.Messages;
using SuperPag.Data;
using System.Xml;
using System.Xml.Serialization;
using SuperPag.Agents.KomerciWS.Messages;
using System.IO;
using Resp = SuperPag.Helper.Xml.Response;
using SuperPag.Helper.Xml.Request;
using SuperPag.Helper.Xml;
using SuperPag.br.com.redecard.ecommerce;

namespace SuperPag.Agents.KomerciWS
{
    public class KomerciWS
    {
        public static Resp.responseOrdersOrderPaymentInstallment ProcessPayment(DOrder order, DOrderInstallment orderInstallment, DPaymentAttempt attempt, SuperPag.Helper.Xml.Request.genericPaymentFormDetailCreditCardInformation paymentInfo, InstallmentType installmentType)
        {
            Ensure.IsNotNull(order, "O pedido deve ser informado");
            Ensure.IsNotNull(attempt, "A tentativa de pagamento deve ser informada");
            Ensure.IsNotNull(orderInstallment, "A parcela relativa deve ser informada");
            Ensure.IsNotNull(paymentInfo, "Os dados do cartão devem ser informados");
            
            DPaymentAgentSetupKomerci agentSetup = DataFactory.PaymentAgentSetupKomerci().Locate(attempt.paymentAgentSetupId);
            Ensure.IsNotNull(agentSetup, "A loja não está configurada corretamente para esse meio de pagamento");
            
            //Inicializo as classes para retorno
            Resp.responseOrdersOrderPaymentInstallment installment = new Resp.responseOrdersOrderPaymentInstallment();
            Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail pfDetail = new Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetail();
            Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation ccInfo = new Resp.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation();
            
            DPaymentAttemptKomerciWS attemptKomerci = new DPaymentAttemptKomerciWS();
            attemptKomerci.paymentAttemptId = attempt.paymentAttemptId;
            attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.Initial;
            if (order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue)
                attemptKomerci.transacao = "04";
            else if (installmentType == InstallmentType.Emissor)
                attemptKomerci.transacao = "06";
            else
                attemptKomerci.transacao = "08";
            attemptKomerci.cardInformation = GenericHelper.CreateCreditCardXml(paymentInfo.cardHolder, paymentInfo.cardNumber.ToString(), paymentInfo.securityCode.ToString(), GenericHelper.ParseDate(paymentInfo.expireDate + "-01", "yyyy-MM-dd"));
            attemptKomerci.autoCapture = (paymentInfo.captureSpecified ? (paymentInfo.capture == capture.auto ? "S" : "N") : "S");
            DataFactory.PaymentAttemptKomerciWS().Insert(attemptKomerci);

            komerci_capture komerciWS = new komerci_capture();
#if HOMOLOG
            string responseGetAuthorized = komerciWS.GetAuthorizedTst(
                    GenericHelper.FormatCurrency(attempt.price),
                    attemptKomerci.transacao,
                    order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue ? "00" : order.installmentQuantity.ToString(),
                    agentSetup.businessNumber.ToString(),
                    order.storeReferenceOrder, //TODO: Verificar se poe o storeReferenceOrder ou nao
                    paymentInfo.cardNumber.ToString(),
                    paymentInfo.securityCode.ToString(),
                    paymentInfo.expireDate.Substring(5, 2),
                    paymentInfo.expireDate.Substring(2, 2),
                    paymentInfo.cardHolder,
                    null, null, null, null, null, null, null, null, null, null, null, null, null,
                    attemptKomerci.autoCapture).InnerXml;
#else
            string responseGetAuthorized = komerciWS.GetAuthorized(
                    GenericHelper.FormatCurrency(attempt.price),
                    attemptKomerci.transacao,
                    order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue ? "00" : order.installmentQuantity.ToString(),
                    agentSetup.businessNumber.ToString(),
                    order.storeReferenceOrder,
                    paymentInfo.cardNumber.ToString(),
                    paymentInfo.securityCode.ToString(),
                    paymentInfo.expireDate.Substring(5, 2), //03
                    paymentInfo.expireDate.Substring(2, 2), //83
                    paymentInfo.cardHolder,
                    null, null, null, null, null, null, null, null, null, null, null, null, null,
                    attemptKomerci.autoCapture,
                    DateTime.Now.ToString("yyyyMMdd")).InnerXml;
#endif

            responseGetAuthorized = "<AUTHORIZATION>" + responseGetAuthorized + "</AUTHORIZATION>";

            AUTHORIZATION responseAUTHORIZATION;
            string msgerror = "";
            if ((responseAUTHORIZATION = (AUTHORIZATION)XmlHelper.GetClass(responseGetAuthorized, typeof(AUTHORIZATION), out msgerror)) == null)
                Ensure.IsNotNull(null, "Komerci return is not recognized as a XML");

            attemptKomerci.data = responseAUTHORIZATION.DATA;
            attemptKomerci.codret = responseAUTHORIZATION.CODRET;
            attemptKomerci.msgret = HttpUtility.UrlDecode(responseAUTHORIZATION.MSGRET, Encoding.GetEncoding("iso-8859-1"));
            attemptKomerci.capcodret = responseAUTHORIZATION.CONFCODRET;
            attemptKomerci.capmsgret = HttpUtility.UrlDecode(responseAUTHORIZATION.CONFMSGRET, Encoding.GetEncoding("iso-8859-1"));
            attemptKomerci.numautent = responseAUTHORIZATION.NUMAUTENT;
            attemptKomerci.numautor = responseAUTHORIZATION.NUMAUTOR;
            attemptKomerci.numcv = responseAUTHORIZATION.NUMCV;
            attemptKomerci.numsqn = responseAUTHORIZATION.NUMSQN;
            attemptKomerci.TruncateStringFields();

            ccInfo.acquirerReturnCode = attemptKomerci.codret;
            ccInfo.acquirerReturnMessage = attemptKomerci.msgret;
            
            int authCodRet;
            if (!int.TryParse(responseAUTHORIZATION.CODRET, out authCodRet))
                Ensure.IsNotNull(null, "Komerci CODRET value not recognized as an int");

            if (authCodRet >= 0 && authCodRet < 50)
            {
                ccInfo.transactionId = attemptKomerci.numcv;
                ccInfo.authorizationId = attemptKomerci.numautor;

                if (attemptKomerci.autoCapture.Equals("S"))
                {
                    attemptKomerci.komerciStatus = String.IsNullOrEmpty(responseAUTHORIZATION.CONFCODRET) ?
                        (byte)PaymentAttemptKomerciWSStatus.WaitingCapture : 
                        (responseAUTHORIZATION.CONFCODRET.Equals("0") || responseAUTHORIZATION.CONFCODRET.Equals("1") ?
                            attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.Captured :
                            attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.WaitingCapture);

                    if (attemptKomerci.komerciStatus == (byte)PaymentAttemptKomerciWSStatus.Captured)
                    {
                        attempt.returnMessage = attemptKomerci.capmsgret;
                        attempt.status = (int)PaymentAttemptStatus.Paid;
                        orderInstallment.status = (int)OrderInstallmentStatus.Paid;
                        installment.status = (byte)PaymentAttemptStatus.Paid;

                        ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item1; //aprovada
                        ccInfo.acquirerReturnCode = attemptKomerci.capcodret;
                        ccInfo.acquirerReturnMessage = attemptKomerci.capmsgret;
                    }
                    else
                    {
                        //Como nao deu certo a tentativa de captura automatica
                        // tentar chamar a captura manualmente
#if HOMOLOG
                        string responseConfirmTxn = komerciWS.ConfirmTxnTst(
                                attemptKomerci.data,
                                attemptKomerci.numsqn,
                                attemptKomerci.numcv,
                                attemptKomerci.numautor,
                                order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue ? "00" : order.installmentQuantity.ToString(),
                                attemptKomerci.transacao,
                                GenericHelper.FormatCurrency(attempt.price),
                                agentSetup.businessNumber.ToString(),
                                null,
                                order.storeReferenceOrder,
                                null, null, null, null,
                                attemptKomerci.paymentAttemptId.ToString(),
                                null, null, null).InnerXml;
#else
                        string responseConfirmTxn = komerciWS.ConfirmTxn(
                                    attemptKomerci.data,
                                    attemptKomerci.numsqn,
                                    attemptKomerci.numcv,
                                    attemptKomerci.numautor,
                                    order.installmentQuantity == 1 || attempt.installmentNumber != int.MinValue ? "00" : order.installmentQuantity.ToString(),
                                    attemptKomerci.transacao,
                                    GenericHelper.FormatCurrency(attempt.price),
                                    agentSetup.businessNumber.ToString(),
                                    null,
                                    order.storeReferenceOrder,
                                    null, null, null, null,
                                    attemptKomerci.paymentAttemptId.ToString(),
                                    null, null, null,
                                    DateTime.Now.ToString("yyyyMMdd")).InnerXml;
#endif

                        responseConfirmTxn = "<CONFIRMATION>" + responseConfirmTxn + "</CONFIRMATION>";

                        CONFIRMATION responseCONFIRMATION;
                        if ((responseCONFIRMATION = (CONFIRMATION)XmlHelper.GetClass(responseConfirmTxn, typeof(CONFIRMATION), out msgerror)) == null)
                            Ensure.IsNotNull(null, "Komerci confirmation return is not recognized as a XML");

                        attemptKomerci.capcodret = responseCONFIRMATION.CODRET;
                        attemptKomerci.capmsgret = HttpUtility.UrlDecode(responseCONFIRMATION.MSGRET, Encoding.GetEncoding("iso-8859-1"));

                        int confCodRet;
                        if (!int.TryParse(responseCONFIRMATION.CODRET, out confCodRet)) //TODO: Quando a REDECARD retornar tags em branco, o que fazer?
                            Ensure.IsNotNull(null, "Komerci confirmation CODRET value not recognized as an int");

                        if (responseCONFIRMATION.CODRET.Equals("0") || responseCONFIRMATION.CODRET.Equals("1"))
                        {
                            attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.Captured;
                            attempt.returnMessage = attemptKomerci.capmsgret;
                            attempt.status = (int)PaymentAttemptStatus.Paid;
                            orderInstallment.status = (int)OrderInstallmentStatus.Paid;
                            installment.status = (byte)PaymentAttemptStatus.Paid;

                            ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item1; //aprovada
                            ccInfo.acquirerReturnCode = attemptKomerci.capcodret;
                            ccInfo.acquirerReturnMessage = attemptKomerci.capmsgret;
                        }
                        else
                        {
                            attempt.returnMessage = attemptKomerci.capmsgret;
                            attempt.status = (int)PaymentAttemptStatus.NotPaid;
                            orderInstallment.status = (int)OrderInstallmentStatus.NotPaid;
                            installment.status = (byte)PaymentAttemptStatus.NotPaid;
                            ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item3; //aprovada e pendente de captura
                        }
                    }

                }
                else
                {
                    attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciWSStatus.WaitingCapture;
                    attempt.returnMessage = attemptKomerci.msgret;
                    attempt.status = (int)PaymentAttemptStatus.Paid;
                    orderInstallment.status = (int)OrderInstallmentStatus.Paid;
                    installment.status = (byte)PaymentAttemptStatus.Paid;
                    ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item3; //aprovada e pendente de captura
                }
            }
            else
            {
                attempt.returnMessage = attemptKomerci.msgret;
                attempt.status = (int)PaymentAttemptStatus.NotPaid;
                orderInstallment.status = (int)OrderInstallmentStatus.NotPaid;
                installment.status = (byte)PaymentAttemptStatus.NotPaid;
                ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item2; //nao aprovada
            }

            attempt.lastUpdate = DateTime.Now;
            attemptKomerci.TruncateStringFields();
            attempt.TruncateStringFields();
            DataFactory.PaymentAttemptKomerciWS().Update(attemptKomerci);
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
