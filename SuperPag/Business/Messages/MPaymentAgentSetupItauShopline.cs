using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAgentSetupItauShopline))]
  [ Serializable() ]
	public class MPaymentAgentSetupItauShopline : Message
	{

		public MPaymentAgentSetupItauShopline() {}

		private int _paymentAgentSetupId;
		[ Mapping ( DPaymentAgentSetupItauShopline.Fields.paymentAgentSetupId ) ]
		public int PaymentAgentSetupId
          {
		    get { return _paymentAgentSetupId; }
		    set { _paymentAgentSetupId = value; }
          }


		private string _criptoKey;
		[ Mapping ( DPaymentAgentSetupItauShopline.Fields.criptoKey ) ]
		public string CriptoKey
          {
		    get { return _criptoKey; }
		    set { _criptoKey = value; }
          }


		private string _businessKey;
		[ Mapping ( DPaymentAgentSetupItauShopline.Fields.businessKey ) ]
		public string BusinessKey
          {
		    get { return _businessKey; }
		    set { _businessKey = value; }
          }


		private string _urlItau;
		[ Mapping ( DPaymentAgentSetupItauShopline.Fields.urlItau ) ]
		public string UrlItau
          {
		    get { return _urlItau; }
		    set { _urlItau = value; }
          }


		private string _urlItauSonda;
		[ Mapping ( DPaymentAgentSetupItauShopline.Fields.urlItauSonda ) ]
		public string UrlItauSonda
          {
		    get { return _urlItauSonda; }
		    set { _urlItauSonda = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MPaymentAgentSetupItauShopline))]
	public class MCPaymentAgentSetupItauShopline : MessageCollection
	{
	}
}