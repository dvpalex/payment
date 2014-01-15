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

public partial class Agents_ABN_abn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação financiamento ABN");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAgentSetupABN agentsetup = DataFactory.PaymentAgentSetupABN().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DPaymentAttemptABN attemptABNReceived = DataFactory.PaymentAttemptABN().Locate(attempt.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        DConsumer consumer = DataFactory.Consumer().Locate(order.consumerId);

        //Seto o status do pedido
        GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + (int)PaymentAgents.FinanciamentoABN);

        //pego os logs com os dados do pedido enviado
        DHandshakeSessionLog[] hsLogs = DataFactory.HandshakeSessionLog().List(new Guid(Session["handshakeSessionId"].ToString()));

        string xmlData = "";
        //pego os parametros especificos do ABN enviados.
        foreach (DHandshakeSessionLog hsLog in hsLogs)
        {
            if (hsLog.step == 3)
                xmlData = hsLog.xmlData;
        }
        Ensure.IsNotNullOrEmpty(xmlData, "Erro: Parâmetros insuficientes para completar uma transação de Financiamento ABN.");

        string valorEntrada = GenericHelper.GetSingleNodeString(xmlData, "//val_entrada_abn");
        string valorParcela = GenericHelper.GetSingleNodeString(xmlData, "//val_parcela_abn");
        Ensure.IsNotNullOrEmpty(valorParcela, "Erro: Parâmetros insuficientes para completar uma transação de Financiamento ABN.");

        string dataVencimento = GenericHelper.GetSingleNodeString(xmlData, "//dat_vencimento1_abn");
        Ensure.IsNotNullOrEmpty(dataVencimento, "Erro: Parâmetros insuficientes para completar uma transação de Financiamento ABN.");
        
        string qtdPrestacao = GenericHelper.GetSingleNodeString(xmlData, "//qtdparcelas_abn");
        Ensure.IsNotNullOrEmpty(valorParcela, "Erro: Parâmetros insuficientes para completar uma transação de Financiamento ABN.");

        if(attemptABNReceived == null)
        {
            DPaymentAttemptABN dPaymentAttemptABN = new DPaymentAttemptABN();
            dPaymentAttemptABN.abnStatus = (int)PaymentAttemptABNStatus.Initial;

            dPaymentAttemptABN.paymentAttemptId = attempt.paymentAttemptId;
            dPaymentAttemptABN.codRet = int.MinValue;
            dPaymentAttemptABN.qtdPrestacao = int.Parse(qtdPrestacao);
            dPaymentAttemptABN.prestacao = GenericHelper.ParseDecimal(valorParcela);
            dPaymentAttemptABN.tabelaFinanciamento = agentsetup.tabelaFinanciamento;
            dPaymentAttemptABN.tipoPessoa = String.IsNullOrEmpty(consumer.CNPJ) ? "F" : "J";
            dPaymentAttemptABN.garantia = agentsetup.garantia;
            dPaymentAttemptABN.valorEntrada = GenericHelper.ParseDecimal(valorEntrada);
            if(Ensure.IsNotNullOrEmpty(dataVencimento))
                dPaymentAttemptABN.dataVencimento = GenericHelper.ParseDateddMMyyyy(dataVencimento.Replace("/",""));
            
            DataFactory.PaymentAttemptABN().Insert(dPaymentAttemptABN);
        }

        if (attempt.isSimulation)
        {
            attemptABNReceived = DataFactory.PaymentAttemptABN().Locate(attempt.paymentAttemptId);
            attemptABNReceived.abnStatus = (byte)PaymentAttemptABNStatus.End;
            attemptABNReceived.numProposta = "0000000";
            attemptABNReceived.statusProposta = "AN";
            attemptABNReceived.msgRet = "Simulacao";
            attemptABNReceived.dataVencimento = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.Paid;

            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.PaymentAttemptABN().Update(attemptABNReceived);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
            Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
        }
        else
        {
            Response.Redirect("~/Agents/ABN/start.aspx");
        }
    }
}
