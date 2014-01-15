using System;
using System.Collections.Generic;
using System.Text;



namespace SuperPag.Business.Messages
{
    public class MRelatorioRecebimentoCSU : MCSUControleEntradaContaCorrente
    {
        //#region CONSTRUTORES

        //public MRelatorioRecebimentoCSU (       DateTime DataCapturaArquivo, string NomeArquivoCapturado,
        //                                        int QtdeDetalhes, decimal ValorTotalDetalhes,
        //                                        int StatusProcessamento, string DescricaoStatus,
        //                                        int QtdePagas,decimal ValorPagas, 
        //                                        int QtdeNaoPagas, decimal ValorNaoPagas,
        //                                        int QtdeReceber, decimal ValorReceber
        //                                )
        //{

        //    this.DataCapturaArquivo = DataCapturaArquivo; 
        //    this.NomeArquivoCapturado = NomeArquivoCapturado;
        //    this.QtdeDetalhes = QtdeDetalhes;
        //    this.ValorTotalDetalhes = ValorTotalDetalhes;
        //    this.DescricaoStatus = DescricaoStatus;
        //    this.QtdePagas = QtdePagas;
        //    this.ValorPagas = ValorPagas;
        //    this.QtdeNaoPagas = QtdeNaoPagas;
        //    this.ValorNaoPagas = ValorNaoPagas;
        //    this.QtdeReceber = QtdeReceber;
        //    this.ValorReceber = ValorReceber;

        //}

        //#endregion

        //#region MEMBROS DE DADOS
        //// Adiciona os Campo para o Relatorio
        //public int QtdePagas
        //{
        //    get { return this._QtdePagas; }
        //    set { _QtdePagas = value; }
        //}

        //public decimal ValorPagas
        //{
        //    get { return this._ValorPagas; }
        //    set { _ValorPagas = value; }
        //}

        //public int QtdeNaoPagas
        //{
        //    get { return this._QtdeNaoPagas; }
        //    set { _QtdeNaoPagas = value; }
        //}

        //public decimal ValorNaoPagas
        //{
        //    get { return this._ValorNaoPagas; }
        //    set { _ValorNaoPagas = value; }
        //}


        //public int QtdeReceber
        //{
        //    get { return this._QtdeReceber; }
        //    set { _QtdeReceber = value; }
        //}

        //public decimal ValorReceber
        //{
        //    get { return this._ValorReceber; }
        //    set { _ValorReceber = value; }
        //}

        //public string DescricaoStatus
        //{
        //    get { return this._DescricaoStatus; }
        //    set { _DescricaoStatus = value; }      
        //}

        //#endregion

        //#region PROPRIEDADES
        //private int _QtdePagas;
        //private decimal _ValorPagas;
        //private int _QtdeNaoPagas;
        //private decimal _ValorNaoPagas;
        //private int _QtdeReceber;
        //private decimal _ValorReceber;
        //private string _DescricaoStatus;
        //#endregion

        // Alteração : Jair Jersey Marinho - 22/10/2008

        #region CONSTRUTORES

        public MRelatorioRecebimentoCSU(DateTime DataCapturaArquivo, string NomeArquivoCapturado,
                                                int QtdeDetalhes, decimal ValorTotalDetalhes,
                                                int StatusProcessamento, string DescricaoStatus
                                        )
        {

            this.DataCapturaArquivo = DataCapturaArquivo;
            this.NomeArquivoCapturado = NomeArquivoCapturado;
            this.QtdeDetalhes = QtdeDetalhes;
            this.ValorTotalDetalhes = ValorTotalDetalhes;
            this.DescricaoStatus = DescricaoStatus;
        }

        public MRelatorioRecebimentoCSU(DateTime DataArquivo,
                                        DateTime DataCapturaArquivo, string NomeArquivoCapturado,
                                        int QtdeDetalhes, decimal ValorTotalDetalhes,
                                        int StatusProcessamento, string DescricaoStatus
                                        )
        {
            this.DataArquivo = DataArquivo;
            this.DataCapturaArquivo = DataCapturaArquivo;
            this.NomeArquivoCapturado = NomeArquivoCapturado;
            this.QtdeDetalhes = QtdeDetalhes;
            this.ValorTotalDetalhes = ValorTotalDetalhes;
            this.DescricaoStatus = DescricaoStatus;
        }

        #endregion

        #region MEMBROS DE DADOS
        // Adiciona os Campo para o Relatorio


        public string DescricaoStatus
        {
            get { return this._DescricaoStatus; }
            set { _DescricaoStatus = value; }
        }

        #endregion

        #region PROPRIEDADES
        private string _DescricaoStatus;
        #endregion
    }
}
