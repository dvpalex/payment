using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentAgentSetupVBV))]
    [Serializable()]
    public class MPaymentAgentSetupVBV : Message
    {

        public MPaymentAgentSetupVBV() { }

        private int _paymentAgentSetupId;
        [Mapping(DPaymentAgentSetupVBV.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }


        private int _businessNumber;
        [Mapping(DPaymentAgentSetupVBV.Fields.businessNumber)]
        public int BusinessNumber
        {
            get { return _businessNumber; }
            set { _businessNumber = value; }
        }

        private bool _autoCapture;
        [Mapping(DPaymentAgentSetupVBV.Fields.autoCapture)]
        public bool AutoCapture
        {
            get { return _autoCapture; }
            set { _autoCapture = value; }
        }
    }

  [Serializable]
    [CollectionOf(typeof(MPaymentAgentSetupVBV))]
	public class MCPaymentAgentSetupVBV : MessageCollection
	{
	}
}