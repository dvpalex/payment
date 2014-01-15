using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetup
    {
        public static MCPaymentAgentSetup List()
        {
            MCPaymentAgentSetup mcPaymentAgentSetup = null;
            DPaymentAgentSetupComplete[] arrDPaymentAgentSetupComplete = DataFactory.PaymentAgentSetup().List();

            if (arrDPaymentAgentSetupComplete != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcPaymentAgentSetup = (MCPaymentAgentSetup)mapper.Do(arrDPaymentAgentSetupComplete, typeof(MCPaymentAgentSetup));
            }
            else
                mcPaymentAgentSetup = new MCPaymentAgentSetup();

            return mcPaymentAgentSetup;
        }

        public static MCPaymentAgentSetup List(int paymentAgentId)
        {
            MCPaymentAgentSetup mcPaymentAgentSetup = null;
            DPaymentAgentSetupComplete[] arrDPaymentAgentSetupComplete = DataFactory.PaymentAgentSetup().List(paymentAgentId);

            if (arrDPaymentAgentSetupComplete != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcPaymentAgentSetup = (MCPaymentAgentSetup)mapper.Do(arrDPaymentAgentSetupComplete, typeof(MCPaymentAgentSetup));
            }
            else
                mcPaymentAgentSetup = new MCPaymentAgentSetup();

            return mcPaymentAgentSetup;
        }

        public static MCPaymentAgentSetup ListNameWithId(int paymentAgentId)
        {
            MCPaymentAgentSetup mcPaymentAgentSetup = List(paymentAgentId);

            for (int i = 0; i < mcPaymentAgentSetup.Count; i++)
            {
                ((MPaymentAgentSetup)(mcPaymentAgentSetup[i])).Title = string.Format("{0} - {1}",
                    ((MPaymentAgentSetup)(mcPaymentAgentSetup[i])).PaymentAgentSetupId.ToString(),
                    ((MPaymentAgentSetup)(mcPaymentAgentSetup[i])).Title);

            }
            
            return mcPaymentAgentSetup;
        }

        public static void Save(MPaymentAgentSetup mPaymentAgentSetup)
        {
            if (mPaymentAgentSetup.PaymentAgentSetupId == int.MinValue)
            {
                MessageMapper mapper = new MessageMapper();
                DPaymentAgentSetup dPaymentAgentSetup = (DPaymentAgentSetup)mapper.Do(mPaymentAgentSetup, typeof(DPaymentAgentSetup));
                //Todo: tratar concorrência de processos
                dPaymentAgentSetup.paymentAgentSetupId = DataFactory.PaymentAgentSetup().MaxId().paymentAgentSetupId + 1;
                DataFactory.PaymentAgentSetup().Insert(dPaymentAgentSetup);
                mPaymentAgentSetup.PaymentAgentSetupId = dPaymentAgentSetup.paymentAgentSetupId;
            }
            else
            {
                MessageMapper mapper = new MessageMapper();
                DPaymentAgentSetup dPaymentAgentSetup = (DPaymentAgentSetup)mapper.Do(mPaymentAgentSetup, typeof(DPaymentAgentSetup));
                DataFactory.PaymentAgentSetup().Update(dPaymentAgentSetup);
            }
        }

        public static void Delete(int paymentAgentSetupId)
        {
            DataFactory.PaymentAgentSetup().Delete(paymentAgentSetupId);
        }



    }
}
