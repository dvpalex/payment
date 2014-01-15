using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAttemptVBVLog")]
	public class DPaymentAttemptVBVLog : DataMessageBase
	{

		public DPaymentAttemptVBVLog() {}

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
            public const string interfaceType = "interfaceType";
            public const string returnDate = "returnDate";
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
		}

		[PrimaryKey]
		public Guid paymentAttemptId;
        public int interfaceType;
        public DateTime returnDate;
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
	}
}
