using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DSPLegacyPaymentGroup))]
  [ Serializable() ]
	public class MSPLegacyPaymentGroup : Message
	{

		public MSPLegacyPaymentGroup() {}

		private int _storeId;
		[ Mapping ( DSPLegacyPaymentGroup.Fields.storeId ) ]
		public int StoreId
          {
		    get { return _storeId; }
		    set { _storeId = value; }
          }


		private int _paymentFormGroupId;
		[ Mapping ( DSPLegacyPaymentGroup.Fields.paymentFormGroupId ) ]
		public int PaymentFormGroupId
          {
		    get { return _paymentFormGroupId; }
		    set { _paymentFormGroupId = value; }
          }


		private string _ucInstructions;
		[ Mapping ( DSPLegacyPaymentGroup.Fields.ucInstructions ) ]
		public string UcInstructions
          {
		    get { return _ucInstructions; }
		    set { _ucInstructions = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MSPLegacyPaymentGroup))]
	public class MCSPLegacyPaymentGroup : MessageCollection
	{
	}
}