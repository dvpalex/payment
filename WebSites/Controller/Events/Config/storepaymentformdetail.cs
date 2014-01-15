using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.StorePaymentFormDetail
{
    public class InsertNewInstallment : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.GetMessage(typeof(MStorePaymentForm));
            
            b = this.MakeCommand(typeof(StorePaymentInstallmentsInsert));
            b.Parameters["StorePaymentForm"] = mStorePaymentForm;

            return b;
        }
    }

    public class InstallmentsEdit : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStorePaymentInstallment mStorePaymentInstallment = (MStorePaymentInstallment)this.GetMessage(typeof(MStorePaymentInstallment));

            b = this.MakeCommand(typeof(EditStorePaymentInstallments));
            b.Parameters["StorePaymentInstallment"] = mStorePaymentInstallment;

            return b;
        }
    }

    public class GoBack : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.GetMessage(typeof(MStorePaymentForm));

            b = this.MakeCommand(typeof(ShowStore));
            b.Parameters["StoreId"] = mStorePaymentForm.StoreId;

            return b;
        }
    }

}
