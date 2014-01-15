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
using SuperPag;
using SuperPag.Helper;

public partial class Agents_ABN_start : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GenericHelper.LogFile("SuperPag::start.aspx.cs::Agents_ABN_start.Page_Load Sessão ABN start " + (HttpContext.Current == null ? "1" : "0") + (Session == null ? "1" : "0") + (Session["PaymentAttemptId"] == null ? "-" : "-" + Session["PaymentAttemptId"].ToString()), LogFileEntryType.Information);
        Ensure.IsNotNull(Session["PaymentAttemptId"], "Sessão inválida iniciando uma transação Financiamento ABN");
        
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate((Guid)Session["PaymentAttemptId"]);
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Session["PaymentAttemptId"].ToString());
        DPaymentAttemptABN attemptABN = DataFactory.PaymentAttemptABN().Locate(attempt.paymentAttemptId);
        
        // verifico se o guid passado na querystring já não foi utilizado anteriormente
        if (attemptABN.abnStatus != (int)PaymentAttemptABNStatus.Initial)
            GenericHelper.RedirectToErrorPage("Transação executada");
        
        DPaymentAgentSetupABN agentsetup = DataFactory.PaymentAgentSetupABN().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(agentsetup, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        DOrderInstallment orderInstallment = DataFactory.OrderInstallment().Locate(order.orderId, 1);
        DOrderItem[] orderItens = DataFactory.OrderItem().List(attempt.orderId, (int)ItemTypes.Regular);
        DConsumer consumer = DataFactory.Consumer().Locate(order.consumerId);
        DConsumerAddress billingAddress = DataFactory.ConsumerAddress().Locate(order.consumerId, (int)AddressTypes.Billing);

        string urlRetorno = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
        urlRetorno += "://" + Request.ServerVariables["SERVER_NAME"];
        urlRetorno += "/Agents/ABN/retorno.aspx?aid=" + attemptABN.agentOrderReference;

        ClientHttpRequisition post = new ClientHttpRequisition();
        post.FormName = "FinanciamentoABN";
        post.Method = "POST";
        post.Encoding = "iso-8859-1";
        post.Url = agentsetup.urlAction.Trim() + (String.IsNullOrEmpty(consumer.CNPJ) ? "" : "PJ");
        post.Parameters.Add("VAR01",agentsetup.codigoABN);
		post.Parameters.Add("VAR02","01");
        post.Parameters.Add("VAR03",urlRetorno);
		post.Parameters.Add("VAR04",attemptABN.agentOrderReference.ToString());
        post.Parameters.Add("VAR05",agentsetup.tipoFinanciamento);
        post.Parameters.Add("VAR06",String.IsNullOrEmpty(consumer.CNPJ) ? "F" : "J");
        post.Parameters.Add("VAR07", String.IsNullOrEmpty(consumer.CNPJ) ? consumer.name : GenericHelper.GetCompanyName(order.orderId));
		post.Parameters.Add("VAR08",consumer.email);
        post.Parameters.Add("VAR09", String.IsNullOrEmpty(consumer.CNPJ) ? consumer.CPF : consumer.CNPJ);
		post.Parameters.Add("VAR10",consumer.birthDate.ToString("dd/MM/yyyy"));
		
        #region De-Para Estado Civil
		string civilState;
        switch(consumer.civilState)
        {
            case "0":
                civilState = "9";
                break;
            case "C":
            case "Casado":
                civilState = "2";
                break;
            case "D":
            case "Divorciado":
                civilState = "5";
                break;
            case "S":
            case "Solteiro":
                civilState = "1";
                break;
            case "V":
            case "Viúvo":
                civilState = "4";
                break;
            case "O":
                civilState = "6";
                break;
            default:
                civilState = consumer.civilState;
                break;
        } 
	    #endregion
        post.Parameters.Add("VAR37",civilState);
		post.Parameters.Add("VAR13",billingAddress.logradouro + " " + billingAddress.address + ", " + billingAddress.addressNumber + " - " + billingAddress.addressComplement);
		post.Parameters.Add("VAR14",billingAddress.district);
		post.Parameters.Add("VAR15",billingAddress.city);
		post.Parameters.Add("VAR16",billingAddress.state.Trim());
		post.Parameters.Add("VAR17",billingAddress.cep);
		post.Parameters.Add("VAR18",consumer.phone.Substring(0,2));
		post.Parameters.Add("VAR19",consumer.phone.Substring(2));
		post.Parameters.Add("VAR20",consumer.ger);
		post.Parameters.Add("VAR21",attemptABN.tabelaFinanciamento);
		post.Parameters.Add("VAR22",attempt.price.ToString());
		post.Parameters.Add("VAR23",attemptABN.dataVencimento.ToString("dd/MM/yyyy"));
		post.Parameters.Add("VAR24",order.installmentQuantity.ToString());
        post.Parameters.Add("VAR25",orderInstallment.installmentValue.ToString());
        post.Parameters.Add("VAR26",attemptABN.garantia);

        #region Monto string com descrição do item de maior valor
        decimal itemValue = 0M;
        string itemDescription = "";
        foreach (DOrderItem item in orderItens)
        {
            if(item.itemValue > itemValue)
            {
                itemValue = item.itemValue;
                itemDescription = item.itemDescription;
            }
        }
        #endregion
		post.Parameters.Add("VAR27","Pedido Loja: " + order.storeReferenceOrder.ToString() + " - Item: " + itemDescription);
        
        if(attemptABN.valorEntrada != decimal.MinValue)
            post.Parameters.Add("VAR28", attemptABN.valorEntrada != decimal.MinValue ? attemptABN.valorEntrada.ToString() : "0");

        if (!String.IsNullOrEmpty(consumer.CNPJ))
        {
            post.Parameters.Add("VAR85", consumer.responsibleName);
            post.Parameters.Add("VAR86", consumer.responsibleCPF);
        }

        attemptABN.abnStatus = (int)PaymentAttemptABNStatus.Post;
        DataFactory.PaymentAttemptABN().Update(attemptABN);

        post.Send();
    }
}
