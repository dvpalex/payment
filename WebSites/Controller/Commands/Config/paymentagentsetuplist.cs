using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class ListPaymentAgentSetup : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MCPaymentAgentSetup mcPaymentAgentSetup = PaymentAgentSetup.List();

            this.AddMessage(mcPaymentAgentSetup);

            return Map.Views.ListPaymentAgentSetup;
		}
	}


}
