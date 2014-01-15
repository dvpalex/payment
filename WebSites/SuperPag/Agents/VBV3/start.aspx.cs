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
using SuperPag.Agents.VBV3;
using SuperPag.Helper;

public partial class Agents_VBV3_start : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação VISA");
        VBV3 vbv3 = new VBV3((Guid)Session["PaymentAttemptId"]);
        vbv3.Step1();
    }
}
