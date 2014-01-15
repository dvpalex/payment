using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using SuperPag;
using SuperPag.Data;
using SuperPag.Data.Messages;
using SuperPag.Helper;

namespace SuperPag.Agents
{
    public class WebAgent : IAgent
    {
        public DPaymentAttempt attempt;
        public DOrder order;
        public DStorePaymentInstallment storePaymentInstallment;
        public DPaymentForm paymentForm;

        public WebAgent() {}
        public WebAgent(Guid paymentAttemptId)
        {
            this.attempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
            if(this.attempt != null)
                this.order = DataFactory.Order().Locate(attempt.orderId);
        }
        public void Fill(Guid paymentAttemptId)
        {
            if (this.attempt == null)
                this.attempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
            if (this.attempt != null && this.order == null)
                this.order = DataFactory.Order().Locate(attempt.orderId);
            if (this.order != null && this.attempt != null && this.storePaymentInstallment == null)
                this.storePaymentInstallment = DataFactory.StorePaymentInstallment().Locate(order.storeId, attempt.paymentFormId, order.installmentQuantity);
            if (this.attempt != null && this.paymentForm == null)
                this.paymentForm = DataFactory.PaymentForm().Locate(attempt.paymentFormId);
        }

        public virtual void Start(Guid paymentAttemptId)
        {
            Ensure.IsNotNull(HttpContext.Current, "Contexto inválido iniciando uma transação web.");
            Fill(paymentAttemptId);

            //Historico de Navegacao
            GenericHelper.SetOrderStatus(HttpContext.Current, WorkflowOrderStatus.AgentCalled, attempt.paymentFormId + "," + order.installmentQuantity + "," + paymentForm.paymentAgentId);

            //Simulação
            if (attempt.isSimulation)
                Simulate();
        }

        public virtual void Finish()
        {
            if ((attempt.status == (int)PaymentAttemptStatus.Paid) ||
                (attempt.status == (int)PaymentAttemptStatus.PendingPaid && GenericHelper.IsBoleto(attempt.paymentFormId)))
                HttpContext.Current.Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId);
            else
                HttpContext.Current.Response.Redirect("~/tryagain.aspx?id=" + attempt.paymentAttemptId);
        }

        public virtual void Simulate()
        {
            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.Paid;
            DataFactory.PaymentAttempt().Update(attempt);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
            HttpContext.Current.Response.Redirect("~/finalization.aspx?id=" + attempt.paymentAttemptId.ToString());
        }

        public virtual void OnError(Exception ex, string logString, LogFileEntryType logFileEntryType)
        {
            GenericHelper.LogFile(logString, logFileEntryType);
            GenericHelper.RedirectToErrorPage("Ocorreu um erro no processamento da transação: " + ex.Message);
        }
    }
}
