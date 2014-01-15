using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAttemptBradesco))]
	public interface IPaymentAttemptBradesco
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptBradesco[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptBradesco Locate(Guid paymentAttemptId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAttemptBradesco dPaymentAttemptBradesco);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAttemptBradesco dPaymentAttemptBradesco);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);


	}
}