using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class ListStores : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MCStore mcStore = Store.List(null);
                        
            this.AddMessage(mcStore);

            return Map.Views.ListStores;
		}
	}


}
