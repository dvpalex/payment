using System;
using System.Collections;
using System.Reflection;

namespace SuperPag.Framework.Helper
{
	public class Reflection
	{
		public static bool Implements( Type memberType,  Type interfaceType )
		{
			if ( memberType.IsClass && memberType != typeof ( System.String ) )
			{
				if ( memberType.IsArray )
				{
					return memberType.GetElementType().GetInterface ( interfaceType.FullName, false ) != null ;
				}			
				else
				{
					return memberType.GetInterface ( interfaceType.FullName, false ) != null ;
				}
			}
			
			return false;
		}	
	}
}
