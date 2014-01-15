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
    public class Reprocess
    {
        public Resp.response ReprocessTransaction(DStore store, int scheduleId)
        {
            Resp.response response = new Resp.response();
            response.orders = new Resp.responseOrders();

            response.orders.order = new Resp.responseOrdersOrder[1];
            response.orders.order[0] = GetResponseOrder(store, scheduleId);

            return response;
        }

        public Resp.responseOrdersOrder GetResponseOrder(DStore store, int scheduleId)
        {
            return null;
        }

    }
}
