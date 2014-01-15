using System;
using System.Reflection;
using System.CodeDom;
using System.Collections;
using System.Text;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using DMAtt = SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders {
	internal class InsertMethodBuilder : MethodBuilderBase {
		public InsertMethodBuilder(CodeMemberMethod methodImpl, MethodInfo methodInfo) : base (methodImpl, methodInfo) {}

		protected override Type[] BuildMethod() {
			//Verifica se a assinatura do método é válida
			if (_methodInfo.GetParameters().Length != 1 || _methodInfo.GetParameters()[0].ParameterType != _defaultMessageType) {
				throw new AutoDataLayerBuildException("Métodos de inclusão devem passar um único parâmetro do tipo ou derivado do tipo da DataMessage padrão da Interface");
			}
			
			//Cria o HashTable para os fields (vai ser utilizado para geração dos parameters)
			ArrayList listOfFields = new ArrayList();
			//Cria a string para o nome do Field Identity
			string identityField = null;
			//Cria a lista de PrimaryKeys
			ArrayList listOfPrimary = new ArrayList();

			//Monta a delcaração do queryBuilder para o insert
			CodeVariableDeclarationStatement insertBuilderSta = new CodeVariableDeclarationStatement("StringBuilder", "insertBuilder");
			insertBuilderSta.InitExpression = new CodeObjectCreateExpression("StringBuilder");

            //Gera o Insert
			CodeStatement[] insert = this.CreateInsert("insertBuilder", _defaultMessageType, listOfFields, out identityField, listOfPrimary);
			//Gera os parâmetros
			CodeStatement[] parameters = Common.CreateInsertUpdateParameters("sqlParameters", _methodInfo.GetParameters()[0].Name + ".", listOfFields);

#if ORACLE
            if (identityField != null)
            {
                string dataTableName = Helpers.GetDataTableName(_defaultMessageType);
                string sequenceName = (dataTableName.Substring(0, 1) == dataTableName.Substring(0, 1).ToLower() ? dataTableName.Substring(0, 1).ToUpper() : dataTableName.Substring(0, 1).ToLower());
                sequenceName += dataTableName.Substring(1, dataTableName.Length - 1);

                CodeMethodInvokeExpression baseExecuteIdentity = new CodeMethodInvokeExpression(
                    new CodeBaseReferenceExpression(), "ExecuteScalar", //Método a ser invocado
                    new CodeSnippetExpression("DefaultConnectionString"), //ConnectionString
                    new CodeSnippetExpression("System.Data.CommandType.Text"), //CommandType
                    new CodeVariableReferenceExpression("\"SELECT \\\"" + sequenceName + "\\\".NEXTVAL FROM DUAL\""), //CommandText
                    new CodeSnippetExpression("null")//Parameters
                    );
            
				if ( _methodInfo.GetParameters()[0].GetType() == typeof(byte) )
					_methodImpl.Statements.Add(new CodeVariableDeclarationStatement(typeof(short), "newID", new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("System.Convert"), "ToByte", baseExecuteIdentity)));
				else
					_methodImpl.Statements.Add(new CodeVariableDeclarationStatement(typeof(int), "newID", new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("System.Convert"), "ToInt32", baseExecuteIdentity)));

				CodeAssignStatement idAss = new CodeAssignStatement();
				idAss.Left = new CodeVariableReferenceExpression(string.Format("{0}.{1}", _methodInfo.GetParameters()[0].Name, listOfPrimary[0]));
				idAss.Right = new CodeVariableReferenceExpression("newID");
				_methodImpl.Statements.Add(idAss);
            }
#endif

            //Gera a linha que verifica o resultado
			_methodImpl.Statements.Add(new CodeCommentStatement("Gera o insert"));
			_methodImpl.Statements.Add(insertBuilderSta);
			_methodImpl.Statements.AddRange(insert);
			_methodImpl.Statements.AddRange(parameters);

#if SQL
			//Verifica se possui field Identity
			string methodName = identityField == null ? "Execute" : "ExecuteScalar";
#elif ORACLE
			//Verifica se possui field Identity
			string methodName = "Execute";
#else
            //Verifica se possui field Identity
            string methodName = identityField == null ? "Execute" : "ExecuteScalar";
#endif

            //Gera a linha de execução
			_methodImpl.Statements.Add(new CodeCommentStatement("Execução do comando"));
			CodeMethodInvokeExpression baseExecute = new CodeMethodInvokeExpression(
				new CodeBaseReferenceExpression() , methodName, //Método a ser invocado
				new CodeSnippetExpression("DefaultConnectionString"), //ConnectionString
				new CodeSnippetExpression("System.Data.CommandType.Text"), //CommandType
				new CodeVariableReferenceExpression("insertBuilder.ToString()"), //CommandText
				new CodeSnippetExpression(parameters != null ? "sqlParameters"  : "null")//Parameters
				);
			//Verifica se possui field Identity
			if (identityField == null) {
				_methodImpl.Statements.Add(new CodeExpressionStatement(baseExecute));
			} else {
				//TODO: teste

#if SQL
				if ( _methodInfo.GetParameters()[0].GetType() == typeof(byte) )
					_methodImpl.Statements.Add(new CodeVariableDeclarationStatement(typeof(short), "newID", new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("System.Convert"), "ToByte", baseExecute)));
				else
					_methodImpl.Statements.Add(new CodeVariableDeclarationStatement(typeof(int), "newID", new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("System.Convert"), "ToInt32", baseExecute)));

				CodeAssignStatement idAss = new CodeAssignStatement();
				idAss.Left = new CodeVariableReferenceExpression(string.Format("{0}.{1}", _methodInfo.GetParameters()[0].Name, listOfPrimary[0]));
				idAss.Right = new CodeVariableReferenceExpression("newID");
				_methodImpl.Statements.Add(idAss);
