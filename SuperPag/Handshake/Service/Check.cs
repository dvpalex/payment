using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Helper.Xml.Request;
using Resp = SuperPag.Helper.Xml.Response;
using SuperPag.Helper;
using System.Reflection;
using System.Web;
using System.Configuration;

namespace SuperPag.Handshake.Service
{
    public class Check
    {
        public Resp.response ProcessCheck(DStore store, long orderId)
        {
            Resp.response response = new Resp.response();
            response.orders = new Resp.responseOrders();
            
            response.orders.order = new Resp.responseOrdersOrder[1];
            response.orders.order[0] = GetResponseOrder(store, orderId);
            
            return response;
        }

        public Resp.response ProcessCheckRecurrence(DStore store, long orderId, DateTime dateFrom, DateTime dateTo)
        {
            Resp.response response = new Resp.response();
            response.orders = new Resp.responseOrders();

            response.orders.order = new Resp.responseOrdersOrder[1];
            response.orders.order[0] = GetResponseOrderRecurrence(store, orderId, dateFrom, dateTo);

            return response;
        }

        public Resp.responseOrdersOrder GetResponseOrder(DStore store, long orderId)
        {
            //Checar se o Id passado corresponde a uma order
            DOrder dOrder = DataFactory.Order().Locate(orderId);
            Ensure.IsNotNull(dOrder, "Pedido inválido");

            HelperService.CheckOrderByStore(store, dOrder);
            
            DOrderInstallment[] installments = DataFactory.OrderInstallment().List(dOrder.orderId);
            Ensure.IsNotNull(installments, "O pedido não possui parcelamento válido");
            
            Resp.responseOrdersOrder responseOrder = new Resp.responseOrdersOrder();

            responseOrder.id = (ulong)dOrder.orderId;
            responseOrder.reference = dOrder.storeReferenceOrder;
            responseOrder.total = GenericHelper.ParseLong(dOrder.finalAmount);
            responseOrder.status = (byte)PaymentAttemptStatus.Pending;

            DRecurrence dRecurrence = DataFactory.Recurrence().Locate(orderId);
            if (dRecurrence != null)
            {
                if (installments.Length > 1)
                    Ensure.IsNotNull(null, "O pedido recorrente enviado não possui parcelamento válido");

                DPaymentAttempt[] attempts = DataFactory.PaymentAttempt().ListSortedByDate(orderId);
                DSchedule[] schedules = DataFactory.Schedule().List(orderId, (int)ScheduleStatus.Scheduled);
                if (attempts == null && schedules == null)
                    return responseOrder;

                responseOrder.payments = new Resp.responseOrdersOrderPayment[(attempts != null ? 1 : 0) + (schedules != null ? schedules.Length : 0)];

                int paycount = 0;
                if (attempts != null)
                    responseOrder.payments[paycount++] = HelperService.GetResponseByPayment(attempts[0]);

                if (schedules != null)
                    for (int i = 0; i < schedules.Length; i++)
                        responseOrder.payments[paycount++] = HelperService.GetResponseBySchedule(schedules[i]);
            }
            else
            {
                DPaymentAttempt[] attempts = DataFactory.PaymentAttempt().ListSortedByDateAsc(orderId);
                DSchedule[] schedules = DataFactory.Schedule().ListSortedByNumber(orderId);
                if (attempts == null && schedules == null)
                    return responseOrder;

                if (attempts != null && schedules == null)
                {
                    int attemptPos = 0;
                    List<DPaymentAttempt> attemptsNull = new List<DPaymentAttempt>();
                    Dictionary<int, DPaymentAttempt> attemptsNotNull = new Dictionary<int, DPaymentAttempt>();
                    for (int i = 0; i < attempts.Length; i++)
                    {
                        if (attempts[i].installmentNumber == int.MinValue)
                            attemptsNull.Add(attempts[i]);
                        else
                            attemptsNotNull[attempts[i].installmentNumber] = attempts[i];
                    }

                    if (attemptsNull.Count == 0 && attemptsNotNull.Count == 0)
                        return responseOrder;

                    responseOrder.payments = new Resp.responseOrdersOrderPayment[(attemptsNull.Count > 0 ? 1 : 0) + (attemptsNotNull.Count > 0 ? 1 : 0)];

                    if (attemptsNull.Count > 0)
                    {
                        DPaymentAttempt attempt = GenericHelper.ChooseAttemptByStatus(attemptsNull.ToArray());
                        responseOrder.payments[attemptPos++] = HelperService.GetResponseByPayment(attempt);
                    }
                    
                    if (attemptsNotNull.Count > 0)
                    {
                        responseOrder.payments[attemptPos] = HelperService.GetResponseByPayment(installments, attemptsNotNull);
                    }
                }
                else if (schedules != null && schedules.Length == 1 && schedules[0].installmentNumber == int.MinValue)
                {
                    responseOrder.payments = new Resp.responseOrdersOrderPayment[1];
                    responseOrder.payments[0] = HelperService.GetResponseBySchedule(installments, schedules[0]);
                }
                else
                {
                    if (!HelperService.CheckScheduleInstallmentNumbers(schedules))
                        Ensure.IsNotNull(null, "O pedido está com o parcelamento inválido");

                    if (schedules[0].installmentNumber == 2)
                    {
                        responseOrder.payments = new Resp.responseOrdersOrderPayment[schedules.Length + 1];
                        responseOrder.payments[0] = HelperService.GetResponseByPaymentNotInSchedule(attempts, schedules);
                        for (int i = 0; i < schedules.Length; i++)
                            responseOrder.payments[i + 1] = HelperService.GetResponseBySchedule(schedules[i]);
                    }
                    else
                    {
                        responseOrder.payments = new Resp.responseOrdersOrderPayment[schedules.Length];
                        for (int i = 0; i < schedules.Length; i++)
                            responseOrder.payments[i] = HelperService.GetResponseBySchedule(schedules[i]);
                    }
                }
            }

            responseOrder.status = (byte)HelperService.GetOrderStatus(responseOrder.payments);

            return responseOrder;
        }

