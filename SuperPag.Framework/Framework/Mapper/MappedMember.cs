using System;
using System.Collections;
using System.Reflection;

namespace SuperPag.Framework
{
	internal class MappableMember 
	{
		public MappableMember ( PropertyInfo property  )
		{
			isProperty = true;
			_property = (PropertyInfo)property;
			_member = property;
		}

		public MappableMember ( FieldInfo field  )
		{
			isField = true;
			_field = (FieldInfo)field;
			_member = field;
		}


		MemberInfo _member;
		FieldInfo _field;
		PropertyInfo _property;
	
		bool isProperty = false;
		bool isField = false;

		public Type DeclaringType
		{
			get
			{
				return _member.DeclaringType;
			}
		}

	
		public bool IsReadOnly
		{
			get
			{
				if ( isField )
				{				
					return false;
				} 
				else 
				{
					return ! _property.CanWrite;				
				}
			}
		}

		public string Name
		{
			get
			{
				return _member.Name;
			}
		}

		public Attribute[] GetAttributes ( Type attributeType )
		{
			return (Attribute[])_member.GetCustomAttributes ( attributeType , true );
		}

		public Type Type 
		{
			get
			{
				if ( isField )
				{				
					return _field.FieldType;				
				} 
				else 
				{
					return _property.PropertyType;				
				}
			}
		}
		

		
		public bool Implements ( Type interfaceType , object obj )
		{
			if ( isField )
			{				
				return Implements ( _field.FieldType, interfaceType , obj );				
			} 
			else if ( isProperty )
			{
				return Implements ( _property.PropertyType, interfaceType , obj  );				
			}

			return false;			
		}

		private bool Implements ( Type memberType, Type interfaceType , object obj )
		{
			if ( memberType.IsClass && memberType != typeof ( System.String ) )
			{
				if ( ! memberType.IsSubclassOf ( typeof ( MessageCollection ) ) )
				{	 			
					return Helper.Reflection.Implements ( memberType, interfaceType );	
				} 
				else
				{
					//					if ( isField )
					//					{
					//						MessageCollection messageColl = (MessageCollection)_field.GetValue ( obj );
					//						return Reflection.Implements ( messageColl.GetElementType(), interfaceType );
					//					} 
					//					else if ( isProperty )
					//					{
					//						MessageCollection messageColl = (MessageCollection)_property.GetValue ( obj , null );
					//						return Reflection.Implements ( messageColl.GetElementType(), interfaceType );				
					//					}
					return  false;
				}
			} 
			return false;
		}
	
		public object GetValue ( object obj )
		{
			if ( isField )
			{
				return _field.GetValue ( obj );
			} 
			else if ( isProperty )
			{
				return _property.GetValue ( obj , null );
			}
			else
			{
				return null;
			}
		}

		public void SetNull (  object obj )
		{
			if ( isField )
			{
				SetNull ( _field.FieldType , obj );					
			} 
			else if ( isProperty )
			{
				SetNull ( _property.PropertyType , obj);
			}
		}

		private void SetNull ( Type type , object obj )
		{
			if ( type == typeof ( int ) )
				SetValue ( obj, int.MinValue );
			else if ( type == typeof ( Decimal ) )
				SetValue ( obj, decimal.MinValue );
			else if ( type  == typeof ( DateTime ) )
				SetValue ( obj, DateTime.MinValue );	
			else if ( type  == typeof ( String ) )
				SetValue ( obj, string.Empty );	
		}

		public void SetValue ( object obj, object value )
		{
			if ( value != null && value.GetType().IsEnum )
			{
                if (_field.FieldType == typeof(Byte))
                    value = Convert.ToByte(value);
                else
                    value = Convert.ToInt32(value);
			}

			if ( isField )
			{
				_field.SetValue ( obj , value );
			} 
			else if ( isProperty )
			{
				if ( _property.PropertyType.IsEnum ) 
				{
					_property.SetValue ( obj , Enum.Parse (  _property.PropertyType, Enum.GetName (  _property.PropertyType, value ) ), null );
				}
				else
				{
					_property.SetValue ( obj , value , null );
				}
			}			
		}

		//TODO: testar para o filho do filho ( exemplo: DEmpresa.DFuncionario.DPessoa )
		public static MappableMember Find( string memberName, Type declaringType, MappableMember[] mappableMembers  , object inputObject, out object mappedMemberObject )
		{
			foreach ( MappableMember b in mappableMembers )
			{
				if ( declaringType == b.Type )
				{
					MappableMember member = MappableMember.Find ( memberName, declaringType, MappableMember.Get ( declaringType ) , inputObject, out mappedMemberObject);

					if ( member != null )
					{
						mappedMemberObject = b.GetValue ( inputObject );
						return member;
					}										
				} 
				else if ( b.Name == memberName && b.DeclaringType == declaringType )
				{
					mappedMemberObject = inputObject;
					return b;
				}
			}
			mappedMemberObject = null;
			return null;
		}


		public static MappableMember[] Get( Type type )
		{
			if ( null == type ) throw new ArgumentNullException ( "type" );

			ArrayList members = new ArrayList( 15 );

			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public ;

			PropertyInfo[] properties = type.GetProperties ( flags );

			foreach ( PropertyInfo p in properties )
			{
				members.Add ( new MappableMember ( p ) );
			}

			FieldInfo[] fields = type.GetFields ( flags );

			foreach ( FieldInfo f in fields )
			{
				members.Add ( new MappableMember ( f  ) );
			}
		
			return (MappableMember[])members.ToArray ( typeof ( MappableMember ) );
		}	
	}

}
