using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("ServicesConfiguration")]
	public class DServicesConfiguration : DataMessageBase
	{
		public DServicesConfiguration() {}

		public class Fields
		{
			public const string storeId = "storeId";
			public const string offLineFinalizationRetries = "offLineFinalizationRetries";
			public const string offLinePaymentRetries = "offLinePaymentRetries";
            public const string contingencyEmails = "contingencyEmails";
            public const string urlBoletoRetry = "urlBoletoRetry";
		}

		[PrimaryKey]
		public int storeId;
		public int offLineFinalizationRetries;
		public int offLinePaymentRetries;
        public string contingencyEmails;
        public string urlBoletoRetry;
	}
}
