using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentAgentSetupBB))]
    [Serializable()]
    public class MPaymentAgentSetupBB : Message
    {

        public MPaymentAgentSetupBB() { }

        private int _paymentAgentSetupId;
        [Mapping(DPaymentAgentSetupBB.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }


        private int _agentOrderReferenceCounter;
        [Mapping(DPaymentAgentSetupBB.Fields.agentOrderReferenceCounter)]
        public int AgentOrderReferenceCounter
        {
            get { return _agentOrderReferenceCounter; }
            set { _agentOrderReferenceCounter = value; }
        }


        private int _businessNumber;
        [Mapping(DPaymentAgentSetupBB.Fields.businessNumber)]
        public int BusinessNumber
        {
            get { return _businessNumber; }
            set { _businessNumber = value; }
        }


        private int _daysExpirationDate;
        [Mapping(DPaymentAgentSetupBB.Fields.daysExpirationDate)]
        public int DaysExpirationDate
        {
            get { return _daysExpirationDate; }
            set { _daysExpirationDate = value; }
        }


        private string _urlBBPag;
        [Mapping(DPaymentAgentSetupBB.Fields.urlBBPag)]
        public string UrlBBPag
        {
            get { return _urlBBPag; }
            set { _urlBBPag = value; }
        }


        private string _urlBBPagSonda;
        [Mapping(DPaymentAgentSetupBB.Fields.urlBBPagSonda)]
        public string UrlBBPagSonda
        {
            get { return _urlBBPagSonda; }
            set { _urlBBPagSonda = value; }
        }
    }

  [Serializable]
    [CollectionOf(typeof(MPaymentAgentSetupBB))]
	public class MCPaymentAgentSetupBB : MessageCollection
	{
	}
}