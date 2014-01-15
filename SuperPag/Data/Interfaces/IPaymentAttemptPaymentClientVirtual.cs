using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAttemptPaymentClientVirtual))]
    public interface IPaymentAttemptPaymentClientVirtual
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptPaymentClientVirtual[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAttemptPaymentClientVirtual Locate(Guid paymentAttemptId);

        [MethodType(MethodTypes.Query)]
        DPaymentAttemptPaymentClientVirtual Locate(int agentOrderReference);
        
        [MethodType(MethodTypes.Insert)]
        void Insert(DPaymentAttemptPaymentClientVirtual dPaymentAttemptPaymentClientVirtual);

		[ MethodType ( MethodTypes.Update ) ]
        void Update(DPaymentAttemptPaymentClientVirtual dPaymentAttemptPaymentClientVirtual);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(Guid paymentAttemptId);


	}
}