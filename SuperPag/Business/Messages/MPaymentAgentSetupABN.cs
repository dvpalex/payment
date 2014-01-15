using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentAgentSetupABN))]
    [Serializable()]
    public class MPaymentAgentSetupABN : Message
    {
        public MPaymentAgentSetupABN() { }

        private int _paymentAgentSetupId = int.MinValue;
        private string _codigoABN;
        private string _urlAction;
        private string _urlConsulta;
        private string _tipoFinanciamento;
        private string _tabelaFinanciamento;
        private string _garantia;

        [Mapping(DPaymentAgentSetupABN.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }
        [Mapping(DPaymentAgentSetupABN.Fields.codigoABN)]
        public string CodigoABN
        {
            get { return _codigoABN; }
            set { _codigoABN = value; }
        }
        [Mapping(DPaymentAgentSetupABN.Fields.urlAction)]
        public string UrlAction
        {
            get { return _urlAction; }
            set { _urlAction = value; }
        }
        [Mapping(DPaymentAgentSetupABN.Fields.urlConsulta)]
        public string UrlConsulta
        {
            get { return _urlConsulta; }
            set { _urlConsulta = value; }
        }
        [Mapping(DPaymentAgentSetupABN.Fields.tipoFinanciamento)]
        public string TipoFinanciamento
        {
            get { return _tipoFinanciamento; }
            set { _tipoFinanciamento = value; }
        }
        [Mapping(DPaymentAgentSetupABN.Fields.tabelaFinanciamento)]
        public string TabelaFinanciamento
        {
            get { return _tabelaFinanciamento; }
            set { _tabelaFinanciamento = value; }
        }
        [Mapping(DPaymentAgentSetupABN.Fields.garantia)]
        public string Garantia
        {
            get { return _garantia; }
            set { _garantia = value; }
        }
    }
}