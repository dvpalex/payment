using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;
using SuperPag.Data.Messages;
using Controller.Lib.Util;

namespace Controller.Lib.Views.Ev.OrderTransactionDetail
{
    public class GoBack : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
            b = this.MakeCommand(typeof(ShowOrder));
            b.Parameters["OrderId"] = mPaymentAttempt.Order.OrderId;

            return b;
        }
    }

    public class Reload : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
            b = this.MakeCommand(typeof(ShowOrderTransaction));
            b.Parameters["PaymentAttemptId"] = mPaymentAttempt.PaymentAttemptId;

            return b;
        }
    }
}
