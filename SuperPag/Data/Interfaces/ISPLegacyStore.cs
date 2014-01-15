using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DSPLegacyStore))]
	public interface ISPLegacyStore
	{

        [ MethodType ( MethodTypes.Query ) ]
        DSPLegacyStore[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DSPLegacyStore Locate(int storeId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DSPLegacyStore dSPLegacyStore);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DSPLegacyStore dSPLegacyStore);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int storeId);


	}
}