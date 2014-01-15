using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetupPaymentClientVirtual
    {
        public static MPaymentAgentSetupPaymentClientVirtual Locate(int paymentAgentSetupId)
        {
            DPaymentAgentSetupPaymentClientVirtual dPaymentAgentSetupPaymentClientVirtual = DataFactory.PaymentAgentSetupPaymentClientVirtual().Locate(paymentAgentSetupId);
        
            MessageMapper mapper = new MessageMapper();
            MPaymentAgentSetupPaymentClientVirtual mPaymentAgentSetupPaymentClientVirtual = (MPaymentAgentSetupPaymentClientVirtual)mapper.Do(dPaymentAgentSetupPaymentClientVirtual, typeof(MPaymentAgentSetupPaymentClientVirtual));
            return mPaymentAgentSetupPaymentClientVirtual;
        }

        public static void Save(MPaymentAgentSetupPaymentClientVirtual mPaymentAgentSetupPaymentClientVirtual)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAgentSetupPaymentClientVirtual dPaymentAgentSetupPaymentClientVirtual = (DPaymentAgentSetupPaymentClientVirtual)mapper.Do(mPaymentAgentSetupPaymentClientVirtual, typeof(DPaymentAgentSetupPaymentClientVirtual));

            if (DataFactory.PaymentAgentSetupPaymentClientVirtual().Locate(mPaymentAgentSetupPaymentClientVirtual.PaymentAgentSetupId) == null)
            {
                DataFactory.PaymentAgentSetupPaymentClientVirtual().Insert(dPaymentAgentSetupPaymentClientVirtual);
            }
            else
            {
                DataFactory.PaymentAgentSetupPaymentClientVirtual().Update(dPaymentAgentSetupPaymentClientVirtual);
            }
        }
    }    
}
