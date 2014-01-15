using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class EditStorePaymentInstallments : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MStorePaymentInstallment mStorePaymentInstallment = (MStorePaymentInstallment)this.Parameters["StorePaymentInstallment"];

            this.AddMessage(mStorePaymentInstallment);
            this.AddEnumeration(new EnumListBuilder(typeof(MStorePaymentInstallment.InstallmentTypeEnum)));

            return Map.Views.EditStorePaymentInstallment;
		}
	}

    public class StorePaymentInstallmentsInsert : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)this.Parameters["StorePaymentForm"];

            MStorePaymentInstallment mStorePaymentInstallment = new MStorePaymentInstallment();
            mStorePaymentInstallment.StoreId = mStorePaymentForm.StoreId;
            mStorePaymentInstallment.PaymentFormId = mStorePaymentForm.PaymentFormId;
            mStorePaymentInstallment.MinValue = 0;
            mStorePaymentInstallment.MaxValue = 1000000000;
            mStorePaymentInstallment.InterestPercentage = 0;
            mStorePaymentInstallment.InstallmentType = MStorePaymentInstallment.InstallmentTypeEnum.Lojista;
            mStorePaymentInstallment.AllowInParcialPayment = false;
            mStorePaymentInstallment.IsNew = true;

            this.AddMessage(mStorePaymentInstallment);
            this.AddEnumeration(new EnumListBuilder(typeof(MStorePaymentInstallment.InstallmentTypeEnum)));

            return Map.Views.EditStorePaymentInstallment;
        }
    }

}
