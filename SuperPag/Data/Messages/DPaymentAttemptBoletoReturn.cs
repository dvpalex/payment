using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("PaymentAttemptBoletoReturn")]
    public class DPaymentAttemptBoletoReturn : DataMessageBase
    {
        public DPaymentAttemptBoletoReturn() { }

        public class Fields
        {
            public const string paymentAttemptBoletoReturnId = "paymentAttemptBoletoReturnId";
            public const string paymentAttemptId = "paymentAttemptId";
            public const string nossoNumero = "nossoNumero";
            public const string nossoNumeroDV = "nossoNumeroDV";
            public const string comando = "comando";
            public const string naturezaRecebimento = "naturezaRecebimento";
            public const string dataLiquidacao = "dataLiquidacao";
            public const string valorTitulo = "valorTitulo";
            public const string codigoBancoRecebedor = "codigoBancoRecebedor";
            public const string prefixoAgenciaRecebedora = "prefixoAgenciaRecebedora";
            public const string digitoAgenciaRecebedora = "digitoAgenciaRecebedora";
            public const string dataCredito = "dataCredito";
            public const string valorTarifa = "valorTarifa";
            public const string outrasDespesas = "outrasDespesas";
            public const string valorAbatimento = "valorAbatimento";
            public const string valorDescontoConcedido = "valorDescontoConcedido";
            public const string valorRecebido = "valorRecebido";
            public const string jurosMora = "jurosMora";
            public const string outrosRecebimentos = "outrosRecebimentos";
            public const string valorCreditoConta = "valorCreditoConta";
            public const string indicativoCreditoDebito = "indicativoCreditoDebito";
            public const string sequencialRegistro = "sequencialRegistro";
            public const string headerBoletoId = "headerBoletoId";
            public const string creationDate = "creationDate";
            public const string status = "status";
        }

        [PrimaryKey]
        [Identity]
        public int paymentAttemptBoletoReturnId;
        public Guid paymentAttemptId;
        public int nossoNumero;
        public string nossoNumeroDV;
        public int comando;
        public int naturezaRecebimento;
        public DateTime dataLiquidacao;
        public decimal valorTitulo;
        public int codigoBancoRecebedor;
        public int prefixoAgenciaRecebedora;
        public string digitoAgenciaRecebedora;
        public DateTime dataCredito;
        public decimal valorTarifa;
        public decimal outrasDespesas;
        public decimal valorAbatimento;
        public decimal valorDescontoConcedido;
        public decimal valorRecebido;
        public decimal jurosMora;
        public decimal outrosRecebimentos;
        public decimal valorCreditoConta;
        public int indicativoCreditoDebito;
        public int sequencialRegistro;
        public int headerBoletoId;
        public DateTime creationDate;
        public int status;
    }
}
