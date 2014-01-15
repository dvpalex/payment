using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetupABN
    {
        public static MPaymentAgentSetupABN Locate(int paymentAgentSetupId)
        {
            DPaymentAgentSetupABN dPaymentAgentSetupABN = DataFactory.PaymentAgentSetupABN().Locate(paymentAgentSetupId);
        
            MessageMapper mapper = new MessageMapper();
            MPaymentAgentSetupABN mPaymentAgentSetupABN = (MPaymentAgentSetupABN)mapper.Do(dPaymentAgentSetupABN, typeof(MPaymentAgentSetupABN));
            return mPaymentAgentSetupABN;
        }

        public static void Save(MPaymentAgentSetupABN mPaymentAgentSetupABN)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAgentSetupABN dPaymentAgentSetupABN = (DPaymentAgentSetupABN)mapper.Do(mPaymentAgentSetupABN, typeof(DPaymentAgentSetupABN));

            if (DataFactory.PaymentAgentSetupABN().Locate(mPaymentAgentSetupABN.PaymentAgentSetupId) == null)
            {
                DataFactory.PaymentAgentSetupABN().Insert(dPaymentAgentSetupABN);
            }
            else
            {
                DataFactory.PaymentAgentSetupABN().Update(dPaymentAgentSetupABN);
            }
        }

    }
}
