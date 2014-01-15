using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Agents.Boleto
{
    public class BoletoHSBC : Boleto
    {
        private BoletosBancariosInfo _boletosBancariosInfo;

        public BoletoHSBC(BoletosBancariosInfo dadosBoleto)
        {
            _boletosBancariosInfo = dadosBoleto;
        }

        public string ObtemNossoNumero()
        {
            int tipoIdentificacao = 5;
            if (_boletosBancariosInfo.DataVencimento != null && _boletosBancariosInfo.DataVencimento != DateTime.MinValue)
                tipoIdentificacao = 4;
            string codigoDocumento = _boletosBancariosInfo.NossoNumero.Trim();

            codigoDocumento += CalculaDigitoNossoNumero(codigoDocumento);
            codigoDocumento += tipoIdentificacao;

            long acumulado = long.Parse(codigoDocumento);
            // TODO: É necessário fazer uma prametrização no banco para isso, pois há convênios no HSBC que tem 7 posicoes
            //       e para o calculo nao deve acrescer 8000000. 
            //if (_boletosBancariosInfo.Convenio.Length == 7)
            //    acumulado += 8000000;
            acumulado += int.Parse(_boletosBancariosInfo.Convenio);
            if (tipoIdentificacao == 4)
                acumulado += int.Parse(_boletosBancariosInfo.DataVencimento.ToString("ddMMyy"));
            
            codigoDocumento += CalculaDigitoNossoNumero(acumulado.ToString());

            return codigoDocumento;
        }

        public string ObtemCodigoBarra()
        {
            string sSaldo = (Math.Truncate(_boletosBancariosInfo.ValorBoleto * 100)).ToString();
            string sFatorVencimento = (_boletosBancariosInfo.CalculaFatorVencimento ? this.FatorVencimento(_boletosBancariosInfo.DataVencimento) : "0");

            string sCodigoBarra = _boletosBancariosInfo.CodBanco.ToString().PadLeft(3, '0');
            sCodigoBarra += _boletosBancariosInfo.CodMoeda.PadLeft(1, '0');
            sCodigoBarra += sFatorVencimento.PadLeft(4, '0') + sSaldo.PadLeft(10, '0');
            sCodigoBarra += _boletosBancariosInfo.Convenio.PadLeft(7, '0');
            sCodigoBarra += _boletosBancariosInfo.NossoNumero.PadLeft(13, '0');
            sCodigoBarra += _boletosBancariosInfo.DataVencimento.DayOfYear.ToString("000");
            sCodigoBarra += _boletosBancariosInfo.DataVencimento.Year.ToString("0000").Substring(3,1);
            sCodigoBarra += "2";

            string sDigitoCodigoBarra = this.DigitoCodigoBarra(sCodigoBarra);
            return sCodigoBarra.Insert(4, sDigitoCodigoBarra);
        }

        private int CalculaDigitoNossoNumero(string codigoDocumento)
        {
            int iIndice = 9;
            
            int iAcumula = 0;
            for (int i = 0; i < codigoDocumento.Length; i++)
            {
                iAcumula += (Convert.ToInt32((codigoDocumento.Substring(codigoDocumento.Length - 1 - i, 1))) * iIndice);
                iIndice = iIndice == 2 ? 9 : iIndice - 1;
            }
            return (iAcumula % 11) == 10 ? 0 : iAcumula % 11;
        }
    }
}
