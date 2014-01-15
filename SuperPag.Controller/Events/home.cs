using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;
using Controller.Lib.Util;

namespace Controller.Lib.Views.Ev.Home
{
    public class Update : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            Controller.Lib.Commands.MSummaryFilter mSummaryFilter = (MSummaryFilter)this.GetMessage(typeof(MSummaryFilter));
            mSummaryFilter.FinishDate = mSummaryFilter.FinishDate.AddHours(23);
            mSummaryFilter.FinishDate = mSummaryFilter.FinishDate.AddMinutes(59);
            mSummaryFilter.FinishDate = mSummaryFilter.FinishDate.AddSeconds(59);
            b = this.MakeCommand(typeof(ShowHome));
            b.Parameters["SummaryFilter"] = mSummaryFilter;

            return b;
        }
    }

    public class List : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            Controller.Lib.Commands.MSummaryFilter mSummaryFilter = (MSummaryFilter)this.GetMessage(typeof(MSummaryFilter));
            mSummaryFilter.FinishDate = mSummaryFilter.FinishDate.AddHours(23);
            mSummaryFilter.FinishDate = mSummaryFilter.FinishDate.AddMinutes(59);
            mSummaryFilter.FinishDate = mSummaryFilter.FinishDate.AddSeconds(59);

            MTransactionSearch mTransactionSearch = new MTransactionSearch();
            mTransactionSearch.OrderDateFrom = mSummaryFilter.StartDate;
            mTransactionSearch.OrderDateTo = mSummaryFilter.FinishDate;
            mTransactionSearch.Status = (MPaymentAttempt.PaymentAttemptStatus)this.Parameters["Status"];
            mTransactionSearch.PaymentFormId = PaymentForm.Locate(ControllerContext.StoreId, (string)this.Parameters["Name"]).PaymentFormId;

            b = this.MakeCommand(typeof(ListTransaction));
            b.Parameters["TransactionSearch"] = mTransactionSearch;

            return b;
        }
    }
}
