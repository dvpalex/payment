using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("Billing")]
	public class DBilling : DataMessageBase
	{

		public DBilling() {}

		public class Fields
		{
			public const string billingId = "billingId";
			public const string billingGroup = "billingGroup";
			public const string orderId = "orderId";
			public const string isInfinityRecurrency = "isInfinityRecurrency";
			public const string recurrencyPaymentFormId = "recurrencyPaymentFormId";
			public const string recurrencyType = "recurrencyType";
		}

		[PrimaryKey]
		public int billingId;
		public int billingGroup;
		public long orderId;
		public bool isInfinityRecurrency;
		public int recurrencyPaymentFormId;
		public int recurrencyType;
	}
}
