using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAttemptBB")]
	public class DPaymentAttemptBB : DataMessageBase
	{

		public DPaymentAttemptBB() {}

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
			public const string agentOrderReference = "agentOrderReference";
			public const string valor = "valor";
			public const string idConvenio = "idConvenio";
			public const string tipoPagamento = "tipoPagamento";
			public const string dataPagamento = "dataPagamento";
			public const string situacao = "situacao";
            public const string dataSonda = "dataSonda";
            public const string qtdSonda = "qtdSonda";
            public const string sondaOffline = "sondaOffline";
            public const string msgret = "msgret";
			public const string bbpagStatus = "bbpagStatus";
        }

		[PrimaryKey]
		public Guid paymentAttemptId;
		[Identity]
		public int agentOrderReference;
		public decimal  valor;
		public int idConvenio;
		public byte tipoPagamento;
		public DateTime dataPagamento;
		public string situacao;
        public DateTime dataSonda = DateTime.MinValue;
        public int qtdSonda = 0;
        public bool sondaOffline = false;
        public string msgret;
		public byte bbpagStatus;
        
        public void TruncateStringFields()
        {
            if (msgret != null && msgret.Length > 200)
                msgret = msgret.Substring(0, 200);
        }
    }
}
