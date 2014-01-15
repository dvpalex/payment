using System;
using System.Data;
using System.Globalization;
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
using SuperPag.Helper;
using SuperPag.Agents.Boleto;

public partial class Agents_Boleto_showboleto : System.Web.UI.Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido iniciando uma transação de boleto");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());
        DPaymentAgentSetupBoleto dPaymentAgentSetupBoleto = DataFactory.PaymentAgentSetupBoleto().Locate(attempt.paymentAgentSetupId);
        Ensure.IsNotNullPage(dPaymentAgentSetupBoleto, "A loja não está configurada corretamente para esse meio de pagamento");
        DOrder dOrder = DataFactory.Order().Locate(attempt.orderId);
        DOrderInstallment dOrderInstallment = DataFactory.OrderInstallment().Locate(attempt.orderId, attempt.installmentNumber);
        DPaymentAttemptBoleto dPaymentAttemptBoleto = DataFactory.PaymentAttemptBoleto().Locate(attempt.paymentAttemptId);

        LinhaDigitavel = dPaymentAttemptBoleto.oct;
        Agencia = dPaymentAgentSetupBoleto.agencyNumber + "-" + dPaymentAgentSetupBoleto.agencyDigit;
        CodigoCedente = dPaymentAgentSetupBoleto.accountNumber + "-" + dPaymentAgentSetupBoleto.accountDigit;
        Cedente = dPaymentAgentSetupBoleto.cederName;
        ValorDocumento = attempt.price.ToString("N", GenericHelper.GetNumberFormatBrasil());
        DataDocumento = dPaymentAttemptBoleto.paymentDate.ToString("dd/MM/yyyy");
        DataVencimento = dPaymentAttemptBoleto.expirationPaymentDate.ToString("dd/MM/yyyy");
        Sacado = dPaymentAttemptBoleto.withdraw.ToUpper();
        CPF_CNPJ = dPaymentAgentSetupBoleto.cederCNPJ;
        NossoNumero = dPaymentAttemptBoleto.ourNumber;

        if (dPaymentAgentSetupBoleto.bankNumber == 399)
            NumeroDocumento = dPaymentAttemptBoleto.agentOrderReference.ToString().PadLeft(13, '0');
        else
            NumeroDocumento = dOrder.storeReferenceOrder;

        Endereco = dPaymentAttemptBoleto.address1.ToUpper() + " " + dPaymentAttemptBoleto.address2.ToUpper() + " " + dPaymentAttemptBoleto.address3.ToUpper();
        Instrucoes = dPaymentAttemptBoleto.instructions.Replace("#", "<br/>").Replace("|", "");
        CodigoBarras = Boleto.MostraImagemCodigoBarras(dPaymentAttemptBoleto.barCode);
        NumeroBanco = dPaymentAgentSetupBoleto.bankNumber.ToString().PadLeft(3, '0');
        DigitoBanco = dPaymentAgentSetupBoleto.bankDigit.ToString();
        Carteira = dPaymentAgentSetupBoleto.wallet;

        switch (NumeroBanco)
        {
            case "399":
                lblLocalPagamento.Text = "Pagar preferencialmente em agência do HSBC";
                CodigoCedente = dPaymentAgentSetupBoleto.accountNumber.ToString();
                lblAgCodCedente.Text = CodigoCedente;
                break;
            default:
                lblLocalPagamento.Text = "Pagável em qualquer banco até o vencimento";
                CodigoCedente = dPaymentAgentSetupBoleto.accountNumber + "-" + dPaymentAgentSetupBoleto.accountDigit;
                lblAgCodCedente.Text = Agencia + "/" + CodigoCedente;
                break;
        }
        lblAgCodCedente2.Text = lblAgCodCedente.Text;
    }

    public string NumeroBanco;
    public string DigitoBanco;
    public string CodigoBarras;
    public string Cedente;
    public string LinhaDigitavel;
    public string Agencia;
    public string NossoNumero;
    public string CodigoCedente;
    public string CPF_CNPJ;
    public string NumeroDocumento;
    public string ValorDocumento;
    public string DataDocumento;
    public string DataVencimento;
    public string Sacado;
    public string Instrucoes;
    public string Endereco;
    public string Carteira;
}
