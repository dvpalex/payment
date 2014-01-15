using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Helper;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag;

namespace Controller.Lib.Commands
{
    public class EditStore : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int storeId = (int)this.Parameters["StoreId"];

            MStore mStore = Store.Locate(storeId);

            this.AddMessage(mStore);

            MCHandshakeConfiguration mcHandshakeConfiguration = HandshakeConfiguration.List(mStore.StoreId);
            this.AddMessage(mcHandshakeConfiguration);

            return Map.Views.EditStore;
		}
	}

    public class InsertStore : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            MStore mStore = new MStore();

            mStore.StoreKey = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            mStore.StoreId = Store.SelectMaxId() + 1;
            mStore.IsNew = true;

            this.AddMessage(mStore);

            MCHandshakeConfiguration mcHandshakeConfiguration = HandshakeConfiguration.List(mStore.StoreId);
            this.AddMessage(mcHandshakeConfiguration);

            return Map.Views.EditStore;
        }
    }

}
