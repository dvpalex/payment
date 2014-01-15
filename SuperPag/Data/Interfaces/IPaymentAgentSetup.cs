using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgentSetup))]
	public interface IPaymentAgentSetup
	{
        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetupComplete[] List();

        [MethodType(MethodTypes.Query)]
        DPaymentAgentSetupComplete[] List(int paymentAgentId);

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgentSetup Locate(int paymentAgentSetupId);

        [Aggregation(DPaymentAgentSetup.Fields.paymentAgentSetupId, AggregationType.Max)]
        [MethodType(MethodTypes.Query)]
        DPaymentAgentSetupMaxId MaxId();

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAgentSetup dPaymentAgentSetup);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAgentSetup dPaymentAgentSetup);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentSetupId);


	}
}