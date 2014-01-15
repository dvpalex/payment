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
using SuperPag;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;

public partial class Agents_BBPag_aprovada : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido exibindo o resultado de uma transação BBPag");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        DPaymentAttemptBB attemptBBPag = DataFactory.PaymentAttemptBB().Locate(attempt.paymentAttemptId);

        // verifico se o guid passado na querystring é de uma transação concluída com êxito
        if (attempt.status != (int)PaymentAttemptStatus.Paid)
            GenericHelper.RedirectToErrorPage("Transação não concluída");
        
        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId);
    }
}
