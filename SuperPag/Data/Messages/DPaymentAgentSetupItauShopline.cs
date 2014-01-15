using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgentSetupItauShopline")]
	public class DPaymentAgentSetupItauShopline : DataMessageBase
	{

		public DPaymentAgentSetupItauShopline() {}

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
			public const string criptoKey = "criptoKey";
			public const string businessKey = "businessKey";
			public const string urlItau = "urlItau";
			public const string urlItauSonda = "urlItauSonda";
		}

		[PrimaryKey]
		public int paymentAgentSetupId;
		public string criptoKey;
		public string businessKey;
		public string urlItau;
		public string urlItauSonda;
	}
}
