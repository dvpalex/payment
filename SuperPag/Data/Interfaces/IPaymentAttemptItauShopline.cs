using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttemptItauShopline))]
	public interface IPaymentAttemptItauShopline
	{
        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptItauShopline[] List();

        [DataRelation(typeof(DPaymentAttemptItauShopline), DPaymentAttemptItauShopline.Fields.paymentAttemptId, typeof(DPaymentAttempt), DPaymentAttempt.Fields.paymentAttemptId, Join.Inner)]
        [Where(0, "qtdSonda", DPaymentAttemptItauShopline.Fields.qtdSonda, Filter.LessThan, Link.And)]
        [Where(1, Block.Begin, Link.And)]
        [Where(2, "status1", typeof(DPaymentAttempt), DPaymentAttempt.Fields.status, Filter.Equal, Link.Or)]
        [Where(3, "status2", typeof(DPaymentAttempt), DPaymentAttempt.Fields.status, Filter.Equal, Link.Or)]
        [Where(4, Block.End, Link.And)]
        [MethodType(MethodTypes.Query)]
        DPaymentAttemptItauShopline[] ListForLead(int status1, int status2, int qtdSonda);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptItauShopline Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptItauShopline Locate(int agentOrderReference);
        
        [MethodType(MethodTypes.Insert)]
		void Insert(DPaymentAttemptItauShopline dPaymentAttemptItauShopline);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAttemptItauShopline dPaymentAttemptItauShopline);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);
	}
}
