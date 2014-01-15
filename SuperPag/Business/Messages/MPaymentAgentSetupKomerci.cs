using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentAgentSetupKomerci))]
    [Serializable()]
    public class MPaymentAgentSetupKomerci : Message
    {
        public MPaymentAgentSetupKomerci() { }

        private int _paymentAgentSetupId;
        private int _businessNumber;
        private string _instalmentType;
        private string _urlKomerci;
        private string _urlKomerciConfirm;
        private string _urlKomerciAVS;
        private bool _checkAVS;

        [Mapping(DPaymentAgentSetupKomerci.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }
        [Mapping(DPaymentAgentSetupKomerci.Fields.businessNumber)]
        public int BusinessNumber
        {
            get { return _businessNumber; }
            set { _businessNumber = value; }
        }
        [Mapping(DPaymentAgentSetupKomerci.Fields.instalmentType)]
        public string InstalmentType
        {
            get { return _instalmentType; }
            set { _instalmentType = value; }
        }
        [Mapping(DPaymentAgentSetupKomerci.Fields.urlKomerci)]
        public string UrlKomerci
        {
            get { return _urlKomerci; }
            set { _urlKomerci = value; }
        }
        [Mapping(DPaymentAgentSetupKomerci.Fields.urlKomerciConfirm)]
        public string UrlKomerciConfirm
        {
            get { return _urlKomerciConfirm; }
            set { _urlKomerciConfirm = value; }
        }
        [Mapping(DPaymentAgentSetupKomerci.Fields.urlKomerciAVS)]
        public string UrlKomerciAVS
        {
            get { return _urlKomerciAVS; }
            set { _urlKomerciAVS = value; }
        }
        [Mapping(DPaymentAgentSetupKomerci.Fields.checkAVS)]
        public bool CheckAVS
        {
            get { return _checkAVS; }
            set { _checkAVS = value; }
        }

    }

  [Serializable]
    [CollectionOf(typeof(MPaymentAgentSetupKomerci))]
	public class MCPaymentAgentSetupKomerci : MessageCollection
	{
	}
}