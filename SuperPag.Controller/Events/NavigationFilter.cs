using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Framework.Web.WebController;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.NavigationFilter
{
    public class NavigationFilter : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = base.MakeCommand(typeof(Controller.Lib.Commands.NavigationList));
            b.Parameters["NavigationSearch"] = (MNavigationSearch)this.GetMessage(typeof(MNavigationSearch));
            return b;
        }
    }
}
