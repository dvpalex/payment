using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Xml;
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

public partial class Agents_BBPag_notifica : System.Web.UI.Page
{
    public bool redirectToFinalization = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        Ensure.IsNotNull(Request["refTran"], "Sessão inválida tentando recuperar o código de transação BBPag");
        
        //Parametros padrao
        string tpPagamento = Request["tpPagamento"];
        string refTran = Request["refTran"];
        string idConv = Request["idConv"];

        //Recupero a attempt
        DPaymentAttemptBB attemptBB = DataFactory.PaymentAttemptBB().Locate(GenericHelper.ParseInt(refTran));
        Ensure.IsNotNullPage(attemptBB, "Não foi possível recuperar a tentativa de pagamento do Comércio Eletrônico Banco do Brasil");
        DPaymentAttempt attempt = DataFactory.PaymentAttempt().Locate(attemptBB.paymentAttemptId);
        DPaymentAgentSetupBB agentsetup = DataFactory.PaymentAgentSetupBB().Locate(attempt.paymentAgentSetupId);
        DOrder order = DataFactory.Order().Locate(attempt.orderId);
        
        Session["PaymentAttemptId"] = attempt.paymentAttemptId;

        ClientHttpRequisition redirect;
        string http, server, path;
        
        //Envio post para sonda
        ServerHttpHtmlRequisition post = new ServerHttpHtmlRequisition();
        post.Method = "POST";
        post.UpperKeys = false;
        post.Url = agentsetup.urlBBPagSonda;
        post.Parameters.Add("idConv", idConv);
        post.Parameters.Add("refTran", refTran);
        post.Parameters.Add("valorSonda", GenericHelper.ParseString(attemptBB.valor));
        post.Parameters.Add("formato", "02"); //01 - HTML , 02 - XML , 03 - String

        attemptBB.bbpagStatus = (byte)PaymentAttemptBBPagStatus.Capture;
        DataFactory.PaymentAttemptBB().Update(attemptBB);

        if (post.Send())
        {
            //Trato o retorno da sonda
            string xml = post.Response.Trim(new char[] { '\r', '\n', '\t', ' ' });
            xml = xml.Replace("<!DOCTYPE lojavirtual SYSTEM 'lojavirtual.dtd'>", "");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            //Parametros padrao
            tpPagamento = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='tpPagamento']").Attributes["valor"].Value;
            refTran = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='reftran']").Attributes["valor"].Value;
            idConv = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='idConv']").Attributes["valor"].Value;
            //Parametros de retorno da Sonda
            string situacao = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='situacao']").Attributes["valor"].Value;
            string valor = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='valor']").Attributes["valor"].Value;
            string dataPagamento = xmlDoc.SelectSingleNode("/FORMULARIO/ENTRADA[@nome='dataPagamento']").Attributes["valor"].Value;

            attemptBB.situacao = situacao;

            if (situacao != null)
            {
                attemptBB.dataSonda = DateTime.Now;
                attemptBB.qtdSonda++;
                
                //00 = pagamento efetuado;
                if (situacao == "00")
                {
                    attemptBB.bbpagStatus = (byte)PaymentAttemptBBPagStatus.End;
                    attemptBB.msgret = "Pagamento efetuado";
                    attempt.status = (int)PaymentAttemptStatus.Paid;
                    attempt.lastUpdate = DateTime.Now;
                    attempt.returnMessage = attemptBB.msgret;
                    attemptBB.TruncateStringFields();
                    attempt.TruncateStringFields();
                    DataFactory.PaymentAttempt().Update(attempt);
                    DataFactory.PaymentAttemptBB().Update(attemptBB);
                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                    http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
                    server = Request.ServerVariables["SERVER_NAME"];
                    path = Request.ServerVariables["PATH_INFO"].Replace("notifica.aspx", "");

                    redirect = new ClientHttpRequisition();
                    redirect.Url = String.Format("{0}://{1}/finalization.aspx", http, server);
                    redirect.Target = "frameRetorno";
                    redirect.Script = "parent.close();self.close();";
                    redirect.Parameters.Add("id", attempt.paymentAttemptId.ToString());
                    redirect.Send();
                    redirectToFinalization = true;
                    return;
                }

                switch (situacao)
                {
                    case "01":
                        attemptBB.msgret = "pagamento não autorizado";
                        break;
                    case "02":
                        attemptBB.msgret = "erro no processamento da consulta";
                        break;
                    case "03":
                        attemptBB.msgret = "pagamento não localizado";
                        break;
                    case "10":
                        attemptBB.msgret = "campo ”idConv” inválido ou nulo";
                        break;
                    case "11":
                        attemptBB.msgret = "valor informado é inválido, nulo ou não confere com o valor registrado";
                        break;
                    case "12":
                        attemptBB.msgret = "campo “refTran” inválido ou nulo";
                        break;
                    case "99":
                        attemptBB.msgret = "operação cancelada pelo cliente";
                        break;
                    default:
                        attemptBB.msgret = "erro desconhecido";
                        break;
                }
            }
            else
            {
                attemptBB.msgret = "campo situação retornou um valor nulo";
            }
        }
        else
        {
            attemptBB.situacao = "-1";
            attemptBB.msgret = post.Response;
        }

        if (!redirectToFinalization)
        {
            attemptBB.bbpagStatus = (byte)PaymentAttemptBBPagStatus.End;
            attempt.status = (int)PaymentAttemptStatus.NotPaid;
            attempt.lastUpdate = DateTime.Now;
            attempt.returnMessage = attemptBB.msgret;

            attemptBB.TruncateStringFields();
            attempt.TruncateStringFields();
            
            DataFactory.PaymentAttempt().Update(attempt);
            DataFactory.PaymentAttemptBB().Update(attemptBB);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

            http = (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https");
            server = Request.ServerVariables["SERVER_NAME"];
            path = Request.ServerVariables["PATH_INFO"].Replace("notifica.aspx", "");

            redirect = new ClientHttpRequisition();
            redirect.Url = String.Format("{0}://{1}{2}/naoaprovada.aspx", http, server, path);
            redirect.Target = "frameRetorno";
            redirect.Script = "parent.close();";
            redirect.Parameters.Add("id", attempt.paymentAttemptId.ToString());
            redirect.Send();
        }
    }
}
