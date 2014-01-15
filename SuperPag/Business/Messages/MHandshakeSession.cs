using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DHandshakeSession))]
  [ Serializable() ]
	public class MHandshakeSession : Message
	{

		public MHandshakeSession() {}

		private Guid _handshakeSessionId;
		[ Mapping ( DHandshakeSession.Fields.handshakeSessionId ) ]
		public Guid HandshakeSessionId
          {
		    get { return _handshakeSessionId; }
		    set { _handshakeSessionId = value; }
          }


		private int _storeId;
		[ Mapping ( DHandshakeSession.Fields.storeId ) ]
		public int StoreId
          {
		    get { return _storeId; }
		    set { _storeId = value; }
          }


		private long _orderId;
		[ Mapping ( DHandshakeSession.Fields.orderId ) ]
		public long OrderId
          {
		    get { return _orderId; }
		    set { _orderId = value; }
          }


		private int _handshakeType;
		[ Mapping ( DHandshakeSession.Fields.handshakeType ) ]
		public int HandshakeType
          {
		    get { return _handshakeType; }
		    set { _handshakeType = value; }
          }


		private DateTime _createDate;
		[ Mapping ( DHandshakeSession.Fields.createDate ) ]
		public DateTime CreateDate
          {
		    get { return _createDate; }
		    set { _createDate = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MHandshakeSession))]
	public class MCHandshakeSession : MessageCollection
	{
	}
}