using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgentSetupMoset))]
	public interface IPaymentAgentSetupMoset
	{
        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupMoset[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupMoset Locate(int paymentAgentSetupId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAgentSetupMoset dPaymentAgentSetupMoset);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAgentSetupMoset dPaymentAgentSetupMoset);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);
	}
}