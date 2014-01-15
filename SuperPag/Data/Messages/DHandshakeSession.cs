using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("HandshakeSession")]
	public class DHandshakeSession : DataMessageBase
	{

		public DHandshakeSession() {}

		public class Fields
		{
			public const string handshakeSessionId = "handshakeSessionId";
			public const string storeId = "storeId";
			public const string orderId = "orderId";
			public const string handshakeType = "handshakeType";
			public const string createDate = "createDate";
        }

		[PrimaryKey]
		public Guid handshakeSessionId;
		public int storeId;
		public long orderId;
		public int handshakeType;
		public DateTime createDate;
    }
}
