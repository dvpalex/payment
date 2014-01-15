using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class StorePaymentForm
    {
        public static MCStorePaymentForm ListByStore(int storeId)
        {
            MCStorePaymentForm mcStorePaymentForm = null;
            DStorePaymentFormComplete[] arrDStorePaymentFormComplete = DataFactory.StorePaymentForm().ListCompleteByStore(storeId);

            if (arrDStorePaymentFormComplete != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcStorePaymentForm = (MCStorePaymentForm)mapper.Do(arrDStorePaymentFormComplete, typeof(MCStorePaymentForm));
            }
            else
                mcStorePaymentForm = new MCStorePaymentForm();

            return mcStorePaymentForm;
        }

        public static MStorePaymentForm Locate(int storeId, int paymentFormId)
        {
            DStorePaymentForm dStorePaymentForm = DataFactory.StorePaymentForm().Locate(storeId, paymentFormId);

            MessageMapper mapper = new MessageMapper();
            MStorePaymentForm mStorePaymentForm = (MStorePaymentForm)mapper.Do(dStorePaymentForm, typeof(MStorePaymentForm));

            return mStorePaymentForm;
        }


        public static void Save(MStorePaymentForm mStorePaymentForm)
        {
            if (Locate(mStorePaymentForm.StoreId, mStorePaymentForm.PaymentFormId) == null)
            {
                //Insert
                MessageMapper mapper = new MessageMapper();
                DStorePaymentForm dStorePaymentForm = (DStorePaymentForm)mapper.Do(mStorePaymentForm, typeof(DStorePaymentForm));
                DataFactory.StorePaymentForm().Insert(dStorePaymentForm);
            }
            else
            {
                //Update
                MessageMapper mapper = new MessageMapper();
                DStorePaymentForm dStorePaymentForm = (DStorePaymentForm)mapper.Do(mStorePaymentForm, typeof(DStorePaymentForm));
                DataFactory.StorePaymentForm().Update(dStorePaymentForm);
            }
        }

        public static void Delete(int storeId, int paymentFormId)
        {
            DataFactory.StorePaymentForm().Delete(storeId, paymentFormId);
        }



    }
}
