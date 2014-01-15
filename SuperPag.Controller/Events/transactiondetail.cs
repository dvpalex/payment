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

namespace Controller.Lib.Views.Ev.TransactionDetail
{
    public class GoBack : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.LastView();

            return b;
        }
    }

    public class Reload : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
            b = this.MakeCommand(typeof(ShowTransaction));
            b.Parameters["PaymentAttemptId"] = mPaymentAttempt.PaymentAttemptId;

            return b;
        }
    }

    public class Alterar : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));

            if (mPaymentAttempt.Order.Status == MOrder.OrderStatus.Cancelled && (mPaymentAttempt.PaymentForm.PaymentAgentId == (int)SuperPag.PaymentAgents.VBV || mPaymentAttempt.PaymentForm.PaymentAgentId == (int)SuperPag.PaymentAgents.VBVInBox))
            {
                SuperPag.Agents.VBV.VBV vbv = new SuperPag.Agents.VBV.VBV(mPaymentAttempt.PaymentAttemptId);
                vbv.CancelCapture();
            }

            if (mPaymentAttempt.Order.Status == MOrder.OrderStatus.Cancelled || mPaymentAttempt.Order.Status == MOrder.OrderStatus.Approved)
            {
                mPaymentAttempt.Order.StatusChangeUserId = Users.Locate(ControllerContext.UserName);
                mPaymentAttempt.Order.StatusChangeDate = DateTime.Now;
            }

            mPaymentAttempt.Order.LastUpdateDate = DateTime.Now;
            Order.Update(mPaymentAttempt.Order);

            b = this.MakeCommand(typeof(ShowTransaction));
            b.Parameters["PaymentAttemptId"] = mPaymentAttempt.PaymentAttemptId;

            return b;
        }
    }

    public class EditBoleto : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
            b = this.MakeCommand(typeof(RefazerBoleto));
            b.Parameters["PaymentAttempt"] = mPaymentAttempt;

            return b;
        }
    }

    public class EnviaBoleto : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
            b = this.MakeCommand(typeof(ReenviarBoleto));
            b.Parameters["PaymentAttempt"] = mPaymentAttempt;
            b.Parameters["SendError"] = "";

            return b;
        }
    }

    public class CapturaVBV : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;
            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)this.GetMessage(typeof(MPaymentAttempt));
            b = this.MakeCommand(typeof(Controller.Lib.Commands.CapturaVBV));
            b.Parameters["PaymentAttempt"] = mPaymentAttempt;
            b.Parameters["LastView"] = this.LastView();
            return b;
        }
    }

}
