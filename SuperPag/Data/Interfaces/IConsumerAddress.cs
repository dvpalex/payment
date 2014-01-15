using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DConsumerAddress))]
	public interface IConsumerAddress
	{

        [ MethodType ( MethodTypes.Query ) ]
        DConsumerAddress[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DConsumerAddress Locate(long consumerAddressId);

        [MethodType(MethodTypes.Query)]
        DConsumerAddress Locate(long consumerId, int addressType);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DConsumerAddress dConsumerAddress);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DConsumerAddress dConsumerAddress);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(long consumerAddressId);


	}
}