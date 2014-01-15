using System;
using System.Reflection;
using System.Collections;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework
{

	public class MessageMapper 
	{	
		public enum MapperDirections
		{
			Undefined = 0,
			MessageToData = 1,
			DataToMessage = 2
		}

		bool _mapChildren = false;
		MapperDirections _direction = MapperDirections.Undefined ;
		Hashtable _outputs = new Hashtable ( 1 );

		public bool MapChildren
		{
			//TODO: o map children nao funciona no sentido message to data
			get { return this._mapChildren; } set { _mapChildren = value; }
		}

		public MapperDirections Direction
		{
			get { return this._direction; }
		}

		public IMessageMapable Do ( IDataMessageMapable dataInput , Type messageOutput )
		{
			this._direction = MapperDirections.DataToMessage ;
			this.Add ( dataInput, messageOutput );

			this.Do();

			return (IMessageMapable)this.Get ( messageOutput );
		}

		public IMessageCollectionMapable Do ( IDataMessageMapable[] arrDataInput , Type messageCollectionOutput )
		{
			this._direction = MapperDirections.DataToMessage ;
			this.Add ( arrDataInput, messageCollectionOutput );

			this.Do();

			return (IMessageCollectionMapable)this.Get ( messageCollectionOutput );
		}

		public IDataMessageMapable Do ( IMessageMapable messageInput , Type dataOutput )
		{
			this._direction = MapperDirections.MessageToData ;
			this.Add ( messageInput, dataOutput );

			this.Do();

			return (IDataMessageMapable)this.Get ( dataOutput );
		}

		public IDataMessageMapable[] Do ( IMessageCollectionMapable messageCollectionInput , Type dataArrayOutput )
		{
			this._direction = MapperDirections.MessageToData ;
			this.Add ( messageCollectionInput, dataArrayOutput );

			this.Do();

			return (IDataMessageMapable[])this.Get ( dataArrayOutput );
		}

		public void Do()
		{
			//Cache de todos os fields do input e output
			CacheUsedMembers();

			foreach ( object input in this._inputObjects )
			{
				foreach ( Type outputType in this._outputTypes )
				{
					DoMap ( input , outputType );
				}
			}
		}

		ArrayList _inputObjects = new ArrayList();
		ArrayList _outputTypes = new ArrayList();

		Hashtable _cache = new Hashtable( 10 );

		private void DoMap( object input , Type outputType )
		{
			Type inputType = input.GetType();
			if ( inputType.IsArray )
			{
				System.Array array  = (System.Array) input ;
				Type elementType =  inputType.GetElementType();
		
				foreach ( object item in array )
				{
					Hashtable result = MapElement ( item , outputType ) ;	
					AddMapResultToOutput ( result , true );
				}
			}  
			else if ( inputType.IsSubclassOf ( typeof ( MessageCollection ) ) )
			{
				System.Array array  = (System.Array)(( MessageCollection )input).ToArray() ;
				Type elementType =  (( MessageCollection )input).GetElementType();
		
				foreach ( object item in array )
				{
					Hashtable result = MapElement ( item , outputType ) ;	
					AddMapResultToOutput ( result , true );		
				}
			}
			else
			{
				Hashtable result = MapElement ( input , outputType ) ;	
				AddMapResultToOutput ( result , false );
			}
		}

		private void  Add ( object input, Type output )
		{
			this.Add ( input );
			this.Add ( output );			
		}

		private void  Add ( object input )
		{
			if ( input != null )
			{
				if ( _inputObjects.IndexOf ( input ) == -1 )
				{
					_inputObjects.Add ( input );
				} 		
			}
		}

		private void  Add ( Type output )
		{
			if ( output == null ) throw new ArgumentNullException( "output" );

			if ( output.IsSubclassOf ( typeof ( MessageCollection ) ) )
			{
				CollectionOfAttribute collectionOf = (CollectionOfAttribute)Attributes.GetSingleAttribute ( output, typeof ( CollectionOfAttribute ) );
				if ( collectionOf == null )
				{
					throw new ArgumentException ( string.Format ( "Atributo 'CollectionOf' não definido para a classe {0} ", output.Name ) );
				}
			
				_outputTypes.Add ( collectionOf.CollectionType );
			} 
			else if ( output.IsArray )
			{
				_outputTypes.Add ( output.GetElementType() );
			} 
			else
			{
				_outputTypes.Add ( output );
			}			 			
		}

		private void CacheUsedMembers()
		{
			foreach ( object o in this._inputObjects )
			{
				AddTypeToCache (  o.GetType() );
			}

			foreach ( Type t in this._outputTypes )
			{
				AddTypeToCache (  t );
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

		private object GetMappedObject( Hashtable output, Type mappedType )
		{
			//se já criei uma instancia, uso a existente
			if ( output.ContainsKey ( mappedType ) )
			{
				return output [ mappedType ]; 
			} 
			else
			{
				if ( output.ContainsKey ( mappedType ) )
				{
					return output [ mappedType ];
				} 
				else
				{
					//se não, crio a instancia e adiciono na saida
					object obj;
					obj = Activator.CreateInstance ( mappedType );
					SetNulls ( mappedType, obj );
					output.Add ( mappedType, obj );
					return obj;
				}
			}
		}

		private Hashtable MapMembers( 
			MappableMember[] members , 
			object inputObject, 
			Type outputType )
		{ 
			Hashtable output = new Hashtable( 10 );

			foreach ( MappableMember member in members )
			{				
				MappingAttribute[] mappingAttributes = (MappingAttribute[])member.GetAttributes ( typeof ( MappingAttribute ) );

				if ( null != mappingAttributes && mappingAttributes.Length != 0 ) 
				{				
					//Para cada atributo de mapeamento
					foreach ( MappingAttribute mappingAtt in mappingAttributes )
					{
						if ( _direction == MapperDirections.MessageToData )
						{
							Type mappedType = GetMappedType( mappingAtt, inputObject.GetType() );

							if ( mappedType == outputType )
							{
								//Obtenho o objeto de mapeamento
								object mappedObject = GetMappedObject( output, mappedType );
								//Obtenho o membro de mapeamento
								object obj;
								MappableMember mappedMember = GetMappedMember ( mappingAtt.Name, mappedType , outputType , inputObject, out obj );

								if ( null != mappedMember )
								{
									object sourceValue =  member.GetValue ( inputObject );
									mappedMember.SetValue ( mappedObject, sourceValue );
								}
							}
						}
						else
						{
							Type mappedType = GetMappedType( mappingAtt, outputType );

							//Obtenho o objeto de mapeamento
							object mappedObject = GetMappedObject( output, outputType );

							object obj;

							//Obtenho o membro de mapeamento
							MappableMember mappedMember = GetMappedMember ( mappingAtt.Name, mappedType , inputObject.GetType() , inputObject, out obj );
						
							if ( null != mappedMember )
							{									
								//TODO: input deve ser alterado para o field
								object sourceValue =  mappedMember.GetValue ( obj );
								member.SetValue ( mappedObject, sourceValue ); 									
							}
						}
					}
				} 
				//se for para maperar os filhos e a direcao for datamessage -> message
				else if ( this._mapChildren && this._direction == MapperDirections.DataToMessage )
				{					
					DoMapChildren ( member , inputObject , output , outputType );
				}
			}

			return output;
		}

		private void DoMapChildren( MappableMember member , object inputObject , Hashtable output , Type outputType )
		{
			Type childType = null;
			
			//verifico se o tipo é uma collection de messages
			if ( member.Type.IsSubclassOf ( typeof ( MessageCollection ) ) )
			{
				//obtenho o tipo da mesagem da coleção
				CollectionOfAttribute collectionOf = (CollectionOfAttribute)Attributes.GetSingleAttribute ( member.Type, typeof ( CollectionOfAttribute ) );
				if ( collectionOf == null )
					return;

				childType = collectionOf.CollectionType;
			} 
			else if ( member.Type.IsSubclassOf ( typeof ( Message ) ) )
			{
				childType = member.Type; 						
			} 
			else
			{
				//vou para o proximo membro
				return;
			}

			//obtenho o tipo da datamessage que o membro deverá mapear
			DefaultMappingAttribute mappingAttribute = 
				(DefaultMappingAttribute)Attributes.GetSingleAttribute ( childType , typeof ( DefaultMappingAttribute ) );

			if ( mappingAttribute != null )
			{
				//TODO: cache 

				if ( mappingAttribute.Type == inputObject.GetType() )
				{
					object memberValue = inputObject;

					//TODO: encapsular
					if ( memberValue != null )
					{							
						Hashtable result = null;

						MappableMember[] members = (MappableMember[])_cache [ member.Type ];
						if ( members == null ) 
						{
							AddTypeToCache ( member.Type );
							members = (MappableMember[])_cache [ member.Type ];
						}
					
						result = MapElement ( memberValue, member.Type );

						if ( result != null )
						{
							object obj = result [  member.Type ] ;

							object mappedObject = GetMappedObject( output, outputType );

							member.SetValue ( mappedObject , obj );
						}
					}

				} 
				else
				{
					//tento encontrar o tipo na DataMessage de entrada
					MappableMember dataMessageMember = (MappableMember)FindMemberByType ( inputObject, mappingAttribute.Type );

					//se achou o membro no datamessage com aquele tipo
					if ( dataMessageMember != null )
					{
						//obtem o valor
						object memberValue = dataMessageMember.GetValue ( inputObject );
						if ( memberValue != null )
						{							
							//TODO: encapsular
							if ( !  memberValue.GetType().IsArray )
							{
								Hashtable result = null;

								MappableMember[] members = (MappableMember[])_cache [ member.Type ];
								if ( members == null ) 
								{
									AddTypeToCache ( member.Type );
									members = (MappableMember[])_cache [ member.Type ];
								}
						
								result = MapElement ( memberValue, member.Type );

								if ( result != null )
								{
									object obj = result [  member.Type ] ;

									object mappedObject = GetMappedObject( output, outputType );

									member.SetValue ( mappedObject , obj );
								}
							} 
							else
							{
//								System.Array array  = (System.Array) memberValue ;
//								Type elementType =  array.GetType().GetElementType();
//
//
//								MappableMember[] members = (MappableMember[])_cache [ elementType ];
//								if ( members == null ) 
//								{
//									AddTypeToCache ( elementType );
//									members = (MappableMember[])_cache [ elementType  ];
//								}
//
//								Type arrayType = null;
//	
//								Hashtable hash1 = new Hashtable();
//								foreach ( object item in array )
//								{
//									Hashtable result = MapElement ( item , outputType ) ;	
//
//									IEnumerator keys = result.GetEnumerator();
//									while ( keys.MoveNext() )
//									{
//										DictionaryEntry current = (DictionaryEntry)keys.Current;
//										object output1 = current.Value ;
//										//"Monto" o tipo do array
//										arrayType = ((object[])Array.CreateInstance ( (Type)current.Key, 0 )).GetType();
//
//										//Verifico se o array já existe
//										if ( hash1.ContainsKey ( arrayType ) )
//										{
//											//Em caso afirmativo incremento
//											object[] array1 = (object[])hash1 [ arrayType ]; 
//											ArrayList temp = new ArrayList ( array1 );
//											temp.Add ( output1 );
//											hash1 [ arrayType ] = (object[])temp.ToArray( (Type)current.Key );
//										}
//										else
//										{
//											//Em caso negativo, crio
//											object[] array1 = (object[])Array.CreateInstance ( (Type)current.Key, 1 );
//											array1 [ 0 ] = output1;
//
//											//Adiciono o array na lista de saidas
//											hash1.Add ( arrayType , array1 );
//										}	
//									}
//								}
//
//								if ( hash1 != null && arrayType != null )
//								{
//									//TODO: verificar em caso de message collection
//									object obj = hash1 [ arrayType ] ;
//
//									object mappedObject = GetMappedObject( output, outputType );
//
//									member.SetValue ( mappedObject , obj );
//								}
							}
						}
					}
				}
			}
		}

		private MappableMember FindMemberByType ( object inputObject , Type type )
		{
			AddTypeToCache ( inputObject.GetType() );
			MappableMember[] members = (MappableMember[])_cache [ inputObject.GetType() ];

			foreach ( MappableMember member in members )
			{
				if ( member.Type == type || member.Type.GetElementType() == type )
				{
					return member;
				}
			}

			return null;
		}


		private MappableMember GetMappedMember( string name , Type inputType, Type outputType , object inputObject, out object mappedMemberObject )
		{
			MappableMember[] members = MappableMember.Get ( outputType );

			return MappableMember.Find ( name, inputType, members, inputObject , out mappedMemberObject );
		}

		private void SetNulls( Type type, object obj )
		{
			MappableMember[] members = (MappableMember[]) _cache [ type ];
			if ( members == null ) 
			{
				AddTypeToCache ( type );
				members = (MappableMember[])_cache [ type  ];
			}

			foreach ( MappableMember member in members )
			{
				if ( ! member.IsReadOnly )
				{
					member.SetNull ( obj );
				}
			}
		}

		private Hashtable MapElement ( object inputObject , Type outputType )
		{
			MappableMember[] members = null;

			//Obtem os membros mapeaveis
			if ( _direction == MapperDirections.DataToMessage )
			{
				members = (MappableMember[])_cache [ outputType ];

				if ( members == null ) 
				{
					AddTypeToCache ( outputType );
					members = (MappableMember[])_cache [ outputType ];
				}
				
			}
			else if ( _direction == MapperDirections.MessageToData ) 
			{
				members = (MappableMember[])_cache [ inputObject.GetType() ];

				if ( members == null ) 
				{
					AddTypeToCache ( inputObject.GetType() );
					members = (MappableMember[])_cache [ inputObject.GetType() ];
				}
			}

			return MapMembers ( members, inputObject, outputType );
		}

		private void AddMapResultToOutput( Hashtable membersResult , bool isCollection )
		{
			IEnumerator keys = membersResult.GetEnumerator();
			while ( keys.MoveNext() )
			{
				DictionaryEntry current = (DictionaryEntry)keys.Current;
				object output = current.Value ;

				if ( isCollection )
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

		private object Get ( Type type )
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

		public void AddInput( object input )
		{
			this.Add ( input );
		}

		public void AddOutput( Type type )
		{
			this.Add ( type );
		}

		public object GetOutput( Type type )
		{
			return this.Get ( type );
		}
	}
}







//using System;
//using System.Reflection;
//using System.Collections;
//
//namespace SuperPag.Framework
//{
//
//	public class MessageMapper 
//	{	
//		public enum MapperDirections
//		{
//			Undefined = 0,
//			MessageToData = 1,
//			DataToMessage = 2
//		}
//
//		bool _mapChildren = false;
//		MapperDirections _direction = MapperDirections.Undefined ;
//		Hashtable _outputs = new Hashtable ( 1 );
//
//		public bool MapChildren
//		{
//			//TODO: o map children nao funciona no sentido message to data
//			get { return this._mapChildren; } set { _mapChildren = value; }
//		}
//
//		public MapperDirections Direction
//		{
//			get { return this._direction; }
//		}
//
//		public IMessageMapable Do ( IDataMessageMapable dataInput , Type messageOutput )
//		{
//			this._direction = MapperDirections.DataToMessage ;
//			this.Add ( dataInput, messageOutput );
//
//			this.Do();
//
//			return (IMessageMapable)this.Get ( messageOutput );
//		}
//
//		public IMessageCollectionMapable Do ( IDataMessageMapable[] arrDataInput , Type messageCollectionOutput )
//		{
//			this._direction = MapperDirections.DataToMessage ;
//			this.Add ( arrDataInput, messageCollectionOutput );
//
//			this.Do();
//
//			return (IMessageCollectionMapable)this.Get ( messageCollectionOutput );
//		}
//
//		public IDataMessageMapable Do ( IMessageMapable messageInput , Type dataOutput )
//		{
//			this._direction = MapperDirections.MessageToData ;
//			this.Add ( messageInput, dataOutput );
//
//			this.Do();
//
//			return (IDataMessageMapable)this.Get ( dataOutput );
//		}
//
//		public IDataMessageMapable[] Do ( IMessageCollectionMapable messageCollectionInput , Type dataArrayOutput )
//		{
//			this._direction = MapperDirections.MessageToData ;
//			this.Add ( messageCollectionInput, dataArrayOutput.GetElementType() );
//
//			this.Do();
//
//			return (IDataMessageMapable[])this.Get ( dataArrayOutput );
//		}
//
//		public void Do()
//		{
//			//Cache de todos os fields do input e output
//			CacheUsedMembers();
//
//			int i = 0;
//			IEnumerator keys = _inputObjects.Keys.GetEnumerator();
//			while ( keys.MoveNext() )
//			{
//				DoMap ( keys.Current , ((Type[])((ArrayList) _inputObjects [ keys.Current ]).ToArray( typeof ( Type ) ))[0] );
//				i ++;
//			}			
//		}
//
//		Hashtable _inputObjects = new Hashtable( 5 );
//		Hashtable _cache = new Hashtable( 10 );
//
//		private void DoMap( object input , Type outputType )
//		{
//			Type inputType = input.GetType();
//			if ( inputType.IsArray )
//			{
//				System.Array array  = (System.Array) input ;
//				Type elementType =  inputType.GetElementType();
//		
//				foreach ( object item in array )
//				{
//					Hashtable result = MapElement ( item , outputType ) ;	
//					AddMapResultToOutput ( result , true );
//				}
//			}  
//			else if ( inputType.IsSubclassOf ( typeof ( MessageCollection ) ) )
//			{
//				System.Array array  = (System.Array)(( MessageCollection )input).ToArray() ;
//				Type elementType =  (( MessageCollection )input).GetElementType();
//		
//				foreach ( object item in array )
//				{
//					Hashtable result = MapElement ( item , outputType ) ;	
//					AddMapResultToOutput ( result , true );		
//				}
//			}
//			else
//			{
//				Hashtable result = MapElement ( input , outputType ) ;	
//				AddMapResultToOutput ( result , false );
//			}
//		}
//
//		private void  Add ( object input, Type output )
//		{
//			if ( input == null ) throw new ArgumentNullException( "input" );
//			if ( output == null ) throw new ArgumentNullException( "output" );
//
//			if ( _inputObjects.ContainsKey ( input ) )
//			{
//				ArrayList lst = (ArrayList)_inputObjects [ input ];
//				lst.Add ( output );
//			} 
//			else
//			{
//				ArrayList lst = new ArrayList( 3 );
//
//				if ( output.IsSubclassOf ( typeof ( MessageCollection ) ) )
//				{
//					CollectionOfAttribute collectionOf = (CollectionOfAttribute)Attributes.GetSingleAttribute ( output, typeof ( CollectionOfAttribute ) );
//					if ( collectionOf == null )
//					{
//						throw new ArgumentException ( string.Format ( "Atributo 'CollectionOf' não definido para a classe {0} ", output.Name ) );
//					}
//
//					lst.Add ( collectionOf.CollectionType );
//				} 
//				else
//				{
//					lst.Add ( output );
//				}
//				_inputObjects [ input ] = lst;
//			}
//		}
//
//		private void CacheUsedMembers()
//		{
//			IEnumerator keys = _inputObjects.Keys.GetEnumerator();
//			while ( keys.MoveNext() )
//			{
//				foreach ( Type type in (ArrayList) _inputObjects [ keys.Current ] )
//				{
//					AddTypeToCache (  type );
//				}
//				
//				AddTypeToCache (  keys.Current.GetType() );
//			}
//		}
//
//		private void AddTypeToCache ( Type type )
//		{
//			if ( type.IsArray )
//			{
//				if ( ! _cache.ContainsKey ( type.GetElementType() ) )
//				{
//					_cache.Add ( type.GetElementType(), MappableMember.Get ( type.GetElementType() ) ) ;
//				}
//			}
//			else
//			{
//				if ( ! _cache.ContainsKey ( type ) )
//				{
//					_cache.Add ( type,  MappableMember.Get ( type ) ) ;
//				}
//			}
//		}
//
//		private Type GetMappedType ( MappingAttribute mappingAttr , Type mappableType )
//		{
//			// Se foi especificado o tipo no atributo
//			if ( ! mappingAttr.IsDefaultAttribute() )		
//				return mappingAttr.Type;
//			else
//			{
//				//Senao obtenho o default da classe
//				DefaultMappingAttribute defaultMapping = (DefaultMappingAttribute)
//					Attributes.GetSingleAttribute ( mappableType, typeof ( DefaultMappingAttribute ) );
//
//				return defaultMapping.Type;
//			}
//		}
//
//		private object GetMappedObject( Hashtable output, Type mappedType )
//		{
//			//se já criei uma instancia, uso a existente
//			if ( output.ContainsKey ( mappedType ) )
//			{
//				return output [ mappedType ]; 
//			} 
//			else
//			{
//				if ( output.ContainsKey ( mappedType ) )
//				{
//					return output [ mappedType ];
//				} 
//				else
//				{
//					//se não, crio a instancia e adiciono na saida
//					object obj;
//					obj = Activator.CreateInstance ( mappedType );
//					SetNulls ( mappedType, obj );
//					output.Add ( mappedType, obj );
//					return obj;
//				}
//			}
//		}
//
//		private Hashtable MapMembers( 
//			MappableMember[] members , 
//			object inputObject, 
//			Type[] outputTypes )
//		{ 
//			Hashtable output = new Hashtable( 10 );
//
//			foreach ( Type outputType in outputTypes )
//			{
//				foreach ( MappableMember member in members )
//				{				
//					MappingAttribute[] mappingAttributes = (MappingAttribute[])member.GetAttributes ( typeof ( MappingAttribute ) );
//
//					if ( null != mappingAttributes && mappingAttributes.Length != 0 ) 
//					{				
//						//Para cada atributo de mapeamento
//						foreach ( MappingAttribute mappingAtt in mappingAttributes )
//						{
//							if ( _direction == MapperDirections.MessageToData )
//							{
//								Type mappedType = GetMappedType( mappingAtt, inputObject.GetType() );
//								//Obtenho o objeto de mapeamento
//								object mappedObject = GetMappedObject( output, mappedType );
//								//Obtenho o membro de mapeamento
//								MappableMember mappedMember = GetMappedMember ( mappingAtt.Name, mappedType , outputType );
//
//								if ( null != mappedMember )
//								{
//									object sourceValue =  member.GetValue ( inputObject );
//									mappedMember.SetValue ( mappedObject, sourceValue ); 								
//								}
//							}
//							else
//							{
//								Type mappedType = GetMappedType( mappingAtt, outputType );
//								//Obtenho o objeto de mapeamento
//								object mappedObject = GetMappedObject( output, outputType );
//								//Obtenho o membro de mapeamento
//								MappableMember mappedMember = GetMappedMember ( mappingAtt.Name, mappedType , inputObject.GetType() );
//
//								if ( null != mappedMember )
//								{
//									object sourceValue =  mappedMember.GetValue ( inputObject );
//									member.SetValue ( mappedObject, sourceValue ); 									
//								}
//							}
//						}
//					} 
//						//se for para maperar os filhos e a direcao for datamessage -> message
//					else if ( this._mapChildren && this._direction == MapperDirections.DataToMessage )
//					{					
//						DoMapChildren ( member , inputObject , output , outputType );
//					}
//				}
//			}
//
//			return output;
//		}
//
//		private void DoMapChildren( MappableMember member , object inputObject , Hashtable output , Type outputType )
//		{
//			Type childType = null;
//			
//			//verifico se o tipo é uma collection de messages
//			if ( member.Type.IsSubclassOf ( typeof ( MessageCollection ) ) )
//			{
//				//obtenho o tipo da mesagem da coleção
//				CollectionOfAttribute collectionOf = (CollectionOfAttribute)Attributes.GetSingleAttribute ( member.Type, typeof ( CollectionOfAttribute ) );
//				if ( collectionOf == null )
//					return;
//
//				childType = collectionOf.CollectionType;
//			} 
//			else if ( member.Type.IsSubclassOf ( typeof ( Message ) ) )
//			{
//				childType = member.Type; 						
//			} 
//			else
//			{
//				//vou para o proximo membro
//				return;
//			}
//
//			//obtenho o tipo da datamessage que o membro deverá mapear
//			DefaultMappingAttribute mappingAttribute = 
//				(DefaultMappingAttribute)Attributes.GetSingleAttribute ( childType , typeof ( DefaultMappingAttribute ) );
//
//			if ( mappingAttribute != null )
//			{
//				//TODO: cache 
//
//				//tento encontrar o tipo na DataMessage de entrada
//				MappableMember dataMessageMember = (MappableMember)FindMemberByType ( inputObject, mappingAttribute.Type );
//
//				//se achou o membro no datamessage com aquele tipo
//				if ( dataMessageMember != null )
//				{
//					//obtem o valor
//					object memberValue = dataMessageMember.GetValue ( inputObject );
//					if ( memberValue != null )
//					{							
//						Hashtable result = null;
//
//						//TODO: encapsular
//						MappableMember[] members = (MappableMember[])_cache [ member.Type ];
//						if ( members == null ) 
//						{
//							AddTypeToCache ( member.Type );
//							members = (MappableMember[])_cache [ member.Type ];
//						}
//						
//						result = MapElement ( memberValue, member.Type );
//
//						if ( result != null )
//						{
//							//TODO: verificar em caso de message collection
//							object obj = result [  member.Type ] ;
//
//							object mappedObject = GetMappedObject( output, outputType );
//
//							member.SetValue ( mappedObject , obj );
//						}
//					}
//				}
//			}
//		}
//
//		private MappableMember FindMemberByType ( object inputObject , Type type )
//		{
//			AddTypeToCache ( inputObject.GetType() );
//			MappableMember[] members = (MappableMember[])_cache [ inputObject.GetType() ];
//
//			foreach ( MappableMember member in members )
//			{
//				if ( member.Type == type )
//				{
//					return member;
//				}
//			}
//
//			return null;
//		}
//
//
//		private MappableMember GetMappedMember( string name , Type inputType, Type outputType )
//		{
//			MappableMember[] members = MappableMember.Get ( outputType );
//
//			return MappableMember.Find ( name, inputType, members );
//		}
//
//		private void SetNulls( Type type, object obj )
//		{
//			MappableMember[] members = (MappableMember[]) _cache [ type ];
//
//			foreach ( MappableMember member in members )
//			{
//				if ( ! member.IsReadOnly )
//				{
//					member.SetNull ( obj );
//				}
//			}
//		}
//
//		private Hashtable MapElement ( object inputObject , Type outputType )
//		{
//			MappableMember[] members = null;
//
//			//Obtem os membros mapeaveis
//			if ( _direction == MapperDirections.DataToMessage )
//			{
//				members = (MappableMember[])_cache [ outputType ];
//
//				if ( members == null ) 
//				{
//					AddTypeToCache ( outputType );
//					members = (MappableMember[])_cache [ outputType ];
//				}
//				
//			}
//			else if ( _direction == MapperDirections.MessageToData ) 
//			{
//				members = (MappableMember[])_cache [ inputObject.GetType() ];
//
//				if ( members == null ) 
//				{
//					AddTypeToCache ( inputObject.GetType() );
//					members = (MappableMember[])_cache [ inputObject.GetType() ];
//				}
//			}
//
//			return MapMembers ( members, inputObject, outputType );
//		}
//
//		private void AddMapResultToOutput( Hashtable membersResult , bool isCollection )
//		{
//			IEnumerator keys = membersResult.GetEnumerator();
//			while ( keys.MoveNext() )
//			{
//				DictionaryEntry current = (DictionaryEntry)keys.Current;
//				object output = current.Value ;
//
//				if ( isCollection )
//				{
//					//"Monto" o tipo do array
//					Type arrayType = ((object[])Array.CreateInstance ( (Type)current.Key, 0 )).GetType();
//
//					//Verifico se o array já existe
//					if ( _outputs.ContainsKey ( arrayType ) )
//					{
//						//Em caso afirmativo incremento
//						object[] array = (object[])_outputs [ arrayType ]; 
//						ArrayList temp = new ArrayList ( array );
//						temp.Add ( output );
//						_outputs [ arrayType ] = (object[])temp.ToArray( (Type)current.Key );
//					}
//					else
//					{
//						//Em caso negativo, crio
//						object[] array = (object[])Array.CreateInstance ( (Type)current.Key, 1 );
//						array [ 0 ] = output;
//
//						//Adiciono o array na lista de saidas
//						_outputs.Add ( array.GetType() , array );
//					}					
//				} 
//				else
//				{
//					if ( ! _outputs.ContainsKey (  current.Key ) )
//					{
//						//Adiciono o objeto na lista de saidas
//						_outputs.Add ( current.Key , output );
//					}
//				}				
//			}
//		}
//
//		private object Get ( Type type )
//		{
//			if ( null == _outputs || _outputs.Count == 0 ) return null;
//
//			if ( type.IsSubclassOf ( typeof ( MessageCollection ) ) )
//			{
//				CollectionOfAttribute collectionOf = (CollectionOfAttribute)Attributes.GetSingleAttribute ( type, typeof ( CollectionOfAttribute ) );
//				if ( collectionOf == null )
//				{
//					throw new ArgumentException ( string.Format ( "Atributo 'CollectionOf' não definido para a classe {0} ",type.Name ) );
//				}
//
//				MessageCollection collection = 
//					( MessageCollection ) Activator.CreateInstance ( type ); 
//
//				Type collectionType = collectionOf.CollectionType;
//
//				Type arrayType = Type.GetType ( string.Format ( "{0}[],{1}", collectionType.FullName, collectionType.Assembly.FullName));
//
//				System.Array array =  ( System.Array ) _outputs [ arrayType ];
//
//				if ( null != array )
//				{
//					foreach ( object item in array )
//					{
//						collection.Add ( ( Message ) item );
//					}
//				}
//
//				return collection;
//			} 
//			else
//			{
//				object o = _outputs [ type ];
//				return o ;
//			}
//		}
//
//		public void AddInput( object o )
//		{
//			throw new NotImplementedException();
//		}
//
//		public void AddOutput( Type type )
//		{
//			throw new NotImplementedException();
//		}
//
//		public object GetOutput( Type type )
//		{
//			throw new NotImplementedException();
//		}
//	}
//}

