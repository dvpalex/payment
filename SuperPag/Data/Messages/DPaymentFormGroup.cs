using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentFormGroup")]
	public class DPaymentFormGroup : DataMessageBase
	{

		public DPaymentFormGroup() {}

		public class Fields
		{
			public const string paymentFormGroupId = "paymentFormGroupId";
			public const string name = "name";
		}

		[PrimaryKey]
		public int paymentFormGroupId;
		public string name;
	}
}
