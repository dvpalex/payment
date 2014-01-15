using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.PaymentAgentSetupBBEdit
{
    public class Save : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAgentSetupBB mPaymentAgentSetupBB = (MPaymentAgentSetupBB)this.GetMessage(typeof(MPaymentAgentSetupBB));

            PaymentAgentSetupBB.Save(mPaymentAgentSetupBB);

            b = this.MakeCommand(typeof(ListPaymentAgentSetup));
            return b;
        }
    }

    public class Cancel : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.MakeCommand(typeof(ListPaymentAgentSetup));
            return b;
        }
    }
}
