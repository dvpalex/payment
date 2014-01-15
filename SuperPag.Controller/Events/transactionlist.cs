using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.TransactionList
{
    public class SelectTransaction : BaseEvent
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

    public class GoBack : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.LastView();

            return b;
        }
    }

    public class SortList : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MTransactionSearch mTransactionSearch = (MTransactionSearch)this.GetMessage(typeof(MTransactionSearch));
            string indexField = (string)this.Parameters["IndexField"];
            b = this.MakeCommand(typeof(ListTransaction));
            b.Parameters["IndexField"] = indexField;
            b.Parameters["TransactionSearch"] = mTransactionSearch;

            return b;
        }
    }
}
