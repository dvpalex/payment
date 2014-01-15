using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SuperPag.Business.Messages;
using System.Data;
using System.Data.Common;

namespace SuperPag.Business
{
    public class PaymentAttemptContaCorrente
    {
        Database db = DatabaseFactory.CreateDatabase("fastpag");

        #region Shared/Static Methods

        static PaymentAttemptContaCorrente ObjPaymentAttemptContaCorrente = null;

        static PaymentAttemptContaCorrente()
        {
            CreateInstance();
        }

        private static void CreateInstance()
        {
            ObjPaymentAttemptContaCorrente = new PaymentAttemptContaCorrente();
        }

        public static PaymentAttemptContaCorrente GetInstance()
        {
            return ObjPaymentAttemptContaCorrente;
        }

        #endregion

        public int Insert(MPaymentAttemptContaCorrente ObjMPaymentAttemptContaCorrente)
        {

            DbCommand dbCommand = db.GetStoredProcCommand("ProcInsertPaymentAttemptContaCorrente");

            db.AddInParameter(dbCommand, "@PaymentAttemptId", DbType.Guid, ObjMPaymentAttemptContaCorrente.PaymentAttemptId);
            db.AddInParameter(dbCommand, "@Ocorrencia", DbType.String, ObjMPaymentAttemptContaCorrente.Ocorrencia);
            db.AddInParameter(dbCommand, "@CodigoLogo", DbType.Int32, ObjMPaymentAttemptContaCorrente.CodigoLogo);
            db.AddInParameter(dbCommand, "@Plastico", DbType.String, ObjMPaymentAttemptContaCorrente.Plastico);
            db.AddInParameter(dbCommand, "@DigVerAg", DbType.String, ObjMPaymentAttemptContaCorrente.DigVerAg);
            db.AddInParameter(dbCommand, "@NumAgencia", DbType.String, ObjMPaymentAttemptContaCorrente.NumAgencia);
            db.AddInParameter(dbCommand, "@Status", DbType.String, ObjMPaymentAttemptContaCorrente.Status);
            db.AddInParameter(dbCommand, "@DigVerCont", DbType.String, ObjMPaymentAttemptContaCorrente.DigVerCont);
            db.AddInParameter(dbCommand, "@NumContCorrent", DbType.String, ObjMPaymentAttemptContaCorrente.NumContCorrent);
            db.AddInParameter(dbCommand, "@DataVencimento", DbType.DateTime, ObjMPaymentAttemptContaCorrente.DataVencimento);
            db.AddInParameter(dbCommand, "@ControleEntradaId", DbType.Int32, ObjMPaymentAttemptContaCorrente.ControleEntradaId);
            //db.AddInParameter(dbCommand, "@DataPagamento", DbType.DateTime, ObjMPaymentAttemptContaCorrente.DataPagamento);
            db.AddInParameter(dbCommand, "@ControleSaidaId", DbType.Int32, ObjMPaymentAttemptContaCorrente.ControleSaidaId);
            db.AddInParameter(dbCommand, "@ValorAgendado", DbType.Decimal, ObjMPaymentAttemptContaCorrente.ValorAgendado);
            db.AddInParameter(dbCommand, "@NumInstituicao", DbType.Int32, ObjMPaymentAttemptContaCorrente.NumInstituicao);
            return Convert.ToInt32(db.ExecuteScalar(dbCommand));

        }
        public MPaymentAttemptContaCorrente Locate(Guid paymentAttemptId)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("ProcSelectPaymentAttemptContaCorrente");
            db.AddInParameter(dbCommand, "@PaymentAttemptId", DbType.Guid, paymentAttemptId);
            MPaymentAttemptContaCorrente ObjMPaymentAttemptContaCorrente = null;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    return new MPaymentAttemptContaCorrente(dr.GetGuid(dr.GetOrdinal("paymentAttemptId")),
                                        dr.GetString(dr.GetOrdinal("Ocorrencia")),
                                        dr.GetInt32(dr.GetOrdinal("NrDocum")),
                                        dr.IsDBNull(dr.GetOrdinal("CodigoLogo")) ? 0 : dr.GetInt32(dr.GetOrdinal("CodigoLogo")),
                                        dr.IsDBNull(dr.GetOrdinal("Plastico")) ? string.Empty : dr.GetString(dr.GetOrdinal("Plastico")),
                                        dr.GetString(dr.GetOrdinal("DigVerAg")),
                                        dr.GetString(dr.GetOrdinal("NumAgencia")),
                                        dr.GetInt32(dr.GetOrdinal("Status")),
                                        dr.GetString(dr.GetOrdinal("DigVerCont")),
                                        dr.GetString(dr.GetOrdinal("NumContCorrent")),
                                        dr.GetDateTime(dr.GetOrdinal("DataVencimento")),
                                        dr.IsDBNull(dr.GetOrdinal("ControleEntradaId")) ? new int?() : dr.GetInt32(dr.GetOrdinal("ControleEntradaId")),
                                        dr.IsDBNull(dr.GetOrdinal("ControleSaidaId")) ? new int?() : dr.GetInt32(dr.GetOrdinal("ControleSaidaId")),
                                        dr.GetDecimal(dr.GetOrdinal("ValorAgendado")),
                                        dr.IsDBNull(dr.GetOrdinal("CnabControleEntradaId")) ? new int?() : dr.GetInt32(dr.GetOrdinal("CnabControleEntradaId")),
                                        dr.IsDBNull(dr.GetOrdinal("CnabControleSaidaId")) ? new int?() : dr.GetInt32(dr.GetOrdinal("CnabControleSaidaId")),
                                        dr.IsDBNull(dr.GetOrdinal("DataProcessamento")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataProcessamento")),
                                        dr.IsDBNull(dr.GetOrdinal("OcorrenciaSuperpag")) ? string.Empty : dr.GetString(dr.GetOrdinal("OcorrenciaSuperpag")),
                                        dr.IsDBNull(dr.GetOrdinal("NumInstituicao")) ? new int?() : dr.GetInt32(dr.GetOrdinal("NumInstituicao"))
                                        );
                }
            }
            return ObjMPaymentAttemptContaCorrente;
        }

        public MPaymentAttemptContaCorrente Locate(int NrDocum)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_SelectPaymentAttemptContaCorrenteByNrDocum");
            db.AddInParameter(dbCommand, "@NrDocum", DbType.Int32, NrDocum);
            MPaymentAttemptContaCorrente ObjMPaymentAttemptContaCorrente = null;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    return new MPaymentAttemptContaCorrente(dr.GetGuid(dr.GetOrdinal("paymentAttemptId")),
                                                            dr.GetString(dr.GetOrdinal("Ocorrencia")),
                                                            dr.GetInt32(dr.GetOrdinal("NrDocum")),
                                                            dr.IsDBNull(dr.GetOrdinal("CodigoLogo")) ? 0 : dr.GetInt32(dr.GetOrdinal("CodigoLogo")),
                                                            dr.IsDBNull(dr.GetOrdinal("Plastico")) ? string.Empty : dr.GetString(dr.GetOrdinal("Plastico")),
                                                            dr.GetString(dr.GetOrdinal("DigVerAg")),
                                                            dr.GetString(dr.GetOrdinal("NumAgencia")),
                                                            dr.GetInt32(dr.GetOrdinal("Status")),
                                                            dr.GetString(dr.GetOrdinal("DigVerCont")),
                                                            dr.GetString(dr.GetOrdinal("NumContCorrent")),
                                                            dr.GetDateTime(dr.GetOrdinal("DataVencimento")),
                                                            dr.IsDBNull(dr.GetOrdinal("ControleEntradaId")) ? new int?() : dr.GetInt32(dr.GetOrdinal("ControleEntradaId")),
                                                            dr.IsDBNull(dr.GetOrdinal("ControleSaidaId")) ? new int?() : dr.GetInt32(dr.GetOrdinal("ControleSaidaId")),
                                                            dr.GetDecimal(dr.GetOrdinal("ValorAgendado")),
                                                            dr.IsDBNull(dr.GetOrdinal("CnabControleEntradaId")) ? new int?() : dr.GetInt32(dr.GetOrdinal("CnabControleEntradaId")),
                                                            dr.IsDBNull(dr.GetOrdinal("CnabControleSaidaId")) ? new int?() : dr.GetInt32(dr.GetOrdinal("CnabControleSaidaId")),
                                                            dr.IsDBNull(dr.GetOrdinal("DataProcessamento")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataProcessamento")),
                                                            dr.GetString(dr.GetOrdinal("OcorrenciaSuperpag")),
                                                            dr.IsDBNull(dr.GetOrdinal("NumInstituicao")) ? new int?() : dr.GetInt32(dr.GetOrdinal("NumInstituicao"))
                                                            );
                }
            }
            return ObjMPaymentAttemptContaCorrente;
        }

        public bool Update(MPaymentAttemptContaCorrente ObjMPaymentAttemptContaCorrente)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("ProcUpdatePaymentAttemptContaCorrente");

            db.AddInParameter(dbCommand, "@PaymentAttemptId", DbType.Guid, ObjMPaymentAttemptContaCorrente.PaymentAttemptId);
            db.AddInParameter(dbCommand, "@Ocorrencia", DbType.String, ObjMPaymentAttemptContaCorrente.Ocorrencia);
            //db.AddInParameter(dbCommand, "@NrDocum", DbType.Int32, ObjMPaymentAttemptContaCorrente.NrDocum);
            db.AddInParameter(dbCommand, "@CodigoLogo", DbType.Int32, ObjMPaymentAttemptContaCorrente.CodigoLogo);
            db.AddInParameter(dbCommand, "@Plastico", DbType.String, ObjMPaymentAttemptContaCorrente.Plastico);
            db.AddInParameter(dbCommand, "@DigVerAg", DbType.String, ObjMPaymentAttemptContaCorrente.DigVerAg);
            db.AddInParameter(dbCommand, "@NumAgencia", DbType.String, ObjMPaymentAttemptContaCorrente.NumAgencia);
            db.AddInParameter(dbCommand, "@Status", DbType.String, ObjMPaymentAttemptContaCorrente.Status);
            db.AddInParameter(dbCommand, "@DigVerCont", DbType.String, ObjMPaymentAttemptContaCorrente.DigVerCont);
            db.AddInParameter(dbCommand, "@NumContCorrent", DbType.String, ObjMPaymentAttemptContaCorrente.NumContCorrent);
            db.AddInParameter(dbCommand, "@DataProcessamento", DbType.DateTime, ObjMPaymentAttemptContaCorrente.DataVencimento);
            db.AddInParameter(dbCommand, "@DataVencimento", DbType.DateTime, ObjMPaymentAttemptContaCorrente.DataVencimento);
            db.AddInParameter(dbCommand, "@ControleEntradaId", DbType.Int32, ObjMPaymentAttemptContaCorrente.ControleEntradaId);
            db.AddInParameter(dbCommand, "@NumInstituicao", DbType.Int32, ObjMPaymentAttemptContaCorrente.NumInstituicao);
            db.AddInParameter(dbCommand, "@CnabControleSaidaId", DbType.Int32, ObjMPaymentAttemptContaCorrente.CnabControleSaidaId);
            if (ObjMPaymentAttemptContaCorrente.ControleSaidaId != null)
            {
                db.AddInParameter(dbCommand, "@ControleSaidaId", DbType.Int32, ObjMPaymentAttemptContaCorrente.ControleSaidaId);
            }

            return Convert.ToBoolean(db.ExecuteNonQuery(dbCommand));
        }
        public void UpdateByNrDocum(MPaymentAttemptContaCorrente ObjMPaymentAttemptContaCorrente)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_UpdateByNrDocum");

            db.AddInParameter(dbCommand, "@Ocorrencia", DbType.String, ObjMPaymentAttemptContaCorrente.Ocorrencia);
            db.AddInParameter(dbCommand, "@OcorrenciaSuperpag", DbType.String, ObjMPaymentAttemptContaCorrente.OcorrenciaSuperpag);
            db.AddInParameter(dbCommand, "@NrDocum", DbType.Int32, ObjMPaymentAttemptContaCorrente.NrDocum);
            db.AddInParameter(dbCommand, "@Status", DbType.String, ObjMPaymentAttemptContaCorrente.Status);

            db.ExecuteNonQuery(dbCommand);
        }
        public void UpdateByCnabControleEntradaId(int CnabControleEntradaId, int ControleEntradaId)
        {
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_UpdateContaCorrenteByCnabControleEntradaId");

            db.AddInParameter(dbCommand, "@CnabControleEntradaId", DbType.Int32, CnabControleEntradaId);
            db.AddInParameter(dbCommand, "@ControleEntradaId", DbType.Int32, ControleEntradaId);

            db.ExecuteNonQuery(dbCommand);
        }

        public static IList<MPaymentAttemptContaCorrente> getRelatorioAnalitico(
                                                                                    Int32 TipoData, DateTime dtInicial, DateTime dtFinal 
                                                                               )
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaotrans");

            db.AddInParameter(dbCommand, "@TipoData", DbType.Int32, TipoData);

            if (dtInicial != null && dtFinal != null)
            {
                db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, dtInicial);
                db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, dtFinal);
            }

            IList<MPaymentAttemptContaCorrente> objLista = new List<MPaymentAttemptContaCorrente>(); ;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        objLista.Add(new MPaymentAttemptContaCorrente(dr.GetGuid(dr.GetOrdinal("paymentAttemptId")),
                                                                dr.IsDBNull(dr.GetOrdinal("NumInstituicao")) ? new Int32?() : dr.GetInt32(dr.GetOrdinal("NumInstituicao")),
                                                                dr.GetDateTime(dr.GetOrdinal("DataVencimento")),
                                                                dr.GetInt32(dr.GetOrdinal("Status")),
                                                                dr.IsDBNull(dr.GetOrdinal("DsStatus")) ? string.Empty : dr.GetString(dr.GetOrdinal("DsStatus")),
                                                                dr.GetDecimal(dr.GetOrdinal("ValorAgendado")),
                                                                null,
                                                                dr.IsDBNull(dr.GetOrdinal("DataArquivo")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataArquivo")),
                                                                dr.IsDBNull(dr.GetOrdinal("OcorrenciaSuperPag")) ? string.Empty : dr.GetString(dr.GetOrdinal("OcorrenciaSuperPag")),
                                                                dr.GetString(dr.GetOrdinal("Ocorrencia")),
                                                                dr.IsDBNull(dr.GetOrdinal("NumAgencia")) ? string.Empty : dr.GetString(dr.GetOrdinal("NumAgencia")),
                                                                dr.GetString(dr.GetOrdinal("NumContCorrent")),
                                                                dr.IsDBNull(dr.GetOrdinal("DigVerCont")) ? string.Empty : dr.GetString(dr.GetOrdinal("DigVerCont")),
                                                                dr.IsDBNull(dr.GetOrdinal("Plastico")) ? string.Empty : dr.GetString(dr.GetOrdinal("Plastico")),
                                                                dr.IsDBNull(dr.GetOrdinal("DataCredito")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataCredito"))
                                                                ))
                                                                ;
                    }
                }
            }
            return objLista;
        }

        public static IList<MPaymentAttemptContaCorrente> getRelatorioAnalitico(
                                                                                    String nomeArquivo
                                                                               )
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaotrans_arquivo");

            db.AddInParameter(dbCommand, "@Arquivo", DbType.String, nomeArquivo);

            IList<MPaymentAttemptContaCorrente> objLista = new List<MPaymentAttemptContaCorrente>(); ;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        objLista.Add(new MPaymentAttemptContaCorrente(dr.GetGuid(dr.GetOrdinal("paymentAttemptId")),
                                                                dr.IsDBNull(dr.GetOrdinal("NumInstituicao")) ? new Int32?() : dr.GetInt32(dr.GetOrdinal("NumInstituicao")),
                                                                dr.GetDateTime(dr.GetOrdinal("DataVencimento")),
                                                                dr.GetInt32(dr.GetOrdinal("Status")),
                                                                dr.IsDBNull(dr.GetOrdinal("DsStatus")) ? string.Empty : dr.GetString(dr.GetOrdinal("DsStatus")),
                                                                dr.GetDecimal(dr.GetOrdinal("ValorAgendado")),
                                                                dr.IsDBNull(dr.GetOrdinal("DataProcessamento")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataProcessamento")),
                                                                dr.IsDBNull(dr.GetOrdinal("DataArquivo")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataArquivo")),
                                                                dr.IsDBNull(dr.GetOrdinal("OcorrenciaSuperPag")) ? string.Empty : dr.GetString(dr.GetOrdinal("OcorrenciaSuperPag")),
                                                                dr.GetString(dr.GetOrdinal("Ocorrencia")),
                                                                dr.IsDBNull(dr.GetOrdinal("NumAgencia")) ? string.Empty : dr.GetString(dr.GetOrdinal("NumAgencia")),
                                                                dr.GetString(dr.GetOrdinal("NumContCorrent")),
                                                                dr.IsDBNull(dr.GetOrdinal("DigVerCont")) ? string.Empty : dr.GetString(dr.GetOrdinal("DigVerCont")),
                                                                dr.IsDBNull(dr.GetOrdinal("Plastico")) ? string.Empty : dr.GetString(dr.GetOrdinal("Plastico")),
                                                                dr.IsDBNull(dr.GetOrdinal("DataCredito")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataCredito"))
                                                                ));
                    }
                }
            }
            return objLista;
        }

        public static IList<MPaymentAttemptContaCorrente> getRelatorioAnalitico(
                                                                String nomeArquivo, int numeroInstituicao)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaotrans_arquivo_instituicao");
            db.AddInParameter(dbCommand, "@Arquivo", DbType.String, nomeArquivo);
            db.AddInParameter(dbCommand, "@NumInstituicao", DbType.Int32, numeroInstituicao);

            IList<MPaymentAttemptContaCorrente> objLista = new List<MPaymentAttemptContaCorrente>(); ;
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            objLista.Add(new MPaymentAttemptContaCorrente(dr.GetGuid(dr.GetOrdinal("paymentAttemptId")),
                            dr.IsDBNull(dr.GetOrdinal("NumInstituicao")) ? new Int32?() : dr.GetInt32(dr.GetOrdinal("NumInstituicao")),
                            dr.GetDateTime(dr.GetOrdinal("DataVencimento")),
                            dr.GetInt32(dr.GetOrdinal("Status")),
                            dr.IsDBNull(dr.GetOrdinal("DsStatus")) ? string.Empty : dr.GetString(dr.GetOrdinal("DsStatus")),
                            dr.GetDecimal(dr.GetOrdinal("ValorAgendado")),
                            dr.IsDBNull(dr.GetOrdinal("DataProcessamento")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataProcessamento")),
                            dr.IsDBNull(dr.GetOrdinal("DataArquivo")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataArquivo")),
                            dr.IsDBNull(dr.GetOrdinal("OcorrenciaSuperPag")) ? string.Empty : dr.GetString(dr.GetOrdinal("OcorrenciaSuperPag")),
                            dr.GetString(dr.GetOrdinal("Ocorrencia")),
                            dr.IsDBNull(dr.GetOrdinal("NumAgencia")) ? string.Empty : dr.GetString(dr.GetOrdinal("NumAgencia")),
                            dr.GetString(dr.GetOrdinal("NumContCorrent")),
                            dr.IsDBNull(dr.GetOrdinal("DigVerCont")) ? string.Empty : dr.GetString(dr.GetOrdinal("DigVerCont")),
                            dr.IsDBNull(dr.GetOrdinal("Plastico")) ? string.Empty : dr.GetString(dr.GetOrdinal("Plastico")),
                            dr.IsDBNull(dr.GetOrdinal("DataCredito")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataCredito"))
                            ));
                        }
                    }
            }
            return objLista;
        }


        public static IList<MRelatorioResumoTransacoes> getRelatorioSintetico(
                                                                                 Int32 TipoData, DateTime dtInicial, DateTime dtFinal
                                                                                 
                                                                               )
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaorestrans");
            
            db.AddInParameter(dbCommand, "@TipoData", DbType.Int32, TipoData);
          
            DateTime dt = new DateTime();
            if (dtInicial != dt && dtFinal != dt)
            {

                db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, dtInicial);
                db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, dtFinal);
            }
            IList<MRelatorioResumoTransacoes> objLista = new List<MRelatorioResumoTransacoes>(); ;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        // Alteração - Jair Jersey Marinho - 22/10/2008
                        // Incluindo Arquivo

                        objLista.Add(new MRelatorioResumoTransacoes(
                                                                Convert.ToInt32(dr["NumInstituicao"]),
                                                                dr["NomeArquivoBanco"].ToString(),
                                                                Convert.ToInt32(dr["QtdeTransacao"]),
                                                                Convert.ToDecimal(dr["TotalTransacao"]),
                                                                Convert.ToInt32(dr["QtdePagas"]),
                                                                Convert.ToDecimal(dr["TotalPagas"]),
                                                                Convert.ToInt32(dr["QtdeRecusadas"]),
                                                                Convert.ToDecimal(dr["TotalRecusadas"]),
                                                                Convert.ToInt32(dr["QtdeReceber"]),
                                                                Convert.ToDecimal(dr["TotalReceber"])
                                                                ));
                    }
                }
            }
            return objLista;
        }


  
        public static IList<MRelatorioContabilizacaoMatera> getRelatorioContabilizacaoMatera(
                                                                                 Int32 Tipo, DateTime dtInicial, DateTime dtFinal
                                                                               )
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_contmat");

            db.AddInParameter(dbCommand, "@Tipo", DbType.Int32, Tipo);
            DateTime dt = new DateTime();
            if (dtInicial != dt && dtFinal != dt)
            {

                db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, dtInicial);
                db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, dtFinal);
            }
            IList<MRelatorioContabilizacaoMatera> objLista = new List<MRelatorioContabilizacaoMatera>(); ;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        objLista.Add(new MRelatorioContabilizacaoMatera(
                                                                dr["NumBanco"].ToString(),
                                                                dr["NomeBanco"].ToString(),
                                                                dr["ContaBanco"].ToString(),
                                                                dr["ContaTransit"].ToString(),
                                                                dr.IsDBNull(dr.GetOrdinal("DataProcessamento")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataProcessamento")),
                                                                dr.IsDBNull(dr.GetOrdinal("DataVencimento")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataVencimento")),
                                                                dr.IsDBNull(dr.GetOrdinal("DataCredito")) ? new DateTime?() : dr.GetDateTime(dr.GetOrdinal("DataCredito")),
                                                                dr.IsDBNull(dr.GetOrdinal("Valor")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Valor")),
                                                                dr.IsDBNull(dr.GetOrdinal("SubTotalBanco")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SubTotalBanco"))
                                                                ));
                    }
                }
            }
            return objLista;
        }


    }
}


