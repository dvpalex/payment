using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAttemptKomerci")]
	public class DPaymentAttemptKomerci : DataMessageBase
	{

		public DPaymentAttemptKomerci() {}

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
			public const string agentOrderReference = "agentOrderReference";
			public const string transacao = "transacao";
			public const string bandeira = "bandeira";
			public const string codver = "codver";
			public const string data = "data";
			public const string nr_cartao = "nr_cartao";
			public const string origem_bin = "origem_bin";
			public const string numautor = "numautor";
			public const string numcv = "numcv";
			public const string numautent = "numautent";
			public const string numsqn = "numsqn";
			public const string codret = "codret";
			public const string msgret = "msgret";
            public const string avs = "avs";
            public const string respavs = "respavs";
            public const string msgavs = "msgavs";
			public const string komerciStatus = "komerciStatus";
		}

		[PrimaryKey]
		public Guid paymentAttemptId;
		[Identity]
		public int agentOrderReference;
		public string transacao;
		public string bandeira;
		public string codver;
		public string data;
		public string nr_cartao;
		public string origem_bin;
		public string numautor;
		public string numcv;
		public string numautent;
		public string numsqn;
		public string codret;
		public string msgret;
        public string avs;
        public string respavs;
        public string msgavs;
		public byte komerciStatus;
        
        public void TruncateStringFields()
        {
            if (msgret != null && msgret.Length > 200)
                msgret = msgret.Substring(0, 200);
            if (msgavs != null && msgavs.Length > 80)
                msgavs = msgavs.Substring(0, 80);
        }
    }
}
