using System;
using System.Reflection;
using System.CodeDom;
using System.Collections;
using System.Text;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using DMAtt = SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders {
	internal class CustomMethodBuilder : MethodBuilderBase {
		public CustomMethodBuilder(CodeMemberMethod methodImpl, System.Reflection.MethodInfo methodInfo) : base (methodImpl, methodInfo) {}
	
		protected override Type[] BuildMethod() {
			//Obtem o methodInfo do custom m�thod
			MethodTypeAttribute methodType = (MethodTypeAttribute)Attribute.GetCustomAttribute(_methodInfo, typeof(MethodTypeAttribute), true);
			MethodInfo customMethodInfo = methodType._customMethodClassType.GetMethod(methodType._customMethodName, BindingFlags.Public | BindingFlags.Static);

			//Verifica se encontrou o custom method
			if (customMethodInfo == null) {
				throw new AutoDataLayerBuildException(string.Format("O m�todo est�tico customizado informado [{0}.{1}] n�o existe.", methodType._customMethodClassType.FullName, methodType._customMethodName));
			}

			//Cria a refer�ncia � lista de par�metros
			string methodParamsText = null;
			ParameterInfo[] arrParameters = _methodInfo.GetParameters();
			//Verifica se possui algum par�metro
			if (arrParameters.Length > 0) {
				for (int i = 0, l = arrParameters.Length; i < l; i++) {
					methodParamsText += arrParameters[i].Name;

					if (i < l - 1) {
						methodParamsText += ", ";
					}
				}
			} 

			CodeExpression methodParams = new CodeSnippetExpression(methodParamsText);

			//Cria a chamada ao m�todo est�tico
			CodeVariableDeclarationStatement resultDecl = new CodeVariableDeclarationStatement(typeof(object), "result");
			//Cria a chamada do invoke
			CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression(
				new CodeVariableReferenceExpression(customMethodInfo.DeclaringType.FullName),
				customMethodInfo.Name, methodParams);
			resultDecl.InitExpression = methodInvoke;

			//Cria o c�digo de tratamento de erro		
			CodeTryCatchFinallyStatement errorHandle = new CodeTryCatchFinallyStatement();
			//Adiciona os statements que estar�o dentro do try...
			errorHandle.TryStatements.Add(resultDecl);
			
			//Cria o c�digo que ir� disparar a mensagem de erro
			CodeThrowExceptionStatement codeThrow = new CodeThrowExceptionStatement();
			codeThrow.ToThrow = new CodeSnippetExpression(string.Format("new ApplicationException(\"Erro no invoke do DataLayer. Verifique se os par�metros foram definidos corretamente. M�todo: [{0}]\", ex)", _methodInfo.Name));

			//Cria a cl�usula catch
			CodeCatchClause catchClause = new CodeCatchClause();
			catchClause.LocalName = "ex";
			catchClause.CatchExceptionType = new CodeTypeReference(typeof(Exception));
			catchClause.Statements.Add(codeThrow);

			//Adiciona o c�digo de tratamento de erro + os statements
			errorHandle.CatchClauses.Add(catchClause);

			//Verifica se o retorno do m�todo n�o � void
			if (_methodInfo.ReturnType != null) {
				//Cria o return method
				CodeMethodReturnStatement returnSta = new CodeMethodReturnStatement(
					new CodeVariableReferenceExpression(string.Format("({0})result", _methodInfo.ReturnType.FullName))
					);
				//Adiciona o retorno
				errorHandle.TryStatements.Add(returnSta);
			}

			//Adiciona o invoke no m�todo
			_methodImpl.Statements.Add(errorHandle);

			//Retorna a lista de tipos referenciados
			return new Type[] {customMethodInfo.DeclaringType};
		}
	}
}
