using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.CodeDom;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using SuperPag.Framework.Helper;
using DMAtt = SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders {
	internal class DataLink {
		public Type leftType = null;
		public Type RightType = null;

		public DataLink (Type leftType, Type RightType) {
			this.leftType = leftType;
			this.RightType = RightType;
		}
	}

	internal class Common {

		#region CreateParamList
		public static CodeStatement[] CreateParam(MethodInfo methodInfo, Type defaultMessageType, ArrayList listOfMessageRelations, Hashtable hashOfParameters, out string parametersVarName, bool ignoreMinValues, string fieldPrefix) 
		{
			ArrayList listOfValidWhere = new ArrayList();

			if ( hashOfParameters != null && hashOfParameters.Count > 0 )
			{
				ArrayList listOfCodeSta = new ArrayList();
				listOfCodeSta.Add( new CodeVariableDeclarationStatement ( "ArrayList", "lstParams" , new CodeObjectCreateExpression ( "ArrayList" ) ) );

				//apenda os parametros
				foreach (Parameter parameter in hashOfParameters.Values) 
				{
#if SQL
                    CodeStatement addPar = CodeHelper.MethodInvoke("lstParams", "Add", string.Format("new System.Data.SqlClient.SqlParameter(\"@{0}\",{0})", parameter._name));
#elif ORACLE
                    CodeStatement addPar = CodeHelper.MethodInvoke("lstParams", "Add", string.Format("new System.Data.OracleClient.OracleParameter(\"{0}\",{0})", parameter._name));
#else
                    CodeStatement addPar = CodeHelper.MethodInvoke("lstParams", "Add", string.Format("new System.Data.SqlClient.SqlParameter(\"@{0}\",{0})", parameter._name));
#endif
                    listOfCodeSta.Add(addPar);
                }

#if SQL
                listOfCodeSta.Add(new CodeExpressionStatement(new CodeSnippetExpression("System.Data.SqlClient.SqlParameter[] lstSqlParams = (System.Data.SqlClient.SqlParameter[])lstParams.ToArray(typeof ( System.Data.SqlClient.SqlParameter ))")));				
#elif ORACLE
                listOfCodeSta.Add(new CodeExpressionStatement(new CodeSnippetExpression("System.Data.OracleClient.OracleParameter[] lstSqlParams = (System.Data.OracleClient.OracleParameter[])lstParams.ToArray(typeof ( System.Data.OracleClient.OracleParameter ))")));
#else
                listOfCodeSta.Add(new CodeExpressionStatement(new CodeSnippetExpression("System.Data.SqlClient.SqlParameter[] lstSqlParams = (System.Data.SqlClient.SqlParameter[])lstParams.ToArray(typeof ( System.Data.SqlClient.SqlParameter ))")));				
#endif

				parametersVarName = "lstSqlParams";
				return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
			}
			else 
			{
				parametersVarName = null;
				return null;
			}
		}
		#endregion

		#region CreateWhere
		public static CodeStatement[] CreateWhere(string queryBuilderVarName, MethodInfo methodInfo, Type defaultMessageType, ArrayList listOfMessageRelations, WhereAttribute[] arrayOFWhereFull, Hashtable hashOfParameters, out string parametersVarName, bool ignoreMinValues, string fieldPrefix) 
		{
			ArrayList listOfValidWhere = new ArrayList();
			//Verifica se existe algum where para implementar
			if ((arrayOFWhereFull == null || arrayOFWhereFull.Length == 0) && (methodInfo.GetParameters() == null || hashOfParameters.Count == 0)) {
				parametersVarName = "null";
				return null;
			}

			//Verifica se foi especificado um where personalizado
			if (arrayOFWhereFull != null && arrayOFWhereFull.Length > 0) {
				//Verifica se o order está correto dos whereAttributes
				if (!Helpers.SortAndCheckOrder(arrayOFWhereFull)) {
					throw new ApplicationException("O parâmetro ORDER do atributo [Where] deve ser sequencial e começar em 0, método: " + methodInfo.DeclaringType.FullName + "." + methodInfo.Name);
				}

				listOfValidWhere.AddRange(arrayOFWhereFull);
			} else {
				//Faz o where simples, percorrendo todos os parameters
				foreach (Parameter parameter in hashOfParameters.Values) {
					if ( Attribute.IsDefined ( methodInfo, typeof( LinkAllAttribute ) ) )
					{						
						Link link = ((LinkAllAttribute)Attributes.GetSingleAttribute ( methodInfo,  typeof( LinkAllAttribute ) ) ).Link;

						if (parameter._type.IsArray) listOfValidWhere.Add(new WhereAttribute(0, Block.Begin, link));
						listOfValidWhere.Add(new WhereAttribute(0, parameter._name, defaultMessageType, parameter._name, Filter.Equal, link ));
						if (parameter._type.IsArray) listOfValidWhere.Add(new WhereAttribute(0, Block.End, link));
					} 
					else
					{
						if (parameter._type.IsArray) listOfValidWhere.Add(new WhereAttribute(0, Block.Begin, Link.And));
						listOfValidWhere.Add(new WhereAttribute(0, parameter._name, defaultMessageType, parameter._name, Filter.Equal, !parameter._type.IsArray ? Link.And : Link.Or));
						if (parameter._type.IsArray) listOfValidWhere.Add(new WhereAttribute(0, Block.End, Link.And));
					}
					
	

				}
			}

			//Verifica se deve gerar o where
			if (listOfValidWhere.Count > 0 ) {
				ArrayList listOfCodeSta = new ArrayList();
				listOfCodeSta.Add(new CodeVariableDeclarationStatement("Where", "where", new CodeObjectCreateExpression("Where")));

				for (int i = 0; i < listOfValidWhere.Count; i++) {
					WhereAttribute whereAttribute = (WhereAttribute)listOfValidWhere[i];

					bool isNullParameter = whereAttribute._parameterName == null || whereAttribute._parameterName.Length == 0;

					if (!whereAttribute.isBlock) {

						//Obtem o parameter, caso não exista, e o filter não seja isnull, ou notisnull dispara um erro
						Parameter parameter = null;
						if (!isNullParameter) {
							parameter = (Parameter)hashOfParameters[whereAttribute._parameterName];
							if (null == parameter) {
								throw new ApplicationException("Não foi encontrado o parâmetro \"" + whereAttribute._parameterName + "\" para o where customizado, método: " + methodInfo.DeclaringType.FullName + "." + methodInfo.Name);
							}
							whereAttribute._typeOfParameter = parameter._type;
						} else {
							//Verifica se o filterType não é isNull ou NotIsNull
							if (whereAttribute._Filter != Filter.IsNull && whereAttribute._Filter != Filter.NotIsNull) {
								throw new ApplicationException("Não foi especificado o parâmetro para o where customizado, método: " + methodInfo.DeclaringType.FullName + "." + methodInfo.Name);
							}
						}
					}

					object[] parameters = null;
					bool isArray = !isNullParameter && whereAttribute._typeOfParameter.IsArray;
					string parameterName = null;
					if (whereAttribute.isBlock) {
						parameters = new object[] {
													  string.Format("Block.{0}", whereAttribute._Block.ToString()),
													  string.Format("Link.{0}", whereAttribute._Link.ToString())
												  };
					} else {
						if (isArray) {
							parameterName = string.Format("{0}[i]", whereAttribute._parameterName);
						} else {
							parameterName = whereAttribute._parameterName;
						}

						ResolvedFieldInfo resolvedField = whereAttribute._fieldInfo.GetResolvedFieldInfo(defaultMessageType);
						parameters = new object[] {
														string.Format("\"{0}\"", resolvedField.DataFieldName),
														string.Format("Filter.{0}", whereAttribute._Filter.ToString()),
														isNullParameter ? "\"\"" : fieldPrefix + parameterName,
														string.Format("Link.{0}", whereAttribute._Link.ToString())										
													};
					}
					
					CodeStatement whereAppend = CodeHelper.MethodInvoke("where", "Append", parameters);

					//Verifica se deve ignorar minValues
					CodeStatement whereSta = null;
					if (!isNullParameter && ignoreMinValues) {
						CodeConditionStatement whereIf = new CodeConditionStatement();
						if (isArray) {
							whereIf.Condition = new CodeSnippetExpression(string.Format("{0}[i] != {1}", whereAttribute._parameterName, Helpers.GetNullValue(whereAttribute._typeOfParameter.GetElementType())));
						} else {
							if ( whereAttribute._typeOfParameter != typeof( bool ) )
							{
								whereIf.Condition = new CodeSnippetExpression(string.Format("{0} != {1}", whereAttribute._parameterName, Helpers.GetNullValue(whereAttribute._typeOfParameter)));
							} 
							else 
							{
								whereIf.Condition = new CodeSnippetExpression("true");
							}
						}
						whereIf.TrueStatements.Add(whereAppend);
						whereSta = whereIf;
					} else {
						whereSta = whereAppend;
					}

					if (isArray) {
						CodeIterationStatement forSta = new CodeIterationStatement();
						forSta.InitStatement = new CodeExpressionStatement(new CodeSnippetExpression("int i = 0"));
						forSta.TestExpression = new CodeSnippetExpression(string.Format("i < {0}.Length", whereAttribute._parameterName));
						forSta.IncrementStatement = new CodeExpressionStatement(new CodeSnippetExpression("i++"));
						forSta.Statements.Add(whereSta);

						CodeConditionStatement arrayIf = new CodeConditionStatement();
						arrayIf.Condition = new CodeSnippetExpression(string.Format("{0} != null && {0}.Length > 0", whereAttribute._parameterName));
						arrayIf.TrueStatements.Add(forSta);

						listOfCodeSta.Add(arrayIf);
					} else {
						listOfCodeSta.Add(whereSta);
					}
				}

				listOfCodeSta.Add(CodeHelper.MethodInvoke(queryBuilderVarName, "Append", "where.ToString()"));

				parametersVarName = "where.GetParameters()";
				return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));;
			} else {
				parametersVarName = null;
				return null;
			}
		}
		#endregion

		#region CreateFrom

		public static CodeStatement[] CreateFrom(string queryBuilderVarName, MethodInfo methodInfo, Type defaultMessageType, ArrayList listOfUsedMessagesToCheck) {
			//Cria uma listra para as DataRelations que serão utilizadas
			Hashtable hashOfDataRelationsToUse = new Hashtable();

			//Retira as messages duplicadas
			ArrayList _listOfUsedMessages = new ArrayList();
			for (int i = 0; i < listOfUsedMessagesToCheck.Count; i++) {
				if (!_listOfUsedMessages.Contains(listOfUsedMessagesToCheck[i])) {
					_listOfUsedMessages.Add(listOfUsedMessagesToCheck[i]);
				}
			}

			//Obtem uma lista dos atributos DataRelations do método e da DefaultDataMessage
			DataRelationAttribute[] methodRelations = (DataRelationAttribute[])Attribute.GetCustomAttributes(methodInfo, typeof(DataRelationAttribute), true);
			DataRelationAttribute[] defaultMessageRelations = (DataRelationAttribute[])Attribute.GetCustomAttributes(defaultMessageType, typeof(DataRelationAttribute), true);

			//Monta a lista de DataRelations que foram solicitadas
			for (int i = 0; i < methodRelations.Length; i++) {
				methodRelations[i].ResolveFields(defaultMessageType);
				Helpers.UpdateHashUniques(hashOfDataRelationsToUse, methodRelations[i]);
			}
			for (int i = 0; i < defaultMessageRelations.Length; i++) {
				defaultMessageRelations[i].ResolveFields(defaultMessageType);
				Helpers.UpdateHashUniques(hashOfDataRelationsToUse, defaultMessageRelations[i]);
			}

			//Obtem os DataRelations das DataMessages utilizadas
			for (int i = 0; i < _listOfUsedMessages.Count; i++) {
				DataRelationAttribute[] messageRelations = (DataRelationAttribute[])Attribute.GetCustomAttributes((Type)_listOfUsedMessages[i], typeof(DataRelationAttribute), false);
				
				for (int j = 0; j < messageRelations.Length; j++) {
					messageRelations[j].ResolveFields((Type)_listOfUsedMessages[j]);
					Helpers.UpdateHashUniques(hashOfDataRelationsToUse, messageRelations[j]);
				}
			}

			//TODO: melhorar lógica (não está muito correta)
			//Verifica se todos os dataMessages utilizados foram relacionados
			for (int i = 0; i < _listOfUsedMessages.Count; i++) {
				bool finded = false;
				Type typeToFind = (Type)_listOfUsedMessages[i];

				foreach(DataRelationAttribute dataRelation in hashOfDataRelationsToUse.Values) {
					if (dataRelation._leftResolvedField.TargetMessageType == typeToFind
						|| dataRelation._rightResolvedField.TargetMessageType == typeToFind
						|| dataRelation._leftResolvedField.TargetMessageType.IsSubclassOf(typeToFind)
						|| dataRelation._rightResolvedField.TargetMessageType.IsSubclassOf(typeToFind)
						) {
						finded = true;
						break;
					}
				}

				if (!finded) {
					throw new ApplicationException("Não foi possível determinar o join com a DataMessage: " + typeToFind.FullName);
				}
			}

			ArrayList listOfCodeSta = new ArrayList();
			//Adiciona a baseMessage
#if SQL
            listOfCodeSta.Add(CodeHelper.MethodInvoke(queryBuilderVarName, "Append", string.Format("\" FROM [{0}]\"", Helpers.GetDataTableName(defaultMessageType))));
#elif ORACLE
            listOfCodeSta.Add(CodeHelper.MethodInvoke(queryBuilderVarName, "Append", string.Format("\" FROM \\\"{0}\\\"\"", Helpers.GetDataTableName(defaultMessageType))));
#else
            listOfCodeSta.Add(CodeHelper.MethodInvoke(queryBuilderVarName, "Append", string.Format("\" FROM [{0}]\"", Helpers.GetDataTableName(defaultMessageType))));
#endif
            GenerateRelations(defaultMessageType, hashOfDataRelationsToUse, listOfCodeSta, defaultMessageType, queryBuilderVarName, listOfUsedMessagesToCheck);

			return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
		}

		public static void GenerateRelations(Type typeToFind, Hashtable hashOfDataRelationsToUse, ArrayList listOfCodeSta, Type defaultMessageType, string queryBuilderVarName, ArrayList listOfUsedMessages) {
			foreach (DataRelationAttribute dataRelation in hashOfDataRelationsToUse.Values) {
				//for (int i = hashOfDataRelationsToUse.Count - 1; i >= 0; i--) {
				//DataRelationAttribute dataRelation = (DataRelationAttribute)hashOfDataRelationsToUse[i];

				if (typeToFind == dataRelation._leftResolvedField.TargetMessageType || typeToFind.IsSubclassOf(dataRelation._leftResolvedField.TargetMessageType)) {
					//hashOfDataRelationsToUse.Remove(dataRelation.GetHashCode());

					ResolvedFieldInfo resolvedLeftField = dataRelation._leftFieldInfo.GetResolvedFieldInfo(defaultMessageType);
					ResolvedFieldInfo resolvedRightField = dataRelation._rightFieldInfo.GetResolvedFieldInfo(defaultMessageType);
	
					//TODO: alterar - montar uma coleção especializada
					//Adiciona na lista de messages utilizadas
					if (!listOfUsedMessages.Contains(resolvedLeftField.TargetMessageType)) {
						listOfUsedMessages.Add(resolvedLeftField.TargetMessageType);
					}
					if (!listOfUsedMessages.Contains(resolvedRightField.TargetMessageType)) {
						listOfUsedMessages.Add(resolvedRightField.TargetMessageType);
					}

					//Monta a linha do join
					string from = string.Format(" {0} JOIN {1}", Helpers.ResolveJoinType(dataRelation._join), resolvedRightField.DataTableName);
					from += string.Format(" ON {0} = {1}", resolvedLeftField.DataFieldName, resolvedRightField.DataFieldName);
	
					listOfCodeSta.Add(CodeHelper.MethodInvoke(queryBuilderVarName, "Append", '\"' + from + '\"'));

					GenerateRelations(resolvedRightField.TargetMessageType, hashOfDataRelationsToUse, listOfCodeSta, defaultMessageType, queryBuilderVarName, listOfUsedMessages);
				}
			}

			//			//Gera os dataRelations
			//			Type typeToFind = defaultMessageType;
			//			for (int i = hashOfDataRelationsToUse.Count; i > 0; i++) {
			//				
			//			}
			//
			//
			//			//Percorre todos os dataRelations
			//			foreach(DataRelationAttribute dataRelation in hashOfDataRelationsToUse.Values) {
			//				ResolvedFieldInfo resolvedLeftField = dataRelation._leftFieldInfo.GetResolvedFieldInfo(defaultMessageType);
			//				ResolvedFieldInfo resolvedRightField = dataRelation._rightFieldInfo.GetResolvedFieldInfo(defaultMessageType);
			//
			//				//Monta a linha do join
			//				string from = string.Format(" {0} JOIN {1}", Helpers.ResolveJoinType(dataRelation._join), resolvedRightField.DataTableName);
			//				from += string.Format(" ON {0} = {1}", resolvedLeftField.DataFieldName, resolvedRightField.DataFieldName);
			//
			//				listOfCodeSta.Add(CodeHelper.MethodInvoke(queryBuilderVarName, "Append", '\"' + from + '\"'));
			//			}
		}


		#endregion

