using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DSchedule))]
	public interface ISchedule
	{

        [ MethodType ( MethodTypes.Query ) ]
        DSchedule[] List();

        [MethodType(MethodTypes.Query)]
        DScheduleComplete[] List(long orderId);

        [Where(0, "orderId", "orderId", Filter.Equal, Link.And)]
        [Where(1, "status", "status", Filter.Equal, Link.And)]
        [Where(2, "dateFrom", "date", Filter.GreaterOrEqual, Link.And)]
        [Where(3, "dateTo", "date", Filter.LessOrEqual, Link.And)]
        [MethodType(MethodTypes.Query)]
        DSchedule[] List(long orderId, int status, DateTime dateFrom, DateTime dateTo);

        [Where(0, "orderId", "orderId", Filter.Equal, Link.And)]
        [Where(1, "status", "status", Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DSchedule[] List(long orderId, int status);

        [Where(0, "today", "date", Filter.Equal, Link.And)]
        [Where(1, "status", "status", Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DSchedule[] List(DateTime today, int status);

        [Where(0, "dateFrom", "date", Filter.GreaterOrEqual, Link.And)]
        [Where(1, "dateTo", "date", Filter.LessOrEqual, Link.And)]
        [Where(2, "status", "status", Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DSchedule[] List(DateTime dateFrom, DateTime dateTo, int status);

        [Where(0, "orderId", "orderId", Filter.Equal, Link.And)]
        [Where(1, "today", "date", Filter.GreaterOrEqual, Link.And)]
        [Where(2, "status", "status", Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DSchedule[] ListAfter(long orderId, DateTime today, int status);

        [Where(0, "orderId", "orderId", Filter.Equal, Link.And)]
        [Where(1, "today", "date", Filter.LessOrEqual, Link.And)]
        [Where(2, "status", "status", Filter.Equal, Link.And)]
        [OrderBy(0, "date", SortOrder.DESC)]
        [MethodType(MethodTypes.Query)]
        DSchedule[] ListBefore(long orderId, DateTime today, int status);

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, "installmentNumber", SortOrder.ASC)]
        DScheduleComplete[] ListSortedByNumber(long orderId);

        [ MethodType ( MethodTypes.Query ) ]
        DSchedule Locate(int scheduleId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DSchedule dSchedule);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DSchedule dSchedule);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int scheduleId);


	}
}