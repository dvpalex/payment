using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.DataAccess;
using System.Web.Security;
using System.Web.Hosting;
using System.Web.Util;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SuperPag.Business.Messages;
using SuperPag.Business.MembershipAdm;


namespace SuperPag.Business.MembershipAdm
{
    public sealed class RolesAdm : RoleProvider
    {

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        // Vincula um Usuário ao um Grupo
        public void AddUsersToRoles(Guid UserId, Guid RoleId)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_VinculaUserInRole");
            db.AddInParameter(dbCommand, "@UserId ", DbType.Guid, UserId);
            db.AddInParameter(dbCommand, "@RoleId ", DbType.Guid, RoleId);
            db.ExecuteNonQuery(dbCommand);
        }

        public override string ApplicationName
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //Recupera Cadastro um novo grupo
        public void CreateRole(string roleName, string LoweredRoleName, int StoreId)
        {
            #region RECUPERA OS PARÂMETROS PARA O INSERT DO NOVO ROLE    
             //MembershipProvider Obj;
             //String ApplicationName = Obj.ApplicationName; // Recupera o Nome da aplicação do Web.Config

              string ApplicationName = "SuperPag";

              Database db = DatabaseFactory.CreateDatabase("fastpag");
              DbCommand dbCommand = db.GetStoredProcCommand("Proc_ReturnApplicationID");
              db.AddInParameter(dbCommand, "@ApplicationName ", DbType.String, ApplicationName);

              Guid _ApplicationID = (Guid)db.ExecuteScalar(dbCommand);//ApplicationId
              Guid _RoleId = Guid.NewGuid(); //RoleId      
              String _RoleName = roleName;   //RoleName
              String _LoweredRoleName = LoweredRoleName;//LoweredRoleName
              int _StoreID = StoreId;        //StoreId

            #endregion

            #region CADASTRA NOVO USUÁRIO
              dbCommand = db.GetStoredProcCommand("Proc_CreateRole");            
              db.AddInParameter(dbCommand, "@ApplicationId ", DbType.Guid, _ApplicationID);
              db.AddInParameter(dbCommand, "@RoleId ", DbType.Guid, _RoleId);
              db.AddInParameter(dbCommand, "@RoleName ", DbType.String, _RoleName);
              db.AddInParameter(dbCommand, "@LoweredRoleName ", DbType.String, _LoweredRoleName);
              db.AddInParameter(dbCommand, "@StoreId ", DbType.Int32, _StoreID);
              db.ExecuteNonQuery(dbCommand);
            #endregion
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //Deleta um Grupo de uma Determinada Loja
        public void DeleteRole(Guid RoleId, int StoreId)
        {
            Role.DeleteRole(RoleId, StoreId);
        }


        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //Recupera TODOS os Grupos de uma determinada loja
        public IList<MRole> GetAllRoles(int StoreID)      
        {
            if (Roles.IsUserInRole("ADMINISTRADORTIVIT").Equals(true))
                return Role.GetRoleInStore(StoreID, 1);
            else
                return Role.GetRoleInStore(StoreID);
        }

        public override string[] GetRolesForUser(string username)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool RoleExists(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] GetAllRoles()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
