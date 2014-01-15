using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentAgentSetupPaymentClientVirtual))]
    [Serializable()]
    public class MPaymentAgentSetupPaymentClientVirtual : Message
    {
        public MPaymentAgentSetupPaymentClientVirtual() { }

        private int _paymentAgentSetupId = int.MinValue;
        private string _accessCode;
        private string _secureHashSecret;
        private string _merchantId;
        private bool _checkAVS;
        private string _version;
        private string _captureUser;
        private string _capturePassword;
        private bool _autoCapture;

        [Mapping(DPaymentAgentSetupPaymentClientVirtual.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }
        [Mapping(DPaymentAgentSetupPaymentClientVirtual.Fields.accessCode)]
        public string AccessCode
        {
            get { return _accessCode; }
            set { _accessCode = value; }
        }
        [Mapping(DPaymentAgentSetupPaymentClientVirtual.Fields.secureHashSecret)]
        public string SecureHashSecret
        {
            get { return _secureHashSecret; }
            set { _secureHashSecret = value; }
        }
        [Mapping(DPaymentAgentSetupPaymentClientVirtual.Fields.merchantId)]
        public string MerchantId
        {
            get { return _merchantId; }
            set { _merchantId = value; }
        }
        [Mapping(DPaymentAgentSetupPaymentClientVirtual.Fields.checkAVS)]
        public bool CheckAVS
        {
            get { return _checkAVS; }
            set { _checkAVS = value; }
        }
        [Mapping(DPaymentAgentSetupPaymentClientVirtual.Fields.version)]
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        [Mapping(DPaymentAgentSetupPaymentClientVirtual.Fields.captureUser)]
        public string CaptureUser
        {
            get { return _captureUser; }
            set { _captureUser = value; }
        }
        [Mapping(DPaymentAgentSetupPaymentClientVirtual.Fields.capturePassword)]
        public string CapturePassword
        {
            get { return _capturePassword; }
            set { _capturePassword = value; }
        }
        [Mapping(DPaymentAgentSetupPaymentClientVirtual.Fields.autoCapture)]
        public bool AutoCapture
        {
            get { return _autoCapture; }
            set { _autoCapture = value; }
        }
    }
}