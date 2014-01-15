using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetup : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MPaymentAgentSetup mPaymentAgentSetup = (MPaymentAgentSetup)this.Parameters["PaymentAgentSetup"];

            this.AddMessage(mPaymentAgentSetup);
            this.AddMessage(PaymentAgent.List());

            return Map.Views.EditPaymentAgentSetup;
		}
	}

    public class InsertPaymentAgentSetup : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            MPaymentAgentSetup mPaymentAgentSetup = new MPaymentAgentSetup();

            this.AddMessage(mPaymentAgentSetup);
            this.AddMessage(PaymentAgent.List());

            return Map.Views.EditPaymentAgentSetup;
        }
    }

}
