using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("BillingSchedule")]
	public class DBillingSchedule : DataMessageBase
	{

		public DBillingSchedule() {}

		public class Fields
		{
			public const string billingScheduleId = "billingScheduleId";
			public const string billingId = "billingId";
			public const string billingDate = "billingDate";
			public const string paymentFormId = "paymentFormId";
			public const string instalmentNumber = "instalmentNumber";
		}

		[PrimaryKey]
		public int billingScheduleId;
		public int billingId;
		public DateTime billingDate;
		public int paymentFormId;
		public int instalmentNumber;
	}
}
