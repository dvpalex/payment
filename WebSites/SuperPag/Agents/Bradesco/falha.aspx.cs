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

public partial class Agents_Bradesco_falha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNull(Request["PaymentAttemptId"], "Sessão inválida tentando recuperar o código de transação Bradesco.");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["PaymentAttemptId"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["PaymentAttemptId"].ToString());
        DPaymentAttemptBradesco attemptBradesco = DataFactory.PaymentAttemptBradesco().Locate(attempt.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        string http, server, path;

        // verifico se o guid passado na querystring é de uma transação não aprovada
        if (attempt.status != (int)PaymentAttemptStatus.Pending && attempt.status != (int)PaymentAttemptStatus.NotPaid && attempt.status != (int)PaymentAttemptStatus.Paid)
            GenericHelper.RedirectToErrorPage("Transação inconsistente");
        else if (attempt.status == (int)PaymentAttemptStatus.Paid)
        {
            // Recupera Session perdida
            Session["PaymentAttemptId"] = attempt.paymentAttemptId;

            attemptBradesco.bradescoStatus = (int)PaymentAttemptBradescoStatus.End;
            DataFactory.PaymentAttemptBradesco().Update(attemptBradesco);

            http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
            server = Request.ServerVariables["SERVER_NAME"];
            path = Request.ServerVariables["PATH_INFO"].Replace("confirma.aspx", "");

            ClientHttpRequisition redirect1 = new ClientHttpRequisition();
            redirect1.Url = String.Format("{0}://{1}/finalization.aspx", http, server);
            redirect1.Target = "frameRetorno";
            redirect1.Script = "parent.close();";
            redirect1.Parameters.Add("id", attempt.paymentAttemptId.ToString());
            redirect1.Send();
        }

        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        attemptBradesco.cod = Request["cod"];
        attempt.returnMessage = Request["errordesc"];

        attemptBradesco.bradescoStatus = (byte)PaymentAttemptBradescoStatus.End;
        attempt.status = (int)PaymentAttemptStatus.NotPaid;
        attempt.lastUpdate = DateTime.Now;

        attempt.TruncateStringFields();

        DataFactory.PaymentAttempt().Update(attempt);
        DataFactory.PaymentAttemptBradesco().Update(attemptBradesco);
        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

        http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
        server = Request.ServerVariables["SERVER_NAME"];
        path = Request.ServerVariables["PATH_INFO"].Replace("falha.aspx", "");

        ClientHttpRequisition redirect2 = new ClientHttpRequisition();
        redirect2.Url = String.Format("{0}://{1}{2}/naoaprovada.aspx", http, server, path);
        redirect2.Target = "frameRetorno";
        redirect2.Script = "parent.close();";
        redirect2.Parameters.Add("id", attempt.paymentAttemptId.ToString());
        redirect2.Send();
    }

}
