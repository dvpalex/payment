using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentAttemptKomerci
    {
        public static MPaymentAttemptKomerci Locate(Guid paymentAttemptId)
        {
            DPaymentAttemptKomerci dPaymentAttemptKomerci = DataFactory.PaymentAttemptKomerci().Locate(paymentAttemptId);

            MessageMapper mapper = new MessageMapper();
            MPaymentAttemptKomerci mPaymentAttemptKomerci = (MPaymentAttemptKomerci)mapper.Do(dPaymentAttemptKomerci, typeof(MPaymentAttemptKomerci));
            return mPaymentAttemptKomerci;
        }

    }
}
