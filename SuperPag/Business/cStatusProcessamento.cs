using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using SuperPag.Business.Messages;

namespace SuperPag.Business
{
    public class cStatusProcessamento
    {

        // Retorna os Dados para Preencher o Relatório de Recebimento de Arquivos da CSU
        public static MStatusProcessamento getStatusById(int Id)
        {
            //Cria uma lista de Objetos da classe Entidade
            MStatusProcessamento Objretorno = null;

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_get_StatusProcessamento");
            db.AddInParameter(dbCommand, "@Id", DbType.Int32, Id);


            //Popula dr
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        //Popula ObjLista
                        Objretorno = new MStatusProcessamento
                                                (Convert.ToInt32(dr["IDSTATUS"].ToString()),
                                                        dr["DSCSTATUS"].ToString()
                                                );
                    }
                }
            }
            return Objretorno;

        }
    }
}
