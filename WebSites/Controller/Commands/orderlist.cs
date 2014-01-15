using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag.Framework;
using Controller.Lib.Util;

namespace Controller.Lib.Commands
{
    public class ListOrder : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            MOrderSearch mOrderSearch = (MOrderSearch)this.Parameters["OrderSearch"];

            MCOrder mcOrder = null;

            if (mOrderSearch.StoreId != int.MinValue && mOrderSearch.StoreReferenceOrder != null)
            {
                mcOrder = Order.List(mOrderSearch.StoreId, mOrderSearch.StoreReferenceOrder);
            }
            else
            {
                mcOrder = Order.List(ControllerContext.StoreId, mOrderSearch.OrderDateFrom, mOrderSearch.OrderDateTo, mOrderSearch.PaymentFormId, 
                    mOrderSearch.Status, mOrderSearch.ConsumerName, mOrderSearch.Cpf, mOrderSearch.Cnpj, mOrderSearch.OrderStatus, mOrderSearch.RecurrenceStatus);
            }

            string indexField = "CreationDate";
            if (this.Parameters["IndexField"] != null)
                indexField = (string)this.Parameters["IndexField"];

            mcOrder.sort(indexField, false);

            this.AddMessage(mcOrder);

            return Map.Views.ListOrder;
		}
	}

    public class ListOrderDetailItem : BaseCommand
    {
        protected override ViewInfo OnExecute()
        {
            MOrderSearch mOrderSearch = (MOrderSearch)this.Parameters["OrderSearch"];
            MCOrder mcOrder = null;
            if (mOrderSearch.StoreId != int.MinValue && mOrderSearch.StoreReferenceOrder != null)
            {
                mcOrder = Order.List(mOrderSearch.StoreId, mOrderSearch.StoreReferenceOrder);
            }
            else
            {
                mcOrder = Order.List(ControllerContext.StoreId, mOrderSearch.OrderDateFrom, mOrderSearch.OrderDateTo, mOrderSearch.PaymentFormId,
                    mOrderSearch.Status, mOrderSearch.ConsumerName, mOrderSearch.Cpf, mOrderSearch.Cnpj, mOrderSearch.OrderStatus, mOrderSearch.RecurrenceStatus);
            }

            MCOrderDetailItem mcOrderDetailItem = new MCOrderDetailItem();
            foreach (MOrder order in mcOrder)
            {
                MOrderDetailItem orderDi = new MOrderDetailItem();
                SuperPag.Framework.Helper.HierarchySet.BaseToChild(order, orderDi);
                string itemsDesc = "";
                MCOrderItem orderItens = OrderItem.List(order.OrderId);
                if (orderItens != null)
                {
                    foreach (MOrderItem item in orderItens)
                        itemsDesc += item.ItemDescription + " + ";
                    itemsDesc = itemsDesc.Remove(itemsDesc.Length - 3, 3);
                    orderDi.ItemsDesc = itemsDesc;
                }
                mcOrderDetailItem.Add(orderDi);
            }

            string indexField = "CreationDate";
            if (this.Parameters["IndexField"] != null)
                indexField = (string)this.Parameters["IndexField"];

            mcOrder.sort(indexField, false);

            this.AddMessage(mcOrderDetailItem);

            return Map.Views.ListOrderDetailItem;
        }
    }
}
