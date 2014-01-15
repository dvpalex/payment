using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SuperPag.Data.Messages;
using SuperPag.Helper;
using SuperPag;
using SuperPag.Data;
using SuperPag.Agents.PaymentClientVirtual;
using System.Collections.Specialized;
using SuperPag.Handshake;

public partial class Agents_PaymentClientVirtual_Request2Party : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação Amex");
        
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupPaymentClientVirtual agentsetup = DataFactory.PaymentAgentSetupPaymentClientVirtual().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.PaymentClientVirtual2Party);

        string xml = GenericHelper.GetCreditCardXmlSession(Context);
        Ensure.IsNotNullOrEmptyPage(xml, "Sessão inválida para os dados do cartão de uma transação Amex");
        CreditCardInformation cardinfo = GenericHelper.GetCreditCardXml(xml);
        Ensure.IsNotNullPage(cardinfo, "Dados inválidos do cartão para uma transação Amex");
        
        DPaymentAttemptPaymentClientVirtual dPaymentAttemptPaymentClientVirtual = new DPaymentAttemptPaymentClientVirtual();
        dPaymentAttemptPaymentClientVirtual.paymentAttemptId = attempt.paymentAttemptId;
        dPaymentAttemptPaymentClientVirtual.purchaseAmount = attempt.price;
        dPaymentAttemptPaymentClientVirtual.cardInformation = xml;
        dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.Initial;
        dPaymentAttemptPaymentClientVirtual.agentOrderReference = int.MinValue;
        dPaymentAttemptPaymentClientVirtual.avs = (agentsetup.checkAVS ? "S" : null);
        dPaymentAttemptPaymentClientVirtual.vpc_Version = agentsetup.version;
        if (agentsetup.checkAVS)
        {
            DConsumerAddress consumerAddress = DataFactory.ConsumerAddress().Locate(order.consumerId, (int)AddressTypes.Billing);
            dPaymentAttemptPaymentClientVirtual.vpc_AVS_Street01 = (consumerAddress != null ? consumerAddress.logradouro.Trim() + " " + consumerAddress.address.Trim() + ", " + consumerAddress.addressNumber.Trim(): null);
            dPaymentAttemptPaymentClientVirtual.vpc_AVS_PostCode = (consumerAddress != null ? consumerAddress.cep.Trim() : null);
        }
        dPaymentAttemptPaymentClientVirtual.TruncateStringFields();
        DataFactory.PaymentAttemptPaymentClientVirtual().Insert(dPaymentAttemptPaymentClientVirtual);

        if (attempt.isSimulation)
        {
            dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.End;
            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.Paid;
            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
            Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
        }
        
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
        post.Parameters.Add("vpc_CardNum", cardinfo.Number);
        post.Parameters.Add("vpc_CardExp", cardinfo.ExpirationDate.ToString("yyMM"));
        post.Parameters.Add("vpc_CardSecurityCode", cardinfo.SecurityNumber);
        if (order.installmentQuantity > 1)
        {
            post.Parameters.Add("vpc_NumPayments", order.installmentQuantity.ToString());
            //Enviar tipo de pagamento especifico
            DStorePaymentInstallment installment = DataFactory.StorePaymentInstallment().Locate(order.storeId, attempt.paymentFormId, order.installmentQuantity);
            if (installment.installmentType == (byte)InstallmentType.Emissor)
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

                if (dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode != null && dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode.Equals("0"))
                {
                    if (!agentsetup.checkAVS || (agentsetup.checkAVS && PaymentClientVirtual.ValidaAVS(dPaymentAttemptPaymentClientVirtual.vpc_AVSResultCode, agentsetup.acceptedAVSReturn)))
                    {
                        if (agentsetup.autoCapture)
                        {
                            dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.Capture;
                            dPaymentAttemptPaymentClientVirtual.TruncateStringFields();
                            DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);

                            ServerHttpHtmlRequisition capture = new ServerHttpHtmlRequisition();
                            capture.Method = "POST";
                            capture.Url = "https://vpos.amxvpos.com/vpcdps"; //URL DA AMEX para  Payment Client Virtual 2-Party
                            capture.Parameters.Add("vpc_AccessCode", agentsetup.accessCode);
                            capture.Parameters.Add("vpc_Amount", GenericHelper.ParseString(dPaymentAttemptPaymentClientVirtual.purchaseAmount));
                            capture.Parameters.Add("vpc_Command", "capture");
                            capture.Parameters.Add("vpc_MerchTxnRef", dPaymentAttemptPaymentClientVirtual.agentOrderReference.ToString());
                            capture.Parameters.Add("vpc_Merchant", agentsetup.merchantId);
                            capture.Parameters.Add("vpc_Version", dPaymentAttemptPaymentClientVirtual.vpc_Version);
                            capture.Parameters.Add("vpc_TransNo", dPaymentAttemptPaymentClientVirtual.vpc_TransactionNo.ToString());
                            capture.Parameters.Add("vpc_User", agentsetup.captureUser);
                            capture.Parameters.Add("vpc_Password", agentsetup.capturePassword);

                            if (capture.Send())
                            {
                                NameValueCollection paramcap = HttpUtility.ParseQueryString(capture.Response);
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

                                    if (dPaymentAttemptPaymentClientVirtual.vpc_CapTxnResponseCode != null && dPaymentAttemptPaymentClientVirtual.vpc_CapTxnResponseCode.Equals("0"))
                                    {
                                        attempt.status = (int)PaymentAttemptStatus.Paid;
                                        attempt.lastUpdate = DateTime.Now;
                                        attempt.returnMessage = dPaymentAttemptPaymentClientVirtual.vpc_Message;
                                        attempt.TruncateStringFields();
                                        dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.End;
                                        dPaymentAttemptPaymentClientVirtual.TruncateStringFields();

                                        DataFactory.PaymentAttempt().Update(attempt);
                                        DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);
                                        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
                                        
                                        //TODO:recorrencia amex
                                        RecurrenceProcess.FinishTransaction(attempt);
                                        //Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
                                    }
                                }
                                else
                                {
                                    dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode = "-1";
                                    dPaymentAttemptPaymentClientVirtual.vpc_Message = "retorno da captura não reconhecido: " + post.Response;
                                    attempt.returnMessage = "Retorno da captura não reconhecido";
                                }
                            }
                            else
                            {
                                dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode = "-1";
                                dPaymentAttemptPaymentClientVirtual.vpc_Message = capture.Response;
                                attempt.returnMessage = "Problemas na captura";
                            }
                        }
                        else
                        {
                            attempt.status = (int)PaymentAttemptStatus.Paid;
                            attempt.lastUpdate = DateTime.Now;
                            attempt.returnMessage = dPaymentAttemptPaymentClientVirtual.vpc_Message;
                            attempt.TruncateStringFields();
                            dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.WaitingCapture;
                            dPaymentAttemptPaymentClientVirtual.TruncateStringFields();

                            DataFactory.PaymentAttempt().Update(attempt);
                            DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);
                            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                            Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
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
            }
        }
        else
        {
            dPaymentAttemptPaymentClientVirtual.vpc_TxnResponseCode = "-1";
            dPaymentAttemptPaymentClientVirtual.vpc_Message = post.Response;
            attempt.returnMessage = "Problemas na transferência das informações";
        }

        attempt.status = (int)PaymentAttemptStatus.NotPaid;
        attempt.lastUpdate = DateTime.Now;
        dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.End;
        dPaymentAttemptPaymentClientVirtual.TruncateStringFields();

        DataFactory.PaymentAttempt().Update(attempt);
        DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);
        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

        Response.Redirect("popupclose.aspx?id=" + attempt.paymentAttemptId);
    }
}
