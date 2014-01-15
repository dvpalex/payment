using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class Recurrence
    {
        public static MRecurrence Locate(int recurrenceId)
        {
            DRecurrence dRecurrence = DataFactory.Recurrence().Locate(recurrenceId);

            MessageMapper mapper = new MessageMapper();
            MRecurrence mRecurrence = (MRecurrence)mapper.Do(dRecurrence, typeof(MRecurrence));

            mRecurrence.Order = Order.Locate(dRecurrence.orderId);
            mRecurrence.PaymentForm = PaymentForm.Locate(dRecurrence.paymentFormId);

            return mRecurrence;
        }

        public static MRecurrence Locate(long orderId)
        {
            DRecurrence dRecurrence = DataFactory.Recurrence().Locate(orderId);

            if (dRecurrence == null)
                return null;

            MessageMapper mapper = new MessageMapper();
            MRecurrence mRecurrence = (MRecurrence)mapper.Do(dRecurrence, typeof(MRecurrence));

            mRecurrence.Order = Order.Locate(dRecurrence.orderId);
            mRecurrence.PaymentForm = PaymentForm.Locate(dRecurrence.paymentFormId);

            return mRecurrence;
        }

        public static MCRecurrence List()
        {
            MCRecurrence mcRecurrence = null;
            DRecurrence[] arrDRecurrence = DataFactory.Recurrence().List();

            if (arrDRecurrence != null)
            {
                MessageMapper mapper = new MessageMapper();
                //mapper.MapChildren = true;
                mcRecurrence = (MCRecurrence)mapper.Do(arrDRecurrence, typeof(MCRecurrence));
            }
            else
                mcRecurrence = new MCRecurrence();

            return mcRecurrence;
        }
    }
}
