using System;
using System.Reflection;

namespace SuperPag.Framework.Web.WebController
{
	public class ModuleConfigurationHelper
	{
		public static AssemblyModuleConfiguration GetModuleConfig(Assembly assembly)
		{
			object[] cnf = 	assembly.GetCustomAttributes( typeof(AssemblyModuleConfiguration), true );
			if(cnf != null && cnf.Length > 0 && cnf[0] is AssemblyModuleConfiguration)
			{
				return (AssemblyModuleConfiguration)cnf[0];
			} 
			else
			{
				throw new Exception("AssemblyModuleConfiguration not found. Set a line in AssemblyInfo.cs -> [assembly: AssemblyModuleConfiguration(\"FullNameTypeOfCommands\",\"FullNameTypeOfEventMap\",\"FullNameOfTranslateClass\")]\"  ");
			}
		}

		public static string GetModuleAcronym(Assembly assembly)
		{
			return GetModuleConfig(assembly).Acronym;
		}

		public static string GetMapTypeName(Assembly assembly)
		{
			return GetModuleConfig(assembly).CommandMapType;
		}

		public static string GetCommandNamespace(Assembly assembly)
		{
			return GetModuleConfig(assembly).CommandNamespace;
		}
	}
	

	[AttributeUsage(AttributeTargets.All)]
	[Serializable]
	public class AssemblyModuleConfiguration: System.Attribute 
	{
		string _commandNamespace;
		string _commandMapType;
		string _acronym;

		public string CommandMapType
		{
			get
			{
				return _commandMapType;
			}
		}

		public string Acronym
		{
			get
			{
				return _acronym;
			}
		}

		public string CommandNamespace
		{
			get
			{
				return _commandNamespace;
			}
		}

	

		public AssemblyModuleConfiguration(
			string commandNamespace, 
			string commandMapType,
			string acronym)
		{
			_commandNamespace = commandNamespace;
			_commandMapType = commandMapType;
			_acronym = acronym;
		}
	}
}
