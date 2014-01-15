using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.TransactionSearch
{
    public class Search : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            Controller.Lib.Commands.MTransactionSearch mTransactionSearch = (MTransactionSearch)this.GetMessage(typeof(MTransactionSearch));
            b = this.MakeCommand(typeof(ListTransaction));
            b.Parameters["TransactionSearch"] = mTransactionSearch;

            return b;
        }
    }

}
