using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DStore))]
	public interface IStore
	{
        [ MethodType ( MethodTypes.Query ) ]
        DStore[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DStore Locate(int storeId);

        [MethodType(MethodTypes.Query)]
        DStore[] List(int[] storeId);

        [Aggregation(DStore.Fields.storeId, AggregationType.Max)]
        [MethodType(MethodTypes.Query)]
        DStoreMaxId MaxId();

        [MethodType(MethodTypes.Query)]
        DStore Locate(string storeKey);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DStore dStore);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DStore dStore);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int storeId);
	}
}
