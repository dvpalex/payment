using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgentSetupVBV")]
	public class DPaymentAgentSetupVBV : DataMessageBase
	{

		public DPaymentAgentSetupVBV() {}

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
			public const string businessNumber = "businessNumber";
            public const string autoCapture = "autoCapture";
		}

		[PrimaryKey]
		public int paymentAgentSetupId;
		public int businessNumber;
        public bool autoCapture;
	}
}
