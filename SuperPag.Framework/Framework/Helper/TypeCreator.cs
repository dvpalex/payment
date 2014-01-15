using System;
using System.Reflection;

namespace SuperPag.Framework.Helper
{
	public class TypeCreator 
	{
		public static object CreateType(string assemblyName, string fullTypeName, params object[] constructorParameters) 
		{
			string fullPathAssembly = ResourcesHelper.ResolvePrivateAssembly(assemblyName);

			//Carrega o assembly
			Assembly assembly = null;
			object typeInstance = null;
			try 
			{
				assembly = Assembly.LoadFrom(fullPathAssembly);
				
				//Cria o tipo solicitado
				typeInstance = assembly.CreateInstance(fullTypeName, true, BindingFlags.Default, null, constructorParameters, null, null);
			} 
			catch (Exception ex) 
			{
				throw new FWCException("Erro na criação do tipo solicitado. Erro: " + ex.Message, ex);
			}

			//Verifica se encontrou o tipo solicitado
			if (null == typeInstance) 
			{
				throw new FWCException("Não foi possível criar o tipo solicitado");
			}
			else 
			{
				return typeInstance;
			}
		}

		/// <summary>
		/// Constroi uma classe a partir de uma string
		/// </summary>
		/// <param name="fullTypeNameWithAssemblyName">Formato {FullTypeName,AssemblyName}</param>
		/// <returns>objeto</returns>
		public static object CreateType( string fullTypeNameWithAssemblyName ) 
		{
			int pos = fullTypeNameWithAssemblyName.IndexOf(",");

			if (pos == -1)
				new Exception(" A string deve conter 'NomeTipo,NomeDoAssembly' ");

			string typeName = fullTypeNameWithAssemblyName.Substring(0, pos);
			string assemblyName = fullTypeNameWithAssemblyName.Substring( pos + 1 );

			return CreateType( assemblyName, typeName, null);
		}
	}
}
