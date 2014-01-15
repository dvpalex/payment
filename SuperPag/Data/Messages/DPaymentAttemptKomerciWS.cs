using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("PaymentAttemptKomerciWS")]
	public class DPaymentAttemptKomerciWS : DataMessageBase
	{

		public DPaymentAttemptKomerciWS() {}

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
			public const string transacao = "transacao";
            public const string autoCapture = "autoCapture";
			public const string data = "data";
            public const string cardInformation = "cardInformation";
			public const string numautor = "numautor";
			public const string numcv = "numcv";
			public const string numautent = "numautent";
			public const string numsqn = "numsqn";
			public const string codret = "codret";
			public const string msgret = "msgret";
            public const string capcodret = "capcodret";
            public const string capmsgret = "capmsgret";
            public const string avs = "avs";
            public const string respavs = "respavs";
            public const string msgavs = "msgavs";
			public const string komerciStatus = "komerciStatus";
		}

		[PrimaryKey]
		public Guid paymentAttemptId;
		public string transacao;
        public string autoCapture;
		public string data;
        public string cardInformation;
		public string numautor;
		public string numcv;
		public string numautent;
		public string numsqn;
		public string codret;
		public string msgret;
        public string capcodret;
        public string capmsgret;
        public string avs;
        public string respavs;
        public string msgavs;
		public byte komerciStatus;
        
        public void TruncateStringFields()
        {
            if (msgret != null && msgret.Length > 200)
                msgret = msgret.Substring(0, 200);
            if (capmsgret != null && capmsgret.Length > 200)
                capmsgret = capmsgret.Substring(0, 200);
            if (msgavs != null && msgavs.Length > 80)
                msgavs = msgavs.Substring(0, 80);
        }
    }
}
