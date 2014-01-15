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
	internal class QueryMethodBuilder : MethodBuilderBase 
	{
		private bool _useMapper = false ;

		public QueryMethodBuilder(CodeMemberMethod methodImpl, MethodInfo methodInfo) : base(methodImpl, methodInfo) {}

		protected override Type[] BuildMethod( ) {
			//Cria uma lista para os DataMessages referenciados
			ArrayList listOfUsedMessages = new ArrayList();
			//Cria uma lista para os fields
			ArrayList listOfFields = new ArrayList();
		
			//Monta a query
			string queryVarName = "queryBuilder";
			CodeStatement[] select = this.CreateSelect(queryVarName, _methodInfo, _returnType, _defaultMessageType, listOfUsedMessages, listOfFields);	

			if ( Attribute.IsDefined ( _methodInfo,  typeof(LinkAllAttribute) ) && Attribute.IsDefined ( _methodInfo, typeof(WhereAttribute) ) )
			{
				throw new AutoDataLayerBuildException(" Não é possivel usar o 'WhereAttribute' em conjunto com 'LinkAllAttribute'" );
			}

			//Obtem os customwhere do método			
			WhereAttribute[] arrayOFWhereFull = (WhereAttribute[])Attribute.GetCustomAttributes(_methodInfo, typeof(WhereAttribute), true);
			//Cria um hash para os parameters
			Hashtable hashOfParameters = new Hashtable();
			//Percorre todos os parâmetros para montar a hash e fazer as verificações necessárias
			foreach(ParameterInfo parameter in _methodInfo.GetParameters()) {
				//Adiciona o parameter na hash que será usada para criação dos SqlParameters

				//adiciona na lista de parametros apenas se não for um field usado em uma mensagem filha bindable
				if ( ! Attribute.IsDefined ( parameter, typeof(SuperPag.Framework.Data.Mapping.UsedForBind )) )
				{
					hashOfParameters.Add(parameter.Name, new Parameter(parameter.ParameterType, parameter.Name));
				}
			}
			
			string sqlParametersVarName = null;
			CodeStatement[] where = Common.CreateWhere(queryVarName, _methodInfo, _defaultMessageType, listOfUsedMessages, arrayOFWhereFull, hashOfParameters, out sqlParametersVarName, true, null);
			//CodeStatement[] parameters = Common.CreateWhereParameters("sqlParameters", listOfValidWhere, null);
			CodeStatement[] group = this.CreateGroupBy(queryVarName, _methodInfo, _defaultMessageType, listOfUsedMessages);
			CodeStatement[] order = this.CreateOrderBy(queryVarName, _methodInfo, _defaultMessageType, listOfUsedMessages);

			//Monta o from
			CodeStatement[] from = Common.CreateFrom(queryVarName, _methodInfo, _defaultMessageType, listOfUsedMessages);
 
			//Monta a declaração da query
			CodeVariableDeclarationStatement queryBuilderSta = new CodeVariableDeclarationStatement("StringBuilder", queryVarName);
			queryBuilderSta.InitExpression = new CodeObjectCreateExpression("StringBuilder");
			_methodImpl.Statements.Add(queryBuilderSta);
			_methodImpl.Statements.AddRange(select);
			_methodImpl.Statements.AddRange(from);
			if (where != null && where.Length > 0) _methodImpl.Statements.AddRange(where);
			if (group != null && group.Length > 0) _methodImpl.Statements.AddRange(group);
			if (order != null && order.Length > 0) _methodImpl.Statements.AddRange(order);

			//Gera a linha de execução (ExecuteReader)
			_methodImpl.Statements.Add(new CodeCommentStatement("Execução do comando"));
			CodeVariableDeclarationStatement objsetVar = new CodeVariableDeclarationStatement(typeof(object[]), "objset");
			objsetVar.InitExpression = new CodeMethodInvokeExpression(
				new CodeBaseReferenceExpression() , "ExecuteReader", //Método a ser invocado
				new CodeSnippetExpression("DefaultConnectionString"), //ConnectionString
				new CodeSnippetExpression("System.Data.CommandType.Text"), //CommandType
				new CodeVariableReferenceExpression(string.Format("{0}.ToString()", queryVarName)), //CommandText
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
			if (!Helpers.CheckIsValidMessage(_returnType)) {
				//Monta o mapeamento de acordo com o tipo (int, bool, string, dateTime)
				//se o tipo não for válido, o método do helper dispara uma exception
				string typeName = _methodInfo.ReturnType.Name;
				resultExp = new CodeSnippetExpression(string.Format("((Hashtable)objset[0])[\"result\"] != DBNull.Value ? ({0})((Hashtable)objset[0])[\"result\"] : {1}", typeName, Helpers.GetNullValue(_methodInfo.ReturnType)));

				_methodImpl.Statements.Add(new CodeMethodReturnStatement(resultExp));
			} else {
				CodeVariableDeclarationStatement returnDecl = new CodeVariableDeclarationStatement(string.Format("{0}[]", _returnType.FullName), "result", new CodeArrayCreateExpression(_returnType, new CodeVariableReferenceExpression("objset.Length")));
				CodeIterationStatement returnSet = new CodeIterationStatement();
				returnSet.InitStatement = new CodeExpressionStatement(new CodeSnippetExpression("int i = 0, l = objset.Length"));
				returnSet.IncrementStatement = new CodeExpressionStatement(new CodeSnippetExpression("i++"));
				returnSet.TestExpression = new CodeSnippetExpression("i < l");

				returnSet.Statements.Add (new CodeExpressionStatement(new CodeSnippetExpression("Hashtable row = (Hashtable)objset[i]")));
				returnSet.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("result[i]"), new CodeObjectCreateExpression(_returnType)));

				string lastAlias = null;
				for (int i = 0, l = listOfFields.Count; i < l; i++) {
					ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfFields[i];
                    string rowRef = string.Format("row[\"{0}\"]", resolvedField.FieldAliasPrefixToDb + resolvedField.FieldInfo.Name);

                    //Verifica se precisa inicializar alguma mensagem interna
					if (resolvedField.FieldAliasPrefix != lastAlias) {
						lastAlias = resolvedField.FieldAliasPrefix;
						if (lastAlias != null) {
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

				if (_isReturnArray) {
					_methodImpl.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("result")));
				} else {
					_methodImpl.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("result[0]")));
				}
			}

			//Retorna a lista de tipos referenciados
			return (Type[])listOfUsedMessages.ToArray(typeof(Type));
		}

		public void ResolveFields(string aliasPrefix, Type messageType, Type defaultMessageType, ArrayList listOfUsedMessages, ArrayList listOfFields) {
			foreach(FieldInfo field in messageType.GetFields()) {
				if ( ! field.IsLiteral )
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
				return otherParameters;
			else 
				return otherParameters;
		}

		#region CreateSelect
		public CodeStatement[] CreateSelect(string queryVarName, MethodInfo methodInfo, Type returnType, Type defaultMessageType, ArrayList listOfUsedMessages, ArrayList listOfFields) {
			bool simpleResult = false;

			//Verifica se o retorno é uma mensagem
			if (Helpers.CheckIsValidMessage(returnType)) {
				//Resolve os fields da DataMessage
				ResolveFields(null, returnType, defaultMessageType, listOfUsedMessages, listOfFields);
				
			} else { //Retorno é um value type ou Tristate
				simpleResult = true;
				//Verifica se encontra o field Attribute
				//Caso não encontre, dispara uma exception
				if (methodInfo.ReturnTypeCustomAttributes.IsDefined(typeof(ReturnFieldAttribute), true)) {
					ReturnFieldAttribute returnFieldAtt = (ReturnFieldAttribute)methodInfo.ReturnTypeCustomAttributes.GetCustomAttributes(typeof(ReturnFieldAttribute), true)[0];
					ResolvedFieldInfo resolvedField = returnFieldAtt._fieldInfo.GetResolvedFieldInfo(defaultMessageType);

					//Verifica se a message é diferente
					if (resolvedField.TargetMessageType != defaultMessageType) listOfUsedMessages.Add(resolvedField.TargetMessageType);

					//TODO: Verifica se encontrou..
					listOfFields.Add(resolvedField);
				} else {
					throw new AutoDataLayerBuildException("Métodos que não retornam DataMessages devem especificar o atributo [ReturnField].");
				}
			}

			//Monta a string do select
			ArrayList listOfCodeSta = new ArrayList();
			listOfCodeSta.Add(CodeHelper.MethodInvoke(queryVarName, "Append", "\" SELECT\""));
			for (int i = 0, l = listOfFields.Count; i < l; i++) {
				ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfFields[i];
				string dataFieldName = resolvedField.DataFieldName;

				//Verifica se o field possui agregação
				AggregationAttribute aggregationAtt = Helpers.CheckForAggregation(resolvedField, defaultMessageType, methodInfo);
				if (aggregationAtt != null) {
					dataFieldName = Helpers.ResolveAggregation(aggregationAtt._aggregationType, dataFieldName);
				}

#if SQL
                string fieldAlias = !simpleResult ? resolvedField.FieldAliasPrefixToDb + resolvedField.FieldInfo.Name : "result";
#elif ORACLE
                string fieldAlias = !simpleResult ? resolvedField.FieldAliasPrefixToDb + "\\\"" + resolvedField.FieldInfo.Name + "\\\"" : "result";
#else
                string fieldAlias = !simpleResult ? resolvedField.FieldAliasPrefixToDb + resolvedField.FieldInfo.Name : "result";
#endif
                string text = string.Format("\" {0} AS {1}{2}\"", dataFieldName, fieldAlias, i + 1 < l ? "," : null);
				
				listOfCodeSta.Add(CodeHelper.MethodInvoke(queryVarName, "Append", text));
			}

			return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
		}
		#endregion

		#region CreateOrderBy
		public CodeStatement[] CreateOrderBy(string queryVarName, MethodInfo methodInfo, Type defaultMessageType, ArrayList listOfUsedMessages) {
			OrderByAttribute[] arrayOfOrderBy = (OrderByAttribute[])Attribute.GetCustomAttributes(methodInfo, typeof(OrderByAttribute), true);
			//Verifica se encontrou algum atributo OrderBy
			if (arrayOfOrderBy == null || arrayOfOrderBy.Length == 0) return null;

			//Verifica se o order está correto
			if (!Helpers.SortAndCheckOrder(arrayOfOrderBy)) {
				throw new ApplicationException("O parâmetro ORDER do atributo [OrderBy] deve ser sequencial e começar em 0, método: " + methodInfo.DeclaringType.FullName + "." + methodInfo.Name);
			}

			ArrayList listOfCodeSta = new ArrayList();
			listOfCodeSta.Add(CodeHelper.MethodInvoke(queryVarName, "Append", "\" ORDER BY\""));
			//Percorre todos os atributos
			for (int i = 0, l = arrayOfOrderBy.Length; i < l; i++) {
				ResolvedFieldInfo resolvedField = arrayOfOrderBy[i]._fieldInfo.GetResolvedFieldInfo(defaultMessageType);

				//Verifica se a message é diferente
				if (resolvedField.TargetMessageType != defaultMessageType) listOfUsedMessages.Add(resolvedField.TargetMessageType);

				//TODO: verifica se encontrou...

				//Monta o orderBy ordenando pelo alias
				string order = string.Format("\" {0} {1}{2}\"", resolvedField.DataFieldName, Helpers.ResolveSortOrderEnum(arrayOfOrderBy[i]._sortOrder), i < l - 1 ? "," : null);
				listOfCodeSta.Add(CodeHelper.MethodInvoke(queryVarName, "Append", order));
			}

			return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
		}
		#endregion

		#region CreateGroupBy
		public CodeStatement[] CreateGroupBy(string queryVarName, MethodInfo methodInfo, Type defaultMessageType, ArrayList listOfUsedMessages) {
			GroupByAttribute[] arrayOfGroupBy = (GroupByAttribute[])Attribute.GetCustomAttributes(methodInfo, typeof(GroupByAttribute), true);
			//Verifica se encontrou algum atributo GroupBy
			if (arrayOfGroupBy == null || arrayOfGroupBy.Length == 0) return null;

			//Verifica se o order está correto
			if (!Helpers.SortAndCheckOrder(arrayOfGroupBy)) {
				throw new ApplicationException("O parâmetro ORDER do atributo [GroupBy] deve ser sequencial e começar em 0, método: " + methodInfo.DeclaringType.FullName + "." + methodInfo.Name);
			}

			ArrayList listOfCodeSta = new ArrayList();
			listOfCodeSta.Add(CodeHelper.MethodInvoke(queryVarName, "Append", "\" GROUP BY\""));
			//Percorre todos os atributos
			for (int i = 0, l = arrayOfGroupBy.Length; i < l; i++) {
				ResolvedFieldInfo resolvedField = arrayOfGroupBy[i]._fieldInfo.GetResolvedFieldInfo(defaultMessageType);

				//Verifica se a message é diferente
				if (resolvedField.TargetMessageType != defaultMessageType) listOfUsedMessages.Add(resolvedField.TargetMessageType);

				//TODO: verifica se encontrou...

				//Monta o groupBy com o nome do dataField
				string group = string.Format("\" {0}{1}\"", resolvedField.DataFieldName, i < l - 1 ? "," : null);
				listOfCodeSta.Add(CodeHelper.MethodInvoke(queryVarName, "Append", group));
			}

			return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
		}
		#endregion

	}
}
