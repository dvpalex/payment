using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("HandshakeConfigurationHtml")]
	public class DHandshakeConfigurationHtml : DataMessageBase
	{

		public DHandshakeConfigurationHtml() {}

		public class Fields
		{
			public const string handshakeConfigurationId = "handshakeConfigurationId";
			public const string urlHandshake = "urlHandshake";
			public const string urlFinalization = "urlFinalization";
			public const string urlFinalizationOffline = "urlFinalizationOffline";
			public const string urlReturn = "urlReturn";
			public const string urlPaymentConfirmation = "urlPaymentConfirmation";
			public const string urlPaymentConfirmationOffline = "urlPaymentConfirmationOffline";
            public const string requestEncoding = "requestEncoding";
            public const string responseEncoding = "responseEncoding";
		}

		[PrimaryKey]
		public int handshakeConfigurationId;
		public string urlHandshake;
		public string urlFinalization;
		public string urlFinalizationOffline;
		public string urlReturn;
		public string urlPaymentConfirmation;
		public string urlPaymentConfirmationOffline;
        public string requestEncoding;
        public string responseEncoding;
    }
}
