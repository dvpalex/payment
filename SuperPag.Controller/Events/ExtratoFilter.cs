using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Framework.Web.WebController;

namespace Controller.Lib.Views.Ev.ExtratoFilter
{
    public class FilterMovement : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = base.MakeCommand(typeof(Controller.Lib.Commands.ExtratoMovement));

            b.Parameters["startDate"] = base.Parameters["startDate"];
            b.Parameters["endDate"] = base.Parameters["endDate"];
            b.Parameters["payementFormId"] = base.Parameters["payementFormId"];
            
            return b;
        }
    }

    public class FilterFinancial : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = base.MakeCommand(typeof(Controller.Lib.Commands.ExtratoFinancial));

            b.Parameters["startDate"] = base.Parameters["startDate"];
            b.Parameters["endDate"] = base.Parameters["endDate"];
            b.Parameters["payementFormId"] = base.Parameters["payementFormId"];
            return b;
        }
    }

    public class FilterFinancial2 : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = base.MakeCommand(typeof(Controller.Lib.Commands.ExtratoFinancial2));

            b.Parameters["startDate"] = base.Parameters["startDate"];
            b.Parameters["endDate"] = base.Parameters["endDate"];
            return b;
        }
    }
}
