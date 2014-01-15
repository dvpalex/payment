using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SuperPag.Business.Messages;

namespace SuperPag.Business
{
    public class CSUControleEntradaContaCorrente
    {


        public static int Insert(MCSUControleEntradaContaCorrente ObjMCSUControleEntradaContaCorrente)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_InsertCSUControleEntradaContaCorrente");

            db.AddInParameter(dbCommand, "@DataArquivo", DbType.DateTime, ObjMCSUControleEntradaContaCorrente.DataArquivo);
            db.AddInParameter(dbCommand, "@NumSeqRemessa", DbType.Int32, ObjMCSUControleEntradaContaCorrente.NumSeqRemessa);
            db.AddInParameter(dbCommand, "@NomeArquivoCapturado", DbType.String, ObjMCSUControleEntradaContaCorrente.NomeArquivoCapturado);
            db.AddInParameter(dbCommand, "@DataCapturaArquivo", DbType.DateTime, ObjMCSUControleEntradaContaCorrente.DataCapturaArquivo);
            db.AddInParameter(dbCommand, "@NomeArquivoArmazenado", DbType.String, ObjMCSUControleEntradaContaCorrente.NomeArquivoArmazenado);
            db.AddInParameter(dbCommand, "@QtdeDetalhes", DbType.Int32, ObjMCSUControleEntradaContaCorrente.QtdeDetalhes);
            db.AddInParameter(dbCommand, "@ValorTotalDetalhes", DbType.Decimal, ObjMCSUControleEntradaContaCorrente.ValorTotalDetalhes);
            db.AddInParameter(dbCommand, "@NumInstituicao", DbType.Int32, ObjMCSUControleEntradaContaCorrente.NumInstituicao);


            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }

        public static void Update(MCSUControleEntradaContaCorrente ObjMCSUControleEntradaContaCorrente)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_UpdateCSUControleEntradaContaCorrente");

            db.AddInParameter(dbCommand, "@ControleEntradaId", DbType.Int32, ObjMCSUControleEntradaContaCorrente.ControleEntradaId);
            db.AddInParameter(dbCommand, "@DataArquivo", DbType.DateTime, ObjMCSUControleEntradaContaCorrente.DataArquivo);
            db.AddInParameter(dbCommand, "@NumSeqRemessa", DbType.Int32, ObjMCSUControleEntradaContaCorrente.NumSeqRemessa);
            db.AddInParameter(dbCommand, "@NomeArquivoCapturado", DbType.String, ObjMCSUControleEntradaContaCorrente.NomeArquivoCapturado);
            db.AddInParameter(dbCommand, "@DataCapturaArquivo", DbType.DateTime, ObjMCSUControleEntradaContaCorrente.DataCapturaArquivo);
            db.AddInParameter(dbCommand, "@NomeArquivoArmazenado", DbType.String, ObjMCSUControleEntradaContaCorrente.NomeArquivoArmazenado);
            db.AddInParameter(dbCommand, "@QtdeDetalhes", DbType.Int32, ObjMCSUControleEntradaContaCorrente.QtdeDetalhes);
            db.AddInParameter(dbCommand, "@ValorTotalDetalhes", DbType.Decimal, ObjMCSUControleEntradaContaCorrente.ValorTotalDetalhes);
            db.AddInParameter(dbCommand, "@NumInstituicao", DbType.Int32, ObjMCSUControleEntradaContaCorrente.NumInstituicao);
            db.AddInParameter(dbCommand, "@StatusProcessamento", DbType.Int32, ObjMCSUControleEntradaContaCorrente.StatusProcessamento);

            db.ExecuteNonQuery(dbCommand);
        }

        public static bool SelectLote(int NumSeqRemessa)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_SelectLotCSUControleEntradaContaCorrente");

            db.AddInParameter(dbCommand, "@NumSeqRemessa", DbType.Int32, NumSeqRemessa);            

            object Ret = db.ExecuteScalar(dbCommand);

            if (Ret == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //// Retorna os Dados para Preencher o Relatório de Recebimento de Arquivos da CSU
        //public static IList<MRelatorioRecebimentoCSU> getRelatorioRecebimentoCSU(
        //                                                                            DateTime DataInicial, 
        //                                                                            DateTime DataFinal
        //                                                                        )
        //{
        //    //Cria uma lista de Objetos da classe Entidade
        //    IList<MRelatorioRecebimentoCSU> ObjLista = new List<MRelatorioRecebimentoCSU>();

        //    Database db = DatabaseFactory.CreateDatabase("fastpag");
        //    DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaorecebimentocsu");
        //    db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, DataInicial);
        //    db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, DataFinal);

        //    //Popula dr
        //    using (IDataReader dr = db.ExecuteReader(dbCommand))
        //    {
        //        if (dr != null)
        //        {
        //            while (dr.Read())
        //            {
        //                //Popula ObjLista
        //                ObjLista.Add(new MRelatorioRecebimentoCSU
        //                                        (
        //                                                Convert.ToDateTime(dr["Recebimento"].ToString()),
        //                                                dr["Arquivo"].ToString(),
        //                                                Convert.ToInt32(dr["QtdeTransacoes"].ToString()),
        //                                                Convert.ToDecimal(dr["Total"].ToString()),
        //                                                Convert.ToInt32(dr["Status"].ToString()),
        //                                                dr["DescricaoStatus"].ToString(),
        //                                                Convert.ToInt32(dr["QtdePagas"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalPagas"].ToString()),
        //                                                Convert.ToInt32(dr["QtdeNaoPagas"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalNaoPagas"].ToString()),
        //                                                Convert.ToInt32(dr["QtdeReceber"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalReceber"].ToString())

        //                                        )
        //                           );
        //            }
        //        }
        //    }
        //    return ObjLista;

        //}

        // Alteração : Jair Jersey Marinho - 22/10/2008
        public static IList<MRelatorioRecebimentoCSU> getRelatorioRecebimentoCSU(   Boolean TipoData,
                                                                                    DateTime DataInicial,
                                                                                    DateTime DataFinal
                                                                                )
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MRelatorioRecebimentoCSU> ObjLista = new List<MRelatorioRecebimentoCSU>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaorecebimentocsu");
            db.AddInParameter(dbCommand, "@TipoData", DbType.Boolean, TipoData);
            db.AddInParameter(dbCommand, "@DataInicial", DbType.DateTime, DataInicial);
            db.AddInParameter(dbCommand, "@DataFinal", DbType.DateTime, DataFinal);

            //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        //Popula ObjLista
                        ObjLista.Add(new MRelatorioRecebimentoCSU
                                                (
                                                        Convert.ToDateTime(dr["DATAARQUIVO"].ToString()),
                                                        Convert.ToDateTime(dr["DATACAPTURAARQUIVO"].ToString()),
                                                        dr["Arquivo"].ToString(),
                                                        Convert.ToInt32(dr["QtdeTransacoes"].ToString()),
                                                        Convert.ToDecimal(dr["Total"].ToString()),
                                                        Convert.ToInt32(dr["Status"].ToString()),
                                                        dr["DescricaoStatus"].ToString()
                                                )
                                   );
                    }
                }
            }
            return ObjLista;

        }
    }
}
