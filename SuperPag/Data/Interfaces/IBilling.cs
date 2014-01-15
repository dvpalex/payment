using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DBilling))]
	public interface IBilling
	{

        [ MethodType ( MethodTypes.Query ) ]
        DBilling[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DBilling Locate(int billingId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DBilling dBilling);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DBilling dBilling);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int billingId);


	}
}