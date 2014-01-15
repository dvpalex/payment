using System;
using SuperPag.Framework;
using System.Web;
using SuperPag.Framework.Helper;


namespace  SuperPag.Framework.Web.WebController
{
	// TODO: Comentário
	public class EventTrap
	{
		// TODO: Comentário
		public void PostProcessing(BaseEvent baseEvent)
		{
			if(baseEvent.Command != null)
			{
				if(BasicFunctions.IsNumeric(baseEvent.Parameters["__SCROLL"]))
				{
					int scroll = 
						Convert.ToInt32(baseEvent.Parameters["__SCROLL"]);

					BaseCommand b = baseEvent.Command.StackManager.GetLastCommand();
					if(b != null && b.Parameters.ContainsKey("__SCROLL"))
					{
						b.Parameters["__SCROLL"] = scroll;
					}
					else if ( b != null )
					{
						b.Parameters.Add("__SCROLL", scroll);
					}					
				}
			}
		}

		// TODO: Comentário
		public void PreProcessing(BaseEvent baseEvent)
		{			
			//Loga o comando
			StepRecorder stepRecorder = new StepRecorder( null);
			
			stepRecorder.RecordEvent(baseEvent);
		}

	}
}

