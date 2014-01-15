using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;

namespace Controller.Lib.Commands
{
	public class ListPosts : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{

            return Map.Views.ListPosts;
		}
	}


}
