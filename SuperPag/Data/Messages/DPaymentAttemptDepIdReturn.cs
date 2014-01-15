using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("PaymentAttemptDepIdReturn")]
    public class DPaymentAttemptDepIdReturn : DataMessageBase
    {
        public DPaymentAttemptDepIdReturn() { }

        public class Fields
        {
            public const string paymentAttemptDepIdReturnId = "paymentAttemptDepIdReturnId";
            public const string paymentAttemptId = "paymentAttemptId";
            public const string data_deposito = "data_deposito";
            public const string ag_acolhedora = "ag_acolhedora";
            public const string digito_agencia = "digito_agencia";
            public const string remetente_deposito = "remetente_deposito";
            public const string valor_deposito_dinheiro = "valor_deposito_dinheiro";
            public const string valor_deposito_cheque = "valor_deposito_cheque";
            public const string valor_total_deposito = "valor_total_deposito";
            public const string digitoAgenciaRecebedora = "digitoAgenciaRecebedora";
            public const string numero_documento = "numero_documento";
            public const string cod_canal_distribuicao = "cod_canal_distribuicao";
            public const string num_sequencia_arquivo = "num_sequencia_arquivo";
            public const string headerDepIdentId = "headerDepIdentId";
            public const string creationDate = "creationDate";
            public const string status = "status";
        }

        [PrimaryKey]
        [Identity]
        public int paymentAttemptDepIdReturnId;
        public Guid paymentAttemptId;
        public DateTime data_deposito;
        public int ag_acolhedora;
        public int digito_agencia;
        public string remetente_deposito;
        public decimal valor_deposito_dinheiro;
        public decimal valor_deposito_cheque;
        public decimal valor_total_deposito;
        public int digitoAgenciaRecebedora;
        public int numero_documento;
        public int cod_canal_distribuicao;
        public int num_sequencia_arquivo;
        public int headerDepIdentId;
        public DateTime creationDate;
        public int status;
    }
}
