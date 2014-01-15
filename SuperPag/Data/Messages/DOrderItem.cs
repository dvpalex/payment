using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("OrderItem")]
	public class DOrderItem : DataMessageBase
	{

		public DOrderItem() {}

		public class Fields
		{
			public const string orderItemId = "orderItemId";
			public const string orderId = "orderId";
			public const string itemType = "itemType";
			public const string itemNumber = "itemNumber";
			public const string itemCode = "itemCode";
			public const string itemDescription = "itemDescription";
			public const string itemQuantity = "itemQuantity";
			public const string itemValue = "itemValue";
		}

		[PrimaryKey]
		[Identity]
		public long orderItemId;
		public long orderId;
		public int itemType;
		public int itemNumber;
		public string itemCode;
		public string itemDescription;
		public int itemQuantity;
		public decimal  itemValue;
	}
}
