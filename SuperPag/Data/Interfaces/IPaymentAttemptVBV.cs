using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttemptVBV))]
	public interface IPaymentAttemptVBV
	{
        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptVBV[] List();

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptVBV[] List(byte vbvStatus);

        [DataRelation(typeof(DPaymentAttemptVBV), DPaymentAttemptVBV.Fields.paymentAttemptId, typeof(DPaymentAttempt), DPaymentAttempt.Fields.paymentAttemptId, Join.Inner)]
        [Where(0, "qtdSonda", DPaymentAttemptVBV.Fields.qtdSonda, Filter.LessThan, Link.And)]
        [Where(1, Block.Begin, Link.And)]
        [Where(2, "status", typeof(DPaymentAttempt), DPaymentAttempt.Fields.status, Filter.Equal, Link.Or)]
        [Where(3, Block.End, Link.And)]
        [Where(4, "untilTime", typeof(DPaymentAttempt), DPaymentAttempt.Fields.startTime, Filter.LessThan, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptVBV[] ListForLead(int[] status, int qtdSonda, DateTime untilTime);

        [DataRelation(typeof(DPaymentAttemptVBV), DPaymentAttemptVBV.Fields.paymentAttemptId, typeof(DPaymentAttempt), DPaymentAttempt.Fields.paymentAttemptId, Join.Inner)]
        [DataRelation(typeof(DPaymentAttempt), DPaymentAttempt.Fields.orderId, typeof(DOrder), DOrder.Fields.orderId, Join.Inner)]
        [Where(0, "vbvStatus", typeof(DPaymentAttemptVBV), DPaymentAttemptVBV.Fields.vbvStatus, Filter.Equal, Link.And)]
        [Where(1, Block.Begin, Link.And)]
        [Where(2, "storeIds", typeof(DOrder), DOrder.Fields.storeId, Filter.Equal, Link.Or)]
        [Where(3, Block.End, Link.And)]
        [Where(4, "sinceDate", typeof(DPaymentAttempt), DPaymentAttempt.Fields.startTime, Filter.GreaterThan, Link.And)]
        [Where(5, "attemptStatus", typeof(DPaymentAttempt), DPaymentAttempt.Fields.status, Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptVBV[] ListToCapture(int vbvStatus, int[] storeIds, DateTime sinceDate, int attemptStatus);

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptVBV Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptVBV Locate(string tid);
        
        [MethodType(MethodTypes.Insert)]
		void Insert(DPaymentAttemptVBV dPaymentAttemptVBV);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAttemptVBV dPaymentAttemptVBV);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);
	}
}