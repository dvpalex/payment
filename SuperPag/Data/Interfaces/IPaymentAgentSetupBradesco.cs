using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgentSetupBradesco))]
	public interface IPaymentAgentSetupBradesco
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupBradesco[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupBradesco Locate(int paymentAgentSetupId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAgentSetupBradesco dPaymentAgentSetupBradesco);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAgentSetupBradesco dPaymentAgentSetupBradesco);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);


	}
}