using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetupVBV
    {
        public static MPaymentAgentSetupVBV Locate(int paymentAgentSetupId)
        {
            DPaymentAgentSetupVBV dPaymentAgentSetupVBV = DataFactory.PaymentAgentSetupVBV().Locate(paymentAgentSetupId);
        
            MessageMapper mapper = new MessageMapper();
            MPaymentAgentSetupVBV mPaymentAgentSetupVBV = (MPaymentAgentSetupVBV)mapper.Do(dPaymentAgentSetupVBV, typeof(MPaymentAgentSetupVBV));
            return mPaymentAgentSetupVBV;
        }

        public static void Save(MPaymentAgentSetupVBV mPaymentAgentSetupVBV)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAgentSetupVBV dPaymentAgentSetupVBV = (DPaymentAgentSetupVBV)mapper.Do(mPaymentAgentSetupVBV, typeof(DPaymentAgentSetupVBV));

            if (DataFactory.PaymentAgentSetupVBV().Locate(mPaymentAgentSetupVBV.PaymentAgentSetupId) == null)
            {
                DataFactory.PaymentAgentSetupVBV().Insert(dPaymentAgentSetupVBV);
            }
            else
            {
                DataFactory.PaymentAgentSetupVBV().Update(dPaymentAgentSetupVBV);
            }
        }
    }    
}
