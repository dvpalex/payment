using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
#if SQL
    [DefaultDataTableName("PaymentAttemptDepIdReturnHdr")]
#elif ORACLE
    [DefaultDataTableName("PaymentAttemptDepIdReturnHeader")]
#else
    [DefaultDataTableName("PaymentAttemptDepIdReturnHdr")]
#endif
    public class DPaymentAttemptDepIdReturnHeader : DataMessageBase
    {

        public DPaymentAttemptDepIdReturnHeader() { }

        public class Fields
        {
            public const string headerId = "headerId";
            public const string bankNumber = "bankNumber";
            public const string sequencialReturnNumber = "sequencialReturnNumber";
            public const string recordFileDate = "recordFileDate";
            public const string agencyNumber = "agencyNumber";
            public const string agencyDV = "agencyDV";
            public const string assignorNumber = "assignorNumber";
            public const string assignorDV = "assignorDV";
            public const string companyCode = "companyCode";
            public const string nameOfCapturedFile = "nameOfCapturedFile";
            public const string creationDateCapturedFile = "creationDateCapturedFile";
            public const string nameOfArquivedFile = "nameOfArquivedFile";
            public const string processDate = "processDate";
            public const string numberOfDetailsRecords = "numberOfDetailsRecords";
        }

        [PrimaryKey]
        [Identity]
        public int headerId;
        public int bankNumber;
        public int sequencialReturnNumber;
        public DateTime recordFileDate;
        public string companyName;
        public int agencyNumber;
        public string agencyDV;
        public int assignorNumber;
        public string assignorDV;
        public int companyCode;
        public string nameOfCapturedFile;
        public DateTime creationDateCapturedFile;
        public string nameOfArquivedFile;
        public DateTime processDate;
        public int numberOfDetailsRecords;
    }
}
