using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgentSetupKomerci")]
	public class DPaymentAgentSetupKomerci : DataMessageBase
	{
		public DPaymentAgentSetupKomerci() {}

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
			public const string businessNumber = "businessNumber";
			public const string instalmentType = "instalmentType";
			public const string urlKomerci = "urlKomerci";
			public const string urlKomerciConfirm = "urlKomerciConfirm";
            public const string urlKomerciAVS = "urlKomerciAVS";
            public const string checkAVS = "checkAVS";
            public const string acceptedAVSReturn = "acceptedAVSReturn";
            public const string AVSExceptionBINs = "AVSExceptionBINs";
		}

		[PrimaryKey]
		public int paymentAgentSetupId;
		public int businessNumber;
		public string instalmentType;
		public string urlKomerci;
		public string urlKomerciConfirm;
        public string urlKomerciAVS;
        public bool checkAVS;
        public string acceptedAVSReturn;
        public string AVSExceptionBINs;
    }
}
