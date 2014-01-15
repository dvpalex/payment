using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Business;
using SuperPag.Agents.VBV;
using SuperPag.Framework;
using Controller.Lib.Util;
using SuperPag;

namespace Controller.Lib.Commands
{
    public class CapturaVBV : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.Parameters["PaymentAttempt"];
            VBV vbv = new VBV(mPaymentAttempt.PaymentAttemptId);
            vbv.Capture((int)PaymentAttemptVBVInterfaces.Controller);
            this.AddMessage(mPaymentAttempt);

            if(this.Parameters["LastView"].GetType().Equals(typeof(Controller.Lib.Commands.ListTransaction)))
                return Map.Views.ShowTransaction;
            else
                return Map.Views.ShowOrderTransaction;
		}
	}


}
