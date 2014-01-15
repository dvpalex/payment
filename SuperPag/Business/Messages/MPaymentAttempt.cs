using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAttempt))]
  [ Serializable() ]
	public class MPaymentAttempt : Message
	{
        public enum PaymentAttemptStatus { Pending = 1, Paid = 2, NotPaid = 3, Canceled = 4, PayPending = 5, Delivered = 6 }

		public MPaymentAttempt() {}

		private Guid _paymentAttemptId;
		[ Mapping ( DPaymentAttempt.Fields.paymentAttemptId ) ]
		public Guid PaymentAttemptId
          {
		    get { return _paymentAttemptId; }
		    set { _paymentAttemptId = value; }
          }


		private int _paymentAgentSetupId;
		[ Mapping ( DPaymentAttempt.Fields.paymentAgentSetupId ) ]
		public int PaymentAgentSetupId
          {
		    get { return _paymentAgentSetupId; }
		    set { _paymentAgentSetupId = value; }
          }


		private MPaymentForm _paymentForm;
        public MPaymentForm PaymentForm
          {
		    get { return _paymentForm; }
		    set { _paymentForm = value; }
          }


		private MOrder _order;
        public MOrder Order
          {
		    get { return _order; }
		    set { _order = value; }
          }


		private DateTime _startTime;
		[ Mapping ( DPaymentAttempt.Fields.startTime ) ]
		public DateTime StartTime
          {
		    get { return _startTime; }
		    set { _startTime = value; }
          }


		private DateTime _lastUpdate;
		[ Mapping ( DPaymentAttempt.Fields.lastUpdate ) ]
		public DateTime LastUpdate
          {
		    get { return _lastUpdate; }
		    set { _lastUpdate = value; }
          }


		private PaymentAttemptStatus _status;
		[ Mapping ( DPaymentAttempt.Fields.status ) ]
        public PaymentAttemptStatus Status
          {
		    get { return _status; }
		    set { _status = value; }
          }


		private int _step;
		[ Mapping ( DPaymentAttempt.Fields.step ) ]
		public int Step
          {
		    get { return _step; }
		    set { _step = value; }
          }


		private int _installmentNumber;
		[ Mapping ( DPaymentAttempt.Fields.installmentNumber ) ]
		public int InstallmentNumber
          {
		    get { return _installmentNumber; }
		    set { _installmentNumber = value; }
          }


		private string _returnMessage;
		[ Mapping ( DPaymentAttempt.Fields.returnMessage ) ]
		public string ReturnMessage
          {
		    get { return _returnMessage; }
		    set { _returnMessage = value; }
          }


        private int _billingScheduleId;
        [ Mapping ( DPaymentAttempt.Fields.billingScheduleId ) ]
        public int BillingScheduleId
          {
            get { return _billingScheduleId; }
            set { _billingScheduleId = value; }
          }


        private bool _isSimulation;
        [Mapping(DPaymentAttempt.Fields.isSimulation)]
        public bool IsSimulation
          {
              get { return _isSimulation; }
              set { _isSimulation = value; }
          }


        private decimal _price;
        [Mapping(DPaymentAttempt.Fields.price)]
        public decimal Price
          {
              get { return _price; }
              set { _price = value; }
          }
      }

  [Serializable]
    [CollectionOf(typeof(MPaymentAttempt))]
	public class MCPaymentAttempt : MessageCollection
	{
	}
}