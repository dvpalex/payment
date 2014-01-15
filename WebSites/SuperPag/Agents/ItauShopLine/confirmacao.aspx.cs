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
using System.Xml;
using SuperPag;
using SuperPag.Agents.ItauShopLine;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper;

public partial class Agents_ItauShopLine_orderConfirm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNullPage(Request["DC"], "Post inválido tentando recuperar os dados da transação ItauShopline");
        Ensure.IsNotNullPage(Request["id"], "Post inválido tentando recuperar o código de transação ItauShopline");
        
        string DC = Request["DC"];

        DPaymentAttemptItauShopline attemptItau = DataFactory.PaymentAttemptItauShopline().Locate(GenericHelper.ParseInt(Request["id"]));
        Ensure.IsNotNullPage(attemptItau, "Não foi possível recuperar a tentativa {0} de pagamento do ItauShopline", Request["id"]);
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptItau.paymentAttemptId);
        DPaymentAgentSetupItauShopline agentsetup = DataFactory.PaymentAgentSetupItauShopline().Locate(attempt.paymentAgentSetupId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        
        Crypto cripto = new Crypto();
        cripto.Decripto(DC, agentsetup.criptoKey);

        int pedido = cripto.Pedido;
        string codEmp = cripto.CodEmp;

        if (pedido != attemptItau.agentOrderReference)
            GenericHelper.RedirectToErrorPage("Transação inconsistente na confirmação");

        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        string consulta = cripto.GeraConsulta(codEmp, pedido.ToString(), "1", agentsetup.criptoKey); //0 - HTML , 1 - XML

        ClientHttpRequisition redirect;
        string http, server, path;
        
        //Envio post para sonda
        ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
        post.Method = "POST";
        post.Url = agentsetup.urlItauSonda;
        post.Parameters.Add("DC", consulta);

        attemptItau.itauStatus = (byte)PaymentAttemptItauShoplineStatus.Capture;
        DataFactory.PaymentAttemptItauShopline().Update(attemptItau);
        
        if (post.Send())
        {
            GenericHelper.LogFile("SuperPag::confirmacao.aspx.cs::Agents_ItauShopLine_orderConfirm.Page_Load storeId=" + order.storeId + " orderId=" + order.orderId + " response=" + post.Response, LogFileEntryType.Information);
            
            //Trato o retorno da sonda
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(post.Response);

            //Parametros padrao
            codEmp = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='CodEmp']").Attributes["VALUE"].Value;
            //Parametros de retorno da Sonda
            string ped = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='Pedido']").Attributes["VALUE"].Value;
            string tipPag = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='tipPag']").Attributes["VALUE"].Value;
            string sitPag = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='sitPag']").Attributes["VALUE"].Value;
            string valor = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='Valor']").Attributes["VALUE"].Value;
            string dtPag = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='dtPag']").Attributes["VALUE"].Value;
            string codAut = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='codAut']").Attributes["VALUE"].Value;
            string numId = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='numId']").Attributes["VALUE"].Value;
            string compVend = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='compVend']").Attributes["VALUE"].Value;
            string tipCart = xmlDoc.SelectSingleNode("/consulta/PARAMETER/PARAM[@ID='tipCart']").Attributes["VALUE"].Value;

            attemptItau.tipPag = tipPag;
            attemptItau.sitPag = sitPag;
            attemptItau.dtPag = dtPag;
            attemptItau.codAut = codAut;
            attemptItau.numId = numId;
            attemptItau.compVend = compVend;
            attemptItau.tipCart = tipCart;

            if (sitPag != null)
            {
                attemptItau.dataSonda = DateTime.Now;
                attemptItau.qtdSonda++;

                //00 = pagamento efetuado;
                if (sitPag == "00" || sitPag == "05")
                {
                    attemptItau.itauStatus = (byte)PaymentAttemptItauShoplineStatus.End;
                    attemptItau.msgret = (sitPag == "00" ? "Pagamento efetuado" : "Pagamento efetuado, aguardando compensação");
                    attempt.status = (int)PaymentAttemptStatus.Paid;
                    attempt.lastUpdate = DateTime.Now;
                    attempt.returnMessage = attemptItau.msgret;
                    attemptItau.TruncateStringFields();
                    attempt.TruncateStringFields();
                    DataFactory.PaymentAttempt().Update(attempt);
                    DataFactory.PaymentAttemptItauShopline().Update(attemptItau);
                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                    http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
                    server = Request.ServerVariables["SERVER_NAME"];
                    path = Request.ServerVariables["PATH_INFO"].Replace("confirmacao.aspx", "");

                    redirect = new ClientHttpRequisition();
                    redirect.Url = String.Format("{0}://{1}/finalization.aspx", http, server);
                    redirect.Target = "frameRetorno";
                    redirect.Script = "parent.close();";
                    redirect.Parameters.Add("id", attempt.paymentAttemptId.ToString());
                    redirect.Send();

                    return;
                }

                switch (sitPag)
                {
                    case "01":
                        attemptItau.msgret = "pagamento não finalizado";
                        break;
                    case "02":
                        attemptItau.msgret = "erro no processamento da consulta";
                        break;
                    case "03":
                        attemptItau.msgret = "pagamento não localizado";
                        break;
                    case "04":
                        attemptItau.msgret = "bloqueto emitido com sucesso";
                        break;
                    case "06":
                        attemptItau.msgret = "pagamento não compensado";
                        break;
                    default:
                        attemptItau.msgret = "erro desconhecido";
                        break;
                }
            }
            else
            {
                attemptItau.msgret = "campo sitPag retornou um valor nulo";
            }
        }
        else
        {
            attemptItau.sitPag = "-1";
            attemptItau.msgret = post.Response;
        }

        attemptItau.itauStatus = (byte)PaymentAttemptItauShoplineStatus.End;
        attempt.status = (int)PaymentAttemptStatus.NotPaid;
        attempt.lastUpdate = DateTime.Now;
        attempt.returnMessage = attemptItau.msgret;

        attemptItau.TruncateStringFields();
        attempt.TruncateStringFields();
        
        DataFactory.PaymentAttempt().Update(attempt);
        DataFactory.PaymentAttemptItauShopline().Update(attemptItau);
        GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

        http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
        server = Request.ServerVariables["SERVER_NAME"];
        path = Request.ServerVariables["PATH_INFO"].Replace("confirmacao.aspx", "");

        redirect = new ClientHttpRequisition();
        redirect.Url = String.Format("{0}://{1}{2}/naoaprovada.aspx", http, server, path);
        redirect.Target = "frameRetorno";
        redirect.Script = "parent.close();";
        redirect.Parameters.Add("id", attempt.paymentAttemptId.ToString());
        redirect.Send();
    }
}
