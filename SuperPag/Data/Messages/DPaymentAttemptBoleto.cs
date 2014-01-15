using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("PaymentAttemptBoleto")]
    public class DPaymentAttemptBoleto : DataMessageBase
    {
        public DPaymentAttemptBoleto() { }

        public class Fields
        {
            public const string paymentAttemptId = "paymentAttemptId";
            public const string agentOrderReference = "agentOrderReference";
            public const string documentNumber = "documentNumber";
            public const string withdraw = "withdraw";
            public const string withdrawDoc = "withdrawDoc";
            public const string address1 = "address1";
            public const string address2 = "address2";
            public const string address3 = "address3";
            public const string oct = "oct";
            public const string barCode = "barCode";
            public const string ourNumber = "ourNumber";
            public const string instructions = "instructions";
            public const string paymentDate = "paymentDate";
            public const string expirationPaymentDate = "expirationPaymentDate";
            public const string paymentAttemptBoletoReturnId = "paymentAttemptBoletoReturnId";
            public const string UserId = "UserId";
            public const string Status = "Status";
            public const string Contrato = "Contrato";
            public const string ErrorMail = "ErrorMail";
        }

        [PrimaryKey]
        public Guid paymentAttemptId;
        [Identity]
        public int agentOrderReference;
        public string documentNumber;
        public string withdraw;
        public string withdrawDoc;
        public string address1;
        public string address2;
        public string address3;
        public string oct;
        public string barCode;
        public string ourNumber;
        public string instructions;
        public DateTime paymentDate;
        public DateTime expirationPaymentDate;
        public int paymentAttemptBoletoReturnId;
        public Guid UserId;
        public bool Status;
        public string Contrato;
        public string ErrorMail;
    }
}
