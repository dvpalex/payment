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
using SuperPag.Data.Messages;
using SuperPag.Data;

public partial class Controls_Amex : System.Web.UI.UserControl
{
    public Guid PaymentAttemptId;
    MPaymentAttempt mPaymentAttempt;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        mPaymentAttempt = (MPaymentAttempt)((MessagePage)this.Page).GetMessage(typeof(MPaymentAttempt));
        PaymentAttemptId = mPaymentAttempt.PaymentAttemptId;

        DPaymentAttemptPaymentClientVirtual dPaymentAttemptPaymentClientVirtual = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(PaymentAttemptId);
        lblAutorizacao.Text = (dPaymentAttemptPaymentClientVirtual != null ? (dPaymentAttemptPaymentClientVirtual.vpc_AuthorizeId != int.MinValue ? dPaymentAttemptPaymentClientVirtual.vpc_AuthorizeId.ToString() : "") : "");
        lblNSU.Text = (dPaymentAttemptPaymentClientVirtual != null ? dPaymentAttemptPaymentClientVirtual.vpc_ReceiptNo : "");
    }
}
