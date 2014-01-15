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
    public class PaymentAttemptVBV
    {
        public static MPaymentAttemptVBV Locate(Guid paymentAttemptId)
        {
            DPaymentAttemptVBV dPaymentAttemptVBV = DataFactory.PaymentAttemptVBV().Locate(paymentAttemptId);

            MessageMapper mapper = new MessageMapper();
            MPaymentAttemptVBV mPaymentAttemptVBV = (MPaymentAttemptVBV)mapper.Do(dPaymentAttemptVBV, typeof(MPaymentAttemptVBV));
            return mPaymentAttemptVBV;
        }

        /// <summary>
        /// Verifica se o Tid ja existe antes de gerar
        /// </summary>
        /// <param name="Tid"></param>
        /// <returns></returns>
        public static bool CheckTid(string Tid)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_CheckTid");
            db.AddInParameter(dbCommand, "@tid", DbType.String, Tid);
             
            if (db.ExecuteScalar(dbCommand) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
