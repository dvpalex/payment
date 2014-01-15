using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetupBB
    {
        public static MPaymentAgentSetupBB Locate(int paymentAgentSetupId)
        {
            DPaymentAgentSetupBB dPaymentAgentSetupBB = DataFactory.PaymentAgentSetupBB().Locate(paymentAgentSetupId);
        
            MessageMapper mapper = new MessageMapper();
            MPaymentAgentSetupBB mPaymentAgentSetupBB = (MPaymentAgentSetupBB)mapper.Do(dPaymentAgentSetupBB, typeof(MPaymentAgentSetupBB));
            return mPaymentAgentSetupBB;
        }

        public static void Save(MPaymentAgentSetupBB mPaymentAgentSetupBB)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAgentSetupBB dPaymentAgentSetupBB = (DPaymentAgentSetupBB)mapper.Do(mPaymentAgentSetupBB, typeof(DPaymentAgentSetupBB));

            if (DataFactory.PaymentAgentSetupBB().Locate(mPaymentAgentSetupBB.PaymentAgentSetupId) == null)
            {
                DataFactory.PaymentAgentSetupBB().Insert(dPaymentAgentSetupBB);
            }
            else
            {
                DataFactory.PaymentAgentSetupBB().Update(dPaymentAgentSetupBB);
            }
        }
    }    
}
