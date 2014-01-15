using System;
using System.Collections;

namespace SuperPag.Framework.Web.WebController
{
	/// <summary>
	/// Summary description for Security.
	/// </summary>
	public class Security
	{

		//TODO: Coment�rio
		private static Hashtable _hash;

		//TODO: Coment�rio
		public static Hashtable GetActions(string module)
		{
			object actions = _hash[module];
			if(actions != null) return (Hashtable)actions;
			else return new Hashtable();
		}

		//TODO: Coment�rio
		public static string[] GetFunctionalities(string module, string action)
		{
			Hashtable actions = GetActions(module);

			object functionalities = actions[action.ToLower()];
			if(functionalities is System.Array) return (string[])functionalities;
			else return new string[0];			
		}

		//TODO: Coment�rio
		static Security()
		{
			//SC _hash = SecuritySqlData.SoleInstance();
			_hash = new Hashtable();
		}
	}
}
