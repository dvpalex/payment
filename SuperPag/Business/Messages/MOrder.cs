using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DOrder))]
    [Serializable()]
    public class MOrder : Message
    {
        public enum OrderStatus
        {
            Unfinished = 1,
            Analysing = 2,
            Approved = 3,
            Cancelled = 4,
            Transportation = 5,
            Delivered = 6,
            Undelivered = 7,
            NotPaid = 8,
            PendingPaid = 9
        }

        public MOrder() { }

        private long _orderId;
        [Mapping(DOrder.Fields.orderId)]
        public long OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }


        private MStore _store;
        public MStore Store
        {
            get { return _store; }
            set { _store = value; }
        }


        private MConsumer _consumer;
        public MConsumer Consumer
        {
            get { return _consumer; }
            set { _consumer = value; }
        }


        private string _storeReferenceOrder;
        [Mapping(DOrder.Fields.storeReferenceOrder)]
        public string StoreReferenceOrder
        {
            get { return _storeReferenceOrder; }
            set { _storeReferenceOrder = value; }
        }


        private decimal _totalAmount;
        [Mapping(DOrder.Fields.totalAmount)]
        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set { _totalAmount = value; }
        }


        private decimal _finalAmount;
        [Mapping(DOrder.Fields.finalAmount)]
        public decimal FinalAmount
        {
            get { return _finalAmount; }
            set { _finalAmount = value; }
        }


        private int _installmentQuantity;
        [Mapping(DOrder.Fields.installmentQuantity)]
        public int InstallmentQuantity
        {
            get { return _installmentQuantity; }
            set { _installmentQuantity = value; }
        }


        private OrderStatus _status;
        [Mapping(DOrder.Fields.status)]
        public OrderStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }        
        
        private DateTime _creationDate;
        [Mapping(DOrder.Fields.creationDate)]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }


        private DateTime _lastUpdateDate;
        [Mapping(DOrder.Fields.lastUpdateDate)]
        public DateTime LastUpdateDate
        {
            get { return _lastUpdateDate; }
            set { _lastUpdateDate = value; }
        }


        private Guid _statusChangeUserId;
        [Mapping(DOrder.Fields.statusChangeUserId)]
        public Guid StatusChangeUserId
        {
            get { return _statusChangeUserId; }
            set { _statusChangeUserId = value; }
        }


        private string _statusChangeUserName;
        public string StatusChangeUserName
        {
            get { return _statusChangeUserName; }
            set { _statusChangeUserName = value; }
        }


        private DateTime _statusChangeDate;
        [Mapping(DOrder.Fields.statusChangeDate)]
        public DateTime StatusChangeDate
        {
            get { return _statusChangeDate; }
            set { _statusChangeDate = value; }
        }

        
        private string _cancelDescription;
        [Mapping(DOrder.Fields.cancelDescription)]
        public string CancelDescription
        {
            get { return _cancelDescription; }
            set { _cancelDescription = value; }
        }
        
        
        private MCOrderItem _itens;
        public MCOrderItem Itens
        {
            get { return _itens; }
            set { _itens = value; }
        }

        private MCPaymentAttempt _paymentAttempts;
        public MCPaymentAttempt PaymentAttempts
        {
            get { return _paymentAttempts; }
            set { _paymentAttempts = value; }
        }

        private MRecurrence _recurrence;
        public MRecurrence Recurrence
        {
            get { return _recurrence; }
            set { _recurrence = value; }
        }

        private MCSchedule _schedulePayments;
        public MCSchedule SchedulePayments
        {
            get { return _schedulePayments; }
            set { _schedulePayments = value; }
        }



    }

    [Serializable]
    [CollectionOf(typeof(MOrder))]
	public class MCOrder : MessageCollection
	{
	}
}