//		#region CreateWhereParameters
//		public static CodeStatement[] CreateWhereParameters(string varParametersName, ArrayList listOfWhere, string parameterPrefix) {
//			ArrayList listOfCodeSta = new ArrayList();
//
//			if (listOfWhere == null || listOfWhere.Count == 0) {
//				return null;
//			}
//
//			//Monta os parameters
//			CodeIterationStatement interaction = null;
//			foreach(WhereAttribute where in listOfWhere) {
//				//Verifica se não é bloco, isnull ou notIsnull
//				if (where._Block == Block.None && where._Filter != Filter.IsNull && where._Filter != Filter.NotIsNull) {
//					Type paramType;
//					string paramName;
//					string sqlParamDeclaration;
//					string sqlParamValue;
//					//Verifica se o parametro é um array
//					string parameterName = parameterPrefix != null ? parameterPrefix + "." + where._parameterName: where._parameterName;
//					if (where._typeOfParameter.IsArray) {
//						interaction = new CodeIterationStatement();
//						interaction.InitStatement = new CodeExpressionStatement(new CodeSnippetExpression( (string.Format("int i = 0, l = {0}.Length", where._parameterName))));
//						interaction.TestExpression = new CodeSnippetExpression(" i <  l");
//						interaction.IncrementStatement = new CodeExpressionStatement(new CodeSnippetExpression("i++"));
//						paramType = where._typeOfParameter.GetElementType();
//						paramName = where._parameterName;
//						sqlParamDeclaration = string.Format("string.Format(\"@{0}\", i)", paramName + "_{0}");
//						if (paramType != typeof(bool)) {
//							sqlParamValue = string.Format("sqlParam.Value = {0}[i] != {1} ? {0}[i] : (object)DBNull.Value", parameterName, Helpers.GetNullValue(paramType));
//						} else {
//							sqlParamValue = string.Format("sqlParam.Value = {0}[i]", parameterName);
//						}
//					} 
//					else {
//						paramType = where._typeOfParameter;
//						paramName = where._parameterName;
//						sqlParamDeclaration = string.Format("\"@{0}\"", paramName);
//						if (paramType != typeof(bool)) {
//							sqlParamValue = string.Format("sqlParam.Value = {0} != {1} ? {0} : (object)DBNull.Value", parameterName, Helpers.GetNullValue(paramType));
//						} else {
//							sqlParamValue = string.Format("sqlParam.Value = {0}", parameterName);
//						}
//					}
//					
//					//CodeAssignStatement paramInit = new CodeAssignStatement(new CodeVariableReferenceExpression("sqlParam"), new CodeObjectCreateExpression("SqlParameter"));
//					CodeAssignStatement paramAss = new CodeAssignStatement();
//					paramAss.Left = new CodeVariableReferenceExpression("sqlParam");
//					paramAss.Right = new CodeObjectCreateExpression("SqlParameter",
//						new CodeSnippetExpression(sqlParamDeclaration),
//						new CodeVariableReferenceExpression(Helpers.CSharpToSqlDBType(paramType))
//						); 
//
//					CodeMethodInvokeExpression paramExpAdd = new CodeMethodInvokeExpression(
//						new CodeVariableReferenceExpression("listOfParameters"),
//						"Add", new CodeVariableReferenceExpression("sqlParam"));
//
//					CodeSnippetExpression paramValueExp = new CodeSnippetExpression(sqlParamValue);
//
//					//Verifica se o parametro é um array
//					if (where._typeOfParameter.IsArray) {
//						interaction.Statements.Add(paramAss);
//						interaction.Statements.Add(new CodeExpressionStatement(paramValueExp));
//						interaction.Statements.Add(new CodeExpressionStatement(paramExpAdd));
//						listOfCodeSta.Add(interaction);
//					} 
//					else {
//						//listOfCodeSta.Add(paramInit);
//						listOfCodeSta.Add(paramAss);
//						listOfCodeSta.Add(new CodeExpressionStatement(paramValueExp));
//						listOfCodeSta.Add(new CodeExpressionStatement(paramExpAdd));
//					}
//				}
//			}
//
//			if (listOfCodeSta.Count > 0) {
//				//Declaração do array de Parameters
//				CodeVariableDeclarationStatement paramDecl 	= new CodeVariableDeclarationStatement(
//					"SqlParameter", "sqlParam");
//				//Adiciona a declaração no início
//				listOfCodeSta.Insert(0, paramDecl);
//
//				//Declaração do listArray 
//				CodeVariableDeclarationStatement listDecl = new CodeVariableDeclarationStatement(
//					"ArrayList", "listOfParameters", new CodeObjectCreateExpression("ArrayList"));
//				//Adiciona a declaração no início
//				listOfCodeSta.Insert(0, listDecl);
//
//				listOfCodeSta.Insert(0, new CodeCommentStatement("Criação e atribuição dos parametros"));
//
//				//Declaração do array de parameters
//				CodeVariableDeclarationStatement arrayParamDecl = new CodeVariableDeclarationStatement(
//					"SqlParameter[]", varParametersName, 
//					new CodeSnippetExpression("(SqlParameter[])listOfParameters.ToArray(typeof(SqlParameter))")
//					);
//				//Adiciona no FINAL a declaração do array
//				listOfCodeSta.Add(arrayParamDecl);
//
//				//Retorna os statements
//				return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
//			} 
//			else {
//				return null;
//			}
//		}
//		#endregion

		#region CreateInsertUpdateParameters
		public static CodeStatement[] CreateInsertUpdateParameters(string varParametersName, string parameterPrefix, ArrayList listOfFields) {
			ArrayList listOfCodeSta = new ArrayList();

			if (listOfFields.Count > 0) {
#if SQL
                CodeVariableDeclarationStatement varParametersDecl = new CodeVariableDeclarationStatement(
					"SqlParameter[]", varParametersName, new CodeArrayCreateExpression(
					"SqlParameter[]", new CodeSnippetExpression(listOfFields.Count.ToString())
					)
					);
#elif ORACLE
                CodeVariableDeclarationStatement varParametersDecl = new CodeVariableDeclarationStatement(
                    "OracleParameter[]", varParametersName, new CodeArrayCreateExpression(
                    "OracleParameter[]", new CodeSnippetExpression(listOfFields.Count.ToString())
                    )
                    );
#else
                CodeVariableDeclarationStatement varParametersDecl = new CodeVariableDeclarationStatement(
					"SqlParameter[]", varParametersName, new CodeArrayCreateExpression(
					"SqlParameter[]", new CodeSnippetExpression(listOfFields.Count.ToString())
					)
					);
#endif
                listOfCodeSta.Add(varParametersDecl);

				//Percorre todos os fields identificados na criação do insert/update
				for (int i = 0, l = listOfFields.Count; i < l; i++) {
					ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfFields[i];

					CodeAssignStatement varParametersInit = new CodeAssignStatement();
					varParametersInit.Left = new CodeVariableReferenceExpression(string.Format("{0}[{1}]", varParametersName, i));
#if SQL
                    varParametersInit.Right = new CodeObjectCreateExpression(
						"SqlParameter", 
						new CodeSnippetExpression(string.Format("\"@{0}\"", resolvedField.FieldInfo.Name)),
						new CodeSnippetExpression(string.Format("System.Data.SqlDbType.{0}", Helpers.CSharpToSqlDBType(resolvedField.FieldInfo.FieldType).ToString()))
						);
#elif ORACLE
                    varParametersInit.Right = new CodeObjectCreateExpression(
                        "OracleParameter",
                        new CodeSnippetExpression(string.Format("\":{0}\"", resolvedField.FieldInfo.Name)),
                        new CodeSnippetExpression(string.Format("System.Data.OracleClient.OracleType.{0}", Helpers.CSharpToOracleType(resolvedField.FieldInfo.FieldType).ToString()))
                        );
#else
                    varParametersInit.Right = new CodeObjectCreateExpression(
						"SqlParameter", 
						new CodeSnippetExpression(string.Format("\"@{0}\"", resolvedField.FieldInfo.Name)),
						new CodeSnippetExpression(string.Format("System.Data.SqlDbType.{0}", Helpers.CSharpToSqlDBType(resolvedField.FieldInfo.FieldType).ToString()))
						);
#endif

                    listOfCodeSta.Add(varParametersInit);
					CodeAssignStatement varParametersAss = new CodeAssignStatement();
					varParametersAss.Left = new CodeVariableReferenceExpression(string.Format("{0}[{1}].Value", varParametersName, i));
					string parameterName = parameterPrefix != null ? parameterPrefix + resolvedField.FieldInfo.Name : resolvedField.FieldInfo.Name;
                    if (resolvedField.FieldInfo.FieldType != typeof(bool)) {
#if SQL
						varParametersAss.Right = new CodeVariableReferenceExpression(string.Format("{0} != {1} ? {0} : (object)DBNull.Value", parameterName, Helpers.GetNullValue(resolvedField.FieldInfo.FieldType)));
#elif ORACLE
                        varParametersAss.Right = new CodeVariableReferenceExpression(string.Format("{0} != {1} ? {2} : (object)DBNull.Value", parameterName, Helpers.GetNullValue(resolvedField.FieldInfo.FieldType), parameterName + (resolvedField.FieldInfo.FieldType.Equals(typeof(Guid)) ? ".ToString()" : "")));
#else
						varParametersAss.Right = new CodeVariableReferenceExpression(string.Format("{0} != {1} ? {0} : (object)DBNull.Value", parameterName, Helpers.GetNullValue(resolvedField.FieldInfo.FieldType)));
#endif
                    }
                    else
                    {
						varParametersAss.Right = new CodeVariableReferenceExpression(string.Format("{0}", parameterName));
					}

					listOfCodeSta.Add(varParametersAss);
				}

				//				int i = 0;
				//				foreach(DictionaryEntry entry in hashOfFields) {
				//					FieldInfo field = (FieldInfo)entry.Key;
				//					string dataField = (string)entry.Value;
				//
				//					CodeAssignStatement varParametersInit = new CodeAssignStatement();
				//					varParametersInit.Left = new CodeVariableReferenceExpression(string.Format("{0}[{1}]", varParametersName, i));
				//					varParametersInit.Right = new CodeObjectCreateExpression(
				//						"SqlParameter", 
				//						new CodeSnippetExpression(string.Format("\"@{0}\"", field.Name)),
				//						new CodeVariableReferenceExpression(Helpers.CSharpToSqlDBType(field.FieldType))
				//						);
				//
				//					listOfCodeSta.Add(varParametersInit);
				//					CodeAssignStatement varParametersAss = new CodeAssignStatement();
				//					varParametersAss.Left = new CodeVariableReferenceExpression(string.Format("{0}[{1}].Value", varParametersName, i));
				//					string parameterName = parameterPrefix != null ? parameterPrefix + "." + field.Name : field.Name;
				//					if (field.FieldType != typeof(bool)) {
				//						varParametersAss.Right = new CodeVariableReferenceExpression(string.Format("{0} != {1} ? {0} : (object)DBNull.Value", parameterName, Helpers.GetNullValue(field.FieldType)));
				//					} else {
				//						varParametersAss.Right = new CodeVariableReferenceExpression(string.Format("{0}", parameterName));
				//					}
				//
				//					listOfCodeSta.Add(varParametersAss);
				//					i++;
				//				}

				return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
			} 
			else {
				return null;
			}
		}
		#endregion
	}

	internal class Parameter {
		public Type _type = null;
		public string _name = null;

		public Parameter(Type type, string name) {
			this._type = type;
			this._name = name;
		}
    }

	internal class DataFieldInfo {
		protected Type _messageType = null;
		protected string _field;

		protected DataFieldInfo() {}

		public DataFieldInfo(Type messageType, string field) {
			this._field = field;
			this._messageType = messageType;
		}

		public ResolvedFieldInfo GetResolvedFieldInfo(Type messageTypeToSearch) {
			return new ResolvedFieldInfo(_messageType, _field, messageTypeToSearch);
		}
	}

	internal class ResolvedFieldInfo {
		private string _dataFieldName = null;
		private string _dataTableName = null;
		private Type _targetMessageType = null;
		internal string FieldAliasPrefix = null;

		internal string FieldAliasPrefixToDb {
			get {
				return FieldAliasPrefix != null ? FieldAliasPrefix.Replace(".", "_") : null;
			}
		}

		private FieldInfo _fieldInfo = null;

		public ResolvedFieldInfo(Type baseMessageType, string field, Type messageTypeToSearch) {
			//Verifica se existe alguma mensagem válida para pesquisar
			if (baseMessageType == null && messageTypeToSearch == null) {
				throw new ApplicationException("Alguma datamessage deve ser especificada");
			}
			//Faz a resolução do field
			//Obtem a mensagem que deverá usar de base
			Type fieldMessageType = baseMessageType != null ? baseMessageType : messageTypeToSearch;

			//Verifica se encon Field
			FieldInfo fieldInfo = fieldMessageType.GetField( field );
			if (null != fieldInfo) 
			{
				this._fieldInfo = fieldInfo;
			} 
			else 
			{
				throw new ApplicationException( string.Format("field não encontrado {0}", field ) );
			}

			//Resolve os dataReferences
			ResolveDataReferences(ref fieldMessageType, ref field);

			this._targetMessageType = fieldMessageType;
#if SQL
            this._dataTableName = string.Format("[{0}]", Helpers.GetDataTableName(fieldMessageType));
			this._dataFieldName = string.Format("{0}.{1}", _dataTableName, field);
#elif ORACLE
            this._dataTableName = string.Format("\\\"{0}\\\"", Helpers.GetDataTableName(fieldMessageType));
            this._dataFieldName = string.Format("{0}.\\\"{1}\\\"", _dataTableName, field);
#else
            this._dataTableName = string.Format("[{0}]", Helpers.GetDataTableName(fieldMessageType));
            this._dataFieldName = string.Format("{0}.{1}", _dataTableName, field);
#endif
        }

		private FieldInfo FindField ( Type messageType , string field )
		{
			FieldInfo [] fields = messageType.GetFields ( BindingFlags.Public | BindingFlags.Instance );

			foreach ( FieldInfo f in fields )
			{
				if ( f.FieldType.IsArray )
				{
					if ( f.FieldType.GetElementType().IsSubclassOf ( typeof ( DataMessageBase ) ) )
					{
						return FindField ( f.FieldType.GetElementType(), field );
					}			
				} 
				else
				{
					if ( f.Name == field )
					{
						return f;
					}
				}
			}
			return null ;
		}

		private void ResolveDataReferences(ref Type fieldMessageType, ref string dataField) {
			//Verifica se encon Field
			FieldInfo fieldInfo = fieldMessageType.GetField(dataField);
			if (null == fieldInfo) {
				throw new ApplicationException( string.Format("field não encontrado {0}", dataField ) );
			}

			//Verifica se possui o atributo DataReference
			DMAtt.DataReferenceAttribute dataReferenceAtt = (DMAtt.DataReferenceAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DMAtt.DataReferenceAttribute), true);
			if (dataReferenceAtt != null) {
				if (dataReferenceAtt._field != null && dataReferenceAtt._field.Length > 0) {
					dataField = dataReferenceAtt._field;
				}

				if (dataReferenceAtt._messageType != null) {
					fieldMessageType = dataReferenceAtt._messageType;
					ResolveDataReferences(ref fieldMessageType, ref dataField);
				}
			}
		}

		public FieldInfo FieldInfo {
			get { return _fieldInfo; }
		}

		public string DataFieldName {
			get { return _dataFieldName; }
		}

		public string DataTableName {
			get { return _dataTableName; }
		}

		public Type TargetMessageType {
			get { return _targetMessageType; }
		}
	}
}
