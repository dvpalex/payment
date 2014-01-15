using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace SuperPag.Agents.Boleto
{
    /// <summary>
    /// Summary description for BoletoBradesco
    /// </summary>
    public class BoletoBradesco : Boleto
    {
        private BoletosBancariosInfo _boletosBancariosInfo;

        public BoletoBradesco(BoletosBancariosInfo dadosBoleto)
        {
            _boletosBancariosInfo = dadosBoleto;
        }

        public string ObtemDigitoNossoNumero(string carteira, string nossoNumero)
        {
            int iBase = 7;
            int iMult = 2;
            int iDigito, iValor = 0;

            string numero = carteira.Trim() + nossoNumero.Trim();

            for (int i = numero.Length - 1; i >= 0; i--)
            {
                if (iMult > iBase)
                    iMult = 2;

                iDigito = Int32.Parse(numero[i].ToString());
                iValor = iValor + (iDigito * iMult);
                iMult++;
            }

            int resto = iValor % 11;
            string iResto = resto.ToString();
            if (resto > 0)
                iResto = (11 - resto).ToString();
            if (iResto == "10")
                iResto = "P";

            return iResto;
        }

        public string ObtemNossoNumero()
        {
            string numero = _boletosBancariosInfo.NossoNumero.Trim().PadLeft(11, '0');
            string carteira = _boletosBancariosInfo.Carteira.PadLeft(2, '0');
            return carteira + "/" + numero + "-" + ObtemDigitoNossoNumero(carteira, numero);
        }

        public string ObtemCodigoBarra()
        {
            string sSaldo = (Math.Truncate(_boletosBancariosInfo.ValorBoleto * 100)).ToString();
            string sFatorVencimento = (_boletosBancariosInfo.CalculaFatorVencimento ? this.FatorVencimento(_boletosBancariosInfo.DataVencimento) : "0");
            string sCedente = _boletosBancariosInfo.ContaCorrente.PadLeft(7, '0');
            string sCodigoBarra = "";

            sCodigoBarra = _boletosBancariosInfo.CodBanco.ToString().PadLeft(3, '0') + _boletosBancariosInfo.CodMoeda.PadLeft(1, '0') + sFatorVencimento.PadLeft(4, '0') + sSaldo.PadLeft(10, '0');
            sCodigoBarra += _boletosBancariosInfo.Agencia.PadLeft(4, '0') + _boletosBancariosInfo.Carteira.PadLeft(2, '0') + _boletosBancariosInfo.NossoNumero.PadLeft(11, '0') + sCedente.PadLeft(7, '0') + "0";

            string sDigitoCodigoBarra = this.DigitoCodigoBarra(sCodigoBarra);

            return sCodigoBarra.Insert(4, sDigitoCodigoBarra);
        }
    }
}