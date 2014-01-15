using System;
using System.Collections.Generic;
using System.Text;

// Alteração : Jair Jersey Marinho - 27/10/2008

namespace SuperPag.Business.Messages
{
    public class MRelatorioEnvioCnab : MCnabControleSaida
    {
        #region CONSTRUTORES

        public MRelatorioEnvioCnab( DateTime DataArquivo , string NomeArquivo,
                                    Int32 NumInstituicao, Int32 QtdeDetalhes,
                                    Decimal ValorTotalDetalhes, Int32 Status,
                                    string DSCSTATUS
                            
                                  )
        {
            this.DataArquivo = DataArquivo;
            this.NomeArquivo = NomeArquivo;
            this.NumInstituicao = NumInstituicao;
            this.QtdeDetalhes = QtdeDetalhes;
            this.ValorTotalDetalhes = ValorTotalDetalhes;
            this.Status = Status;
            this.DescricaoStauts = DSCSTATUS;
        }

        public MRelatorioEnvioCnab( DateTime DataTransacao,
                                    DateTime DataArquivo, string NomeArquivo,
                                    Int32 NumInstituicao, Int32 QtdeDetalhes,
                                    Decimal ValorTotalDetalhes, Int32 Status,
                                    string DSCSTATUS

                                  )
        {
            this.DataTransacao = DataTransacao;
            this.DataArquivo = DataArquivo;
            this.NomeArquivo = NomeArquivo;
            this.NumInstituicao = NumInstituicao;
            this.QtdeDetalhes = QtdeDetalhes;
            this.ValorTotalDetalhes = ValorTotalDetalhes;
            this.Status = Status;
            this.DescricaoStauts = DSCSTATUS;
        }

        #endregion

        #region MEMBROS DE DADOS
        public string DescricaoStauts
        {
            get { return this._DescricaoStatus;}
            set { this._DescricaoStatus = value;  }
        }

        public DateTime DataTransacao
        {
            get { return this._DataTransacao; }
            set { this._DataTransacao = value; }
        }
 
        #endregion

        #region PROPRIEDADES
        private string _DescricaoStatus;
        private DateTime _DataTransacao;
        #endregion
    }

}
//public class MRelatorioEnvioCnab : MCnabControleSaida
//{
//    #region CONSTRUTORES

//    public MRelatorioEnvioCnab(DateTime DataArquivo, string NomeArquivo,
//                                Int32 NumInstituicao, Int32 QtdeDetalhes,
//                                Decimal ValorTotalDetalhes, Int32 Status,
//                                string DSCOcorrencia, string DSCSTATUS,
//                                Int32 QtdeTransacoesPagas, Decimal TotalTransacoesPagas,
//                                Int32 QtdeTransacoesRecusadas, Decimal TotalTransacoesRecusadas,
//                                Int32 QtdeTransacoesPendentes, Decimal TotalTransacoesPendente

//                              )
//    {
//        this.DataArquivo = DataArquivo;
//        this.NomeArquivo = NomeArquivo;
//        this.NumInstituicao = NumInstituicao;
//        this.QtdeDetalhes = QtdeDetalhes;
//        this.ValorTotalDetalhes = ValorTotalDetalhes;
//        this.Status = Status;
//        this.DescricaoStauts = DSCSTATUS;
//        this.DescricaoOcorrencia = DSCOcorrencia;
//        this.QtdeTransacoesPagas = QtdeTransacoesPagas;
//        this.TotalTransacoesPagas = TotalTransacoesPagas;
//        this.QtdeTransacoesPendentes = QtdeTransacoesPendentes;
//        this.TotalTransacoesPendente = TotalTransacoesPendente;
//        this.QtdeTransacoesRecusadas = QtdeTransacoesRecusadas;
//        this.TotalTransacoesRecusadas = TotalTransacoesRecusadas;
//    }

//    #endregion

//    #region MEMBROS DE DADOS
//    public string DescricaoStauts
//    {
//        get { return this._DescricaoStatus; }
//        set { this._DescricaoStatus = value; }
//    }

//    public string DescricaoOcorrencia
//    {
//        get { return this._DescricaoOcorrencia; }
//        set { this._DescricaoOcorrencia = value; }
//    }

//    public int QtdeTransacoesPagas
//    {
//        get { return this._QtdeTransacoesPagas; }
//        set { this._QtdeTransacoesPagas = value; }
//    }

//    public int QtdeTransacoesRecusadas
//    {
//        get { return this._QtdeTransacoesRecusadas; }
//        set { this._QtdeTransacoesRecusadas = value; }
//    }

//    public int QtdeTransacoesPendentes
//    {
//        get { return this._QtdeTransacoesPendentes; }
//        set { this._QtdeTransacoesPendentes = value; }
//    }

//    public Decimal TotalTransacoesPagas
//    {
//        get { return this._TotalTransacoesPagas; }
//        set { this._TotalTransacoesPagas = value; }
//    }

//    public Decimal TotalTransacoesRecusadas
//    {
//        get { return this._TotalTransacoesRecusadas; }
//        set { this._TotalTransacoesRecusadas = value; }
//    }

//    public Decimal TotalTransacoesPendente
//    {
//        get { return this._TotalTransacoesPendente; }
//        set { this._TotalTransacoesPendente = value; }
//    }

//    #endregion

//    #region PROPRIEDADES
//    private string _DescricaoStatus;
//    private string _DescricaoOcorrencia;
//    private int _QtdeTransacoesPagas;
//    private int _QtdeTransacoesRecusadas;
//    private int _QtdeTransacoesPendentes;
//    private decimal _TotalTransacoesPagas;
//    private decimal _TotalTransacoesRecusadas;
//    private decimal _TotalTransacoesPendente;
//    #endregion
//}
