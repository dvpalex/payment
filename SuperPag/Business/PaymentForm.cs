using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace SuperPag.Business
{
    public class PaymentForm
    {
        public static MCPaymentForm List()
        {
            MCPaymentForm mcPaymentForm = null;
            DPaymentFormComplete[] arrDPaymentForm = DataFactory.PaymentForm().List();

            if (arrDPaymentForm != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcPaymentForm = (MCPaymentForm)mapper.Do(arrDPaymentForm, typeof(MCPaymentForm));
            }
            else
                mcPaymentForm = new MCPaymentForm();

            return mcPaymentForm;
        }

        public static MCPaymentForm ListNotUsePaymentForm(int storeId)
        {
            MCStorePaymentForm storePaymentForm = StorePaymentForm.ListByStore(storeId);

            int[] storePaymentFormsId = new int[storePaymentForm.Count];

            for (int i = 0; i < storePaymentForm.Count; i++)
            {
                storePaymentFormsId[i] = ((MStorePaymentForm)storePaymentForm[i]).PaymentFormId;
            }

            MCPaymentForm mcPaymentForm = null;
            DPaymentFormComplete[] arrDPaymentForm = DataFactory.PaymentForm().ListOtherPaymentForms(storePaymentFormsId);

            if (arrDPaymentForm != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcPaymentForm = (MCPaymentForm)mapper.Do(arrDPaymentForm, typeof(MCPaymentForm));

                //Coloco o nome do agente na frente do nome da forma de pagamento para melhor identificação
                foreach (MPaymentForm mPaymentForm in mcPaymentForm)
                {
                    mPaymentForm.Name = string.Format("{0} ({1})", mPaymentForm.Name, mPaymentForm.PaymentAgent.Name);
                }
            }
            else
                mcPaymentForm = new MCPaymentForm();


            return mcPaymentForm;
        }


        public static MCPaymentForm ListFromClient(int storeId)
        {
            MCPaymentForm mcPaymentForm = null;
            DPaymentForm[] arrDPaymentForm = DataFactory.PaymentForm().ListFromClient(storeId);

            if (arrDPaymentForm != null)
            {
                MessageMapper mapper = new MessageMapper();
                mcPaymentForm = (MCPaymentForm)mapper.Do(arrDPaymentForm, typeof(MCPaymentForm));
            }
            else
                mcPaymentForm = new MCPaymentForm();

            return mcPaymentForm;

        }

        public static MPaymentForm Locate(int storeId, string paymentFormName)
        {
            DPaymentForm dPaymentForm = DataFactory.PaymentForm().Locate(storeId, paymentFormName);

            MessageMapper mapper = new MessageMapper();
            MPaymentForm mPaymentForm = (MPaymentForm)mapper.Do(dPaymentForm, typeof(MPaymentForm));

            return mPaymentForm;

        }

        public static MPaymentForm Locate(int paymentFormId)
        {
            DPaymentForm dPaymentForm = DataFactory.PaymentForm().Locate(paymentFormId);

            MessageMapper mapper = new MessageMapper();
            MPaymentForm mPaymentForm = (MPaymentForm)mapper.Do(dPaymentForm, typeof(MPaymentForm));

            return mPaymentForm;

        }

        /// <summary>
        /// Description:	passo o numero do banco e me retorna a forma de pagamento
        /// </summary>
        /// <param name="NumSeqRemessa"></param>
        /// <returns></returns>
        public static string SelectPaymentFormId(string NumBanco,int StoreId)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_SelectPaymentFormId");

            db.AddInParameter(dbCommand, "@NumBanco", DbType.String, NumBanco);
            db.AddInParameter(dbCommand, "@StoreId", DbType.Int32, StoreId);            

            object strobject = db.ExecuteScalar(dbCommand);

            if (strobject == null)
            {
                throw new Exception("Numero de banco invàlido.");
            }
            return strobject.ToString();
        }
    }
}
