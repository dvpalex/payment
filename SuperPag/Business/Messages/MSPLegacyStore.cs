using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;


namespace SuperPag.Business.Messages
{
    [ DefaultMapping( typeof (DSPLegacyStore))]
  [ Serializable() ]
	public class MSPLegacyStore : Message
	{

		public MSPLegacyStore() {}

		private int _storeId;
		[ Mapping ( DSPLegacyStore.Fields.storeId ) ]
		public int StoreId
          {
		    get { return _storeId; }
		    set { _storeId = value; }
          }


		private string _ucTableTop;
		[ Mapping ( DSPLegacyStore.Fields.ucTableTop ) ]
		public string UcTableTop
          {
		    get { return _ucTableTop; }
		    set { _ucTableTop = value; }
          }


	}

  [Serializable]
    [CollectionOf(typeof(MSPLegacyStore))]
	public class MCSPLegacyStore : MessageCollection
	{
	}
}