using System;
using System.Collections;
using SuperPag;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    //Extrato Financeiro Herbalife
    public class ExtratoFinancial2 : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            DateTime startDate = (DateTime)base.Parameters["startDate"];
            DateTime endDate = (DateTime)base.Parameters["endDate"];

            MCExtrato2 mCExtrato = SuperPag.Business.Extrato.ListFinancial2(ControllerContext.StoreId, startDate, endDate);
            base.AddMessage(mCExtrato);

            return Map.Views.ExtratoFinancial2;
		}
	}

}
