using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DOrderCreditCard))]
	public interface IOrderCreditCard
	{

        [ MethodType ( MethodTypes.Query ) ]
        DOrderCreditCard[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DOrderCreditCard Locate(long orderId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DOrderCreditCard dOrderCreditCard);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DOrderCreditCard dOrderCreditCard);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(long orderId);


	}
}