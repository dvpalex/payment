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


public partial class Agents_VBVInBox_popupclose : System.Web.UI.Page
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
        Ensure.IsNotNullPage(Request["id"], "Post inv�lido exibindo o resultado de uma transa��o Visa");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} n�o encontrada", Request["id"].ToString());
        DPaymentAttemptVBV attemptVBV = DataFactory.PaymentAttemptVBV().Locate(attempt.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        FillTableTop(order.storeId);

        // verifico se o guid passado na querystring � de uma transa��o n�o aprovada
        if (attempt.status != (int)PaymentAttemptStatus.Pending && attempt.status != (int)PaymentAttemptStatus.NotPaid)
            GenericHelper.RedirectToErrorPage("Transa��o inconsistente");

        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        lblTid.Text = attemptVBV.tid;
        if (attemptVBV.lr == decimal.MinValue)
            lblCode.Text = "189";
        else
            lblCode.Text = attemptVBV.lr.ToString();
        if (attemptVBV.ars == null)
            lblMessage.Text = "O usu�rio fechou a janela antes do t�rmino da transa��o";
        else
            lblMessage.Text = attemptVBV.ars;

        GenericHelper.SetPaymentFormSession(Context, int.MinValue);
        GenericHelper.SetPaymentGroupSession(Context, int.MinValue);
        GenericHelper.SetInstallmentNumber(Context, int.MinValue);
    }
}
