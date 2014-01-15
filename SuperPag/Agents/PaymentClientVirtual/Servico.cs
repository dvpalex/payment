using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using SuperPag.Data.Messages;
using SuperPag.Helper;
using SuperPag.Data;
using SuperPag.Helper.Xml.Response;
using SuperPag.Helper.Xml.Request;
using SuperPag;

namespace SuperPag.Agents.PaymentClientVirtual
{
    class SystemInterface
    {
        public static responseOrdersOrderPaymentInstallment ProcessPayment(DOrder order, DOrderInstallment orderInstallment, DPaymentAttempt attempt, SuperPag.Helper.Xml.Request.genericPaymentFormDetailCreditCardInformation paymentInfo, InstallmentType installmentType)
        {
            Ensure.IsNotNull(order, "O pedido deve ser informado");
            Ensure.IsNotNull(attempt, "A tentativa de pagamento deve ser informada");
            Ensure.IsNotNull(orderInstallment, "A parcela relativa deve ser informada");
            Ensure.IsNotNull(paymentInfo, "Os dados do cartão devem ser informados");

            DPaymentAgentSetupPaymentClientVirtual agentsetup = DataFactory.PaymentAgentSetupPaymentClientVirtual().Locate(attempt.paymentAgentSetupId);
            Ensure.IsNotNull(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");

            //Inicializo as classes para retorno
            responseOrdersOrderPaymentInstallment installment = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallment();
            responseOrdersOrderPaymentInstallmentPaymentFormDetail pfDetail = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetail();
            responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation ccInfo = new SuperPag.Helper.Xml.Response.responseOrdersOrderPaymentInstallmentPaymentFormDetailCreditCardInformation();

            //TODO: Alteração marcos
            //bool autoCapture = (paymentInfo.captureSpecified && paymentInfo.capture == SuperPag.Helper.Xml.Request.capture.auto ? true : false);
            bool autoCapture = (paymentInfo.captureSpecified ? (paymentInfo.capture == SuperPag.Helper.Xml.Request.capture.auto ? true : false) : true);

            DPaymentAttemptPaymentClientVirtual dPaymentAttemptPaymentClientVirtual = new DPaymentAttemptPaymentClientVirtual();
            dPaymentAttemptPaymentClientVirtual.paymentAttemptId = attempt.paymentAttemptId;
            dPaymentAttemptPaymentClientVirtual.purchaseAmount = attempt.price;
            dPaymentAttemptPaymentClientVirtual.cardInformation = GenericHelper.CreateCreditCardXml(paymentInfo.cardHolder, paymentInfo.cardNumber.ToString(), paymentInfo.securityCode.ToString(), GenericHelper.ParseDate(paymentInfo.expireDate + "-01", "yyyy-MM-dd"));
            dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.Initial;
            dPaymentAttemptPaymentClientVirtual.agentOrderReference = int.MinValue;
            dPaymentAttemptPaymentClientVirtual.avs = (agentsetup.checkAVS ? "S" : null);
            dPaymentAttemptPaymentClientVirtual.vpc_Version = agentsetup.version;
            if (agentsetup.checkAVS)
            {
                DConsumerAddress consumerAddress = DataFactory.ConsumerAddress().Locate(order.consumerId, (int)AddressTypes.Billing);
                dPaymentAttemptPaymentClientVirtual.vpc_AVS_Street01 = (consumerAddress != null ? consumerAddress.logradouro.Trim() + " " + consumerAddress.address.Trim() + ", " + consumerAddress.addressNumber.Trim() : null);
                dPaymentAttemptPaymentClientVirtual.vpc_AVS_PostCode = (consumerAddress != null ? consumerAddress.cep.Trim() : null);
            }
            dPaymentAttemptPaymentClientVirtual.TruncateStringFields();
            DataFactory.PaymentAttemptPaymentClientVirtual().Insert(dPaymentAttemptPaymentClientVirtual);

            ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
            post.Method = "POST";
            post.Url = "https://vpos.amxvpos.com/vpcdps"; //URL DA AMEX para  Payment Client Virtual 2-Party
            post.Parameters.Add("vpc_AccessCode", agentsetup.accessCode);
            post.Parameters.Add("vpc_Amount", GenericHelper.ParseString(dPaymentAttemptPaymentClientVirtual.purchaseAmount));
            post.Parameters.Add("vpc_Command", "pay");
            post.Parameters.Add("vpc_Locale", "pt");
            //TODO: verificar se o parametro abaixo funciona em producao
            //post.Parameters.Add("vpc_TxSource", "INTERNET");
            post.Parameters.Add("vpc_MerchTxnRef", dPaymentAttemptPaymentClientVirtual.agentOrderReference.ToString());
            post.Parameters.Add("vpc_Merchant", agentsetup.merchantId);
            post.Parameters.Add("vpc_OrderInfo", order.storeReferenceOrder);
            post.Parameters.Add("vpc_Version", dPaymentAttemptPaymentClientVirtual.vpc_Version);
            post.Parameters.Add("vpc_CardNum", paymentInfo.cardNumber.ToString());
            post.Parameters.Add("vpc_CardExp", GenericHelper.ParseDate(paymentInfo.expireDate + "-01", "yyyy-MM-dd").ToString("yyMM"));
            post.Parameters.Add("vpc_CardSecurityCode", paymentInfo.securityCode.ToString());
            if (order.installmentQuantity > 1 && attempt.installmentNumber == int.MinValue)
            {
                post.Parameters.Add("vpc_NumPayments", order.installmentQuantity.ToString());
                //Enviar tipo de pagamento especifico
                if (installmentType == InstallmentType.Emissor)
                    post.Parameters.Add("vpc_PlanAmex", "PlanAmex");
                else
                    post.Parameters.Add("vpc_PlanN", "PlanN");
            }
            if (agentsetup.checkAVS)
            {
                post.Parameters.Add("vpc_AVS_Street01", dPaymentAttemptPaymentClientVirtual.vpc_AVS_Street01);
                post.Parameters.Add("vpc_AVS_PostCode", dPaymentAttemptPaymentClientVirtual.vpc_AVS_PostCode);
            }

            dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.Send;
            DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);

            dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.End;
            attempt.status = (int)PaymentAttemptStatus.NotPaid;
            orderInstallment.status = (int)OrderInstallmentStatus.NotPaid;
            installment.status = (byte)PaymentAttemptStatus.NotPaid;
            ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item2; //nao aprovada
            
            if (post.Send())
            {
                NameValueCollection param = HttpUtility.ParseQueryString(post.Response);
                if (param != null && param["vpc_TxnResponseCode"] != null)
                {
                    dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode = param["vpc_TxnResponseCode"];
                    dPaymentAttemptPaymentClientVirtual.vpc_Card = param["vpc_Card"];
                    dPaymentAttemptPaymentClientVirtual.vpc_ReceiptNo = param["vpc_ReceiptNo"];
                    dPaymentAttemptPaymentClientVirtual.vpc_AcqResponseCode = param["vpc_AcqResponseCode"];
                    dPaymentAttemptPaymentClientVirtual.vpc_Message = param["vpc_Message"];
                    dPaymentAttemptPaymentClientVirtual.vpc_AVSResultCode = param["vpc_AVSResultCode"];
                    dPaymentAttemptPaymentClientVirtual.vpc_AcqAVSRespCode = param["vpc_AcqAVSRespCode"];
                    dPaymentAttemptPaymentClientVirtual.vpc_CSCResultCode = param["vpc_CSCResultCode"];
                    dPaymentAttemptPaymentClientVirtual.vpc_AcqCSCRespCode = param["vpc_AcqCSCRespCode"];
                    if (param["vpc_BatchNo"] != null && param["vpc_BatchNo"] != string.Empty)
                        dPaymentAttemptPaymentClientVirtual.vpc_BatchNo = int.Parse(param["vpc_BatchNo"]);
                    if (param["vpc_AuthorizeId"] != null && param["vpc_AuthorizeId"] != string.Empty)
                        dPaymentAttemptPaymentClientVirtual.vpc_AuthorizeId = int.Parse(param["vpc_AuthorizeId"]);
                    if (param["vpc_TransactionNo"] != null && param["vpc_TransactionNo"] != string.Empty)
                        dPaymentAttemptPaymentClientVirtual.vpc_TransactionNo = int.Parse(param["vpc_TransactionNo"]);

                    attempt.returnMessage = PaymentClientVirtual.getResponseDescription(dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode);

                    ccInfo.acquirerReturnCode = dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode;
                    ccInfo.authorizationId = (param["vpc_AuthorizeId"] == null ? "" : param["vpc_AuthorizeId"]);
                    ccInfo.transactionId = (param["vpc_TransactionNo"] == null ? "" : param["vpc_TransactionNo"]);

                    if (dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode != null && dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode.Equals("0"))
                    {
                        ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item3; //aprovada e pendente de captura
                        
                        if (!agentsetup.checkAVS || (agentsetup.checkAVS && PaymentClientVirtual.ValidaAVS(dPaymentAttemptPaymentClientVirtual.vpc_AVSResultCode, agentsetup.acceptedAVSReturn)))
                        {
                            if (autoCapture)
                            {
                                dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.Capture;
                                dPaymentAttemptPaymentClientVirtual.TruncateStringFields();
                                DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);

                                ServerHttpHtmlRequisition cap = new ServerHttpHtmlRequisition();
                                cap.Method = "POST";
                                cap.Url = "https://vpos.amxvpos.com/vpcdps"; //URL DA AMEX para  Payment Client Virtual 2-Party
                                cap.Parameters.Add("vpc_AccessCode", agentsetup.accessCode);
                                cap.Parameters.Add("vpc_Amount", GenericHelper.ParseString(dPaymentAttemptPaymentClientVirtual.purchaseAmount));
                                cap.Parameters.Add("vpc_Command", "capture");
                                cap.Parameters.Add("vpc_MerchTxnRef", dPaymentAttemptPaymentClientVirtual.agentOrderReference.ToString());
                                cap.Parameters.Add("vpc_Merchant", agentsetup.merchantId);
                                cap.Parameters.Add("vpc_Version", dPaymentAttemptPaymentClientVirtual.vpc_Version);
                                cap.Parameters.Add("vpc_TransNo", dPaymentAttemptPaymentClientVirtual.vpc_TransactionNo.ToString());
                                cap.Parameters.Add("vpc_User", agentsetup.captureUser);
                                cap.Parameters.Add("vpc_Password", agentsetup.capturePassword);

                                if (cap.Send())
                                {
                                    NameValueCollection paramcap = HttpUtility.ParseQueryString(cap.Response);
                                    if (paramcap != null && paramcap["vpc_TxnResponseCode"] != null)
                                    {
                                        dPaymentAttemptPaymentClientVirtual.vpc_CapTxnResponseCode = paramcap["vpc_TxnResponseCode"];
                                        dPaymentAttemptPaymentClientVirtual.vpc_CaptureMessage = paramcap["vpc_Message"];
                                        dPaymentAttemptPaymentClientVirtual.vpc_ShopTransactionNo = paramcap["vpc_ShopTransactionNo"];
                                        dPaymentAttemptPaymentClientVirtual.vpc_AuthorisedAmount = paramcap["vpc_AuthorisedAmount"];
                                        dPaymentAttemptPaymentClientVirtual.vpc_CapturedAmount = paramcap["vpc_CapturedAmount"];
                                        dPaymentAttemptPaymentClientVirtual.vpc_TicketNumber = paramcap["vpc_TicketNumber"];
                                        if (paramcap["vpc_TransactionNo"] != null && paramcap["vpc_TransactionNo"] != string.Empty)
                                            dPaymentAttemptPaymentClientVirtual.vpc_CapTransactionNo = int.Parse(paramcap["vpc_TransactionNo"]);

                                        attempt.returnMessage = PaymentClientVirtual.getResponseDescription(dPaymentAttemptPaymentClientVirtual.vpc_CapTxnResponseCode);

                                        ccInfo.acquirerReturnCode = dPaymentAttemptPaymentClientVirtual.vpc_CapTxnResponseCode;

                                        if (dPaymentAttemptPaymentClientVirtual.vpc_CapTxnResponseCode != null && dPaymentAttemptPaymentClientVirtual.vpc_CapTxnResponseCode.Equals("0"))
                                        {
                                            dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.End;
                                            attempt.returnMessage = dPaymentAttemptPaymentClientVirtual.vpc_Message;
                                            attempt.status = (int)PaymentAttemptStatus.Paid;
                                            orderInstallment.status = (int)OrderInstallmentStatus.Paid;
                                            installment.status = (byte)PaymentAttemptStatus.Paid;
                                            ccInfo.returnCode = SuperPag.Helper.Xml.Response.returnCode.Item1; //aprovada
                                        }
                                    }
                                    else
                                    {
                                        dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode = "-1";
                                        dPaymentAttemptPaymentClientVirtual.vpc_Message = "retorno da captura não reconhecido: " + post.Response;
                                        attempt.returnMessage = "Retorno da captura não reconhecido";
                                        ccInfo.acquirerReturnCode = dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode;
                                    }
                                }
                                else
                                {
                                    dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode = "-1";
                                    dPaymentAttemptPaymentClientVirtual.vpc_Message = cap.Response;
                                    attempt.returnMessage = "Problemas na captura";
                                    ccInfo.acquirerReturnCode = dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode;
                                }
                            }
                            else
                            {
                                dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.WaitingCapture;
                                attempt.returnMessage = dPaymentAttemptPaymentClientVirtual.vpc_Message;
                                attempt.status = (int)PaymentAttemptStatus.Paid;
                                orderInstallment.status = (int)OrderInstallmentStatus.Paid;
                                installment.status = (byte)PaymentAttemptStatus.Paid;
                            }
                        }
                        else
                        {
                            attempt.returnMessage = "O AVS não foi validado";
                        }
                    }
                }
                else
                {
                    dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode = "-1";
                    dPaymentAttemptPaymentClientVirtual.vpc_Message = "retorno de autorização não reconhecido: " + post.Response;
                    attempt.returnMessage = "Retorno de autorização não reconhecido";
                    ccInfo.acquirerReturnCode = dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode;
                }
            }
            else
            {
                dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode = "-1";
                dPaymentAttemptPaymentClientVirtual.vpc_Message = post.Response;
                attempt.returnMessage = "Problemas na transferência das informações";
                ccInfo.acquirerReturnCode = dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode;
            }

            ccInfo.acquirerReturnMessage = attempt.returnMessage;
            
            attempt.lastUpdate = DateTime.Now;
            attempt.TruncateStringFields();
            dPaymentAttemptPaymentClientVirtual.TruncateStringFields();
            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);
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
