using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAttempt")]
	public class DPaymentAttempt : DataMessageBase
	{
		public DPaymentAttempt() {}

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
			public const string paymentAgentSetupId = "paymentAgentSetupId";
			public const string paymentFormId = "paymentFormId";
            public const string price = "price";
			public const string orderId = "orderId";
			public const string startTime = "startTime";
			public const string lastUpdate = "lastUpdate";
			public const string status = "status";
			public const string step = "step";
			public const string installmentNumber = "installmentNumber";
			public const string returnMessage = "returnMessage";
			public const string billingScheduleId = "billingScheduleId";
            public const string isSimulation = "isSimulation";
		}

		[PrimaryKey]
		public Guid paymentAttemptId;
		public int paymentAgentSetupId;
		public int paymentFormId;
        public decimal price;
		public long orderId;
		public DateTime startTime;
		public DateTime lastUpdate;
		public int status;
		public int step;
		public int installmentNumber;
		public string returnMessage;
		public int billingScheduleId;
        public bool isSimulation;
        
        public void TruncateStringFields()
        {
            if (returnMessage != null && returnMessage.Length > 200)
                returnMessage = returnMessage.Substring(0, 200);
        }
    }

    [DataRelation(DPaymentAttempt.Fields.orderId, typeof(DOrder), DOrder.Fields.orderId, Join.Inner)]
    [DataRelation(DPaymentForm.Fields.paymentFormId, typeof(DPaymentForm), DPaymentForm.Fields.paymentFormId, Join.Inner)]
    public class DPaymentAttemptComplete : DPaymentAttempt
    {
        [DataReference(typeof(DOrder))]
        public DOrder dOrder;

        [DataReference(typeof(DPaymentForm))]
        public DPaymentForm dPaymentForm;
    }

}
