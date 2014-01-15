using System;
using System.Reflection;
using System.CodeDom;
using System.Collections;
using System.Text;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using DMAtt = SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders {
	internal class UpdateMethodBuilder : MethodBuilderBase {
		public UpdateMethodBuilder(CodeMemberMethod methodImpl, MethodInfo methodInfo) : base(methodImpl, methodInfo) {}

		protected override Type[] BuildMethod() {
			//Obtem os par�metros do m�todo
			ParameterInfo[] methodParameters = _methodInfo.GetParameters();
			//Verifica se a assinatura do m�todo � v�lida
			ValidateParameters(methodParameters);

			//Verifica se estamos trabalhando com uma DataMessage ou par�metros set e by
			bool paramIsDataMessage = methodParameters.Length == 1;

			//Cria o ArrayList para os fields (vai ser utilizado para gera��o dos parameters)
			ArrayList listOfFields = new ArrayList();
			WhereAttribute[] arrayOfWhere = null;
			Hashtable hashOfParameters = null;

			//Obtem o nome do parametro que ser� usado como �lias caso seja o par�metro seja uma DataMessage
			string parametersAlias = null;

			//Monta a lista de campos de acordo com o tipo de par�metro
			if (paramIsDataMessage) {
				parametersAlias = methodParameters[0].Name + ".";
				ExtractDataMessageData(ref listOfFields, out arrayOfWhere, out hashOfParameters, _defaultMessageType);
			} else {
				ExtractParametersSetByData(ref listOfFields, out arrayOfWhere, out hashOfParameters, _defaultMessageType, methodParameters);
			}
			
			//Monta a delcara��o do queryBuilder para o update
			CodeVariableDeclarationStatement updateBuilderSta = new CodeVariableDeclarationStatement("StringBuilder", "updateBuilder");
			updateBuilderSta.InitExpression = new CodeObjectCreateExpression("StringBuilder");

			//Gera o Insert
			CodeStatement[] update = this.CreateUpdate("updateBuilder", _defaultMessageType, listOfFields);

			//Gera os par�metros
			CodeStatement[] parameters = Common.CreateInsertUpdateParameters("updateParameters", paramIsDataMessage ?  parametersAlias : "set_", listOfFields);

			//Gera o where
			string sqlParametesVarName = null;
			CodeStatement[] where = Common.CreateWhere("updateBuilder", _methodInfo, _defaultMessageType, null, arrayOfWhere, hashOfParameters, out sqlParametesVarName, false, paramIsDataMessage ?  parametersAlias : "by_");

			//Gera a linha que verifica o resultado
			_methodImpl.Statements.Add(new CodeCommentStatement("Gera o update"));
			_methodImpl.Statements.Add(updateBuilderSta);
			_methodImpl.Statements.AddRange(update);
			_methodImpl.Statements.AddRange(where);
			
			//Declara e cria os parametros do where
#if SQL
            _methodImpl.Statements.Add(new CodeVariableDeclarationStatement("SqlParameter[]", "whereParameters",
				new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("where"), "GetParameters")
				));
#elif ORACLE
            _methodImpl.Statements.Add(new CodeVariableDeclarationStatement("OracleParameter[]", "whereParameters",
                new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("where"), "GetParameters")
                ));
#else
            _methodImpl.Statements.Add(new CodeVariableDeclarationStatement("SqlParameter[]", "whereParameters",
				new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("where"), "GetParameters")
				));
#endif

            //Gera��o dos parametros do update
			_methodImpl.Statements.AddRange(parameters);

			//Cria a declara��o do array de SqlParameters final
			CodeVariableDeclarationStatement sqlParamterDecl = new CodeVariableDeclarationStatement();
#if SQL
            sqlParamterDecl.Type = new CodeTypeReference("SqlParameter[]");
#elif ORACLE
            sqlParamterDecl.Type = new CodeTypeReference("OracleParameter[]");
#else
            sqlParamterDecl.Type = new CodeTypeReference("SqlParameter[]");
#endif
            sqlParamterDecl.Name = "sqlParameters";
#if SQL
            sqlParamterDecl.InitExpression = new CodeArrayCreateExpression("SqlParameter", new CodeSnippetExpression("updateParameters.Length + whereParameters.Length"));
#elif ORACLE
            sqlParamterDecl.InitExpression = new CodeArrayCreateExpression("OracleParameter", new CodeSnippetExpression("updateParameters.Length + whereParameters.Length"));
#else
            sqlParamterDecl.InitExpression = new CodeArrayCreateExpression("SqlParameter", new CodeSnippetExpression("updateParameters.Length + whereParameters.Length"));
