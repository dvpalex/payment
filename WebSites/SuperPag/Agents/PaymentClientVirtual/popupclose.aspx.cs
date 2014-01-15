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
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag;
using SuperPag.Helper;


public partial class Agents_PaymentClientVirtual_popupclose : System.Web.UI.Page
{
    private void FillTableTop(int storeId)
    {
        DSPLegacyStore dSPLegay = DataFactory.SPLegacyStore().Locate(storeId);
        if (Ensure.IsNotNull(dSPLegay) && Ensure.IsNotNull(dSPLegay.ucTableTop))
        {
            plhTableTop.Controls.Add(Page.LoadControl(dSPLegay.ucTableTop));
        }
        else
        {
            plhTableTop.Visible = false;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido exibindo o resultado de uma transação Amex");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());
        DPaymentAttemptPaymentClientVirtual attemptPaymentClientVirtual = DataFactory.PaymentAttemptPaymentClientVirtual().Locate(attempt.paymentAttemptId);
        DStore store = GenericHelper.CheckSessionStore(Context);
        FillTableTop(store.storeId);
        FillButtonImage();
        
        // verifico se o guid passado na querystring é de uma transação não aprovada
        if (attempt.status != (int)PaymentAttemptStatus.Pending && attempt.status != (int)PaymentAttemptStatus.NotPaid)
            GenericHelper.RedirectToErrorPage("Transação inconsistente");

        lblAgentReferenceId.Text = attempt.paymentAttemptId.ToString();
        if (attemptPaymentClientVirtual.vpc_CapTxnResponseCode != null)
            lblAmexMessage.Text = attemptPaymentClientVirtual.vpc_CapTxnResponseCode;
        else if (attemptPaymentClientVirtual.vpc_TxnResponseCode != null)
            lblAmexMessage.Text = attemptPaymentClientVirtual.vpc_TxnResponseCode;
        else
            lblAmexMessage.Text = "A transação não foi concluída";
        if (attempt.returnMessage != null)
            lblMessage.Text = attempt.returnMessage;
        else
            lblMessage.Text = "O usuário fechou a janela antes do término da transação";

       GenericHelper.SetPaymentFormSession(Context, int.MinValue);
       GenericHelper.SetPaymentGroupSession(Context, int.MinValue);
       GenericHelper.SetInstallmentNumber(Context, int.MinValue);
    }

    private void FillButtonImage()
    {
        //Seta imagem do botao continuar
        string LinkBotao6, UrlBotao6;
        if (Session["htmlHandshake"] != null)
        {
            LinkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//linkbotao6"));
            UrlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//urlbotao6"));
        }
        else
        {
            LinkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//link_botao6"));
            UrlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//urlbotao6"));
        }

        if (!String.IsNullOrEmpty(UrlBotao6))
            lnkReturn.ImageUrl = UrlBotao6;
        if (!String.IsNullOrEmpty(LinkBotao6))
        {
            lnkReturn.NavigateUrl = LinkBotao6;
            lnkReturn.Visible = true;
        }
    }
}
