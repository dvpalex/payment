using System;
using System.Reflection;

namespace SuperPag.Framework.Data.Builder.Parser
{
	internal class MethodParser
	{
		DataMethod[] _dataMethods;
		MethodInfo[] _interfaceMethods ;
		Type _interfaceType;

		public MethodParser ( Type interfaceType )
		{
			this._interfaceType = interfaceType;
			
			this._interfaceMethods = interfaceType.GetMethods();

			_dataMethods = new DataMethod [ _interfaceMethods.Length ];
		}

		public DataMethod[] ExtractAllMethods ( )
		{

//			foreach ( MethodInfo m in _interfaceMethods )
//			{
//				//Se for select
//				//Monta a clausa de select
//
//				//Se for delete
//				//Monta a clausa de select
//
//				//Se for update
//				//Monta a clausa de select
//
//				//Se for insert
//				//Monta a clausa de insert			
//
//				//se for update , select ou delete
//					//Monta a clausula de where
//				
//				//se for select
//					//monta a clausa de orderby
//
//				//Concatena as clausulas
//
//				//Seta no metodo
//			}

			return null;
		}
	
	}
}
