using System;
using System.Collections.Generic;
using System.Text;
using SuperPag.Business.Messages;
using SuperPag.Data.Messages;
using SuperPag.Data;
using SuperPag.Framework;
using SuperPag.Data.Interfaces;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration;
using System.Collections;
using SuperPag.Business;

using System.Web.Security; //Seguraça


namespace SuperPag.Business
{
    public class Users
    {
        public static Guid Locate(string LoweredUsername)
        {
            DUsers dUsers = DataFactory.Users().Locate(LoweredUsername);

            if (dUsers == null)
                return Guid.Empty;

            return dUsers.UserId;

        }

        public static string Name(Guid UserId)
        {
            if (UserId == Guid.Empty)
                return string.Empty;

            DUsers dUsers = DataFactory.Users().Locate(UserId);
            if (dUsers == null)
                return String.Empty;

            return dUsers.Username;

        }
        
        public static MCUsersInStore List(Guid UserId)
        {
            MCUsersInStore mcUsersInStore = null;
            DUsersInStore[] arrDUsersInStore = DataFactory.UsersInStore().List(UserId);

            if (arrDUsersInStore != null)
            {
                MessageMapper mapper = new MessageMapper();
                mcUsersInStore = (MCUsersInStore)mapper.Do(arrDUsersInStore, typeof(MCUsersInStore));
            }
            else
                mcUsersInStore = new MCUsersInStore();

            return mcUsersInStore;

        }

        public static MCUsersInStore List(string LoweredUsername)
        {
            DUsers dUsers = DataFactory.Users().Locate(LoweredUsername);

            if (dUsers != null)
                return Users.List(dUsers.UserId);

            return new MCUsersInStore();

        }

        public static int StoreByUser(string LoweredUsername)
        {
            MCUsersInStore mcUsersInStore = Users.List(LoweredUsername);
            if(mcUsersInStore.Count == 0)
                throw new ApplicationException("Nenhuma loja cadastrada para o usuário.");

            return ((MUsersInStore)mcUsersInStore[0]).StoreId;
        }

        //Passando o Nome, retorna a chave primária de um determinado Usuário
        public static Guid RetunUserId(string UserName)
        {
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_RecuperaUserId");
            db.AddInParameter(dbCommand, "@UserName", DbType.String, UserName);

            Guid Key = (Guid)db.ExecuteScalar(dbCommand);

            return Key;
        }
    }
}
