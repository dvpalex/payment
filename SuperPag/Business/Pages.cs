using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Web.Security;


namespace SuperPag.Business
{
    public class Pages
    {
        #region Shared/Static Methods

        static Pages ObjPages = null;

        static Pages()
        {
            CreateInstance();
        }

        private static void CreateInstance()
        {
            ObjPages = new Pages();
        }

        public static Pages GetInstance()
        {
            return ObjPages;
        }

        #endregion


        //Este método, retorna as PAGES que um Grupo pode acessar
        public static bool ReturnPagesInRole(String PageIdentification, int StoreId)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            
            //Recupera os Grupos do Uusário Logado
            string[] oGruposUsu = Roles.GetRolesForUser();
            bool Page = false;

            for (int i = 0; i < oGruposUsu.Length; i++)
            {
                if (Page.Equals(false))
                {
                    Guid RoleId = Role.RetunRoleId(oGruposUsu[i], StoreId);//Recupera a chave do Grupo
                    DbCommand dbCommand = db.GetStoredProcCommand("Proc_ReturnPagesInRole");
                    db.AddInParameter(dbCommand, "@RoleId", DbType.Guid, RoleId);

                    using (IDataReader dr = db.ExecuteReader(dbCommand))
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                if ((dr["NameIdentification"].ToString().Equals(PageIdentification)))
                                    Page = true;
                            }
                        }
                    }
                }
            }
            return Page;
        }
            
         

        //SELECIONA OS MENUS PAIS CONFORME PERMISSÃO DO GRUPO
        public static IList<MPages> GetMenu(int PageParentId, Guid RoleId)
        {
            //Cria uma lista de Objetos da classe Entidade
            IList<MPages> ObjLista = new List<MPages>();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetPages");
            db.AddInParameter(dbCommand, "@PageParentId", DbType.Int32, PageParentId);
            db.AddInParameter(dbCommand, "@RoleId", DbType.Guid, RoleId);

            //Popula dr
              using (IDataReader dr = db.ExecuteReader(dbCommand))
                {                   
                    //lê o dr e add os sub-menus na lista de retorno
                    if (dr != null)
                    {
                        while(dr.Read())
                        {
                            ObjLista.Add(new MPages(dr.GetInt32(dr.GetOrdinal("PageId")),
                                                    dr.GetString(dr.GetOrdinal("PageName")),
                                                    dr.IsDBNull(dr.GetOrdinal("PagePath")) ? String.Empty : dr.GetString(dr.GetOrdinal("PagePath")),
                                                    dr.GetInt32(dr.GetOrdinal("PageParentId")),
                                                    dr.GetInt32(dr.GetOrdinal("PageOrder"))
                                                   ));
                        }
                    }
                }
                return ObjLista;
            }

            //SELECIONA OS MENUS PAIS CONFORME PERMISSÃO DO GRUPO
            public static IList<MPages> GetMenu(int PageParentId)
            {
                //Cria uma lista de Objetos da classe Entidade
                IList<MPages> ObjLista = new List<MPages>();

                Database db = DatabaseFactory.CreateDatabase("fastpag");
                DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetPagesADM");
                db.AddInParameter(dbCommand, "@PageParentId", DbType.Int32, PageParentId);

                //Popula dr
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    //lê o dr e add os sub-menus na lista de retorno
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            ObjLista.Add(new MPages(dr.GetInt32(dr.GetOrdinal("PageId")),
                                                    dr.GetString(dr.GetOrdinal("PageName")),
                                                    dr.IsDBNull(dr.GetOrdinal("PagePath")) ? String.Empty : dr.GetString(dr.GetOrdinal("PagePath")),
                                                    dr.GetInt32(dr.GetOrdinal("PageParentId")),
                                                    dr.GetInt32(dr.GetOrdinal("PageOrder"))
                                                   ));
                        }
                    }
                }
                return ObjLista;
            }

        }
    }