using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("ServiceEmailPaymentForm")]
    public class DServiceEmailPaymentForm : DataMessageBase
    {

        public DServiceEmailPaymentForm() { }

        public class Fields
        {
            public const string storeId = "storeId";
            public const string emailType = "emailType";
            public const string paymentFormId = "paymentFormId";
            public const string idioma = "idioma";
            public const string sendHtml = "sendHtml";
            public const string encoding = "encoding";
            public const string bodyTemplate = "bodyTemplate";
            public const string subjectTemplate = "subjectTemplate";
            public const string itensTemplate = "itensTemplate";
            public const string installmentTemplate = "installmentTemplate";
            public const string toField = "toField";
            public const string ccField = "ccField";
            public const string fromField = "fromField";
        }

        [PrimaryKey]
        public int storeId;
        [PrimaryKey]
        public int emailType;
        [PrimaryKey]
        public int paymentFormId;
        [PrimaryKey]
        public string idioma;
        public bool sendHtml;
        public string encoding;
        public string bodyTemplate;
        public string subjectTemplate;
        public string itensTemplate;
        public string installmentTemplate;
        public string toField;
        public string ccField;
        public string fromField;
    }
}

