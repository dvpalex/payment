using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAttemptABN")]
	public class DPaymentAttemptABN : DataMessageBase
	{

		public DPaymentAttemptABN() {}

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
			public const string agentOrderReference = "agentOrderReference";
            public const string numControle = "numControle";
            public const string numProposta = "numProposta";
            public const string statusProposta = "statusProposta";
            public const string qtdPrestacao = "qtdPrestacao";
            public const string prestacao = "prestacao";
            public const string tabelaFinanciamento = "tabelaFinanciamento";
            public const string tipoPessoa = "tipoPessoa";
            public const string garantia = "garantia";
            public const string valorEntrada = "valorEntrada";
            public const string dataVencimento = "dataVencimento";
            public const string codRet = "codRet";
            public const string msgRet = "msgRet";
            public const string abnStatus = "abnStatus";
		}

		[PrimaryKey]
		public Guid paymentAttemptId;
		[Identity]
		public int agentOrderReference;
        public decimal numControle;
        public string numProposta;
        public string statusProposta;
        public int qtdPrestacao;
        public decimal prestacao;
        public string tabelaFinanciamento;
        public string tipoPessoa;
        public string garantia;
        public decimal valorEntrada;
        public DateTime dataVencimento;
        public int codRet;
        public string msgRet;
        public int abnStatus;
	}
}
