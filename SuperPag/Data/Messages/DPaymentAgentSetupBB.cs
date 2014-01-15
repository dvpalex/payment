using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgentSetupBB")]
	public class DPaymentAgentSetupBB : DataMessageBase
	{

		public DPaymentAgentSetupBB() {}

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
			public const string agentOrderReferenceCounter = "agentOrderReferenceCounter";
			public const string businessNumber = "businessNumber";
			public const string daysExpirationDate = "daysExpirationDate";
			public const string urlBBPag = "urlBBPag";
			public const string urlBBPagSonda = "urlBBPagSonda";
		}

		[PrimaryKey]
		public int paymentAgentSetupId;
		public int agentOrderReferenceCounter;
		public int businessNumber;
		public int daysExpirationDate;
		public string urlBBPag;
		public string urlBBPagSonda;
	}
}
