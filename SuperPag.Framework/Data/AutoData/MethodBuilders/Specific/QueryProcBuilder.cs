using System;
using System.Reflection;
using System.CodeDom;
using System.Collections;
using System.Text;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Helper;
using DMAtt = SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders 
{
	internal class QueryProcMethodBuilder : MethodBuilderBase 
	{
		private bool _useMapper = false ;

		public QueryProcMethodBuilder(CodeMemberMethod methodImpl, MethodInfo methodInfo) : base(methodImpl, methodInfo) {}

		protected override Type[] BuildMethod( ) 
		{
			//Cria uma lista para os DataMessages referenciados
			ArrayList listOfUsedMessages = new ArrayList();
			//Cria uma lista para os fields
			ArrayList listOfFields = new ArrayList();
		
			if ( Attribute.IsDefined ( _methodInfo,  typeof(LinkAllAttribute) ) && Attribute.IsDefined ( _methodInfo, typeof(WhereAttribute) ) )
			{
				throw new AutoDataLayerBuildException(" Não é possivel usar o 'WhereAttribute' em conjunto com 'LinkAllAttribute'" );
			}

			//Cria um hash para os parameters
			Hashtable hashOfParameters = new Hashtable();
			//Percorre todos os parâmetros para montar a hash e fazer as verificações necessárias
			foreach(ParameterInfo parameter in _methodInfo.GetParameters()) 
			{
				//Adiciona o parameter na hash que será usada para criação dosSqlParameters

				//adiciona na lista de parametros apenas se não for um field usado em uma mensagem filha bindable
				if ( ! Attribute.IsDefined ( parameter, typeof(SuperPag.Framework.Data.Mapping.UsedForBind )) )
				{
					hashOfParameters.Add(parameter.Name, new Parameter(parameter.ParameterType, parameter.Name));
				}
			}
			
			string sqlParametersVarName = null;
			CodeStatement[] where = Common.CreateParam(_methodInfo, _defaultMessageType, listOfUsedMessages, hashOfParameters, out sqlParametersVarName, true, null);

			if (where != null && where.Length > 0) _methodImpl.Statements.AddRange(where);

            //Obtem o methodInfo do custom méthod
			MethodTypeAttribute methodType = (MethodTypeAttribute)Attribute.GetCustomAttribute(_methodInfo, typeof(MethodTypeAttribute), true);
						
			//Gera a linha de execução (ExecuteReader)
			_methodImpl.Statements.Add(new CodeCommentStatement("Execução do comando"));
			CodeVariableDeclarationStatement objsetVar = new CodeVariableDeclarationStatement(typeof(object[]), "objset");
            objsetVar.InitExpression = new CodeMethodInvokeExpression(
				new CodeBaseReferenceExpression() , "ExecuteReader", //Método a ser invocado
                new CodeSnippetExpression("DefaultConnectionString"), //ConnectionString
				new CodeSnippetExpression("System.Data.CommandType.StoredProcedure"), //CommandType
#if SQL
				new CodeVariableReferenceExpression(string.Format("\"{0}\"", methodType._procName )), //CommandText
#elif ORACLE
                new CodeVariableReferenceExpression(string.Format("\"\\\"{0}\\\"\"", methodType._procName)), //CommandText
#else
				new CodeVariableReferenceExpression(string.Format("\"{0}\"", methodType._procName )), //CommandText
#endif
                new CodeSnippetExpression(sqlParametersVarName)//Parameters
				);			
			_methodImpl.Statements.Add(objsetVar);

            //Gera a linha que verifica o resultado
			_methodImpl.Statements.Add(new CodeCommentStatement("Verifica se obteve algum resultado"));
			CodeStatement objsetsetIfSta = new CodeConditionStatement(
				new CodeVariableReferenceExpression("objset.Length == 0"), //Condição
				new CodeMethodReturnStatement(new CodeSnippetExpression(Helpers.GetNullValue(_methodInfo.ReturnType)))); //Retorno se verdadeiro
			_methodImpl.Statements.Add(objsetsetIfSta);

			//Gera o mapper
			CodeExpression resultExp = null;
			//Verifica se o retorno é uma mensagem
			if (!Helpers.CheckIsValidMessage(_returnType)) 
			{
				//Monta o mapeamento de acordo com o tipo (int, bool, string, dateTime)
				//se o tipo não for válido, o método do helper dispara uma exception
				string typeName = _methodInfo.ReturnType.Name;
				resultExp = new CodeSnippetExpression(string.Format("((Hashtable)objset[0])[\"result\"] != DBNull.Value ? ({0})((Hashtable)objset[0])[\"result\"] : {1}", typeName, Helpers.GetNullValue(_methodInfo.ReturnType)));

				_methodImpl.Statements.Add(new CodeMethodReturnStatement(resultExp));
			} 
			else 
			{
				CodeVariableDeclarationStatement returnDecl = new CodeVariableDeclarationStatement(string.Format("{0}[]", _returnType.FullName), "result", new CodeArrayCreateExpression(_returnType, new CodeVariableReferenceExpression("objset.Length")));
				CodeIterationStatement returnSet = new CodeIterationStatement();
				returnSet.InitStatement = new CodeExpressionStatement(new CodeSnippetExpression("int i = 0, l = objset.Length"));
				returnSet.IncrementStatement = new CodeExpressionStatement(new CodeSnippetExpression("i++"));
				returnSet.TestExpression = new CodeSnippetExpression("i < l");

				returnSet.Statements.Add (new CodeExpressionStatement(new CodeSnippetExpression("Hashtable row = (Hashtable)objset[i]")));
				returnSet.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("result[i]"), new CodeObjectCreateExpression(_returnType)));

				string lastAlias = null;

                ResolveFields(null, _returnType, _defaultMessageType, listOfUsedMessages, listOfFields);

				for (int i = 0, l = listOfFields.Count; i < l; i++) 
				{
					ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfFields[i];
					string rowRef = string.Format("row[\"{0}\"]", resolvedField.FieldAliasPrefixToDb + resolvedField.FieldInfo.Name);

					//Verifica se precisa inicializar alguma mensagem interna
					if (resolvedField.FieldAliasPrefix != lastAlias) 
					{
						lastAlias = resolvedField.FieldAliasPrefix;
						if (lastAlias != null) 
						{
							returnSet.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("result[i]." + lastAlias.Substring(0, lastAlias.Length - 1)), new CodeObjectCreateExpression(resolvedField.FieldInfo.DeclaringType)));
						}
					}

					returnSet.Statements.Add(new CodeAssignStatement(
						new CodeVariableReferenceExpression(string.Format("result[i].{0}", lastAlias + resolvedField.FieldInfo.Name)),
						new CodeSnippetExpression(string.Format("{0} != DBNull.Value ? {1} : {2}", rowRef, Helpers.GetConvertTo(resolvedField.FieldInfo.FieldType, rowRef), Helpers.GetNullValue(resolvedField.FieldInfo.FieldType)))
						));
				}

				_methodImpl.Statements.Add(returnDecl);
				_methodImpl.Statements.Add(returnSet);
				
				if ( _useMapper )
				{	
					ArrayList hashOfOtherParameters = this.GetOtherParameters( _methodInfo );

					if ( hashOfOtherParameters == null && hashOfOtherParameters.Count > 0 )
					{
						_methodImpl.Statements.Add( new CodeSnippetStatement (  "Hashtable otherParameters = null;"  ) );
					}
					else
					{
						_methodImpl.Statements.Add( new CodeSnippetStatement (  "Hashtable otherParameters = new Hashtable();"  ) );
						foreach ( string key in hashOfOtherParameters )
						{
							_methodImpl.Statements.Add( new CodeSnippetStatement ( "otherParameters.Add(\"" + key + "\"," + key + ");" ) );
						}
					}

					_methodImpl.Statements.Add( new CodeSnippetStatement (  "DataMapper mapper = new DataMapper( result, otherParameters, DefaultConnectionString );"  ) );
					_methodImpl.Statements.Add( new CodeSnippetStatement ( "mapper.Do();" ) );
				}

				if (_isReturnArray) 
				{
					_methodImpl.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("result")));
				} 
				else 
				{
					_methodImpl.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("result[0]")));
				}
			}

			//Retorna a lista de tipos referenciados
			return (Type[])listOfUsedMessages.ToArray(typeof(Type));
		}

		public void ResolveFields(string aliasPrefix, Type messageType, Type defaultMessageType, ArrayList listOfUsedMessages, ArrayList listOfFields) 
		{
			foreach(FieldInfo field in messageType.GetFields()) 
			{
				//Verifica o tipo do campo
				if (! field.IsLiteral && Helpers.CheckISValueType(field.FieldType)) 
				{ //ValueType ou Tristate
					//Verifica se o campo não deve ser incluido da consulta
					//caso possua o atributo, não o coloca na lista de select					
					if (!Attribute.IsDefined(field, typeof(DMAtt.ExcludeFromQueryAttribute))) 
					{
						ResolvedFieldInfo resolvedField = new DataFieldInfo(messageType, field.Name).GetResolvedFieldInfo(messageType);
							
						//Verifica se a message é diferente
						if (resolvedField.TargetMessageType != defaultMessageType) listOfUsedMessages.Add(resolvedField.TargetMessageType);

						//Verifica se temos algum prefixo no alias
						resolvedField.FieldAliasPrefix = aliasPrefix;

						//Adiciona o field
						listOfFields.Add(resolvedField);
					}					
				} 
				else 
				{ 
					//Verifica se não é um array
					if (!field.FieldType.IsArray) 
					{
						//Verifica se é uma DataMessage válida
						if (Helpers.CheckIsValidMessage(field.FieldType)) 
						{
							//Verifica se não existe recursividade
							string fieldToAppend = field.Name + ".";
							if (aliasPrefix != null && aliasPrefix.IndexOf(fieldToAppend) != -1) 
							{
								throw new AutoDataLayerBuildException(string.Format("Foi detectado mensagens com recursividade. DataMessage: {0}", messageType.FullName));
							} 
							else 
							{
								ResolveFields(aliasPrefix + fieldToAppend, field.FieldType, defaultMessageType, listOfUsedMessages, listOfFields);
							}
						} 
						else 
						{
							throw new AutoDataLayerBuildException(string.Format("Field {0} data DataMessage {1} deve derivar da DefaultDataMessage", field.Name, messageType.FullName));
						}
					} 
					else 
					{

						object bindable = Attributes.GetSingleAttribute ( field, typeof(SuperPag.Framework.Data.Mapping.Bindable ));

						_useMapper  = true;

						if ( bindable == null )
						{
							throw new AutoDataLayerBuildException("DataMessages só podem possuir arrays de DataMessages se o attributo Bindable for utilizado");
						}
					}
				}
			}
		}

		private ArrayList GetOtherParameters( MethodInfo methodInfo )
		{
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if ( parameters == null ) return null;
			
			ArrayList otherParameters = new ArrayList();
			foreach( ParameterInfo p in parameters )
			{
				if ( Attribute.IsDefined ( p, typeof( SuperPag.Framework.Data.Mapping.UsedForBind ) ) )
				{
					otherParameters.Add ( p.Name );
				}
			}

			if ( otherParameters.Count == 0 ) 
				return null;
			else 
				return otherParameters;
		}

		

	}
}
