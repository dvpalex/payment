using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SuperPag.Agents.Boleto
{
    /// <summary>
    /// Summary description for BoletosBancariosInfo
    /// </summary>
    public class BoletosBancariosInfo
    {
        public BoletosBancariosInfo()
        {
        }

        public int CodBanco;
        public string Carteira;
        public string Convenio;
        public string Agencia;
        public string ContaCorrente;
        public string CodMoeda;
        public decimal ValorBoleto;
        public DateTime DataVencimento;
        public string NossoNumero;
        public string CodigoPedidoLoja;
        public bool CalculaFatorVencimento;
    }
}