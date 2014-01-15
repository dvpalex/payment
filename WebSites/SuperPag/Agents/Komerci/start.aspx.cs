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

public partial class Agents_Komerci_start : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNull(Request["id"], "Post inválido iniciando uma transação Master/Diners");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());
        DPaymentAttemptKomerci attemptKomerci = DataFactory.PaymentAttemptKomerci().Locate(attempt.paymentAttemptId);

        // verifico se o guid passado na querystring já não foi utilizado anteriormente
        if (attemptKomerci.komerciStatus != (byte)PaymentAttemptKomerciStatus.Initial)
            GenericHelper.RedirectToErrorPage("Transação executada");

        DPaymentAgentSetupKomerci agentsetup = DataFactory.PaymentAgentSetupKomerci().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        
        string http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
        string server = Request.ServerVariables["SERVER_NAME"];
        
        ClientHttpRequisition post = new ClientHttpRequisition();
        post.Method = "POST";
        post.FormName = "start";
        post.Url = (agentsetup.checkAVS ? agentsetup.urlKomerciAVS : agentsetup.urlKomerci);
        post.Parameters.Add("TOTAL", GenericHelper.FormatCurrency(attempt.price));
        post.Parameters.Add("TRANSACAO", attemptKomerci.transacao);
        post.Parameters.Add("PARCELAS", (order.installmentQuantity==1?"00":order.installmentQuantity.ToString().PadLeft(2, '0')));
        post.Parameters.Add("FILIACAO", agentsetup.businessNumber.ToString());
        post.Parameters.Add("DISTRIBUIDOR", "");
        post.Parameters.Add("BANDEIRA", attemptKomerci.bandeira);
        post.Parameters.Add("NUMPEDIDO", attemptKomerci.agentOrderReference.ToString());
        post.Parameters.Add("PAX1", "");
        post.Parameters.Add("CODVER", attemptKomerci.codver);
        post.Parameters.Add("URLBACK", String.Format("{0}://{1}/agents/komerci/retorno.aspx", http, server));
        post.Parameters.Add("TARGET", "_self");
        if(agentsetup.checkAVS)
            post.Parameters.Add("AVS", "S");

        attemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciStatus.Post;
        DataFactory.PaymentAttemptKomerci().Update(attemptKomerci);

        post.Send();
    }
}
