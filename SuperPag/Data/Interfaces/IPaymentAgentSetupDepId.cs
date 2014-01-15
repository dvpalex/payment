using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgentSetupDepId))]
	public interface IPaymentAgentSetupDepId
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupDepId[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupDepId Locate(int paymentAgentSetupId);

        [ MethodType ( MethodTypes.Insert ) ]
        void Insert(DPaymentAgentSetupDepId dPaymentAgentSetupDepId);

		[ MethodType ( MethodTypes.Update ) ]
        void Update(DPaymentAgentSetupDepId dPaymentAgentSetupDepId);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);


	}
}