using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("SPLegacyPaymentGroup")]
	public class DSPLegacyPaymentGroup : DataMessageBase
	{

		public DSPLegacyPaymentGroup() {}

		public class Fields
		{
			public const string storeId = "storeId";
			public const string paymentFormGroupId = "paymentFormGroupId";
			public const string ucInstructions = "ucInstructions";
		}

		[PrimaryKey]
		public int storeId;
		[PrimaryKey]
		public int paymentFormGroupId;
		public string ucInstructions;
	}
}
