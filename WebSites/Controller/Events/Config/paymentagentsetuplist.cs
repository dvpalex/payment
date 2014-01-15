using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.PaymentAgentSetupList
{
    public class PaymentAgentSetupEdit : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAgentSetup mPaymentAgentSetup = (MPaymentAgentSetup)this.GetMessage(typeof(MPaymentAgentSetup));
            b = this.MakeCommand(typeof(EditPaymentAgentSetup));
            b.Parameters["PaymentAgentSetup"] = mPaymentAgentSetup;

            return b;
        }
    }

    public class PaymentAgentSetupInsert : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.MakeCommand(typeof(InsertPaymentAgentSetup));

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
}
