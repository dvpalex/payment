using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DRecurrence))]
    [Serializable()]
    public class MRecurrence : Message
    {
        public enum RecurrenceStatus
        {
            Active = 1,
            Cancelled = 2
        }

        public MRecurrence() { }

        private int _recurrenceId;
        [Mapping(DRecurrence.Fields.recurrenceId)]
        public int RecurrenceId
        {
            get { return _recurrenceId; }
            set { _recurrenceId = value; }
        }


        private MOrder _order;
        public MOrder Order
        {
            get { return _order; }
            set { _order = value; }
        }


        private int _interval;
        [Mapping(DRecurrence.Fields.interval)]
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }


        private DateTime _startDate;
        [Mapping(DRecurrence.Fields.startDate)]
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }


        private MPaymentForm _paymentForm;
        public MPaymentForm PaymentForm
        {
            get { return _paymentForm; }
            set { _paymentForm = value; }
        }


        private string _paymentFormDetail;
        [Mapping(DSchedule.Fields.paymentFormDetail)]
        public string PaymentFormDetail
        {
            get { return _paymentFormDetail; }
            set { _paymentFormDetail = value; }
        }


        private RecurrenceStatus _status;
        [Mapping(DRecurrence.Fields.status)]
        public RecurrenceStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }        
    }

    [Serializable]
    [CollectionOf(typeof(MRecurrence))]
	public class MCRecurrence : MessageCollection
	{
	}
}