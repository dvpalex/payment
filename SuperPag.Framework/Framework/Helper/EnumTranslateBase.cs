using System;
using System.Threading;
using System.Resources;
using System.Globalization;
using System.Reflection;

namespace SuperPag.Framework.Helper
{

	public abstract class EnumListBuilderBase 
	{
		protected Type _type;
		private string _typeName;

		public abstract EnumKeyText[] GetList();

		public string TypeName 
		{
			 get { return _typeName; }
			 set { _typeName = value; }
		}
		
		public EnumListBuilderBase(Type enumType) 
		{
			_typeName = enumType.Name;
			_type = enumType;
		}
	}

	[Serializable()]
	public class EnumKeyText 
	{
		public int _key;
		public string _text;

		public int Key 
		{
			get { return _key; }
			set { _key = value; }
		}

		public string Text 
		{
			get { return _text; }
			set { _text = value; }
		}
	}

	//////	public class EnumTranslate {
	//////		private ResourceManager _resManager;
	//////		private Assembly _callingAssembly;
	//////
	//////		public EnumTranslate() {
	//////			//Obtem o assembly chamador
	//////			_callingAssembly = Assembly.GetCallingAssembly();
	//////			//Obtem o path para o resource EnumTranslate
	//////			string resPath = ResourcesHelper.FindResourcePath(_callingAssembly, "EnumTranslate");
	//////			//Remove o .resources do path
	//////			resPath = resPath.Replace(".resources", "");
	//////			//Obtem o ResourceManager
	//////			_resManager	= new ResourceManager(resPath, _callingAssembly);
	//////		}
	//////
	//////		public ~EnumTranslate() {
	//////			//TODO: verificar
	//////			_resManager.ReleaseAllResources();
	//////		}
	//////	}

	public abstract class EnumTranslateBase 
	{
		//TODO: este método estava público
		internal static string Teste(Type type, int keyValue) 
		{
			Assembly asm = Assembly.GetCallingAssembly();
			string path = ResourcesHelper.FindResourcePath(asm, "EnumTranslate");
			path = path.Replace(".resources", "");
			string[] x = asm.GetManifestResourceNames();
			ResourceManager man = new ResourceManager(path, asm);
		
			return man.GetString(type.Name + "_" + keyValue.ToString());
		}

		public static string GetTranslateValue(Type type, int keyValue, ResourceManager res) 
		{
			//Monta a chave para pesquisa no resources
			string resKey = type.Name + "_" + keyValue.ToString();

			return res.GetString(resKey);
		}

		public static EnumKeyText[] GetTranslateEnumValues(Type type, ResourceManager res, bool firstEmpty) 
		{
			//Obtem a lista de valores do enum
			Array arrayOfValues = Enum.GetValues(type);
			System.Collections.ArrayList listOfItens = new System.Collections.ArrayList();
			
			if (firstEmpty) 
			{
				EnumKeyText emptyKeyText = new EnumKeyText();
				emptyKeyText._key = int.MinValue;
				emptyKeyText._text = "";
				listOfItens.Add(emptyKeyText);
			}
			
			//Percorre cada key e obtem o texto da resource
			for (int i = 0; i < arrayOfValues.Length; i++) 
			{
				int keyValue = Convert.ToInt32(arrayOfValues.GetValue(i));
				//Verifica se não é int.minvalue
				if (keyValue != int.MinValue) 
				{
					EnumKeyText enumKeyText = new EnumKeyText();
					enumKeyText._key = keyValue;
					enumKeyText._text = GetTranslateValue(type, keyValue, res);

					listOfItens.Add(enumKeyText);
				}
			}

			return (EnumKeyText[])listOfItens.ToArray(typeof(EnumKeyText));
		}
	}
}
