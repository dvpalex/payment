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
    public class PermissionPages
    {

        #region MÉTODOS DE DADOS

        //Retorna as permissões das páginas que contém os Grupos, para preenchimento do TreeView
        public static IDataReader GetTree(int None)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetPermissons");
            db.AddInParameter(dbCommand, "@NonePai", DbType.Int32, None);
            IDataReader dr = db.ExecuteReader(dbCommand);

            return dr;
        }


        //Insere as Permissões de um GRUPO no Banco
        public static void PermissionInRolesINSERT(IList<string[]> oListRoles)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
                       
            for (int i = 0; i < oListRoles.Count; i++)
            {
                DbCommand dbCommand = db.GetStoredProcCommand("Proc_PermissonsInRolesINSERT");
                db.AddInParameter(dbCommand, "@PermissionPagesId ", DbType.Int32, oListRoles[i].GetValue(0));
                db.AddInParameter(dbCommand, "@RoleId", DbType.Guid,new Guid((string)oListRoles[i].GetValue(1)));
                db.ExecuteNonQuery(dbCommand);            
            }
        }

        //Exclui as Permissões de um GRUPO no Banco
        public static void PermissionInRolesDELETE(Guid RoleId)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_PermissonsDELETE");
            db.AddInParameter(dbCommand, "@RoleId", DbType.Guid, RoleId);
            db.ExecuteNonQuery(dbCommand);
        }

        //Retorna as Permissões de um GRUPO
        public static IList<MPermissionInRoles> PermissionInRolesSELECT(Guid RoleId)
        {
                //Lista de Objetos da classe Entidade
                IList<MPermissionInRoles> ObjLista = new List<MPermissionInRoles>();

                Database db = DatabaseFactory.CreateDatabase("fastpag");
                DbCommand dbCommand = db.GetStoredProcCommand("Proc_PermissonsInRolesSELECT");
                db.AddInParameter(dbCommand, "@RoleId", DbType.Guid, RoleId);
                
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                 {                   
                    //lê o dr e add os sub-menus na lista de retorno
                    if (dr != null)
                    {
                        while(dr.Read())
                        {
                            ObjLista.Add(new MPermissionInRoles(dr.GetInt32(dr.GetOrdinal("PermissionPagesId"))));
                        }
                    }
                }
            return ObjLista;
        }

        #endregion
    }
}
