using System;
using System.Collections.Generic;
using System.Text;



namespace SuperPag.Business.Messages
{
    public class MRelatorioResumoTransacoes : MPaymentAttemptContaCorrente
    {
        #region CONSTRUTORES

        // Alteração - Jair Jersey Marinho - 22/10/2008
        // Incluindo Arquivo

        public MRelatorioResumoTransacoes   (
                                                Int32 NumInstituicao, string NomeArquivo,
                                                Int32 QtdeTransacoes, Decimal TotalTransacoes,
                                                Int32 QtdeTransacoesPagas, Decimal TotalTransacoesPagas,
                                                Int32 QtdeTransacoesRecusadas, Decimal TotalTransacoesRecusadas,
                                                Int32 QtdeTransacoesPendentes, Decimal TotalTransacoesPendentes
                                            )
        {
            this.NumInstituicao = NumInstituicao;
            this.NomeArquivo = NomeArquivo;
            this.QtdeTransacoes = QtdeTransacoes;
            this.TotalTransacoes = TotalTransacoes;
            this.QtdeTransacoesPagas = QtdeTransacoesPagas;
            this.TotalTransacoesPagas = TotalTransacoesPagas;
            this.QtdeTransacoesReceber= QtdeTransacoesPendentes;
            this.TotalTransacoesReceber = TotalTransacoesPendentes;
            this.QtdeTransacoesRecusadas = QtdeTransacoesRecusadas;
            this.TotalTransacoesRecusadas = TotalTransacoesRecusadas;

            if (this.QtdeTransacoes > 0)
            {
                this.Devolucao = Convert.ToDecimal((this.QtdeTransacoesRecusadas * 100) / (this.QtdeTransacoes));
            }
            else
            {
                this.Devolucao = 0;
            }

            if (this.Devolucao > 30)
                this.Semafaro = 1; //Vermelho
            else if (this.Devolucao <= 30 && this.Devolucao > 10)
                this.Semafaro = 2; //Amarelo
            else
                this.Semafaro = 3;//Verde


        }

        #endregion

        #region MEMBROS DE DADOS

        public string NomeArquivo
        {
            get { return this._NomeArquivo; }
            set { this._NomeArquivo = value; }
        }

        public Int32 QtdeTransacoes
        {
            get { return this._QtdeTransacoes; }
            set { this._QtdeTransacoes = value; }
        }

        public Decimal TotalTransacoes
        {
            get { return this._TotalTransacoes; }
            set { this._TotalTransacoes = value; }
        }

        public Int32 QtdeTransacoesPagas
        {
            get { return this._QtdeTransacoesPagas; }
            set { this._QtdeTransacoesPagas = value; }
        }

        public Decimal TotalTransacoesPagas
        {
            get { return this._TotalTransacoesPagas; }
            set { this._TotalTransacoesPagas = value; }
        }

        public Int32 QtdeTransacoesRecusadas
        {
            get { return this._QtdeTransacoesRecusadas; }
            set { this._QtdeTransacoesRecusadas = value; }
        }

        public Decimal TotalTransacoesRecusadas
        {
            get { return this._TotalTransacoesRecusadas; }
            set { this._TotalTransacoesRecusadas = value; }
        }

        public Int32 QtdeTransacoesReceber
        {
            get { return this._QtdeTransacoesReceber; }
            set { this._QtdeTransacoesReceber = value; }
        }


        public Decimal TotalTransacoesReceber
        {
            get { return this._TotalTransacoesReceber; }
            set { this._TotalTransacoesReceber = value; }
        }

        public Int32 Semafaro
        {
            get { return this._Semafaro; }
            set { this._Semafaro = value; }
        }


        public Decimal Devolucao
        {
            get { return this._Devolucao; }
            set { this._Devolucao = value; }
        }

        public Int32 NumInstituicao
        {
            get { return this._NumInstituicao; }
            set { this._NumInstituicao = value; }
        }
        #endregion

        #region PROPRIEDADES
        private string _NomeArquivo;
        private Int32 _QtdeTransacoes;
        private Decimal _TotalTransacoes;
        private Int32 _QtdeTransacoesPagas;
        private Decimal _TotalTransacoesPagas;
        private Int32 _QtdeTransacoesRecusadas;
        private Decimal _TotalTransacoesRecusadas;
        private Int32 _QtdeTransacoesReceber;
        private Decimal _TotalTransacoesReceber;
        private Decimal _Devolucao;
        private Int32 _Semafaro;
        private Int32 _NumInstituicao;
        #endregion
    }
}
