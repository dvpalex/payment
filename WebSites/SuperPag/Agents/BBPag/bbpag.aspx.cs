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

public partial class Agents_BBPag_bbpag : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação Pagamento Eletrônico BB");
        
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupBB agentsetup = DataFactory.PaymentAgentSetupBB().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.BBPag);

        DPaymentAttemptBB dPaymentAttemptBBReceived = DataFactory.PaymentAttemptBB().Locate(attempt.paymentAttemptId);
        if (dPaymentAttemptBBReceived != null)
            return;

        DPaymentAttemptBB dPaymentAttemptBB = new DPaymentAttemptBB();
        dPaymentAttemptBB.paymentAttemptId = attempt.paymentAttemptId;
        dPaymentAttemptBB.valor = attempt.price;
        dPaymentAttemptBB.idConvenio = agentsetup.businessNumber;
        
        if(attempt.paymentFormId == (int)PaymentForms.BBPagCrediario)
            dPaymentAttemptBB.tipoPagamento = 5;
        else
            dPaymentAttemptBB.tipoPagamento = 3;

        dPaymentAttemptBB.dataPagamento = DateTime.Now;
        dPaymentAttemptBB.bbpagStatus = (byte)PaymentAttemptBBPagStatus.Initial;
        DataFactory.PaymentAttemptBB().Insert(dPaymentAttemptBB);
    }
}
