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
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;
using SuperPag;

public partial class Agents_BBPag_start : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNull(Request["id"], "Post inválido iniciando uma transação BBPag");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());
        DPaymentAttemptBB attemptBBPag = DataFactory.PaymentAttemptBB().Locate(attempt.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        DConsumer consumer = DataFactory.Consumer().Locate(order.consumerId);
        DConsumerAddress billingAddress = DataFactory.ConsumerAddress().Locate(order.consumerId, (int)AddressTypes.Billing);

        // verifico se o guid passado na querystring já não foi utilizado anteriormente
        if (attemptBBPag.bbpagStatus != (byte)PaymentAttemptBBPagStatus.Initial)
            GenericHelper.RedirectToErrorPage("Transação executada");

        DPaymentAgentSetupBB agentsetup = DataFactory.PaymentAgentSetupBB().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        
        ClientHttpRequisition post = new ClientHttpRequisition();
        post.Method = "POST";
        post.FormName = "pagamento";
        post.Url = agentsetup.urlBBPag;

        post.Parameters.Add("idConv", agentsetup.businessNumber.ToString());
        post.Parameters.Add("valor", (attemptBBPag.valor * 100.0m).ToString("G0"));
        post.Parameters.Add("refTran", attemptBBPag.agentOrderReference.ToString());
        post.Parameters.Add("urlRetorno", "/Agents/BBPag/notifica.aspx");
        post.Parameters.Add("dtVenc", attemptBBPag.dataPagamento.ToString("ddMMyyyy"));
        post.Parameters.Add("versao", "002");
        post.Parameters.Add("moeda", "986");
        post.Parameters.Add("convClasse", "001");
        post.Parameters.Add("tpPagamento", attemptBBPag.tipoPagamento.ToString());

        //para crediario e boleto
        post.Parameters.Add("nome", consumer.name);
        post.Parameters.Add("endereco", billingAddress.logradouro + " " + billingAddress.address + ", " + billingAddress.addressNumber + " - " + billingAddress.addressComplement);
        post.Parameters.Add("cidade", billingAddress.city);
        post.Parameters.Add("uf", billingAddress.state);
        post.Parameters.Add("cep", billingAddress.cep);

        attemptBBPag.bbpagStatus = (byte)PaymentAttemptBBPagStatus.Post;
        DataFactory.PaymentAttemptBB().Update(attemptBBPag);

        post.Send();
    }
}
