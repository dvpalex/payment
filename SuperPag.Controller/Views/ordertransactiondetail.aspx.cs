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
using SuperPag;
using System.Text;


public partial class Views_ordertransactiondetail : SuperPag.Framework.Web.WebControls.MessagePage 
{
    MPaymentAttempt mPaymentAttempt = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblNomeUsuario.Text = User.Identity.Name;

        mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
        lnkPostConfimacao.Enabled = (mPaymentAttempt.Status != MPaymentAttempt.PaymentAttemptStatus.Pending);
        lnkPostPagamento.Enabled = (mPaymentAttempt.Status == MPaymentAttempt.PaymentAttemptStatus.Paid);


        switch ((PaymentAgents)mPaymentAttempt.PaymentForm.PaymentAgentId)
        {
            case PaymentAgents.VBV:
            case PaymentAgents.VBVInBox:
                phTransactionDetail.Controls.Add(LoadControl("../Controls/VBV.ascx"));
                break;
            case PaymentAgents.Komerci:
            case PaymentAgents.KomerciInBox:
                phTransactionDetail.Controls.Add(LoadControl("../Controls/Komerci.ascx"));
                break;
            case PaymentAgents.Boleto:
                phTransactionDetail.Controls.Add(LoadControl("../Controls/boletoBB.ascx"));
                break;
            case PaymentAgents.ItauShopLine:
                phTransactionDetail.Controls.Add(LoadControl("../Controls/ItauShopLine.ascx"));
                break;
            case PaymentAgents.BBPag:
                phTransactionDetail.Controls.Add(LoadControl("../Controls/BBPag.ascx"));
                break;
            case PaymentAgents.PaymentClientVirtual2Party:
            case PaymentAgents.PaymentClientVirtual3Party:
                phTransactionDetail.Controls.Add(LoadControl("../Controls/Amex.ascx"));
                break;
            case PaymentAgents.VisaMoset:
                phTransactionDetail.Controls.Add(LoadControl("../Controls/VisaMoset.ascx"));
                break;
            case PaymentAgents.DepositoIdentificado:
                phTransactionDetail.Controls.Add(LoadControl("../Controls/DepId.ascx"));
                break;
        }
    }       

    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.OrderTransactionDetail.GoBack));
    }

    protected void lnkPostConfimacao_Click(object sender, EventArgs e)
    {
        SuperPag.Handshake.Helper.SendFinalizationPost(mPaymentAttempt.PaymentAttemptId);
        ClientScript.RegisterStartupScript(this.GetType(), "post", "<script>alert('Post enviado!');</script>");
    }

    protected void lnkPostPagamento_Click(object sender, EventArgs e)
    {
        SuperPag.Handshake.Helper.SendPaymentPost(mPaymentAttempt.PaymentAttemptId);
        
        ClientScript.RegisterStartupScript(this.GetType(), "post", "<script>alert('Post enviado!');</script>");
    }
}
