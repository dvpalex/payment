using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace SuperPag.Business
{
    public class Order
    {
        public static MOrder Locate(long orderId)
        {
            DOrder dOrder = DataFactory.Order().Locate(orderId);

            MessageMapper mapper = new MessageMapper();
            MOrder mOrder = (MOrder)mapper.Do(dOrder, typeof(MOrder));
            mOrder.Itens = OrderItem.List(mOrder.OrderId);
            mOrder.Consumer = Consumer.Locate(dOrder.consumerId);
            mOrder.Store = Store.Locate(dOrder.storeId);
            mOrder.StatusChangeUserName = Users.Name(dOrder.statusChangeUserId);

            return mOrder;
        }

        public static MOrder LocateWithAttempts(long orderId)
        {
            DOrder dOrder = DataFactory.Order().Locate(orderId);

            MessageMapper mapper = new MessageMapper();
            MOrder mOrder = (MOrder)mapper.Do(dOrder, typeof(MOrder));
            mOrder.Itens = OrderItem.List(mOrder.OrderId);
            mOrder.Consumer = Consumer.Locate(dOrder.consumerId);
            mOrder.Store = Store.Locate(dOrder.storeId);
            mOrder.StatusChangeUserName = Users.Name(dOrder.statusChangeUserId);
            mOrder.PaymentAttempts = PaymentAttempt.List(mOrder.OrderId);
            return mOrder;
        }

        public static MOrder LocateComplete(long orderId)
        {
            DOrder dOrder = DataFactory.Order().Locate(orderId);

            MessageMapper mapper = new MessageMapper();
            MOrder mOrder = (MOrder)mapper.Do(dOrder, typeof(MOrder));
            mOrder.Itens = OrderItem.List(mOrder.OrderId);
            mOrder.Consumer = Consumer.Locate(dOrder.consumerId);
            mOrder.Store = Store.Locate(dOrder.storeId);
            mOrder.StatusChangeUserName = Users.Name(dOrder.statusChangeUserId);
            mOrder.PaymentAttempts = PaymentAttempt.List(mOrder.OrderId);
            mOrder.Recurrence = Recurrence.Locate(orderId);
            mOrder.SchedulePayments = Schedule.List(orderId);

            return mOrder;
        }

        public static void Update(MOrder mOrder)
        {
            MessageMapper mapper = new MessageMapper();
            DOrder dOrder = (DOrder)mapper.Do(mOrder, typeof(DOrder));
            dOrder.consumerId = mOrder.Consumer.ConsumerId;
            dOrder.storeId = mOrder.Store.StoreId;

            DataFactory.Order().Update(dOrder);
        }

        public static MCOrder List(int storeId, string storeReferenceOrder)
        {
            MCOrder mcOrder = null;
            DOrder[] arrDOrder = DataFactory.Order().List(storeId, storeReferenceOrder);

            if (arrDOrder != null)
            {
                MessageMapper mapper = new MessageMapper();
                //mapper.MapChildren = true;
                mcOrder = (MCOrder)mapper.Do(arrDOrder, typeof(MCOrder));
            }
            else
                mcOrder = new MCOrder();

            return mcOrder;
        }

        public static MCOrder List(int storeId, DateTime startTimeFrom, DateTime startTimeTo, int paymentFormId, MPaymentAttempt.PaymentAttemptStatus status,
            string consumerName, string cpf, string cnpj, MOrder.OrderStatus orderStatus, MRecurrence.RecurrenceStatus recurrenceStatus)
        {
            MCOrder mcOrder = null;
            DOrder[] arrDOrder = DataFactory.Order().List(storeId, startTimeFrom, startTimeTo, paymentFormId, (int)status,
              consumerName, cpf, cnpj, (int)orderStatus, (int)recurrenceStatus);

            if (arrDOrder != null)
            {
                MessageMapper mapper = new MessageMapper();
                //mapper.MapChildren = true;
                mcOrder = (MCOrder)mapper.Do(arrDOrder, typeof(MCOrder));
            }
            else
                mcOrder = new MCOrder();

            return mcOrder;
        }

        /// <summary>
        /// verifica se o usuario pode ter storeReferenceOrder duplicada
        /// se retornar 1 disparar uma mensagem.
        /// </summary>
        /// <param name="storeReferenceOrder"></param>
        /// <param name="StoreId"></param>
        /// <returns></returns>       
        public static bool GetDuplicateOrder(string storeReferenceOrder, int StoreId)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetDuplicateOrder");

            db.AddInParameter(dbCommand, "@storeReferenceOrder", DbType.String, storeReferenceOrder);
            db.AddInParameter(dbCommand, "@storeId", DbType.Int32, StoreId);

            Object var = db.ExecuteScalar(dbCommand);

            if (var != DBNull.Value && var != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
