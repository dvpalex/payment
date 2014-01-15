using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Framework;
using SuperPag.Data.Messages;

namespace SuperPag.Business.Messages
{
 
    [Serializable()]
    public class MExtrato : Message
    {
        private DateTime _date;
        private int _group;
        private string _storeReferenceOrder;
        private decimal _value;
        private string _consumerName;
        private DateTime _paymentDate;
        private int _instalmentNumber;
        private int _status;
        private Guid _attemptId;

        public enum PaymentAttemptStatus { Pending = 1, Paid = 2, NotPaid = 3, Canceled = 4, PayPending = 5 }
        
        public DateTime Date
        {
            get { return this._date; } 
            set { this._date = value; }
        }      

        public DateTime PaymentDate
        {
            get { return this._paymentDate; }
            set { this._paymentDate = value; }
        }

        public int Group
        {
            get { return this._group; }
            set { this._group = value; }
        }

        public string GroupName
        {
            get 
            {
                return Enum.GetName(typeof(SuperPag.PaymentGroups), this._group);
            }            
        }

        public string StoreReferenceOrder
        {
            get { return this._storeReferenceOrder; }
            set { this._storeReferenceOrder = value; }
        }

        public Guid AttemptId
        {
            get { return this._attemptId; }
            set { this._attemptId = value; }
        }

        public string Document
        {
            get { return string.Format("{0} / {1}", this._storeReferenceOrder, this._instalmentNumber); }
        }


        public string ConsumerName
        {
            get { return this._consumerName; }
            set { this._consumerName = value; }
        }

        public int InstalmentNumber
        {
            get { return this._instalmentNumber; }
            set { this._instalmentNumber = value; }
        }

        public decimal Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        public int Status
        {
            get { return this._status; }
            set { this._status = value; }
        }

        public PaymentAttemptStatus StatusName
        {
            get { return (PaymentAttemptStatus)this._status; }
        }
    }

    [Serializable]
    [CollectionOf(typeof(MExtrato))]
    public class MCExtrato : MessageCollection
    {
    }
}
