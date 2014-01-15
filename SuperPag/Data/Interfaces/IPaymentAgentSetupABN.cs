using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgentSetupABN))]
	public interface IPaymentAgentSetupABN
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupABN[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupABN Locate(int paymentAgentSetupId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAgentSetupABN dPaymentAgentSetupABN);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAgentSetupABN dPaymentAgentSetupABN);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);

	}
}