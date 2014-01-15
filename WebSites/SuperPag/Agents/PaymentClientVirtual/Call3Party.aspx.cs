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
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag;
using SuperPag.Helper;
using SuperPag.Agents.PaymentClientVirtual;

public partial class Agents_PaymentClientVirtual_Call3Party : System.Web.UI.Page
{
    protected string queryString;

    protected void Page_Init(object sender, EventArgs e)
    {
        ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Master.FindControl("thebody")).Attributes.Add("onmousemove", "javscript: focusPopUpClick();");
        ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Master.FindControl("thebody")).Attributes.Add("onfocus", "focusPopUp();");
    }

    private void FillTableTop(int storeId)
    {
        DSPLegacyStore dSPLegay = DataFactory.SPLegacyStore().Locate(storeId);
        if (Ensure.IsNotNull(dSPLegay) && Ensure.IsNotNull(dSPLegay.ucTableTop))
        {
            plhTableTop.Controls.Add(Page.LoadControl(dSPLegay.ucTableTop));
        }
        else
        {
            plhTableTop.Visible = false;
        }
    }

    protected void Page_Load(object sender, System.EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação Amex");
        
        DStore dStore = GenericHelper.CheckSessionStore(Context);
        FillTableTop(dStore.storeId);

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupPaymentClientVirtual agentsetup = DataFactory.PaymentAgentSetupPaymentClientVirtual().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.PaymentClientVirtual3Party);

        DPaymentAttemptPaymentClientVirtual dPaymentAttemptPaymentClientVirtual = new DPaymentAttemptPaymentClientVirtual();
        dPaymentAttemptPaymentClientVirtual.paymentAttemptId = attempt.paymentAttemptId;
        dPaymentAttemptPaymentClientVirtual.purchaseAmount = attempt.price;
        dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.Initial;
        dPaymentAttemptPaymentClientVirtual.agentOrderReference = int.MinValue;
        dPaymentAttemptPaymentClientVirtual.vpc_Version = agentsetup.version;
        if (agentsetup.checkAVS)
        {
            DConsumerAddress consumerAddress = DataFactory.ConsumerAddress().Locate(order.consumerId, (int)AddressTypes.Billing);
            dPaymentAttemptPaymentClientVirtual.vpc_AVS_Street01 = (consumerAddress != null && consumerAddress.address != null ? consumerAddress.address : "");
            dPaymentAttemptPaymentClientVirtual.vpc_AVS_PostCode = (consumerAddress != null && consumerAddress.cep != null ? consumerAddress.cep : "");
        }
        dPaymentAttemptPaymentClientVirtual.TruncateStringFields();
        DataFactory.PaymentAttemptPaymentClientVirtual().Insert(dPaymentAttemptPaymentClientVirtual);

        //URL DA AMEX para  Payment Client Virtual 3-Party
        queryString = "https://vpos.amxvpos.com/vpcpay";

        System.Collections.SortedList transactionData = new System.Collections.SortedList(new VPCStringComparer());

        transactionData.Add("AgainLink", "");
        transactionData.Add("Title", "Superpag - AMEX");
        transactionData.Add("vpc_AccessCode", agentsetup.accessCode);
        transactionData.Add("vpc_Amount", GenericHelper.ParseString(dPaymentAttemptPaymentClientVirtual.purchaseAmount));
        transactionData.Add("vpc_Command", "pay");
        transactionData.Add("vpc_Locale", "pt");
        //TODO: verificar se o parametro abaixo funciona em producao
        //post.Parameters.Add("vpc_TxSource", "INTERNET");
        transactionData.Add("vpc_MerchTxnRef", dPaymentAttemptPaymentClientVirtual.agentOrderReference.ToString());
        transactionData.Add("vpc_Merchant", agentsetup.merchantId);
        transactionData.Add("vpc_OrderInfo", order.storeReferenceOrder);
        transactionData.Add("vpc_ReturnURL", System.Configuration.ConfigurationManager.AppSettings["PaymentClientVirtualReturnPage"]);
        transactionData.Add("vpc_Version", dPaymentAttemptPaymentClientVirtual.vpc_Version);
        if (order.installmentQuantity > 1)
        {
            transactionData.Add("vpc_NumPayments", order.installmentQuantity.ToString());
            transactionData.Add("vpc_PlanN", "PlanN");
        }
        if (agentsetup.checkAVS)
        {
            transactionData.Add("vpc_AVS_Street01", dPaymentAttemptPaymentClientVirtual.vpc_AVS_Street01);
            transactionData.Add("vpc_AVS_PostCode", dPaymentAttemptPaymentClientVirtual.vpc_AVS_PostCode);
        }

        string rawHashData = agentsetup.secureHashSecret;
        string seperator = "";
        queryString += "?";
        // Loop through all the data in the SortedList transaction data
        foreach (System.Collections.DictionaryEntry item in transactionData) 
        {
            // build the query string, URL Encoding all keys and their values
            queryString += seperator + System.Web.HttpUtility.UrlEncode(item.Key.ToString()) + "=" + System.Web.HttpUtility.UrlEncode(item.Value.ToString());
            seperator = "&";

            rawHashData += item.Value.ToString();
        }
        
        // Create the MD5 signature and add it to the query string
        string signature = PaymentClientVirtual.CreateMD5Signature(rawHashData);
        queryString += "&vpc_SecureHash=" + signature;
     
        dPaymentAttemptPaymentClientVirtual.signatureCreated = signature;

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
        else
        {
            dPaymentAttemptPaymentClientVirtual.paymentClientVirtualiStatus = (int)PaymentAttemptPaymentClientVirtualStatus.Send;
            DataFactory.PaymentAttemptPaymentClientVirtual().Update(dPaymentAttemptPaymentClientVirtual);

            Session["queryString"] = queryString;
        }
    }
}
