using System;

namespace SuperPag.Framework.Web.WebControls
{
	/// <summary>
	/// Summary description for IEventControl.
	/// </summary>
	public interface IEventControl : MessageControl
	{
		// TODO: Comentário
		void Hide();
		
		// TODO: Comentário
		void Show();

		// TODO: Comentário
		string GetEventName();
	}
}
