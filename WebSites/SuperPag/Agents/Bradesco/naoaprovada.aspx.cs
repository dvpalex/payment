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

public partial class Agents_Bradesco_naoaprovada : System.Web.UI.Page
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
        Ensure.IsNotNull(Request["id"], "Post inválido exibindo o resultado de uma transação Bradesco");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"]);
        DPaymentAttemptBradesco attemptBradesco = DataFactory.PaymentAttemptBradesco().Locate(attempt.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        FillTableTop(order.storeId);

        // verifico se o guid passado na querystring é de uma transação não aprovada
        if (attempt.status == (int)PaymentAttemptStatus.Paid)
        {
            string http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
            string server = Request.ServerVariables["SERVER_NAME"];
            Response.Redirect(String.Format("{0}://{1}/finalization.aspx?id={2}", http, server, attempt.paymentAttemptId.ToString()));
        }
        else if (attempt.status != (int)PaymentAttemptStatus.Pending && attempt.status != (int)PaymentAttemptStatus.NotPaid)
            GenericHelper.RedirectToErrorPage("Transação inconsistente");

        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        if (attemptBradesco.cod != null)
            lblCode.Text = attemptBradesco.cod;
        if (attempt.returnMessage != null)
            lblMessage.Text = attempt.returnMessage;
        else
            lblMessage.Text = "O usuário fechou a janela antes do término da transação";

        GenericHelper.SetPaymentFormSession(Context, int.MinValue);
        GenericHelper.SetPaymentGroupSession(Context, int.MinValue);
        GenericHelper.SetInstallmentNumber(Context, int.MinValue);
    }
}
