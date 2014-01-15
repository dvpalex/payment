using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetupKomerci : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int paymentAgentSetupId = (int)this.Parameters["PaymentAgentSetupId"];

            MPaymentAgentSetupKomerci mPaymentAgentSetupKomerci = PaymentAgentSetupKomerci.Locate(paymentAgentSetupId);

            if (mPaymentAgentSetupKomerci == null)
            {
                mPaymentAgentSetupKomerci = new MPaymentAgentSetupKomerci();
                mPaymentAgentSetupKomerci.PaymentAgentSetupId = paymentAgentSetupId;
            }

            this.AddMessage(mPaymentAgentSetupKomerci);

            return Map.Views.EditPaymentAgentSetupKomerci;
		}
	}


}
