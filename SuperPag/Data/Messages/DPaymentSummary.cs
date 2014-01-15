using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("ProcPaymentSummary")]
    public class DPaymentSummary : DataMessageBase
	{
        public DPaymentSummary() { }

		public class Fields
		{
			public const string name = "name";
            public const string status = "status";
            public const string qtde = "qtde";
            public const string total = "total";
		}
        public string name;
        public int status;
        public int qtde;
        public decimal total;
	}
}