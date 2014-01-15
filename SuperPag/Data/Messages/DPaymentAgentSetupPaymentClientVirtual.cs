using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
#if SQL
    [DefaultDataTableName("PaymentAgentSetupPaymentClientVirtual")]
#elif ORACLE
    [DefaultDataTableName("PaymentAgentSetupVPC")]
#else
    [DefaultDataTableName("PaymentAgentSetupPaymentClientVirtual")]
#endif
    public class DPaymentAgentSetupPaymentClientVirtual : DataMessageBase
	{

        public DPaymentAgentSetupPaymentClientVirtual() { }

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
            public const string accessCode = "accessCode";
            public const string secureHashSecret = "secureHashSecret";
            public const string merchantId = "merchantId";
            public const string checkAVS = "checkAVS";
            public const string acceptedAVSReturn = "acceptedAVSReturn";
            public const string version = "version";
            public const string captureUser = "captureUser";
            public const string capturePassword = "capturePassword";
            public const string autoCapture = "autoCapture";
        }

		[PrimaryKey]
		public int paymentAgentSetupId;
        public string accessCode;
        public string secureHashSecret;
        public string merchantId;
        public bool checkAVS;
        public string acceptedAVSReturn;
        public string version;
        public string captureUser;
        public string capturePassword;
        public bool autoCapture;
    }
}
