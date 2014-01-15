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
    public class PaymentAttemptBoleto
    {
        public static MPaymentAttemptBoleto Locate(Guid paymentAttemptId)
        {
            DPaymentAttemptBoleto dPaymentAttemptBoleto = DataFactory.PaymentAttemptBoleto().Locate(paymentAttemptId);

            MessageMapper mapper = new MessageMapper();
            MPaymentAttemptBoleto mPaymentAttemptBoleto = (MPaymentAttemptBoleto)mapper.Do(dPaymentAttemptBoleto, typeof(MPaymentAttemptBoleto));
            return mPaymentAttemptBoleto;
        }

        public static void Insert(DPaymentAttemptBoleto ObjDPaymentAttemptBoleto)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_InsertBoleto");
            dbCommand.CommandTimeout = 80;

            db.AddInParameter(dbCommand, "@documentNumber", DbType.String, ObjDPaymentAttemptBoleto.documentNumber);
            db.AddInParameter(dbCommand, "@withdraw", DbType.String, ObjDPaymentAttemptBoleto.withdraw);
            db.AddInParameter(dbCommand, "@withdrawDoc", DbType.String, ObjDPaymentAttemptBoleto.withdrawDoc);
            db.AddInParameter(dbCommand, "@address1", DbType.String, ObjDPaymentAttemptBoleto.address1);
            db.AddInParameter(dbCommand, "@address2", DbType.String, ObjDPaymentAttemptBoleto.address2);
            db.AddInParameter(dbCommand, "@address3", DbType.String, ObjDPaymentAttemptBoleto.address3);
            db.AddInParameter(dbCommand, "@oct", DbType.String, ObjDPaymentAttemptBoleto.oct);
            db.AddInParameter(dbCommand, "@barCode", DbType.String, ObjDPaymentAttemptBoleto.barCode);
            db.AddInParameter(dbCommand, "@ourNumber", DbType.String, ObjDPaymentAttemptBoleto.ourNumber);
            db.AddInParameter(dbCommand, "@instructions", DbType.String, ObjDPaymentAttemptBoleto.instructions);
            db.AddInParameter(dbCommand, "@paymentDate", DbType.DateTime, ObjDPaymentAttemptBoleto.paymentDate);
            db.AddInParameter(dbCommand, "@expirationPaymentDate", DbType.DateTime, ObjDPaymentAttemptBoleto.expirationPaymentDate);
            db.AddInParameter(dbCommand, "@paymentAttemptBoletoReturnId", DbType.Int32, ObjDPaymentAttemptBoleto.paymentAttemptBoletoReturnId);
            db.AddInParameter(dbCommand, "@UserId", DbType.Guid, ObjDPaymentAttemptBoleto.UserId);
            db.AddInParameter(dbCommand, "@paymentAttemptId", DbType.Guid, ObjDPaymentAttemptBoleto.paymentAttemptId);
            db.AddInParameter(dbCommand, "@Contrato", DbType.String, ObjDPaymentAttemptBoleto.Contrato);
            db.AddInParameter(dbCommand, "@Status", DbType.Boolean, ObjDPaymentAttemptBoleto.Status);

            db.ExecuteNonQuery(dbCommand);
        }

        //public static IList<MRelPontoCredAnalitico> GetRelBoleto(DateTime DataInicial, DateTime DataFinal, int StoreId)
        public static IList<MRelPontoCredAnalitico> GetRelBoleto(DateTime DataInicial, DateTime DataFinal, int StoreId, string NomeSacado, string MeioEnvio, string IPTE, string Contrato)
        {
            //Lista de Objetos da classe Entidade
            IList<MRelPontoCredAnalitico> ObjLista = new List<MRelPontoCredAnalitico>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetRelBoleto");
            dbCommand.CommandTimeout = 900;

            db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, DataInicial);
            db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, DataFinal);
            db.AddInParameter(dbCommand, "@storeId", DbType.Int32, StoreId);

            if (NomeSacado != String.Empty)
                db.AddInParameter(dbCommand, "@NomeSacado", DbType.String, NomeSacado);
            if (MeioEnvio != String.Empty && MeioEnvio != "Todos" && MeioEnvio != "Selecione")
                db.AddInParameter(dbCommand, "@MeioEnvio", DbType.String, MeioEnvio);
            if (IPTE != String.Empty)
                db.AddInParameter(dbCommand, "@IPTE", DbType.String, IPTE);
            if (Contrato != String.Empty)
                db.AddInParameter(dbCommand, "@Contrato", DbType.String, Contrato);

            //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    ObjLista.Add(new MRelPontoCredAnalitico(dr.GetDateTime(dr.GetOrdinal("PaymentDate")),
                    dr.GetString(dr.GetOrdinal("UserName")),
                    ("'" + dr.GetString(dr.GetOrdinal("CodIPTE"))),
                    dr.GetDateTime(dr.GetOrdinal("ExpirationPaymentDate")),
                    (dr.IsDBNull(dr.GetOrdinal("MeioEnvio")) ? String.Empty : dr.GetString(dr.GetOrdinal("MeioEnvio"))),
                    (dr.IsDBNull(dr.GetOrdinal("Sacado")) ? String.Empty : dr.GetString(dr.GetOrdinal("Sacado"))),
                    (dr.IsDBNull(dr.GetOrdinal("Contrato")) ? String.Empty : dr.GetString(dr.GetOrdinal("Contrato"))),
                    (dr.IsDBNull(dr.GetOrdinal("Status")) ? true : dr.GetBoolean(dr.GetOrdinal("Status"))),
                    (dr.IsDBNull(dr.GetOrdinal("ErrorMail")) ? String.Empty : dr.GetString(dr.GetOrdinal("ErrorMail")))
                    ));
                }
            }
            return ObjLista;
        }
    }
}
