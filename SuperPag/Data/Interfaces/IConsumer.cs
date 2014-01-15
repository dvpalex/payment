using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DConsumer))]
	public interface IConsumer
	{

        [ MethodType ( MethodTypes.Query ) ]
        DConsumer[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DConsumer Locate(long consumerId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DConsumer dConsumer);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DConsumer dConsumer);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(long consumerId);


	}
}