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
using SuperPag.Helper;
using SuperPag.Data;

public partial class Agents_PaymentClientVirtual_check : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido exibindo o resultado de uma transação Amex");
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());

        if (attempt.status == (int)PaymentAttemptStatus.Paid)
            Response.Redirect("~/finalization.aspx?id=" + Request["id"]);
        else
            Response.Redirect("~/Agents/PaymentClientVirtual/popupclose.aspx?id=" + Request["id"]);
    }
}
