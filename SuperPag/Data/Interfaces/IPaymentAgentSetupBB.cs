using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgentSetupBB))]
	public interface IPaymentAgentSetupBB
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupBB[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupBB Locate(int paymentAgentSetupId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAgentSetupBB dPaymentAgentSetupBB);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAgentSetupBB dPaymentAgentSetupBB);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);


	}
}