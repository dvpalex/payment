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

public partial class tryagain : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido exibindo o resultado de uma transação Visa");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        FillTableTop(order.storeId);
        FillButtonImage();

        // verifico se o guid passado na querystring é de uma transação não aprovada
        if (attempt.status != (int)PaymentAttemptStatus.Pending && attempt.status != (int)PaymentAttemptStatus.NotPaid)
            GenericHelper.RedirectToErrorPage("Transação inconsistente");

        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        GenericHelper.SetPaymentFormSession(Context, int.MinValue);
        GenericHelper.SetPaymentGroupSession(Context, int.MinValue);
        GenericHelper.SetInstallmentNumber(Context, int.MinValue);
    }

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
