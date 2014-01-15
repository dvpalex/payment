using System;
using System.Reflection;
using System.CodeDom;

namespace SuperPag.Framework.Data.Builder
{
    internal class Builder
	{
		private string _assemblyPath;
		private Type _interfaceType;
		private string _className;
		private string _cachePath;
		
		public Builder( string assemblyPath, Type interfaceType, string className, string cachePath ) 
		{
			this._assemblyPath = assemblyPath;
			this._interfaceType = interfaceType;
			this._className = className;
			this._cachePath = cachePath;
		}

		public void Build()
		{

			Parser.MethodParser parser = new Parser.MethodParser ( _interfaceType );

			Parser.DataMethod[] methods = parser.ExtractAllMethods();
						
		}

		private CodeTypeDeclaration CreateClass()
		{
			//Cria o namespace 
			CodeNamespace dlNamespace = new CodeNamespace(_interfaceType.Namespace);

			//Cria a classe
			CodeTypeDeclaration dlClass = new CodeTypeDeclaration( _className );

			dlClass.IsClass = true;
			dlClass.TypeAttributes = TypeAttributes.NestedAssembly | TypeAttributes.Public;

			dlClass.BaseTypes.Add( _interfaceType.Name );

			//adiciona a classe no namespace
			dlNamespace.Types.Add(dlClass);

			return dlClass;
		}
	}
}
//using System;
//using System.IO;
//using Microsoft.CSharp;
//using System.Reflection;
//using SuperPag.Framework.Data.Components.AutoDataLayer;
//using SuperPag.Framework.Data.Components.AutoDataLayer.CodeDom;
//using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
//using System.Collections;
//using System.CodeDom;
//
//namespace SuperPag.Framework.Data.Components.AutoDataLayer 
//{
//	public class AutoDataLayerBuildException : Exception 
//	{
//		public AutoDataLayerBuildException(string Message) : base(Message) {}
//		public AutoDataLayerBuildException(string Message, Exception InnerException) : base (Message, InnerException) {}
//	}
//
//	internal class DataLayerBuilder 
//	{

//
//
//		public void Build() 
//		{
//			CodeDomHelper codeDomHelper = new CodeDomHelper( _assemblyPath, _interfaceType, _className, _cachePath );

//
//			//Cria os métodos do dataLayer
//			Type[] usedTypes = CreateDLMethods(dlClass, _interfaceType);
//			
//			//Compila e gera o código (quando em DEBUG) do data layer
//			codeDomHelper.Compile(dlNamespace, usedTypes, true);
//		}
//
//
//		#region Criação dos métodos
//
//		private Type[] CreateDLMethods(System.CodeDom.CodeTypeDeclaration dlClass, Type interfaceType) 
//		{
//			
//			//Cria o início do código da classe (constantes, etc...)
//			BuildClassBegin(dlClass, interfaceType);
//			ArrayList listOfUsedTypes = new ArrayList();
//			
//			//Percorre cada definição de método da interface
//			foreach (MethodInfo methodInfo in interfaceType.GetMethods()) 
//			{
//				
//				//Cria o methodBuilder
//				MethodBuilders.MethodBuilderBase methodBuilder = null;
//				
//				//Obtem o atributo de tipo de método
//				MethodTypeAttribute methodType = 
//					(MethodTypeAttribute)Attribute.GetCustomAttribute(methodInfo, typeof(MethodTypeAttribute));
//				
//				//Verifica se o atributo foi informado				
//				if (methodType == null) throw new ApplicationException("Atributo [MethodType] não foi definido para o método: " + interfaceType.FullName + "." + methodInfo.Name);
//				
//				//Cria o memberMethod
//				CodeMemberMethod memberMethod = CreateMethodDeclaration(methodInfo);
//
//				//Verifica de que tipo é o método
//				switch(methodType._type) 
//				{
//					case MethodTypes.Query:
//						methodBuilder = 
//							new MethodBuilders.QueryMethodBuilder(memberMethod, methodInfo); break;
//					case MethodTypes.Insert:
//						methodBuilder = 
//							new MethodBuilders.InsertMethodBuilder(memberMethod, methodInfo); break;
//					case MethodTypes.Update:
//						methodBuilder = 
//							new MethodBuilders.UpdateMethodBuilder(memberMethod, methodInfo); break;
//					case MethodTypes.Delete:
//						methodBuilder = 
//							new MethodBuilders.DeleteMethodBuilder(memberMethod, methodInfo); break;
//					case MethodTypes.Custom:
//						methodBuilder = new MethodBuilders.CustomMethodBuilder(memberMethod, methodInfo); break;
//					default:
//						//methodBuilder = null; break;
//						throw new ApplicationException("não implementado");
//				}
//
//				//Cria o código do método
//				if (methodBuilder != null) 
//				{
//					Type[] usedTypes = methodBuilder.Build( listOfUsedTypes );
//
//					//Verifica se algum tipo foi informado
//					if (usedTypes != null && usedTypes.Length > 0) 
//					{
//						listOfUsedTypes.AddRange(usedTypes);
//					}
//				}
//
//				//Adiciona o método na classe
//				dlClass.Members.Add(memberMethod);
//			}
//
//		
//			//Retorna os tipos utilizados
//			return (Type[])listOfUsedTypes.ToArray(typeof(Type));
//		}
//		#endregion
//
//		#region Criação da declaração do método
//
//		private System.CodeDom.CodeMemberMethod CreateMethodDeclaration(MethodInfo methodInfo) 
//		{
//			//Cria o método, define o nome e o seu tipo de retorno
//			CodeMemberMethod method = new System.CodeDom.CodeMemberMethod();
//			method.Name = methodInfo.Name;
//			method.Attributes = MemberAttributes.Public | MemberAttributes.Final;
//			method.ReturnType = new CodeTypeReference(methodInfo.ReturnType);
//
//			//Cria os parâmeros
//			foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) 
//			{
//				method.Parameters.Add(new CodeParameterDeclarationExpression(parameterInfo.ParameterType, parameterInfo.Name));
//			}
//			
//			return method;
//		}
//		#endregion
//
//		#region Criação do código do início da classe
//		public static void BuildClassBegin(System.CodeDom.CodeTypeDeclaration dlClass, Type interfaceType)
//		{
//			//Cria a constante com da connectionString
//			CodeMemberField staticField = new CodeMemberField(typeof(string), "DefaultConnectionString");
//			staticField.Attributes = MemberAttributes.Private;
//			dlClass.Members.Add(staticField);
//
//			//Cria a linha que irá atribuir a connectionString
//			CodeAssignStatement connectionStringAss = new CodeAssignStatement();
//			connectionStringAss.Left = new CodeVariableReferenceExpression("DefaultConnectionString");
//			connectionStringAss.Right = new CodeVariableReferenceExpression("conn");
//
//			//Cria o construtor que irá receber a connection string
//			CodeConstructor construtor = new CodeConstructor();
//			construtor.Attributes = MemberAttributes.Public;
//			construtor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "conn"));
//			construtor.Statements.Add(connectionStringAss);
//			dlClass.Members.Add(construtor);
//		}
//		#endregion
//
//		#region Geração do código
//		private void CodeGenerateDataLayer(string codePath, System.CodeDom.Compiler.CodeDomProvider codeProvider, System.CodeDom.CodeCompileUnit compileUnit) 
//		{
//			
//		}
//		#endregion
//
//	}
//}
