using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Helper;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag;

namespace Controller.Lib.Commands
{
    public class ShowStorePaymentInstallment : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.Parameters["StorePaymentForm"];

            mStorePaymentForm.StorePaymentInstallments = StorePaymentInstallment.List(mStorePaymentForm.StoreId, mStorePaymentForm.PaymentFormId);
            mStorePaymentForm.PaymentForm = PaymentForm.Locate(mStorePaymentForm.PaymentFormId);
            MStore mStore = Store.Locate(mStorePaymentForm.StoreId);


            this.AddMessage(mStorePaymentForm);
            this.AddMessage(mStore);

            return Map.Views.ShowStorePaymentForm;
		}
	}


}
