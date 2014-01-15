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

public partial class Agents_PaymentClientVirtual_popup : System.Web.UI.Page
{
    protected string queryString;

    protected void Page_Load(object sender, EventArgs e)
    {
        queryString = Session["queryString"].ToString();
        Session.Remove("queryString");
    }
}
