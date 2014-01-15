using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DHandshakeConfigurationXml))]
	public interface IHandshakeConfigurationXml
	{

        [ MethodType ( MethodTypes.Query ) ]
        DHandshakeConfigurationXml[] List();

        [ MethodType ( MethodTypes.Query ) ]
        DHandshakeConfigurationXml Locate(int handshakeConfigurationId);

        [ MethodType ( MethodTypes.Insert ) ]
		void Insert(DHandshakeConfigurationXml dHandshakeConfigurationXml);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DHandshakeConfigurationXml dHandshakeConfigurationXml);

		[ MethodType ( MethodTypes.Delete ) ]
		void Delete(int handshakeConfigurationId);


	}
}