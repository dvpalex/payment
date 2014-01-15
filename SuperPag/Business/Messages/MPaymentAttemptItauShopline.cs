using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAttemptItauShopline))]
  [ Serializable() ]
	public class MPaymentAttemptItauShopline : Message
	{

		public MPaymentAttemptItauShopline() {}

		private Guid _paymentAttemptId;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.paymentAttemptId ) ]
		public Guid PaymentAttemptId
          {
		    get { return _paymentAttemptId; }
		    set { _paymentAttemptId = value; }
          }


		private int _agentOrderReference;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.agentOrderReference ) ]
		public int AgentOrderReference
          {
		    get { return _agentOrderReference; }
		    set { _agentOrderReference = value; }
          }


		private string _codEmp;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.codEmp ) ]
		public string CodEmp
          {
		    get { return _codEmp; }
		    set { _codEmp = value; }
          }


		private string _valor;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.valor ) ]
		public string Valor
          {
		    get { return _valor; }
		    set { _valor = value; }
          }


		private string _chave;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.chave ) ]
		public string Chave
          {
		    get { return _chave; }
		    set { _chave = value; }
          }


		private string _dc;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.dc ) ]
		public string Dc
          {
		    get { return _dc; }
		    set { _dc = value; }
          }


		private string _tipPag;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.tipPag ) ]
		public string TipPag
          {
		    get { return _tipPag; }
		    set { _tipPag = value; }
          }


		private string _sitPag;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.sitPag ) ]
		public string SitPag
          {
		    get { return _sitPag; }
		    set { _sitPag = value; }
          }


		private string _dtPag;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.dtPag ) ]
		public string DtPag
          {
		    get { return _dtPag; }
		    set { _dtPag = value; }
          }


		private string _codAut;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.codAut ) ]
		public string CodAut
          {
		    get { return _codAut; }
		    set { _codAut = value; }
          }


		private string _numId;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.numId ) ]
		public string NumId
          {
		    get { return _numId; }
		    set { _numId = value; }
          }


		private string _compVend;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.compVend ) ]
		public string CompVend
          {
		    get { return _compVend; }
		    set { _compVend = value; }
          }


		private string _tipCart;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.tipCart ) ]
		public string TipCart
          {
		    get { return _tipCart; }
		    set { _tipCart = value; }
          }


        //private string _mEasyPaget;
        //[ Mapping ( DPaymentAttemptItauShopline.Fields.mEasyPaget ) ]
        //public string MEasyPaget
        //  {
        //    get { return _mEasyPaget; }
        //    set { _mEasyPaget = value; }
        //  }


		private byte _itauStatus;
		[ Mapping ( DPaymentAttemptItauShopline.Fields.itauStatus ) ]
		public byte ItauStatus
          {
		    get { return _itauStatus; }
		    set { _itauStatus = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MPaymentAttemptItauShopline))]
	public class MCPaymentAttemptItauShopline : MessageCollection
	{
	}
}