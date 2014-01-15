using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentAgentSetup))]
    [Serializable()]
    public class MPaymentAgentSetup : Message
    {

        public MPaymentAgentSetup() { }

        private int _paymentAgentSetupId = int.MinValue;
        [Mapping(DPaymentAgentSetup.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }


        private int _paymentAgentId;
        [Mapping(DPaymentAgentSetup.Fields.paymentAgentId)]
        public int PaymentAgentId
        {
            get { return _paymentAgentId; }
            set { _paymentAgentId = value; }
        }

        private string _title;
        [Mapping(DPaymentAgentSetup.Fields.title)]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private MPaymentAgent _paymentAgent;
        public MPaymentAgent PaymentAgent
        {
            get { return _paymentAgent; }
            set { _paymentAgent = value; }
        }

    }

    [Serializable]
    [CollectionOf(typeof(MPaymentAgentSetup))]
	public class MCPaymentAgentSetup : MessageCollection
	{
	}
}