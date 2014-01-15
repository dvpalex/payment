using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using SuperPag.Business.Messages;

namespace SuperPag.Business
{
    public class CnabControleSaida
    {


        public static int Insert(MCnabControleSaida ObjMCnabControleSaida)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_InsertCnabControleSaida");

            db.AddInParameter(dbCommand, "@NumInstituicao", DbType.Int32, ObjMCnabControleSaida.NumInstituicao);
            db.AddInParameter(dbCommand, "@DataArquivo", DbType.DateTime, ObjMCnabControleSaida.DataArquivo);
            db.AddInParameter(dbCommand, "@NumSeqRemessa", DbType.Int32, ObjMCnabControleSaida.NumSeqRemessa);
            db.AddInParameter(dbCommand, "@NomeArquivo", DbType.String, ObjMCnabControleSaida.NomeArquivo);
            db.AddInParameter(dbCommand, "@NomeArquivoArmazenado", DbType.String, ObjMCnabControleSaida.NomeArquivoArmazenado);
            db.AddInParameter(dbCommand, "@QtdeDetalhes", DbType.Int32, ObjMCnabControleSaida.QtdeDetalhes);
            db.AddInParameter(dbCommand, "@ValorTotalDetalhes", DbType.Decimal, ObjMCnabControleSaida.ValorTotalDetalhes);
            db.AddInParameter(dbCommand, "@Status", DbType.Int32, ObjMCnabControleSaida.Status);
            db.AddInParameter(dbCommand, "@Ocorrencia", DbType.String, ObjMCnabControleSaida.Ocorrencia);

            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }

        public static void Update(MCnabControleSaida ObjMCnabControleSaida)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_UpdateCnabControleSaida");

            db.AddInParameter(dbCommand, "@CnabControleSaidaId", DbType.Int32, ObjMCnabControleSaida.CnabControleSaidaId);
            db.AddInParameter(dbCommand, "@NumInstituicao", DbType.Int32, ObjMCnabControleSaida.NumInstituicao);
            db.AddInParameter(dbCommand, "@DataArquivo", DbType.DateTime, ObjMCnabControleSaida.DataArquivo);
            db.AddInParameter(dbCommand, "@NumSeqRemessa", DbType.Int32, ObjMCnabControleSaida.NumSeqRemessa);
            db.AddInParameter(dbCommand, "@NomeArquivo", DbType.String, ObjMCnabControleSaida.NomeArquivo);
            db.AddInParameter(dbCommand, "@NomeArquivoArmazenado", DbType.String, ObjMCnabControleSaida.NomeArquivoArmazenado);
            db.AddInParameter(dbCommand, "@QtdeDetalhes", DbType.Int32, ObjMCnabControleSaida.QtdeDetalhes);
            db.AddInParameter(dbCommand, "@ValorTotalDetalhes", DbType.Decimal, ObjMCnabControleSaida.ValorTotalDetalhes);
            db.AddInParameter(dbCommand, "@Status", DbType.Int32, ObjMCnabControleSaida.Status);
            db.AddInParameter(dbCommand, "@Ocorrencia", DbType.String, ObjMCnabControleSaida.Ocorrencia);

