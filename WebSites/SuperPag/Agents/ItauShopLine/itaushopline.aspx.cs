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
using SuperPag.Agents.ItauShopLine;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Helper;
using SuperPag;

public partial class Agents_ItauShopLine_startitau : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação ItauShopline");
        
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupItauShopline dPaymentAgentSetupItauShopline = DataFactory.PaymentAgentSetupItauShopline().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(dPaymentAgentSetupItauShopline, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder dOrder = DataFactory.Order().Locate(attempt.orderId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + dOrder.installmentQuantity + "," + (int)PaymentAgents.ItauShopLine);

        DPaymentAttemptItauShopline dPaymentAttemptItauReceived = DataFactory.PaymentAttemptItauShopline().Locate(attempt.paymentAttemptId);
        if (dPaymentAttemptItauReceived != null)
            return;

        DPaymentAttemptItauShopline dPaymentAttemptItau = new DPaymentAttemptItauShopline();
        dPaymentAttemptItau.paymentAttemptId = attempt.paymentAttemptId;
        dPaymentAttemptItau.codEmp = dPaymentAgentSetupItauShopline.businessKey;
        dPaymentAttemptItau.valor = attempt.price.ToString("0.00", GenericHelper.GetNumberFormatBrasil());
        dPaymentAttemptItau.chave = dPaymentAgentSetupItauShopline.criptoKey;
        dPaymentAttemptItau.itauStatus = (byte)PaymentAttemptItauShoplineStatus.Initial;
        DataFactory.PaymentAttemptItauShopline().Insert(dPaymentAttemptItau);

        string convenio = dPaymentAttemptItau.codEmp;
        string key = dPaymentAttemptItau.chave;
        string referenceOrder = dPaymentAttemptItau.agentOrderReference.ToString();
        string value = dPaymentAttemptItau.valor;
        string returnUrl = "/Agents/ItauShopline/confirmacao.aspx?id=" + dPaymentAttemptItau.agentOrderReference;

        string obs = "";
        string sederName = "";
        string clienteCode = "";
        string clienteNumber = "";
        string sederAdress = "";
        string sederDistricit = "";
        string sederPostalCode = "";
        string sederCity = "";
        string sederState = "";
        string expirationPaymentValue = "";

        Crypto cripto = new Crypto(convenio, referenceOrder, value, obs, key, sederName, clienteCode, clienteNumber,
            sederAdress, sederDistricit, sederPostalCode, sederCity, sederState, expirationPaymentValue, returnUrl);

        dPaymentAttemptItau.dc = cripto.CriptStr;
        DataFactory.PaymentAttemptItauShopline().Update(dPaymentAttemptItau);
    }
}
