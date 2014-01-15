using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DRecurrence))]
	public interface IRecurrence
	{

        [ MethodType ( MethodTypes.Query ) ]
        DRecurrence[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DRecurrence Locate(int recurrenceId);

        [MethodType(MethodTypes.Query)]
        DRecurrence Locate(long orderId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DRecurrence dRecurrence);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DRecurrence dRecurrence);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int recurrenceId);


	}
}