using System;
using System.IO;
using Microsoft.CSharp;
using System.Reflection;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.CodeDom;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using System.Collections;
using System.CodeDom;
using SuperPag.Framework.Data.Mapping;

namespace SuperPag.Framework.Data.Components.AutoDataLayer 
{
	public class AutoDataLayerBuildException : Exception 
	{
		public AutoDataLayerBuildException(string Message) : base(Message) {}
		public AutoDataLayerBuildException(string Message, Exception InnerException) : base (Message, InnerException) {}
	}

	internal class DataLayerBuilder 
	{
		private string _assemblyPath;
		private Type _interfaceType;
		private string _className;
		private string _cachePath;

		public DataLayerBuilder( string assemblyPath, Type interfaceType, string className, string cachePath ) 
		{
			this._assemblyPath = assemblyPath;
			this._interfaceType = interfaceType;
			this._className = className;
			this._cachePath = cachePath;
		}


		public void Build() 
		{
			CodeDomHelper codeDomHelper = new CodeDomHelper( _assemblyPath, _interfaceType, _className, _cachePath );
			//Cria o namespace e a classe
			CodeNamespace dlNamespace = codeDomHelper.CreateNamespace();
			CodeTypeDeclaration dlClass = codeDomHelper.CreateClass();

			dlNamespace.Types.Add(dlClass);

			//Cria os m�todos do dataLayer
			Type[] usedTypes = CreateDLMethods(dlClass, _interfaceType);
			
			//Compila e gera o c�digo (quando em DEBUG) do data layer
			codeDomHelper.Compile(dlNamespace, usedTypes, true);
		}


		#region Cria��o dos m�todos

		private Type[] CreateDLMethods(System.CodeDom.CodeTypeDeclaration dlClass, Type interfaceType) {
			
			//Cria o in�cio do c�digo da classe (constantes, etc...)
			BuildClassBegin(dlClass, interfaceType);
			ArrayList listOfUsedTypes = new ArrayList();
			
			//Percorre cada defini��o de m�todo da interface
			foreach (MethodInfo methodInfo in interfaceType.GetMethods()) {
				
				//Cria o methodBuilder
				MethodBuilders.MethodBuilderBase methodBuilder = null;
				
				//Obtem o atributo de tipo de m�todo
				MethodTypeAttribute methodType = 
					(MethodTypeAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(MethodTypeAttribute));
				
				//Verifica se o atributo foi informado				
				if (methodType == null) throw new ApplicationException("Atributo [MethodType] n�o foi definido para o m�todo: " + interfaceType.FullName + "." + methodInfo.Name);
				
				//Cria o memberMethod
				CodeMemberMethod memberMethod = CreateMethodDeclaration(methodInfo);

				//Verifica de que tipo � o m�todo
				switch(methodType._type) 
				{
					case MethodTypes.QueryProc:
						methodBuilder = 
							new MethodBuilders.QueryProcMethodBuilder(memberMethod, methodInfo); break;
					case MethodTypes.Query:
						methodBuilder = 
							new MethodBuilders.QueryMethodBuilder(memberMethod, methodInfo); break;
					case MethodTypes.Insert:
						methodBuilder = 
							new MethodBuilders.InsertMethodBuilder(memberMethod, methodInfo); break;
					case MethodTypes.Update:
						methodBuilder = 
							new MethodBuilders.UpdateMethodBuilder(memberMethod, methodInfo); break;
					case MethodTypes.Delete:
						methodBuilder = 
							new MethodBuilders.DeleteMethodBuilder(memberMethod, methodInfo); break;
					case MethodTypes.Custom:
						methodBuilder = new MethodBuilders.CustomMethodBuilder(memberMethod, methodInfo); break;
					default:
						//methodBuilder = null; break;
						throw new ApplicationException("n�o implementado");
				}

				//Cria o c�digo do m�todo
				if (methodBuilder != null) {
					Type[] usedTypes = methodBuilder.Build( listOfUsedTypes );

					//Verifica se algum tipo foi informado
					if (usedTypes != null && usedTypes.Length > 0) {
						listOfUsedTypes.AddRange(usedTypes);
					}
				}

				//Adiciona o m�todo na classe
				dlClass.Members.Add(memberMethod);
			}

		
			//Retorna os tipos utilizados
			return (Type[])listOfUsedTypes.ToArray(typeof(Type));
		}
		#endregion

		#region Cria��o da declara��o do m�todo

		private System.CodeDom.CodeMemberMethod CreateMethodDeclaration(MethodInfo methodInfo) {
			
			//Cria o m�todo, define o nome e o seu tipo de retorno
			CodeMemberMethod method = new System.CodeDom.CodeMemberMethod();
			method.Name = methodInfo.Name;
			method.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			method.ReturnType = new CodeTypeReference(methodInfo.ReturnType);

			//Cria os atributos
			object[] attributes = methodInfo.GetCustomAttributes( false );
			foreach (object attribute in attributes )
			{
				if ( attribute is BindingAllowed )
				{
					BindingAllowed bindAllowedAttribute = (BindingAllowed)attribute;

					;
					CodeAttributeDeclaration attributeDeclaration = 
						new CodeAttributeDeclaration( typeof(BindingAllowed).FullName , new CodeAttributeArgument ( new CodeSnippetExpression ( string.Format  ( "typeof({0})", bindAllowedAttribute.DataMessageType.FullName ) ) ) ) ;
					
					method.CustomAttributes.Add ( attributeDeclaration );
				}				
			}

			//Cria os par�meros
			foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
				method.Parameters.Add(new CodeParameterDeclarationExpression(parameterInfo.ParameterType, parameterInfo.Name));
			}
			
			return method;
		}
		#endregion

		#region Cria��o do c�digo do in�cio da classe
		public static void BuildClassBegin(System.CodeDom.CodeTypeDeclaration dlClass, Type interfaceType)
		{
			//Cria a constante com da connectionString
			CodeMemberField staticField = new CodeMemberField(typeof(string), "DefaultConnectionString");
			staticField.Attributes = MemberAttributes.Private;
			dlClass.Members.Add(staticField);

			//Cria a linha que ir� atribuir a connectionString
			CodeAssignStatement connectionStringAss = new CodeAssignStatement();
			connectionStringAss.Left = new CodeVariableReferenceExpression("DefaultConnectionString");
			connectionStringAss.Right = new CodeVariableReferenceExpression("conn");

			//Cria o construtor que ir� receber a connection string
			CodeConstructor construtor = new CodeConstructor();
			construtor.Attributes = MemberAttributes.Public;
			construtor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "conn"));
			construtor.Statements.Add(connectionStringAss);
			dlClass.Members.Add(construtor);
		}
		#endregion

		#region Gera��o do c�digo
		private void CodeGenerateDataLayer(string codePath, System.CodeDom.Compiler.CodeDomProvider codeProvider, System.CodeDom.CodeCompileUnit compileUnit) {
			
		}
		#endregion

	}
}
