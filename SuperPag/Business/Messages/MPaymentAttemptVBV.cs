using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAttemptVBV))]
  [ Serializable() ]
	public class MPaymentAttemptVBV : Message
	{

		public MPaymentAttemptVBV() {}

		private Guid _paymentAttemptId;
		[ Mapping ( DPaymentAttemptVBV.Fields.paymentAttemptId ) ]
		public Guid PaymentAttemptId
          {
		    get { return _paymentAttemptId; }
		    set { _paymentAttemptId = value; }
          }


		private string _tidMaster;
		[ Mapping ( DPaymentAttemptVBV.Fields.tidMaster ) ]
		public string TidMaster
          {
		    get { return _tidMaster; }
		    set { _tidMaster = value; }
          }


		private string _tid;
		[ Mapping ( DPaymentAttemptVBV.Fields.tid ) ]
		public string Tid
          {
		    get { return _tid; }
		    set { _tid = value; }
          }


		private decimal _lr;
		[ Mapping ( DPaymentAttemptVBV.Fields.lr ) ]
		public decimal Lr
          {
		    get { return _lr; }
		    set { _lr = value; }
          }


		private int _arp;
		[ Mapping ( DPaymentAttemptVBV.Fields.arp ) ]
		public int Arp
          {
		    get { return _arp; }
		    set { _arp = value; }
          }


		private string _ars;
		[ Mapping ( DPaymentAttemptVBV.Fields.ars ) ]
		public string Ars
          {
		    get { return _ars; }
		    set { _ars = value; }
          }


		private string _vbvOrderId;
		[ Mapping ( DPaymentAttemptVBV.Fields.vbvOrderId ) ]
		public string VbvOrderId
          {
		    get { return _vbvOrderId; }
		    set { _vbvOrderId = value; }
          }


		private int _price;
		[ Mapping ( DPaymentAttemptVBV.Fields.price ) ]
		public int Price
          {
		    get { return _price; }
		    set { _price = value; }
          }


		private string _free;
		[ Mapping ( DPaymentAttemptVBV.Fields.free ) ]
		public string Free
          {
		    get { return _free; }
		    set { _free = value; }
          }


		private string _pan;
		[ Mapping ( DPaymentAttemptVBV.Fields.pan ) ]
		public string Pan
          {
		    get { return _pan; }
		    set { _pan = value; }
          }


		private int _bank;
		[ Mapping ( DPaymentAttemptVBV.Fields.bank ) ]
		public int Bank
          {
		    get { return _bank; }
		    set { _bank = value; }
          }


		private int _authent;
		[ Mapping ( DPaymentAttemptVBV.Fields.authent ) ]
		public int Authent
          {
		    get { return _authent; }
		    set { _authent = value; }
          }


		private string _cap;
		[ Mapping ( DPaymentAttemptVBV.Fields.cap ) ]
		public string Cap
          {
		    get { return _cap; }
		    set { _cap = value; }
          }


		private byte _vbvStatus;
		[ Mapping ( DPaymentAttemptVBV.Fields.vbvStatus ) ]
		public byte VbvStatus
          {
		    get { return _vbvStatus; }
		    set { _vbvStatus = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MPaymentAttemptVBV))]
	public class MCPaymentAttemptVBV : MessageCollection
	{
	}
}