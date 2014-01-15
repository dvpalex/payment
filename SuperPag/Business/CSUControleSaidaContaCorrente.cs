using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace SuperPag.Business
{
    public class CSUControleSaidaContaCorrente
    {
        

        public static int Insert(MCSUControleSaidaContaCorrente ObjMCSUControleSaidaContaCorrente)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("Proc_InsertCSUControleSaidaContaCorrente");

            db.AddInParameter(dbCommand, "@DataGeracaoLote", DbType.DateTime, ObjMCSUControleSaidaContaCorrente.DataGeracaoLote);
            db.AddInParameter(dbCommand, "@NumLote", DbType.Int32, ObjMCSUControleSaidaContaCorrente.NumLote);
            db.AddInParameter(dbCommand, "@QtdeDetalhes", DbType.Int32, ObjMCSUControleSaidaContaCorrente.QtdeDetalhes);
            db.AddInParameter(dbCommand, "@ValorTotalDetalhes", DbType.Decimal, ObjMCSUControleSaidaContaCorrente.ValorTotalDetalhes);
            db.AddInParameter(dbCommand, "@NomeArquivoEnviado", DbType.String, ObjMCSUControleSaidaContaCorrente.NomeArquivoEnviado);
            db.AddInParameter(dbCommand, "@NomeArquivoArmazenado", DbType.String, ObjMCSUControleSaidaContaCorrente.NomeArquivoArmazenado);
            db.AddInParameter(dbCommand, "@ProtocoloRecebimento", DbType.String, ObjMCSUControleSaidaContaCorrente.ProtocoloRecebimento);
            db.AddInParameter(dbCommand, "@StatusProcessamento", DbType.Int32, ObjMCSUControleSaidaContaCorrente.StatusProcessamento);

            return Convert.ToInt32(db.ExecuteScalar(dbCommand));
        }

        public void Update(MCSUControleSaidaContaCorrente ObjMCSUControleSaidaContaCorrente)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");

            DbCommand dbCommand = db.GetStoredProcCommand("");

            db.AddInParameter(dbCommand, "@ControleSaidaId", DbType.Int32, ObjMCSUControleSaidaContaCorrente.ControleSaidaId);
            db.AddInParameter(dbCommand, "@DataGeracaoLote", DbType.DateTime, ObjMCSUControleSaidaContaCorrente.DataGeracaoLote);
            db.AddInParameter(dbCommand, "@NumLote", DbType.Int32, ObjMCSUControleSaidaContaCorrente.NumLote);
            db.AddInParameter(dbCommand, "@QtdeDetalhes", DbType.Int32, ObjMCSUControleSaidaContaCorrente.QtdeDetalhes);
            db.AddInParameter(dbCommand, "@ValorTotalDetalhes", DbType.Decimal, ObjMCSUControleSaidaContaCorrente.ValorTotalDetalhes);
            db.AddInParameter(dbCommand, "@NomeArquivoEnviado", DbType.String, ObjMCSUControleSaidaContaCorrente.NomeArquivoEnviado);
            db.AddInParameter(dbCommand, "@NomeArquivoArmazenado", DbType.String, ObjMCSUControleSaidaContaCorrente.NomeArquivoArmazenado);
            db.AddInParameter(dbCommand, "@ProtocoloRecebimento", DbType.String, ObjMCSUControleSaidaContaCorrente.ProtocoloRecebimento);
            db.AddInParameter(dbCommand, "@StatusProcessamento", DbType.Int32, ObjMCSUControleSaidaContaCorrente.StatusProcessamento);

            db.ExecuteNonQuery(dbCommand);
        }

        // Retorna os Dados para Preencher o Relatório de Recebimento de Arquivos da CSU
        //public static IList<MRelatorioEnvioCSU> getRelatorioEnvioCSU(
        //                                                                            DateTime DataInicial,
        //                                                                            DateTime DataFinal
        //                                                                        )
        //{
        //    //Cria uma lista de Objetos da classe Entidade
        //    IList<MRelatorioEnvioCSU> ObjLista = new List<MRelatorioEnvioCSU>();

        //    Database db = DatabaseFactory.CreateDatabase("fastpag");
        //    DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaoenviocsu");
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
        //                ObjLista.Add(new MRelatorioEnvioCSU
        //                                        (
        //                                                Convert.ToDateTime(dr["DataEnvio"].ToString()),
        //                                                dr["NomeArquivoCSU"].ToString(),
        //                                                Convert.ToInt32(dr["QtdeTransacoes"].ToString()),
        //                                                Convert.ToDecimal(dr["TotalTransacoes"].ToString()),
        //                                                Convert.ToInt32(dr["Status"].ToString()),
        //                                                dr["DescricaoStatus"].ToString()
        //                                        )
        //                           );
        //            }
        //        }
        //    }
        // Alteração : Jair Jersey Marinho - 23/10/2008
            public static IList<MRelatorioEnvioCSU> getRelatorioEnvioCSU(           Int32 TipoData,
                                                                                    DateTime DataInicial,
                                                                                    DateTime DataFinal
                                                                                )
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MRelatorioEnvioCSU> ObjLista = new List<MRelatorioEnvioCSU>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("proc_rel_visaoenviocsu");
            db.AddInParameter(dbCommand, "@TipoData", DbType.Int32, TipoData);
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
                        ObjLista.Add(new MRelatorioEnvioCSU
                                                (
                                                        Convert.ToDateTime(dr["DataEnvio"].ToString()),
                                                        dr["NomeArquivoCSU"].ToString(),
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

    }
}

