using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttemptDepId))]
	public interface IPaymentAttemptDepId
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptDepId[] List();

        [MethodType(MethodTypes.Query)]
        [DataRelation(typeof(DPaymentAttemptDepId), DPaymentAttemptDepId.Fields.paymentAttemptId, typeof(DPaymentAttempt), DPaymentAttempt.Fields.paymentAttemptId, Join.Inner)]
        [DataRelation(typeof(DPaymentAttempt), DPaymentAttempt.Fields.orderId, typeof(DOrder), DOrder.Fields.orderId, Join.Inner)]
        [Where(0, "idNumber", typeof(DPaymentAttemptDepId), DPaymentAttemptDepId.Fields.idNumber, Filter.Equal, Link.And)]
        [Where(1, "storeId", typeof(DOrder), DOrder.Fields.storeId, Filter.Equal, Link.And)]
        [OrderBy(0, typeof(DPaymentAttempt), DPaymentAttempt.Fields.startTime, SortOrder.DESC)]
        DPaymentAttemptDepId LocateLast(string idNumber, int storeId);

        [MethodType(MethodTypes.Query)]
        [OrderBy(0, DPaymentAttemptDepId.Fields.agentOrderReference, SortOrder.DESC)]
        DPaymentAttemptDepId LocateLast(string idNumber);

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptDepId Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptDepId Locate(int agentOrderReference);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAttemptDepId dPaymentAttemptDepId);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAttemptDepId dPaymentAttemptDepId);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);


	}
}