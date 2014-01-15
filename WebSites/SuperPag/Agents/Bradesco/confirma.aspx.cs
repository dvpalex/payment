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
using SuperPag;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;

public partial class Agents_Bradesco_confirma : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNull(Request["PaymentAttemptId"], "Sessão inválida tentando recuperar o código de transação Bradesco.");
        
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["PaymentAttemptId"]));
        DPaymentAttemptBradesco attemptBradesco = DataFactory.PaymentAttemptBradesco().Locate(attempt.paymentAttemptId);

        // verifico se o guid passado na querystring é de uma transação concluída com êxito
        if (attempt.status != (int)PaymentAttemptStatus.Paid)
            GenericHelper.RedirectToErrorPage("Transação não concluída");

        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        attemptBradesco.bradescoStatus = (int)PaymentAttemptBradescoStatus.End;
        DataFactory.PaymentAttemptBradesco().Update(attemptBradesco);

        string http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
        string server = Request.ServerVariables["SERVER_NAME"];
        string path = Request.ServerVariables["PATH_INFO"].Replace("confirma.aspx", "");

        ClientHttpRequisition redirect = new ClientHttpRequisition();
        redirect.Url = "javascript:window.close()";
        //redirect.Target = "frameRetorno";
        redirect.Script = "parent.close();";
        //redirect.Parameters.Add("id", attempt.paymentAttemptId.ToString());
        redirect.Send();
    }
}
