using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetupMoset : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int paymentAgentSetupId = (int)this.Parameters["PaymentAgentSetupId"];

            MPaymentAgentSetupMoset mPaymentAgentSetupMoset = PaymentAgentSetupMoset.Locate(paymentAgentSetupId);

            if (mPaymentAgentSetupMoset == null)
            {
                mPaymentAgentSetupMoset = new MPaymentAgentSetupMoset();
                mPaymentAgentSetupMoset.PaymentAgentSetupId = paymentAgentSetupId;
            }

            this.AddMessage(mPaymentAgentSetupMoset);

            return Map.Views.EditPaymentAgentSetupMoset;
		}
	}


}
