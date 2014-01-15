using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DPaymentAgentSetupBradesco))]
  [ Serializable() ]
	public class MPaymentAgentSetupBradesco : Message
	{

		public MPaymentAgentSetupBradesco() {}

		private int _paymentAgentSetupId;
		[ Mapping ( DPaymentAgentSetupBradesco.Fields.paymentAgentSetupId ) ]
		public int PaymentAgentSetupId
          {
		    get { return _paymentAgentSetupId; }
		    set { _paymentAgentSetupId = value; }
          }


		private int _businessNumber;
		[ Mapping ( DPaymentAgentSetupBradesco.Fields.businessNumber ) ]
		public int BusinessNumber
          {
		    get { return _businessNumber; }
		    set { _businessNumber = value; }
          }


		private string _mngLogin;
		[ Mapping ( DPaymentAgentSetupBradesco.Fields.mngLogin ) ]
		public string MngLogin
          {
		    get { return _mngLogin; }
		    set { _mngLogin = value; }
          }


		private string _mngPassword;
		[ Mapping ( DPaymentAgentSetupBradesco.Fields.mngPassword ) ]
		public string MngPassword
          {
		    get { return _mngPassword; }
		    set { _mngPassword = value; }
          }

	}

  [Serializable]
    [CollectionOf(typeof(MPaymentAgentSetupBradesco))]
	public class MCPaymentAgentSetupBradesco : MessageCollection
	{
	}
}