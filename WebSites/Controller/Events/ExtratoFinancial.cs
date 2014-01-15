using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;

namespace Controller.Lib.Views.Ev.ExtratoFinancial
{
    public class ShowTransactionDetails : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            MExtrato mExtrato = (MExtrato)base.GetMessage(typeof(MExtrato));

            BaseCommand b = base.MakeCommand(typeof(Controller.Lib.Commands.ShowTransaction));
            b.Parameters["PaymentAttemptId"] = mExtrato.AttemptId;

            return b;
        }
    }

    public class GoBack : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {            
            return CommandStack.GetBaseCommandById("ShowFilter", true);
        }
    }
}
