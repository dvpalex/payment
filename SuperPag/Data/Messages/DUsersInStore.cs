using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Data.Messages
{
    [DefaultDataTableName("UsersInStore")]
    public class DUsersInStore : DataMessageBase
    {
        public DUsersInStore() { }

		public class Fields
		{
			public const string UserId = "UserId";
            public const string storeId = "storeId";
		}

		[PrimaryKey]
		public Guid UserId;
		[PrimaryKey]
        public int storeId;
    }
}
