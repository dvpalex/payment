using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration;
using System.Collections;
using SuperPag.Business;
using SuperPag.Business.Messages;
using System.Web.Security; //Seguraça
using System.Collections.Generic;

namespace SuperPag.Business
{
    //DALC / FACTORY
    public class Role
    {
        #region METODOS
            //Passando o Nome, retorna a chave primária de um determinado Grupo
            public static Guid RetunRoleId(string RoleName, int StoreId)
            {
                Database db = DatabaseFactory.CreateDatabase("fastpag");
                DbCommand dbCommand = db.GetStoredProcCommand("Proc_RecuperaRoleId");
                db.AddInParameter(dbCommand, "@RoleName", DbType.String, RoleName);
                db.AddInParameter(dbCommand, "@StoreId", DbType.Int32, StoreId);

                return (Guid)db.ExecuteScalar(dbCommand);
            }


            //Retorna os Roles vinculados a uma determinada Loja e retorna também o Usuário ADMTIVIT
            public static IList<MRole> GetRoleInStore(int StoreID, byte admTivit)
            {
               //Cria uma lista de Objetos da classe Entidade
                IList<MRole> ObjLista = new List<MRole>();

                Database db = DatabaseFactory.CreateDatabase("fastpag");
                DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetRolesInStore");
                db.AddInParameter(dbCommand, "@StoreID", DbType.Int32, StoreID);
                db.AddInParameter(dbCommand, "@admTivit", DbType.Byte, admTivit);

                //Popula dr
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {                   
                    if (dr != null)
                    {
                        while(dr.Read())
                        {   
                            //Popula ObjLista
                            ObjLista.Add(new MRole( dr.GetGuid(dr.GetOrdinal("ApplicationId")),
                                                    dr.GetGuid(dr.GetOrdinal("RoleId")),
                                                    dr.GetString(dr.GetOrdinal("RoleName")),
                                                    dr.GetString(dr.GetOrdinal("LoweredRoleName")),
                                                    dr.IsDBNull(dr.GetOrdinal("Description")) ? String.Empty : dr.GetString(dr.GetOrdinal("Description")),
                                                    dr.GetInt32(dr.GetOrdinal("StoreId"))
                                                   ));
                        }
                    }
                }
                return ObjLista;
             }

             //Retorna os Roles vinculados a uma determinada Loja menos o Usuário ADMTIVIT
             public static IList<MRole> GetRoleInStore(int StoreID)
             {
                 //Cria uma lista de Objetos da classe Entidade
                 IList<MRole> ObjLista = new List<MRole>();

                 Database db = DatabaseFactory.CreateDatabase("fastpag");
                 DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetRolesInStore");
                 db.AddInParameter(dbCommand, "@StoreID", DbType.Int32, StoreID);
                 
                 //Popula dr
                 using (IDataReader dr = db.ExecuteReader(dbCommand))
                 {
                     if (dr != null)
                     {
                         while (dr.Read())
                         {
                             //Popula ObjLista
                             ObjLista.Add(new MRole(dr.GetGuid(dr.GetOrdinal("ApplicationId")),
                                                     dr.GetGuid(dr.GetOrdinal("RoleId")),
                                                     dr.GetString(dr.GetOrdinal("RoleName")),
                                                     dr.GetString(dr.GetOrdinal("LoweredRoleName")),
                                                     dr.IsDBNull(dr.GetOrdinal("Description")) ? String.Empty : dr.GetString(dr.GetOrdinal("Description")),
                                                     dr.GetInt32(dr.GetOrdinal("StoreId"))
                                                    ));
                         }
                     }
                 }
                 return ObjLista;
             }


            //Exclui um Rolede uma determinada loja
            public static void DeleteRole(Guid RoleId, int StoreID)
            {
                Database db = DatabaseFactory.CreateDatabase("fastpag");
                DbCommand dbCommand = db.GetStoredProcCommand("Proc_DeleteRole");
                db.AddInParameter(dbCommand, "@RoleId", DbType.Guid, RoleId);
                db.AddInParameter(dbCommand, "@StoreID", DbType.Int32, StoreID);
                db.ExecuteNonQuery(dbCommand);
            }

            //Exclui os Roles de um usuário
            public static void UserInRolesDELETE(Guid UserId)
            {
                Database db = DatabaseFactory.CreateDatabase("fastpag");
                DbCommand dbCommand = db.GetStoredProcCommand("Proc_UsersInRolesDELETE");
                db.AddInParameter(dbCommand, "@UserId", DbType.Guid, UserId);
                db.ExecuteNonQuery(dbCommand);
            }
        #endregion
    }
}
