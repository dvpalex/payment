using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttempt))]
	public interface IPaymentAttempt
	{
        
        [Where(0, "storeId", typeof(DOrder), "storeId", Filter.Equal, Link.And)]
        [Where(1, "startTimeFrom", "startTime", Filter.GreaterOrEqual, Link.And)]
        [Where(2, "startTimeTo", "startTime", Filter.LessOrEqual, Link.And)]
        [Where(3, "status", "status", Filter.Equal, Link.And)]
        [Where(4, Block.Begin, Link.And)]
        [Where(5, "paymentFormId", "paymentFormId", Filter.Equal, Link.Or)]
        [Where(6, Block.End, Link.And)]        
        [OrderBy(0, typeof(DOrder), "storeReferenceOrder", SortOrder.ASC)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptComplete[] List(int storeId, DateTime startTimeFrom, DateTime startTimeTo, int status, int[] paymentFormId);

        [DataRelation(typeof(DPaymentAttemptComplete),DPaymentAttemptComplete.Fields.paymentAttemptId, typeof(DPaymentAttemptBoleto), DPaymentAttemptBoleto.Fields.paymentAttemptId, Join.Left)]
        [DataRelation(typeof(DPaymentAttemptBoleto), DPaymentAttemptBoleto.Fields.agentOrderReference, typeof(DPaymentAttemptBoletoReturn), DPaymentAttemptBoletoReturn.Fields.nossoNumero, Join.Left)]
        [Where(0, "storeId", typeof(DOrder), "storeId", Filter.Equal, Link.And)]
        [Where(1, Block.Begin, Link.And)]
        [Where(2, Block.Begin, Link.And)]
        [Where(3, "startTimeFrom", "startTime", Filter.GreaterOrEqual, Link.And)]
        [Where(4, "startTimeTo", "startTime", Filter.LessOrEqual, Link.And)]
        [Where(5, "boletoPaymentForms", "paymentFormId", Filter.NotEqual, Link.And)]
        [Where(6, Block.End, Link.Or)]
        [Where(7, Block.Begin, Link.Or)]
        [Where(8, "startTimeFrom", typeof(DPaymentAttemptBoletoReturn), DPaymentAttemptBoletoReturn.Fields.dataLiquidacao, Filter.GreaterOrEqual, Link.And)]
        [Where(9, "startTimeTo", typeof(DPaymentAttemptBoletoReturn), DPaymentAttemptBoletoReturn.Fields.dataLiquidacao, Filter.LessOrEqual, Link.And)]
        [Where(10, Block.Begin, Link.And)]
        [Where(11, "boletoPaymentForms", "paymentFormId", Filter.Equal, Link.Or)]
        [Where(12, Block.End, Link.And)]
        [Where(13, Block.End, Link.And)]
        [Where(14, Block.End, Link.And)]
        [Where(15, "status", "status", Filter.Equal, Link.And)]
        [Where(16, Block.Begin, Link.And)]
        [Where(17, "paymentFormId", "paymentFormId", Filter.Equal, Link.Or)]
        [Where(18, Block.End, Link.And)]
        [OrderBy(0, typeof(DOrder), "storeReferenceOrder", SortOrder.ASC)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptComplete[] ListFinancial(int storeId, DateTime startTimeFrom, DateTime startTimeTo, int status, int[] paymentFormId, int[] boletoPaymentForms);

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttempt[] List();

        [MethodType(MethodTypes.Query)]
        DPaymentAttempt[] List(long orderId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttempt[] List(long orderId, int status);

        [Where(0, "orderId", DPaymentAttempt.Fields.orderId, Filter.Equal, Link.And)]
        [Where(1, "dateFrom", DPaymentAttempt.Fields.startTime, Filter.GreaterOrEqual, Link.And)]
        [Where(2, "dateTo", DPaymentAttempt.Fields.startTime, Filter.LessOrEqual, Link.And)]
        [OrderBy(0, DPaymentAttempt.Fields.startTime, SortOrder.DESC)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttempt[] List(long orderId, DateTime dateFrom, DateTime dateTo);
        
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptComplete[] ListComplete(long orderId);

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DPaymentAttempt.Fields.lastUpdate, SortOrder.DESC)]
        DPaymentAttempt[] ListSortedByDate(long orderId);

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DPaymentAttempt.Fields.lastUpdate, SortOrder.DESC)]
        DPaymentAttempt[] ListSortedByDate(long orderId, int[] paymentFormId);

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DPaymentAttempt.Fields.lastUpdate, SortOrder.ASC)]
        DPaymentAttempt[] ListSortedByDateAsc(long orderId);
        
        [MethodType(MethodTypes.Query)]
        DPaymentAttempt Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DPaymentAttempt.Fields.lastUpdate, SortOrder.DESC)]
        DPaymentAttempt Locate(long orderId, int installmentNumber);

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DPaymentAttempt.Fields.lastUpdate, SortOrder.DESC)]
        DPaymentAttempt Locate(long orderId, int installmentNumber, int[] paymentFormId);
        
        [MethodType(MethodTypes.Insert)]
		void Insert(DPaymentAttempt dPaymentAttempt);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAttempt dPaymentAttempt);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);

        [Where(0, "paymentAttemptId", "paymentAttemptId", Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptComplete[] List(Guid paymentAttemptId);

        [Where(0, "storeId", typeof(DOrder), "storeId", Filter.Equal, Link.And)]
        [Where(1, "storeReferenceOrder", typeof(DOrder), "storeReferenceOrder", Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptComplete[] List(int storeId, string storeReferenceOrder);

        [DataRelation(typeof(DOrder),"consumerId",typeof(DConsumer),"consumerId",Join.Inner)]
        [Where(0, "storeId", typeof(DOrder), "storeId", Filter.Equal, Link.And)]
        [Where(1, "startTimeFrom", "startTime", Filter.GreaterOrEqual, Link.And)]
        [Where(2, "startTimeTo", "startTime", Filter.LessOrEqual, Link.And)]
        [Where(3, "paymentFormId", "paymentFormId", Filter.Equal, Link.And)]
        [Where(4, "status", "status", Filter.Equal, Link.And)]
        [Where(5, "consumerName", typeof(DConsumer), "name", Filter.LikeLeftRight, Link.And)]
        [Where(6, "CPF", typeof(DConsumer), "CPF", Filter.Equal, Link.And)]
        [Where(7, "CNPJ", typeof(DConsumer), "CNPJ", Filter.Equal, Link.And)]
        [Where(8, "orderStatus", typeof(DOrder), "status", Filter.Equal, Link.And)]
        [OrderBy(0, typeof(DOrder), "storeReferenceOrder", SortOrder.ASC)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptComplete[] List(int storeId, DateTime startTimeFrom, DateTime startTimeTo, int paymentFormId, int status, 
            string consumerName, string CPF, string CNPJ, int orderStatus);        

	}
}