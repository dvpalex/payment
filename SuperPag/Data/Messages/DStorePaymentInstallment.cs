using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("StorePaymentInstallment")]
	public class DStorePaymentInstallment : DataMessageBase
	{

		public DStorePaymentInstallment() {}

		public class Fields
		{
			public const string storeId = "storeId";
			public const string paymentFormId = "paymentFormId";
			public const string installmentNumber = "installmentNumber";
			public const string description = "description";
			public const string minValue = "minValue";
			public const string maxValue = "maxValue";
			public const string interestPercentage = "interestPercentage";
			public const string installmentType = "installmentType";
			public const string allowInParcialPayment = "allowInParcialPayment";
		}

		[PrimaryKey]
		public int storeId;
		[PrimaryKey]
		public int paymentFormId;
		[PrimaryKey]
		public byte installmentNumber;
		public string description;
		public decimal  minValue;
		public decimal  maxValue;
		public decimal  interestPercentage;
		public byte installmentType;
		public bool allowInParcialPayment;
	}
}
