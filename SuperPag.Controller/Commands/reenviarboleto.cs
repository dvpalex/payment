using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class ReenviarBoleto : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.Parameters["PaymentAttempt"];
            string error = (string)this.Parameters["SendError"];

            this.AddMessage(mPaymentAttempt);
            this.AddValue(error, "SendError");

            return Map.Views.ReenviarBoleto;
		}
	}


}
