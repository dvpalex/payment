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
using SuperPag.Framework.Web.WebControls;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business;
using SuperPag.Business.Messages;
using Ev = Controller.Lib.Views.Ev;

public partial class Controls_BBPag : System.Web.UI.UserControl
{
    public Guid PaymentAttemptId;
    MPaymentAttempt mPaymentAttempt;

    protected void Page_Load(object sender, EventArgs e)
    {
        mPaymentAttempt = (MPaymentAttempt)((MessagePage)this.Page).GetMessage(typeof(MPaymentAttempt));
        PaymentAttemptId = mPaymentAttempt.PaymentAttemptId;

        MPaymentAttemptBB mPaymentAttemptBB = PaymentAttemptBBPag.Locate(PaymentAttemptId);
        if (mPaymentAttemptBB == null)
            return;

        lblAgentOrderReference.Text = mPaymentAttemptBB.AgentOrderReference.ToString();
        if (mPaymentAttemptBB.IdConvenio <= 0)
            lblConvenio.Text = "";
        else
            lblConvenio.Text = mPaymentAttemptBB.IdConvenio.ToString();
    }
}
