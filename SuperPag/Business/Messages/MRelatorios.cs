using System;
using System.Collections.Generic;
using System.Text;

namespace SuperPag.Business.Messages
{
    public class MRelPontoCredAnalitico
    {
        public MRelPontoCredAnalitico(DateTime PaymentDate, string UserName, string BarCode,
                                      DateTime ExpirationPaymentDate, string MeioPagamento,
                                      string Sacado, string Contrato, bool Status, string ErrorMail                          
                                     )
        {
            this.PaymentDate = PaymentDate;
            this.UserName = UserName;
            this.BarCode = BarCode;
            this.ExpirationPaymentDate = ExpirationPaymentDate;
            this.MeioEnvio = MeioPagamento;
            this.Sacado = Sacado;
            this.Contrato = Contrato;
            this.Status = Status;
            this.ErrorMail = ErrorMail;
        }

        private DateTime _paymentDate;
        private string _UserName;
        private string _barCode;
        private DateTime _expirationPaymentDate;
        private string _MeioEnvio;
        private string _Sacado;
        private string _Contrato;
        private bool _Status;
        private string _ErrorMail;        

        public DateTime PaymentDate
        {
            get { return _paymentDate; }
            set { _paymentDate = value; }
        }
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        public string BarCode
        {
            get { return _barCode; }
            set { _barCode = value; }
        }
        public DateTime ExpirationPaymentDate
        {
            get { return _expirationPaymentDate; }
            set { _expirationPaymentDate = value; }
        }
        public string MeioEnvio
        {
            get { return _MeioEnvio; }
            set { _MeioEnvio = value; }
        }
        public string Sacado
        {
            get { return _Sacado; }
            set { _Sacado = value; }
        }
        public string Contrato
        {
            get { return _Contrato; }
            set { _Contrato = value; }
        }
        public bool Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string ErrorMail
        {
            get { return _ErrorMail; }
            set { _ErrorMail = value; }
        }

    }
}
