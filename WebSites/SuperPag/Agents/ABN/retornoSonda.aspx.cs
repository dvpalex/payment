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

public partial class Agents_ABN_retornoSonda : System.Web.UI.Page
{
    int ret01;
    string mensagem;
    string status;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ParseReturn();
            Response.Write(ReturnXmlData());
        }
        catch (Exception ex)
        {
            status = "ER";
            mensagem = ex.Message;
            Response.Write(ReturnXmlData());
        }
    }
    public string ReturnXmlData()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.AppendChild(xmlDoc.CreateElement("return"));

        XmlElement elemRet01 = xmlDoc.CreateElement("ret01");
        elemRet01.Value = ret01.ToString();
        xmlDoc.DocumentElement.AppendChild(elemRet01);

        XmlElement elemStatus = xmlDoc.CreateElement("status");
        elemStatus.Value = status;
        xmlDoc.DocumentElement.AppendChild(elemStatus);

        XmlElement elemMessage = xmlDoc.CreateElement("message");
        elemMessage.Value = mensagem;
        xmlDoc.DocumentElement.AppendChild(elemMessage);

        return xmlDoc.CreateXmlDeclaration("1.0", "iso-8859-1", null).OuterXml + xmlDoc.OuterXml;
    }
    public void ParseReturn()
    {
        if (String.IsNullOrEmpty(Request["RET01"]))
        {
            status = "ER";
            mensagem = "Não houve código de retorno.";
            return;
        }
        else if (!Int32.TryParse(Request["RET01"],out  ret01))
        {
            status = "ER";
            mensagem = "Código de retorno desconhecido (não numérico).";
            return;
        }

        switch (ret01)
        {
            case 0:
                if (String.IsNullOrEmpty(Request["RET02"].Trim()))
                    throw new Exception("Erro desconhecido.");

                status = "ER";
                mensagem = "Erro de Processamento: ";

                switch (Request["RET02"].Trim())
                {
                    case "-8001":
                        mensagem += "Identificação do certificado da loja virtual";
                        break;
                    case "-8002":
                        mensagem += "Versão de layout";
                        break;
                    case "-8009":
                        mensagem += "CGC/CPF";
                        break;
                    case "-8029":
                        mensagem += "Código identificador da proposta no FLV";
                        break;
                    case "-8501":
                        mensagem += "Código identificador da proposta no FLV incompatível com certificado do lojista";
                        break;
                    case "-8502":
                        mensagem += "Código identificador da proposta no FLV incompatível com CGC/CPF do cliente / empresa";
                        break;
                    case "-8504":
                        mensagem += "Situação da proposta com problemas (comunique-se com o ABN)";
                        break;
                    case "-9000":
                        mensagem += "Erro de infra-estrutura no ABN AMRO Bank.";
                        break;
                    default:
                        mensagem += "Erro desconhecido.";
                        break;
                }
                break;
            case 1:
                status = "AP";
                mensagem = "Proposta Aprovada.";
                break;
            case 2:
                status = "AN";
                mensagem = "Proposta Em Análise.";
                break;
            case 3:
                status = "CA";
                mensagem = "Proposta Cancelada.";
                break;
            case 4:
                status = "EF";
                mensagem = "Proposta Efetivada.";
                break;
            case 5:
                status = "RE";
                mensagem = "Proposta Recusada.";
                break;
            default:
                status = "ER";
                mensagem = "Código de retorno desconhecido.";
                break;
        }
    }

    //Definicao Smartpag:
    //AN = Em análise
    //AP = Aprovada
    //EF = Efetivada
    //CA = Cancelada
    //RE = Recusada
    //ER = Erro
}