        public Resp.responseOrdersOrder GetResponseOrderRecurrence(DStore store, long orderId, DateTime dateFrom, DateTime dateTo)
        {
            //Checar se o Id passado corresponde a uma order
            DOrder dOrder = DataFactory.Order().Locate(orderId);
            Ensure.IsNotNull(dOrder, "Pedido inválido");

            HelperService.CheckOrderByStore(store, dOrder);

            DOrderInstallment[] installments = DataFactory.OrderInstallment().List(dOrder.orderId);
            Ensure.IsNotNull(installments, "O pedido não possui parcelamento válido");

            if (dateFrom == DateTime.MinValue || dateTo == DateTime.MinValue || dateFrom > dateTo)
                Ensure.IsNotNull(null, "O período de pesquisa enviado está inválido");
            
            Resp.responseOrdersOrder responseOrder = new Resp.responseOrdersOrder();

            responseOrder.id = (ulong)dOrder.orderId;
            responseOrder.reference = dOrder.storeReferenceOrder;
            responseOrder.total = GenericHelper.ParseLong(dOrder.finalAmount);
            responseOrder.status = (byte)PaymentAttemptStatus.Pending;

            DRecurrence dRecurrence = DataFactory.Recurrence().Locate(orderId);
            Ensure.IsNotNull(dRecurrence, "O pedido enviado não pertence a uma recorrência válida");
            
            if (installments.Length > 1)
                Ensure.IsNotNull(null, "O pedido recorrente enviado não possui parcelamento válido");

            DPaymentAttempt[] attempts = DataFactory.PaymentAttempt().List(orderId, dateFrom, dateTo);
            DSchedule[] schedules = DataFactory.Schedule().List(orderId, (int)ScheduleStatus.Scheduled, dateFrom, dateTo);
            if (attempts == null && schedules == null)
            {
                GenericHelper.LogFile("SuperPag.Handshake.Service::Check::CheckResponseOrderRecurrence não há tentativas de pagamento nem pagamentos agendados. orderId=" + orderId.ToString() + " dateFrom=" + dateFrom.ToString("yyyy-MM-dd") + " dateTo=" + dateTo.ToString("yyyy-MM-dd"), LogFileEntryType.Information);
                return responseOrder;
            }

            responseOrder.payments = new Resp.responseOrdersOrderPayment[(attempts != null ? attempts.Length : 0) + (schedules != null ? schedules.Length : 0)];

            int paycount = 0;
            if (attempts != null)
                for (int i = 0; i < attempts.Length; i++)
                    responseOrder.payments[paycount++] = HelperService.GetResponseByPayment(attempts[i]);

            if (schedules != null)
                for (int i = 0; i < schedules.Length; i++)
                    responseOrder.payments[paycount++] = HelperService.GetResponseBySchedule(schedules[i]);
            
            responseOrder.status = (byte)HelperService.GetOrderStatus(responseOrder.payments);

            return responseOrder;
        }
    }
}