            db.ExecuteNonQuery(dbCommand);
        }

        public static MCnabControleSaida Locate(int CnabControleSaidaId)
        {

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_SelectCnabControleSaidaByCnabControleSaidaId");

            db.AddInParameter(dbCommand, "@CnabControleSaidaId", DbType.Int32, CnabControleSaidaId);


            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    return new MCnabControleSaida
                                            (
                                                dr.GetInt32(dr.GetOrdinal("CnabControleSaidaId")),
                                                dr.GetInt32(dr.GetOrdinal("NumInstituicao")),
                                                dr.GetDateTime(dr.GetOrdinal("DataArquivo")),
                                                dr.GetInt32(dr.GetOrdinal("NumSeqRemessa")),
                                                dr.GetString(dr.GetOrdinal("NomeArquivo")),
                                                dr.GetString(dr.GetOrdinal("NomeArquivoArmazenado")),
                                                dr.GetInt32(dr.GetOrdinal("QtdeDetalhes")),
                                                dr.GetDecimal(dr.GetOrdinal("ValorTotalDetalhes")),
                                                dr.GetInt32(dr.GetOrdinal("Status")),
                                                string.Empty
                                            );
                }
            }
            return null;
        }

        //// Retorna os Dados para Preencher o Relatório de Recebimento de Arquivos da CSU

        //public static IList<MRelatorioEnvioCnab> getRelatorioEnvioCnab(     DateTime DataInicial,
        //                                                                    DateTime DataFinal,
        //                                                                    Int32 Banco,
        //                                                                    Int32 Status
        //)

        //     {
        //    //Cria uma lista de Objetos da classe Entidade
        //    IList<MRelatorioEnvioCnab> ObjLista = new List<MRelatorioEnvioCnab>();

        //    Database db = DatabaseFactory.CreateDatabase("fastpag");

        //    DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaoenviobanco");
        //    db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, DataInicial);
        //    db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, DataFinal);
        //    db.AddInParameter(dbCommand, "@Banco", DbType.Int32, Banco);
        //    db.AddInParameter(dbCommand, "@Status", DbType.Int32, Status);


        //    //    //Popula dr
        //    using (IDataReader dr = db.ExecuteReader(dbCommand))
        //    {
        //        if (dr != null)
        //        {
        //            while (dr.Read())
        //            {
        //                //Popula ObjLista
        //                ObjLista.Add(new MRelatorioEnvioCnab
        //                                        (
        //                                                Convert.ToDateTime(dr["DataEnvio"].ToString()),
        //                                                dr["NomeArquivoBanco"].ToString(),
        //                                                Convert.ToInt32(dr["NumerodoBanco"].ToString()),
        //                                                Convert.ToInt32(dr["QtdeTransacoes"].ToString()),
        //                                                Convert.ToDecimal(dr["ValorTransacoes"].ToString()),
        //                                                Convert.ToInt32(dr["Status"].ToString()),
        //                                                dr["DescricaoStatus"].ToString(),
        //                                                dr["DescricaoOcorrencia"].ToString(),
        //                                                Convert.ToInt32(dr["QtdeTransacoesPagas"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalTransacoesPagas"].ToString()),
        //                                                Convert.ToInt32(dr["QtdeTransacoesRecusadas"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalTransacoesRecusadas"].ToString()),
        //                                                Convert.ToInt32(dr["QtdeTransacoesPendentes"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalTransacoesPendente"].ToString())
        //                                        )
        //                           );
        //            }
        //        }
        //    }
        //    return ObjLista;

        //}

        // Alteração : Jair Jersey Marinho - 27/10/2008

        public static IList<MRelatorioEnvioCnab> getRelatorioEnvioCnab(Int32 TipoData,
                                                                            DateTime DataInicial,
                                                                            DateTime DataFinal,
                                                                            Int32 Banco,
                                                                            Int32 Status
                                                                      )
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MRelatorioEnvioCnab> ObjLista = new List<MRelatorioEnvioCnab>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaoenviobanco1");
            db.AddInParameter(dbCommand, "@TipoData", DbType.Int32, TipoData);
            db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, DataInicial);
            db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, DataFinal);
            db.AddInParameter(dbCommand, "@Banco", DbType.Int32, Banco);
            db.AddInParameter(dbCommand, "@Status", DbType.Int32, Status);


            //    //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        //Popula ObjLista
                        ObjLista.Add(new MRelatorioEnvioCnab
                                                (
                                                        Convert.ToDateTime(dr["DataTransacao"].ToString()),
                                                        Convert.ToDateTime(dr["DataArquivo"].ToString()),
                                                        dr["NomeArquivoBanco"].ToString(),
                                                        Convert.ToInt32(dr["Banco"].ToString()),
                                                        Convert.ToInt32(dr["QtdeTransacoes"].ToString()),
                                                        Convert.ToDecimal(dr["TotalTransacoes"].ToString()),
                                                        Convert.ToInt32(dr["Status"].ToString()),
                                                        dr["DescricaoStatus"].ToString()
                                                )
                                   );
                    }
                }
            }
            return ObjLista;

        }


        public static IList<MStatusSuperPag> getRecuperaStatus()
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MStatusSuperPag> ObjLista = new List<MStatusSuperPag>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetStatusPerObject");
            db.AddInParameter(dbCommand, "@Object", DbType.String, "CnabControleSaida");

            //    //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    ObjLista.Add(new MStatusSuperPag(0, "Todos", "Todos", 0));
                    while (dr.Read())
                    {
                        //Popula ObjLista
                        ObjLista.Add(new MStatusSuperPag
                                                (
                                                        Convert.ToInt32(dr["IdStatus"].ToString()),
                                                        dr["NmStatus"].ToString(),
                                                        dr["DsStatus"].ToString(),
                                                        Convert.ToInt32(dr["IdWorkflowWeiseIT"].ToString())


                                                )
                                   );
                    }
                }
            }
            return ObjLista;

        }


    }
}
