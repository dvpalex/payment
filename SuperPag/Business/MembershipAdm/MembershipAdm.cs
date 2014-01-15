using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;


namespace SuperPag.Business.MembershipAdm
{
    public sealed class MemberShipProviderAdm : MembershipProvider
    {
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

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool EnablePasswordReset
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public MembershipUserCollection FindUsersByName(string usernameToMatch, int StoreId)
        {
            //Retorna uusários de uma determinada Loja e com Iniciais iguais as do filtro
            MembershipUserCollection ObjLista = new MembershipUserCollection();

            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetUserInStore");
            db.AddInParameter(dbCommand, "@UserName", DbType.String, usernameToMatch);
            db.AddInParameter(dbCommand, "@StoreID", DbType.Int32, StoreId);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr != null)
                {
                    //Objeto auxiliar para validar a duplicidade de usuários
                    List<string> str = new List<string>();

                    while (dr.Read())
                    {
                        //Valida duplicidade de usuários na lista
                        if (!str.Contains(dr.GetString(dr.GetOrdinal("UserName"))))
                        {
                            str.Add(dr.GetString(dr.GetOrdinal("UserName")));
                            ObjLista.Add(new MembershipUser("SqlMembershipProvider",//ProvaderName
                                                           dr.GetString(dr.GetOrdinal("UserName")),
                                                           dr.GetGuid(dr.GetOrdinal("UserId")),
                                                           dr.IsDBNull(dr.GetOrdinal("Email")) ? String.Empty : dr.GetString(dr.GetOrdinal("Email")),
                                                           String.Empty,//PasswordQuestion
                                                           dr.IsDBNull(dr.GetOrdinal("Comment")) ? String.Empty : dr.GetString(dr.GetOrdinal("Comment")),
                                                           dr.GetBoolean(dr.GetOrdinal("IsApproved")),
                                                           dr.IsDBNull(dr.GetOrdinal("IsLockedOut")) ? true : dr.GetBoolean(dr.GetOrdinal("IsLockedOut")),
                                                           DateTime.MinValue,//Date
                                                           DateTime.MinValue,//Date
                                                           DateTime.MinValue,//Date
                                                           DateTime.MinValue,//Date
                                                           DateTime.MinValue//Date
                                                          )
                                      );
                        }
                    }
                }
            }
            return ObjLista;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetPassword(string username, string answer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool UnlockUser(string userName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool ValidateUser(string username, string password)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public MembershipUserCollection GetAllUsers(int StoreId)
        {
          //Retorna TODOS os usuários de uma determinada Loja
            MembershipUserCollection ObjLista  = new MembershipUserCollection();
            
            Database db = DatabaseFactory.CreateDatabase("fastpag");
            DbCommand dbCommand = db.GetStoredProcCommand("Proc_GetAllUserInStore");
            db.AddInParameter(dbCommand, "@StoreID", DbType.Int32, StoreId);
            
            using (IDataReader dr = db.ExecuteReader(dbCommand))
             {                   
                if (dr != null)
                {
                    //Objeto auxiliar para validar a duplicidade de usuários
                    List<string> str = new List<string>();

                    while (dr.Read())
                    {
                        //Valida duplicidade de usuários na lista
                       if (!str.Contains(dr.GetString(dr.GetOrdinal("UserName"))))
                       {
                          str.Add(dr.GetString(dr.GetOrdinal("UserName")));
                          ObjLista.Add(new MembershipUser("SqlMembershipProvider",//ProvaderName
                                                          dr.GetString(dr.GetOrdinal("UserName")),
                                                          dr.GetGuid(dr.GetOrdinal("UserId")),
                                                          dr.IsDBNull(dr.GetOrdinal("Email")) ? String.Empty : dr.GetString(dr.GetOrdinal("Email")),
                                                          String.Empty,//PasswordQuestion
                                                          dr.IsDBNull(dr.GetOrdinal("Comment")) ? String.Empty : dr.GetString(dr.GetOrdinal("Comment")),
                                                          dr.GetBoolean(dr.GetOrdinal("IsApproved")),
                                                          dr.IsDBNull(dr.GetOrdinal("IsLockedOut")) ? true : dr.GetBoolean(dr.GetOrdinal("IsLockedOut")),
                                                          DateTime.MinValue,//Date
                                                          DateTime.MinValue,//Date
                                                          DateTime.MinValue,//Date
                                                          DateTime.MinValue,//Date
                                                          DateTime.MinValue //Date
                                                      )
                                    );
                       }
                    }
                }
            }
        return ObjLista;
        }

    }
}
