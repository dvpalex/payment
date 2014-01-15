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
using SuperPag.Helper;
using SuperPag;

public partial class Agents_Bradesco_start : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNull(Session["PaymentAttemptId"], "Post inválido iniciando uma transação Bradesco");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Session["PaymentAttemptId"].ToString()));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAttemptBradesco attemptBradesco = DataFactory.PaymentAttemptBradesco().Locate(attempt.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        // verifico se o guid passado na querystring já não foi utilizado anteriormente
        if (attemptBradesco.bradescoStatus != (int)PaymentAttemptBradescoStatus.Initial)
            GenericHelper.RedirectToErrorPage("Transação executada");

        DPaymentAgentSetupBradesco agentsetup = DataFactory.PaymentAgentSetupBradesco().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");

        attemptBradesco.bradescoStatus = (int)PaymentAttemptBradescoStatus.Post;

        DataFactory.PaymentAttemptBradesco().Update(attemptBradesco);
        Response.Redirect(String.Format(agentsetup.bradescoUrl, agentsetup.businessNumber, attemptBradesco.numOrder, attempt.paymentAttemptId.ToString()));
    }
}
