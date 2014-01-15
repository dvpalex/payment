using System;

namespace SuperPag.Framework.Helper
{
	public class Types
	{
		public static Type ExtractType( string typeName )
		{
			Ensure.IsNotNull ( typeName , "typeName" );
			
			int pos = typeName.IndexOf ( ",") ;
			
			if ( pos == -1 ) throw new ArgumentException ( " Missing assembly information " );

			string type = typeName.Substring ( 0 , pos );
			string assemblyName = typeName.Substring ( pos + 1);
			assemblyName = assemblyName.Trim();

			System.Reflection.Assembly asm = System.Reflection.Assembly.Load ( assemblyName );

			return asm.GetType ( type , true );

		}
	}
}
