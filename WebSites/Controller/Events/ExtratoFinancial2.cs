using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;

namespace Controller.Lib.Views.Ev.ExtratoFinancial2
{
    public class GoBack : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {            
            return this.LastView();
        }
    }
}
