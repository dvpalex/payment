using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DStore))]
    [Serializable()]

    public class MStore : Message
    {
        #region CONSTRUTORES
        public MStore() { }

        public MStore(int storeId, string name)
        {
            this.StoreId = storeId;
            this.Name = name;
        }
        public MStore(int storeId, string name, string storeKey)
        {
            this.StoreId = storeId;
            this.Name = name;
            this.StoreKey = storeKey;
        }
        public MStore(int storeId,
                       string name,
                       string urlSite,
                       string storeKey,
                       string password,
                       int handshakeConfigurationId,
                       DateTime creationDate,
                       DateTime lastUpdate,
                       string mailSenderEmail,
                       MCStorePaymentForm storePaymentForms,
                     MHandshakeConfiguration handshakeConfiguration,
                     bool isNew
                      )
        {
            this.StoreId = storeId;
            this.Name = name;
            this.UrlSite = urlSite;
            this.StoreKey = storeKey;
            this.Password = password;
            this.HandshakeConfigurationId = handshakeConfigurationId;
            this.CreationDate = creationDate;
            this.LastUpdate = lastUpdate;
            this.MailSenderEmail = mailSenderEmail;
            this.StorePaymentForms = storePaymentForms;
            this.HandshakeConfiguration = handshakeConfiguration;
            this.IsNew = isNew;
        }
        #endregion

        private int _storeId = int.MinValue;
        private string _name;
        private string _urlSite;
        private string _storeKey;
        private string _password;
        private int _handshakeConfigurationId;
        private DateTime _creationDate;
        private DateTime _lastUpdate;
        private string _mailSenderEmail;
        private MCStorePaymentForm _storePaymentForms;
        private MHandshakeConfiguration _handshakeConfiguration;
        private bool _isNew = false;

        [Mapping(DStore.Fields.storeId)]
        public int StoreId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }
        [Mapping(DStore.Fields.name)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        [Mapping(DStore.Fields.urlSite)]
        public string UrlSite
        {
            get { return _urlSite; }
            set { _urlSite = value; }
        }

        [Mapping(DStore.Fields.storeKey)]
        public string StoreKey
        {
            get { return _storeKey; }
            set { _storeKey = value; }
        }
        [Mapping(DStore.Fields.password)]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        [Mapping(DStore.Fields.handshakeConfigurationId)]
        public int HandshakeConfigurationId
        {
            get { return _handshakeConfigurationId; }
            set { _handshakeConfigurationId = value; }
        }
        [Mapping(DStore.Fields.creationDate)]
        public DateTime CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }
        [Mapping(DStore.Fields.lastUpdate)]
        public DateTime LastUpdate
        {
            get { return _lastUpdate; }
            set { _lastUpdate = value; }
        }
        [Mapping(DStore.Fields.mailSenderEmail)]
        public string MailSenderEmail
        {
            get { return _mailSenderEmail; }
            set { _mailSenderEmail = value; }
        }
        public MCStorePaymentForm StorePaymentForms
        {
            get { return _storePaymentForms; }
            set { _storePaymentForms = value; }
        }
        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }
        public MHandshakeConfiguration HandshakeConfiguration
        {
            get { return _handshakeConfiguration; }
            set { _handshakeConfiguration = value; }
        }

    }

    [Serializable]
    [CollectionOf(typeof(MStore))]
    public class MCStore : MessageCollection
    {
    }
}