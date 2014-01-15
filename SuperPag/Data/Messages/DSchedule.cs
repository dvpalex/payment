using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("Schedule")]
	public class DSchedule : DataMessageBase
	{

		public DSchedule() {}

		public class Fields
		{
			public const string scheduleId = "scheduleId";
            public const string orderId = "orderId";
            public const string recurrenceId = "recurrenceId";
            public const string installmentNumber = "installmentNumber";
            public const string installmentType = "installmentType";
			public const string date = "date";
			public const string paymentFormId = "paymentFormId";
            public const string paymentFormDetail = "paymentFormDetail";
            public const string status = "status";
            public const string paymentAttemptId = "paymentAttemptId";
		}

		[PrimaryKey]
        [Identity]
		public int scheduleId;
		public int orderId;
        public int recurrenceId;
        public int installmentNumber;
        public int installmentType;
		public DateTime date;
		public int paymentFormId;
        public string paymentFormDetail;
        public int status;
        public Guid paymentAttemptId;
	}

    [DataRelation(DPaymentForm.Fields.paymentFormId, typeof(DPaymentForm), DPaymentForm.Fields.paymentFormId, Join.Inner)]
    public class DScheduleComplete : DSchedule
    {
        [DataReference(typeof(DPaymentForm))]
        public DPaymentForm dPaymentForm;
    }
}
