using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentAgentSetupDepId))]
    [Serializable()]
    public class MPaymentAgentSetupDepId : Message
    {
        private int _paymentAgentSetupId;
        private int _bankNumber;
        private int _bankDigit;
        private int _agencyNumber;
        private int _agencyDigit;
        private int _accountNumber;
        private int _accountDigit;
        private string _cederName;
        private string _cederCNPJ;
        private string _conventionType;
        private string _calcType;
        private int _expirationDays;
        private int _idPattern;

        public MPaymentAgentSetupDepId() { }

        [Mapping(DPaymentAgentSetupDepId.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.bankNumber)]
        public int BankNumber
        {
            get { return _bankNumber; }
            set { _bankNumber = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.bankDigit)]
        public int BankDigit
        {
            get { return _bankDigit; }
            set { _bankDigit = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.agencyNumber)]
        public int AgencyNumber
        {
            get { return _agencyNumber; }
            set { _agencyNumber = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.agencyDigit)]
        public int AgencyDigit
        {
            get { return _agencyDigit; }
            set { _agencyDigit = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.accountNumber)]
        public int AccountNumber
        {
            get { return _accountNumber; }
            set { _accountNumber = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.accountDigit)]
        public int AccountDigit
        {
            get { return _accountDigit; }
            set { _accountDigit = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.cederName)]
        public string CederName
        {
            get { return _cederName; }
            set { _cederName = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.cederName)]
        public string CederCNPJ
        {
            get { return _cederCNPJ; }
            set { _cederCNPJ = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.conventionType)]
        public string ConventionType
        {
            get { return _conventionType; }
            set { _conventionType = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.calcType)]
        public string CalcType
        {
            get { return _calcType; }
            set { _calcType = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.expirationDays)]
        public int ExpirationDays
        {
            get { return _expirationDays; }
            set { _expirationDays = value; }
        }
        [Mapping(DPaymentAgentSetupDepId.Fields.idPattern)]
        public int IdPattern
        {
            get { return _idPattern; }
            set { _idPattern = value; }
        }
    }

    [Serializable]
    [CollectionOf(typeof(MPaymentAgentSetupDepId))]
    public class MCPaymentAgentSetupDepId : MessageCollection
    {
    }
}
