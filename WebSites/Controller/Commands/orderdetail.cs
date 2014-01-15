using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Helper;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag;

namespace Controller.Lib.Commands
{
    public class ShowOrder : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            long orderId = (long)this.Parameters["OrderId"];

            MOrder mOrder = Order.LocateComplete(orderId);

            if (mOrder.Consumer != null)
            {
                MConsumerAddress mConsumerAddressBilling = ConsumerAddress.Locate(mOrder.Consumer.ConsumerId, MConsumerAddress.AddressTypes.Billing);
                MConsumerAddress mConsumerAddressDelivery = ConsumerAddress.Locate(mOrder.Consumer.ConsumerId, MConsumerAddress.AddressTypes.Delivery);
                this.AddMessage(mConsumerAddressBilling, "1");
                this.AddMessage(mConsumerAddressDelivery, "2");
            }

            this.AddEnumeration(new EnumListBuilder(typeof(OrderStatusForCombo)));
            this.AddMessage(mOrder);

            return Map.Views.ShowOrder;
		}
	}


}
