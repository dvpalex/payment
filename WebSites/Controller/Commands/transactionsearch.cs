using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
	public class SearchTransaction : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int[] storeIds = new int[] { ControllerContext.StoreId };
            MCStore mcStore = Store.List(storeIds);
            MCPaymentForm mcPaymentForms = PaymentForm.ListFromClient(ControllerContext.StoreId);

            this.AddMessage(mcStore);
            this.AddMessage(mcPaymentForms);
            this.AddMessage(new MTransactionSearch());
            this.AddEnumeration(new EnumListBuilder(typeof(MPaymentAttempt.PaymentAttemptStatus)));
            this.AddEnumeration(new EnumListBuilder(typeof(MOrder.OrderStatus)));

            return Map.Views.SearchTransaction;
		}
	}

    [Serializable()]
    public class MTransactionSearch : Message
    {
        private Guid _paymentAttemptId = Guid.Empty;
        private int _storeId = int.MinValue;
        private string _storeReferenceOrder = string.Empty;
        private DateTime _orderDateFrom = DateTime.MinValue;
        private DateTime _orderDateTo = DateTime.MinValue;
        private DateTime _orderTimeFrom = DateTime.MinValue;
        private DateTime _orderTimeTo = DateTime.MinValue;
        private String _consumerName;
        private String _cpf;
        private String _cnpj;
        private MPaymentAttempt.PaymentAttemptStatus _status;
        private MOrder.OrderStatus _orderStatus = (MOrder.OrderStatus)int.MinValue;
        private int _paymentFormId = short.MinValue;

        public Guid PaymentAttemptId
        {
            get { return _paymentAttemptId; }
            set { _paymentAttemptId = value; }
        }

        public int StoreId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }

        public string StoreReferenceOrder
        {
            get { return _storeReferenceOrder; }
            set { _storeReferenceOrder = value; }
        }

        public DateTime OrderDateFrom
        {
            get { return _orderDateFrom; }
            set { _orderDateFrom = value; }
        }

        public DateTime OrderDateTo
        {
            get { return _orderDateTo; }
            set { _orderDateTo = value; }
        }

        public DateTime OrderTimeFrom
        {
            get { return _orderTimeFrom; }
            set { _orderTimeFrom = value; }
        }

        public DateTime OrderTimeTo
        {
            get { return _orderTimeTo; }
            set { _orderTimeTo = value; }
        }

        public String ConsumerName
        {
            get { return _consumerName; }
            set { _consumerName = value; }
        }

        public String Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }

        public String Cnpj
        {
            get { return _cnpj; }
            set { _cnpj = value; }
        }

        public MPaymentAttempt.PaymentAttemptStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public MOrder.OrderStatus OrderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }

        public int PaymentFormId
        {
            get { return _paymentFormId; }
            set { _paymentFormId = value; }
        }
    }

}
