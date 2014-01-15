using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace SuperPag.Business
{
    public class Cnab
    {
        public static IList<MCnab> GetCnab(string Layout, string Versao, DateTime Ano, int TypeId, int TypeCnab)
        {
            IList<MCnab> ObjLista = new List<MCnab>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetCnab");

            db.AddInParameter(dbCommand, "@Layout", DbType.String, Layout);
            db.AddInParameter(dbCommand, "@Versao", DbType.String, Versao);
            db.AddInParameter(dbCommand, "@Ano", DbType.DateTime, Ano);
            db.AddInParameter(dbCommand, "@TypeId", DbType.Int32, TypeId);
            db.AddInParameter(dbCommand, "@TypeCnab", DbType.Int32, TypeCnab);

            try
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        ObjLista.Add(new MCnab(dr.GetString(dr.GetOrdinal("Descricao")),
                                               dr.GetInt32(dr.GetOrdinal("Inicio")),
                                               dr.GetInt32(dr.GetOrdinal("Fim")),
                                               dr.GetInt32(dr.GetOrdinal("Tamanho")),
                                               dr.GetInt32(dr.GetOrdinal("Layout")),
                                               dr.GetString(dr.GetOrdinal("Versao")),
                                               dr.IsDBNull(dr.GetOrdinal("Valor")) ? string.Empty : dr.GetString(dr.GetOrdinal("Valor")),
                                               dr.GetDateTime(dr.GetOrdinal("Ano")),
                                               dr.GetInt32(dr.GetOrdinal("TypeId")),
                                               dr.GetString(dr.GetOrdinal("Picture"))
                                               ));
                    }
                }
                return ObjLista;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
