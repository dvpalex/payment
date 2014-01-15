using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.PaymentAgentSetupBradescoEdit
{
    public class Save : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAgentSetupBradesco mPaymentAgentSetupBradesco = (MPaymentAgentSetupBradesco)this.GetMessage(typeof(MPaymentAgentSetupBradesco));

            PaymentAgentSetupBradesco.Save(mPaymentAgentSetupBradesco);

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
