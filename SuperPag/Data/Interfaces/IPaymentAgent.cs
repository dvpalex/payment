using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DPaymentAgent))]
	public interface IPaymentAgent
	{

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgent[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DPaymentAgent Locate(int paymentAgentId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DPaymentAgent dPaymentAgent);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DPaymentAgent dPaymentAgent);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int paymentAgentId);


	}
}