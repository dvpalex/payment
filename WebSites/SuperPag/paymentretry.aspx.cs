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
using SuperPag.Helper;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag;

public partial class paymentrety : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string agentWebPage = "";

        try
        {
            Ensure.IsNotNull(Request["id"], "Post inválido para o reprocessamento do pagamento");

            GenericHelper.LogFile("SuperPag::paymentretry.aspx.cs::Page_Load tentativa de pagamento " + Request["id"].ToString() + " recebida para reprocessamento", SuperPag.LogFileEntryType.Information);

            if (!ValidatePaymentAttempt(Request["id"].ToString()))
                Ensure.IsNotNull(null, "Tentativa de pagamento {0} não autorizada para reprocessamento", Request["id"].ToString());

            DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
            Ensure.IsNotNull(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());

            DOrder order = DataFactory.Order().Locate(attempt.orderId);

            if (OrderIsPaid(order))
                Ensure.IsNotNull(null, "O pedido {0} já foi reprocessado anteriormente", order.orderId);

            if (attempt.paymentFormId != (int)PaymentForms.MasterKomerciInBox && attempt.paymentFormId != (int)PaymentForms.DinersKomerciInBox)
                Ensure.IsNotNull(null, "Reprocessamento válido apenas para Master e Diners");

            int paymentAgentSetupId = GenericHelper.GetPaymentAgentSetupId(order.storeId, attempt.paymentFormId);
            int paymentAgentId = GenericHelper.GetPaymentAgentId(attempt.paymentFormId);

            DPaymentAttempt paymentAttempt = new DPaymentAttempt();
            paymentAttempt.paymentAttemptId = Guid.NewGuid();
            paymentAttempt.price = order.finalAmount - (order.installmentQuantity * 0.10M);
            paymentAttempt.orderId = order.orderId;
            paymentAttempt.paymentFormId = attempt.paymentFormId;
            paymentAttempt.paymentAgentSetupId = paymentAgentSetupId;
            paymentAttempt.startTime = DateTime.Now;
            paymentAttempt.lastUpdate = DateTime.Now;
            paymentAttempt.step = 0;
            paymentAttempt.installmentNumber = int.MinValue;
            paymentAttempt.status = (int)PaymentAttemptStatus.Pending;
            paymentAttempt.billingScheduleId = int.MinValue;
            DataFactory.PaymentAttempt().Insert(paymentAttempt);

            GenericHelper.RefillSessionByAttempt(paymentAttempt.paymentAttemptId);

            GenericHelper.LogFile("SuperPag::paymentretry.aspx.cs::Page_Load a nova tentativa de pagamento " + paymentAttempt.paymentAttemptId + " será redirecionada para o agente de pagamento", SuperPag.LogFileEntryType.Information);

            agentWebPage = GenericHelper.GetPaymentAgentWebPage(paymentAgentId, order.storeId);
        }
        catch (ApplicationException ex)
        {
            GenericHelper.LogFile("SuperPag::paymentretry.aspx.cs::Page_Load " + ex.Message, SuperPag.LogFileEntryType.Warning);
            GenericHelper.RedirectToErrorPage(ex.Message);
        }
        catch (Exception ex)
        {
            GenericHelper.LogFile("SuperPag::paymentretry.aspx.cs::Page_Load " + ex.Message, SuperPag.LogFileEntryType.Error);
            GenericHelper.RedirectToErrorPage("Problemas no processamento");
        }

        Response.Redirect(agentWebPage);
    }

    private bool ValidatePaymentAttempt(string attempt)
    {
        switch(attempt.ToUpper())
        {
            case "C3D2C2E8-9741-48EB-8ADB-4E6F40A3752C":
            case "5C9063A7-A674-45F2-AED6-800F451CF835":
            case "65EAF09F-EBCA-4748-88C6-C2E75B1B4B55":
            case "67275B06-9711-4835-B599-9CEB0A47249E":
            case "DE9CF35B-0D35-4A09-9156-0D54F8368447":
            case "114D5C0A-6DB1-4A60-914B-74AD2792BC95":
                return true;
            default:
                return false;
        }
    }
    
    private bool OrderIsPaid(DOrder order)
    {
        bool ret = false;

        DPaymentAttempt[] arrAttempt = DataFactory.PaymentAttempt().List(order.orderId);
        if (Ensure.IsNotNull(arrAttempt))
        {
            foreach (DPaymentAttempt attempt in arrAttempt)
                if (attempt.price != decimal.MinValue && attempt.status == (int)PaymentAttemptStatus.Paid)
                {
                    ret = true;
                    break;
                }
        }

        return ret;
    }
}
