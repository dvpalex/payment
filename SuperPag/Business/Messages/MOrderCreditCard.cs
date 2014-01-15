using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DOrderCreditCard))]
  [ Serializable() ]
	public class MOrderCreditCard : Message
	{

		public MOrderCreditCard() {}

		private long _orderId;
		[ Mapping ( DOrderCreditCard.Fields.orderId ) ]
		public long OrderId
          {
		    get { return _orderId; }
		    set { _orderId = value; }
          }


		private string _securityNumber;
		[ Mapping ( DOrderCreditCard.Fields.securityNumber ) ]
		public string SecurityNumber
          {
		    get { return _securityNumber; }
		    set { _securityNumber = value; }
          }


		private string _cardHolderName;
		[ Mapping ( DOrderCreditCard.Fields.cardHolderName ) ]
		public string CardHolderName
          {
		    get { return _cardHolderName; }
		    set { _cardHolderName = value; }
          }


		private string _cardNumber;
		[ Mapping ( DOrderCreditCard.Fields.cardNumber ) ]
		public string CardNumber
          {
		    get { return _cardNumber; }
		    set { _cardNumber = value; }
          }


		private DateTime _dateExpiration;
		[ Mapping ( DOrderCreditCard.Fields.dateExpiration ) ]
		public DateTime DateExpiration
          {
		    get { return _dateExpiration; }
		    set { _dateExpiration = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MOrderCreditCard))]
	public class MCOrderCreditCard : MessageCollection
	{
	}
}