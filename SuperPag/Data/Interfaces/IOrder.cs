using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DOrder))]
	public interface IOrder
	{

        [ MethodType ( MethodTypes.Query ) ]
        DOrder[] List();

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DOrder.Fields.lastUpdateDate, SortOrder.DESC)]
        DOrder[] List(int storeId, string storeReferenceOrder);

        [DataRelation(typeof(DOrder), DOrder.Fields.orderId, typeof(DPaymentAttempt), DPaymentAttempt.Fields.orderId, Join.Left)]
        [DataRelation(typeof(DOrder), DOrder.Fields.orderId, typeof(DRecurrence), DRecurrence.Fields.orderId, Join.Left)]
        [DataRelation(typeof(DOrder), "consumerId", typeof(DConsumer), "consumerId", Join.Left)]
        [Where(0, "storeId", "storeId", Filter.Equal, Link.And)]
        [Where(1, "startTimeFrom", "creationDate", Filter.GreaterOrEqual, Link.And)]
        [Where(2, "startTimeTo", "creationDate", Filter.LessOrEqual, Link.And)]
        [Where(3, "paymentFormId", typeof(DPaymentAttempt), "paymentFormId", Filter.Equal, Link.And)]
        [Where(4, "status", typeof(DPaymentAttempt), "status", Filter.Equal, Link.And)]
        [Where(5, "consumerName", typeof(DConsumer), "name", Filter.LikeLeftRight, Link.And)]
        [Where(6, "CPF", typeof(DConsumer), "CPF", Filter.Equal, Link.And)]
        [Where(7, "CNPJ", typeof(DConsumer), "CNPJ", Filter.Equal, Link.And)]
        [Where(8, "orderStatus", "status", Filter.Equal, Link.And)]
        [Where(9, "recurrenceStatus", typeof(DRecurrence), DRecurrence.Fields.status, Filter.Equal, Link.And)]
        [OrderBy(0, "storeReferenceOrder", SortOrder.ASC)]
        [MethodType(MethodTypes.Query)]
        DOrder[] List(int storeId, DateTime startTimeFrom, DateTime startTimeTo, int paymentFormId, int status,
            string consumerName, string CPF, string CNPJ, int orderStatus, int recurrenceStatus);       

        [ MethodType ( MethodTypes.Query ) ]
        DOrder Locate(long orderId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DOrder dOrder);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DOrder dOrder);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(long orderId);

        [MethodType(MethodTypes.Update)]
        void Update(long set_consumerId, long by_orderId);
    }
}