#endif
            _methodImpl.Statements.Add(sqlParamterDecl);

			//Cria o c�digo que ir� fazer a jun��o dos dos arrays de parameters, do where e do update
			CodeMethodInvokeExpression sqlParameterConcat1 = new CodeMethodInvokeExpression(
				new CodeVariableReferenceExpression("updateParameters"),
				"CopyTo", new CodeVariableReferenceExpression("sqlParameters"), new CodeSnippetExpression("0"));
			_methodImpl.Statements.Add(sqlParameterConcat1);

			CodeMethodInvokeExpression sqlParameterConcat2 = new CodeMethodInvokeExpression(
				new CodeVariableReferenceExpression("whereParameters"),
				"CopyTo", new CodeVariableReferenceExpression("sqlParameters"), new CodeSnippetExpression("updateParameters.Length"));
			_methodImpl.Statements.Add(sqlParameterConcat2);

			//Gera a linha de execu��o
			_methodImpl.Statements.Add(new CodeCommentStatement("Execu��o do comando"));
			CodeMethodInvokeExpression baseExecute = new CodeMethodInvokeExpression(
				new CodeBaseReferenceExpression() , "Execute", //M�todo a ser invocado
				new CodeSnippetExpression("DefaultConnectionString"), //ConnectionString
				new CodeSnippetExpression("System.Data.CommandType.Text"), //CommandType
				new CodeVariableReferenceExpression("updateBuilder.ToString()"), //CommandText
				new CodeSnippetExpression("sqlParameters")//Parameters
				);
			_methodImpl.Statements.Add(baseExecute);

			//Retorna a lista de tipos referenciados
			return null;
		}

		#region ValidateParameters
		/// <summary>
		/// Valida se os par�metros do m�todo update est�o corretos
		/// </summary>
		/// <param name="methodParameters">Par�metros a serem validados</param>
		private void ValidateParameters(ParameterInfo[] methodParameters) {
			if (methodParameters.Length == 0 || (methodParameters.Length == 1 && methodParameters[0].ParameterType != _defaultMessageType)) {
				throw new AutoDataLayerBuildException("M�todos de atualiza��o devem passar um par�metro do tipo ou derivado do tipo da DataMessage padr�o da Interface ou passar par�metros utilizando a nomenclatura set_ e by_.");
			} else if (methodParameters.Length > 1) {
				bool setParams = false;
				bool byParams = false;
				//Valida os par�metros informados no m�todo
				for (int i = 0; i < methodParameters.Length; i++) {
					string paramName = methodParameters[i].Name;
					//Verifica se os par�metros iniciam com set ou by
					if (paramName.StartsWith("set_")) { 
						setParams = true;
						//Verifica se j� foi encontrado algum par�metro by, caso sim dispara um erro
						if (byParams) {
							throw new AutoDataLayerBuildException("M�todos de atualiza��o que passam par�metros set_ e by_ devem primeiro passar todos os par�metros set_ e depois todos os by_.");
						}
					} else if (paramName.StartsWith("by_")) {
						byParams = true;
						//Verifica se j� foram encontrados par�metros set, caso n�o dispara um erro
						if (!setParams) {
							throw new AutoDataLayerBuildException("M�todos de atualiza��o que passam par�metros set_ e by_ devem primeiro passar todos os par�metros set_ e depois todos os by_.");
						}
					} else {
						throw new AutoDataLayerBuildException("M�todos de atualiza��o devem passar um par�metro do tipo ou derivado do tipo da DataMessage padr�o da Interface ou passar par�metros utilizando a nomenclatura set_ e by_.");
					}
				}

				//Verifica se tanto os par�metros set e by foram informados
				if (!setParams || !byParams) {
					throw new AutoDataLayerBuildException("M�todos de atualiza��o que passam par�metros set_ e by_ devem obrigatoriamente passar ambos tipos de par�metros.");
				}
			}
		}
		#endregion

		#region ExtractDataMessageData
		private void ExtractDataMessageData(ref ArrayList listOfFields, out WhereAttribute[] arrayOfWhere, out Hashtable hashOfParameters, Type defaultMessageType) {
			ArrayList listOfPrimary = new ArrayList();
			//Percorre todos os campos da mensagem e monta a lista de campos v�lidos
			foreach(FieldInfo field in defaultMessageType.GetFields()) {				
				//Verifica se o campo � v�lido (se � value type, string, tristate)
				if ( ! field.IsLiteral && Helpers.CheckISValueType(field.FieldType)) {
					ResolvedFieldInfo resolvedField = new DataFieldInfo(defaultMessageType, field.Name).GetResolvedFieldInfo(defaultMessageType);

					//Verifica se o campo n�o � de outra dataMessage (relacionamento, etc..)
					if (resolvedField.TargetMessageType != defaultMessageType) {
						continue;
					} else if (Helpers.CheckIsPrimaryKey(field)) { //Verifica se o campo � chave prim�ria
						//Adiciona o campo na lista de chaves prim�rias
						listOfPrimary.Add(resolvedField);
						continue;
					} else if (Helpers.CheckIsIdentity(field)) {
						continue;
					}			

					//Adiciona o campo e o fieldInfo na lista de fields
					listOfFields.Add(resolvedField);
				}
			}

			//Verifica se encontrou primaryKeys
			if (listOfPrimary.Count == 0) {
				throw new AutoDataLayerBuildException("N�o foi poss�vel encontrar o atributo [PrimaryKey] na DataMessage: " + _defaultMessageType.FullName + ", necess�ria para o m�todo de Update");
			} else {
				arrayOfWhere = new WhereAttribute[listOfPrimary.Count];
				hashOfParameters = new Hashtable(listOfPrimary.Count);

				//Percorre a lista de chaves prim�rias
				for(int i = 0; i < listOfPrimary.Count; i++) {
					ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfPrimary[i];
					hashOfParameters.Add(resolvedField.FieldInfo.Name, new Parameter(resolvedField.FieldInfo.FieldType, resolvedField.FieldInfo.Name));
					arrayOfWhere[i] = new WhereAttribute(i, resolvedField.FieldInfo.Name, resolvedField.FieldInfo.Name, Filter.Equal, Link.And);
				}
			}
		}
		#endregion

		#region ExtractParametersSetByData
		private void ExtractParametersSetByData(ref ArrayList listOfFields, out WhereAttribute[] arrayOfWhere, out Hashtable hashOfParameters, Type defaultMessageType, ParameterInfo[] methodParameters) {
			ArrayList listOfBy = new ArrayList();

			//Percorre os par�metros
			for (int i = 0; i < methodParameters.Length; i++) {
				//Verifica se � SET ou BY, a valida��o j� foi feita
				string paramName = methodParameters[i].Name;
				bool isSet = paramName.StartsWith("set_"); 
				if (isSet) {
					paramName = paramName.Substring(4);
				} else {
					paramName = paramName.Substring(3);
				}
				
				ResolvedFieldInfo resolvedField = new DataFieldInfo(defaultMessageType, paramName).GetResolvedFieldInfo(defaultMessageType);
				if (isSet) {
					listOfFields.Add(resolvedField);
				} else {
					listOfBy.Add(resolvedField);
				}
			}

			//Cria os whereAttributes e HashParameters
			arrayOfWhere = new WhereAttribute[listOfBy.Count];
			hashOfParameters = new Hashtable(listOfBy.Count);

			//Percorre a lista de parametros do tipo by
			for(int i = 0; i < listOfBy.Count; i++) {
				ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfBy[i];
				hashOfParameters.Add(resolvedField.FieldInfo.Name, new Parameter(resolvedField.FieldInfo.FieldType, resolvedField.FieldInfo.Name));
				arrayOfWhere[i] = new WhereAttribute(i, resolvedField.FieldInfo.Name, resolvedField.FieldInfo.Name, Filter.Equal, Link.And);
			}
		}
		#endregion

		#region CreateUpdate
		private CodeStatement[] CreateUpdate(string updateBuilderVarName, Type messageType, ArrayList listOfFields) {
			ArrayList listOfCodeSta = new ArrayList();

			//Obtem o nome da tabela
			string dataTableName = Helpers.GetDataTableName(messageType);

			//Verifica se encontrou campos v�lidos
			if (listOfFields.Count == 0) {
				throw new AutoDataLayerBuildException("N�o existem campos v�lidos na DataMessage para gera��o do Update");
			} else {
#if SQL
				listOfCodeSta.Add(CodeHelper.MethodInvoke(updateBuilderVarName, "Append", string.Format("\" UPDATE [{0}]\"", dataTableName)));
#elif ORACLE
                listOfCodeSta.Add(CodeHelper.MethodInvoke(updateBuilderVarName, "Append", string.Format("\" UPDATE \\\"{0}\\\"\"", dataTableName)));
#else
				listOfCodeSta.Add(CodeHelper.MethodInvoke(updateBuilderVarName, "Append", string.Format("\" UPDATE [{0}]\"", dataTableName)));
#endif
                listOfCodeSta.Add(CodeHelper.MethodInvoke(updateBuilderVarName, "Append", "\" SET \""));
				
				for (int i = 0, l = listOfFields.Count; i < l; i++) {
					ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfFields[i];
#if SQL
					string inserContent = string.Format("\" {0} = @{1}{2}\"", resolvedField.DataFieldName, resolvedField.FieldInfo.Name, i < l - 1 ? "," : null);
#elif ORACLE
                    string inserContent = string.Format("\" {0} = :{1}{2}\"", resolvedField.DataFieldName, resolvedField.FieldInfo.Name, i < l - 1 ? "," : null);
#else
					string inserContent = string.Format("\" {0} = @{1}{2}\"", resolvedField.DataFieldName, resolvedField.FieldInfo.Name, i < l - 1 ? "," : null);
#endif
                    listOfCodeSta.Add(CodeHelper.MethodInvoke(updateBuilderVarName, "Append", inserContent));
				}
			}

			return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
		}
		#endregion
	}
}
