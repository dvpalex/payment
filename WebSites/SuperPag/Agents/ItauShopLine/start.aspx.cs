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
using SuperPag.Helper;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag;

public partial class Agents_ItauShopLine_popupItau : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido iniciando uma transação ItauShopline");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());
        DPaymentAttemptItauShopline attemptItau = DataFactory.PaymentAttemptItauShopline().Locate(attempt.paymentAttemptId);

        // verifico se o guid passado na querystring já não foi utilizado anteriormente
        if (attemptItau.itauStatus != (byte)PaymentAttemptItauShoplineStatus.Initial)
            GenericHelper.RedirectToErrorPage("Transação executada");

        DPaymentAgentSetupItauShopline dPaymentAgentSetupItauShopline = DataFactory.PaymentAgentSetupItauShopline().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(dPaymentAgentSetupItauShopline, "A loja não está configurada corretamente para esse meio de pagamento");
        
        ClientHttpRequisition post = new ClientHttpRequisition();
        post.Method = "POST";
        post.Url = dPaymentAgentSetupItauShopline.urlItau;
        post.FormName = "ItauShoplineSubmit";
        post.Parameters.Add("DC", attemptItau.dc);

        attemptItau.itauStatus = (byte)PaymentAttemptItauShoplineStatus.Post;
        DataFactory.PaymentAttemptItauShopline().Update(attemptItau);
        
        post.Send();
    }
}
