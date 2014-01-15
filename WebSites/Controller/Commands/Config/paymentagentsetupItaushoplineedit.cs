using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetupItaushopline : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int paymentAgentSetupId = (int)this.Parameters["PaymentAgentSetupId"];

            MPaymentAgentSetupItauShopline mPaymentAgentSetupItauShopline = PaymentAgentSetupItauShopline.Locate(paymentAgentSetupId);

            if (mPaymentAgentSetupItauShopline == null)
            {
                mPaymentAgentSetupItauShopline = new MPaymentAgentSetupItauShopline();
                mPaymentAgentSetupItauShopline.PaymentAgentSetupId = paymentAgentSetupId;
            }

            this.AddMessage(mPaymentAgentSetupItauShopline);

            return Map.Views.EditPaymentAgentSetupItauShopLine;
		}
	}


}
