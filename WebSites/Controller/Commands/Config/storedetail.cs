using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Helper;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag;

namespace Controller.Lib.Commands
{
    public class ShowStore : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int storeId = (int)this.Parameters["StoreId"];

            MStore mStore = Store.Locate(storeId);

            this.AddMessage(mStore);

            return Map.Views.ShowStore;
		}
	}


}
