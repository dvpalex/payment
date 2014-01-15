using System;
using System.Collections;
using SuperPag.Framework.Web.WebController;
using SuperPag.Framework.Helper;
using SuperPag.Business.Messages;
using SuperPag.Business;
using SuperPag;

namespace Controller.Lib.Commands
{
	public class ShowTransaction : BaseCommand
	{
		protected override ViewInfo OnExecute()
		{
            Guid paymentAttemptId = (Guid)this.Parameters["PaymentAttemptId"];

            MPaymentAttempt mPaymentAttempt = PaymentReports.Locate(paymentAttemptId);

            if (mPaymentAttempt.Order.Consumer != null)
            {
                MConsumerAddress mConsumerAddressBilling = ConsumerAddress.Locate(mPaymentAttempt.Order.Consumer.ConsumerId, MConsumerAddress.AddressTypes.Billing);
                MConsumerAddress mConsumerAddressDelivery = ConsumerAddress.Locate(mPaymentAttempt.Order.Consumer.ConsumerId, MConsumerAddress.AddressTypes.Delivery);
                this.AddMessage(mConsumerAddressBilling, "1");
                this.AddMessage(mConsumerAddressDelivery, "2");
            }

            this.AddEnumeration(new EnumListBuilder(typeof(OrderStatusForCombo)));
            this.AddMessage(mPaymentAttempt);
            return Map.Views.ShowTransaction;
		}
	}


}
