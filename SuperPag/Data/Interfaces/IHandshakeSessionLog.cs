using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DHandshakeSessionLog))]
	public interface IHandshakeSessionLog
	{
        [ MethodType ( MethodTypes.Query ) ]
        DHandshakeSessionLog[] List();

        [MethodType(MethodTypes.Query)]
        DHandshakeSessionLog[] List(Guid handshakeSessionId);

        [ MethodType ( MethodTypes.Insert ) ]
        void Insert(DHandshakeSessionLog dHandshakeSessionLog);
	}
}