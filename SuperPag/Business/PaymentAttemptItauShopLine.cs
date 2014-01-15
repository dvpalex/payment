using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAttemptItauShopLine
    {
        public static MPaymentAttemptItauShopline Locate(Guid paymentAttemptId)
        {
            DPaymentAttemptItauShopline dPaymentAttemptItauShopline = DataFactory.PaymentAttemptItauShopline().Locate(paymentAttemptId);

            MessageMapper mapper = new MessageMapper();
            MPaymentAttemptItauShopline mPaymentAttemptItauShopline = (MPaymentAttemptItauShopline)mapper.Do(dPaymentAttemptItauShopline, typeof(MPaymentAttemptItauShopline));
            return mPaymentAttemptItauShopline;
        }

    }
}
