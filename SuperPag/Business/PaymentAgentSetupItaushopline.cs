using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgentSetupItauShopline
    {
        public static MPaymentAgentSetupItauShopline Locate(int paymentAgentSetupId)
        {
            DPaymentAgentSetupItauShopline dPaymentAgentSetupItauShopline = DataFactory.PaymentAgentSetupItauShopline().Locate(paymentAgentSetupId);
        
            MessageMapper mapper = new MessageMapper();
            MPaymentAgentSetupItauShopline mPaymentAgentSetupItauShopline = (MPaymentAgentSetupItauShopline)mapper.Do(dPaymentAgentSetupItauShopline, typeof(MPaymentAgentSetupItauShopline));
            return mPaymentAgentSetupItauShopline;
        }

        public static void Save(MPaymentAgentSetupItauShopline mPaymentAgentSetupItauShopline)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAgentSetupItauShopline dPaymentAgentSetupItauShopline = (DPaymentAgentSetupItauShopline)mapper.Do(mPaymentAgentSetupItauShopline, typeof(DPaymentAgentSetupItauShopline));

            if (DataFactory.PaymentAgentSetupItauShopline().Locate(mPaymentAgentSetupItauShopline.PaymentAgentSetupId) == null)
            {
                DataFactory.PaymentAgentSetupItauShopline().Insert(dPaymentAgentSetupItauShopline);
            }
            else
            {
                DataFactory.PaymentAgentSetupItauShopline().Update(dPaymentAgentSetupItauShopline);
            }
        }
    }    
}
