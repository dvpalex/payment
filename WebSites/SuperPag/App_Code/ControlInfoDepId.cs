using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace SuperPag
{
    [Serializable]
    public class ControlInfoDepId : ControlInfo
    {
        private List<InfoDepId> _infoDepIdList;
        private int bankNumber;
        private int agencyNumber;
        private int agencyDigit;
        private int accountNumber;

        public int AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }
        public int AgencyDigit
        {
            get { return agencyDigit; }
            set { agencyDigit = value; }
        }
        public int AgencyNumber
        {
            get { return agencyNumber; }
            set { agencyNumber = value; }
        }
        public int BankNumber
        {
            get { return bankNumber; }
            set { bankNumber = value; }
        }
        public List<InfoDepId> InfoDepIdList
        {
            get { return _infoDepIdList; }
            set { _infoDepIdList = value; }
        }

        public ControlInfoDepId()
        {
        }
    }

    [Serializable]
    public class InfoDepId
    {
        private decimal _installmentValue;
        private string _idNumber;
        private decimal _totalValue;
        private Guid _paymentAttemptId;
        private int _installmentNumber;

        public decimal InstallmentValue
        {
            get { return _installmentValue; }
            set { _installmentValue = value; }
        }
        public string IdNumber
        {
            get { return _idNumber; }
            set { _idNumber = value; }
        }
        public decimal TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
        }
        public Guid PaymentAttemptId
        {
            get { return _paymentAttemptId; }
            set { _paymentAttemptId = value; }
        }
        public int InstallmentNumber
        {
            get { return _installmentNumber; }
            set { _installmentNumber = value; }
        }

        public InfoDepId()
        {
        }
    }
}
