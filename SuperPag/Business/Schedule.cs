using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class Schedule
    {
        public static MSchedule Locate(int scheduleId)
        {
            DSchedule dSchedule = DataFactory.Schedule().Locate(scheduleId);

            MessageMapper mapper = new MessageMapper();
            MSchedule mSchedule = (MSchedule)mapper.Do(dSchedule, typeof(MSchedule));

            mSchedule.Order = Order.Locate(dSchedule.orderId);
            mSchedule.PaymentForm = PaymentForm.Locate(dSchedule.paymentFormId);

            return mSchedule;
        }

        public static MCSchedule List(long orderId)
        {
            MCSchedule mcSchedule = null;
            DScheduleComplete[] arrDSchedule = DataFactory.Schedule().List(orderId);

            if (arrDSchedule != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcSchedule = (MCSchedule)mapper.Do(arrDSchedule, typeof(MCSchedule));
            }
            else
                mcSchedule = new MCSchedule();

            return mcSchedule;
        }
    }
}
