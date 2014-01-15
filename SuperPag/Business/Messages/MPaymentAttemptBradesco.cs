using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAttemptBradesco))]
  [ Serializable() ]
	public class MPaymentAttemptBradesco : Message
	{

		public MPaymentAttemptBradesco() {}

		private Guid _paymentAttemptId;
		[ Mapping ( DPaymentAttemptBradesco.Fields.paymentAttemptId ) ]
		public Guid PaymentAttemptId
          {
		    get { return _paymentAttemptId; }
		    set { _paymentAttemptId = value; }
          }


		private int _agentOrderReference;
		[ Mapping ( DPaymentAttemptBradesco.Fields.agentOrderReference ) ]
		public int AgentOrderReference
          {
		    get { return _agentOrderReference; }
		    set { _agentOrderReference = value; }
          }


		private string _tipoPagto;
		[ Mapping ( DPaymentAttemptBradesco.Fields.tipoPagto ) ]
		public string TipoPagto
          {
		    get { return _tipoPagto; }
		    set { _tipoPagto = value; }
          }


		private string _prazo;
		[ Mapping ( DPaymentAttemptBradesco.Fields.prazo ) ]
		public string Prazo
          {
		    get { return _prazo; }
		    set { _prazo = value; }
          }


		private string _numParcelas;
		[ Mapping ( DPaymentAttemptBradesco.Fields.numparc ) ]
		public string NumParcelas
          {
		    get { return _numParcelas; }
		    set { _numParcelas = value; }
          }


		private string _valorParcela;
		[ Mapping ( DPaymentAttemptBradesco.Fields.valparc ) ]
		public string ValorParcela
          {
		    get { return _valorParcela; }
		    set { _valorParcela = value; }
          }


		private string _total;
		[ Mapping ( DPaymentAttemptBradesco.Fields.valtotal ) ]
		public string Total
          {
		    get { return _total; }
		    set { _total = value; }
          }

		private string _cod;
		[ Mapping ( DPaymentAttemptBradesco.Fields.cod ) ]
		public string Cod
          {
		    get { return _cod; }
		    set { _cod = value; }
          }


		private string _ccname;
		[ Mapping ( DPaymentAttemptBradesco.Fields.ccname ) ]
		public string Ccname
          {
		    get { return _ccname; }
		    set { _ccname = value; }
          }


		private string _ccemail;
		[ Mapping ( DPaymentAttemptBradesco.Fields.ccemail ) ]
		public string Ccemail
          {
		    get { return _ccemail; }
		    set { _ccemail = value; }
          }


		private string _cctype;
		[ Mapping ( DPaymentAttemptBradesco.Fields.cctype ) ]
		public string Cctype
          {
		    get { return _cctype; }
		    set { _cctype = value; }
          }


		private string _assinatura;
		[ Mapping ( DPaymentAttemptBradesco.Fields.assinatura ) ]
		public string Assinatura
          {
		    get { return _assinatura; }
		    set { _assinatura = value; }
          }

	}

  [Serializable]
    [CollectionOf(typeof(MPaymentAttemptBradesco))]
	public class MCPaymentAttemptBradesco : MessageCollection
	{
	}
}