using System;
using System.Collections.Generic;
using System.Text;
using SuperPag;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Helper;
using SuperPag.Handshake.Service;
using SuperPag.Helper.Xml.Request;
using SuperPag.Helper.Xml;
using System.Configuration;

namespace Schedule
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int success = 0, error = 0;
                DateTime dateFrom = DateTime.Today, dateTo = DateTime.MinValue;               
                if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["DateFrom"] != null && ConfigurationManager.AppSettings["DateTo"] != null)
                {
                    DateTime date1 = GenericHelper.ParseDate(ConfigurationManager.AppSettings["DateFrom"].ToString(), "yyyy-MM-dd");
                    DateTime date2 = GenericHelper.ParseDate(ConfigurationManager.AppSettings["DateTo"].ToString(), "yyyy-MM-dd");

                    if (date1 != DateTime.MinValue && date2 != DateTime.MinValue && date2 <= DateTime.Today && date1 <= date2)
                    {
                        dateFrom = date1;
                        dateTo = date2;
                    }
                    else
                    {
                        GenericHelper.LogFile("Schedule::Program.cs::Main os parâmetros 'DateFrom' e 'DateTo' definem um período inválido, o processamento será feito com a data de hoje", LogFileEntryType.Warning);
                    }
                }
                else if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings["DaysBefore"] != null)
                {
                    uint days = 0;
                    if (uint.TryParse(ConfigurationManager.AppSettings["DaysBefore"].ToString(), out days))
                        dateFrom = dateFrom.AddDays(-days);
                    else
                        GenericHelper.LogFile("Schedule::Program.cs::Main o valor do parâmetro 'DaysBefore' está inválido, o processamento será feito com a data de hoje", LogFileEntryType.Warning);
                }

                DSchedule[] schedules = null;
                if (dateFrom != DateTime.MinValue && dateTo != DateTime.MinValue)
                {
                    schedules = DataFactory.Schedule().List(dateFrom, dateTo, (int)ScheduleStatus.Scheduled);
                }
                else
                {
                    schedules = DataFactory.Schedule().List(dateFrom, (int)ScheduleStatus.Scheduled);
                }

                if (schedules != null)
                    foreach (DSchedule schedule in schedules)
                    {
                        if (ProcessSchedule(schedule))
                            success++;
                        else
                            error++;
                        
                        schedule.status = (int)ScheduleStatus.Processed;
                        DataFactory.Schedule().Update(schedule);
                    }

                if (schedules == null)
                    GenericHelper.LogFile("Schedule::Program.cs::Main nenhum agendamento pendente", LogFileEntryType.Information);
                else
                    GenericHelper.LogFile("Schedule::Program.cs::Main foram processados " + schedules.Length.ToString() + " agendamentos, sucesso=" + success.ToString() + " falha=" + error.ToString(), LogFileEntryType.Information);
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("Schedule::Program.cs::Main " + e.Message, LogFileEntryType.Error);
            }
        }
        
        internal static bool ProcessSchedule(DSchedule schedule)
        {
            try
            {
                DOrder order = DataFactory.Order().Locate(schedule.orderId);
                Ensure.IsNotNull(order, "Pedido inválido scheduleId=" + schedule.scheduleId.ToString());

                DOrderInstallment[] installments = DataFactory.OrderInstallment().List(order.orderId);
                Ensure.IsNotNull(installments, "O pedido não possui parcelamento válido scheduleId=" + schedule.scheduleId.ToString());
                
                genericPaymentFormDetail pay;
                DPaymentAttempt attempt = null;

                string msgerror = "";
                if ((pay = (genericPaymentFormDetail)XmlHelper.GetClass(schedule.paymentFormDetail, typeof(genericPaymentFormDetail), out msgerror)) == null)
                    Ensure.IsNotNull(null, msgerror);

                
                if (schedule.installmentNumber == int.MinValue)
                {
                   HelperService.StartTransaction(order, installments, (InstallmentType)schedule.installmentType, pay, out attempt,null);
                }
                else
                {
                   HelperService.StartTransaction(order, installments[schedule.installmentNumber-1], (InstallmentType)schedule.installmentType, pay, out attempt,null);
                }

                schedule.paymentAttemptId = attempt.paymentAttemptId;
                DataFactory.Schedule().Update(schedule);

                if (schedule.recurrenceId != int.MinValue)
                {
                    DRecurrence recurrence = DataFactory.Recurrence().Locate(schedule.recurrenceId);
                    if (recurrence != null && recurrence.status == (int)RecurrenceStatus.Active)
                    {
                        DSchedule newSchedule = new DSchedule();
                        newSchedule.orderId = (int)recurrence.orderId;
                        newSchedule.recurrenceId = recurrence.recurrenceId;
                        newSchedule.installmentNumber = schedule.installmentNumber;
                        newSchedule.installmentType = schedule.installmentType;

                        DateTime newDate = schedule.date;
                        if (recurrence.interval % 30 != 0)
                            newDate = newDate.AddDays(recurrence.interval);
                        else
                            newDate = newDate.AddMonths(recurrence.interval / 30);

                        newSchedule.date = newDate;
                        newSchedule.paymentFormId = recurrence.paymentFormId;

                        //TESTAR: Se boleto, adicionar o intervalo passado na data de vencimento
                        genericPaymentFormDetail recPay = (genericPaymentFormDetail)XmlHelper.GetClass(recurrence.paymentFormDetail, typeof(genericPaymentFormDetail), out msgerror);
                        if(recPay == null)
                            Ensure.IsNotNull(null, msgerror);

                        if (SuperPag.Helper.GenericHelper.IsBoleto(recurrence.paymentFormId) && recPay.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                        {
                            if (recurrence.interval % 30 != 0)
                                ((genericPaymentFormDetailBoletoInformation)recPay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddDays(recurrence.interval);
                            else
                                ((genericPaymentFormDetailBoletoInformation)recPay.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)pay.Item).dueDate.AddMonths(recurrence.interval / 30);
                        }

                        newSchedule.paymentFormDetail = XmlHelper.GetXml(typeof(genericPaymentFormDetail), recPay);
                        newSchedule.status = (int)ScheduleStatus.Scheduled;
                        DataFactory.Schedule().Insert(newSchedule);
                    }
                }

                //Como o SuperPag é transacional, sempre atualizamos o status da Order
                //(mesmo na recorrência). Caso seja um pedido (com agendamento) que tenha
                //parcelas não pagas, ai sim fixamos o status como pendente de pagamento.
                bool allPaid = true;
                foreach (DOrderInstallment installment in installments)
                {
                    if (installment.status != (int)OrderInstallmentStatus.Paid)
                    {
                        allPaid = false;
                        break;
                    }
                }
                if (!allPaid && schedule.installmentNumber != int.MinValue)
                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, (int)PaymentAttemptStatus.PendingPaid);
                else
                    GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);

                return true;
            }
            catch (Exception e)
            {
                GenericHelper.LogFile("Schedule::Program.cs::ProcessSchedule " + e.Message, LogFileEntryType.Error);
                return false;
            }
        }
    }
}
