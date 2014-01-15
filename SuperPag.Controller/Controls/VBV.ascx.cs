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
using SuperPag.Agents.VBV;

public partial class Controls_VBV : System.Web.UI.UserControl
{
    public Guid PaymentAttemptId;
    MPaymentAttempt mPaymentAttempt;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.lnkCapturar.Attributes.Add("onclick", "return confirm('Voc� tem certeza que deseja capturar manualmente esta transa��o?');");
        
        mPaymentAttempt = (MPaymentAttempt)((MessagePage)this.Page).GetMessage(typeof(MPaymentAttempt));
        PaymentAttemptId = mPaymentAttempt.PaymentAttemptId;

        MPaymentAttemptVBV mPaymentAttemptVBV = PaymentAttemptVBV.Locate(PaymentAttemptId);
        lblTID.Text = mPaymentAttemptVBV.Tid;
        if (mPaymentAttemptVBV.Arp == int.MinValue)
            lblAuthentic.Text = "";
        else
            lblAuthentic.Text = mPaymentAttemptVBV.Arp.ToString();

        switch (mPaymentAttemptVBV.VbvStatus)
        {
            case 1:
                lblVbvStatus.Text = "Comunica��o n�o iniciada.";
                break;
            case 2:
                lblVbvStatus.Text = "Comunica��o iniciada, mas n�o houve retorno.";
                break;
            case 3:
                lblVbvStatus.Text = "Transa��o aprovada, aguardando captura manual.";
                lnkCapturar.Visible = true;
                break;
            case 4:
                lblVbvStatus.Text = "Comunica��o de captura iniciada, mas n�o houve retorno.";
                break;
            case 5:
                lblVbvStatus.Text = "Etapas realizadas com sucesso.";
                break;
            case 6:
                lblVbvStatus.Text = "Prazo de captura expirado. Por favor, contate a operadora caso haja a necessidade de capturar essa transa��o.";
                break;
        }
    }

    protected void lnkCaptura_Click(object sender, EventArgs e)
    {
        ((MessagePage)this.Page).RaiseEvent(typeof(Ev.TransactionDetail.CapturaVBV));
    }
}
