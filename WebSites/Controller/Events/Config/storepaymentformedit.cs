using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;

namespace Controller.Lib.Views.Ev.EditStorePaymentForm
{
    public class Save : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.GetMessage(typeof(MStorePaymentForm));

            StorePaymentForm.Save(mStorePaymentForm);

            BaseCommand b = null;

            b = this.MakeCommand(typeof(ShowStore));
            b.Parameters["StoreId"] = mStorePaymentForm.StoreId;
            return b;
        }
    }

    public class Cancel : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.GetMessage(typeof(MStorePaymentForm));

            BaseCommand b = null;

            b = this.MakeCommand(typeof(ShowStore));
            b.Parameters["StoreId"] = mStorePaymentForm.StoreId;
            return b;
        }
    }

    public class Delete : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.GetMessage(typeof(MStorePaymentForm));

            BaseCommand b = null;

            b = this.MakeCommand(typeof(ShowStore));
            b.Parameters["StoreId"] = mStorePaymentForm.StoreId;

            StorePaymentForm.Delete(mStorePaymentForm.StoreId, mStorePaymentForm.PaymentFormId);

            return b;
        }
    }

    public class Reload : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.GetMessage(typeof(MStorePaymentForm));
            mStorePaymentForm.PaymentFormId = (int)this.Parameters["PaymentFormId"];

            b = this.MakeCommand(typeof(InsertStorePaymentForm));
            b.Parameters["StorePaymentForm"] = mStorePaymentForm;

            return b;
        }
    }

}
