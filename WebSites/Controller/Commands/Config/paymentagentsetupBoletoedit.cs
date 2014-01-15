using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetupBoleto : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int paymentAgentSetupId = (int)this.Parameters["PaymentAgentSetupId"];

            MPaymentAgentSetupBoleto mPaymentAgentSetupBoleto = PaymentAgentSetupBoleto.Locate(paymentAgentSetupId);

            if (mPaymentAgentSetupBoleto == null)
            {
                mPaymentAgentSetupBoleto = new MPaymentAgentSetupBoleto();
                mPaymentAgentSetupBoleto.PaymentAgentSetupId = paymentAgentSetupId;
            }

            this.AddMessage(mPaymentAgentSetupBoleto);

            return Map.Views.EditPaymentAgentSetupBoleto;
		}
	}


}
