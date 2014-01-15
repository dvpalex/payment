using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.OrderSearch
{
    public class Search : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            Controller.Lib.Commands.MOrderSearch mOrderSearch = (MOrderSearch)this.GetMessage(typeof(MOrderSearch));
            b = this.MakeCommand(typeof(ListOrder));
            b.Parameters["OrderSearch"] = mOrderSearch;

            return b;
        }
    }

    public class SearchDetailItem : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            Controller.Lib.Commands.MOrderSearch mOrderSearch = (MOrderSearch)this.GetMessage(typeof(MOrderSearch));
            b = this.MakeCommand(typeof(ListOrderDetailItem));
            b.Parameters["OrderSearch"] = mOrderSearch;

            return b;
        }
    }

}
