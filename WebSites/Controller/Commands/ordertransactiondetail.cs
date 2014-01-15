using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Helper;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag;

namespace Controller.Lib.Commands
{
	public class ShowOrderTransaction : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            Guid paymentAttemptId = (Guid)this.Parameters["PaymentAttemptId"];

            MPaymentAttempt mPaymentAttempt = PaymentReports.Locate(paymentAttemptId);
         
            this.AddEnumeration(new EnumListBuilder(typeof(OrderStatusForCombo)));
            this.AddMessage(mPaymentAttempt);

            return Map.Views.ShowOrderTransaction;
		}
	}


}
