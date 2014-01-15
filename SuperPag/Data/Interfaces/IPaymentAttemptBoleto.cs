using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttemptBoleto))]
	public interface IPaymentAttemptBoleto
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptBoleto[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptBoleto Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptBoleto Locate(int agentOrderReference);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAttemptBoleto dPaymentAttemptBoleto);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAttemptBoleto dPaymentAttemptBoleto);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);


	}
}