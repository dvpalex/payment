using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business.Messages
{
    [DefaultMapping(typeof(DPaymentSummary))]
  [ Serializable() ]
	public class MPaymentSummary : Message
	{

        public MPaymentSummary() { }

        private string _name;
        [Mapping(DPaymentSummary.Fields.name)]
        public string Name
          {
              get { return _name; }
              set { _name = value; }
          }


        private int _status;
        [Mapping(DPaymentSummary.Fields.status)]
        public int Status
          {
              get { return _status; }
              set { _status = value; }
          }


		private int _qtde;
        [Mapping(DPaymentSummary.Fields.qtde)]
        public int Qtde
          {
              get { return _qtde; }
              set { _qtde = value; }
          }


		private decimal _total;
		[ Mapping ( DPaymentSummary.Fields.total ) ]
        public decimal Total
          {
              get { return _total; }
              set { _total = value; }
          }



	}

  [Serializable]
    [CollectionOf(typeof(MPaymentSummary))]
    public class MCPaymentSummary : MessageCollection
	{
	}
}