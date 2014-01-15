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

public partial class Controls_boletoBB : System.Web.UI.UserControl
{
    public Guid PaymentAttemptId;
    MPaymentAttempt mPaymentAttempt;

    protected void Page_Load(object sender, EventArgs e)
    {
        mPaymentAttempt = (MPaymentAttempt)((MessagePage)this.Page).GetMessage(typeof(MPaymentAttempt));
        PaymentAttemptId = mPaymentAttempt.PaymentAttemptId;

        MPaymentAttemptBoleto mPaymentAttemptBoleto = PaymentAttemptBoleto.Locate(PaymentAttemptId);

        if (mPaymentAttemptBoleto == null)
        {
            lblDataVencimento.Text = "";
            lblOct.Text = "";
            lnkPaid.Enabled = false;
            lnkReenviar.Enabled = false;
            lnkRefazer.Enabled = false;

        }
        else
        {
            lblDataVencimento.Text = mPaymentAttemptBoleto.ExpirationPaymentDate.ToString("dd/MM/yyyy");
            lblOct.Text = mPaymentAttemptBoleto.Oct;

            lnkPaid.Enabled = (mPaymentAttempt.Status == MPaymentAttempt.PaymentAttemptStatus.PayPending);
            lnkReenviar.Enabled = (mPaymentAttempt.Status == MPaymentAttempt.PaymentAttemptStatus.PayPending);
            lnkRefazer.Enabled = (mPaymentAttempt.Status == MPaymentAttempt.PaymentAttemptStatus.PayPending);
        }
    }

    protected void lnkPagar(object sender, EventArgs e)
    {
        mPaymentAttempt.Status = MPaymentAttempt.PaymentAttemptStatus.Paid;
        PaymentAttempt.Update(mPaymentAttempt);
        Order.Update(mPaymentAttempt.Order);

        SuperPag.Handshake.Helper.SendPaymentPost(mPaymentAttempt.PaymentAttemptId);

        ((MessagePage)this.Page).RaiseEvent(typeof(Ev.TransactionDetail.Reload));

    }

    protected void lnkReenvia(object sender, EventArgs e)
    {
        ((MessagePage)this.Page).RaiseEvent(typeof(Ev.TransactionDetail.EnviaBoleto));
    }

    
    protected void lnkRefazerBoleto(object sender, EventArgs e)
    {
        ((MessagePage)this.Page).RaiseEvent(typeof(Ev.TransactionDetail.EditBoleto));
    
    }
}
