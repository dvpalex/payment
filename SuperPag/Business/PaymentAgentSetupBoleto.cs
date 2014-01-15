using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetupBoleto
    {
        public static MPaymentAgentSetupBoleto Locate(int paymentAgentSetupId)
        {
            DPaymentAgentSetupBoleto dPaymentAgentSetupBoleto = DataFactory.PaymentAgentSetupBoleto().Locate(paymentAgentSetupId);
        
            MessageMapper mapper = new MessageMapper();
            MPaymentAgentSetupBoleto mPaymentAgentSetupBoleto = (MPaymentAgentSetupBoleto)mapper.Do(dPaymentAgentSetupBoleto, typeof(MPaymentAgentSetupBoleto));
            return mPaymentAgentSetupBoleto;
        }

        public static void Save(MPaymentAgentSetupBoleto mPaymentAgentSetupBoleto)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAgentSetupBoleto dPaymentAgentSetupBoleto = (DPaymentAgentSetupBoleto)mapper.Do(mPaymentAgentSetupBoleto, typeof(DPaymentAgentSetupBoleto));

            if (DataFactory.PaymentAgentSetupBoleto().Locate(mPaymentAgentSetupBoleto.PaymentAgentSetupId) == null)
            {
                DataFactory.PaymentAgentSetupBoleto().Insert(dPaymentAgentSetupBoleto);
            }
            else
            {
                DataFactory.PaymentAgentSetupBoleto().Update(dPaymentAgentSetupBoleto);
            }
        }
    }    
}
