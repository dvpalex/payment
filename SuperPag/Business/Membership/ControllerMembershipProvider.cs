using System;
using System.Text;
using System.Security.Cryptography;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Configuration;
using SuperPag.Data;
using SuperPag.Data.Messages;

namespace SuperPag.Business.Membership
{
    public sealed class ControllerMemberShipProvider : MembershipProvider
    {
        private enum FailureType
        {
            Password = 1,
            PasswordAnswer = 2
        }


        private string usernameRegularExpression;
        private string passwordStrengthRegularExpression;
        private int newPasswordLength;
        private int minRequiredPasswordLength;
        private int minRequiredNonAlphanumericCharacters;
        private int maxInvalidPasswordAttempts;
        private int passwordAttemptWindow;
        private MembershipPasswordFormat passwordFormat;
        private int passwordFormatInt;
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval;
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;
        private MachineKeySection machineKey;

        public override string ApplicationName
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
        public override bool EnablePasswordReset
        {
            get { return enablePasswordReset; }
        }
        public override bool EnablePasswordRetrieval
        {
            get { return enablePasswordRetrieval; }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { return requiresQuestionAndAnswer; }
        }
        public override bool RequiresUniqueEmail
        {
            get { return requiresUniqueEmail; }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { return maxInvalidPasswordAttempts; }
        }
        public override int PasswordAttemptWindow
        {
            get { return passwordAttemptWindow; }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return passwordFormat; }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return minRequiredNonAlphanumericCharacters; }
        }
        public override int MinRequiredPasswordLength
        {
            get { return minRequiredPasswordLength; }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { return passwordStrengthRegularExpression; }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            #region Default Implementation to call Base method
            if (config == null)
                throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name))
                name = "ControllerMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Superpag Membership Provider");
            }

            base.Initialize(name, config);
            #endregion

            #region Setting attributes...
            usernameRegularExpression = GetConfigValue(config["UsernameRegularExpression"], @"^[a-zA-Z][a-zA-Z0-9_-]{3,}$");
            config.Remove("UsernameRegularExpression");

            passwordStrengthRegularExpression = GetConfigValue(config["PasswordStrengthRegularExpression"], @"");
            config.Remove("PasswordStrengthRegularExpression");

            newPasswordLength = int.Parse(GetConfigValue(config["NewPasswordLength"], "8"));
            config.Remove("NewPasswordLength");

            minRequiredPasswordLength = int.Parse(GetConfigValue(config["MinRequiredPasswordLength"], "6"));
            config.Remove("MinRequiredPasswordLength");

            minRequiredNonAlphanumericCharacters = int.Parse(GetConfigValue(config["MinRequiredNonAlphanumericCharacters"], "0"));
            config.Remove("MinRequiredNonAlphanumericCharacters");

            maxInvalidPasswordAttempts = int.Parse(GetConfigValue(config["MaxInvalidPasswordAttempts"], "3"));
            config.Remove("MaxInvalidPasswordAttempts");

            passwordAttemptWindow = int.Parse(GetConfigValue(config["PasswordAttemptWindow"], "15"));
            config.Remove("PasswordAttemptWindow");

            enablePasswordReset = bool.Parse(GetConfigValue(config["EnablePasswordReset"], "true"));
            config.Remove("EnablePasswordReset");

            enablePasswordRetrieval = bool.Parse(GetConfigValue(config["EnablePasswordRetrieval"], "true"));
            config.Remove("EnablePasswordRetrieval");

            requiresQuestionAndAnswer = bool.Parse(GetConfigValue(config["RequiresQuestionAndAnswer"], "true"));
            config.Remove("RequiresQuestionAndAnswer");

            requiresUniqueEmail = bool.Parse(GetConfigValue(config["RequiresUniqueEmail"], "true"));
            config.Remove("RequiresUniqueEmail");

            string tempPasswordFormat = GetConfigValue(config["PasswordFormat"], "Hashed");
            switch (tempPasswordFormat)
            {
                case "Hashed":
                    passwordFormat = MembershipPasswordFormat.Hashed;
                    passwordFormatInt = (int)MembershipPasswordFormat.Hashed;
                    enablePasswordRetrieval = false;
                    break;
                case "Encrypted":
                    passwordFormat = MembershipPasswordFormat.Encrypted;
                    passwordFormatInt = (int)MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    passwordFormat = MembershipPasswordFormat.Clear;
                    passwordFormatInt = (int)MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new SuperPag.Framework.FWCException("Password format not supported.");
            }
            config.Remove("PasswordFormat");



            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException
                        ("Unrecognized attribute: " + attr);
            }
            #endregion

            Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");
        }
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //if (!ValidateUser(username, oldPassword))
            // return false;

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
            }

            DUsers user = DataFactory.Users().Locate(username.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found.");

            user.Password = EncodePassword(newPassword);
            user.LastPasswordChangedDate = DateTime.Now;
            user.LastActivityDate = DateTime.Now;

            DataFactory.Users().Update(user);
            
            return true;
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            if (!ValidateUser(username, password))
                return false;

            DUsers user = DataFactory.Users().Locate(username.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found.");

            user.PasswordQuestion = newPasswordQuestion;
            user.PasswordAnswer = EncodePassword(newPasswordAnswer);
            user.LastActivityDate = DateTime.Now;

            DataFactory.Users().Update(user);
            
            return true;
        }
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            try
            {
                Regex reg = new Regex(@usernameRegularExpression);
                if (!reg.IsMatch(username))
                {
                    status = MembershipCreateStatus.InvalidUserName;
                    return null;
                }

                DUsers user = DataFactory.Users().Locate(username.ToLower());
                if (user != null)
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                    return null;
                }

                user = DataFactory.Users().LocateByEmail(email.ToLower());
                if (RequiresUniqueEmail && user != null)
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                    return null;
                }

                if (providerUserKey == null)
                {
                    providerUserKey = Guid.NewGuid();
                }
                else
                {
                    if (!(providerUserKey is Guid))
                    {
                        status = MembershipCreateStatus.InvalidProviderUserKey;
                        return null;
                    }
                }

                user = new DUsers();

                user.UserId = (Guid)providerUserKey;
                user.Username = username;
                user.LoweredUsername = username.ToLower();
                user.Password = EncodePassword(password);
                user.PasswordFormat = passwordFormatInt;
                user.Email = email;
                user.LoweredEmail = email.ToLower();
                user.PasswordQuestion = passwordQuestion;
                user.PasswordAnswer = passwordAnswer;
                user.IsApproved = isApproved;
                user.IsLockedOut = false;
                user.IsOnLine = false;
                user.CreateDate = DateTime.Now;
                user.LastLoginDate = DateTime.Now;
                user.LastPasswordChangedDate = DateTime.Now;
                user.LastLockedOutDate = DateTime.Now;
                user.LastActivityDate = DateTime.Now;
                user.FailedPasswordAttemptCount = 0;
