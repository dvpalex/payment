using System;
using System.Reflection;

namespace SuperPag.Framework.Helper
{
	public sealed class Attributes
	{
		public static object[] GetAllAttribute( MemberInfo member , Type attributeType )
		{
			object[] attribs = member.GetCustomAttributes ( attributeType,  false );

			return attribs;
		}

		public static object GetSingleAttribute( MemberInfo member , Type attributeType )
		{
			object[] attribs = member.GetCustomAttributes ( attributeType , false );

			if ( null != attribs && attribs.Length > 0 )
			{
				if ( attribs.Length != 1 ) throw new ArgumentOutOfRangeException ( attributeType.Name );

				return attribs [ 0 ];
			}
			return null;
		}
	}
}
