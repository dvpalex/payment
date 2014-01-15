using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;

namespace SuperPag.Framework.Web.WebControls
{
	[ToolboxData("<{0}:PopUpHyperLink runat=server></{0}:PopUpHyperLink>")]
	public class PopUpHyperLink : System.Web.UI.WebControls.HyperLink
	{

		protected override void OnPreRender(EventArgs e)
		{
			this.NavigateUrl = "javascript:OpenPrintPopUpWindow('" + this.NavigateUrl + "')";
			base.OnPreRender (e);
		}

	}
}
