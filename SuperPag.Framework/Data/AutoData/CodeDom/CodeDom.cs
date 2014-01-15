using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.CodeDom;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.CodeDom
{
	internal class CodeDomHelper
	{

		private string _assemblyPath;
		private Type _interfaceType;
		private string _className;
		private string _cachePath;

		public CodeDomHelper( string assemblyPath, Type interfaceType, string className, string cachePath ) 
		{
			this._assemblyPath = assemblyPath;
			this._interfaceType = interfaceType;
			this._className = className;
			this._cachePath = cachePath;
		}


		/// <summary>
		/// COMMENT:
		/// </summary>
		/// <returns></returns>
		public CodeTypeDeclaration CreateClass( ) 
		{
			//Cria a classe
			CodeTypeDeclaration dlClass = new CodeTypeDeclaration( _className );

			//Define os atributos para a classe (classe, internal)
			dlClass.IsClass = true;
			dlClass.TypeAttributes = TypeAttributes.NestedAssembly | TypeAttributes.Public;

			//Define o tipo base DataObject e (interface)
			//dlClass.BaseTypes.Add("DataObject");
			dlClass.BaseTypes.Add( typeof(DataLayerBase) );
			dlClass.BaseTypes.Add( _interfaceType.Name );

			return dlClass;
		}		

		/// <summary>
		/// COMMENT:
		/// </summary>
		/// <returns></returns>
		public CodeNamespace CreateNamespace() 
		{
			//Cria o namespace
			CodeNamespace dlNamespace = new CodeNamespace(_interfaceType.Namespace);
			//Adiciona os using...
			dlNamespace.Imports.Add(new CodeNamespaceImport("System"));
			dlNamespace.Imports.Add(new CodeNamespaceImport("System.Data"));
#if SQL
            dlNamespace.Imports.Add(new CodeNamespaceImport("System.Data.SqlClient"));
#elif ORACLE
            dlNamespace.Imports.Add(new CodeNamespaceImport("System.Data.OracleClient"));
#else
            dlNamespace.Imports.Add(new CodeNamespaceImport("System.Data.SqlClient"));
#endif
            dlNamespace.Imports.Add(new CodeNamespaceImport("System.Text"));
			dlNamespace.Imports.Add(new CodeNamespaceImport("System.Collections"));
			dlNamespace.Imports.Add(new CodeNamespaceImport("SuperPag.Framework.Data.Components"));
			dlNamespace.Imports.Add(new CodeNamespaceImport("SuperPag.Framework.Data.Components.Data.Objects.Helpers"));
			dlNamespace.Imports.Add(new CodeNamespaceImport("SuperPag.Framework.Data.Components.Data.Objects.SqlServer"));
			dlNamespace.Imports.Add(new CodeNamespaceImport("SuperPag.Framework.Data.Components.Data.Objects.SqlServer.Helpers"));
			dlNamespace.Imports.Add(new CodeNamespaceImport("SuperPag.Framework.Data.Components.AutoDataLayer"));
			dlNamespace.Imports.Add(new CodeNamespaceImport("SuperPag.Framework.Data.Mapping"));
			
			return dlNamespace;
		}

		/// <summary>
		///  COMMENT:
		/// </summary>
		/// <param name="dlNamespace"></param>
		/// <param name="usedTypes"></param>
		public void Compile(
			CodeNamespace dlNamespace, 
			Type[] usedTypes, 
			bool saveSourceCodeFile ) 
		{
			///Verifica se o diretório de cache existe
			if ( !Directory.Exists( _cachePath )) System.IO.Directory.CreateDirectory( _cachePath );

			//Obtem o code provider para C#
			Microsoft.CSharp.CSharpCodeProvider codeProvider = new  Microsoft.CSharp.CSharpCodeProvider();
 
			//Cria a unidade de compilação e adiciona o namespace
			CodeCompileUnit compileUnit = new CodeCompileUnit();
			compileUnit.Namespaces.Add(dlNamespace);

			if ( saveSourceCodeFile )
			{
				string codePath = Path.Combine( _cachePath, _className + ".cs");

				//Cria a stream para gravação do arquivo .cs
				System.IO.StreamWriter streamWriter = null;
				try 
				{
					streamWriter= new System.IO.StreamWriter(codePath, false, System.Text.Encoding.UTF8);
					//Cria o codeGenerator
					System.CodeDom.Compiler.ICodeGenerator codeGenerator = codeProvider.CreateGenerator();

					//Cria e define as configurações para geração de código
					System.CodeDom.Compiler.CodeGeneratorOptions codeGeneratorOptions = new System.CodeDom.Compiler.CodeGeneratorOptions();
					codeGeneratorOptions.BracingStyle = "C#";
					codeGeneratorOptions.ElseOnClosing = true;
					codeGeneratorOptions.BlankLinesBetweenMembers = false;
			
					//Grava o código no disco
					codeGenerator.GenerateCodeFromCompileUnit(compileUnit, streamWriter, codeGeneratorOptions);
				} 
				finally 
				{
					//Fecha e grava o conteúdo da stream
					if (streamWriter != null) streamWriter.Close();
				}
			}

			
			System.CodeDom.Compiler.ICodeCompiler codeCompiler = codeProvider.CreateCompiler();

			//Cria e define as configurações para a geração do assembly
			System.CodeDom.Compiler.CompilerParameters compilerParameters = new System.CodeDom.Compiler.CompilerParameters();
			compilerParameters.OutputAssembly = _assemblyPath;
			
#if DEBUG
			//Define as opções exclusivas para o DEBUG
			compilerParameters.IncludeDebugInformation = true;
			compilerParameters.WarningLevel = 4;
			compilerParameters.TreatWarningsAsErrors = true;			
#endif

			//Cria a lista de tipos referenciados
			Hashtable hashOfUsedTypes = new Hashtable();
			hashOfUsedTypes.Add(_interfaceType.Assembly.Location, null);
            if ( ! hashOfUsedTypes.ContainsKey(this.GetType().Assembly.Location)  )
            {
                hashOfUsedTypes.Add(this.GetType().Assembly.Location, null);
            }
			
			hashOfUsedTypes.Add(typeof(System.Data.DataRow).Assembly.Location, null);
			hashOfUsedTypes.Add(typeof(System.Xml.Serialization.XmlSerializer).Assembly.Location, null);
#if SQL
#elif ORACLE
            hashOfUsedTypes.Add(typeof(System.Data.OracleClient.OracleParameter).Assembly.Location, null);
#else
#endif

			//Obtem os tipos referenciados na geração dos métodos
			if (usedTypes != null) 
			{
				for (int i = 0; i < usedTypes.Length; i++) 
				{
					if (!hashOfUsedTypes.ContainsKey(usedTypes[i].Assembly.Location)) 
					{
						hashOfUsedTypes.Add(usedTypes[i].Assembly.Location, null);
					}
				}
			}

			//Faz as referências aos assemblies
			foreach (DictionaryEntry entry in hashOfUsedTypes) 
			{
				string location = (string)entry.Key;

				compilerParameters.ReferencedAssemblies.Add(location);
			}

			System.CodeDom.Compiler.CompilerResults compilerResults = codeCompiler.CompileAssemblyFromDom(compilerParameters, compileUnit);
			
			if (compilerResults.Errors.Count > 0) 
				throw(new ApplicationException(string.Format("Erro na compilação do DataLayer. Classe: {0}. Erro: {1}.", _className, compilerResults.Errors[0].ErrorText)));
		}
	}
}
