using System;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Mapping;
using SuperPag.Data.Messages;

namespace SuperPag.Data.Interfaces
{
	[DefaultDataMessage(typeof(DHandshakeConfiguration))]
	public interface IHandshakeConfiguration
	{

        [ MethodType ( MethodTypes.Query ) ]
        DHandshakeConfiguration[] List();

        [MethodType(MethodTypes.Query)]
        DHandshakeConfiguration[] List(int storeId);

        [Aggregation(DHandshakeConfiguration.Fields.handshakeConfigurationId, AggregationType.Max)]
        [MethodType(MethodTypes.Query)]
        DHandshakeConfigurationMaxId MaxId();

        [MethodType(MethodTypes.Query)]
        DHandshakeConfiguration Locate(int handshakeConfigurationId);

        [MethodType(MethodTypes.Query)]
        DHandshakeConfiguration Locate(int storeId, int handshakeType);
        
        [MethodType(MethodTypes.Insert)]
		void Insert(DHandshakeConfiguration dHandshakeConfiguration);

		[ MethodType ( MethodTypes.Update ) ]
		void Update(DHandshakeConfiguration dHandshakeConfiguration);

		[ MethodType ( MethodTypes.Delete ) ]
        void Delete(int handshakeConfigurationId);
	}
}
