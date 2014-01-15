using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SuperPag.Business.Messages;

namespace SuperPag.Business
{
    public class CnabControleEntrada
    {
        public static int Insert(MCnabControleEntrada ObjMCnabControleEntrada)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_InsertCnabControleEntrada");

            db.AddInParameter(dbCommand, "@NumInstituicao", DbType.Int32, ObjMCnabControleEntrada.NumInstituicao);
            db.AddInParameter(dbCommand, "@DataArquivo", DbType.DateTime, ObjMCnabControleEntrada.DataArquivo);
            db.AddInParameter(dbCommand, "@NumSeqRemessa", DbType.Int32, ObjMCnabControleEntrada.NumSeqRemessa);
            db.AddInParameter(dbCommand, "@NomeArquivo", DbType.String, ObjMCnabControleEntrada.NomeArquivo);
            db.AddInParameter(dbCommand, "@QtdeDetalhes", DbType.Int32, ObjMCnabControleEntrada.QtdeDetalhes);
            db.AddInParameter(dbCommand, "@ValorTotalDetalhes", DbType.Decimal, ObjMCnabControleEntrada.ValorTotalDetalhes);
            db.AddInParameter(dbCommand, "@Ocorrencia", DbType.String, ObjMCnabControleEntrada.Ocorrencia);
            db.AddInParameter(dbCommand, "@Status", DbType.Int32, ObjMCnabControleEntrada.Status);

            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }


        //proc_rel_confproc

