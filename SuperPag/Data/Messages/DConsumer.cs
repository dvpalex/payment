using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("Consumer")]
	public class DConsumer : DataMessageBase
	{
		public DConsumer() {}

		public class Fields
		{
			public const string consumerId = "consumerId";
			public const string CPF = "CPF";
			public const string RG = "RG";
			public const string CNPJ = "CNPJ";
			public const string IE = "IE";
			public const string name = "name";
			public const string birthDate = "birthDate";
			public const string ger = "ger";
			public const string civilState = "civilState";
			public const string occupation = "occupation";
			public const string phone = "phone";
			public const string commercialPhone = "commercialPhone";
			public const string celularPhone = "celularPhone";
			public const string fax = "fax";
			public const string responsibleName = "responsibleName";
			public const string responsibleCPF = "responsibleCPF";
			public const string email = "email";
		}

		[PrimaryKey]
		[Identity]
		public long consumerId;
		public string CPF;
		public string RG;
		public string CNPJ;
		public string IE;
		public string name;
		public DateTime birthDate;
		public string ger;
		public string civilState;
		public string occupation;
		public string phone;
		public string commercialPhone;
		public string celularPhone;
		public string fax;
		public string responsibleName;
		public string responsibleCPF;
		public string email;

        public void TruncateStringFields()
        {
            if (name != null && name.Length > 50)
                name = name.Substring(0, 50);
            if (responsibleName != null && responsibleName.Length > 50)
                responsibleName = responsibleName.Substring(0, 50);
        }
	}
}
