using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DOrderInstallment))]
  [ Serializable() ]
	public class MOrderInstallment : Message
	{

		public MOrderInstallment() {}

		private long _orderId;
		[ Mapping ( DOrderInstallment.Fields.orderId ) ]
		public long OrderId
          {
		    get { return _orderId; }
		    set { _orderId = value; }
          }


		private int _installmentNumber;
		[ Mapping ( DOrderInstallment.Fields.installmentNumber ) ]
		public int InstallmentNumber
          {
		    get { return _installmentNumber; }
		    set { _installmentNumber = value; }
          }


		private int _paymentFormId;
		[ Mapping ( DOrderInstallment.Fields.paymentFormId ) ]
		public int PaymentFormId
          {
		    get { return _paymentFormId; }
		    set { _paymentFormId = value; }
          }


		private decimal _installmentValue;
		[ Mapping ( DOrderInstallment.Fields.installmentValue ) ]
		public decimal InstallmentValue
          {
		    get { return _installmentValue; }
		    set { _installmentValue = value; }
          }


		private decimal _interestPercentage;
		[ Mapping ( DOrderInstallment.Fields.interestPercentage ) ]
		public decimal InterestPercentage
          {
		    get { return _interestPercentage; }
		    set { _interestPercentage = value; }
          }


		private int _status;
		[ Mapping ( DOrderInstallment.Fields.status ) ]
		public int Status
          {
		    get { return _status; }
		    set { _status = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MOrderInstallment))]
	public class MCOrderInstallment : MessageCollection
	{
	}
}