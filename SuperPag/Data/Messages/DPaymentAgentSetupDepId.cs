using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgentSetupDepId")]
	public class DPaymentAgentSetupDepId : DataMessageBase
	{

		public DPaymentAgentSetupDepId() {}

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
			public const string bankNumber = "bankNumber";
			public const string bankDigit = "bankDigit";
			public const string agencyNumber = "agencyNumber";
			public const string agencyDigit = "agencyDigit";
			public const string accountNumber = "accountNumber";
			public const string accountDigit = "accountDigit";
			public const string cederName = "cederName";
			public const string cederCNPJ = "cederCNPJ";
            public const string conventionType = "conventionType";
            public const string calcType = "calcType";
            public const string expirationDays = "expirationDays";
            public const string idPattern = "storeReferencePattern";
		}

		[PrimaryKey]
		public int paymentAgentSetupId;
		public int bankNumber;
		public int bankDigit;
		public int agencyNumber;
		public int agencyDigit;
		public int accountNumber;
		public int accountDigit;
		public string cederName;
		public string cederCNPJ;
        public string conventionType;
        public string calcType;
        public int expirationDays;
        public string idPattern;
	}
}
