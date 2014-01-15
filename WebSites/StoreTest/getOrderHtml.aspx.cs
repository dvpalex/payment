using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;
using SuperPag.Helper;

namespace StoreTest
{
	public partial class getOrderHtml : System.Web.UI.Page
	{
        private int sitem = 1;
        private decimal spv = 0;
        private decimal sfrete = 0;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            ClientHttpRequisition post = new ClientHttpRequisition();
            post.Url = ConfigurationManager.AppSettings["urlHandshakeHTML"];
            post.Method = "POST";

            post.Parameters.Add("36948FFEF212F5E4", Request["36948FFEF212F5E4"]);
            post.Parameters.Add("91D4C3128BF7DA7F", Request["91D4C3128BF7DA7F"]);

			post.Parameters.Add("SHOW_TELA_FINALIZACAO","1");
			post.Parameters.Add("URLRETORNOLOJA",String.Format("{0}://{1}{2}/storeReturn.aspx", (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https"), Request.ServerVariables["SERVER_NAME"], Request.ServerVariables["PATH_INFO"].Replace("getOrderHtml.aspx", "")));
			post.Parameters.Add("URLPOSTLOJA",String.Format("{0}://{1}{2}/orderConfirmHtml.aspx", (Request.ServerVariables["HTTPS"] == "off" ? "http" : "https"), Request.ServerVariables["SERVER_NAME"], Request.ServerVariables["PATH_INFO"].Replace("getOrderHtml.aspx", "")));
			
			post.Parameters.Add("cpf","12312312387");
			post.Parameters.Add("nome","Teste e-financial");
            post.Parameters.Add("email","rodolfo.camara@e-financial.com.br");
			post.Parameters.Add("nasc","19830319");
			post.Parameters.Add("fone","1138189000");
			post.Parameters.Add("sexo","M");
            
			//Dados de entrega
			post.Parameters.Add("nome_d","Teste e-financial");
			post.Parameters.Add("logradouro_d","Rua");
			post.Parameters.Add("endereco_d","Hungia");
			post.Parameters.Add("numero_d","1100");
            post.Parameters.Add("complemento_d", "ap. 666");
			post.Parameters.Add("cep_d","01455000");
			post.Parameters.Add("cidade_d","São Paulo");
			post.Parameters.Add("estado_d","SP");
			
			//Dados de cobranca
			post.Parameters.Add("logradouro","Rua");
			post.Parameters.Add("endereco","Fulaninho");
			post.Parameters.Add("numero","666");
            post.Parameters.Add("complemento", "ap. 666");
			post.Parameters.Add("cep","01455000");
			post.Parameters.Add("cidade","São Paulo");
			post.Parameters.Add("estado","SP");

            post.Parameters.Add("param_op1_ped", "teste1");
            post.Parameters.Add("param_op2_ped", "teste2");
            post.Parameters.Add("param_op3_ped", "teste3");

            post.Parameters.Add("sitem", sitem.ToString());
			for (int i = 1; i <= sitem; i++)
			{
				post.Parameters.Add("Qtd_" + i,"1");
				post.Parameters.Add("Cod_" + i,"COD" + i);
				post.Parameters.Add("Des_" + i,"DESC" + i);
				post.Parameters.Add("Val_" + i,"100");
                spv += 100;
			}

            post.Parameters.Add("sfrete", sfrete.ToString());
            post.Parameters.Add("spv", (spv + sfrete).ToString());

            post.Send();
		}
	}
}
