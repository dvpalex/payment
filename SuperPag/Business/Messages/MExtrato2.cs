using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Framework;
using SuperPag.Data.Messages;

namespace SuperPag.Business.Messages
{
 
    [Serializable()]
    public class MExtrato2 : Message
    {
        private string _pedido;
        private string _arp;
        private string _tid;
        private string _numautor;
        private string _numcv;
        private string _numautent;
        private string _nossoNumero;
        private string _valorParcela;
        private string _valorPedido;
        private string _valorRecebido;
        private string _statusConciliacao;
        private string _valorLiq;
        private string _dataEntradaPedido;
        private string _dataParcela;
        private string _dataPagamento;
        private string _statusCobranca;
        private string _formaPagamento;
        private string _qtdParcelas;
        private string _statusPostFinalizacao;
        private string _statusPostPagamento;


        [Mapping(DExtratoFinanceiro2.Fields.pedido)]
        public string pedido
	    {
		    get { return _pedido;}
		    set { _pedido = value;}
	    }

        [Mapping(DExtratoFinanceiro2.Fields.arp)]
        public string arp
        {
            get { return _arp; }
            set { _arp = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.tid)]
        public string tid
        {
            get { return _tid; }
            set { _tid = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.numautor)]
        public string numautor
        {
            get { return _numautor; }
            set { _numautor = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.numcv)]
        public string numcv
        {
            get { return _numcv; }
            set { _numcv = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.numautent)]
        public string numautent
        {
            get { return _numautent; }
            set { _numautent = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.nossoNumero)]
        public string nossoNumero
        {
            get { return _nossoNumero; }
            set { _nossoNumero = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.valorParcela)]
        public string valorParcela
        {
            get { return _valorParcela; }
            set { _valorParcela = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.valorPedido)]
        public string valorPedido
        {
            get { return _valorPedido; }
            set { _valorPedido = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.valorRecebido)]
        public string valorRecebido
        {
            get { return _valorRecebido; }
            set { _valorRecebido = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.statusConciliacao)]
        public string statusConciliacao
        {
            get { return _statusConciliacao; }
            set { _statusConciliacao = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.valorLiq)]
        public string valorLiq
        {
            get { return _valorLiq; }
            set { _valorLiq = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.dataEntradaPedido)]
        public string dataEntradaPedido
        {
            get { return _dataEntradaPedido; }
            set { _dataEntradaPedido = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.dataParcela)]
        public string dataParcela
        {
            get { return _dataParcela; }
            set { _dataParcela = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.dataPagamento)]
        public string dataPagamento
        {
            get { return _dataPagamento; }
            set { _dataPagamento = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.statusCobranca)]
        public string statusCobranca
        {
            get { return _statusCobranca; }
            set { _statusCobranca = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.formaPagamento)]
        public string formaPagamento
        {
            get { return _formaPagamento; }
            set { _formaPagamento = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.qtdParcelas)]
        public string qtdParcelas
        {
            get { return _qtdParcelas; }
            set { _qtdParcelas = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.statusPostFinalizacao)]
        public string statusPostFinalizacao
        {
            get { return _statusPostFinalizacao; }
            set { _statusPostFinalizacao = value; }
        }

        [Mapping(DExtratoFinanceiro2.Fields.statusPostPagamento)]
        public string statusPostPagamento
        {
            get { return _statusPostPagamento; }
            set { _statusPostPagamento = value; }
        }

    }

    [Serializable]
    [CollectionOf(typeof(MExtrato2))]
    public class MCExtrato2 : MessageCollection
    {
    }
}
