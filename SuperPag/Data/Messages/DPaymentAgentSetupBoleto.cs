using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAgentSetupBoleto")]
	public class DPaymentAgentSetupBoleto : DataMessageBase
	{

		public DPaymentAgentSetupBoleto() {}

		public class Fields
		{
			public const string paymentAgentSetupId = "paymentAgentSetupId";
			public const string bankNumber = "bankNumber";
			public const string bankDigit = "bankDigit";
			public const string agencyNumber = "agencyNumber";
			public const string agencyDigit = "agencyDigit";
			public const string accountNumber = "accountNumber";
			public const string accountDigit = "accountDigit";
			public const string cederCode = "cederCode";
			public const string cederName = "cederName";
			public const string cederCNPJ = "cederCNPJ";
			public const string wallet = "wallet";
			public const string conventionNumber = "conventionNumber";
			public const string expirationDays = "expirationDays";
            public const string bodyMail = "bodyMail";
		}

		[PrimaryKey]
		public int paymentAgentSetupId;
		public int bankNumber;
		public int bankDigit;
		public int agencyNumber;
		public int agencyDigit;
		public int accountNumber;
		public int accountDigit;
		public string cederCode;
		public string cederName;
		public string cederCNPJ;
		public string wallet;
		public string conventionNumber;
		public int expirationDays;
        public string bodyMail;
	}
}
