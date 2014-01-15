using System;
using SuperPag.Framework;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DBillingSchedule))]
  [ Serializable() ]
	public class MBillingSchedule : Message
	{

		public MBillingSchedule() {}

		private int _billingScheduleId;
		[ Mapping ( DBillingSchedule.Fields.billingScheduleId ) ]
		public int BillingScheduleId
          {
		    get { return _billingScheduleId; }
		    set { _billingScheduleId = value; }
          }


		private int _billingId;
		[ Mapping ( DBillingSchedule.Fields.billingId ) ]
		public int BillingId
          {
		    get { return _billingId; }
		    set { _billingId = value; }
          }


		private DateTime _billingDate;
		[ Mapping ( DBillingSchedule.Fields.billingDate ) ]
		public DateTime BillingDate
          {
		    get { return _billingDate; }
		    set { _billingDate = value; }
          }


		private int _paymentFormId;
		[ Mapping ( DBillingSchedule.Fields.paymentFormId ) ]
		public int PaymentFormId
          {
		    get { return _paymentFormId; }
		    set { _paymentFormId = value; }
          }


		private int _instalmentNumber;
		[ Mapping ( DBillingSchedule.Fields.instalmentNumber ) ]
		public int InstalmentNumber
          {
		    get { return _instalmentNumber; }
		    set { _instalmentNumber = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MBillingSchedule))]
	public class MCBillingSchedule : MessageCollection
	{
	}
}