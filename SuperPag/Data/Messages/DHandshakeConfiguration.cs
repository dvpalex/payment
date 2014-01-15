using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("HandshakeConfiguration")]
	public class DHandshakeConfiguration : DataMessageBase
	{

		public DHandshakeConfiguration() {}

		public class Fields
		{
			public const string handshakeConfigurationId = "handshakeConfigurationId";
			public const string storeId = "storeId";
			public const string handshakeType = "handshakeType";
            public const string autoPaymentConfirm = "autoPaymentConfirm";
			public const string acceptDuplicateOrder = "acceptDuplicateOrder";
			public const string validateEmail = "validateEmail";
            public const string sendEmailStoreKeeper = "sendEmailStoreKeeper";
            public const string sendEmailConsumer = "sendEmailConsumer";
			public const string creationDate = "creationDate";
			public const string lastUpdate = "lastUpdate";
            public const string finalizationHtml = "finalizationHtml";
            public const string paymentHtml = "paymentHtml";
            public const string validateItemsTotal = "validateItemsTotal";
        }

		[PrimaryKey]
		[Identity]
		public int handshakeConfigurationId;
		public int storeId;
		public int handshakeType;
        public bool autoPaymentConfirm;
		public bool acceptDuplicateOrder;
		public bool validateEmail;
		public bool sendEmailStoreKeeper;
        public bool sendEmailConsumer;
		public DateTime creationDate;
		public DateTime lastUpdate;
        public bool finalizationHtml;
        public bool paymentHtml;
        public bool validateItemsTotal;
    }

    [DefaultDataTableName("HandshakeConfiguration")]
    public class DHandshakeConfigurationMaxId : DataMessageBase
    {
        public DHandshakeConfigurationMaxId() { }

        public int handshakeConfigurationId;
    }
}
