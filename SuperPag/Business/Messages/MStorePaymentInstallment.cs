using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DStorePaymentInstallment))]
    [Serializable()]
    public class MStorePaymentInstallment : Message
    {
        public enum InstallmentTypeEnum : byte { Lojista = 1, Operadora = 2, Recorrente = 3 };
        private int _storeId;
        private int _paymentFormId;
        private byte _installmentNumber;
        private string _description;
        private decimal _minValue;
        private decimal _maxValue;
        private decimal _interestPercentage;
        private InstallmentTypeEnum _installmentType;
        private bool _allowInParcialPayment;
        private bool _isNew;
    
        public MStorePaymentInstallment() { }

        [Mapping(DStorePaymentInstallment.Fields.storeId)]
        public int StoreId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }
        [Mapping(DStorePaymentInstallment.Fields.paymentFormId)]
        public int PaymentFormId
        {
            get { return _paymentFormId; }
            set { _paymentFormId = value; }
        }
        [Mapping(DStorePaymentInstallment.Fields.installmentNumber)]
        public byte InstallmentNumber
        {
            get { return _installmentNumber; }
            set { _installmentNumber = value; }
        }
        [Mapping(DStorePaymentInstallment.Fields.description)]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        [Mapping(DStorePaymentInstallment.Fields.minValue)]
        public decimal MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }
        [Mapping(DStorePaymentInstallment.Fields.maxValue)]
        public decimal MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }
        [Mapping(DStorePaymentInstallment.Fields.interestPercentage)]
        public decimal InterestPercentage
        {
            get { return _interestPercentage; }
            set { _interestPercentage = value; }
        }
        [Mapping(DStorePaymentInstallment.Fields.installmentType)]
        public InstallmentTypeEnum InstallmentType
        {
            get { return _installmentType; }
            set { _installmentType = value; }
        }
        [Mapping(DStorePaymentInstallment.Fields.allowInParcialPayment)]
        public bool AllowInParcialPayment
        {
            get { return _allowInParcialPayment; }
            set { _allowInParcialPayment = value; }
        }
        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }
    }

    [Serializable]
    [CollectionOf(typeof(MStorePaymentInstallment))]
	public class MCStorePaymentInstallment : MessageCollection
	{
	}
}