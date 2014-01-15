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


public partial class Views_transactiondetail : SuperPag.Framework.Web.WebControls.MessagePage 
{
    MPaymentAttempt mPaymentAttempt = null;

    protected void Page_Load(object sender, EventArgs e)
    {        
        lblNomeUsuario.Text = User.Identity.Name;

        mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
        lnkPostConfimacao.Enabled = (mPaymentAttempt.Status != MPaymentAttempt.PaymentAttemptStatus.Pending);
        lnkPostPagamento.Enabled = (mPaymentAttempt.Status == MPaymentAttempt.PaymentAttemptStatus.Paid);

        if (mPaymentAttempt.Order.Consumer != null)
        {
            pnlPessoaFisica.Visible = mPaymentAttempt.Order.Consumer.CNPJ == null || mPaymentAttempt.Order.Consumer.CNPJ.Equals(String.Empty);
            pnlPessoaJuridica.Visible = !pnlPessoaFisica.Visible;

            if ((MConsumerAddress)this.GetMessage("1") != null)
                pnlBillingAddress.Visible = true;
            if ((MConsumerAddress)this.GetMessage("2") != null)
                pnlDeliveryAddress.Visible = true;
        }
        else
        {
            pnlConsumer.Visible = false;
        }
        
        

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

        if (mPaymentAttempt.Order.Status == MOrder.OrderStatus.Cancelled)
        {
            Label48.Visible = true;
            MsgLabel47.Visible = true;
            Label49.Visible = true;
            MsgLabel48.Visible = true;
            Label59.Visible = true;
            TextBox1.Visible = true;
            TextBox1.Enabled = false;
        }
        else if (mPaymentAttempt.Order.Status == MOrder.OrderStatus.Approved)
        {
            Label48.Visible = true;
            MsgLabel47.Visible = true;
            Label49.Visible = true;
            MsgLabel48.Visible = true;
        }
        else
        {
            Label58.Visible = true;
            Label59.Visible = true;
            Msgdropdownlist1.Visible = true;
            btnAlterar.Visible = true;
            TextBox1.Visible = true;

            Motivo.Style.Add(HtmlTextWriterStyle.Display, "none");

            Msgdropdownlist1.Attributes.Add("onchange", "javascript: Mostrar();");

            StringBuilder sb = new StringBuilder();
            sb.Append("<script>function Mostrar(){ var combo = document.getElementById('" + Msgdropdownlist1.ClientID + "');");
            sb.Append("if(combo.options[combo.selectedIndex].value == " + (int)OrderStatus.Cancelled + ") { var td = document.getElementById('" + Motivo.ClientID + "'); td.style.display = 'block'; }");
            sb.Append("else { var td = document.getElementById('" + Motivo.ClientID + "'); var ta = document.getElementById('" + TextBox1.ClientID + "');");
            sb.Append("td.style.display = 'none'; ta.value = ''; }; }</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "mostrar", sb.ToString());
        }

        //switch (mPaymentAttempt.Order.Status)
        //{
        //    case MOrder.OrderStatus.Unfinished:
        //        Msglabel2.Text = "Não Concluído";
        //        break;
        //    case MOrder.OrderStatus.Analysing:
        //        Msglabel2.Text = "Em Análise";
        //        break;
        //    case MOrder.OrderStatus.Approved:
        //        Msglabel2.Text = "Aprovado";
        //        break;
        //    case MOrder.OrderStatus.Cancelled:
        //        Msglabel2.Text = "Cancelado";
        //        break;
        //}
    }

    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.TransactionDetail.GoBack));
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
    protected void btnAlterar_Click(object sender, EventArgs e)
    {
        this.RaiseEvent(typeof(Ev.TransactionDetail.Alterar));
    }
}
