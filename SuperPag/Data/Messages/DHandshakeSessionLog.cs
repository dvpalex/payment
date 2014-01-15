using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("HandshakeSessionLog")]
	public class DHandshakeSessionLog : DataMessageBase
	{

		public DHandshakeSessionLog() {}

		public class Fields
		{
			public const string handshakeSessionId = "handshakeSessionId";
			public const string step = "step";
			public const string xmlData = "xmlData";
			public const string url = "url";
			public const string createDate = "createDate";
		}

		public Guid handshakeSessionId;
		public int step;
		public string xmlData;
		public string url;
		public DateTime createDate;
	}
}
