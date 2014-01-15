using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace SuperPag.Business.Messages
{
    public class NumberSNA
    {
        Database db = DatabaseFactory.CreateDatabase("fastpag");

        //Retorna um o proximo sequencia
        public Int32 getSNA(int PaymentAgentSetupID)
        {

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetSNA");
            db.AddInParameter(dbCommand, "@PaymentAgentSetupID", DbType.Int32, PaymentAgentSetupID);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
               return dr.GetInt32(dr.GetOrdinal("NumSeq"));
            }

            return -1;
        }

        //Fechar o Sequencia
        public bool completeSNA(int PaymentAgentSetupID)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_CompleteSNA");
            db.AddInParameter(dbCommand, "@PaymentAgentSetupID", DbType.Int32, PaymentAgentSetupID);

            return Convert.ToBoolean(db.ExecuteNonQuery(dbCommand));

        }


    }
}
