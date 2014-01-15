using System;
using System.Web;
using System.Collections;
using System.Web.SessionState;

namespace SuperPag.Framework.Web.WebController
{
	// TODO: comentário
	public class ActionHandler : 
			IHttpHandler, 
			IRequiresSessionState
	{
		bool IHttpHandler.IsReusable{get{return true;}}
		
		//TODO: Comentário
		void IHttpHandler.ProcessRequest(HttpContext context)
		{
			// Agora usamos o web.config para setar quais extensões são mapeadas
//			if(context.Request.Url.AbsolutePath.EndsWith(".action"))
//			{
				// Obtenho o nome do commando
				string commandName = 
					context.Request.Url.AbsolutePath;
				commandName = commandName.Substring(commandName.LastIndexOf("/") + 1);
				commandName = commandName.ToLower();

				// Executo o comando
				ExecuteCommand(commandName);
//			}	
		}

		//TODO: Comentário
		private void ExecuteCommand(string commandName)
		{
			//Crio o commando
			CommandFactory factory = new CommandFactory();
			BaseCommand command = factory.Make(commandName);
	
			System.Collections.Specialized.NameValueCollection queryString =
				HttpContext.Current.Request.QueryString;

			Hashtable parameters = new Hashtable();
			ArrayList keys = new ArrayList(queryString.AllKeys);			
			foreach(string key in keys)
			{	
				if(key != null)
				{
					string queryStringValue = queryString[key];
					if(queryStringValue != null)
					{
						parameters.Add(key, 
							HttpUtility.UrlDecode(queryString[key]));
					}
				}
				
			}
			command.SetParams(parameters);
			
			command.Execute();
		}
	}

}
