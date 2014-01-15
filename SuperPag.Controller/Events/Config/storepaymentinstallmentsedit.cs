using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.StorePaymentInstallmentsEdit
{
    public class Save : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStorePaymentInstallment mStorePaymentInstallment = (MStorePaymentInstallment)this.GetMessage(typeof(MStorePaymentInstallment));

            StorePaymentInstallment.Save(mStorePaymentInstallment);

            MStorePaymentForm mStorePaymentForm = StorePaymentForm.Locate(mStorePaymentInstallment.StoreId, mStorePaymentInstallment.PaymentFormId);

            b = this.MakeCommand(typeof(ShowStorePaymentInstallment));
            b.Parameters["StorePaymentForm"] = mStorePaymentForm;

            return b;
        }
    }

    public class Cancel : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.LastView();

            return b;
        }
    }

    public class Delete : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStorePaymentInstallment mStorePaymentInstallment = (MStorePaymentInstallment)this.GetMessage(typeof(MStorePaymentInstallment));

            MStorePaymentForm mStorePaymentForm = StorePaymentForm.Locate(mStorePaymentInstallment.StoreId, mStorePaymentInstallment.PaymentFormId);

            b = this.MakeCommand(typeof(ShowStorePaymentInstallment));
            b.Parameters["StorePaymentForm"] = mStorePaymentForm;

            StorePaymentInstallment.Delete(mStorePaymentInstallment.StoreId, mStorePaymentInstallment.PaymentFormId, mStorePaymentInstallment.InstallmentNumber);

            return b;
        }
    }

}
