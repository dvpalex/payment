using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditPaymentAgentSetupPaymentclientvirtual : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            int paymentAgentSetupId = (int)this.Parameters["PaymentAgentSetupId"];

            MPaymentAgentSetupPaymentClientVirtual mPaymentAgentSetupPaymentclientvirtual = PaymentAgentSetupPaymentClientVirtual.Locate(paymentAgentSetupId);

            if (mPaymentAgentSetupPaymentclientvirtual == null)
            {
                mPaymentAgentSetupPaymentclientvirtual = new MPaymentAgentSetupPaymentClientVirtual();
                mPaymentAgentSetupPaymentclientvirtual.PaymentAgentSetupId = paymentAgentSetupId;
            }

            this.AddMessage(mPaymentAgentSetupPaymentclientvirtual);

            return Map.Views.EditPaymentAgentSetupPaymentClientVirtual;
		}
	}


}
