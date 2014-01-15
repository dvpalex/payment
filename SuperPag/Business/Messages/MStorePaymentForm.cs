using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DStorePaymentForm))]
    [Serializable()]
    public class MStorePaymentForm : Message
    {
        private int _storeId;
        private int _paymentFormId;
        private int _paymentAgentSetupId;
        private bool _showInCombo;
        private bool _useTestValues;
        private bool _isActive;
        private bool _isInsert = false;
        private MPaymentAgentSetup _paymentAgentSetup;
        private MPaymentForm _paymentForm;
        private MCStorePaymentInstallment _storePaymentInstallments;

        public MStorePaymentForm() { }

        [Mapping(DStorePaymentForm.Fields.storeId)]
        public int StoreId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }
        [Mapping(DStorePaymentForm.Fields.paymentFormId)]
        public int PaymentFormId
        {
            get { return _paymentFormId; }
            set { _paymentFormId = value; }
        }
        [Mapping(DStorePaymentForm.Fields.paymentAgentSetupId)]
        public int PaymentAgentSetupId
        {
            get { return _paymentAgentSetupId; }
            set { _paymentAgentSetupId = value; }
        }
        [Mapping(DStorePaymentForm.Fields.showInCombo)]
        public bool ShowInCombo
        {
            get { return _showInCombo; }
            set { _showInCombo = value; }
        }
        [Mapping(DStorePaymentForm.Fields.useTestValues)]
        public bool UseTestValues
        {
            get { return _useTestValues; }
            set { _useTestValues = value; }
        }
        [Mapping(DStorePaymentForm.Fields.isActive)]
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        public MPaymentAgentSetup PaymentAgentSetup
        {
            get { return _paymentAgentSetup; }
            set { _paymentAgentSetup = value; }
        }
        public MPaymentForm PaymentForm
        {
            get { return _paymentForm; }
            set { _paymentForm = value; }
        }
        public bool IsInsert
        {
            get { return _isInsert; }
            set { _isInsert = value; }
        }
        public MCStorePaymentInstallment StorePaymentInstallments
        {
            get { return _storePaymentInstallments; }
            set { _storePaymentInstallments = value; }
        }
    }

    [Serializable]
    [CollectionOf(typeof(MStorePaymentForm))]
	public class MCStorePaymentForm : MessageCollection
	{
	}
}