using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentAgentSetupMoset))]
    [Serializable()]
    public class MPaymentAgentSetupMoset : Message
    {
        public MPaymentAgentSetupMoset() { }

        private int _paymentAgentSetupId;
        private string _merchantId;
        private bool _autoCapture;

        [Mapping(DPaymentAgentSetupMoset.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }
        [Mapping(DPaymentAgentSetupMoset.Fields.merchantId)]
        public string MerchantId
        {
            get { return _merchantId; }
            set { _merchantId = value; }
        }
        [Mapping(DPaymentAgentSetupMoset.Fields.autoCapture)]
        public bool AutoCapture
        {
            get { return _autoCapture; }
            set { _autoCapture = value; }
        }
    }
}