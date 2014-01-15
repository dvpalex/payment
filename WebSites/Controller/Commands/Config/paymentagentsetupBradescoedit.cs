using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetupBradesco : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int paymentAgentSetupId = (int)this.Parameters["PaymentAgentSetupId"];

            MPaymentAgentSetupBradesco mPaymentAgentSetupBradesco = PaymentAgentSetupBradesco.Locate(paymentAgentSetupId);

            if (mPaymentAgentSetupBradesco == null)
            {
                mPaymentAgentSetupBradesco = new MPaymentAgentSetupBradesco();
                mPaymentAgentSetupBradesco.PaymentAgentSetupId = paymentAgentSetupId;
            }

            this.AddMessage(mPaymentAgentSetupBradesco);

            return Map.Views.EditPaymentAgentSetupBradesco;
		}
	}


}
