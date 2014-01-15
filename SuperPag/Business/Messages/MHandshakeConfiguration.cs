using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DHandshakeConfiguration))]
    [Serializable()]
    public class MHandshakeConfiguration : Message
    {
        public enum HandshakeTypeEnum
        {
            Html = 1,
            Xml = 2
        }

        private int _handshakeConfigurationId;
        private int _storeId;
        private HandshakeTypeEnum _handshakeType;
        private bool _autoPaymentConfirm;
        private bool _acceptDuplicateOrder;
        private bool _validateEmail;
        private bool _sendEmailStoreKeeper;
        private bool _sendEmailConsumer;
        private DateTime _creationDate;
        private DateTime _lastUpdate;
        private bool _isNew = false;

        public MHandshakeConfiguration() { }

        [Mapping(DHandshakeConfiguration.Fields.handshakeConfigurationId)]
        public int HandshakeConfigurationId
        {
            get { return _handshakeConfigurationId; }
            set { _handshakeConfigurationId = value; }
        }
        [Mapping(DHandshakeConfiguration.Fields.storeId)]
        public int StoreId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }
        [Mapping(DHandshakeConfiguration.Fields.handshakeType)]
        public HandshakeTypeEnum HandshakeType
        {
            get { return _handshakeType; }
            set { _handshakeType = value; }
        }
        [Mapping(DHandshakeConfiguration.Fields.autoPaymentConfirm)]
        public bool AutoPaymentConfirm
        {
            get { return _autoPaymentConfirm; }
            set { _autoPaymentConfirm = value; }
        }
        [Mapping(DHandshakeConfiguration.Fields.acceptDuplicateOrder)]
        public bool AcceptDuplicateOrder
        {
            get { return _acceptDuplicateOrder; }
            set { _acceptDuplicateOrder = value; }
        }
        [Mapping(DHandshakeConfiguration.Fields.validateEmail)]
        public bool ValidateEmail
        {
            get { return _validateEmail; }
            set { _validateEmail = value; }
        }
        [Mapping(DHandshakeConfiguration.Fields.sendEmailStoreKeeper)]
        public bool SendEmailStoreKeeper
        {
            get { return _sendEmailStoreKeeper; }
            set { _sendEmailStoreKeeper = value; }
        }
        [Mapping(DHandshakeConfiguration.Fields.sendEmailConsumer)]
        public bool SendEmailConsumer
        {
            get { return _sendEmailConsumer; }
            set { _sendEmailConsumer = value; }
        }
        [Mapping(DHandshakeConfiguration.Fields.creationDate)]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }
        [Mapping(DHandshakeConfiguration.Fields.lastUpdate)]
        public DateTime LastUpdate
        {
            get { return _lastUpdate; }
            set { _lastUpdate = value; }
        }
        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }
    }

    [Serializable]
    [CollectionOf(typeof(MHandshakeConfiguration))]
	public class MCHandshakeConfiguration : MessageCollection
	{
	}
}