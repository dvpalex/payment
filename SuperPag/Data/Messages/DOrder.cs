using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("Order")]
	public class DOrder : DataMessageBase
	{

		public DOrder() {}

		public class Fields
		{
			public const string orderId = "orderId";
			public const string storeId = "storeId";
			public const string consumerId = "consumerId";
			public const string storeReferenceOrder = "storeReferenceOrder";
			public const string totalAmount = "totalAmount";
			public const string finalAmount = "finalAmount";
            public const string installmentQuantity = "installmentQuantity";
            public const string status = "status";
			public const string creationDate = "creationDate";
            public const string lastUpdateDate = "lastUpdateDate";
            public const string statusChangeUserId = "statusChangeUserId";
            public const string statusChangeDate = "statusChangeDate";
            public const string cancelDescription = "cancelDescription";
		}

		[PrimaryKey]
		[Identity]
		public long orderId;
		public int storeId;
		public long consumerId;
		public string storeReferenceOrder;
		public decimal  totalAmount;
		public decimal  finalAmount;
		public int installmentQuantity;
        public int status;
		public DateTime creationDate;
		public DateTime lastUpdateDate;
        public Guid statusChangeUserId;
        public DateTime statusChangeDate;
        public string cancelDescription;
	}
    
}
