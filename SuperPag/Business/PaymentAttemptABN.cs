using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace SuperPag.Business
{
    public class PaymentAttemptABN
    {
        public static IList<MPaymentAttemptABN> ListSonda(string statusProposta)
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MPaymentAttemptABN> ObjLista = new List<MPaymentAttemptABN>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_ListSonda");
            db.AddInParameter(dbCommand, "@statusProposta", DbType.String, statusProposta);

            //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                {
                    //Popula ObjLista
                    ObjLista.Add(new MPaymentAttemptABN(
                                                        dr.GetGuid(dr.GetOrdinal("paymentAttemptId")),
                                                        (dr.IsDBNull(dr.GetOrdinal("agentOrderReference")) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("agentOrderReference"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("numControle")) ? int.MinValue : Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("numControle")))),
                                                        (dr.IsDBNull(dr.GetOrdinal("numProposta")) ? string.Empty : dr.GetString(dr.GetOrdinal("numProposta"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("statusProposta")) ? string.Empty : dr.GetString(dr.GetOrdinal("statusProposta"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("qtdPrestacao")) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("qtdPrestacao"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("prestacao")) ? int.MinValue : Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("prestacao")))),
                                                        (dr.IsDBNull(dr.GetOrdinal("tabelaFinanciamento")) ? string.Empty : dr.GetString(dr.GetOrdinal("tabelaFinanciamento"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("tipoPessoa")) ? string.Empty : dr.GetString(dr.GetOrdinal("tipoPessoa"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("garantia")) ? string.Empty : dr.GetString(dr.GetOrdinal("garantia"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("valorEntrada")) ? int.MinValue : Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("valorEntrada")))),
                                                        (dr.IsDBNull(dr.GetOrdinal("dataVencimento")) ? DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("dataVencimento"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("codRet")) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("codRet"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("msgRet")) ? string.Empty : dr.GetString(dr.GetOrdinal("msgRet"))),
                                                        (dr.IsDBNull(dr.GetOrdinal("abnStatus")) ? int.MinValue : dr.GetInt32(dr.GetOrdinal("abnStatus")))
                                                        ));
                }
            }
            return ObjLista;
        }
        public static void Update(MPaymentAttemptABN ObjAttemptABN)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_UpdatePaymentAttemptABN");

            db.AddInParameter(dbCommand, "@paymentAttemptId", DbType.String, ObjAttemptABN.PaymentAttemptId);
            db.AddInParameter(dbCommand, "@numControle", DbType.String, ObjAttemptABN.NumControle);
            db.AddInParameter(dbCommand, "@numProposta", DbType.String, ObjAttemptABN.NumProposta);
            db.AddInParameter(dbCommand, "@statusProposta", DbType.String, ObjAttemptABN.StatusProposta);
            db.AddInParameter(dbCommand, "@qtdPrestacao", DbType.String, ObjAttemptABN.QtdPrestacao);
            db.AddInParameter(dbCommand, "@prestacao", DbType.String, ObjAttemptABN.Prestacao);
            db.AddInParameter(dbCommand, "@tabelaFinanciamento", DbType.String, ObjAttemptABN.TabelaFinanciamento);
            db.AddInParameter(dbCommand, "@tipoPessoa", DbType.String, ObjAttemptABN.TipoPessoa);
            db.AddInParameter(dbCommand, "@garantia", DbType.String, ObjAttemptABN.Garantia);
            db.AddInParameter(dbCommand, "@valorEntrada", DbType.String, ObjAttemptABN.ValorEntrada);
            db.AddInParameter(dbCommand, "@dataVencimento", DbType.String, ObjAttemptABN.DataVencimento);
            db.AddInParameter(dbCommand, "@codRet", DbType.String, ObjAttemptABN.CodRet);
            db.AddInParameter(dbCommand, "@msgRet", DbType.String, ObjAttemptABN.MsgRet);
            db.AddInParameter(dbCommand, "@abnStatus", DbType.String, ObjAttemptABN.AbnStatus);

            db.ExecuteNonQuery(dbCommand);
        }
    }
}
