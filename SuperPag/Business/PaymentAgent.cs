using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAgent
    {
        public static MCPaymentAgent List()
        {
            MCPaymentAgent mcPaymentAgent = null;
            DPaymentAgent[] arrDPaymentAgent = DataFactory.PaymentAgent().List();

            if (arrDPaymentAgent != null)
            {
                MessageMapper mapper = new MessageMapper();
                mcPaymentAgent = (MCPaymentAgent)mapper.Do(arrDPaymentAgent, typeof(MCPaymentAgent));
            }
            else
                mcPaymentAgent = new MCPaymentAgent();

            return mcPaymentAgent;
        }
    }
}
