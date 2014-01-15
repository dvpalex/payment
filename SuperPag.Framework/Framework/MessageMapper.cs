using System;
using System.Reflection;
using System.Collections;
using SuperPag.Framework.Helper;
using SuperPag.Framework;

namespace TC.Framework
{
	public interface IMappable 
	{
		
	}

	public class MessageMapper
	{	
		//TODO: key no add
		//TODO: filtrar o output type
		//TODO: seguir a ordem do add na hora de mesclar
		//TODO: nao mesclar mensagens a nao ser que seja especificado
		//TODO: do all 

		public MessageMapper() {}
		
		private void MapElement ( object inputObject , Type elementType , Type[] outputTypes , bool isArray )
		{
			//Verifico se as mensagens de entrada implementam o IMappable
			if  ( Reflection.Implements ( elementType , typeof( IMappable )  ) )
			{
				//Obtem os membros mapeaveis
				MappableMember[] members = (MappableMember[])_cache [ elementType ];

				if ( members == null ) 
				{
					AddTypeToCache ( elementType );
					members = (MappableMember[])_cache [ elementType ];
				}
				
				Hashtable outputs = 
					MapMembers ( members, inputObject, null , outputTypes);

				//Adiciona o resultado
				AddMapMembersResultsToOutput ( outputs , isArray );			
			} 
			else 
			{
				//caso contrario, faz o mapeamento atraves das mensagens de saida
				foreach ( Type outputType in outputTypes )
				{
					//Se nao implementar a IMappable, excepetion
					if ( ! Reflection.Implements ( outputType , typeof( IMappable ) ) )
						throw new Exception ( "Não foi possivel encontrar o objeto mapeavel"); 

					MappableMember[] members;		

					//Obtem os membros mapeaveis
					members = (MappableMember[])_cache [ outputType ];

					Hashtable outputs = 
						MapMembers ( members, inputObject, outputType , outputTypes );

					//Adiciona o resultado
					AddMapMembersResultsToOutput ( outputs , isArray );		
				}
			}
			
			
		}

		private Hashtable MapMembers( 
			MappableMember[] members , 
			object inputObject, 
			Type outputType,
			Type[] outputTypes)
		{
			Hashtable outputs = new Hashtable( 1 );

			foreach ( MappableMember member in members )
			{
				if ( member.Implements ( typeof ( IMappable ) , inputObject ) )
				{
					if (  member.DeclaringType == inputObject.GetType() )
					{
						object child = member.GetValue ( inputObject );
						if ( child != null )
						{
							DoMap ( child , outputTypes );
						}
					} 
				}

				MappingAttribute[] mappingAttributes = (MappingAttribute[])member.GetAttributes ( typeof ( MappingAttribute ) );

				if ( null == mappingAttributes ) continue;
				
				//Para cada atributo de mapeamento
				foreach ( MappingAttribute mappingAtt in mappingAttributes )
				{
					//obtenho o tipo que contem informacoes de mapeamento é o tipo de entrada
					if ( member.DeclaringType == inputObject.GetType() )
					{
						Type mappedType = GetMappedType( mappingAtt,  inputObject.GetType() );

						//Obtenho o objeto de mapeamento
						object mappedObject = GetMappedObject( outputs, mappedType );

						//Obtenho o membro de mapeamento
						MappableMember mappedMember = GetMappedMember ( mappingAtt.Name, mappedType, mappedType );

						if ( mappedMember != null )
						{
							//Seto o valor do membro
							mappedMember.SetValue ( mappedObject, member.GetValue ( inputObject ) );						
						}
					}
					else //se o tipo que contem informacoes de mapeamento é o de saida
					{
						Type mappedType = GetMappedType( mappingAtt,  outputType );

						//Obtenho o objeto de mapeamento
						object mappedObject = GetMappedObject( outputs, outputType );

						//Obtenho o membro de mapeamento
						MappableMember mappedMember = GetMappedMember ( mappingAtt.Name, mappedType , inputObject.GetType() );

						if ( null != mappedMember )
						{
							member.SetValue ( mappedObject, mappedMember.GetValue ( inputObject ) ); 	
						}
					}
					
				}
			}

			return outputs;
		}

		private MappableMember GetMappedMember( string name , Type inputType, Type outputType )
		{
			MappableMember[] members = MappableMember.Get ( outputType );

			return MappableMember.Find ( name, inputType, members );
		}

		private Type GetMappedType ( MappingAttribute mappingAttr , Type mappableType )
		{
			// Se foi especificado o tipo no atributo
			if ( ! mappingAttr.IsDefaultAttribute() )		
				return mappingAttr.Type;
			else
			{
				//Senao obtenho o default da classe
				DefaultMappingAttribute defaultMapping = (DefaultMappingAttribute)
						Attributes.GetSingleAttribute ( mappableType, typeof ( DefaultMappingAttribute ) );

				return defaultMapping.Type;
			}
		}

