using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("UsersInRoles")]
	public class DUsersInRoles : DataMessageBase
	{
		public DUsersInRoles() {}

		public class Fields
		{
			public const string UserId = "UserId";
			public const string RoleId = "RoleId";
		}

		[PrimaryKey]
		public Guid UserId;
		[PrimaryKey]
		public Guid RoleId;
	}
}
