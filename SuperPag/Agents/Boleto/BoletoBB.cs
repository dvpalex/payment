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
    /// Summary description for BoletoBB
    /// </summary>
    public class BoletoBB : Boleto
    {
        private BoletosBancariosInfo _boletosBancariosInfo;

        public BoletoBB(BoletosBancariosInfo dadosBoleto)
        {
            _boletosBancariosInfo = dadosBoleto;
        }

        public string ObtemDigitoNossoNumero(string nossoNumero)
        {
            int iBase = 7;
            int iMult = 2;
            int iDigito, iValor = 0;

            for (int i = nossoNumero.Length - 1; i >= 0; i--)
            {
                if (iMult > iBase)
                    iMult = 2;

                iDigito = Int32.Parse(nossoNumero[i].ToString());
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
            string numero = _boletosBancariosInfo.NossoNumero.Trim().PadLeft(17, '0');
            return numero + "-" + ObtemDigitoNossoNumero(numero);
        }

        public string ObtemCodigoBarra()
        {
            string sSaldo = (Math.Truncate(_boletosBancariosInfo.ValorBoleto * 100)).ToString();
            string sFatorVencimento = (_boletosBancariosInfo.CalculaFatorVencimento ? this.FatorVencimento(_boletosBancariosInfo.DataVencimento) : "0");
            string sCodigoBarra = "";

            sCodigoBarra = _boletosBancariosInfo.CodBanco.ToString().PadLeft(3, '0') + _boletosBancariosInfo.CodMoeda.PadLeft(1, '0') + sFatorVencimento.PadLeft(4, '0') + sSaldo.PadLeft(10, '0');
            sCodigoBarra += _boletosBancariosInfo.Convenio.PadLeft(6, '0') + _boletosBancariosInfo.NossoNumero.Trim().PadLeft(17, '0') + "21";

            string sDigitoCodigoBarra = this.DigitoCodigoBarra(sCodigoBarra);
            return sCodigoBarra.Insert(4, sDigitoCodigoBarra);
        }
    }
}
