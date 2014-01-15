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

public partial class Controls_bbpag : ControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    public override void ShowControl()
    {
        DPaymentAttemptBB paymentAttemptBB = DataFactory.PaymentAttemptBB().Locate(this.PaymentAttemptId);
        lblRefTran.Text = paymentAttemptBB.agentOrderReference.ToString();
        lblMessage.Text = paymentAttemptBB.msgret;
    }
}
