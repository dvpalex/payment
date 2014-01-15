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
using SuperPag.Helper;
using SuperPag.Data;
using SuperPag;
using SuperPag.Agents.Boleto;

public partial class Agents_Boleto_boletoretry : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string storeKey = Request["5DED746B8F924F2E"];
        string storeReferenceOrder = Request["91D4C3128BF7DA7F"];
        string parcela = Request["NUM_PARCELA"];
        string data = Request["DATA_BOLETO"];
        string valor = Request["VAL_BOLETO"];
        string instrucao = Request["INSTRUCAO_BOLETO"];
        
        if (storeKey == null || storeKey == string.Empty)
            GenericHelper.RedirectToErrorPage("Código de loja inválido");
        
        DStore dStore = DataFactory.Store().Locate(storeKey);
        Ensure.IsNotNullPage(dStore, "A chave de loja {0} é invalida", storeKey);

        //obtem os dados do da url de retorno
        DServicesConfiguration dServicesConfiguration = DataFactory.ServicesConfiguration().Locate(dStore.storeId);
        Ensure.IsNotNull(dServicesConfiguration, "A loja {0} não está configurada", dStore.storeId);

        if(String.IsNullOrEmpty(dServicesConfiguration.urlBoletoRetry))
            GenericHelper.RedirectToErrorPage("A loja {0} não está configurada para este serviço", dStore.storeId);
        
        DOrder[] aOrder = DataFactory.Order().List(dStore.storeId, storeReferenceOrder);
        Ensure.IsNotNullPage(aOrder, "O pedido {0} é invalido", storeReferenceOrder);

        DateTime expirationPaymentDate = GenericHelper.ParseDateyyyyMMdd(data);
        if (expirationPaymentDate == DateTime.MinValue)
            GenericHelper.RedirectToErrorPage("A data {0} é inválida", data);
        
        int installmentNumber = 0;
        if (!Int32.TryParse(parcela, out installmentNumber))
            GenericHelper.RedirectToErrorPage("Parcela {0} é inválida", parcela);

        // utilizo o pedido mais recente. O método anterior "List" devolve a lista
        // ordenada decrescente por data
        long orderId = aOrder[0].orderId;

        // também utilizo a tentativa de pagamento mais recente
        DPaymentAttempt dPaymentAttempt = DataFactory.PaymentAttempt().Locate(orderId, installmentNumber, new int[] { (int)PaymentForms.BoletoBancoDoBrasil, (int)PaymentForms.BoletoBradesco, (int)PaymentForms.BoletoItau, (int)PaymentForms.BoletoHSBC });
        Ensure.IsNotNullPage(dPaymentAttempt, "Nenhuma tentativa de pagamento de boleto foi encontrada para o pedido {0}", orderId);

        DPaymentAgentSetupBoleto dPaymentAgentSetupBoleto = DataFactory.PaymentAgentSetupBoleto().Locate(dPaymentAttempt.paymentAgentSetupId);
        Ensure.IsNotNull(dPaymentAgentSetupBoleto, "A loja não está configurada corretamente para esse meio de pagamento");
        
        DPaymentAttemptBoleto dPaymentAttemptBoleto = DataFactory.PaymentAttemptBoleto().Locate(dPaymentAttempt.paymentAttemptId);
        Ensure.IsNotNullPage(dPaymentAttemptBoleto, "Nenhuma tentativa de pagamento de boleto foi encontrada para o identificador {0}", dPaymentAttempt.paymentAttemptId);

        decimal installmentValue = dPaymentAttempt.price;
        if (valor != null && valor != string.Empty)
        {
            installmentValue = GenericHelper.ParseDecimal(valor);
            if (installmentValue == Decimal.MinValue)
                GenericHelper.RedirectToErrorPage("A valor {0} é inválido", valor);
        }
        else if (dPaymentAttempt.price == decimal.MinValue)
        {
            DOrderInstallment dOrderInstallment = DataFactory.OrderInstallment().Locate(orderId, installmentNumber);
            Ensure.IsNotNullPage(dOrderInstallment, "A Parcela {0} do pedido {1} não foi encontrada", installmentNumber, orderId);
            installmentValue = dOrderInstallment.installmentValue;
        }

        DPaymentAttempt paymentAttemptNew = new DPaymentAttempt();
        
        paymentAttemptNew.paymentAttemptId = Guid.NewGuid();
        paymentAttemptNew.orderId = dPaymentAttempt.orderId;
        paymentAttemptNew.paymentFormId = dPaymentAttempt.paymentFormId;
        paymentAttemptNew.paymentAgentSetupId = dPaymentAttempt.paymentAgentSetupId;
        paymentAttemptNew.price = installmentValue;
        paymentAttemptNew.startTime = DateTime.Now;
        paymentAttemptNew.lastUpdate = DateTime.Now;
        paymentAttemptNew.step = dPaymentAttempt.step;
        paymentAttemptNew.installmentNumber = dPaymentAttempt.installmentNumber;
        paymentAttemptNew.status = (int)dPaymentAttempt.status;
        paymentAttemptNew.returnMessage = dPaymentAttempt.returnMessage;
        paymentAttemptNew.billingScheduleId = dPaymentAttempt.billingScheduleId;
        paymentAttemptNew.isSimulation = dPaymentAttempt.isSimulation;
        DataFactory.PaymentAttempt().Insert(paymentAttemptNew);

        DPaymentAttemptBoleto paymentAttemptBoletoNew = new DPaymentAttemptBoleto();
        paymentAttemptBoletoNew.paymentAttemptId = paymentAttemptNew.paymentAttemptId;
        paymentAttemptBoletoNew.documentNumber = dPaymentAttemptBoleto.documentNumber;
        paymentAttemptBoletoNew.withdraw = dPaymentAttemptBoleto.withdraw;
        paymentAttemptBoletoNew.withdrawDoc = dPaymentAttemptBoleto.withdrawDoc;
        paymentAttemptBoletoNew.address1 = dPaymentAttemptBoleto.address1;
        paymentAttemptBoletoNew.address2 = dPaymentAttemptBoleto.address2;
        paymentAttemptBoletoNew.address3 = dPaymentAttemptBoleto.address3;
        paymentAttemptBoletoNew.paymentDate = dPaymentAttemptBoleto.paymentDate;
        paymentAttemptBoletoNew.instructions = instrucao;
        paymentAttemptBoletoNew.expirationPaymentDate = expirationPaymentDate;
        DataFactory.PaymentAttemptBoleto().Insert(paymentAttemptBoletoNew);

        #region Cálculo do código de barras, nosso número e oct
        BoletosBancariosInfo boletosBancariosInfo = new BoletosBancariosInfo();
        boletosBancariosInfo.Carteira = dPaymentAgentSetupBoleto.wallet;
        boletosBancariosInfo.Convenio = dPaymentAgentSetupBoleto.conventionNumber;
        boletosBancariosInfo.CodBanco = dPaymentAgentSetupBoleto.bankNumber;
        boletosBancariosInfo.CodMoeda = "9";
        boletosBancariosInfo.DataVencimento = paymentAttemptBoletoNew.expirationPaymentDate;
        boletosBancariosInfo.NossoNumero = paymentAttemptBoletoNew.agentOrderReference.ToString();
        boletosBancariosInfo.ValorBoleto = installmentValue;
        boletosBancariosInfo.CalculaFatorVencimento = true;
        boletosBancariosInfo.ContaCorrente = dPaymentAgentSetupBoleto.accountNumber.ToString();
        boletosBancariosInfo.Agencia = dPaymentAgentSetupBoleto.agencyNumber.ToString();
        boletosBancariosInfo.CodigoPedidoLoja = paymentAttemptBoletoNew.documentNumber;

        string codigoBarras = "";
        string linhaDigitavel = "";
        string nossoNumero = "";
        switch (dPaymentAttempt.paymentFormId)
        {
            case (int)PaymentForms.BoletoBancoDoBrasil:
                BoletoBB boletoBB = new BoletoBB(boletosBancariosInfo);
                nossoNumero = boletoBB.ObtemNossoNumero();
                codigoBarras = boletoBB.ObtemCodigoBarra();
                linhaDigitavel = boletoBB.LinhaDigitavel(codigoBarras);
                break;
            case (int)PaymentForms.BoletoBradesco:
                BoletoBradesco boletoBradesco = new BoletoBradesco(boletosBancariosInfo);
                nossoNumero = boletoBradesco.ObtemNossoNumero();
                codigoBarras = boletoBradesco.ObtemCodigoBarra();
                linhaDigitavel = boletoBradesco.LinhaDigitavel(codigoBarras);
                break;
            case (int)PaymentForms.BoletoItau:
                BoletoItau boletoItau = new BoletoItau(boletosBancariosInfo);
                nossoNumero = boletoItau.ObtemNossoNumero();
                codigoBarras = boletoItau.ObtemCodigoBarra();
                linhaDigitavel = boletoItau.LinhaDigitavel(codigoBarras);
                break;
            case (int)PaymentForms.BoletoHSBC:
                BoletoHSBC boletoHSBC = new BoletoHSBC(boletosBancariosInfo);
                nossoNumero = boletoHSBC.ObtemNossoNumero();
                codigoBarras = boletoHSBC.ObtemCodigoBarra();
                linhaDigitavel = boletoHSBC.LinhaDigitavel(codigoBarras);
                break;
        }
        #endregion

        paymentAttemptBoletoNew.oct = linhaDigitavel;
        paymentAttemptBoletoNew.barCode = codigoBarras;
        paymentAttemptBoletoNew.ourNumber = nossoNumero;
        DataFactory.PaymentAttemptBoleto().Update(paymentAttemptBoletoNew);

        string serverUrl = "";
        if (HttpContext.Current != null)
        {
            string http = (HttpContext.Current.Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
            string server = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            serverUrl = String.Format("{0}://{1}", http, server);
        }
        else if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["ServerUrl"] != null)
            serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
        
        ClientHttpRequisition post = new ClientHttpRequisition();
        post.Method = "POST";
        post.FormName = "boletoretry";
        post.Url = dServicesConfiguration.urlBoletoRetry;

        post.Parameters.Add("91D4C3128BF7DA7F", storeReferenceOrder);
        post.Parameters.Add("QTD_PARCELAS", aOrder[0].installmentQuantity.ToString());
        post.Parameters.Add(String.Format("URLBOLETO{0}", installmentNumber), String.Format("{0}/Agents/Boleto/showboleto.aspx?id={1}", serverUrl, paymentAttemptNew.paymentAttemptId));
        
        post.Send();
    }
}
