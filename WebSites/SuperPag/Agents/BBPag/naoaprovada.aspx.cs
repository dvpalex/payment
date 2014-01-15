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
using SuperPag;
using SuperPag.Data;
using SuperPag.Helper;

public partial class Agents_BBPag_naoaprovada : System.Web.UI.Page
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
        Ensure.IsNotNullPage(Request["id"], "Post inv�lido exibindo o resultado de uma transa��o BBPAG");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} n�o encontrada", Request["id"].ToString());
        DPaymentAttemptBB attemptBB = DataFactory.PaymentAttemptBB().Locate(attempt.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        FillTableTop(order.storeId);

        // verifico se o guid passado na querystring � de uma transa��o n�o aprovada
        if (attempt.status != (int)PaymentAttemptStatus.Pending && attempt.status != (int)PaymentAttemptStatus.NotPaid)
            GenericHelper.RedirectToErrorPage("Transa��o inconsistente");

        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        if (attemptBB.situacao != null)
            lblCode.Text = attemptBB.situacao;
        if (attemptBB.msgret != null)
            lblMessage.Text = HttpUtility.UrlDecode(attemptBB.msgret.Replace("%E1", "�"));
        else
            lblMessage.Text = "O usu�rio fechou a janela antes do t�rmino da transa��o";

        GenericHelper.SetPaymentFormSession(Context, int.MinValue);
        GenericHelper.SetPaymentGroupSession(Context, int.MinValue);
        GenericHelper.SetInstallmentNumber(Context, int.MinValue);
    }
}
