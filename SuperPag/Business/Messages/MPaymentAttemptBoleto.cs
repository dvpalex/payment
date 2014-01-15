using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAttemptBoleto))]
  [ Serializable() ]
	public class MPaymentAttemptBoleto : Message
	{

		public MPaymentAttemptBoleto() {}

		private Guid _paymentAttemptId;
		[ Mapping ( DPaymentAttemptBoleto.Fields.paymentAttemptId ) ]
		public Guid PaymentAttemptId
          {
		    get { return _paymentAttemptId; }
		    set { _paymentAttemptId = value; }
          }


		private int _agentOrderReference;
		[ Mapping ( DPaymentAttemptBoleto.Fields.agentOrderReference ) ]
		public int AgentOrderReference
          {
		    get { return _agentOrderReference; }
		    set { _agentOrderReference = value; }
          }


		private string _documentNumber;
		[ Mapping ( DPaymentAttemptBoleto.Fields.documentNumber ) ]
		public string DocumentNumber
          {
		    get { return _documentNumber; }
		    set { _documentNumber = value; }
          }


		private string _withdraw;
		[ Mapping ( DPaymentAttemptBoleto.Fields.withdraw ) ]
		public string Withdraw
          {
		    get { return _withdraw; }
		    set { _withdraw = value; }
          }


		private string _withdrawDoc;
		[ Mapping ( DPaymentAttemptBoleto.Fields.withdrawDoc ) ]
		public string WithdrawDoc
          {
		    get { return _withdrawDoc; }
		    set { _withdrawDoc = value; }
          }


		private string _address1;
		[ Mapping ( DPaymentAttemptBoleto.Fields.address1 ) ]
		public string Address1
          {
		    get { return _address1; }
		    set { _address1 = value; }
          }


		private string _address2;
		[ Mapping ( DPaymentAttemptBoleto.Fields.address2 ) ]
		public string Address2
          {
		    get { return _address2; }
		    set { _address2 = value; }
          }


		private string _address3;
		[ Mapping ( DPaymentAttemptBoleto.Fields.address3 ) ]
		public string Address3
          {
		    get { return _address3; }
		    set { _address3 = value; }
          }


		private string _oct;
		[ Mapping ( DPaymentAttemptBoleto.Fields.oct ) ]
		public string Oct
          {
		    get { return _oct; }
		    set { _oct = value; }
          }


		private string _barCode;
		[ Mapping ( DPaymentAttemptBoleto.Fields.barCode ) ]
		public string BarCode
          {
		    get { return _barCode; }
		    set { _barCode = value; }
          }


        private string _ourNumber;
        [Mapping(DPaymentAttemptBoleto.Fields.ourNumber)]
        public string OurNumber
          {
              get { return _ourNumber; }
              set { _ourNumber = value; }
          }


		private string _instructions;
		[ Mapping ( DPaymentAttemptBoleto.Fields.instructions ) ]
		public string Instructions
          {
		    get { return _instructions; }
		    set { _instructions = value; }
          }


		private DateTime _paymentDate;
		[ Mapping ( DPaymentAttemptBoleto.Fields.paymentDate ) ]
		public DateTime PaymentDate
          {
		    get { return _paymentDate; }
		    set { _paymentDate = value; }
          }


		private DateTime _expirationPaymentDate;
		[ Mapping ( DPaymentAttemptBoleto.Fields.expirationPaymentDate ) ]
		public DateTime ExpirationPaymentDate
          {
		    get { return _expirationPaymentDate; }
		    set { _expirationPaymentDate = value; }
          }


        private int _paymentAttemptBoletoReturnId;
        [Mapping(DPaymentAttemptBoleto.Fields.paymentAttemptBoletoReturnId)]
        public int PaymentAttemptBoletoReturnId
          {
              get { return _paymentAttemptBoletoReturnId; }
              set { _paymentAttemptBoletoReturnId = value; }
          }
	}

  [Serializable]
    [CollectionOf(typeof(MPaymentAttemptBoleto))]
	public class MCPaymentAttemptBoleto : MessageCollection
	{
	}
}