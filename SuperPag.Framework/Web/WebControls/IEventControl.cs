using System;

namespace SuperPag.Framework.Web.WebControls
{
	/// <summary>
	/// Summary description for IEventControl.
	/// </summary>
	public interface IEventControl : MessageControl
	{
		// TODO: Coment�rio
		void Hide();
		
		// TODO: Coment�rio
		void Show();

		// TODO: Coment�rio
		string GetEventName();
	}
}
