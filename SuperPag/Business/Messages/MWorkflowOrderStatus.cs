using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DWorkflowOrderStatus))]
  [ Serializable() ]
	public class MWorkflowOrderStatus : Message
	{

		public MWorkflowOrderStatus() {}

		private long _orderId;
		[ Mapping ( DWorkflowOrderStatus.Fields.orderId ) ]
		public long OrderId
          {
		    get { return _orderId; }
		    set { _orderId = value; }
          }


		private int _status;
		[ Mapping ( DWorkflowOrderStatus.Fields.status ) ]
		public int Status
          {
		    get { return _status; }
		    set { _status = value; }
          }


		private DateTime _creationDate;
		[ Mapping ( DWorkflowOrderStatus.Fields.creationDate ) ]
		public DateTime CreationDate
          {
		    get { return _creationDate; }
		    set { _creationDate = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MWorkflowOrderStatus))]
	public class MCWorkflowOrderStatus : MessageCollection
	{
	}
}