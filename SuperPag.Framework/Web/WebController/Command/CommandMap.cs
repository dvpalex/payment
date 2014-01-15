using System;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Configuration;
using System.Collections;
using System.Diagnostics;

namespace SuperPag.Framework.Web.WebController
{
	//TODO: Comentário
	public class CommandHandler : System.Attribute
	{
		public Type _commandType;

		public CommandHandler(Type commandType)
		{
			this._commandType = commandType;
		}
	}

	// TODO: Comentario
	public class CommandMap 
	{
		private static Hashtable _map = new Hashtable();
		

		// TODO: Comentario
		protected static Type[] EventsToRemove(params Type[] eventTypes)
		{
			return eventTypes;
		}

		// TODO: Comentario
		public static string[] GetEventsToRemove(Type type)
		{
			string webAssemblyName = type.Assembly.FullName.Substring(0, type.Assembly.FullName.IndexOf(","));

			if(!_map.ContainsKey(webAssemblyName))
			{
				SetUpMap(webAssemblyName, type.Assembly);
			}			

			object eventsList = null;

			//Se continua sem a chave é porque não setamos o mapeamento
			if( _map.ContainsKey(webAssemblyName) )
			{
				Hashtable moduleMap = (Hashtable)_map[webAssemblyName];			
				eventsList = moduleMap[type.Name];
			}
			
			if(eventsList != null)
			{
				return (string[])eventsList;
			} 
			else
			{
				return new string[0];
			}			
		}	

		public static void SetUpMap(string webAssemblyName, Assembly assembly)
		{
			Hashtable moduleMap = new Hashtable();

			string mapTypeName = ModuleConfigurationHelper.GetMapTypeName(assembly);
				
			if (mapTypeName != null && mapTypeName != string.Empty)
			{
				object mapObject = assembly.CreateInstance(mapTypeName, true);

				//TODO: verificar - OBN
				//esse código não deve ser obrigatório
				if (mapObject != null) 
				{
					BindingFlags _fl = BindingFlags.Static | BindingFlags.Public;
					MethodInfo[] methods = mapObject.GetType().GetMethods( _fl );
			
					foreach(MethodInfo m in methods) 
					{
						object[] handlerAtt =  m.GetCustomAttributes(typeof(CommandHandler), false);					
						if(handlerAtt != null && handlerAtt.Length > 0) 
						{
							FieldInfo commandType = handlerAtt[0].GetType().GetField("_commandType");

							Type[] eventsList = (Type[])m.Invoke(mapObject, null);
							string[] eventsNameList = new string[eventsList.Length];

							for(int i = 0; i < eventsList.Length; i++) 
							{
								eventsNameList[i] = eventsList[i].Name;
							}
						
							moduleMap.Add(((System.Type)commandType.GetValue(handlerAtt[0])).Name, eventsNameList);
						}
					}
				}
				_map.Add(webAssemblyName, moduleMap);
			}
		}

	}

}
