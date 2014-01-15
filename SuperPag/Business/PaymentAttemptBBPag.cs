using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAttemptBBPag
    {
        public static MPaymentAttemptBB Locate(Guid paymentAttemptId)
        {
            DPaymentAttemptBB dPaymentAttemptBB = DataFactory.PaymentAttemptBB().Locate(paymentAttemptId);

            MessageMapper mapper = new MessageMapper();
            MPaymentAttemptBB mPaymentAttemptBB = (MPaymentAttemptBB)mapper.Do(dPaymentAttemptBB, typeof(MPaymentAttemptBB));
            return mPaymentAttemptBB;
        }

    }
}
