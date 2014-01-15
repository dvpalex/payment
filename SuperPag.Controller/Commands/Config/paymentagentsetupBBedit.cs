using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetupBB : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int paymentAgentSetupId = (int)this.Parameters["PaymentAgentSetupId"];

            MPaymentAgentSetupBB mPaymentAgentSetupBB = PaymentAgentSetupBB.Locate(paymentAgentSetupId);

            if (mPaymentAgentSetupBB == null)
            {
                mPaymentAgentSetupBB = new MPaymentAgentSetupBB();
                mPaymentAgentSetupBB.PaymentAgentSetupId = paymentAgentSetupId;
            }

            this.AddMessage(mPaymentAgentSetupBB);

            return Map.Views.EditPaymentAgentSetupBB;
		}
	}


}
