using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DHandshakeConfigurationXml))]
    [Serializable()]
    public class MHandshakeConfigurationXml : Message
    {
        private int _handshakeConfigurationId;
        private string _urlHandshake;
        private string _urlHandshakeError;
        private string _urlFinalization;
        private string _urlFinalizationOffline;
        private string _urlReturn;
        private string _urlPaymentConfirmation;
        private string _urlPaymentConfirmationOffline;
        private string _requestEncoding;
        private string _responseEncoding;

        public MHandshakeConfigurationXml() { }

        [Mapping(DHandshakeConfigurationXml.Fields.handshakeConfigurationId)]
        public int HandshakeConfigurationId
        {
            get { return _handshakeConfigurationId; }
            set { _handshakeConfigurationId = value; }
        }
        [Mapping(DHandshakeConfigurationXml.Fields.urlHandshake)]
        public string UrlHandshake
        {
            get { return _urlHandshake; }
            set { _urlHandshake = value; }
        }
        [Mapping(DHandshakeConfigurationXml.Fields.urlHandshakeError)]
        public string UrlHandshakeError
        {
            get { return _urlHandshakeError; }
            set { _urlHandshakeError = value; }
        }
        [Mapping(DHandshakeConfigurationXml.Fields.urlFinalization)]
        public string UrlFinalization
        {
            get { return _urlFinalization; }
            set { _urlFinalization = value; }
        }
        [Mapping(DHandshakeConfigurationXml.Fields.urlFinalizationOffline)]
        public string UrlFinalizationOffline
        {
            get { return _urlFinalizationOffline; }
            set { _urlFinalizationOffline = value; }
        }
        [Mapping(DHandshakeConfigurationXml.Fields.urlReturn)]
        public string UrlReturn
        {
            get { return _urlReturn; }
            set { _urlReturn = value; }
        }
        [Mapping(DHandshakeConfigurationXml.Fields.urlPaymentConfirmation)]
        public string UrlPaymentConfirmation
        {
            get { return _urlPaymentConfirmation; }
            set { _urlPaymentConfirmation = value; }
        }
        [Mapping(DHandshakeConfigurationXml.Fields.urlPaymentConfirmationOffline)]
        public string UrlPaymentConfirmationOffline
        {
            get { return _urlPaymentConfirmationOffline; }
            set { _urlPaymentConfirmationOffline = value; }
        }
        [Mapping(DHandshakeConfigurationXml.Fields.requestEncoding)]
        public string RequestEncoding
        {
            get { return _requestEncoding; }
            set { _requestEncoding = value; }
        }
        [Mapping(DHandshakeConfigurationXml.Fields.responseEncoding)]
        public string ResponseEncoding
        {
            get { return _responseEncoding; }
            set { _responseEncoding = value; }
        }

    }

    [Serializable]
    [CollectionOf(typeof(MHandshakeConfigurationXml))]
	public class MCHandshakeConfigurationXml : MessageCollection
	{
	}
}