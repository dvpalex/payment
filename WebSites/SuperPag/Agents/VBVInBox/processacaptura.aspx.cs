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

public partial class Agents_VBVInBox_processacaptura : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["TID"], "Post inválido tentando recuperar o TID de uma transação VISA");
        Ensure.IsNotNullPage(Request["FREE"], "Post inválido tentando recuperar o FREE de uma transação VISA");
        
        string TID = Request["TID"];
        string LR = Request["LR"];
        string ARS = Request["ARS"];
        string CAP = Request["CAP"];
        string FREE = Request["FREE"];

        DPaymentAttemptVBV attemptVBV = DataFactory.PaymentAttemptVBV().Locate(new Guid(FREE));
        Ensure.IsNotNullPage(attemptVBV, "Tentativa de pagamento {0} não encontrada", FREE);

        // verifico se o TID retornado no posto é mesmo que está gravado
        // no registro recupera pelo paymentAttemptId
        if (TID != attemptVBV.tid)
            GenericHelper.RedirectToErrorPage("TID inconsistente");

        // verifico se a transação enviada está realmente esperando a captura
        if (attemptVBV.vbvStatus != (byte)PaymentAttemptVBVStatus.Capture)
            GenericHelper.RedirectToErrorPage("Transação inconsistente na captura");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptVBV.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        // Recupera Session perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        attemptVBV.lr = GenericHelper.ParseInt(LR);
        attemptVBV.ars = ARS;
        attemptVBV.cap = CAP;
        attemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.End;

        if (attemptVBV.lr != 0 && attemptVBV.lr != 3)
        {
            attempt.status = (int)PaymentAttemptStatus.NotPaid;
            attempt.lastUpdate = DateTime.Now;
            switch (LR)
            {
                case "108":
                    attempt.returnMessage = "Falha de comunicação entre o SuperPag e o servidor de POS da VISANET";
                    break;
                case "112":
                    attempt.returnMessage = "Gateway da VisaNet fora de operação";
                    break;
                default:
                    attempt.returnMessage = "Código de erro " + attemptVBV.lr.ToString();
                    break;
            }

            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.PaymentAttemptVBV().Update(attemptVBV);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

            Response.Redirect("~/Agents/VBVInBox/popupclose.aspx?id=" + attempt.paymentAttemptId);

            return;
        }

        attempt.status = (int)PaymentAttemptStatus.Paid;
        attempt.lastUpdate = DateTime.Now;
        attempt.returnMessage = ARS;
        
        DataFactory.PaymentAttempt().Update(attempt);
        DataFactory.PaymentAttemptVBV().Update(attemptVBV);
        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

        Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId);
    }
}
