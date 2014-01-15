using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework
{
	public class MessageRelation
	{
		public MessageRelation ( string parentField, string childField )
		{
		}

		public MessageRelation ( string childField )
		{
		}

	}


	[ Serializable() ]
	public abstract class Message : IMessageMapable
	{
		public string ToXml()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat ( "<{0}>", this.GetType().Name );
			AppendProperties ( this , sb );
			sb.AppendFormat ( "</{0}>", this.GetType().Name );
			
			return sb.ToString();
		}

		private void AppendProperties( object obj , StringBuilder sb )
		{
			PropertyInfo[] properties = obj.GetType().GetProperties ();

			foreach ( PropertyInfo p in properties )
			{
				if ( ! p.PropertyType.IsValueType && p.PropertyType != typeof ( String ) )
				{
					sb.AppendFormat ( "<{0}>", p.Name );
					object propertyValue = p.GetValue ( obj, null );
					if ( propertyValue != null )
					{
						AppendProperties ( propertyValue, sb );
					} 
					sb.AppendFormat ( "</{0}>", p.Name );
				}
				else
					AppendProperty ( obj, p , sb );
			}
		}

		private void AppendProperty ( object obj, PropertyInfo p , StringBuilder sb )
		{
			sb.AppendFormat ( "<{0}>{1}</{0}>", p.Name, p.GetValue ( obj , null ));
		}
	}

	[ Serializable() ]
	public abstract class MessageCollection : ICollection, IMessageCollectionMapable
	{	
		
		SerializableArrayList  _items = new SerializableArrayList();
		Message _parent;

		public MessageCollection ( Message parent , ICollection c , params MessageRelation[] relations )
		{
			EnsureIsCollectionOf();
			_items = new SerializableArrayList ( 5 );
			_parent = parent; 
		}

		public MessageCollection ( Message parent , params MessageRelation[] relations )
		{
			EnsureIsCollectionOf();
			_items = new SerializableArrayList( 5 );
			_parent = parent; 
		}

		public MessageCollection ( )
		{
			EnsureIsCollectionOf();
		}

		private void EnsureIsCollectionOf()
		{
			if ( ! Attribute.IsDefined ( this.GetType(), typeof ( CollectionOfAttribute ) ) )
			{
				throw new ArgumentException ( string.Format ( "Atributo 'CollectionOf' não definido para a classe {0}", this.GetType().Name ) );
			}
		}

		public System.Array ToArray( Type type )
		{
			return _items.ToArray( type );
		}

		public Message[] ToArray()
		{
			return (Message[])_items.ToArray( typeof ( Message ) );		
		}

        public void sort(string propriedade, bool isCrescente)
        {
            this._items.Sort(new GenericSort(propriedade, isCrescente));
        }

		public Message FindFirstByValue( string fieldName, string value )
		{
			if ( _items == null  || _items.Count == 0 ) return null;

			PropertyInfo p = _items [ 0 ].GetType().GetProperty ( fieldName );

			foreach ( Message message in _items )
			{
				string o = p.GetValue ( message, null ).ToString();
				if ( o == value )
				{
					return message;
				}
			}

			return null;
		}

		public Type GetElementType()
		{
			if ( _items.Count > 0 )
			{
				return _items [ 0 ].GetType();
			} else return null;
		}

		public Message this [ int index ] 
		{
			get
			{
				return (Message) _items [ index ];
			}
			set
			{
				_items [ index ] = value;
			}
		}

		public void Remove ( Message value )
		{
			_items.Remove ( value );
		}
		public bool Contains ( Message value )
		{
			return _items.Contains ( value );
		}

		public int Add ( Message value )
		{			
			return _items.Add ( value );
		}

		public void Clear()
		{
			_items.Clear();
		}

		public System.Array GetArray ( string field )
		{
			if ( this._items != null && this._items.Count > 0 )
			{
				PropertyInfo p = _items[ 0 ].GetType().GetProperty ( field );

				System.Array array = Array.CreateInstance ( p.PropertyType ,this._items.Count );
				int i = 0;
				foreach ( Message m in this._items )
				{
					array.SetValue (  p.GetValue ( m, null ) , i ) ;
					i++;
				}
				return array;
			}
			else
			{
				return new object[ 0 ];
			}
		}



		public decimal SumDecimal( string field )
		{
			return Messages.SumDecimal ( this , field , "" );
		}

        public int SumInt(string field)
        {
            return Messages.SumInt(this, field, "");
        }

		public decimal Sum( string field )
		{
			return Messages.SumDecimal ( this , field , "" );
		}

		public decimal Sum( string field , string fieldToGroup )
		{
			return Messages.SumDecimal ( this , field , fieldToGroup );
		}

		#region ICollection Members
		public bool IsSynchronized { get { return false; } }

		public object SyncRoot { get { return _items.SyncRoot; } }

		public int Count { get { return _items.Count ; } }

		public void CopyTo(Array array, int index) { _items.CopyTo ( array, index ); }

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator() { return _items.GetEnumerator(); }

		#endregion		
	}

	public class Messages
	{
		public static decimal SumDecimal ( MessageCollection messages, string field , string group )
		{
			return SumDecimal ( messages.ToArray(), field, group, messages.GetElementType() );
		}

        public static int SumInt(MessageCollection messages, string field, string group)
        {
            return SumInt(messages.ToArray(), field, group, messages.GetElementType());
        }

		public static decimal SumDecimal ( Message[] messages, string field , string group  )
		{
			return SumDecimal ( messages, field, group, messages.GetType().GetElementType() );
		}

		private static decimal SumDecimal ( Message[] messages, string field , string group , Type messageType )
		{
			if ( messageType == null ) return 0;
			if ( messages == null || messages.Length == 0 ) return 0;

			PropertyInfo p = messageType.GetProperty ( field );

			if ( p.PropertyType != typeof ( decimal ) )
				throw new ArgumentException ( "Campo não é decimal", "field" );

			if ( p == null ) throw new ArgumentException ( "Campo inexistente", "field" );

			decimal sum = 0;

			foreach ( Message m in messages )
			{
				sum = sum + (decimal)p.GetValue ( m , null );
			}

			return sum;
		}

        private static int SumInt(Message[] messages, string field, string group, Type messageType)
        {
            if (messageType == null) return 0;
            if (messages == null || messages.Length == 0) return 0;

            PropertyInfo p = messageType.GetProperty(field);

            if (p.PropertyType != typeof(int))
                throw new ArgumentException("Campo não é inteiro", "field");

            if (p == null) throw new ArgumentException("Campo inexistente", "field");

            int sum = 0;

            foreach (Message m in messages)
            {
                sum = sum + (int)p.GetValue(m, null);
            }

            return sum;
        }

	}

	[AttributeUsage ( AttributeTargets.Class ) ]
	public class CollectionOfAttribute : Attribute 
	{
		private Type _collectionType;

		public Type CollectionType
		{
			get
			{
				return _collectionType;
			}
		}

		public CollectionOfAttribute ( Type collectionType )
		{
			this._collectionType = collectionType;
		}
	}

	[Serializable]
	public class SerializableArrayList : System.Collections.ArrayList, ISerializable, IDeserializationCallback 
	{
		public SerializableArrayList() : base(){}
		public SerializableArrayList( int capacity ) : base( capacity ){}
		
		SerializationInfo serializationInfo;

		public void GetObjectData( SerializationInfo info, StreamingContext context ) 
		{			
			object[] values = new object[ this.Count ];
			this.CopyTo( values, 0 );

			info.AddValue( "Values", values );			
		}

		protected SerializableArrayList( SerializationInfo info, StreamingContext context ) 
		{		
			serializationInfo = info;
		}

		public void OnDeserialization( object sender ) 
		{
			object[] values = (object[])serializationInfo.GetValue( "Values", typeof( object[] ) );

			for( int idx = 0; idx < values.Length; ++idx ) 
			{
				this.Add( values[ idx ] );
			}

			serializationInfo = null;
		}
	}
}
