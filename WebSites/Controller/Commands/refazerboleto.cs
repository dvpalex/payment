using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;
using System.Web;

namespace Controller.Lib.Commands
{
    public class RefazerBoleto : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.Parameters["PaymentAttempt"];
            MPaymentAttemptBoleto mPaymentAttemptBoleto = PaymentAttemptBoleto.Locate(mPaymentAttempt.PaymentAttemptId);

            mPaymentAttemptBoleto.Instructions = mPaymentAttemptBoleto.Instructions;

            this.AddMessage(mPaymentAttempt);
            this.AddMessage(mPaymentAttemptBoleto);
            
            return Map.Views.RefazerBoleto;
		}
	}
}
