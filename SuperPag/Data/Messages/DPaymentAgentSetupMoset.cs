using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgentSetupMoset")]
	public class DPaymentAgentSetupMoset : DataMessageBase
	{

        public DPaymentAgentSetupMoset() { }

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
            public const string merchantId = "merchantId";
            public const string autoCapture = "autoCapture";
		}

		[PrimaryKey]
		public int paymentAgentSetupId;
        public string merchantId;
        public bool autoCapture;
	}
}
