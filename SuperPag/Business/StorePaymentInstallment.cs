using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class StorePaymentInstallment
    {
        public static MCStorePaymentInstallment List(int storeId, int paymentFormId)
        {
            MCStorePaymentInstallment mcStorePaymentInstallment = null;
            DStorePaymentInstallment[] arrDStorePaymentInstallment = DataFactory.StorePaymentInstallment().List(storeId, paymentFormId);

            if (arrDStorePaymentInstallment != null)
            {
                MessageMapper mapper = new MessageMapper();
                mcStorePaymentInstallment = (MCStorePaymentInstallment)mapper.Do(arrDStorePaymentInstallment, typeof(MCStorePaymentInstallment));
            }
            else
                mcStorePaymentInstallment = new MCStorePaymentInstallment();

            return mcStorePaymentInstallment;
        }

        public static MStorePaymentInstallment Locate(int storeId, int paymentFormId, int installmentNumber)
        {
            DStorePaymentInstallment dStorePaymentInstallment = DataFactory.StorePaymentInstallment().Locate(storeId, paymentFormId, installmentNumber);

            MessageMapper mapper = new MessageMapper();
            MStorePaymentInstallment mStorePaymentInstallment = (MStorePaymentInstallment)mapper.Do(dStorePaymentInstallment, typeof(MStorePaymentInstallment));

            return mStorePaymentInstallment;
        }


        public static void Save(MStorePaymentInstallment mStorePaymentInstallment)
        {
            if (mStorePaymentInstallment.IsNew)
            {
                //Insert
                MessageMapper mapper = new MessageMapper();
                DStorePaymentInstallment dStorePaymentInstallment = (DStorePaymentInstallment)mapper.Do(mStorePaymentInstallment, typeof(DStorePaymentInstallment));
                DataFactory.StorePaymentInstallment().Insert(dStorePaymentInstallment);
            }
            else
            {
                //Update
                MessageMapper mapper = new MessageMapper();
                DStorePaymentInstallment dStorePaymentInstallment = (DStorePaymentInstallment)mapper.Do(mStorePaymentInstallment, typeof(DStorePaymentInstallment));
                DataFactory.StorePaymentInstallment().Update(dStorePaymentInstallment);
            }
        }

        public static void Delete(int storeId, int paymentFormId, int installmentNumber)
        {
            DataFactory.StorePaymentInstallment().Delete(storeId, paymentFormId, installmentNumber);
        }
    }
}
