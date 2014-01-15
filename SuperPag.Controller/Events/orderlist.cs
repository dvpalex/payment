using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.OrderList
{
    public class SelectOrder : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MOrder mOrder = (MOrder)this.GetMessage(typeof(MOrder));
            b = this.MakeCommand(typeof(ShowOrder));
            b.Parameters["OrderId"] = mOrder.OrderId;

            return b;
        }
    }

    public class GoBack : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.LastView();

            return b;
        }
    }

    public class SortList : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MOrderSearch mOrderSearch = (MOrderSearch)this.GetMessage(typeof(MOrderSearch));
            string indexField = (string)this.Parameters["IndexField"];
            b = this.MakeCommand(typeof(ListOrder));
            b.Parameters["IndexField"] = indexField;
            b.Parameters["OrderSearch"] = mOrderSearch;

            return b;
        }
    }
}
