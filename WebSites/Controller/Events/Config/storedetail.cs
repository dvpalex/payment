using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Web;
using SuperPag.Business.Messages;
using SuperPag.Business;
using Controller.Lib;
using Controller.Lib.Commands;
using SuperPag.Data.Messages;
using Controller.Lib.Util;

namespace Controller.Lib.Views.Ev.StoreDetail
{
    public class GoBack : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            b = this.CommandStack.GetBaseCommand("ListStores", true);

            return b;
        }
    }

    public class ShowStorePaymentInstallments : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;


            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.GetMessage(typeof(MStorePaymentForm));

            b = this.MakeCommand(typeof(ShowStorePaymentInstallment));
            b.Parameters["StorePaymentForm"] = mStorePaymentForm;

            return b;

        }
    }

    public class EditStorePaymentForm : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.GetMessage(typeof(MStorePaymentForm));
            
            b = this.MakeCommand(typeof(StorePaymentFormEdit));
            b.Parameters["StorePaymentForm"] = mStorePaymentForm;

            return b;
        }
    }

    public class StorePaymentFormInsert : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStore mStore = (MStore)this.GetMessage(typeof(MStore));

            b = this.MakeCommand(typeof(InsertStorePaymentForm));
            b.Parameters["StoreId"] = mStore.StoreId;

            return b;
        }
    }

    public class StoreEdit : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStore mStore = (MStore)this.GetMessage(typeof(MStore));

            b = this.MakeCommand(typeof(EditStore));
            b.Parameters["StoreId"] = mStore.StoreId;

            return b;
        }
    }

    public class ChangeHandshake : BaseEvent
    {
        protected override BaseCommand OnExecute()
        {
            BaseCommand b = null;

            MStore mStore = (MStore)this.GetMessage(typeof(MStore));

            b = this.MakeCommand(typeof(ListHandshakeConfiguration));
            b.Parameters["StoreId"] = mStore.StoreId;

            return b;
        }
    }

}
