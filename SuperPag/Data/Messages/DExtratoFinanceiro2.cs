using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("ProcExtratoFinanceiro2")]
    public class DExtratoFinanceiro2 : DataMessageBase
	{
        public DExtratoFinanceiro2() { }

		public class Fields
		{
			public const string pedido = "pedido";
            public const string arp = "arp";
            public const string tid = "tid";
            public const string numautor = "numautor";
            public const string numcv = "numcv";
            public const string numautent = "numautent";
            public const string nossoNumero = "nossoNumero";
            public const string valorParcela = "valorParcela";
            public const string valorPedido = "valorPedido";
            public const string valorRecebido = "valorRecebido";
            public const string statusConciliacao = "statusConciliacao";
            public const string valorLiq = "valorLiq";
            public const string dataEntradaPedido = "dataEntradaPedido";
            public const string dataParcela = "dataParcela";
            public const string dataPagamento = "dataPagamento";
            public const string statusCobranca = "statusCobranca";
            public const string formaPagamento = "formaPagamento";
            public const string qtdParcelas = "qtdParcelas";
            public const string statusPostFinalizacao = "statusPostFinalizacao";
            public const string statusPostPagamento = "statusPostPagamento";
		}

        public string pedido;
        public string arp;
        public string tid;
        public string numautor;
        public string numcv;
        public string numautent;
        public string nossoNumero;
        public string valorParcela;
        public string valorPedido;
        public string valorRecebido;
        public string statusConciliacao;
        public string valorLiq;
        public string dataEntradaPedido;
        public string dataParcela;
        public string dataPagamento;
        public string statusCobranca;
        public string formaPagamento;
        public string qtdParcelas;
        public string statusPostFinalizacao;
        public string statusPostPagamento;
	}
}