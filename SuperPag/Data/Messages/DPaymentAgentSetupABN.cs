using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("PaymentAgentSetupABN")]
    public class DPaymentAgentSetupABN : DataMessageBase
    {

        public DPaymentAgentSetupABN() { }

        public class Fields
        {
            public const string paymentAgentSetupId = "paymentAgentSetupId";
            public const string codigoABN = "codigoABN";
            public const string urlAction = "urlAction";
            public const string urlConsulta = "urlConsulta";
            public const string tipoFinanciamento = "tipoFinanciamento";
            public const string tabelaFinanciamento = "tabelaFinanciamento";
            public const string garantia = "garantia";
        }

        [PrimaryKey]
        public int paymentAgentSetupId;
        public string codigoABN;
        public string urlAction;
        public string urlConsulta;
        public string tipoFinanciamento;
        public string tabelaFinanciamento;
        public string garantia;
    }
}
