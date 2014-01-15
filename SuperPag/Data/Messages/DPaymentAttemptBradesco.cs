using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAttemptBradesco")]
	public class DPaymentAttemptBradesco : DataMessageBase
	{

		public DPaymentAttemptBradesco() {}

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
			public const string agentOrderReference = "agentOrderReference";
            public const string numOrder = "numOrder";
            public const string merchantid = "merchantid";
			public const string tipoPagto = "tipoPagto";
			public const string prazo = "prazo";
			public const string numparc = "numparc";
			public const string valparc = "valparc";
			public const string valtotal = "valtotal";
			public const string cod = "cod";
			public const string ccname = "ccname";
			public const string ccemail = "ccemail";
			public const string cctype = "cctype";
			public const string assinatura = "assinatura";
            public const string bradescoStatus = "bradescoStatus";
		}

		[PrimaryKey]
		public Guid paymentAttemptId;
        [Identity]
		public int agentOrderReference;
        public string numOrder;
        public string merchantid;
        public int tipoPagto;
		public string prazo;
		public string numparc;
		public string valparc;
		public string valtotal;
		public string cod;
		public string ccname;
		public string ccemail;
		public string cctype;
		public string assinatura;
        public int bradescoStatus;
	}
}
