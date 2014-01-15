using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;

namespace Controller.Lib.Commands
{
	public class ChangePassword : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
			return Map.Views.ChangePassword;
		}
	}    
}
