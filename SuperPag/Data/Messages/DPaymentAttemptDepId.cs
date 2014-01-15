using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("PaymentAttemptDepId")]
    public class DPaymentAttemptDepId : DataMessageBase
    {
        public DPaymentAttemptDepId() { }

        public class Fields
        {
            public const string paymentAttemptId = "paymentAttemptId";
            public const string agentOrderReference = "agentOrderReference";
            public const string bankNumber = "bankNumber";
            public const string idNumber = "idNumber";
            public const string paymentDate = "paymentDate";
            public const string dueDate = "dueDate";
            public const string paymentAttemptDepIdReturnId = "paymentAttemptDepIdReturnId";
            public const string paymentStatus = "paymentStatus";
        }

        [PrimaryKey]
        public Guid paymentAttemptId;
        [Identity]
        public int agentOrderReference;
        public int bankNumber;
        public string idNumber;
        public DateTime paymentDate;
        public DateTime dueDate;
        public int paymentAttemptDepIdReturnId;
        public int paymentStatus;
    }
}
