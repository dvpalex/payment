using System;
using System.Reflection;
using System.CodeDom;
using System.Collections;
using System.Text;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using DMAtt = SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders {
	internal class DeleteMethodBuilder : MethodBuilderBase {
		public DeleteMethodBuilder(CodeMemberMethod methodImpl, MethodInfo methodInfo) : base (methodImpl, methodInfo) {}

		protected override Type[] BuildMethod() {
			//Verifica se o parametro é uma mensagem ou um campo
			//Obtem o nome do parametro
			bool customWhere = false;
			string parameterName;
			if(_methodInfo.GetParameters()[0].ParameterType.IsSubclassOf(typeof(DataMessageBase))) {				
				parameterName = _methodInfo.GetParameters()[0].Name + ".";			
			}
			else {
				customWhere = true;
				parameterName = null;
			}
			
			//Cria o HashTable para os fields (vai ser utilizado para geração dos parameters)
			//Cria a lista de PrimaryKeys
			ArrayList listOfPrimary = new ArrayList();

			//Monta a delcaração do queryBuilder para o insert
			CodeVariableDeclarationStatement deleteBuilderSta = new CodeVariableDeclarationStatement("StringBuilder", "deleteBuilder");
			deleteBuilderSta.InitExpression = new CodeObjectCreateExpression("StringBuilder");
			
			Hashtable hashOfParameters = new Hashtable();
			//Percorre todos os parâmetros para montar a hash e fazer as verificações necessárias
			foreach(ParameterInfo parameter in _methodInfo.GetParameters()) {
				//Adiciona o parameter na hash que será usada para criação dos SqlParameters
				hashOfParameters.Add(parameter.Name, new Parameter(parameter.ParameterType, parameter.Name));
			}

			//Gera o Delete
			CodeStatement[] delete  = this.CreateDelete("deleteBuilder", _defaultMessageType, listOfPrimary);

			if(! customWhere) {			
				//Verifica se encontrou primaryKeys
				if (listOfPrimary.Count == 0) { 
				 throw new AutoDataLayerBuildException("Não foi possível encontrar o atributo [PrimaryKey] na DataMessage: " + _defaultMessageType.FullName + ", necessária para o método de Delete");}			
			}

			WhereAttribute[] arrayOfWhere = new WhereAttribute[listOfPrimary.Count];
			if(! customWhere) {
				for(int i = 0; i < listOfPrimary.Count; i++) {
					ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)listOfPrimary[i];

					arrayOfWhere[i] = new WhereAttribute(i, resolvedField.FieldInfo.Name, resolvedField.FieldInfo.Name, Filter.Equal, Link.And);
					arrayOfWhere[i]._typeOfParameter = resolvedField.FieldInfo.FieldType;
				} 
			} else {
				int i = 0;
				//Verifica se existe o qtde de parametros diferentes entre as chaves primarias e a lista de parametros 
				if ( hashOfParameters.Count != listOfPrimary.Count ) {
					arrayOfWhere = new WhereAttribute[hashOfParameters.Count];
				}

				foreach (Parameter parameter in hashOfParameters.Values) {
					arrayOfWhere[i] = new WhereAttribute(i, (string)parameter._name, parameter._name, Filter.Equal, Link.And);
					i ++;
				}
			}

			//Gera o where
			string sqlParametersVarName = null;
			CodeStatement[] where = Common.CreateWhere("deleteBuilder", _methodInfo, _defaultMessageType, null, arrayOfWhere, hashOfParameters, out sqlParametersVarName, false, parameterName);

			//Gera a linha que verifica o resultado
			_methodImpl.Statements.Add(new CodeCommentStatement("Gera o delete"));
			_methodImpl.Statements.Add(deleteBuilderSta);
			_methodImpl.Statements.AddRange(delete);
			_methodImpl.Statements.AddRange(where);

			//Gera a linha de execução
			_methodImpl.Statements.Add(new CodeCommentStatement("Execução do comando"));
			CodeMethodInvokeExpression baseExecute = new CodeMethodInvokeExpression(
				new CodeBaseReferenceExpression() , "Execute", //Método a ser invocado
				new CodeSnippetExpression("DefaultConnectionString"), //ConnectionString
				new CodeSnippetExpression("System.Data.CommandType.Text"), //CommandType
				new CodeVariableReferenceExpression("deleteBuilder.ToString()"), //CommandText
				new CodeSnippetExpression(sqlParametersVarName)//Parameters
				);
			_methodImpl.Statements.Add(baseExecute);

			//Retorna a lista de tipos referenciados
			return null;
		}

		private CodeStatement[] CreateDelete(string deleteBuilderVarName, Type messageType, ArrayList listOfPrimary) {
			ArrayList listOfCodeSta = new ArrayList();
		
			//Percorre todos os campos da mensagem e monta a lista de campos válidos
			foreach(FieldInfo field in messageType.GetFields()) {
				//Verifica se o campo é válido (se é value type, string, tristate)
				if (! field.IsLiteral && Helpers.CheckISValueType(field.FieldType)) {
					ResolvedFieldInfo resolvedField = (ResolvedFieldInfo)new DataFieldInfo(messageType, field.Name).GetResolvedFieldInfo(messageType);

					//Verifica se o campo não é de outra dataMessage (relacionamento, etc..)
					if (resolvedField.TargetMessageType != messageType) {
						continue;
					} else if (Helpers.CheckIsPrimaryKey(field)) { //Verifica se é chave primária
						//Adiciona o campo na lista de chaves primárias
						listOfPrimary.Add(resolvedField);
					}
				}
			}
		
			string dataTableName = Helpers.GetDataTableName(messageType);
#if SQL
            listOfCodeSta.Add(CodeHelper.MethodInvoke(deleteBuilderVarName, "Append", string.Format("\" DELETE FROM [{0}]\"", dataTableName)));
#elif ORACLE
            listOfCodeSta.Add(CodeHelper.MethodInvoke(deleteBuilderVarName, "Append", string.Format("\" DELETE FROM \\\"{0}\\\"\"", dataTableName)));
#else
            listOfCodeSta.Add(CodeHelper.MethodInvoke(deleteBuilderVarName, "Append", string.Format("\" DELETE FROM [{0}]\"", dataTableName)));
#endif
            return (CodeStatement[])listOfCodeSta.ToArray(typeof(CodeStatement));
		}
	}
}
