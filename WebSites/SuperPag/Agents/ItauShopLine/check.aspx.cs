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

public partial class Agents_ItauShopLine_check : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido exibindo o resultado de uma transação ItauShopline");
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));

        if (attempt.status == (int)PaymentAttemptStatus.Paid)
            Response.Redirect("~/finalization.aspx?id=" + Request["id"]);
        else
            Response.Redirect("~/Agents/ItauShopline/naoaprovada.aspx?id=" + Request["id"]);
    }
}
