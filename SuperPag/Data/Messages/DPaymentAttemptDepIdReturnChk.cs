using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("PaymentAttemptDepIdReturnChk")]
    public class DPaymentAttemptDepIdReturnChk : DataMessageBase
    {
        public DPaymentAttemptDepIdReturnChk() { }

        public class Fields
        {
            public const string checkId = "checkId";
            public const string paymentAttemptDepIdReturnId = "paymentAttemptDepIdReturnId";
            public const string data_deposito_cheque = "data_deposito_cheque";
            public const string ag_acolhedora_cheque = "ag_acolhedora_cheque";
            public const string dig_acolhedora_cheque = "dig_acolhedora_cheque";
            public const string vlr_deposito_total = "vlr_deposito_total";
            public const string vlr_cheque = "vlr_cheque";
            public const string cod_banco = "cod_banco";
            public const string cod_agencia_cheque = "cod_agencia_cheque";
            public const string numero_cheque = "numero_cheque";
            public const string sequencia_arquivo = "sequencia_arquivo";
            public const string creationDate = "creationDate";
            public const string status = "status";
            public const string cod_depositante_cheque = "cod_depositante_cheque";
        }

        [PrimaryKey]
        [Identity]
        public int checkId;
        public int paymentAttemptDepIdReturnId;
        public DateTime data_deposito_cheque;
        public int ag_acolhedora_cheque;
        public int dig_acolhedora_cheque;
        public decimal vlr_deposito_total;
        public decimal vlr_cheque;
        public int cod_banco;
        public int cod_agencia_cheque;
        public string numero_cheque;
        public int sequencia_arquivo;
        public DateTime creationDate;
        public int status;
        public string cod_depositante_cheque;
    }
}
