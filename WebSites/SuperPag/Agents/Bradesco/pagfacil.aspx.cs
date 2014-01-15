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

public partial class Agents_Bradesco_pagfacil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação Pagamento Fácil Bradesco");
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupBradesco agentsetup = DataFactory.PaymentAgentSetupBradesco().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        
        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.Bradesco);

        DPaymentAttemptBradesco dPaymentAttemptBradescoReceived = DataFactory.PaymentAttemptBradesco().Locate(attempt.paymentAttemptId);
        if (dPaymentAttemptBradescoReceived != null)
            return;
        DPaymentAttemptBradesco dPaymentAttemptBradesco = new DPaymentAttemptBradesco();
        dPaymentAttemptBradesco.paymentAttemptId = attempt.paymentAttemptId;
        dPaymentAttemptBradesco.bradescoStatus = (int)PaymentAttemptBradescoStatus.Initial;
        DataFactory.PaymentAttemptBradesco().Insert(dPaymentAttemptBradesco);

        //FEATURE: Possibilidade de enviar o proprio pedido da loja ao bradesco
        //20071019 - rodolfo.camara
        dPaymentAttemptBradesco.numOrder = agentsetup.useStoreOrderReference && order.storeReferenceOrder.Length <= 27 ? order.storeReferenceOrder : dPaymentAttemptBradesco.agentOrderReference.ToString();
        DataFactory.PaymentAttemptBradesco().Update(dPaymentAttemptBradesco);
        
        if (attempt.isSimulation)
        {
            dPaymentAttemptBradescoReceived = DataFactory.PaymentAttemptBradesco().Locate(attempt.paymentAttemptId);
            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.Paid;
            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.PaymentAttemptBradesco().Update(dPaymentAttemptBradescoReceived);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
            Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
        }

        Response.Redirect("checkpopup.aspx");
    }
}
