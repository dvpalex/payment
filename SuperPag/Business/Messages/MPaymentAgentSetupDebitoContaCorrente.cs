using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Data.Messages
{
    public class MPaymentAgentSetupDebitoContaCorrente
    {
        public MPaymentAgentSetupDebitoContaCorrente(int PaymentAgentSetupId, string Path, BankNumber NumBanco,
                                                     string Layout, string Versao, DateTime Data, string CodConvenio,
                                                     string NEmpresa, string NBanco, int NumSeq, string Carteira,
                                                     string Agencia, string ContaCorrente
                                                    )
        {
            this.PaymentAgentSetupId = PaymentAgentSetupId;
            this.Path = Path;
            this.NumBanco = NumBanco;
            this.Layout = Layout;
            this.Versao = Versao;
            this.Data = Data;
            this.CodConvenio = CodConvenio;
            this.NEmpresa = NEmpresa;
            this.NBanco = NBanco;
            this.NumSeq = NumSeq;
            this.Carteira = Carteira;
            this.Agencia = Agencia;
            this.ContaCorrente = ContaCorrente;
        }
        public MPaymentAgentSetupDebitoContaCorrente(Int32 NumBanco,string NBanco
                                                    )
        {
            this.numBanc = NumBanco;
            this.NBanco = NBanco;
        }
        private int _paymentAgentSetupId;
        private string _Path;
        private Int32 _numBanc;
        private BankNumber _NumBanco;
        private string _Layout;
        private string _Versao;
        private DateTime _Data;
        private string _CodConvenio;
        private string _NEmpresa;
        private string _NBanco;
        private int _NumSeq;
        private string _Carteira;
        private string _Agencia;
        private string _ContaCorrente;

        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }
        public BankNumber NumBanco
        {
            get { return _NumBanco; }
            set { _NumBanco = value; }
        }
        public Int32 numBanc
        {
            get { return _numBanc; }
            set { _numBanc = value; }
        }
        /// <summary>
        /// Tamanho do arquivo
        /// </summary>
        public string Layout
        {
            get { return _Layout; }
            set { _Layout = value; }
        }
        public string Versao
        {
            get { return _Versao; }
            set { _Versao = value; }
        }
        /// <summary>
        /// Data da versão do arquivo
        /// </summary>
        public DateTime Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
        /// <summary>
        /// Código do convênio
        /// </summary>
        public string CodConvenio
        {
            get { return _CodConvenio; }
            set { _CodConvenio = value; }
        }
        /// <summary>
        /// Nome da Enpresa
        /// </summary>
        public string NEmpresa
        {
            get { return _NEmpresa; }
            set { _NEmpresa = value; }
        }
        /// <summary>
        /// Nome do Banco
        /// </summary>
        public string NBanco
        {
            get { return _NBanco; }
            set { _NBanco = value; }
        }
        /// <summary>
        /// Número sequencial do arquivo : Este numero devera evoluir de 1 em 1 para cada arquivo gerado.
        /// </summary>
        public int NumSeq
        {
            get { return _NumSeq; }
            set { _NumSeq = value; }
        }
        /// <summary>
        /// Carteira da empresa no banco
        /// </summary>
        public string Carteira
        {
            get { return _Carteira; }
            set { _Carteira = value; }
        }
        /// <summary>
        /// Agencia da empresa no banco
        /// </summary>
        public string Agencia
        {
            get { return _Agencia; }
            set { _Agencia = value; }
        }
        /// <summary>
        /// Conta da empresa no banco.
        /// </summary>
        public string ContaCorrente
        {
            get { return _ContaCorrente; }
            set { _ContaCorrente = value; }
        }
    }
}
