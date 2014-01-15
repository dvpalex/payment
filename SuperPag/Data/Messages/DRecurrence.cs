using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("Recurrence")]
	public class DRecurrence : DataMessageBase
	{

		public DRecurrence() {}

		public class Fields
		{
			public const string recurrenceId = "recurrenceId";
			public const string orderId = "orderId";
            public const string interval = "interval";
            public const string startDate = "startDate";
            public const string paymentFormId = "paymentFormId";
            public const string paymentFormDetail = "paymentFormDetail";
            public const string status = "status";
		}

		[PrimaryKey]
        [Identity]
		public int recurrenceId;
		public long orderId;
        public int interval;
        public DateTime startDate;
		public int paymentFormId;
        public string paymentFormDetail;
        public int status;
	}
}
