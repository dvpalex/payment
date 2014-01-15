using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag.Business.Messages;


namespace SuperPag.Agents.DepId.Messages
{
    [ DefaultMapping( typeof (DPaymentAttemptDepId))]
    [ Serializable() ]
    public class MPaymentAttemptDepId : MPaymentAttempt
	{
        public MPaymentAttemptDepId() {}

        private Guid _paymentAttemptId;
        [Mapping(DPaymentAttemptDepId.Fields.paymentAttemptId)]
        public new Guid PaymentAttemptId
        {
            get { return _paymentAttemptId; }
            set { _paymentAttemptId = value; }
        }

    	private int _agentOrderReference;
		[ Mapping ( DPaymentAttemptDepId.Fields.agentOrderReference ) ]
		public int AgentOrderReference
          {
		    get { return _agentOrderReference; }
		    set { _agentOrderReference = value; }
          }


        private string _idNumber;
		[ Mapping ( DPaymentAttemptDepId.Fields.idNumber ) ]
        public string IdNumber
        {
          get { return _idNumber; }
          set { _idNumber = value; }
        }


		private DateTime _paymentDate;
		[ Mapping ( DPaymentAttemptDepId.Fields.paymentDate ) ]
		public DateTime PaymentDate
          {
		    get { return _paymentDate; }
		    set { _paymentDate = value; }
          }

        private DateTime _dueDate;
		[ Mapping ( DPaymentAttemptDepId.Fields.dueDate ) ]
        public DateTime DueDate
        {
          get { return _dueDate; }
          set { _dueDate = value; }
        }

        private int _bankNumber;
        [Mapping(DPaymentAttemptDepId.Fields.bankNumber)]
        public int BankNumber
        {
            get { return _bankNumber; }
            set { _bankNumber = value; }
        }

        private int _paymentAttemptDepIdReturnId;
        [Mapping(DPaymentAttemptDepId.Fields.paymentAttemptDepIdReturnId)]
        public int PaymentAttemptDepIdReturnId
          {
              get { return _paymentAttemptDepIdReturnId; }
              set { _paymentAttemptDepIdReturnId = value; }
          }

          private int _paymentStatus;
          [Mapping(DPaymentAttemptDepId.Fields.paymentStatus)]
            public int PaymentStatus
          {
              get { return _paymentStatus; }
              set { _paymentStatus = value; }
          }

        public string PaymentStatusName
        {
            get
            {
                switch (_paymentStatus)
                {
                    case (int)SuperPag.DepIdStatusEnum.ReturnNotFound: return "Sem retorno";
                    case (int)SuperPag.DepIdStatusEnum.AttemptNotFound: return "Tentativa não encontrada";
                    case (int)SuperPag.DepIdStatusEnum.BiggerPaymentValue: return "Pago a maior";
                    case (int)SuperPag.DepIdStatusEnum.LesserPaymentValue: return "Pago a menor";
                    case (int)SuperPag.DepIdStatusEnum.PaymentValueOk: return "Pago";
                    default: return String.Empty;
                }
            }
        }

	}

    [Serializable]
    [CollectionOf(typeof(MPaymentAttemptDepId))]
	public class MCPaymentAttemptDepId : MessageCollection
	{
	}
}