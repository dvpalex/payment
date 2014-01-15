using System;
using System.Collections;
using System.Reflection;

namespace SuperPag.Framework.Data.Components.Data.Objects.Helpers {
	public class Map {
		public Map( string field, string member ) : this( field, member, null ){}
		public Map( string field ) : this (field, field) {}
		public Map( string field, IDataCustomMap map ) : this( field, field, map ){}

		public Map( string field, string member, IDataCustomMap map ) {
			this.field = field;
			this.member = member;
			this.map = map;
		}
		
		public string field;
		public string member;
		public IDataCustomMap map;

		public static Map[] Mount( string field, params string[] fields ) {
			ArrayList list = new ArrayList();

			list.Add( new Map( field ) );

			foreach( string f in fields )
				list.Add( new Map( f ) );

			return (Map[]) list.ToArray( typeof( Map ) );
		}
	}

	public interface IDataCustomMap {
		object Map( object value );		
	}

	public sealed class Mapper {
		private Mapper(){}

		private static object GetObjectFromRow( Hashtable row, string field_index ) {
			object row_value = row[ field_index ];
			
			if( null == row_value ) {
				string m_field = field_index.ToLower();

				foreach( string key in row.Keys ) {
					if( m_field == key.ToLower() ) {
						return row[ key ];						
					}					
				}

				throw new IndexOutOfRangeException( "field from map not exists" );
			}

			return row_value;
		}
		
		public static object GetObject( Type t, Map[] map, object result ) {
			if( null == result )
				throw new ArgumentNullException( "result", "Cannot be null" ); 

			if( null == map || map.Length < 1 )
				throw new ArgumentException( "Invalid size. Must be greater than 1", "map" ); 
			
			object instance = Activator.CreateInstance( t ); 
			
			Hashtable row = (Hashtable) result;

			if(  null == row || null == instance ) 
				return null; 

			foreach( Map m in map ) {
				FieldInfo fi = t.GetField( m.member );

				if( fi != null ) {
					object row_value = Mapper.GetValue( row,  m.field, fi.FieldType );
					
					fi.SetValue( instance, m.map != null ? m.map.Map( row_value ) : row_value );
										
					continue;
				}

				PropertyInfo pi = t.GetProperty( m.member );

				if( pi != null ) {
					object row_value = Mapper.GetValue( row,  m.field, pi.PropertyType );
						
					pi.SetValue( instance, m.map != null ? m.map.Map( row_value ) : row_value, null );
					
				}
			}			
		
			return instance;
		}
		
		private static object GetValue( Hashtable row, string field_index, Type t ) {
			object row_value = GetObjectFromRow( row,  field_index );

			if (!(row_value is DBNull)) {
				if( t == typeof(SuperPag.Framework.Data.Components.Tristate)) {
					SuperPag.Framework.Data.Components.Tristate tristateValue = (bool)row_value;
					return tristateValue ;
				}
				else {
					return row_value;
				}
			}else {
				//Verifica o tipo e inicializa com o valor mínimo ou null
				if( t == typeof(int)) {
					return int.MinValue;
				} else if( t == typeof(decimal)) {
					return decimal.MinValue;
				} else if( t == typeof(DateTime)) {
					return DateTime.MinValue;
				} else if( t == typeof(long)) {
					return long.MinValue;
				} else if( t == typeof(Double)) {
					return Double.MinValue;
				} else {
					return null;
				}
			}
		}
		
		public static object GetObjectArray( Type t, Map[] map, object[] result ) {
			if( null == result || result.Length < 1 )
				throw new ArgumentException( "Invalid size. Must be greater than 1", "result" ); 

			if( null == map || map.Length < 1 )
				throw new ArgumentException( "Invalid size. Must be greater than 1", "map" ); 
			
			object[] instances = (object[])Array.CreateInstance( t, result.Length );
			
			for( int idx = 0, len = instances.Length; idx < len; idx++ ) {
				instances[ idx ] = Activator.CreateInstance( t ); 
			}

			int i = 0;
			foreach( Hashtable row in result ) {
				if( null == row )
					continue;
				
				foreach( Map m in map ) {
					FieldInfo fi = t.GetField( m.member );

					if( fi != null ) {
						object instance = instances[ i ];
						
						if( null != instance ) {
							object row_value = Mapper.GetValue( row,  m.field, fi.FieldType );
							
							fi.SetValue( instance, m.map != null ? m.map.Map( row_value ) : row_value );
						}
						
						continue;
					}

					PropertyInfo pi = t.GetProperty( m.member );

					if( pi != null ) {
						object instance = instances[ i ];

						if( null != instance ) {
							object row_value = Mapper.GetValue( row,  m.field, pi.PropertyType );

							pi.SetValue( instance, m.map != null ? m.map.Map( row_value ) : row_value, null );
						}
					}
				}

				i++;
			}
		
			return instances;
		}
	}
}
