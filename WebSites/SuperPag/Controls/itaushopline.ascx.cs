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

public partial class Controls_itaushopline : ControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    public override void ShowControl()
    {
        DPaymentAttemptItauShopline paymentAttemptItau = DataFactory.PaymentAttemptItauShopline().Locate(this.PaymentAttemptId);
        lblCompVend.Text = paymentAttemptItau.agentOrderReference.ToString().PadLeft(8, '0');
        lblNumId.Text = paymentAttemptItau.numId;
        lblMessage.Text = paymentAttemptItau.msgret;
    }
}
