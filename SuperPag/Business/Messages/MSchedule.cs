using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DSchedule))]
    [Serializable()]
    public class MSchedule : Message
    {
        public enum ScheduleStatus
        {
            Scheduled = 1,
            Processed = 2,
            Canceled = 3
        }

        public MSchedule() { }

        private long _scheduleId;
        [Mapping(DSchedule.Fields.scheduleId)]
        public long ScheduleId
        {
            get { return _scheduleId; }
            set { _scheduleId = value; }
        }

        private MOrder _order;
        public MOrder Order
        {
            get { return _order; }
            set { _order = value; }
        }

        private int _installmentType;
        [Mapping(DSchedule.Fields.installmentType)]
        public int InstallmentType
        {
            get { return _installmentType; }
            set { _installmentType = value; }
        }

        private DateTime _date;
        [Mapping(DSchedule.Fields.date)]
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
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


        private ScheduleStatus _status;
        [Mapping(DSchedule.Fields.status)]
        public ScheduleStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private MPaymentAttempt _paymentAttempt;
        public MPaymentAttempt PaymentAttemptId
        {
            get { return _paymentAttempt; }
            set { _paymentAttempt = value; }
        }

    }

    [Serializable]
    [CollectionOf(typeof(MSchedule))]
	public class MCSchedule : MessageCollection
	{
	}
}