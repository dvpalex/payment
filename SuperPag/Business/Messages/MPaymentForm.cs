using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentForm))]
    [Serializable()]
    public class MPaymentForm : Message
    {
        private int _paymentFormId;
        private int _paymentFormGroupId;
        private int _paymentAgentId;
        private string _name;
        private MPaymentAgent _paymentAgent;

        public MPaymentForm() { }

        [Mapping(DPaymentForm.Fields.paymentFormId)]
        public int PaymentFormId
        {
            get { return _paymentFormId; }
            set { _paymentFormId = value; }
        }
        [Mapping(DPaymentForm.Fields.paymentFormGroupId)]
        public int PaymentFormGroupId
        {
            get { return _paymentFormGroupId; }
            set { _paymentFormGroupId = value; }
        }
        [Mapping(DPaymentForm.Fields.paymentAgentId)]
        public int PaymentAgentId
        {
            get { return _paymentAgentId; }
            set { _paymentAgentId = value; }
        }
        [Mapping(DPaymentForm.Fields.name)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public MPaymentAgent PaymentAgent
        {
            get { return _paymentAgent; }
            set { _paymentAgent = value; }
        }

    }

    [Serializable]
    [CollectionOf(typeof(MPaymentForm))]
	public class MCPaymentForm : MessageCollection
	{
	}
}