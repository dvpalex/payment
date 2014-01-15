using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DHandshakeConfigurationHtml))]
	public interface IHandshakeConfigurationHtml
	{

        [ MethodType ( MethodTypes.Query ) ]
        DHandshakeConfigurationHtml[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DHandshakeConfigurationHtml Locate(int handshakeConfigurationId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DHandshakeConfigurationHtml dHandshakeConfigurationHtml);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DHandshakeConfigurationHtml dHandshakeConfigurationHtml);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int handshakeConfigurationId);


	}
}