using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
	public class ShowHome : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{

            MSummaryFilter mSummaryFilter = null;

            if (this.Parameters["SummaryFilter"] != null)
            {
                mSummaryFilter = (MSummaryFilter)this.Parameters["SummaryFilter"];
            }
            else
            {
                mSummaryFilter = new MSummaryFilter();
            }

            ControllerContext.StoreId = Users.StoreByUser(User.Identity.Name.ToLower());
            ControllerContext.UserName = User.Identity.Name.ToLower();

            MCPaymentSummary mcPaymentSummary = PaymentReports.PaymentSummary(ControllerContext.StoreId, mSummaryFilter.StartDate, mSummaryFilter.FinishDate);

            this.AddMessage(mSummaryFilter);
            this.AddMessage(mcPaymentSummary);

            return Map.Views.Home;
		}
	}

    [Serializable]
    public class MSummaryFilter : SuperPag.Framework.Message
    {
        private DateTime _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        private DateTime _finishDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        public DateTime FinishDate
        {
            get { return _finishDate; }
            set { _finishDate = value; }
        }
        
    }

}
