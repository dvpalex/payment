using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using SuperPag.Helper;
using SuperPag.Helper.Xml.Request;
using SuperPag.Data.Messages;
using SuperPag.Data;

namespace SuperPag.Handshake
{
    public class RecurrenceParams
    {
        public string cob_recorrencia;
        public string cob_liq_1par;
        public DateTime cob_data_base_agendamento;
        public string cob_quantidade;

        public static RecurrenceParams GetRecurrenceParams()
        {
            RecurrenceParams recParams = new RecurrenceParams();

            if (HttpContext.Current.Session["htmlHandshake"] != null)
            {
                recParams.cob_recorrencia = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["htmlHandshake"], "//cob_recorrencia");
                recParams.cob_liq_1par = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["htmlHandshake"], "//cob_liq_1par");
                recParams.cob_quantidade = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["htmlHandshake"], "//cob_quantidade");
                recParams.cob_data_base_agendamento = GenericHelper.ParseDateyyyyMMdd(GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["htmlHandshake"], "//cob_data_base_agendamento"));
            }
            else
            {
                recParams.cob_recorrencia = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["xmlHandshake"], "/pedido/parametros_opcionais/cob_recorrencia");
                recParams.cob_liq_1par = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["xmlHandshake"], "/pedido/parametros_opcionais/cob_liq_1par");
                recParams.cob_quantidade = GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["xmlHandshake"], "/pedido/parametros_opcionais/cob_quantidade");
                recParams.cob_data_base_agendamento = GenericHelper.ParseDateyyyyMMdd(GenericHelper.GetSingleNodeString((string)HttpContext.Current.Session["xmlHandshake"], "/pedido/parametros_opcionais/cob_data_base_agendamento"));
            }

            return recParams;
        }
    }

    public class RecurrenceProcess
    {
        public static void FinishTransaction(DPaymentAttempt attempt)
        {
            //Recupero parametros de recorrencia
            RecurrenceParams recParams = RecurrenceParams.GetRecurrenceParams();

            if (!String.IsNullOrEmpty(recParams.cob_recorrencia) && recParams.cob_recorrencia == "1")
            {
                //TODO: Agendamento de Parcelas

                genericPaymentFormDetail rec = MountPaymentFormDetail(attempt);

                DSchedule schedule;
                for (int i = 1; i < int.Parse(recParams.cob_quantidade); i++)
                {
                    //Agenda transações mensais 
                    if (rec.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                        ((genericPaymentFormDetailBoletoInformation)rec.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)rec.Item).dueDate.AddMonths(1);

                    schedule = new DSchedule();
                    schedule.orderId = (int)attempt.orderId;
                    schedule.recurrenceId = int.MinValue;
                    schedule.installmentNumber = attempt.installmentNumber;
                    schedule.installmentType = (int)InstallmentType.Merchant;
                    schedule.date = Convert.ToDateTime(recParams.cob_data_base_agendamento == DateTime.MinValue ? DateTime.Now.AddMonths(i) : recParams.cob_data_base_agendamento.AddMonths(i)).Date; // agendamento dos proximos pagamentos
                    schedule.paymentFormId = attempt.paymentFormId;
                    schedule.paymentFormDetail = SuperPag.Helper.Xml.XmlHelper.GetXml(typeof(genericPaymentFormDetail), rec);
                    schedule.status = (int)ScheduleStatus.Scheduled;
                    DataFactory.Schedule().Insert(schedule);
                }
            }
            else if (!String.IsNullOrEmpty(recParams.cob_recorrencia) && recParams.cob_recorrencia == "-1")
            {
                //TODO: Recorrencia (Indeterminada)

                //TODO: Salvo as informações da recorrência
                DRecurrence dRecurrence = new DRecurrence();
                dRecurrence.orderId = attempt.orderId;
                dRecurrence.interval = int.Parse(recParams.cob_quantidade) < 1 ? 30 : int.Parse(recParams.cob_quantidade);
                dRecurrence.paymentFormId = attempt.paymentFormId;

                genericPaymentFormDetail rec = MountPaymentFormDetail(attempt);
                if (rec.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                    ((genericPaymentFormDetailBoletoInformation)rec.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)rec.Item).dueDate.AddMonths(1);

                dRecurrence.paymentFormDetail = SuperPag.Helper.Xml.XmlHelper.GetXml(typeof(genericPaymentFormDetail), rec);

                dRecurrence.startDate = recParams.cob_data_base_agendamento == DateTime.MinValue ? DateTime.Now : recParams.cob_data_base_agendamento;
                dRecurrence.status = (int)RecurrenceStatus.Active;
                DataFactory.Recurrence().Insert(dRecurrence);

                //TODO:Agenda próxima transação
                if (rec.Item.GetType().Equals(typeof(genericPaymentFormDetailBoletoInformation)))
                    ((genericPaymentFormDetailBoletoInformation)rec.Item).dueDate = ((genericPaymentFormDetailBoletoInformation)rec.Item).dueDate.AddMonths(1);

                DSchedule schedule = new DSchedule();
                schedule.orderId = (int)attempt.orderId;
                schedule.recurrenceId = dRecurrence.recurrenceId;
                schedule.installmentNumber = attempt.installmentNumber;
                schedule.installmentType = (int)InstallmentType.Merchant;
                schedule.date = dRecurrence.startDate.AddDays(dRecurrence.interval).Date;
                schedule.paymentFormId = attempt.paymentFormId;
                schedule.paymentFormDetail = SuperPag.Helper.Xml.XmlHelper.GetXml(typeof(genericPaymentFormDetail), rec);
                schedule.status = (int)ScheduleStatus.Scheduled;
                DataFactory.Schedule().Insert(schedule);
                
            }

            HttpContext.Current.Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
        }

        private static genericPaymentFormDetail MountPaymentFormDetail(DPaymentAttempt attempt)
        {
            genericPaymentFormDetail paymentFormDetail = new genericPaymentFormDetail();

            if (GenericHelper.IsBoleto(attempt.paymentFormId))
            {
                genericPaymentFormDetailBoletoInformation boletoInformation = new genericPaymentFormDetailBoletoInformation();

                if (HttpContext.Current.Session["htmlHandshake"] != null)
                {
                    boletoInformation.instructions = GenericHelper.GetSingleNodeString(HttpContext.Current.Session["htmlHandshake"].ToString(), "/root/form/instrucao_boleto");
                    boletoInformation.dueDate = GenericHelper.ParseDateyyyyMMdd(GenericHelper.GetSingleNodeString(HttpContext.Current.Session["htmlHandshake"].ToString(), "/root/form/data_boleto"));
                }
                else
                {
                    boletoInformation.instructions = GenericHelper.GetSingleNodeString(HttpContext.Current.Session["xmlHandshake"].ToString(), "//instrucao_boleto");
                    boletoInformation.dueDate = GenericHelper.ParseDateyyyyMMdd(GenericHelper.GetSingleNodeString(HttpContext.Current.Session["xmlHandshake"].ToString(), "/pedido/parametros_opcionais/data_boleto"));
                }

                paymentFormDetail.Item = boletoInformation;
            }
            else
            {
                genericPaymentFormDetailCreditCardInformation creditCardInformation = new genericPaymentFormDetailCreditCardInformation();
                CreditCardInformation cardinfo = GenericHelper.GetCreditCardInformation();

                creditCardInformation.cardHolder = cardinfo.Name;
                creditCardInformation.cardNumber = ulong.Parse(cardinfo.Number);
                creditCardInformation.expireDate = cardinfo.ExpirationDate.ToString("yyyy-MM");
                creditCardInformation.securityCode = uint.Parse(cardinfo.SecurityNumber);

                paymentFormDetail.Item = creditCardInformation;
            }

            return paymentFormDetail;
        }
    }
}
