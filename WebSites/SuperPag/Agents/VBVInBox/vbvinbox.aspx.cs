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

public partial class Agents_VBVInBox_vbvinbox : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação VISA");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupVBV agentsetup = DataFactory.PaymentAgentSetupVBV().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.VBVInBox);

        string tid = "";
        DateTime tidDate = DateTime.Now;

        tid = agentsetup.businessNumber.ToString().PadLeft(10, '0').Substring(4, 5);
        tid += tidDate.Year.ToString("0000").Substring(3, 1);
        tid += tidDate.DayOfYear.ToString("000");
        tid += tidDate.ToString("HHmmss");
        tid += tidDate.Millisecond.ToString().Substring(0, 1);
        if (order.installmentQuantity == 1)
            tid += "1001"; // Cartões VISA à vista
        else
        {
            //Enviar tipo de pagamento especifico
            DStorePaymentInstallment installment = DataFactory.StorePaymentInstallment().Locate(order.storeId, attempt.paymentFormId, order.installmentQuantity);
            tid += (installment.installmentType == (byte)InstallmentType.Emissor ? "3" : "2") + order.installmentQuantity.ToString("000");
        }

        DPaymentAttemptVBV dPaymentAttemptVBVReceived = DataFactory.PaymentAttemptVBV().Locate(attempt.paymentAttemptId);
        if (dPaymentAttemptVBVReceived != null)
        {
            if (dPaymentAttemptVBVReceived.price != (int)(attempt.price * 100))
            {
                dPaymentAttemptVBVReceived.price = (int)(attempt.price * 100);
                DataFactory.PaymentAttemptVBV().Update(dPaymentAttemptVBVReceived);
            }
        }
        else
        {
            DPaymentAttemptVBV dPaymentAttemptVBV = new DPaymentAttemptVBV();
            dPaymentAttemptVBV.tid = tid;
            dPaymentAttemptVBV.paymentAttemptId = attempt.paymentAttemptId;
            dPaymentAttemptVBV.vbvStatus = (byte)PaymentAttemptVBVStatus.Initial;
            dPaymentAttemptVBV.vbvOrderId = attempt.orderId.ToString();
            dPaymentAttemptVBV.free = attempt.paymentAttemptId.ToString();
            dPaymentAttemptVBV.price = (int)(attempt.price * 100);
            DataFactory.PaymentAttemptVBV().Insert(dPaymentAttemptVBV);
        }
    }
}
