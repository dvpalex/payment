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

public partial class paymentConfirmHtml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string paymentAttemptId = Request["COD_CONTROLE"];
        string numParcela = Request["NUM_PARCELA"];
        if (System.Configuration.ConfigurationManager.AppSettings["urlHandshakeHTML"] != null && paymentAttemptId != null)
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["urlHandshakeHTML"] + "?STS_RECEIVE_TRANS=OK&COD_CONTROLE=" + paymentAttemptId + "&NUM_PARCELA=" + numParcela);
    }
}
