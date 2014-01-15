using System;
using System.Web;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;

namespace SuperPag.Framework.Web.WebController
{
	/// <summary>
	/// Summary description for CommandFactory.
	/// </summary>
	public class CommandFactory
	{
		//TODO: Comentario
		public BaseCommand Make(Type commandType)
		{
			BaseCommand command = (BaseCommand)
				commandType.Assembly.CreateInstance(commandType.FullName, true);

			return command;
		}
		
		public BaseCommand Make( string commandAssemblyName, string commandFullNamespace)
		{
			Assembly commandAssembly = Assembly.Load(commandAssemblyName);
			BaseCommand command = (BaseCommand)
				commandAssembly.CreateInstance(commandFullNamespace, true);
			if(command == null)			
			{
				throw new Exception("Command name not found: " + commandFullNamespace);
			}
			return command;
		}

		//TODO: Comentario
		public BaseCommand Make(string commandName)
		{			

			string extension = commandName.Substring( commandName.LastIndexOf(".") ).ToLower();

			if ( !Configuration.ConfigurationSettings.AppSettings.CommandMaps.ContainsKey( extension ) )
			{
				throw new Exception( string.Format("Não foi possivel achar o map relacionando o nome do assembly a ser instanciado para extensão '{0}' dentro da configuração <paymentSettings><commandsMap> no web.config ",
					extension) );
			}

			string assemblyName = (string)Configuration.ConfigurationSettings.AppSettings.CommandMaps[ extension ];

			if(assemblyName == "" || assemblyName == null)
			{
				string rawUrl = "";
				if( HttpContext.Current != null ||
					HttpContext.Current.Request != null)
				{
					rawUrl = HttpContext.Current.Request.RawUrl;
				}

				throw new Exception("Não foi possivel encontrar o assembly do módulo " + rawUrl);
			}

			commandName = commandName.ToLower().Replace(extension, "");

			Assembly commandAssembly = Assembly.Load(assemblyName);

			string commandNamespace = ModuleConfigurationHelper.GetCommandNamespace(commandAssembly);
		
			BaseCommand command = (BaseCommand) 
				commandAssembly.CreateInstance(commandNamespace + "." + commandName, true);
		
			if(command == null)			
			{
				throw new Exception("Command name not found: " + commandName);
			}
			return command;

			/* 
			 * 
			 * sessão antiga - fixa para extensões .action 

			if(System.Configuration.ConfigurationSettings.AppSettings["CommandAssembly"] == null)
			{				
				throw new Exception("Não foi possivel achar a chave  'CommandAssembly' no web.config ");
			}


			string commandAssemblyName =
				System.Configuration.ConfigurationSettings.AppSettings["CommandAssembly"].ToString();

			if(commandAssemblyName == "" || commandAssemblyName == null)
			{
				string rawUrl = "";
				if( HttpContext.Current != null ||
					HttpContext.Current.Request != null)
				{
					rawUrl = HttpContext.Current.Request.RawUrl;
				}

				throw new Exception("Não foi possivel encontrar o assembly do módulo " + rawUrl);
			}

			commandName = commandName.ToLower().Replace(".action", "");

			Assembly commandAssembly = Assembly.Load(commandAssemblyName);

			string commandNamespace = ModuleConfigurationHelper.GetCommandNamespace(commandAssembly);
		
			BaseCommand command = (BaseCommand)
				commandAssembly.CreateInstance(commandNamespace + "." + commandName, true);
		
			if(command == null)			
			{
				throw new Exception("Command name not found: " + commandName);
			}

			return command;
			*/
		}
	}

	//TODO: Comentario
	public class CommandParameter
	{
		public string key;
		public object _value;

		public CommandParameter(string key, object _value)
		{
			this.key = key;
			this._value = _value;
		}
	}
}
