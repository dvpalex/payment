using System;
using System.Reflection;

namespace SuperPag.Framework.Helper
{
	public class HierarchySet
	{
		public static void BaseToChild( object _base, object _child )
		{
			BindingFlags fi_flags = BindingFlags.Instance | BindingFlags.Public;
			FieldInfo[] fields = _base.GetType().GetFields( fi_flags );
			if( null != fields )
			{
				foreach( FieldInfo fi in fields )
				{
					FieldInfo _fi = _child.GetType().GetField( fi.Name, fi_flags );
					if( _fi != null )
					{
						_fi.SetValue( _child, fi.GetValue( _base ) );
					}
				}
			}

			BindingFlags pi_flags = BindingFlags.Instance | BindingFlags.Public;
			PropertyInfo[] properties = _base.GetType().GetProperties( pi_flags );
			if( null != properties )
			{
				foreach( PropertyInfo pi in properties )
				{
					PropertyInfo _pi = _child.GetType().GetProperty( pi.Name, pi_flags );
					if( _pi != null )
					{
						if(_pi.CanWrite)
						{
							_pi.SetValue( _child, pi.GetValue( _base, null ), null );
						}						
					}
				}
			}
		}
	}
}
