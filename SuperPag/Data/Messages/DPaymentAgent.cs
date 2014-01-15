using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgent")]
	public class DPaymentAgent : DataMessageBase
	{

		public DPaymentAgent() {}

		public class Fields
		{
			public const string paymentAgentId = "paymentAgentId";
			public const string name = "name";
			public const string webPage = "webPage";
		}

		[PrimaryKey]
		public int paymentAgentId;
		public string name;
		public string webPage;
	}
}
