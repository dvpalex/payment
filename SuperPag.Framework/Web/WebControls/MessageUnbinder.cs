using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace SuperPag.Framework.Web.WebControls
{
	public class MessageUnBinder
	{
		//TODO: Comentário
		public void UnBind(MessagePage page)
		{			
			UnBindChildren(page.Controls);
		}

		//TODO: Comentário
		private void UnBindChildren(ControlCollection controls)
		{
			foreach(Control control in controls ) 
			{
				if (control is MessageControl) 
				{
					UnBindMessageControl((MessageControl)control);					
				} 
				else if( control.HasControls() )
				{
					UnBindChildren(control.Controls);
				}
			}
		}

		//TODO: Comentário
		private void UnBindMessageControl(MessageControl control)
		{
			control.MessageInit();
			control.MessageUnBind();
		}
		
	}
}
