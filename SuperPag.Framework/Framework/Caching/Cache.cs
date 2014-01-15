using System;
using System.Collections;
using System.EnterpriseServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace SuperPag.Framework.Caching
{
	public interface ICache
	{
		Hashtable Alloc( string cache_key );		
		Hashtable GetAllocated( string cache_key );		
		string ServerInfo{ get; }
		void Dealloc( string cache_key );
				
		void AddItem( string cache_key, object key, object value );
		void AddItem( string cache_key, object key, object value, uint minutes_to_expires );
		bool Exists( string cache_key );		
		bool Exists( string cache_key, object key );		
		object GetItem( string cache_key, object key );
		void RemoveItem( string cache_key, object key );		
	}
	class DataSlot
	{
		public DataSlot( object value, bool mustExpires, DateTime expires )
		{
			Value = value;
			MustExpires = mustExpires;
			Expires = expires;
		}

		public DataSlot( object value ) : this( value, false, DateTime.MinValue ){}
		
		public object Value;
		public bool MustExpires;		
		public DateTime Expires;
	}

	public sealed class Cache : ICache, IDisposable
	{
		static Hashtable hash_cache = new Hashtable( 10 );
		//TODO: porta
		static int remoting_port = 2051;
		static string server_info = String.Format( "Cache started at {0} in {1} machine. Using TCP port {2} to remote communications", DateTime.Now, System.Environment.MachineName, remoting_port );
				
		public Hashtable Alloc( string cache_key )
		{
			if( ! Exists( cache_key ) )
			{
				Hashtable hash = new Hashtable( 50 );
				hash_cache.Add( cache_key, hash );
				return hash;
			}

			return null;
		}

		public Hashtable GetAllocated( string cache_key )
		{
			if( hash_cache.ContainsKey( cache_key ) )
				return (Hashtable)hash_cache[ cache_key ];
			return null;
		}

		public string ServerInfo
		{
			get
			{
				return server_info;
			}
		}

		public void Dealloc( string cache_key )
		{
			if( hash_cache.ContainsKey( cache_key ) )
			{
				hash_cache.Remove( cache_key );
			}
		}
		
		private void _AddItem( string cache_key, object key, DataSlot slot )
		{
			Hashtable hash = GetAllocated( cache_key );
			if( null == hash )
			{
				hash = Alloc( cache_key );	
			}
			
			if( hash.ContainsKey( key ) )
			{
				hash[ key ] = slot;
			}
			else
			{
				hash.Add( key, slot );
			}
		}
		
		public void AddItem( string cache_key, object key, object value )
		{			
			AddItem( cache_key, key, value, 0 );
		}

		public void AddItem( string cache_key, object key, object value, uint minutes_to_expires )
		{			
			if( minutes_to_expires == 0 )
			{
				_AddItem( cache_key, key, new DataSlot( value ) );
				return;
			}

			_AddItem( cache_key, key, new DataSlot( value, true, DateTime.Now.AddMinutes( Convert.ToDouble( minutes_to_expires  ) ) ) );
		}

		public bool Exists( string cache_key )
		{
			return hash_cache.ContainsKey( cache_key );
		}

		private DataSlot GetDataSlot( string cache_key, object key )
		{
			Hashtable hash = GetAllocated( cache_key );
			if( null != hash )
			{
				if( hash.ContainsKey( key ) )
				{
					DataSlot slot = (DataSlot)hash[key];
					if( !slot.MustExpires || ( slot.MustExpires && DateTime.Now < slot.Expires ) )
					{
						return slot;
					}
					else
					{
						hash.Remove( key );
					}
				}
			}
			
			return null;
		}

		public bool Exists( string cache_key, object key )
		{
			return null != GetDataSlot( cache_key, key );
		}
		
		public object GetItem( string cache_key, object key )
		{			
			DataSlot slot = GetDataSlot( cache_key, key );
			if( null != slot )
				return slot.Value;
			return null;
		}

		public void RemoveItem( string cache_key, object key )
		{
			Hashtable hash = GetAllocated( cache_key );
			if( null != hash && hash.ContainsKey( key ) )
			{
				hash.Remove( key );
			}				
		}

		public void Dispose() {}
		
	}
	
	public sealed class CacheProxy : MarshalByRefObject, ICache
	{
		private SuperPag.Framework.Caching.Cache cache = null;
		
		public CacheProxy()
		{}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		#region ICache Members

		public System.Collections.Hashtable Alloc(string cache_key)
		{
			using( cache = new SuperPag.Framework.Caching.Cache() )
			{
				return cache.Alloc( cache_key );	
			}
		}

		public System.Collections.Hashtable GetAllocated(string cache_key)
		{
			using( cache = new SuperPag.Framework.Caching.Cache() )
			{
				return cache.GetAllocated( cache_key );	
			}
		}

		public string ServerInfo
		{
			get
			{
				using( cache = new SuperPag.Framework.Caching.Cache() )
				{
					return cache.ServerInfo;
				}
			}
		}

		public void Dealloc(string cache_key)
		{
			using( cache = new SuperPag.Framework.Caching.Cache() )
			{
				cache.Dealloc( cache_key );	
			}
		}

		public void AddItem(string cache_key, object key, object value)
		{
			using( cache = new SuperPag.Framework.Caching.Cache() )
			{
				cache.AddItem( cache_key, key, value );	
			}
		}

		public void AddItem(string cache_key, object key, object value, uint minutes_to_expires)
		{
			using( cache = new SuperPag.Framework.Caching.Cache() )
			{
				cache.AddItem( cache_key, key, value, minutes_to_expires );	
			}
		}

		public bool Exists(string cache_key)
		{
			using( cache = new SuperPag.Framework.Caching.Cache() )
			{
				return cache.Exists( cache_key );	
			}
		}

		public bool Exists(string cache_key, object key)
		{
			using( cache = new SuperPag.Framework.Caching.Cache() )
			{
				return cache.Exists( cache_key, key );	
			}
		}

		public object GetItem(string cache_key, object key)
		{
			using( cache = new SuperPag.Framework.Caching.Cache() )
			{
				return cache.GetItem( cache_key, key );	
			}
		}

		public void RemoveItem(string cache_key, object key)
		{
			using( cache = new SuperPag.Framework.Caching.Cache() )
			{
				cache.RemoveItem( cache_key, key );	
			}
		}

		#endregion
	}
}