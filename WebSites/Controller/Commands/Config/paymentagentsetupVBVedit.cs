using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetupVBV : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int paymentAgentSetupId = (int)this.Parameters["PaymentAgentSetupId"];

            MPaymentAgentSetupVBV mPaymentAgentSetupVBV = PaymentAgentSetupVBV.Locate(paymentAgentSetupId);

            if (mPaymentAgentSetupVBV == null)
            {
                mPaymentAgentSetupVBV = new MPaymentAgentSetupVBV();
                mPaymentAgentSetupVBV.PaymentAgentSetupId = paymentAgentSetupId;
            }

            this.AddMessage(mPaymentAgentSetupVBV);

            return Map.Views.EditPaymentAgentSetupVBV;
		}
	}


}
