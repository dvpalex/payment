using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MPaymentAttemptABN
    {
        public MPaymentAttemptABN(Guid PaymentAttemptId, int AgentOrderReference, decimal NumControle,
                                  string NumProposta, string StatusProposta, int QtdPrestacao, decimal Prestacao,
                                  string TabelaFinanciamento, string TipoPessoa, string Garantia, decimal ValorEntrada,
                                  DateTime DataVencimento, int CodRet, string MsgRet, int AbnStatus
                                 )
        {
            this.PaymentAttemptId = PaymentAttemptId;
            this.AgentOrderReference = AgentOrderReference;
            this.NumControle = NumControle;
            this.NumProposta = NumProposta;
            this.StatusProposta = StatusProposta;
            this.QtdPrestacao = QtdPrestacao;
            this.Prestacao = Prestacao;
            this.TabelaFinanciamento = TabelaFinanciamento;
            this.TipoPessoa = TipoPessoa;
            this.Garantia = Garantia;
            this.ValorEntrada = ValorEntrada;
            this.DataVencimento = DataVencimento;
            this.CodRet = CodRet;
            this.MsgRet = MsgRet;
            this.AbnStatus = AbnStatus;
        }

        private Guid _paymentAttemptId;
        private int _agentOrderReference;
        private decimal _numControle;
        private string _numProposta;
        private string _statusProposta;
        private int _qtdPrestacao;
        private decimal _prestacao;
        private string _tabelaFinanciamento;
        private string _tipoPessoa;
        private string _garantia;
        private decimal _valorEntrada;
        private DateTime _dataVencimento;
        private int _codRet;
        private string _msgRet;
        private int _abnStatus;

        public Guid PaymentAttemptId
        {
            get { return _paymentAttemptId; }
            set { _paymentAttemptId = value; }
        }
        public int AgentOrderReference
        {
            get { return _agentOrderReference; }
            set { _agentOrderReference = value; }
        }
        public decimal NumControle
        {
            get { return _numControle; }
            set { _numControle = value; }
        }
        public string NumProposta
        {
            get { return _numProposta; }
            set { _numProposta = value; }
        }
        public string StatusProposta
        {
            get { return _statusProposta; }
            set { _statusProposta = value; }
        }
        public int QtdPrestacao
        {
            get { return _qtdPrestacao; }
            set { _qtdPrestacao = value; }
        }
        public decimal Prestacao
        {
            get { return _prestacao; }
            set { _prestacao = value; }
        }
        public string TabelaFinanciamento
        {
            get { return _tabelaFinanciamento; }
            set { _tabelaFinanciamento = value; }
        }
        public string TipoPessoa
        {
            get { return _tipoPessoa; }
            set { _tipoPessoa = value; }
        }
        public string Garantia
        {
            get { return _garantia; }
            set { _garantia = value; }
        }
        public decimal ValorEntrada
        {
            get { return _valorEntrada; }
            set { _valorEntrada = value; }
        }
        public DateTime DataVencimento
        {
            get { return _dataVencimento; }
            set { _dataVencimento = value; }
        }
        public int CodRet
        {
            get { return _codRet; }
            set { _codRet = value; }
        }
        public string MsgRet
        {
            get { return _msgRet; }
            set { _msgRet = value; }
        }
        public int AbnStatus
        {
            get { return _abnStatus; }
            set { _abnStatus = value; }
        }
    }
}
