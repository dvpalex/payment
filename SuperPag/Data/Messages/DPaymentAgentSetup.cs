using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgentSetup")]
	public class DPaymentAgentSetup : DataMessageBase
	{

		public DPaymentAgentSetup() {}

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
			public const string paymentAgentId = "paymentAgentId";
            public const string title = "title";
        }

		[PrimaryKey]
		public int paymentAgentSetupId;
		public int paymentAgentId;
        public string title;
    }

    [DataRelation(DPaymentAgentSetup.Fields.paymentAgentId, typeof(DPaymentAgent), DPaymentAgent.Fields.paymentAgentId, Join.Inner)]
    public class DPaymentAgentSetupComplete : DPaymentAgentSetup
    {
        [DataReference(typeof(DPaymentAgent))]
        public DPaymentAgent dPaymentAgent;
    }

    [DefaultDataTableName("PaymentAgentSetup")]
    public class DPaymentAgentSetupMaxId : DataMessageBase
    {

        public DPaymentAgentSetupMaxId() { }

        public int paymentAgentSetupId;
    }

}
