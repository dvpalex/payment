using System;
using System.Collections.Generic;
using System.Text;



namespace SuperPag.Business.Messages
{
    public class MRelatorioEnvioCSU : MCSUControleSaidaContaCorrente
    {
        #region CONSTRUTORES

        public MRelatorioEnvioCSU(DateTime DataGeracaoLote, string NomeArquivoEnviado,
                                                int QtdeDetalhes, decimal ValorTotalDetalhes,
                                                int StatusProcessamento, string DescricaoStatus
                                        )
        {
            this.DataGeracaoLote = DataGeracaoLote;
            this.NomeArquivoEnviado = NomeArquivoEnviado;
            this.QtdeDetalhes = QtdeDetalhes;
            this.ValorTotalDetalhes = ValorTotalDetalhes;
            this.DescricaoStauts = DescricaoStatus;
        }

        public MRelatorioEnvioCSU(  DateTime DataArquivo,
                                    DateTime DataGeracaoLote, string NomeArquivoEnviado,
                                    int QtdeDetalhes, decimal ValorTotalDetalhes,
                                    int StatusProcessamento, string DescricaoStatus
                                        )
        {
            this.DataArquivo = DataArquivo;
            this.DataGeracaoLote = DataGeracaoLote;
            this.NomeArquivoEnviado = NomeArquivoEnviado;
            this.QtdeDetalhes = QtdeDetalhes;
            this.ValorTotalDetalhes = ValorTotalDetalhes;
            this.DescricaoStauts = DescricaoStatus;
        }

        #endregion

        #region MEMBROS DE DADOS

        private string _DescricaoStatus;
        private DateTime _DataArquivo;

        #endregion

        #region PROPRIEDADES

        public string DescricaoStauts
        {
            get { return this._DescricaoStatus; }
            set { this._DescricaoStatus = value; }
        }

        public DateTime DataArquivo
        {
            get { return this._DataArquivo; }
            set { this._DataArquivo = value; }
        }

        #endregion
    }
}
