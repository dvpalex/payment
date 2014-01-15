using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetupMoset
    {
        public static MPaymentAgentSetupMoset Locate(int paymentAgentSetupId)
        {
            DPaymentAgentSetupMoset dPaymentAgentSetupMoset = DataFactory.PaymentAgentSetupMoset().Locate(paymentAgentSetupId);
        
            MessageMapper mapper = new MessageMapper();
            MPaymentAgentSetupMoset mPaymentAgentSetupMoset = (MPaymentAgentSetupMoset)mapper.Do(dPaymentAgentSetupMoset, typeof(MPaymentAgentSetupMoset));
            return mPaymentAgentSetupMoset;
        }

        public static void Save(MPaymentAgentSetupMoset mPaymentAgentSetupMoset)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAgentSetupMoset dPaymentAgentSetupMoset = (DPaymentAgentSetupMoset)mapper.Do(mPaymentAgentSetupMoset, typeof(DPaymentAgentSetupMoset));

            if (DataFactory.PaymentAgentSetupMoset().Locate(mPaymentAgentSetupMoset.PaymentAgentSetupId) == null)
            {
                DataFactory.PaymentAgentSetupMoset().Insert(dPaymentAgentSetupMoset);
            }
            else
            {
                DataFactory.PaymentAgentSetupMoset().Update(dPaymentAgentSetupMoset);
            }
        }
    }    
}
