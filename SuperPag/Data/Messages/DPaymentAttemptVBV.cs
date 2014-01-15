using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAttemptVBV")]
	public class DPaymentAttemptVBV : DataMessageBase
	{

		public DPaymentAttemptVBV() {}

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
			public const string tidMaster = "tidMaster";
			public const string tid = "tid";
			public const string lr = "lr";
			public const string arp = "arp";
			public const string ars = "ars";
			public const string vbvOrderId = "vbvOrderId";
			public const string price = "price";
			public const string free = "free";
			public const string pan = "pan";
			public const string bank = "bank";
			public const string authent = "authent";
			public const string cap = "cap";
            public const string dataSonda = "dataSonda";
            public const string qtdSonda = "qtdSonda";
            public const string sondaOffline = "sondaOffline";
            public const string vbvStatus = "vbvStatus";
		}

		[PrimaryKey]
		public Guid paymentAttemptId;
		public string tidMaster;
		public string tid;
		public decimal lr;
		public int arp;
		public string ars;
		public string vbvOrderId;
		public int price;
		public string free;
		public string pan;
		public int bank;
		public int authent;
		public string cap;
        public DateTime dataSonda = DateTime.MinValue;
        public int qtdSonda = 0;
        public bool sondaOffline = false;
        public byte vbvStatus;
	}
}
