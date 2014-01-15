using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("StorePaymentForm")]
	public class DStorePaymentForm : DataMessageBase
	{

		public DStorePaymentForm() {}

		public class Fields
		{
			public const string storeId = "storeId";
			public const string paymentFormId = "paymentFormId";
			public const string paymentAgentSetupId = "paymentAgentSetupId";
            public const string showInCombo = "showInCombo";
            public const string useTestValues = "useTestValues";
			public const string isActive = "isActive";
		}

		[PrimaryKey]
		public int storeId;
		[PrimaryKey]
		public int paymentFormId;
		public int paymentAgentSetupId;
        public bool showInCombo;
        public bool useTestValues;
        public bool isActive;
	}

    [DataRelation(DStorePaymentForm.Fields.paymentAgentSetupId, typeof(DPaymentAgentSetup), DPaymentAgentSetup.Fields.paymentAgentSetupId, Join.Inner)]
    [DataRelation(DStorePaymentForm.Fields.paymentFormId, typeof(DPaymentForm), DPaymentForm.Fields.paymentFormId, Join.Inner)]
    public class DStorePaymentFormComplete : DStorePaymentForm
    {
        [DataReference(typeof(DPaymentAgentSetup))]
        public DPaymentAgentSetup dPaymentAgentSetup;

        [DataReference(typeof(DPaymentForm))]
        public DPaymentForm dPaymentForm;

    }

}
