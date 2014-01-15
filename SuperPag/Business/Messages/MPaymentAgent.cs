using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAgent))]
  [ Serializable() ]
	public class MPaymentAgent : Message
	{
		public MPaymentAgent() {}

		private int _paymentAgentId;
		[ Mapping ( DPaymentAgent.Fields.paymentAgentId ) ]
		public int PaymentAgentId
          {
		    get { return _paymentAgentId; }
		    set { _paymentAgentId = value; }
          }


		private string _name;
		[ Mapping ( DPaymentAgent.Fields.name ) ]
		public string Name
          {
		    get { return _name; }
		    set { _name = value; }
          }


		private string _webPage;
		[ Mapping ( DPaymentAgent.Fields.webPage ) ]
		public string WebPage
          {
		    get { return _webPage; }
		    set { _webPage = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MPaymentAgent))]
	public class MCPaymentAgent : MessageCollection
	{
	}
}