#elif ORACLE
				_methodImpl.Statements.Add(new CodeExpressionStatement(baseExecute));
#else
                if (_methodInfo.GetParameters()[0].GetType() == typeof(byte))
                    _methodImpl.Statements.Add(new CodeVariableDeclarationStatement(typeof(short), "newID", new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("System.Convert"), "ToByte", baseExecute)));
                else
                    _methodImpl.Statements.Add(new CodeVariableDeclarationStatement(typeof(int), "newID", new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("System.Convert"), "ToInt32", baseExecute)));

                CodeAssignStatement idAss = new CodeAssignStatement();
                idAss.Left = new CodeVariableReferenceExpression(string.Format("{0}.{1}", _methodInfo.GetParameters()[0].Name, listOfPrimary[0]));
                idAss.Right = new CodeVariableReferenceExpression("newID");
                _methodImpl.Statements.Add(idAss);
#endif
            }

			//Retorna a lista de tipos referenciados
			return null;
		}

		private CodeStatement[] CreateInsert(string insertBuilderVarName, Type messageType, ArrayList listOfFields, out string identityField, ArrayList listOfPrimary) {
			ArrayList listOfCodeSta = new ArrayList();
			identityField = null;
			
			//Percorre todos os campos da mensagem e monta a lista de campos válidos
			foreach(FieldInfo field in messageType.GetFields()) {
				//Verifica se o campo é válido (se é value type, string, tristate)
				if (! field.IsLiteral && Helpers.CheckISValueType(field.FieldType)) {
					ResolvedFieldInfo resolvedField = new DataFieldInfo(messageType, field.Name).GetResolvedFieldInfo(messageType);

					//Verifica se o campo não é de outra dataMessage (relacionamento, etc..)
					if (resolvedField.TargetMessageType != messageType) {
						continue;
					} else if (Helpers.CheckIsIdentity(field)) { //Verifica se é Identity
						//Adiciona o campo na lista de chaves primárias
						listOfPrimary.Add(field.Name);

						//Confirma que o identityField ainda não foi encontrado
						if (identityField != null) {
							throw new AutoDataLayerBuildException("Não é permitido mais de um campo com o atributo [Identity], DataMessage: " + messageType.FullName);
						} else {
							identityField = resolvedField.FieldInfo.Name;
#if ORACLE
                            //Adiciona o campo e o fieldInfo na lista de fields
                            listOfFields.Add(resolvedField);
#endif
                            continue;
						}
					}			

					//Adiciona o campo e o fieldInfo na lista de fields
					listOfFields.Add(resolvedField);
				}
			}

			//Obtem o nome da tabela
			string dataTableName = Helpers.GetDataTableName(messageType);

			//Verifica se encontrou campos válidos
			if (listOfFields.Count == 0) {
				throw new AutoDataLayerBuildException("Não existe campos válidos na mensagem para geração do Insert");
			} else {
#if SQL
				listOfCodeSta.Add(CodeHelper.MethodInvoke(insertBuilderVarName, "Append", string.Format("\" INSERT INTO [{0}] (\"", dataTableName)));
#elif ORACLE
                listOfCodeSta.Add(CodeHelper.MethodInvoke(insertBuilderVarName, "Append", string.Format("\" INSERT INTO \\\"{0}\\\" (\"", dataTableName)));
#else
				listOfCodeSta.Add(CodeHelper.MethodInvoke(insertBuilderVarName, "Append", string.Format("\" INSERT INTO [{0}] (\"", dataTableName)));
#endif
                for (int i = 0, l = listOfFields.Count; i < l; i++) {
					ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfFields[i];

					string inserContent = string.Format("\" {0}{1}\"", resolvedField.DataFieldName, i < l - 1 ? "," : null);
					listOfCodeSta.Add(CodeHelper.MethodInvoke(insertBuilderVarName, "Append", inserContent));
				}				
				listOfCodeSta.Add(CodeHelper.MethodInvoke(insertBuilderVarName, "Append", "\" ) VALUES (\""));

				for (int i = 0, l = listOfFields.Count; i < l; i++) {
					ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfFields[i];

#if SQL
					string inserContent = string.Format("\" @{0}{1}\"", resolvedField.FieldInfo.Name, i < l - 1 ? "," : ")");
#elif ORACLE
                    string inserContent = string.Format("\" :{0}{1}\"", resolvedField.FieldInfo.Name, i < l - 1 ? "," : ")");
#else
					string inserContent = string.Format("\" @{0}{1}\"", resolvedField.FieldInfo.Name, i < l - 1 ? "," : ")");
#endif
                    listOfCodeSta.Add(CodeHelper.MethodInvoke(insertBuilderVarName, "Append", inserContent));
				}

				//Verifica se possui campo Identity
#if SQL
				if (identityField != null) {
					listOfCodeSta.Add(CodeHelper.MethodInvoke(insertBuilderVarName, "Append", string.Format("\" ;SELECT SCOPE_IDENTITY() as NEWID\"", dataTableName)));
				}
#elif ORACLE
#else
                if (identityField != null)
                {
                    listOfCodeSta.Add(CodeHelper.MethodInvoke(insertBuilderVarName, "Append", string.Format("\" ;SELECT SCOPE_IDENTITY() as NEWID\"", dataTableName)));
                }
#endif
            }

			return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
		}
	}
}
