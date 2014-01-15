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

namespace Controller.Lib.Views.Ev.OrderDetail
{
    public class GoBack : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.CommandStack.GetBaseCommand("ListOrder", true);

            return b;
        }
    }

    public class Reload : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MOrder mOrder = (MOrder)this.GetMessage(typeof(MOrder));
            b = this.MakeCommand(typeof(ShowOrder));
            b.Parameters["OrderId"] = mOrder.OrderId;

            return b;
        }
    }

    public class ShowOrderTransactionDetail : BaseEvent
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

    public class Alterar : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MOrder mOrder = (MOrder)this.GetMessage(typeof(MOrder));

            if (mOrder.Status == MOrder.OrderStatus.Cancelled)
            {
                MCPaymentAttempt mcPaymentAttempt = PaymentAttempt.List(mOrder.OrderId);

                if (mcPaymentAttempt != null)
                {
                    foreach (MPaymentAttempt mPaymentAttempt in mcPaymentAttempt)
                    {
                        if (mPaymentAttempt.PaymentForm.PaymentAgentId == (int)SuperPag.PaymentAgents.VBV || mPaymentAttempt.PaymentForm.PaymentAgentId == (int)SuperPag.PaymentAgents.VBVInBox)
                        {
                            SuperPag.Agents.VBV.VBV vbv = new SuperPag.Agents.VBV.VBV(mPaymentAttempt.PaymentAttemptId);
                            vbv.CancelCapture();
                        }
                    }
                }
            }

            if (mOrder.Status == MOrder.OrderStatus.Cancelled || mOrder.Status == MOrder.OrderStatus.Approved)
            {
                mOrder.StatusChangeUserId = Users.Locate(ControllerContext.UserName);
                mOrder.StatusChangeDate = DateTime.Now;
            }

            mOrder.LastUpdateDate = DateTime.Now;
            Order.Update(mOrder);

            b = this.MakeCommand(typeof(ShowOrder));
            b.Parameters["OrderId"] = mOrder.OrderId;

            return b;
        }
    }
}
