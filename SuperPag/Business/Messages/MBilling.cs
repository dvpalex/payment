using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework;
using SuperPag.Data.Messages;
using SuperPag.Data;

namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DBilling))]
  [ Serializable() ]
	public class MBilling : Message
	{

		public MBilling() {}

		private int _billingId;
		[ Mapping ( DBilling.Fields.billingId ) ]
		public int BillingId
          {
		    get { return _billingId; }
		    set { _billingId = value; }
          }


		private int _billingGroup;
		[ Mapping ( DBilling.Fields.billingGroup ) ]
		public int BillingGroup
          {
		    get { return _billingGroup; }
		    set { _billingGroup = value; }
          }


		private long _orderId;
		[ Mapping ( DBilling.Fields.orderId ) ]
		public long OrderId
          {
		    get { return _orderId; }
		    set { _orderId = value; }
          }


		private bool _isInfinityRecurrency;
		[ Mapping ( DBilling.Fields.isInfinityRecurrency ) ]
		public bool IsInfinityRecurrency
          {
		    get { return _isInfinityRecurrency; }
		    set { _isInfinityRecurrency = value; }
          }


		private int _recurrencyPaymentFormId;
		[ Mapping ( DBilling.Fields.recurrencyPaymentFormId ) ]
		public int RecurrencyPaymentFormId
          {
		    get { return _recurrencyPaymentFormId; }
		    set { _recurrencyPaymentFormId = value; }
          }


		private int _recurrencyType;
		[ Mapping ( DBilling.Fields.recurrencyType ) ]
		public int RecurrencyType
          {
		    get { return _recurrencyType; }
		    set { _recurrencyType = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MBilling))]
	public class MCBilling : MessageCollection
	{
	}
}