using System;
using System.Web;

namespace  SuperPag.Framework.Web.WebController
{
	// TODO: Coment�rio
	public class CommandTrap
	{
		// TODO: Coment�rio
		public void PostProcessing(BaseCommand baseCommand)
		{
			//TODO: Melhorar
			// TODO: Coment�rio
			string viewName = "";
			if(baseCommand.View != null)
			{
				viewName = baseCommand.View.Name;
			}
	
			if(baseCommand.UseStack) 
			{
				baseCommand.StackManager.AddCommand(
					baseCommand.GetType().Name, 
					baseCommand.GetType().FullName,
					baseCommand.GetType().Assembly.GetName().Name,
					baseCommand.ID,
					baseCommand.Parameters, 
					viewName);
			}

			if(baseCommand.Error != null)
			{
				HttpContext.Current.Items["__ERROR"] = baseCommand.Error;
			}
		}

		// TODO: Coment�rio
		public void PreProcessing(BaseCommand baseCommand)
		{
			//Loga o comando
			StepRecorder stepRecorder = new StepRecorder( null );
			
			stepRecorder.RecordCommand(baseCommand);

			// TODO: Coment�rio
			string[] eventsToRemove = CommandMap.GetEventsToRemove(baseCommand.GetType());
			HttpContext.Current.Items["__REMOVEEVENTS"] = string.Join("|", eventsToRemove);
		}

	}
}
