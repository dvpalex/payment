using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace SuperPag.Framework.Caching
{
	public sealed class CachedValue
	{
		private CachedValue(){}

		public static void Wrap( CacheProxy proxy, string cache_key, string key, object value )
		{
			Wrap( proxy, cache_key, key, value, 0 );
		}
		
		public static void Wrap( CacheProxy proxy, string cache_key, string key, object value, uint minutes_to_expire )
		{			
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			
			bin.Serialize( ms, value );
			
			proxy.AddItem( cache_key, key, ms.ToArray(), minutes_to_expire );
		}

		public static object Unwrap( CacheProxy proxy, string cache_key, string key )
		{
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

			//Adiciona um handle no evento de falha no load do assembly, pois aqui podemos ter esse tipo de problema
			//na desserialização do EFinancialFramework
			//TODO: ver como implementar melhor.
			ResolveEventHandler resolveDelegate = new ResolveEventHandler(CurrentDomain_AssemblyResolve);

			AppDomain.CurrentDomain.AssemblyResolve += resolveDelegate;

			object result = bin.Deserialize( new System.IO.MemoryStream( (byte[])proxy.GetItem( cache_key, key ) ) );
			
			//TODO: VER SE ESTÁ CORRETO
			AppDomain.CurrentDomain.AssemblyResolve -= resolveDelegate;

			return result;
		}

		private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
			//TODO: checar ECoop
			if (args.Name.ToUpper().StartsWith("EFINANCIALFRAMEWORK")) {
				return typeof(CacheConfig).Assembly;
			}

			return null;
		}
	}
	
	public sealed class CacheConfig
	{
		private CacheConfig(){}

		static CacheConfig()
		{
			//Se no futuro se usarmos outros componentes remotos
			//O código do ChannelServices deve sair daqui, 
			//somente permanecerá o RemotingConfiguration
			if( ChannelServices.RegisteredChannels.Length < 1 )
			{				
				ChannelServices.RegisterChannel(new TcpChannel(0));
  
//				RemotingConfiguration.RegisterWellKnownClientType( typeof(SuperPag.Framework.Caching.CacheProxy), string.Format( "tcp://{0}:2051/CacheURI", SuperPag.Framework.Configuration.ConfigurationReader.GetRemoteCache() ) 	);				
			}
		}
		
		public static CacheProxy GetProxy()
		{			
			return new CacheProxy();			
		}
	}
}