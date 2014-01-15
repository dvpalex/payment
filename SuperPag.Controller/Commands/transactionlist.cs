using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
	public class ListTransaction : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MTransactionSearch mTransactionSearch = (MTransactionSearch)this.Parameters["TransactionSearch"];

            MCPaymentAttempt mcPaymentAttempt = null;

            if (mTransactionSearch.PaymentAttemptId != Guid.Empty)
            {
                mcPaymentAttempt = PaymentReports.PaymentList(mTransactionSearch.PaymentAttemptId);
            }
            else if (mTransactionSearch.StoreId != int.MinValue && mTransactionSearch.StoreReferenceOrder != null)
            {
                mcPaymentAttempt = PaymentReports.PaymentList(mTransactionSearch.StoreId, mTransactionSearch.StoreReferenceOrder);
            }
            else
            {
                mcPaymentAttempt = PaymentReports.PaymentList(ControllerContext.StoreId, mTransactionSearch.OrderDateFrom, mTransactionSearch.OrderDateTo, mTransactionSearch.PaymentFormId,
                    mTransactionSearch.Status, mTransactionSearch.ConsumerName, mTransactionSearch.Cpf, mTransactionSearch.Cnpj, mTransactionSearch.OrderStatus);
            }

            string indexField = "StartTime";
            if (this.Parameters["IndexField"] != null)
                indexField = (string)this.Parameters["IndexField"];

            mcPaymentAttempt.sort(indexField, false);

            this.AddMessage(mcPaymentAttempt);

            return Map.Views.ListTransaction;
		}
	}


}
