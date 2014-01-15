using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetupKomerci
    {
        public static MPaymentAgentSetupKomerci Locate(int paymentAgentSetupId)
        {
            DPaymentAgentSetupKomerci dPaymentAgentSetupKomerci = DataFactory.PaymentAgentSetupKomerci().Locate(paymentAgentSetupId);
        
            MessageMapper mapper = new MessageMapper();
            MPaymentAgentSetupKomerci mPaymentAgentSetupKomerci = (MPaymentAgentSetupKomerci)mapper.Do(dPaymentAgentSetupKomerci, typeof(MPaymentAgentSetupKomerci));
            return mPaymentAgentSetupKomerci;
        }

        public static void Save(MPaymentAgentSetupKomerci mPaymentAgentSetupKomerci)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAgentSetupKomerci dPaymentAgentSetupKomerci = (DPaymentAgentSetupKomerci)mapper.Do(mPaymentAgentSetupKomerci, typeof(DPaymentAgentSetupKomerci));

            if (DataFactory.PaymentAgentSetupKomerci().Locate(mPaymentAgentSetupKomerci.PaymentAgentSetupId) == null)
            {
                DataFactory.PaymentAgentSetupKomerci().Insert(dPaymentAgentSetupKomerci);
            }
            else
            {
                DataFactory.PaymentAgentSetupKomerci().Update(dPaymentAgentSetupKomerci);
            }
        }
    }    
}
