using System;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using SuperPag.Framework;
using SuperPag.Framework.Helper;

namespace SuperPag.Business.Messages
{
    public class EnumTranslate : EnumTranslateBase 
	{
		public static string TranslateValue(Type type, int keyValue) 
		{	
			return EnumTranslateBase.GetTranslateValue(type, keyValue, GetResourceManager());
		}

		public static EnumKeyText[] TranslateEnumValues(Type type, bool firstEmpty) 
		{
			return EnumTranslateBase.GetTranslateEnumValues(type, GetResourceManager(), firstEmpty);
		}

		private static ResourceManager GetResourceManager() 
		{
			return new ResourceManager(typeof(EnumTranslate));
		}

	}

	public class EnumListBuilder : EnumListBuilderBase
	{
		public EnumListBuilder(Type type): base(type){}

		public override EnumKeyText[] GetList()
		{
			return EnumTranslate.TranslateEnumValues(this._type, false);
		}
	}

}