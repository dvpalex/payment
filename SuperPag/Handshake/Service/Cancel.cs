using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper.Xml.Request;
using Resp = SuperPag.Helper.Xml.Response;
using SuperPag.Helper;
using System.Reflection;

namespace SuperPag.Handshake.Service
{
    public class Cancel
    {
        public Resp.response ProcessCancel(DStore store, long orderId)
        {
            Resp.response response = new Resp.response();
            response.orders = new Resp.responseOrders();

            response.orders.order = new Resp.responseOrdersOrder[1];
            response.orders.order[0] = CancelOrder(store, orderId);

            return response;
        }

        public Resp.responseOrdersOrder CancelOrder(DStore store, long orderId)
        {
            //Checar se o Id passado corresponde a uma order
            DOrder dOrder = DataFactory.Order().Locate(orderId);
            if (dOrder == null)
                Ensure.IsNotNull(null, "Pedido inválido");

            HelperService.CheckOrderByStore(store, dOrder);

            DOrderInstallment[] installments = DataFactory.OrderInstallment().List(dOrder.orderId);
            if (installments == null)
                Ensure.IsNotNull(null, "O pedido não possui parcelamento válido");

            DRecurrence dRecurrence = DataFactory.Recurrence().Locate(orderId);
            if (dRecurrence != null)
            {
                dRecurrence.status = (int)RecurrenceStatus.Cancelled;
                DataFactory.Recurrence().Update(dRecurrence);
            }
            
            DSchedule[] dSchedules = DataFactory.Schedule().List(orderId);
            if (dSchedules != null)
                foreach (DSchedule schedule in dSchedules)
                    if (schedule.status == (int)ScheduleStatus.Scheduled)
                    {
                        schedule.status = (int)ScheduleStatus.Canceled;
                        DataFactory.Schedule().Update(schedule);
                    }

            Check check = new Check();
            return check.GetResponseOrder(store, dOrder.orderId);
        }
    }
}
