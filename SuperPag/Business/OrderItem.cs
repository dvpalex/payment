using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;

namespace SuperPag.Business
{
    public class OrderItem
    {
        public static MCOrderItem List(long orderId)
        {
            MCOrderItem mcOrderItem = null;
            DOrderItem[] arrDOrderItem = DataFactory.OrderItem().List(orderId);

            if (arrDOrderItem != null)
            {
                MessageMapper mapper = new MessageMapper();
                mcOrderItem = (MCOrderItem)mapper.Do(arrDOrderItem, typeof(MCOrderItem));
            }
            else
                mcOrderItem = new MCOrderItem();

            return mcOrderItem;


        }

    }
}
