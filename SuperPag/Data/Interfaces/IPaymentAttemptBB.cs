using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttemptBB))]
	public interface IPaymentAttemptBB
	{
        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptBB[] List();

        [DataRelation(typeof(DPaymentAttemptBB), DPaymentAttemptBB.Fields.paymentAttemptId, typeof(DPaymentAttempt), DPaymentAttempt.Fields.paymentAttemptId, Join.Inner)]
        [Where(0, "qtdSonda", DPaymentAttemptBB.Fields.qtdSonda, Filter.LessThan, Link.And)]
        [Where(1, Block.Begin, Link.And)]
        [Where(2, "status1", typeof(DPaymentAttempt), DPaymentAttempt.Fields.status, Filter.Equal, Link.Or)]
        [Where(3, "status2", typeof(DPaymentAttempt), DPaymentAttempt.Fields.status, Filter.Equal, Link.Or)]
        [Where(4, Block.End, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptBB[] ListForLead(int status1, int status2, int qtdSonda);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptBB Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptBB Locate(int agentOrderReference);
        
        [MethodType(MethodTypes.Insert)]
		void Insert(DPaymentAttemptBB dPaymentAttemptBB);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAttemptBB dPaymentAttemptBB);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);
	}
}
