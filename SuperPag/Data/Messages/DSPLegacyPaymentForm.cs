using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("SPLegacyPaymentForm")]
	public class DSPLegacyPaymentForm : DataMessageBase
	{

		public DSPLegacyPaymentForm() {}

		public class Fields
		{
			public const string storeId = "storeId";
			public const string paymentFormId = "paymentFormId";
			public const string ucInstructions = "ucInstructions";
		}

		[PrimaryKey]
		public int storeId;
		[PrimaryKey]
		public int paymentFormId;
		public string ucInstructions;
	}
}
