using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DOrderItem))]
	public interface IOrderItem
	{
        [ MethodType ( MethodTypes.Query ) ]
        DOrderItem[] List();

        [MethodType(MethodTypes.Query)]
        DOrderItem[] List(long orderId, int itemType);

        [ MethodType(MethodTypes.Query) ]
        DOrderItem[] List(long orderId, int[] itemType);

        [ MethodType ( MethodTypes.Query ) ]
        DOrderItem Locate(long orderItemId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DOrderItem dOrderItem);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DOrderItem dOrderItem);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(long orderItemId);

        [MethodType(MethodTypes.Query)]
        DOrderItem[] List(long orderId);
	}
}