		private object GetMappedObject( Hashtable outputs , Type mappedType )
		{
			//se já criei uma instancia, uso a existente
			if ( _outputs.ContainsKey ( mappedType ) )
			{
				return _outputs [ mappedType ]; 
			} 
			else
			{
				if ( outputs.ContainsKey ( mappedType ) )
				{
					return outputs [ mappedType ];
				} 
				else
				{
					//se não, crio a instancia e adiciono na saida
					object obj;
					obj = Activator.CreateInstance ( mappedType );
					SetNulls ( mappedType, obj );
					outputs.Add ( mappedType, obj );
					return obj;
				}
			}
		}

		private void SetNulls( Type type, object obj )
		{
			MappableMember[] members = (MappableMember[]) _cache [ type ];

			foreach ( MappableMember member in members )
			{
				member.SetNull ( obj );
			}
		}
		
		private void DoMap( object input , Type[] outputTypes )
		{
			Type inputType = input.GetType();
			if ( inputType.IsArray )
			{
				System.Array array  = (System.Array) input ;
				Type elementType =  inputType.GetElementType();

				foreach ( object item in array )
				{
					MapElement ( item , elementType, outputTypes , true ) ;		
				}
			}  
			else if ( inputType.IsSubclassOf ( typeof ( MessageCollection ) ) )
			{
				System.Array array  = (System.Array)(( MessageCollection )input).ToArray() ;
				Type elementType =  (( MessageCollection )input).GetElementType();

				foreach ( object item in array )
				{
					MapElement ( item , elementType, outputTypes , true ) ;		
				}
			}
			else
			{
				MapElement ( input , inputType, outputTypes , false );
			}
		}
		
		public object Do ( object input , Type outputType )
		{
			this.Add ( input, outputType );
		
			this.Do();

			return this.Get ( outputType );
		}

		public void Do()
		{
			//Cache de todos os fields do input e output
			CacheUsedMembers();

			//Para cada informacao de mapeamento
			int i = 0;
			IEnumerator keys = _maps.Keys.GetEnumerator();
			while ( keys.MoveNext() )
			{
				DoMap ( keys.Current , (Type[])((ArrayList) _maps [ keys.Current ]).ToArray( typeof ( Type ) ) );
				i ++;
			}			
		}


		#region Add and Get

		Hashtable _maps = new Hashtable( 5 );		
		Hashtable _outputs = new Hashtable ( 1 );

		public object Get ( Type type )
		{
			if ( null == _outputs || _outputs.Count == 0 ) return null;

			if ( type.IsSubclassOf ( typeof ( MessageCollection ) ) )
			{
				CollectionOfAttribute collectionOf = (CollectionOfAttribute)Attributes.GetSingleAttribute ( type, typeof ( CollectionOfAttribute ) );
				if ( collectionOf == null )
				{
					throw new ArgumentException ( string.Format ( "Atributo 'CollectionOf' não definido para a classe {0} ",type.Name ) );
				}

				MessageCollection collection = 
					( MessageCollection ) Activator.CreateInstance ( type ); 

				Type collectionType = collectionOf.CollectionType;

				Type arrayType = Type.GetType ( string.Format ( "{0}[],{1}", collectionType.FullName, collectionType.Assembly.FullName));

				System.Array array =  ( System.Array ) _outputs [ arrayType ];

				if ( null != array )
				{
					foreach ( object item in array )
					{
						collection.Add ( ( Message ) item );
					}
				}

				return collection;
			} 
			else
			{
				object o = _outputs [ type ];
				return o ;
			}
		}

		public void  Add ( object input, Type output )
		{
			if ( input == null ) throw new ArgumentNullException( "input" );
			if ( output == null ) throw new ArgumentNullException( "output" );

			if ( _maps.ContainsKey ( input ) )
			{
				ArrayList lst = (ArrayList)_maps [ input ];
				lst.Add ( output );
			} 
			else
			{
				ArrayList lst = new ArrayList( 3 );

				if ( output.IsSubclassOf ( typeof ( MessageCollection ) ) )
				{
					CollectionOfAttribute collectionOf = (CollectionOfAttribute)Attributes.GetSingleAttribute ( output, typeof ( CollectionOfAttribute ) );
					if ( collectionOf == null )
					{
						throw new ArgumentException ( string.Format ( "Atributo 'CollectionOf' não definido para a classe {0} ", output.Name ) );
					}

					lst.Add ( collectionOf.CollectionType );
				} 
				else
				{
					lst.Add ( output );
				}
				_maps [ input ] = lst;
			}
		}

		#endregion

		#region Cache member details

		Hashtable _cache = new Hashtable( 10 );

