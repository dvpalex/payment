using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("OrderInstallment")]
	public class DOrderInstallment : DataMessageBase
	{

		public DOrderInstallment() {}

		public class Fields
		{
			public const string orderId = "orderId";
			public const string installmentNumber = "installmentNumber";
			public const string paymentFormId = "paymentFormId";
			public const string installmentValue = "installmentValue";
			public const string interestPercentage = "interestPercentage";
			public const string status = "status";
		}

		[PrimaryKey]
		public long orderId;
		[PrimaryKey]
		public int installmentNumber;
		public int paymentFormId;
		public decimal  installmentValue;
		public decimal  interestPercentage;
		public int status;
	}
}
