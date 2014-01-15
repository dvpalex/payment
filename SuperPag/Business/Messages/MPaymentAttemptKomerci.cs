using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAttemptKomerci))]
  [ Serializable() ]
	public class MPaymentAttemptKomerci : Message
	{

		public MPaymentAttemptKomerci() {}

		private Guid _paymentAttemptId;
		[ Mapping ( DPaymentAttemptKomerci.Fields.paymentAttemptId ) ]
		public Guid PaymentAttemptId
          {
		    get { return _paymentAttemptId; }
		    set { _paymentAttemptId = value; }
          }


		private int _agentOrderReference;
		[ Mapping ( DPaymentAttemptKomerci.Fields.agentOrderReference ) ]
		public int AgentOrderReference
          {
		    get { return _agentOrderReference; }
		    set { _agentOrderReference = value; }
          }


		private string _transacao;
		[ Mapping ( DPaymentAttemptKomerci.Fields.transacao ) ]
		public string Transacao
          {
		    get { return _transacao; }
		    set { _transacao = value; }
          }


		private string _bandeira;
		[ Mapping ( DPaymentAttemptKomerci.Fields.bandeira ) ]
		public string Bandeira
          {
		    get { return _bandeira; }
		    set { _bandeira = value; }
          }


		private string _codver;
		[ Mapping ( DPaymentAttemptKomerci.Fields.codver ) ]
		public string Codver
          {
		    get { return _codver; }
		    set { _codver = value; }
          }


		private string _data;
		[ Mapping ( DPaymentAttemptKomerci.Fields.data ) ]
		public string Data
          {
		    get { return _data; }
		    set { _data = value; }
          }


		private string _nr_cartao;
		[ Mapping ( DPaymentAttemptKomerci.Fields.nr_cartao ) ]
		public string Nr_cartao
          {
		    get { return _nr_cartao; }
		    set { _nr_cartao = value; }
          }


		private string _origem_bin;
		[ Mapping ( DPaymentAttemptKomerci.Fields.origem_bin ) ]
		public string Origem_bin
          {
		    get { return _origem_bin; }
		    set { _origem_bin = value; }
          }


		private string _numautor;
		[ Mapping ( DPaymentAttemptKomerci.Fields.numautor ) ]
		public string Numautor
          {
		    get { return _numautor; }
		    set { _numautor = value; }
          }


		private string _numcv;
		[ Mapping ( DPaymentAttemptKomerci.Fields.numcv ) ]
		public string Numcv
          {
		    get { return _numcv; }
		    set { _numcv = value; }
          }


		private string _numautent;
		[ Mapping ( DPaymentAttemptKomerci.Fields.numautent ) ]
		public string Numautent
          {
		    get { return _numautent; }
		    set { _numautent = value; }
          }


		private string _numsqn;
		[ Mapping ( DPaymentAttemptKomerci.Fields.numsqn ) ]
		public string Numsqn
          {
		    get { return _numsqn; }
		    set { _numsqn = value; }
          }


		private string _codret;
		[ Mapping ( DPaymentAttemptKomerci.Fields.codret ) ]
		public string Codret
          {
		    get { return _codret; }
		    set { _codret = value; }
          }


        //private string _mEasyPaget;
        //[ Mapping ( DPaymentAttemptKomerci.Fields.mEasyPaget ) ]
        //public string MEasyPaget
        //  {
        //    get { return _mEasyPaget; }
        //    set { _mEasyPaget = value; }
        //  }


		private byte _komerciStatus;
		[ Mapping ( DPaymentAttemptKomerci.Fields.komerciStatus ) ]
		public byte KomerciStatus
          {
		    get { return _komerciStatus; }
		    set { _komerciStatus = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MPaymentAttemptKomerci))]
	public class MCPaymentAttemptKomerci : MessageCollection
	{
	}
}