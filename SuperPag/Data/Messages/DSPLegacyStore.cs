using System;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;


namespace SuperPag.Data.Messages
{
	[DefaultDataTableName("SPLegacyStore")]
	public class DSPLegacyStore : DataMessageBase
	{

		public DSPLegacyStore() {}

		public class Fields
		{
			public const string storeId = "storeId";
			public const string ucTableTop = "ucTableTop";
		}

		[PrimaryKey]
		public int storeId;
		public string ucTableTop;
	}
}
