using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("ConsumerAddress")]
	public class DConsumerAddress : DataMessageBase
	{

		public DConsumerAddress() {}

		public class Fields
		{
			public const string consumerAddressId = "consumerAddressId";
			public const string consumerId = "consumerId";
			public const string addressType = "addressType";
			public const string logradouro = "logradouro";
			public const string address = "address";
			public const string addressNumber = "addressNumber";
			public const string addressComplement = "addressComplement";
			public const string cep = "cep";
			public const string district = "district";
			public const string city = "city";
			public const string state = "state";
			public const string country = "country";
		}

		[PrimaryKey]
		[Identity]
		public long consumerAddressId;
		public long consumerId;
		public int addressType;
		public string logradouro;
		public string address;
		public string addressNumber;
		public string addressComplement;
		public string cep;
		public string district;
		public string city;
		public string state;
		public string country;
	}
}
