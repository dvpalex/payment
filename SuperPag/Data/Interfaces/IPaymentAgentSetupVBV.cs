using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgentSetupVBV))]
	public interface IPaymentAgentSetupVBV
	{
        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupVBV[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupVBV Locate(int paymentAgentSetupId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAgentSetupVBV dPaymentAgentSetupVBV);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAgentSetupVBV dPaymentAgentSetupVBV);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);
	}
}