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
using K = SuperPag.Agents.Komerci;
using SuperPag.Helper;
using SuperPag;

public partial class Agents_KomerciInBox_komerciinbox : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação Master/Diners");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupKomerci agentsetup = DataFactory.PaymentAgentSetupKomerci().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.KomerciInBox);

        DPaymentAttemptKomerci dPaymentAttemptKomerciReceived = DataFactory.PaymentAttemptKomerci().Locate(attempt.paymentAttemptId);
        if (dPaymentAttemptKomerciReceived != null)
            return;

        DPaymentAttemptKomerci dPaymentAttemptKomerci = new DPaymentAttemptKomerci();
        dPaymentAttemptKomerci.paymentAttemptId = attempt.paymentAttemptId;
        dPaymentAttemptKomerci.bandeira = (attempt.paymentFormId == (int)PaymentForms.DinersKomerci || attempt.paymentFormId == (int)PaymentForms.DinersKomerciInBox ? "DINERS" : "MASTERCARD");
        dPaymentAttemptKomerci.codver = K.Komerci.GeraCodVerificacao(agentsetup.businessNumber.ToString(), GenericHelper.FormatCurrency(attempt.price), Request.UserHostAddress);

        //Enviar tipo de pagamento especifico
        DStorePaymentInstallment installment = DataFactory.StorePaymentInstallment().Locate(order.storeId, attempt.paymentFormId, order.installmentQuantity);
        if (order.installmentQuantity == 1)
            dPaymentAttemptKomerci.transacao = "04";
        else if (installment.installmentType == (byte)InstallmentType.Emissor)
            dPaymentAttemptKomerci.transacao = "06";
        else
            dPaymentAttemptKomerci.transacao = "08";
        
        dPaymentAttemptKomerci.komerciStatus = (byte)PaymentAttemptKomerciStatus.Initial;
        dPaymentAttemptKomerci.avs = (agentsetup.checkAVS ? "S" : null);
        DataFactory.PaymentAttemptKomerci().Insert(dPaymentAttemptKomerci);
    }
}
