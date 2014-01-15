using System;
using System.Reflection;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Data.Mapping
{
	public class DataMapper
	{
		private FieldInfo[] _rowsetFields;
		private string _connectionString;
		private FieldInfo[] _messageFields;
		private MethodAdapter[] _adapters;
		private string[] _columns;
		private ArrayList _rowsets;
		private Type _type;
#if SQL
        private SqlDataReader _reader;
#elif ORACLE
        private OracleDataReader _reader;
#else
        private SqlDataReader _reader;
#endif
        private Hashtable _otherParameters;

		public DataMapper (  object[] arrayOfMessages , Hashtable otherParameters ,  string connectionString )
		{
			if ( arrayOfMessages == null ) throw new ArgumentNullException( "arrayOfMessages" );

			this._otherParameters = otherParameters;
			this._type = arrayOfMessages.GetType().GetElementType();

			_rowsets = new ArrayList();

			this._messageFields = 
				this._type.GetFields ( BindingFlags.Instance | BindingFlags.Public );

			foreach ( object message in arrayOfMessages )
			{
				_rowsets.Add ( message );
			}

			this._connectionString = connectionString;

			
		}

#if SQL
        public DataMapper(Type type, SqlDataReader reader)
#elif ORACLE
        public DataMapper(Type type, OracleDataReader reader)
#else
        public DataMapper(Type type, SqlDataReader reader)
#endif
		{
			if ( reader == null ) throw new ArgumentNullException( "reader" );

			this._type = type;
			this._rowsets  = new ArrayList ( 10 );
			this._reader = reader;
			this._messageFields = 
				this._type.GetFields ( BindingFlags.Instance | BindingFlags.Public );
		}

		public System.Array Do ()
		{
			ParseChildren ();
			
			if ( _reader != null )
			{
				SetColumns ();

				while ( _reader.Read() )
				{
					Append ( );
				}
			}

			ProcessChildren();

			return _rowsets.ToArray ( this._type );
		}

		private void ProcessChildren()
		{
			if ( _adapters != null )
			{
				foreach ( MethodAdapter adapter in _adapters )
				{
					foreach ( object rowSet in _rowsets )
					{
						adapter.MessageField.SetValue ( rowSet, adapter.Invoke ( rowSet ) );
					}					
				}
			}
		}

		private void ParseChildren()
		{
			foreach ( FieldInfo f in _messageFields )
			{
				//TODO: if especificado 

				object attribute = Attributes.GetSingleAttribute ( f, typeof ( Bindable ) );

				if ( null != attribute ) //throw new Exception ( string.Format ( " Attribute FillFromObject not specified for field {0}", f.Name ) );
				{
					MethodAdapter method = GetChildMethod ( (Bindable)attribute );
					
					if ( method != null )
					{
						method.MessageField = f;

						ArrayList lst;
						if ( _adapters != null )
							lst = new ArrayList( _adapters );
						else 
							lst = new ArrayList( );

						lst.Add ( method );

						_adapters = (MethodAdapter[]) lst.ToArray ( typeof ( MethodAdapter ) );
					}
					
				}
			}
		}

		private MethodAdapter GetChildMethod( Bindable childObjectType )
		{
			MethodInfo[] methods;

			if ( childObjectType.MethodType.IsInterface )
			{
				object dataObject = SuperPag.Framework.Data.Components.AutoDataLayer.DataLayerCache.GetCachedDataLayer ( childObjectType.MethodType, _connectionString );

				methods = dataObject.GetType().GetMethods ( BindingFlags.Instance | BindingFlags.Public );
			}
			else
			{
				methods = childObjectType.MethodType.GetMethods ( BindingFlags.Static | BindingFlags.Public );
			}

			foreach ( MethodInfo method in methods )
			{
				object attribute = Attributes.GetSingleAttribute ( method, typeof ( BindingAllowed ) );
				
				if ( attribute != null )
				{
					ParameterInfo[] parameters =  method.GetParameters();

					if ( parameters == null || parameters.Length < 1 ) throw new Exception ( "At least one parameters are required when FillForObject attribute are used" );  

					MethodAdapter adapter = new MethodAdapter();
					adapter.Method = method;

					if ( childObjectType.MethodType.IsInterface )
					{
						adapter.ConnectionString = _connectionString;
					}

					foreach ( ParameterInfo parameter in parameters )
					{
						if ( null != _otherParameters )
						{							
							foreach ( string key in _otherParameters.Keys )
							{
								if ( key == parameter.Name )
								{
									adapter.AddParameter ( _otherParameters [ key ] );
									break;
								}
							}
						}

						foreach ( FieldInfo fld in _messageFields )
						{
							if ( fld.Name == parameter.Name )
							{
								adapter.AddParameter ( fld );
								break;
							} 
						}
					}

					return adapter;

				}
			}

			return null;
		}

		private void SetColumns( )
		{
			_columns = new string [ _reader.FieldCount ] ;

			DataTable schema =  _reader.GetSchemaTable();

			int i = 0;
			foreach ( DataRow dr in schema.Rows )
			{
				_columns [ i ] = dr [ "ColumnName" ].ToString();
				i++;
			}

			Array.Sort ( _columns );

			ArrayList tempArray = new ArrayList( _messageFields.Length );

			foreach ( FieldInfo fld in _messageFields )
			{
				if ( Array.BinarySearch ( _columns, fld.Name ) >= 0 )
				{
					tempArray.Add ( fld );
				}
			}

			_rowsetFields = (FieldInfo[])tempArray.ToArray ( typeof ( FieldInfo ) );
		}

		private void Append()
		{
			object dataMessage = Activator.CreateInstance ( _type );

			foreach ( FieldInfo fld in _rowsetFields )
			{
				fld.SetValue ( dataMessage , _reader [ fld.Name ] );
			}

			_rowsets.Add ( dataMessage );
		}
		
		private class MethodAdapter
		{
			private class InvokeParameters
			{
				public FieldInfo FielInfo;
				public object Value;
			}

			public MethodInfo Method;
			public string ConnectionString;
			public FieldInfo MessageField;
			private InvokeParameters[] _invokeParameters;

			public void AddParameter ( FieldInfo field )
			{ 
				ArrayList lst;
				if ( _invokeParameters == null )
					lst = new ArrayList( );
				else 
					lst = new ArrayList( _invokeParameters );

				InvokeParameters invokeParameters = new InvokeParameters();
				invokeParameters.FielInfo = field;

				lst.Add ( invokeParameters );

				_invokeParameters = (InvokeParameters[])lst.ToArray ( typeof ( InvokeParameters ) );
			}

			public void AddParameter ( object value )
			{ 
				ArrayList lst;
				if ( _invokeParameters == null )
					lst = new ArrayList( );
				else 
					lst = new ArrayList( _invokeParameters );

				InvokeParameters invokeParameters = new InvokeParameters();
				invokeParameters.Value = value;
				
				lst.Add ( invokeParameters );

				_invokeParameters = (InvokeParameters[])lst.ToArray ( typeof ( InvokeParameters ) );
			}

			public object Invoke( object dataMessage )
			{
				object dataObject;

				if ( ConnectionString != null )
					dataObject = Activator.CreateInstance ( Method.DeclaringType , new object[] { ConnectionString } );
				else
					dataObject = Activator.CreateInstance ( Method.DeclaringType );
				
				object[] arrParameters = new object [ _invokeParameters.Length ] ;

				int i = 0;
				foreach ( InvokeParameters p in _invokeParameters )
				{
					if ( p.FielInfo != null )
					{
						arrParameters [ i ] = p.FielInfo.GetValue ( dataMessage ); 
					} 
					else
					{
						arrParameters [ i ] = p.Value;
					}
					i ++;
				}

				try
				{
					object returnedObject = Method.Invoke ( dataObject, arrParameters );

					return Convert.ChangeType ( returnedObject, MessageField.FieldType );
				} 
				catch ( TargetInvocationException ex )
				{
					throw ex.InnerException;
				}
			}
		}
	
	}
}
