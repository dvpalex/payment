using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace SuperPag.Framework.Web.WebControls
{
	public class MessageBinder
	{
		//TODO: Comentário
		private void BindMessageControl(MessageControl control)
		{
			control.MessageInit();
			control.MessageBind();
		}
		

		//TODO: Comentário
		private void BindChildren(ControlCollection controls)
		{
			foreach(Control control in controls ) 
			{
				if (control is MessageControl) 
				{
					BindMessageControl((MessageControl)control);

					if(control is IEventControl)
					{
						IEventControl eventControl = (IEventControl)control;

						string[] eventsToRemove = new string[0];
						if(System.Web.HttpContext.Current.Items["__REMOVEEVENTS"] != null)
						{
							eventsToRemove = System.Web.HttpContext.Current.Items["__REMOVEEVENTS"].ToString().Split('|');
						}
						
						string eventName = eventControl.GetEventName();

						foreach(string ev in eventsToRemove)
						{
							if(ev == eventName)
							{
								eventControl.Hide();
							}
						}
					}
				} 
				else if( control.HasControls() )
				{
					BindChildren(control.Controls);
				}
			}
		}

		//TODO: Comentário
		public void Bind(MessagePage page)
		{			
			BindChildren(page.Controls);
		}
	}
}
