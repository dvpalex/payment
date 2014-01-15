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

public partial class Controls_komerci : ControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    public override void ShowControl()
    {
        DPaymentAttemptKomerci paymentAgentSetupKomerci = DataFactory.PaymentAttemptKomerci().Locate(this.PaymentAttemptId);
        lblNumaut.Text = paymentAgentSetupKomerci.numautent;
        lblMessage.Text = paymentAgentSetupKomerci.msgret;
    }
}
