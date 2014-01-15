using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("ServicePaymentPost")]
    public class DServicePaymentPost : DataMessageBase
    {

        public DServicePaymentPost() { }

        public class Fields
        {
            public const string paymentAttemptId = "paymentAttemptId";
            public const string installmentNumber = "installmentNumber";
            public const string postStatus = "postStatus";
            public const string postRetries = "postRetries";
            public const string lastUpdate = "lastUpdate";
            public const string emailSentDate = "emailSentDate";
        }

        [PrimaryKey]
        public Guid paymentAttemptId;
        [PrimaryKey]
        public int installmentNumber;
        public int postStatus;
        public int postRetries;
        public DateTime lastUpdate;
        public DateTime emailSentDate;
    }
}
