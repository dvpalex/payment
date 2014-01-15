using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MRelatorioContabilizacaoMatera
    {
        #region CONSTRUTORES

        public MRelatorioContabilizacaoMatera(
                                                string NumBanco,
                                                string NomeBanco,
                                                string ContaCorrente,
                                                string ContaTransit,
                                                DateTime? DataProcessamento,
                                                DateTime? DataOperacao,
                                                DateTime? DataCredito,
                                                Decimal Valor,
                                                Decimal SubTotalBanco
                                            )
        {
            this.NumBanco = NumBanco;
            this.NomeBanco = NomeBanco;
            this.ContaCorrente = ContaCorrente;
            this.ContaTransit = ContaTransit;
            this.DataProcessamento = DataProcessamento;
            this.DataOperacao = DataOperacao;
            this.DataCredito = DataCredito;
            this.Valor = Valor;
            this.SubTotalBanco = SubTotalBanco;
        }

        #endregion

        #region PROPRIEDADES
        private string _NumBanco;
        private string _NomeBanco;
        private string _ContaCorrente;
        private string _ContaTransit;
        private DateTime? _DataProcessamento;
        private DateTime? _DataOperacao;
        private DateTime? _DataCredito;
        private Decimal _Valor;
        private Decimal _SubTotalBanco;
        #endregion

        #region MEMBRO DE DADOS
        public string NumBanco
        {
            get { return _NumBanco; }
            set { _NumBanco = value; }
        }

        public string NomeBanco
        {
            get { return _NomeBanco; }
            set { _NomeBanco = value; }
        }

        public string ContaCorrente
        {
            get { return _ContaCorrente; }
            set { _ContaCorrente = value; }
        }

        public string ContaTransit
        {
            get { return _ContaTransit; }
            set { _ContaTransit = value; }
        }

        public DateTime? DataProcessamento
        {
            get { return _DataProcessamento; }
            set { _DataProcessamento = value; }
        }

        public DateTime? DataOperacao
        {
            get { return _DataOperacao; }
            set { _DataOperacao = value; }
        }

        public DateTime? DataCredito
        {
            get { return _DataCredito; }
            set { _DataCredito = value; }
        }

        public Decimal Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }

        public Decimal SubTotalBanco
        {
            get { return _SubTotalBanco; }
            set { _SubTotalBanco = value; }
        }
        #endregion
    }
}
