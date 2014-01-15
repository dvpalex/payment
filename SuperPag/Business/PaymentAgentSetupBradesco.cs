using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetupBradesco
    {
        public static MPaymentAgentSetupBradesco Locate(int paymentAgentSetupId)
        {
            DPaymentAgentSetupBradesco dPaymentAgentSetupBradesco = DataFactory.PaymentAgentSetupBradesco().Locate(paymentAgentSetupId);
        
            MessageMapper mapper = new MessageMapper();
            MPaymentAgentSetupBradesco mPaymentAgentSetupBradesco = (MPaymentAgentSetupBradesco)mapper.Do(dPaymentAgentSetupBradesco, typeof(MPaymentAgentSetupBradesco));
            return mPaymentAgentSetupBradesco;
        }

        public static void Save(MPaymentAgentSetupBradesco mPaymentAgentSetupBradesco)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAgentSetupBradesco dPaymentAgentSetupBradesco = (DPaymentAgentSetupBradesco)mapper.Do(mPaymentAgentSetupBradesco, typeof(DPaymentAgentSetupBradesco));

            if (DataFactory.PaymentAgentSetupBradesco().Locate(mPaymentAgentSetupBradesco.PaymentAgentSetupId) == null)
            {
                DataFactory.PaymentAgentSetupBradesco().Insert(dPaymentAgentSetupBradesco);
            }
            else
            {
                DataFactory.PaymentAgentSetupBradesco().Update(dPaymentAgentSetupBradesco);
            }
        }
    }    
}
