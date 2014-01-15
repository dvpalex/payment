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


public partial class Agents_ABN_popupclose : System.Web.UI.Page
{
    private void FillTableTop(int storeId)
    {
        DSPLegacyStore dSPLegay = DataFactory.SPLegacyStore().Locate(storeId);
        if (Ensure.IsNotNull(dSPLegay) && Ensure.IsNotNull(dSPLegay.ucTableTop))
        {
            plhTableTop.Controls.Add(Page.LoadControl(dSPLegay.ucTableTop));
        }
        else
        {
            plhTableTop.Visible = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["id"], "Post inválido exibindo o resultado de uma transação Visa");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} não encontrada", Request["id"].ToString());
        DPaymentAttemptABN attemptABN = DataFactory.PaymentAttemptABN().Locate(attempt.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        FillTableTop(order.storeId);
        FillButtonImage();
        
        // verifico se o guid passado na querystring é de uma transação não aprovada
        if (attempt.status != (int)PaymentAttemptStatus.Pending && attempt.status != (int)PaymentAttemptStatus.NotPaid)
            GenericHelper.RedirectToErrorPage("Transação inconsistente");

        // Recupera Session se perdida
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        if(attemptABN.numControle != decimal.MinValue)
            lblTid.Text = attemptABN.numControle.ToString();

        switch(attemptABN.codRet)
        {
            case 0:
                #region Tabela de erros ABN
                switch (attemptABN.msgRet)
                {
                    case "-8001":
                        lblMessage.Text = "Identificação do certificado da loja virtual inválido";
                        break;
                    case "-8002":
                        lblMessage.Text = "Versão de layout inválida inválida";
                        break;
                    case "-8003":
                        lblMessage.Text = "Endereço da página de retorno inválido";
                        break;
                    case "-8004":
                        lblMessage.Text = "Código seqüencial da operação inválido";
                        break;
                    case "-8005":
                        lblMessage.Text = "Produto do financiamento inválido";
                        break;
                    case "-8006":
                        lblMessage.Text = "Tipo de pessoa inválido";
                        break;
                    case "-8007":
                        lblMessage.Text = "Nome completo / Razão Social inválido";
                        break;
                    case "-8008":
                        lblMessage.Text = "Endereço de email inválido";
                        break;
                    case "-8009":
                        lblMessage.Text = "CGC/CPF inválido";
                        break;
                    case "-8021":
                        lblMessage.Text = "Tabela de financiamento inválida";
                        break;
                    case "-8022":
                        lblMessage.Text = "Valor de compra inválido";
                        break;
                    case "-8023":
                        lblMessage.Text = "Data do 1o. vencimento inválida";
                        break;
                    case "-8024":
                        lblMessage.Text = "Quantidade de prestações inválida";
                        break;
                    case "-8025":
                        lblMessage.Text = "Valor da prestação inválido";
                        break;
                    case "-8026":
                        lblMessage.Text = "Garantia inválida";
                        break;
                    case "-8027":
                        lblMessage.Text = "Descrição do objeto financiado inválido";
                        break;
                    case "-8028":
                        lblMessage.Text = "Valor da entrada inválido";
                        break;
                    case "-8029":
                        lblMessage.Text = "Código da proposta no FLV inválido";
                        break;
                    case "-8032":
                        lblMessage.Text = "Taxa ano ABN inválida (comunique-se com o ABN)";
                        break;
                    case "-8033":
                        lblMessage.Text = "Valor do IOF inválido (comunique-se com o ABN)";
                        break;
                    case "-8034":
                        lblMessage.Text = "Valor financiado inválido (comunique-se com o ABN)";
                        break;
                    case "-8035":
                        lblMessage.Text = "Valor total do financiamento inválido (comunique-se com o ABN)";
                        break;
                    case "-8036":
                        lblMessage.Text = "Data do último vencimento inválida (comunique-se com o ABN)";
                        break;
                    case "-9000":
                        lblMessage.Text = "Erro de infra-estrutura no ABN AMRO Bank";
                        break;
                    default:
                        lblMessage.Text = attemptABN.msgRet;
                        break;
                } 
                #endregion
                break;
            case 3:
                lblMessage.Text = "Proposta de financiamento já cancelada";
                break;
            default:
                lblMessage.Text = "Erro desconhecido. Código: " + attemptABN.codRet + ", Mensagem: " + attemptABN.msgRet;
                break;
        }

        GenericHelper.SetPaymentFormSession(Context, int.MinValue);
    }
    
    private void FillButtonImage()
    {
        //Seta imagem do botao continuar
        string LinkBotao6, UrlBotao6;
        if (Session["htmlHandshake"] != null)
        {
            LinkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//linkbotao6"));
            UrlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["htmlHandshake"], "//urlbotao6"));
        }
        else
        {
            LinkBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//link_botao6"));
            UrlBotao6 = HttpUtility.HtmlDecode(GenericHelper.GetSingleNodeString((string)Session["xmlHandshake"], "//urlbotao6"));
        }

        if (!String.IsNullOrEmpty(UrlBotao6))
            lnkReturn.ImageUrl = UrlBotao6;
        if (!String.IsNullOrEmpty(LinkBotao6))
        {
            lnkReturn.NavigateUrl = LinkBotao6;
            lnkReturn.Visible = true;
        }
    }
}