        public static IList<MRelatorioRecebimentoCnab> getRelatorioConferecia(
                                                                            DateTime DataInicial,
                                                                            DateTime DataFinal,
                                                                            Int32 tipo
                                                                      )
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MRelatorioRecebimentoCnab> ObjLista = new List<MRelatorioRecebimentoCnab>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_confproc");
            db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, DataInicial);
            db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, DataFinal);
            db.AddInParameter(dbCommand, "@Status", DbType.Int32, tipo);


            //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {

                    while (dr.Read())
                    {
                        //Popula ObjLista
                        ObjLista.Add(new MRelatorioRecebimentoCnab
                                                (
                                                        Convert.ToInt32(dr["Banco"].ToString()),
                                                        Convert.ToInt32(dr["QtdeTransacoes"].ToString()),
                                                        Convert.ToDecimal(dr["TotalTransacoes"].ToString())
                                                )
                                   );
                    }
                }
            }
            return ObjLista;

        }

        public static IList<MRelatorioRecebimentoCnab> getRelatorioConferecia(
                                                                            DateTime DataInicial,
                                                                            DateTime DataFinal,
                                                                            Int32 tipo,
                                                                            Int32 NumInstituicao
                                                                      )
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MRelatorioRecebimentoCnab> ObjLista = new List<MRelatorioRecebimentoCnab>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_confproc_bancos");
            db.AddInParameter(dbCommand, "@Tipo", DbType.Int32, tipo);
            db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, DataInicial);
            db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, DataFinal);
            db.AddInParameter(dbCommand, "@Banco", DbType.Int32, NumInstituicao);

            //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {

                    while (dr.Read())
                    {
                        //Popula ObjLista
                        ObjLista.Add(new MRelatorioRecebimentoCnab
                                                (
                                                        Convert.ToInt32(dr["Banco"].ToString()),
                                                        Convert.ToInt32(dr["QtdeTransacoes"].ToString()),
                                                        Convert.ToDecimal(dr["TotalTransacoes"].ToString())
                                                )
                                   );
                    }
                }
            }
            return ObjLista;

        }

        // Retorna os Dados para Preencher o Relatório de Recebimento de Arquivos da CSU
        //public static IList<MRelatorioRecebimentoCnab> getRelatorioRecebimentoCnab(
        //                                                            DateTime DataInicial,
        //                                                            DateTime DataFinal,
        //                                                            Int32 Banco,
        //                                                            Int32 Status
        //                                                      )
        //{
        //    //Cria uma lista de Objetos da classe Entidade
        //    IList<MRelatorioRecebimentoCnab> ObjLista = new List<MRelatorioRecebimentoCnab>();

        //    Database db = DatabaseFactory.CreateDatabase("fastpag");
        //    DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaorecebimentobanco");
        //    db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, DataInicial);
        //    db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, DataFinal);
        //    db.AddInParameter(dbCommand, "@Banco", DbType.Int32, Banco);
        //    db.AddInParameter(dbCommand, "@Status", DbType.Int32, Status);


        //    //Popula dr
        //    using (IDataReader dr = db.ExecuteReader(dbCommand))
        //    {
        //        if (dr != null)
        //        {

        //            while (dr.Read())
        //            {
        //                //Popula ObjLista
        //                ObjLista.Add(new MRelatorioRecebimentoCnab
        //                                        (
        //                                                Convert.ToDateTime(dr["DataRecebimento"].ToString()),
        //                                                dr["NomeArquivoBanco"].ToString(),
        //                                                Convert.ToInt32(dr["Banco"].ToString()),
        //                                                Convert.ToInt32(dr["QtdeTransacoes"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalTransacoes"].ToString()),
        //                                                Convert.ToInt32(dr["Status"].ToString()),
        //                                                dr["DescricaoOcorrencia"].ToString(),
        //                                                dr["DescricaoStatus"].ToString(),
        //                                                Convert.ToInt32(dr["QtdePagas"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalPagas"].ToString()),
        //                                                Convert.ToInt32(dr["QtdeNaoPagas"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalNaoPagas"].ToString())
        //                                        )
        //                           );
        //            }
        //        }
        //    }
        //    return ObjLista;

        //}

        // Retorna os Dados para Preencher o Relatório de Recebimento de Arquivos da CSU
        // Alteração : Jair Jersey Marinho - 22/10/2008
        public static IList<MRelatorioRecebimentoCnab> getRelatorioRecebimentoCnab(
                                                                            Int32 TipoData,
                                                                            DateTime DataInicial,
                                                                            DateTime DataFinal,
                                                                            Int32 Banco,
                                                                            Int32 Status
                                                                      )
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MRelatorioRecebimentoCnab> ObjLista = new List<MRelatorioRecebimentoCnab>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaorecebimentobanco1");
            db.AddInParameter(dbCommand, "@TipoData", DbType.Int32, TipoData);
            db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, DataInicial);
            db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, DataFinal);
            db.AddInParameter(dbCommand, "@Banco", DbType.Int32, Banco);
            db.AddInParameter(dbCommand, "@Status", DbType.Int32, Status);


            //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {

                    while (dr.Read())
                    {
                        //Popula ObjLista
                        ObjLista.Add(new MRelatorioRecebimentoCnab
                                                (
                                                        Convert.ToDateTime(dr["DataTransacao"].ToString()),
                                                        Convert.ToDateTime(dr["DataArquivo"].ToString()),
                                                        dr["NomeArquivoBanco"].ToString(),
                                                        Convert.ToInt32(dr["Banco"].ToString()),
                                                        Convert.ToInt32(dr["QtdeTransacoes"].ToString()),
                                                        Convert.ToDecimal(dr["ValorTransacoes"].ToString()),
                                                        Convert.ToInt32(dr["Status"].ToString()),
                                                        //dr["DescricaoOcorrencia"].ToString(),
                                                        dr["DescricaoStatus"].ToString(),
                                                        Convert.ToInt32(dr["QtdePagas"].ToString()),
                                                        Convert.ToDecimal(dr["TotalPagas"].ToString()),
                                                        Convert.ToInt32(dr["QtdeNaoPagas"].ToString()),
                                                        Convert.ToDecimal(dr["TotalNaoPagas"].ToString())
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
            db.AddInParameter(dbCommand, "@Object", DbType.String, "CnabControleEntrada");

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
