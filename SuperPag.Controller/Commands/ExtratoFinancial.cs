using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
	public class ExtratoFinancial : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            DateTime startDate = (DateTime)base.Parameters["startDate"];
            DateTime endDate = (DateTime)base.Parameters["endDate"];
            int[] payments = (int[])base.Parameters["payementFormId"];

            //lista o extrato
            MCExtrato mCExtrato = SuperPag.Business.Extrato.ListFinancial(ControllerContext.StoreId, startDate, endDate, payments);

            decimal searchValue = 0m;
            foreach (MExtrato mExtrato in mCExtrato)
            {
                searchValue += mExtrato.Value;
            }

            decimal totalPaid, totalNonPaid;

            //para obter o valor anterior, eu passo a data inicial como data final no calculo do total
            SuperPag.Business.Extrato.CalculateTotal(ControllerContext.StoreId, DateTime.MinValue, startDate.AddSeconds(-1), out totalPaid, out totalNonPaid, payments);

            base.AddValue(startDate.AddMonths(-1), "previousDate");
            base.AddValue(searchValue, "searchValue");
            base.AddValue(mCExtrato.Count, "searchRecords");
            base.AddValue(totalPaid, "previousTotal");
            base.AddValue(startDate, "startDate");
            base.AddValue(endDate, "endDate");
            base.AddMessage(mCExtrato);

            return Map.Views.ExtratoFinancial;
		}
	}

}
