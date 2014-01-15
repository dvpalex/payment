using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
#if SQL
    [DefaultDataTableName("PaymentAttemptPaymentClientVirtual")]
#elif ORACLE
    [DefaultDataTableName("PaymentAttemptVPC")]
#else
    [DefaultDataTableName("PaymentAttemptPaymentClientVirtual")]
#endif
    public class DPaymentAttemptPaymentClientVirtual : DataMessageBase
    {

        public DPaymentAttemptPaymentClientVirtual() { }

        public class Fields
        {
            public const string paymentAttemptId = "paymentAttemptId";
            public const string agentOrderReference = "agentOrderReference";
            public const string purchaseAmount = "purchaseAmount";
            public const string cardInformation = "cardInformation";
            public const string signatureCreated = "signatureCreated";
            public const string avs = "avs";
            public const string vpc_Version = "vpc_Version";
            public const string vpc_AuthorizeId = "vpc_AuthorizeId";
            public const string vpc_AVS_Street01 = "vpc_AVS_Street01";
            public const string vpc_AVS_PostCode = "vpc_AVS_PostCode";
            public const string vpc_AVSResultCode = "vpc_AVSResultCode";
            public const string vpc_AcqAVSRespCode = "vpc_AcqAVSRespCode";
            public const string vpc_AcqCSCRespCode = "vpc_AcqCSCRespCode";
            public const string vpc_AcqResponseCode = "vpc_AcqResponseCode";
            public const string vpc_BatchNo = "vpc_BatchNo";
            public const string vpc_CSCResultCode = "vpc_CSCResultCode";
            public const string vpc_Card = "vpc_Card";
            public const string vpc_Message = "vpc_Message";
            public const string vpc_CaptureMessage = "vpc_CaptureMessage";
            public const string vpc_ReceiptNo = "vpc_ReceiptNo";
            public const string vpc_SecureHash = "vpc_SecureHash";
            public const string vpc_TransactionNo = "vpc_TransactionNo";
            public const string vpc_CapTransactionNo = "vpc_CapTransactionNo";
            public const string vpc_TxnResponseCode = "vpc_TxnResponseCode";
            public const string vpc_CapTxnResponseCode = "vpc_CapTxnResponseCode";
            public const string vpc_ShopTransactionNo = "vpc_ShopTransactionNo";
            public const string vpc_AuthorisedAmount = "vpc_AuthorisedAmount";
            public const string vpc_CapturedAmount = "vpc_CapturedAmount";
            public const string vpc_TicketNumber = "vpc_TicketNumber";
            public const string paymentClientVirtualiStatus = "paymentClientVirtualiStatus";
        }

        [PrimaryKey]
        public Guid paymentAttemptId;
        [Identity]
        public int agentOrderReference;
        public decimal purchaseAmount;
        public string cardInformation;
        public string signatureCreated;
        public string avs;
        public string vpc_Version;
        public int vpc_AuthorizeId = int.MinValue;
        public string vpc_AVS_Street01;
        public string vpc_AVS_PostCode;
        public string vpc_AVSResultCode;
        public string vpc_AcqAVSRespCode;
        public string vpc_AcqCSCRespCode;
        public string vpc_AcqResponseCode;
        public int vpc_BatchNo = int.MinValue;
        public string vpc_CSCResultCode;
        public string vpc_Card;
        public string vpc_Message;
        public string vpc_CaptureMessage;
        public string vpc_ReceiptNo;
        public string vpc_SecureHash;
        public int vpc_TransactionNo = int.MinValue;
        public int vpc_CapTransactionNo = int.MinValue;
        public string vpc_TxnResponseCode;
        public string vpc_CapTxnResponseCode;
        public string vpc_ShopTransactionNo;
        public string vpc_AuthorisedAmount;
        public string vpc_CapturedAmount;
        public string vpc_TicketNumber;
        public int paymentClientVirtualiStatus;
        
        public void TruncateStringFields()
        {
            if (vpc_Message != null && vpc_Message.Length > 300)
                vpc_Message = vpc_Message.Substring(0, 300);
            if (vpc_CaptureMessage != null && vpc_CaptureMessage.Length > 300)
                vpc_CaptureMessage = vpc_CaptureMessage.Substring(0, 300);
            if (vpc_AVS_Street01 != null && vpc_AVS_Street01.Length > 128)
                vpc_AVS_Street01 = vpc_AVS_Street01.Substring(0, 128);
            if (vpc_AVS_PostCode != null && vpc_AVS_PostCode.Length > 9)
                vpc_AVS_PostCode = vpc_AVS_PostCode.Substring(0, 9);
        }
    }
}
