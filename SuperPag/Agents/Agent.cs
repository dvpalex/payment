using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Helper;
using SuperPag.Data.Messages;
using SuperPag.Data;

namespace SuperPag.Agents
{
    public class Agent : IAgent
    {
        public DPaymentAttempt attempt;
        public DOrder order;
        public DStorePaymentInstallment storePaymentInstallment;
        public DPaymentForm paymentForm;

        public Agent() { }
        public Agent(Guid paymentAttemptId)
        {
            this.attempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
            if (this.attempt != null)
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
            //Simulação
            if (attempt.isSimulation)
                Simulate();
        }

        public virtual void Finish()
        {
        }

        public virtual void Simulate()
        {
            attempt.lastUpdate = DateTime.Now;
            attempt.status = (int)PaymentAttemptStatus.Paid;
            DataFactory.PaymentAttempt().Update(attempt);
            GenericHelper.UpdateOrderStatusByAttemptStatus(order, attempt.status);
        }

        public virtual void OnError(Exception ex, string logString, LogFileEntryType logFileEntryType)
        {
            GenericHelper.LogFile(logString, logFileEntryType);
            GenericHelper.RedirectToErrorPage("Ocorreu um erro no processamento da transação: " + ex.Message);
        }
    }
}
