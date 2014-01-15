using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DConsumerAddress))]
  [ Serializable() ]
	public class MConsumerAddress : Message
	{
        public enum AddressTypes
        {
            Billing = 1,
            Delivery = 2
        }

		public MConsumerAddress() {}

		private long _consumerAddressId;
		[ Mapping ( DConsumerAddress.Fields.consumerAddressId ) ]
		public long ConsumerAddressId
          {
		    get { return _consumerAddressId; }
		    set { _consumerAddressId = value; }
          }


		private long _consumerId;
		[ Mapping ( DConsumerAddress.Fields.consumerId ) ]
		public long ConsumerId
          {
		    get { return _consumerId; }
		    set { _consumerId = value; }
          }


		private int _addressType;
		[ Mapping ( DConsumerAddress.Fields.addressType ) ]
		public int AddressType
          {
		    get { return _addressType; }
		    set { _addressType = value; }
          }


		private string _logradouro;
		[ Mapping ( DConsumerAddress.Fields.logradouro ) ]
		public string Logradouro
          {
		    get { return _logradouro; }
		    set { _logradouro = value; }
          }


		private string _address;
		[ Mapping ( DConsumerAddress.Fields.address ) ]
		public string Address
          {
		    get { return _address; }
		    set { _address = value; }
          }


		private string _addressNumber;
		[ Mapping ( DConsumerAddress.Fields.addressNumber ) ]
		public string AddressNumber
          {
		    get { return _addressNumber; }
		    set { _addressNumber = value; }
          }


		private string _addressComplement;
		[ Mapping ( DConsumerAddress.Fields.addressComplement ) ]
		public string AddressComplement
          {
		    get { return _addressComplement; }
		    set { _addressComplement = value; }
          }


		private string _cep;
		[ Mapping ( DConsumerAddress.Fields.cep ) ]
		public string Cep
          {
		    get { return _cep; }
		    set { _cep = value; }
          }


		private string _district;
		[ Mapping ( DConsumerAddress.Fields.district ) ]
		public string District
          {
		    get { return _district; }
		    set { _district = value; }
          }


		private string _city;
		[ Mapping ( DConsumerAddress.Fields.city ) ]
		public string City
          {
		    get { return _city; }
		    set { _city = value; }
          }


		private string _state;
		[ Mapping ( DConsumerAddress.Fields.state ) ]
		public string State
          {
		    get { return _state; }
		    set { _state = value; }
          }


		private string _country;
		[ Mapping ( DConsumerAddress.Fields.country ) ]
		public string Country
          {
		    get { return _country; }
		    set { _country = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MConsumerAddress))]
	public class MCConsumerAddress : MessageCollection
	{
	}
}