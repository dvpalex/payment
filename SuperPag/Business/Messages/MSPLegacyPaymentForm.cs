using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DSPLegacyPaymentForm))]
  [ Serializable() ]
	public class MSPLegacyPaymentForm : Message
	{

		public MSPLegacyPaymentForm() {}

		private int _storeId;
		[ Mapping ( DSPLegacyPaymentForm.Fields.storeId ) ]
		public int StoreId
          {
		    get { return _storeId; }
		    set { _storeId = value; }
          }


		private int _paymentFormId;
		[ Mapping ( DSPLegacyPaymentForm.Fields.paymentFormId ) ]
		public int PaymentFormId
          {
		    get { return _paymentFormId; }
		    set { _paymentFormId = value; }
          }


		private string _ucInstructions;
		[ Mapping ( DSPLegacyPaymentForm.Fields.ucInstructions ) ]
		public string UcInstructions
          {
		    get { return _ucInstructions; }
		    set { _ucInstructions = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MSPLegacyPaymentForm))]
	public class MCSPLegacyPaymentForm : MessageCollection
	{
	}
}