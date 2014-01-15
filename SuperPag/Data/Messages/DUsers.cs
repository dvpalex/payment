using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("Users")]
	public class DUsers : DataMessageBase
	{

		public DUsers() {}

		public class Fields
		{
			public const string UserId = "UserId";
			public const string Username = "Username";
			public const string LoweredUsername = "LoweredUsername";
			public const string Password = "Password";
			public const string PasswordFormat = "PasswordFormat";
			public const string Email = "Email";
			public const string LoweredEmail = "LoweredEmail";
			public const string PasswordQuestion = "PasswordQuestion";
			public const string PasswordAnswer = "PasswordAnswer";
			public const string IsApproved = "IsApproved";
			public const string IsLockedOut = "IsLockedOut";
			public const string IsOnLine = "IsOnLine";
			public const string CreateDate = "CreateDate";
			public const string LastLoginDate = "LastLoginDate";
			public const string LastPasswordChangedDate = "LastPasswordChangedDate";
			public const string LastLockedOutDate = "LastLockedOutDate";
			public const string LastActivityDate = "LastActivityDate";
			public const string FailedPasswordAttemptCount = "FailedPasswordAttemptCount";
#if SQL
			public const string FailedPasswordAttemptWindowStart = "FailedPasswordAttemptWindowStart";
			public const string FailedPasswordAnswerAttemptCount = "FailedPasswordAnswerAttemptCount";
            public const string FailedPasswordAnswerAttemptWindowStart = "FailedPasswordAnswerAttemptWindowStart";
			public const string Comment = "Comment";
#elif ORACLE
            public const string FailedPasswordAttemptWindowSta = "FailedPasswordAttemptWindowSta";
			public const string FailedPasswordAnswerAttemptCou = "FailedPasswordAnswerAttemptCou";
            public const string FailedPasswordAnswerAttemptWin = "FailedPasswordAnswerAttemptWin";
            public const string Comments = "Comments";
#else
            public const string FailedPasswordAttemptWindowStart = "FailedPasswordAttemptWindowStart";
            public const string FailedPasswordAnswerAttemptCount = "FailedPasswordAnswerAttemptCount";
            public const string FailedPasswordAnswerAttemptWindowStart = "FailedPasswordAnswerAttemptWindowStart";
			public const string Comment = "Comment";
#endif
        }

		[PrimaryKey]
		public Guid UserId;
        public string Username;
        public string LoweredUsername;
        public string Password;
		public int PasswordFormat;
        public string Email;
        public string LoweredEmail;
        public string PasswordQuestion;
        public string PasswordAnswer;
		public bool IsApproved;
		public bool IsLockedOut;
		public bool IsOnLine;
		public DateTime CreateDate;
		public DateTime LastLoginDate;
		public DateTime LastPasswordChangedDate;
		public DateTime LastLockedOutDate;
		public DateTime LastActivityDate;
		public int FailedPasswordAttemptCount;
#if SQL
		public DateTime FailedPasswordAttemptWindowStart;
        public int FailedPasswordAnswerAttemptCount;
        public DateTime FailedPasswordAnswerAttemptWindowStart;
        public string Comment;
#elif ORACLE
        public DateTime FailedPasswordAttemptWindowSta;
        public int FailedPasswordAnswerAttemptCou;
        public DateTime FailedPasswordAnswerAttemptWin;
        public string Comments;
#else
        public DateTime FailedPasswordAttemptWindowStart;
        public int FailedPasswordAnswerAttemptCount;
        public DateTime FailedPasswordAnswerAttemptWindowStart;
        public string Comment;
#endif
    }
}
