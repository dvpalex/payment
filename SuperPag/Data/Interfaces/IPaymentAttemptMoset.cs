using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttemptMoset))]
    public interface IPaymentAttemptMoset
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptMoset[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptMoset Locate(Guid paymentAttemptId);
       
        [MethodType(MethodTypes.Insert)]
		void Insert(DPaymentAttemptMoset dPaymentAttemptMoset);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAttemptMoset dPaymentAttemptMoset);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);


	}
}