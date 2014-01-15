using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class PaymentReports
    {
        public static MCPaymentSummary PaymentSummary(int storeId, DateTime startDate, DateTime finishDate)
        {
            MCPaymentSummary mcPaymentSummary = null;
            DPaymentSummary[] arrPaymentSummary = DataFactory.PaymentSummary().List(storeId, startDate, finishDate);

            if (arrPaymentSummary != null)
            {
                MessageMapper mapper = new MessageMapper();
                mcPaymentSummary = (MCPaymentSummary)mapper.Do(arrPaymentSummary, typeof(MCPaymentSummary));
            }
            else
                mcPaymentSummary = new MCPaymentSummary();

            return mcPaymentSummary;
        }

        public static MCPaymentAttempt PaymentList(int storeId, DateTime startTimeFrom, DateTime startTimeTo, int paymentFormId, MPaymentAttempt.PaymentAttemptStatus status,
            string consumerName, string cpf, string cnpj, MOrder.OrderStatus orderStatus)
        {
            MCPaymentAttempt mcPaymentAttempt = null;
            DPaymentAttemptComplete[] arrPaymentAttemptComplete = DataFactory.PaymentAttempt().List(storeId, startTimeFrom, startTimeTo, paymentFormId, (int)status,
              consumerName, cpf, cnpj, (int)orderStatus);

            if (arrPaymentAttemptComplete != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcPaymentAttempt = (MCPaymentAttempt)mapper.Do(arrPaymentAttemptComplete, typeof(MCPaymentAttempt));
            }
            else
                mcPaymentAttempt = new MCPaymentAttempt();

            return mcPaymentAttempt;
        }

        public static MCPaymentAttempt PaymentList(int storeId, DateTime startTimeFrom, DateTime startTimeTo, int paymentFormId, MPaymentAttempt.PaymentAttemptStatus status)
        {
            MCPaymentAttempt mcPaymentAttempt = null;
            DPaymentAttemptComplete[] arrPaymentAttemptComplete = DataFactory.PaymentAttempt().List(storeId, startTimeFrom, startTimeTo, (int)status, new int[1] { paymentFormId });

            if (arrPaymentAttemptComplete != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcPaymentAttempt = (MCPaymentAttempt)mapper.Do(arrPaymentAttemptComplete, typeof(MCPaymentAttempt));
            }
            else
                mcPaymentAttempt = new MCPaymentAttempt();

            return mcPaymentAttempt;
        }

        public static MCPaymentAttempt PaymentList(int storeId, string storeReferenceOrder)
        {
            MCPaymentAttempt mcPaymentAttempt = null;
            DPaymentAttemptComplete[] arrPaymentAttemptComplete = DataFactory.PaymentAttempt().List(storeId, storeReferenceOrder);

            if (arrPaymentAttemptComplete != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcPaymentAttempt = (MCPaymentAttempt)mapper.Do(arrPaymentAttemptComplete, typeof(MCPaymentAttempt));
            }
            else
                mcPaymentAttempt = new MCPaymentAttempt();

            return mcPaymentAttempt;
        }


        public static MCPaymentAttempt PaymentList(Guid paymentAttemptId)
        {
            MCPaymentAttempt mcPaymentAttempt = null;
            DPaymentAttemptComplete[] arrPaymentAttemptComplete = DataFactory.PaymentAttempt().List(paymentAttemptId);

            if (arrPaymentAttemptComplete != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcPaymentAttempt = (MCPaymentAttempt)mapper.Do(arrPaymentAttemptComplete, typeof(MCPaymentAttempt));
            }
            else
                mcPaymentAttempt = new MCPaymentAttempt();

            return mcPaymentAttempt;
        }

        public static MPaymentAttempt Locate(Guid paymentAttemptId)
        {
            DPaymentAttempt dPaymentAttempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);

            MessageMapper mapper = new MessageMapper();
            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)mapper.Do(dPaymentAttempt, typeof(MPaymentAttempt));
            mPaymentAttempt.Order = Order.Locate(dPaymentAttempt.orderId);
            mPaymentAttempt.PaymentForm = PaymentForm.Locate(dPaymentAttempt.paymentFormId);            
            return mPaymentAttempt;
        }

    }
}
