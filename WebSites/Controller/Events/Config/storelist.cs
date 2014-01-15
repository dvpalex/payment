using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.StoreList
{
    public class SelectStore : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStore mStore = (MStore)this.GetMessage(typeof(MStore));
            b = this.MakeCommand(typeof(ShowStore));
            b.Parameters["StoreId"] = mStore.StoreId;

            return b;
        }
    }

    public class InsertNewStore : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.MakeCommand(typeof(InsertStore));

            return b;
        }
    }

}
