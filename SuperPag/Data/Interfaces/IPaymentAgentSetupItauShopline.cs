using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgentSetupItauShopline))]
	public interface IPaymentAgentSetupItauShopline
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupItauShopline[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupItauShopline Locate(int paymentAgentSetupId);

        [MethodType(MethodTypes.Query)]
        DPaymentAgentSetupItauShopline Locate(string businessKey);
        
        [MethodType(MethodTypes.Insert)]
		void Insert(DPaymentAgentSetupItauShopline dPaymentAgentSetupItauShopline);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAgentSetupItauShopline dPaymentAgentSetupItauShopline);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);


	}
}