		private void CacheUsedMembers()
		{
			IEnumerator keys = _maps.Keys.GetEnumerator();
			while ( keys.MoveNext() )
			{
				foreach ( Type type in (ArrayList) _maps [ keys.Current ] )
				{
					AddTypeToCache (  type );
				}
				
				AddTypeToCache (  keys.Current.GetType() );
			}
		}


		private void AddTypeToCache ( Type type )
		{
			if ( type.IsArray )
			{
				if ( ! _cache.ContainsKey ( type.GetElementType() ) )
				{
					_cache.Add ( type.GetElementType(), MappableMember.Get ( type.GetElementType() ) ) ;
				}
			}
			else
			{
				if ( ! _cache.ContainsKey ( type ) )
				{
					_cache.Add ( type,  MappableMember.Get ( type ) ) ;
				}
			}
		}


		#endregion

		private void AddMapMembersResultsToOutput( Hashtable membersResult , bool isArray )
		{
			IEnumerator keys = membersResult.GetEnumerator();
			while ( keys.MoveNext() )
			{
				DictionaryEntry current = (DictionaryEntry)keys.Current;
				object output = current.Value ;

				if ( isArray )
				{
					//"Monto" o tipo do array
					Type arrayType = ((object[])Array.CreateInstance ( (Type)current.Key, 0 )).GetType();

					//Verifico se o array já existe
					if ( _outputs.ContainsKey ( arrayType ) )
					{
						//Em caso afirmativo incremento
						object[] array = (object[])_outputs [ arrayType ]; 
						ArrayList temp = new ArrayList ( array );
						temp.Add ( output );
						_outputs [ arrayType ] = (object[])temp.ToArray( (Type)current.Key );
					}
					else
					{
						//Em caso negativo, crio
						object[] array = (object[])Array.CreateInstance ( (Type)current.Key, 1 );
						array [ 0 ] = output;

						//Adiciono o array na lista de saidas
						_outputs.Add ( array.GetType() , array );
					}					
				} 
				else
				{
					if ( ! _outputs.ContainsKey (  current.Key ) )
					{
						//Adiciono o objeto na lista de saidas
						_outputs.Add ( current.Key , output );
					}
				}				
			}
		}
		
	}

	#region MappableMember

	public class MappableMember 
	{
		public MappableMember ( PropertyInfo property ,  bool isInMappableClass )
		{
			isProperty = true;
			_isInMappableClass = isInMappableClass;
			_property = (PropertyInfo)property;
			_member = property;
		}

		public MappableMember ( FieldInfo field , bool isInMappableClass )
		{
			isField = true;
			_isInMappableClass = isInMappableClass;
			_field = (FieldInfo)field;
			_member = field;
		}


		MemberInfo _member;
		FieldInfo _field;
		PropertyInfo _property;
		bool _isInMappableClass;

		bool isProperty = false;
		bool isField = false;

		public Type DeclaringType
		{
			get
			{
				return _member.DeclaringType;
			}
		}

		public bool IsInMappableClass
		{
			get
			{
				return _isInMappableClass;
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
					return Reflection.Implements ( memberType, interfaceType );	
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
				value = Convert.ToInt32( value );
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

		public static MappableMember Find( string memberName, Type declaringType, MappableMember[] mappableMembers  )
		{
			foreach ( MappableMember b in mappableMembers )
			{
				if ( b.Name == memberName && b.DeclaringType == declaringType )
				{
					return b;
				}
			}
			return null;
		}


		public static MappableMember[] Get( Type type )
		{
			if ( null == type ) throw new ArgumentNullException ( "type" );

			ArrayList members = new ArrayList( 15 );

			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public ;

			PropertyInfo[] properties = type.GetProperties ( flags );

			bool isInMappableClass = Reflection.Implements ( type , typeof ( IMappable ) );

			foreach ( PropertyInfo p in properties )
			{
				members.Add ( new MappableMember ( p , isInMappableClass) );
			}

			FieldInfo[] fields = type.GetFields ( flags );

			foreach ( FieldInfo f in fields )
			{
				members.Add ( new MappableMember ( f , isInMappableClass ) );
			}
		
			return (MappableMember[])members.ToArray ( typeof ( MappableMember ) );
		}	
	}

	#endregion

	#region attributes

	public class DefaultMappingAttribute : Attribute 
	{
		public Type Type;

		public DefaultMappingAttribute ( Type type )
		{
			this.Type = type; 
		}
	}

	public class MappingAttribute : Attribute 
	{
		public string Name;
		public Type Type;

		public MappingAttribute ( string name )
		{
			this.Name = name;			
		}

		public MappingAttribute ( string name , Type type )
		{
			this.Name = name;
			this.Type = type; 
		}

		public override bool IsDefaultAttribute()
		{
			return Type == null ;
		}
	}

	public class NoMappingAttribute : Attribute 
	{
		public NoMappingAttribute ()
		{
		}
	}

	#endregion
}
