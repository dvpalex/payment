using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("ServiceFinalizationPost")]
    public class DServiceFinalizationPost : DataMessageBase
	{

        public DServiceFinalizationPost() { }

		public class Fields
		{
			public const string paymentAttemptId = "paymentAttemptId";
            public const string postStatus = "postStatus";
			public const string postRetries = "postRetries";
			public const string lastUpdate = "lastUpdate";
            public const string emailSentDate = "emailSentDate";
		}

		[PrimaryKey]
		public Guid paymentAttemptId;
		public int postStatus;
		public int postRetries;
		public DateTime lastUpdate;
        public DateTime emailSentDate;
	}
}
