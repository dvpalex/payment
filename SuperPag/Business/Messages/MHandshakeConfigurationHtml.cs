using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DHandshakeConfigurationHtml))]
    [Serializable()]
    public class MHandshakeConfigurationHtml : Message
    {
        private int _handshakeConfigurationId;
        private string _urlHandshake;
        private string _urlFinalization;
        private string _urlFinalizationOffline;
        private string _urlReturn;
        private string _urlPaymentConfirmation;
        private string _urlPaymentConfirmationOffline;
        private string _requestEncoding;
        private string _responseEncoding;

        public MHandshakeConfigurationHtml() { }

        [Mapping(DHandshakeConfigurationHtml.Fields.handshakeConfigurationId)]
        public int HandshakeConfigurationId
        {
            get { return _handshakeConfigurationId; }
            set { _handshakeConfigurationId = value; }
        }
        [Mapping(DHandshakeConfigurationHtml.Fields.urlHandshake)]
        public string UrlHandshake
        {
            get { return _urlHandshake; }
            set { _urlHandshake = value; }
        }
        [Mapping(DHandshakeConfigurationHtml.Fields.urlFinalization)]
        public string UrlFinalization
        {
            get { return _urlFinalization; }
            set { _urlFinalization = value; }
        }
        [Mapping(DHandshakeConfigurationHtml.Fields.urlFinalizationOffline)]
        public string UrlFinalizationOffline
        {
            get { return _urlFinalizationOffline; }
            set { _urlFinalizationOffline = value; }
        }
        [Mapping(DHandshakeConfigurationHtml.Fields.urlReturn)]
        public string UrlReturn
        {
            get { return _urlReturn; }
            set { _urlReturn = value; }
        }
        [Mapping(DHandshakeConfigurationHtml.Fields.urlPaymentConfirmation)]
        public string UrlPaymentConfirmation
        {
            get { return _urlPaymentConfirmation; }
            set { _urlPaymentConfirmation = value; }
        }
        [Mapping(DHandshakeConfigurationHtml.Fields.urlPaymentConfirmationOffline)]
        public string UrlPaymentConfirmationOffline
        {
            get { return _urlPaymentConfirmationOffline; }
            set { _urlPaymentConfirmationOffline = value; }
        }
        [Mapping(DHandshakeConfigurationHtml.Fields.requestEncoding)]
        public string RequestEncoding
        {
            get { return _requestEncoding; }
            set { _requestEncoding = value; }
        }
        [Mapping(DHandshakeConfigurationHtml.Fields.responseEncoding)]
        public string ResponseEncoding
        {
            get { return _responseEncoding; }
            set { _responseEncoding = value; }
        }

    }

    [Serializable]
    [CollectionOf(typeof(MHandshakeConfigurationHtml))]
	public class MCHandshakeConfigurationHtml : MessageCollection
	{
	}
}