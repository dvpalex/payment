using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgentSetupBradesco")]
	public class DPaymentAgentSetupBradesco : DataMessageBase
	{

		public DPaymentAgentSetupBradesco() {}

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
            public const string bradescoUrl = "bradescoUrl";
			public const string businessNumber = "businessNumber";
			public const string mngLogin = "mngLogin";
			public const string mngPassword = "mngPassword";
            public const string useStoreOrderReference = "useStoreOrderReference";
		}

		[PrimaryKey]
		public int paymentAgentSetupId;
        public string bradescoUrl;
		public string businessNumber;
		public string mngLogin;
		public string mngPassword;
        public bool useStoreOrderReference;
	}
}
