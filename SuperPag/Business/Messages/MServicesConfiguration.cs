using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DServicesConfiguration))]
  [ Serializable() ]
	public class MServicesConfiguration : Message
	{

		public MServicesConfiguration() {}

		private int _storeId;
		[ Mapping ( DServicesConfiguration.Fields.storeId ) ]
		public int StoreId
          {
		    get { return _storeId; }
		    set { _storeId = value; }
          }


		private int _offLineFinalizationRetries;
		[ Mapping ( DServicesConfiguration.Fields.offLineFinalizationRetries ) ]
		public int OffLineFinalizationRetries
          {
		    get { return _offLineFinalizationRetries; }
		    set { _offLineFinalizationRetries = value; }
          }


		private int _offLinePaymentRetries;
		[ Mapping ( DServicesConfiguration.Fields.offLinePaymentRetries ) ]
		public int OffLinePaymentRetries
          {
		    get { return _offLinePaymentRetries; }
		    set { _offLinePaymentRetries = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MServicesConfiguration))]
	public class MCServicesConfiguration : MessageCollection
	{
	}
}