using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;

namespace Controller.Lib.Commands
{
    public class ShowExtratoFilterMovement : BaseCommand
    {
       protected override ViewInfo OnExecute()
       {
           base.ID = "ShowFilter";   
           ViewInfo v = Map.Views.ExtratoFilter;
           return new ViewInfo("Extrato Movimento", v.Name, v.Url);
       }
    }
                 
    public class ShowExtratoFilterFinancial : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            base.ID = "ShowFilter";
            ViewInfo v = Map.Views.ExtratoFilter;
            return new ViewInfo("Extrato Financeiro", v.Name, v.Url);
        }
    }

    public class ShowExtratoFilterFinancial2 : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            ViewInfo v = Map.Views.ExtratoFilterFinancial2;
            return new ViewInfo("Extrato Financeiro Herbalife", v.Name, v.Url);
        }
    }
}
