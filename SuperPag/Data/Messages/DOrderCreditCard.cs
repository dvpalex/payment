using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("OrderCreditCard")]
	public class DOrderCreditCard : DataMessageBase
	{

		public DOrderCreditCard() {}

		public class Fields
		{
			public const string orderId = "orderId";
			public const string securityNumber = "securityNumber";
			public const string cardHolderName = "cardHolderName";
			public const string cardNumber = "cardNumber";
			public const string dateExpiration = "dateExpiration";
		}

		[PrimaryKey]
		public long orderId;
		public string securityNumber;
		public string cardHolderName;
		public string cardNumber;
		public DateTime dateExpiration;
	}
}
