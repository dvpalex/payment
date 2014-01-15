using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DBillingSchedule))]
	public interface IBillingSchedule
	{

        [ MethodType ( MethodTypes.Query ) ]
        DBillingSchedule[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DBillingSchedule Locate(int billingScheduleId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DBillingSchedule dBillingSchedule);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DBillingSchedule dBillingSchedule);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int billingScheduleId);


	}
}