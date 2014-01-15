using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("Roles")]
	public class DRoles : DataMessageBase
	{

		public DRoles() {}

		public class Fields
		{
			public const string RoleId = "RoleId";
			public const string RoleName = "RoleName";
			public const string LoweredRoleName = "LoweredRoleName";
			public const string Description = "Description";
		}

		[PrimaryKey]
		public Guid RoleId;
        public string RoleName;
        public string LoweredRoleName;
        public string Description;
	}
}
