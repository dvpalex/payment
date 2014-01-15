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
        Ensure.IsNotNullPage(Request["id"], "Post inv�lido exibindo o resultado de uma transa��o Visa");

        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(new Guid(Request["id"]));
        Ensure.IsNotNullPage(attempt, "Tentativa de pagamento {0} n�o encontrada", Request["id"].ToString());
        DPaymentAttemptABN attemptABN = DataFactory.PaymentAttemptABN().Locate(attempt.paymentAttemptId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        FillTableTop(order.storeId);
        FillButtonImage();
        
        // verifico se o guid passado na querystring � de uma transa��o n�o aprovada
        if (attempt.status != (int)PaymentAttemptStatus.Pending && attempt.status != (int)PaymentAttemptStatus.NotPaid)
            GenericHelper.RedirectToErrorPage("Transa��o inconsistente");

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
                        lblMessage.Text = "Identifica��o do certificado da loja virtual inv�lido";
                        break;
                    case "-8002":
                        lblMessage.Text = "Vers�o de layout inv�lida inv�lida";
                        break;
                    case "-8003":
                        lblMessage.Text = "Endere�o da p�gina de retorno inv�lido";
                        break;
                    case "-8004":
                        lblMessage.Text = "C�digo seq�encial da opera��o inv�lido";
                        break;
                    case "-8005":
                        lblMessage.Text = "Produto do financiamento inv�lido";
                        break;
                    case "-8006":
                        lblMessage.Text = "Tipo de pessoa inv�lido";
                        break;
                    case "-8007":
                        lblMessage.Text = "Nome completo / Raz�o Social inv�lido";
                        break;
                    case "-8008":
                        lblMessage.Text = "Endere�o de email inv�lido";
                        break;
                    case "-8009":
                        lblMessage.Text = "CGC/CPF inv�lido";
                        break;
                    case "-8021":
                        lblMessage.Text = "Tabela de financiamento inv�lida";
                        break;
                    case "-8022":
                        lblMessage.Text = "Valor de compra inv�lido";
                        break;
                    case "-8023":
                        lblMessage.Text = "Data do 1o. vencimento inv�lida";
                        break;
                    case "-8024":
                        lblMessage.Text = "Quantidade de presta��es inv�lida";
                        break;
                    case "-8025":
                        lblMessage.Text = "Valor da presta��o inv�lido";
                        break;
                    case "-8026":
                        lblMessage.Text = "Garantia inv�lida";
                        break;
                    case "-8027":
                        lblMessage.Text = "Descri��o do objeto financiado inv�lido";
                        break;
                    case "-8028":
                        lblMessage.Text = "Valor da entrada inv�lido";
                        break;
                    case "-8029":
                        lblMessage.Text = "C�digo da proposta no FLV inv�lido";
                        break;
                    case "-8032":
                        lblMessage.Text = "Taxa ano ABN inv�lida (comunique-se com o ABN)";
                        break;
                    case "-8033":
                        lblMessage.Text = "Valor do IOF inv�lido (comunique-se com o ABN)";
                        break;
                    case "-8034":
                        lblMessage.Text = "Valor financiado inv�lido (comunique-se com o ABN)";
                        break;
                    case "-8035":
                        lblMessage.Text = "Valor total do financiamento inv�lido (comunique-se com o ABN)";
                        break;
                    case "-8036":
                        lblMessage.Text = "Data do �ltimo vencimento inv�lida (comunique-se com o ABN)";
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
                lblMessage.Text = "Proposta de financiamento j� cancelada";
                break;
            default:
                lblMessage.Text = "Erro desconhecido. C�digo: " + attemptABN.codRet + ", Mensagem: " + attemptABN.msgRet;
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
