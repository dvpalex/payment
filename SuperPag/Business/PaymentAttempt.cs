using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
//EnterpriseLibrary
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace SuperPag.Business
{
    public class PaymentAttempt
    {
        public static void Update(MPaymentAttempt mPaymentAttempt)
        {
            MessageMapper mapper = new MessageMapper();
            DPaymentAttempt dPaymentAttempt = (DPaymentAttempt)mapper.Do(mPaymentAttempt, typeof(DPaymentAttempt));
            dPaymentAttempt.orderId = mPaymentAttempt.Order.OrderId;
            dPaymentAttempt.paymentFormId = mPaymentAttempt.PaymentForm.PaymentFormId;
            DataFactory.PaymentAttempt().Update(dPaymentAttempt);
        }

        public static MCPaymentAttempt List(long orderId)
        {
            MCPaymentAttempt mcPaymentAttempt = null;
            DPaymentAttemptComplete[] arrDPaymentAttemptComplete = DataFactory.PaymentAttempt().ListComplete(orderId);

            if (arrDPaymentAttemptComplete != null)
            {
                MessageMapper mapper = new MessageMapper();
                mapper.MapChildren = true;
                mcPaymentAttempt = (MCPaymentAttempt)mapper.Do(arrDPaymentAttemptComplete, typeof(MCPaymentAttempt));
            }
            else
                mcPaymentAttempt = new MCPaymentAttempt();

            return mcPaymentAttempt;
        }

        public static MPaymentAttempt Locate(Guid paymentAttemptId)
        {
            DPaymentAttempt dPaymentAttempt = DataFactory.PaymentAttempt().Locate(paymentAttemptId);
            MessageMapper mapper = new MessageMapper();
            MPaymentAttempt mPaymentAttempt = (MPaymentAttempt)mapper.Do(dPaymentAttempt, typeof(MPaymentAttempt));
            if (mPaymentAttempt != null)
                return mPaymentAttempt;
            else
                return new MPaymentAttempt();
        }

        //Retorna o email do cliente, para enviar o Boleto por e-mail
        public static string RetunEmailPaymentAttempt(Guid PaymentAttemptId)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_EnviaBoleto");
            db.AddInParameter(dbCommand, "@PaymentAttenptId ", DbType.Guid, PaymentAttemptId);

            string Email = (string)db.ExecuteScalar(dbCommand);
            return Email;
        }
            
    }
}
