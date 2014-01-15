using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.PaymentAgentSetupBoletoEdit
{
    public class Save : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAgentSetupBoleto mPaymentAgentSetupBoleto = (MPaymentAgentSetupBoleto)this.GetMessage(typeof(MPaymentAgentSetupBoleto));

            PaymentAgentSetupBoleto.Save(mPaymentAgentSetupBoleto);

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
