using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetupABN : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int paymentAgentSetupId = (int)this.Parameters["PaymentAgentSetupId"];

            MPaymentAgentSetupABN mPaymentAgentSetupABN = PaymentAgentSetupABN.Locate(paymentAgentSetupId);

            if (mPaymentAgentSetupABN == null)
            {
                mPaymentAgentSetupABN = new MPaymentAgentSetupABN();
                mPaymentAgentSetupABN.PaymentAgentSetupId = paymentAgentSetupId;
            }

            this.AddMessage(mPaymentAgentSetupABN);

            return Map.Views.EditPaymentAgentSetupABN;
		}
	}


}