#if SQL
                user.FailedPasswordAttemptWindowStart = DateTime.Now;
                user.FailedPasswordAnswerAttemptCount = 0;
                user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
#elif ORACLE
                user.FailedPasswordAttemptWindowSta = DateTime.Now;
                user.FailedPasswordAnswerAttemptCou = 0;
                user.FailedPasswordAnswerAttemptWin = DateTime.Now;
#else
                user.FailedPasswordAttemptWindowStart = DateTime.Now;
                user.FailedPasswordAnswerAttemptCount = 0;
                user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
#endif

                DataFactory.Users().Insert(user);

                status = MembershipCreateStatus.Success;
                return NewMembershipUserFromUsersEntity(user);
            }
            catch (Exception ex)
            {
                status = MembershipCreateStatus.ProviderError;
                throw new MembershipCreateUserException(ex.Message);
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            DUsers user = DataFactory.Users().Locate(username.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found.");

            if (!deleteAllRelatedData)
            {
                DataFactory.Users().Delete(user.UserId);
                return true;
            }
            else
            {
                //TODO: Verificar todas as tabelas que usam o User e deletar.
                return false;
            }
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection muCol = new MembershipUserCollection();

            DUsers[] usersPaged = null;
            DUsers[] users = DataFactory.Users().ListSortedByUsername();
            if(users != null)
                usersPaged = GetMulti(users, pageIndex, pageSize);

            foreach (DUsers user in usersPaged)
                muCol.Add(NewMembershipUserFromUsersEntity(user));

            totalRecords = muCol.Count;
            return muCol;
        }
        public override int GetNumberOfUsersOnline()
        {
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

            DUsers[] users = DataFactory.Users().ListGraterLastActivityDate(compareTime);

            return (users != null ? users.Length : 0);
        }
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
                throw new SuperPag.Framework.FWCException("Password Retrieval Not Enabled.");

            DUsers user = DataFactory.Users().Locate(username.ToLower());
            if (user == null)
                throw new MembershipPasswordException("Username not found.");

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, user.PasswordAnswer))
            {
                UpdateFailureCount(user, FailureType.PasswordAnswer);
                throw new MembershipPasswordException("Incorrect password answer.");
            }

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    return user.Password;
                case MembershipPasswordFormat.Encrypted:
                    return UnEncodePassword(user.Password);
                case MembershipPasswordFormat.Hashed:
                    throw new SuperPag.Framework.FWCException("Cannot retrieve Hashed passwords.");
                default:
                    throw new SuperPag.Framework.FWCException("Unknow password format.");
            }
        }
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            DUsers user = DataFactory.Users().Locate(username.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found.");

            if (userIsOnline)
            {
                user.LastActivityDate = DateTime.Now;

                DataFactory.Users().Update(user);
            }

            return NewMembershipUserFromUsersEntity(user);
        }
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (!(providerUserKey is Guid))
                throw new SuperPag.Framework.FWCException("Invalid UserId.");

            DUsers user = DataFactory.Users().Locate((Guid)providerUserKey);
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found.");

            if (userIsOnline)
            {
                user.LastActivityDate = DateTime.Now;

                DataFactory.Users().Update(user);
            }

            return NewMembershipUserFromUsersEntity(user);
        }
        public override bool UnlockUser(string userName)
        {
            DUsers user = DataFactory.Users().Locate(userName.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found.");

            user.IsLockedOut = false;
            user.LastActivityDate = DateTime.Now;

            DataFactory.Users().Update(user);

            return true;
        }
        public override string GetUserNameByEmail(string email)
        {
            DUsers user = DataFactory.Users().LocateByEmail(email.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("User's email not found.");

            return user.Username;
        }
        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
                throw new NotSupportedException("Password reset is not enabled.");

            DUsers user = DataFactory.Users().Locate(username.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found");

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(user, FailureType.PasswordAnswer);
                throw new ProviderException("Password answer required for password reset.");
            }

            string newPassword = "";// Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");
            }

            if (user.IsLockedOut == true)
                throw new MembershipPasswordException("The supplied user is locked out.");

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, user.PasswordAnswer))
            {
                UpdateFailureCount(user, FailureType.PasswordAnswer);
                throw new MembershipPasswordException("Incorrect password answer.");
            }

            user.Password = newPassword;
            user.LastPasswordChangedDate = DateTime.Now;
            user.LastActivityDate = DateTime.Now;

            DataFactory.Users().Update(user);

            return newPassword;
        }


        public override void UpdateUser(MembershipUser mUser)
        {
            DUsers user = DataFactory.Users().Locate(mUser.UserName.ToLower());
            if (user == null)
                throw new SuperPag.Framework.FWCException("Username not found.");

            //UsersEntity emailUser = new UsersEntity();
            //            if (RequiresUniqueEmail && emailUser.FetchUsingUCLoweredEmail(mUser.Email.ToLower()))
            //              throw new SuperPag.Framework.FWCException("Email already exists.");

            user.Email = mUser.Email;
            user.LoweredEmail = mUser.Email.ToLower();
            user.IsApproved = mUser.IsApproved;
#if SQL
            user.Comment = mUser.Comment;
#elif ORACLE
            user.Comments = mUser.Comment;
#else
            user.Comment = mUser.Comment;
#endif
            user.LastActivityDate = DateTime.Now;
            // user.Password = EncodePassword("000000");

            DataFactory.Users().Update(user);
        }
        public override bool ValidateUser(string username, string password)
        {
            DUsers user = DataFactory.Users().Locate(username.ToLower());
            if (user == null)
                return false;

            if (user.IsLockedOut || !user.IsApproved)
                return false;

            if (!CheckPassword(password, user.Password))
            {
                UpdateFailureCount(user, FailureType.Password);
                return false;
            }

            user.LastLoginDate = DateTime.Now;
            user.LastActivityDate = DateTime.Now;

            DataFactory.Users().Update(user);
            
            return true;
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection muCol = new MembershipUserCollection();

            DUsers[] usersPaged = null;
            DUsers[] users = DataFactory.Users().ListLikeByUsername("%" + usernameToMatch + "%");
            if (users != null)
                usersPaged = GetMulti(users, pageIndex, pageSize);

            foreach (DUsers user in usersPaged)
                muCol.Add(NewMembershipUserFromUsersEntity(user));

            totalRecords = muCol.Count;

            return muCol;
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection muCol = new MembershipUserCollection();

            DUsers[] usersPaged = null;
            DUsers[] users = DataFactory.Users().ListLikeByEmail("%" + emailToMatch + "%");
            if (users != null)
                usersPaged = GetMulti(users, pageIndex, pageSize);
            
            foreach (DUsers user in users)
                muCol.Add(NewMembershipUserFromUsersEntity(user));

            totalRecords = muCol.Count;

            return muCol;
        }

        protected override byte[] DecryptPassword(byte[] encodedPassword)
        {
            return base.DecryptPassword(encodedPassword);
        }
        protected override byte[] EncryptPassword(byte[] password)
        {
            return base.EncryptPassword(password);
        }
        protected override void OnValidatingPassword(ValidatePasswordEventArgs e)
        {
            base.OnValidatingPassword(e);
        }

        #region Helper Functions
        private MembershipUser NewMembershipUserFromUsersEntity(DUsers user)
        {
#if SQL
            return new MembershipUser(this.Name, user.Username, user.UserId,
                                    user.Email, user.PasswordQuestion, user.Comment, user.IsApproved,
                                    user.IsLockedOut, user.CreateDate, user.LastLoginDate, user.LastActivityDate,
                                    user.LastPasswordChangedDate, user.LastLockedOutDate);
#elif ORACLE
            return new MembershipUser(this.Name, user.Username, user.UserId,
                                    user.Email, user.PasswordQuestion, user.Comments, user.IsApproved,
                                    user.IsLockedOut, user.CreateDate, user.LastLoginDate, user.LastActivityDate,
                                    user.LastPasswordChangedDate, user.LastLockedOutDate);
#else
            return new MembershipUser(this.Name, user.Username, user.UserId,
                                    user.Email, user.PasswordQuestion, user.Comment, user.IsApproved,
                                    user.IsLockedOut, user.CreateDate, user.LastLoginDate, user.LastActivityDate,
                                    user.LastPasswordChangedDate, user.LastLockedOutDate);
#endif
        }
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }
        private bool CheckPassword(string password, string dataPassword)
        {
            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    dataPassword = UnEncodePassword(dataPassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    password = EncodePassword(password);
                    break;
                default:
                    break;
            }

            return (password == dataPassword);
        }
        private string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword = Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    SHA1 hash = new SHA1CryptoServiceProvider();
                    encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new SuperPag.Framework.FWCException("Unsupported password format.");
            }

            return encodedPassword;
        }
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new SuperPag.Framework.FWCException("Cannot unencode a hashed password.");
                default:
                    throw new SuperPag.Framework.FWCException("Unsupported password format.");
            }

            return password;
        }
        private void UpdateFailureCount(DUsers user, FailureType failureType)
        {
            int failureCount = 0;
            DateTime windowStart = new DateTime();

            switch (failureType)
            {
                case FailureType.Password:
                    failureCount = user.FailedPasswordAttemptCount;
#if SQL
                    windowStart = user.FailedPasswordAttemptWindowStart;
#elif ORACLE
                    windowStart = user.FailedPasswordAttemptWindowSta;
#else
                    windowStart = user.FailedPasswordAttemptWindowStart;
#endif
                    break;
                case FailureType.PasswordAnswer:
#if SQL
                    failureCount = user.FailedPasswordAnswerAttemptCount;
                    windowStart = user.FailedPasswordAnswerAttemptWindowStart;
#elif ORACLE
                    failureCount = user.FailedPasswordAnswerAttemptCou;
                    windowStart = user.FailedPasswordAnswerAttemptWin;
#else
                    failureCount = user.FailedPasswordAnswerAttemptCount;
                    windowStart = user.FailedPasswordAnswerAttemptWindowStart;
#endif
                    break;
            }

            DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);
            if (failureCount == 0 || DateTime.Now > windowEnd)
            {
                // First password failure or outside of PasswordAttemptWindow. 
                // Start a new password failure count from 1 and a new window starting now.
                switch (failureType)
                {
                    case FailureType.Password:
                        user.FailedPasswordAttemptCount = 1;
#if SQL
                        user.FailedPasswordAttemptWindowStart = DateTime.Now;
#elif ORACLE
                        user.FailedPasswordAttemptWindowSta = DateTime.Now;
#else
                        user.FailedPasswordAttemptWindowStart = DateTime.Now;
#endif
                        break;
                    case FailureType.PasswordAnswer:
#if SQL
                        user.FailedPasswordAnswerAttemptCount = 1;
                        user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
#elif ORACLE
                        user.FailedPasswordAnswerAttemptCou = 1;
                        user.FailedPasswordAnswerAttemptWin = DateTime.Now;
#else
                        user.FailedPasswordAnswerAttemptCount = 1;
                        user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
#endif
                        break;
                }
            }
            else
            {
                if (failureCount++ >= MaxInvalidPasswordAttempts)
                {
                    // Password attempts have exceeded the failure threshold. Lock out
                    // the user.
                    user.IsLockedOut = true;
                    user.LastLockedOutDate = DateTime.Now;
                }
                else
                {
                    // Password attempts have not exceeded the failure threshold. Update
                    // the failure counts. Leave the window the same.
                    if (failureType == FailureType.Password)
                        user.FailedPasswordAttemptCount = failureCount;
                    if (failureType == FailureType.PasswordAnswer)
#if SQL
                        user.FailedPasswordAnswerAttemptCount = failureCount;
#elif ORACLE
                        user.FailedPasswordAnswerAttemptCou = failureCount;
#else
                        user.FailedPasswordAnswerAttemptCount = failureCount;
#endif
                }
            }

            DataFactory.Users().Update(user);
        }
        private DUsers[] GetMulti(DUsers[] users, int pageIndex, int pageSize)
        {
            if (users == null || pageIndex < 1 || pageSize < 1)
                return null;

            decimal pages = decimal.Ceiling(((decimal)users.Length) / pageSize);
            
            if (pageIndex > pages)
                return null;

            DUsers[] newUsers = new DUsers[(pageIndex == pages && ((users.Length % pageSize) != 0) ? users.Length % pageSize : pageSize)];

            for (int i = 0; i < newUsers.Length; i++)
                newUsers[i] = users[((pageIndex - 1) * pageSize) + i];

            return newUsers;
        }
        #endregion
    }
}
