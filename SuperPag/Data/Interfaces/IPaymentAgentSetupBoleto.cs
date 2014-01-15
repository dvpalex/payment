using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgentSetupBoleto))]
	public interface IPaymentAgentSetupBoleto
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupBoleto[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupBoleto Locate(int paymentAgentSetupId);

        [ MethodType ( MethodTypes.Insert ) ]
        void Insert(DPaymentAgentSetupBoleto dPaymentAgentSetupBoletoBB);

		[ MethodType ( MethodTypes.Update ) ]
        void Update(DPaymentAgentSetupBoleto dPaymentAgentSetupBoletoBB);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);


	}
}