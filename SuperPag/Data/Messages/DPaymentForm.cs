using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentForm")]
	public class DPaymentForm : DataMessageBase
	{

		public DPaymentForm() {}

		public class Fields
		{
			public const string paymentFormId = "paymentFormId";
			public const string paymentFormGroupId = "paymentFormGroupId";
			public const string paymentAgentId = "paymentAgentId";
			public const string name = "name";
		}

		[PrimaryKey]
		public int paymentFormId;
		public int paymentFormGroupId;
		public int paymentAgentId;
		public string name;
	}

    [DataRelation(DPaymentForm.Fields.paymentAgentId, typeof(DPaymentAgent), DPaymentAgent.Fields.paymentAgentId, Join.Inner)]
    public class DPaymentFormComplete : DPaymentForm
    {
        [DataReference(typeof(DPaymentAgent))]
        public DPaymentAgent dPaymentAgent;
    }

}
