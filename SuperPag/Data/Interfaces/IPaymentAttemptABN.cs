using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttemptABN))]
	public interface IPaymentAttemptABN
	{
        [DataRelation(typeof(DPaymentAttempt), DPaymentAttempt.Fields.paymentAttemptId, typeof(DPaymentAttemptABN), DPaymentAttemptABN.Fields.paymentAttemptId, Join.Inner)]
        [Where(0, "statusProposta", typeof(DPaymentAttemptABN), DPaymentAttemptABN.Fields.statusProposta, Filter.Equal, Link.And)]
        [Where(1, "sinceDate", typeof(DPaymentAttempt), DPaymentAttempt.Fields.startTime, Filter.Equal, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptABN[] ListSonda(string statusProposta, DateTime sinceDate);

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptABN[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptABN Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptABN Locate(int agentOrderReference);
        
        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAttemptABN dPaymentAttemptBB);

		[ MethodType ( MethodTypes.Update ) ]
        void Update(DPaymentAttemptABN dPaymentAttemptBB);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);


	}
}