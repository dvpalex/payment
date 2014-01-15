using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("Store")]
	public class DStore : DataMessageBase
	{

		public DStore() {}

		public class Fields
		{
			public const string storeId = "storeId";
			public const string name = "name";
			public const string urlSite = "urlSite";
			public const string storeKey = "storeKey";
			public const string password = "password";
            public const string handshakeConfigurationId = "handshakeConfigurationId";
			public const string creationDate = "creationDate";
			public const string lastUpdate = "lastUpdate";
            public const string mailSenderEmail = "mailSenderEmail";
		}

		[PrimaryKey]
		public int storeId;
		public string name;
		public string urlSite;
		public string storeKey;
		public string password;
        public int handshakeConfigurationId;
		public DateTime creationDate;
		public DateTime lastUpdate;
        public string mailSenderEmail;
    }

    [DefaultDataTableName("Store")]
    public class DStoreMaxId : DataMessageBase
    {

        public DStoreMaxId() { }

        public int storeId;
    }
}
