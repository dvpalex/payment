using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAttemptItauShopline")]
	public class DPaymentAttemptItauShopline : DataMessageBase
	{

		public DPaymentAttemptItauShopline() {}

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
			public const string agentOrderReference = "agentOrderReference";
			public const string codEmp = "codEmp";
			public const string valor = "valor";
			public const string chave = "chave";
			public const string dc = "dc";
			public const string tipPag = "tipPag";
			public const string sitPag = "sitPag";
			public const string dtPag = "dtPag";
			public const string codAut = "codAut";
			public const string numId = "numId";
			public const string compVend = "compVend";
			public const string tipCart = "tipCart";
            public const string dataSonda = "dataSonda";
            public const string qtdSonda = "qtdSonda";
            public const string sondaOffline = "sondaOffline";
            public const string msgret = "msgret";
			public const string itauStatus = "itauStatus";
		}

		[PrimaryKey]
		public Guid paymentAttemptId;
		[Identity]
		public int agentOrderReference;
		public string codEmp;
		public string valor;
		public string chave;
		public string dc;
		public string tipPag;
		public string sitPag;
		public string dtPag;
		public string codAut;
		public string numId;
		public string compVend;
		public string tipCart;
		public string msgret;
        public DateTime dataSonda = DateTime.MinValue;
        public int qtdSonda = 0;
        public bool sondaOffline = false;
        public byte itauStatus;
        
        public void TruncateStringFields()
        {
            if (msgret != null && msgret.Length > 200)
                msgret = msgret.Substring(0, 200);
        }
    }
}
