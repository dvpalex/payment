using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("PaymentAttemptMoset")]
    public class DPaymentAttemptMoset : DataMessageBase
    {

        public DPaymentAttemptMoset() { }

        public class Fields
        {
            public const string paymentAttemptId = "paymentAttemptId";
            public const string merchantId = "merchantId";
            public const string cardInformation = "cardInformation";
            public const string lr = "lr";
            public const string tid = "tid";
            public const string free = "free";
            public const string capturedCod = "capturedCod";
            public const string capturedTid = "capturedTid";
            public const string capturedArs = "capturedArs";
            public const string capturedCap = "capturedCap";
            public const string capturedValue = "capturedValue";
            public const string capturedCurrency = "capturedCurrency";
            public const string message = "message";
            public const string mosetStatus = "mosetStatus";
        }

        [PrimaryKey]
        public Guid paymentAttemptId;
        public string merchantId;
        public string cardInformation;
        public int lr = int.MinValue;
        public string tid;
        public string free;
        public int capturedCod = int.MinValue;
        public string capturedTid;
        public string capturedArs;
        public string capturedCap;
        public decimal capturedValue = decimal.MinValue;
        public int capturedCurrency = int.MinValue;
        public string message;
        public int mosetStatus;
        
        public void TruncateStringFields()
        {
            if (capturedCap != null && capturedCap.Length > 100)
                capturedCap = capturedCap.Substring(0, 100);
            if (message != null && message.Length > 300)
                message = message.Substring(0, 300);
        }
    }
}
