using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
    [DefaultDataMessage(typeof(DPaymentAgentSetupPaymentClientVirtual))]
    public interface IPaymentAgentSetupPaymentClientVirtual
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupPaymentClientVirtual[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupPaymentClientVirtual Locate(int paymentAgentSetupId);

        [ MethodType ( MethodTypes.Insert ) ]
        void Insert(DPaymentAgentSetupPaymentClientVirtual dPaymentAgentSetupPaymentClientVirtual);

		[ MethodType ( MethodTypes.Update ) ]
        void Update(DPaymentAgentSetupPaymentClientVirtual dPaymentAgentSetupPaymentClientVirtual);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);


	}
}