using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("WorkflowOrderStatus")]
	public class DWorkflowOrderStatus : DataMessageBase
	{

		public DWorkflowOrderStatus() {}

		public class Fields
		{
			public const string orderId = "orderId";
			public const string status = "status";
            public const string text = "text";
			public const string creationDate = "creationDate";
		}

		public long orderId;
		public int status;
        public string text;
		public DateTime creationDate;
	}

    [DataRelation(typeof(DWorkflowOrderStatus), DWorkflowOrderStatus.Fields.orderId, typeof(DOrder), DOrder.Fields.orderId, Join.Inner)]
    public class DWorkflowOrderStatus_Order : DWorkflowOrderStatus
    {
        [DataReference(typeof(DOrder))]
        public DOrder dOrder;
    }
}
