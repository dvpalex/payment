using System;
using System.Web;

namespace  SuperPag.Framework.Web.WebController
{
	// TODO: Comentário
	public class CommandTrap
	{
		// TODO: Comentário
		public void PostProcessing(BaseCommand baseCommand)
		{
			//TODO: Melhorar
			// TODO: Comentário
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

		// TODO: Comentário
		public void PreProcessing(BaseCommand baseCommand)
		{
			//Loga o comando
			StepRecorder stepRecorder = new StepRecorder( null );
			
			stepRecorder.RecordCommand(baseCommand);

			// TODO: Comentário
			string[] eventsToRemove = CommandMap.GetEventsToRemove(baseCommand.GetType());
			HttpContext.Current.Items["__REMOVEEVENTS"] = string.Join("|", eventsToRemove);
		}

	}
}
