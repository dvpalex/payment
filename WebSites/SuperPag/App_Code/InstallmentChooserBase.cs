using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SuperPag.Data;
using SuperPag.Data.Messages;

namespace SuperPag
{
    public abstract class InstallmentChooserBase : ControlBase
    {
        List<InstallmentInfo> _installments;
        int _storeId;
        int _paymentFormId;

        public int storeId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }

        public int PaymentFormId
        {
            get { return _paymentFormId; }
            set { _paymentFormId = value; }
        }

        public List<InstallmentInfo> Installments
        {
            get { return _installments; }
            set { _installments = value; }
        }

    }

    public class InstallmentInfo
    {
        int _installmentNumber;
        string _installmentString;
        string _installmentValue;
        string _finalAmount;
        string _interestPercentage;

        public int InstallmentNumber
        {
            get { return _installmentNumber; }
            set { _installmentNumber = value; }
        }

        public string InstallmentString
        {
            get { return _installmentString; }
            set { _installmentString = value; }
        }

        public string InstallmentValue
        {
            get { return _installmentValue; }
            set { _installmentValue = value; }
        }

        public string FinalAmount
        {
            get { return _finalAmount; }
            set { _finalAmount = value; }
        }

        public string InterestPercentage
        {
            get { return _interestPercentage; }
            set { _interestPercentage = value; }
        }

        public InstallmentInfo(
            int installmentNumber,
            string installmentString,
            string installmentValue,
            string finalAmount,
            string interestPercentage)
        {
            _installmentNumber = installmentNumber;
            _installmentString = installmentString;
            _installmentValue = installmentValue;
            _finalAmount = finalAmount;
            _interestPercentage = interestPercentage;
        }

    }
}


