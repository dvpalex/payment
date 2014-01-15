using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAttemptBB))]
  [ Serializable() ]
	public class MPaymentAttemptBB : Message
	{

		public MPaymentAttemptBB() {}

		private Guid _paymentAttemptId;
		[ Mapping ( DPaymentAttemptBB.Fields.paymentAttemptId ) ]
		public Guid PaymentAttemptId
          {
		    get { return _paymentAttemptId; }
		    set { _paymentAttemptId = value; }
          }


		private int _agentOrderReference;
		[ Mapping ( DPaymentAttemptBB.Fields.agentOrderReference ) ]
		public int AgentOrderReference
          {
		    get { return _agentOrderReference; }
		    set { _agentOrderReference = value; }
          }


		private decimal _valor;
		[ Mapping ( DPaymentAttemptBB.Fields.valor ) ]
		public decimal Valor
          {
		    get { return _valor; }
		    set { _valor = value; }
          }


		private int _idConvenio;
		[ Mapping ( DPaymentAttemptBB.Fields.idConvenio ) ]
		public int IdConvenio
          {
		    get { return _idConvenio; }
		    set { _idConvenio = value; }
          }


		private byte _tipoPagamento;
		[ Mapping ( DPaymentAttemptBB.Fields.tipoPagamento ) ]
		public byte TipoPagamento
          {
		    get { return _tipoPagamento; }
		    set { _tipoPagamento = value; }
          }


		private DateTime _dataPagamento;
		[ Mapping ( DPaymentAttemptBB.Fields.dataPagamento ) ]
		public DateTime DataPagamento
          {
		    get { return _dataPagamento; }
		    set { _dataPagamento = value; }
          }


		private string _situacao;
		[ Mapping ( DPaymentAttemptBB.Fields.situacao ) ]
		public string Situacao
          {
		    get { return _situacao; }
		    set { _situacao = value; }
          }


        //private string _mEasyPaget;
        //[ Mapping ( DPaymentAttemptBB.Fields.mEasyPaget ) ]
        //public string MEasyPaget
        //  {
        //    get { return _mEasyPaget; }
        //    set { _mEasyPaget = value; }
        //  }


		private byte _bbpagStatus;
		[ Mapping ( DPaymentAttemptBB.Fields.bbpagStatus ) ]
		public byte BbpagStatus
          {
		    get { return _bbpagStatus; }
		    set { _bbpagStatus = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MPaymentAttemptBB))]
	public class MCPaymentAttemptBB : MessageCollection
	{
	}
}