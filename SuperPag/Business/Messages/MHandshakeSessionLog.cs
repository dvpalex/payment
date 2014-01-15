using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DHandshakeSessionLog))]
  [ Serializable() ]
	public class MHandshakeSessionLog : Message
	{

		public MHandshakeSessionLog() {}

		private Guid _handshakeSessionId;
		[ Mapping ( DHandshakeSessionLog.Fields.handshakeSessionId ) ]
		public Guid HandshakeSessionId
          {
		    get { return _handshakeSessionId; }
		    set { _handshakeSessionId = value; }
          }


		private int _step;
		[ Mapping ( DHandshakeSessionLog.Fields.step ) ]
		public int Step
          {
		    get { return _step; }
		    set { _step = value; }
          }


		private byte[] _xmlData;
		[ Mapping ( DHandshakeSessionLog.Fields.xmlData ) ]
		public byte[] XmlData
          {
		    get { return _xmlData; }
		    set { _xmlData = value; }
          }


		private string _url;
		[ Mapping ( DHandshakeSessionLog.Fields.url ) ]
		public string Url
          {
		    get { return _url; }
		    set { _url = value; }
          }


		private DateTime _createDate;
		[ Mapping ( DHandshakeSessionLog.Fields.createDate ) ]
		public DateTime CreateDate
          {
		    get { return _createDate; }
		    set { _createDate = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MHandshakeSessionLog))]
	public class MCHandshakeSessionLog : MessageCollection
	{
	}
}