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
    public class ControlInfoBoleto : ControlInfo
    {
        private List<InfoBoleto> _infoBoletoList;

        public List<InfoBoleto> InfoBoletoList
        {
            get { return _infoBoletoList; }
            set { _infoBoletoList = value; }
        }

        public ControlInfoBoleto()
        {
        }
    }

    [Serializable]
    public class InfoBoleto
    {
        private string _sederName;
        private decimal _installmentValue;
        private string _billing;
        private decimal _totalValue;
        private Guid _paymentAttemptId;
        private int _installmentNumber;

        public int InstallmentNumber
        {
            get { return _installmentNumber; }
            set { _installmentNumber = value; }
        }
        public Guid PaymentAttemptId
        {
            get { return _paymentAttemptId; }
            set { _paymentAttemptId = value; }
        }
        public decimal TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; }
        }
        public string Billing
        {
            get { return _billing; }
            set { _billing = value; }
        }
        public decimal InstallmentValue
        {
            get { return _installmentValue; }
            set { _installmentValue = value; }
        }
        public string SederName
        {
            get { return _sederName; }
            set { _sederName = value; }
        }

        public InfoBoleto()
        {
        }
    }
}
