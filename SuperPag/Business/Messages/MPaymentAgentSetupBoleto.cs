using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentAgentSetupBoleto))]
    [Serializable()]
    public class MPaymentAgentSetupBoleto : Message
    {
        private int _paymentAgentSetupId;
        private int _bankNumber;
        private int _bankDigit;
        private int _agencyNumber;
        private int _agencyDigit;
        private int _accountNumber;
        private int _accountDigit;
        private string _cederCode;
        private string _cederName;
        private string _wallet;
        private string _conventionNumber;
        private int _expirationDays;
        private string _bodyMail;

        public MPaymentAgentSetupBoleto() { }

        [Mapping(DPaymentAgentSetupBoleto.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.bankNumber)]
        public int BankNumber
        {
            get { return _bankNumber; }
            set { _bankNumber = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.bankDigit)]
        public int BankDigit
        {
            get { return _bankDigit; }
            set { _bankDigit = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.agencyNumber)]
        public int AgencyNumber
        {
            get { return _agencyNumber; }
            set { _agencyNumber = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.agencyDigit)]
        public int AgencyDigit
        {
            get { return _agencyDigit; }
            set { _agencyDigit = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.accountNumber)]
        public int AccountNumber
        {
            get { return _accountNumber; }
            set { _accountNumber = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.accountDigit)]
        public int AccountDigit
        {
            get { return _accountDigit; }
            set { _accountDigit = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.cederCode)]
        public string CederCode
        {
            get { return _cederCode; }
            set { _cederCode = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.cederName)]
        public string CederName
        {
            get { return _cederName; }
            set { _cederName = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.wallet)]
        public string Wallet
        {
            get { return _wallet; }
            set { _wallet = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.conventionNumber)]
        public string ConventionNumber
        {
            get { return _conventionNumber; }
            set { _conventionNumber = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.expirationDays)]
        public int ExpirationDays
        {
            get { return _expirationDays; }
            set { _expirationDays = value; }
        }
        [Mapping(DPaymentAgentSetupBoleto.Fields.bodyMail)]
        public string BodyMail
        {
            get { return _bodyMail; }
            set { _bodyMail = value; }
        }
    }

    [Serializable]
    [CollectionOf(typeof(MPaymentAgentSetupBoleto))]
	public class MCPaymentAgentSetupBoleto : MessageCollection
	{
	}
}