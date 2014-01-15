using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class ExtratoMovement : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            DateTime startDate = (DateTime)base.Parameters["startDate"];
            DateTime endDate = (DateTime)base.Parameters["endDate"];
            int[] payments = (int[])base.Parameters["payementFormId"];
            MCExtrato mCExtrato = SuperPag.Business.Extrato.ListMovement(ControllerContext.StoreId, startDate, endDate, payments);

            decimal searchPaidValue = 0m;
            decimal searchNonPaidValue = 0m;
            foreach (MExtrato mExtrato in mCExtrato)
            {            
                if (mExtrato.Status == (int)SuperPag.Business.Messages.MPaymentAttempt.PaymentAttemptStatus.Paid)
                {
                    searchPaidValue += mExtrato.Value;
                }
                else
                {
                    searchNonPaidValue += mExtrato.Value;
                }

            }

            decimal totalPaid, totalNonPaid;

            SuperPag.Business.Extrato.CalculateTotal(ControllerContext.StoreId, DateTime.MinValue, DateTime.MinValue, out totalPaid, out totalNonPaid, payments);

            base.AddValue(searchPaidValue, "searchPaidValue");
            base.AddValue(searchNonPaidValue, "searchNonPaidValue");
            base.AddValue(mCExtrato.Count,"searchRecords");
            base.AddMessage(mCExtrato);
            base.AddValue(totalPaid, "totalPaid");
            base.AddValue(totalNonPaid, "totalNonPaid");

            return Map.Views.ExtratoMovement;
        }
    }


}
