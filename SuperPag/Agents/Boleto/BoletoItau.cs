using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Agents.Boleto
{
    public class BoletoItau : Boleto
    {
        private BoletosBancariosInfo _boletosBancariosInfo;

        public BoletoItau(BoletosBancariosInfo dadosBoleto)
        {
            _boletosBancariosInfo = dadosBoleto;
        }
        
        public string ObtemDigitoNossoNumero(string ag, string conta, string carteira, string nossoNumero)
        {
            int iAcumula = 0;
            int iIndice = ((nossoNumero.Length % 2) == 0 ? 1 : 2);
            int iResult = 0;
            int iDigito = 0;

            string numero = ag.Trim() + conta.Trim() + carteira.Trim() + nossoNumero.Trim();

            for (int i = 0; i < numero.Length; i++)
            {
                iResult = (Convert.ToInt32((numero.Substring(i, 1))) * iIndice);
                if (iResult >= 10)
                    iResult = Convert.ToInt16(numero.Substring(0, 1)) + Convert.ToInt16(numero.Substring(1, 1));
                
                iAcumula += iResult;

                iIndice = (iIndice == 1 ? 2 : 1);
            }


            iDigito = 10 - (iAcumula % 10);
            iDigito = (iDigito == 10 ? 0 : iDigito);
            
            return iDigito.ToString();
        }

        public string ObtemNossoNumero()
        {
            string numero = _boletosBancariosInfo.NossoNumero.Trim().PadLeft(8, '0');
            string carteira = _boletosBancariosInfo.Carteira.PadLeft(3, '0');
            string ag = _boletosBancariosInfo.Agencia.PadLeft(4, '0');
            string conta = _boletosBancariosInfo.ContaCorrente.PadLeft(5, '0');
            return carteira + "/" + numero + "-" + ObtemDigitoNossoNumero(ag, conta, carteira, numero);
        }

        public string ObtemCodigoBarra()
        {
            string sSaldo = (Math.Truncate(_boletosBancariosInfo.ValorBoleto * 100)).ToString();
            string sFatorVencimento = (_boletosBancariosInfo.CalculaFatorVencimento ? this.FatorVencimento(_boletosBancariosInfo.DataVencimento) : "0");
            string sCodigoBarra = "";

            string numero = _boletosBancariosInfo.NossoNumero.Trim().PadLeft(8, '0');
            string carteira = _boletosBancariosInfo.Carteira.PadLeft(3, '0');
            string ag = _boletosBancariosInfo.Agencia.PadLeft(4, '0');
            string conta = _boletosBancariosInfo.ContaCorrente.PadLeft(5, '0');
            
            sCodigoBarra = _boletosBancariosInfo.CodBanco.ToString().PadLeft(3, '0') + _boletosBancariosInfo.CodMoeda.PadLeft(1, '0') + sFatorVencimento.PadLeft(4, '0') + sSaldo.PadLeft(10, '0');
            sCodigoBarra += carteira + numero + ObtemDigitoNossoNumero(ag, conta, carteira, numero);
            sCodigoBarra += ag + conta + ObtemDigitoNossoNumero(ag, conta, "", "") + "000";

            string sDigitoCamposItau = this.DigitoCodigoBarra(sCodigoBarra.Substring(18, 23));
            sCodigoBarra.Insert(41, sDigitoCamposItau);
            
            string sDigitoCodigoBarra = this.DigitoCodigoBarra(sCodigoBarra);
            return sCodigoBarra.Insert(4, sDigitoCodigoBarra);
        }
    }
}
