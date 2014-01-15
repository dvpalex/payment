using System;

namespace SuperPag.Framework.Helper
{
	public sealed class Check
	{
		public static bool IsNumeric( string value )
		{
			if ( value == null || value == string.Empty )
				return false;

			foreach ( char c in value )
			{
				if ( ! Char.IsNumber ( c ) ) return false;
			}

			return true; 		
		}

		public static bool IsEmpty( string value )
		{
			return value == null && value == string.Empty;		
		}
	}

	/*
	public sealed class Ensure
	{
		public static void IsNotNull( object o )
		{
			if ( o == null ) throw new ArgumentNullException ( );			
		}

		public static void IsNotNull( object o , string paramName )
		{
			if ( o == null ) throw new ApplicationException ( paramName );			
		}
	}
	*/

	public sealed class Ensure
	{
		private Ensure(){}
		
		public static bool DateTimeIsValid( DateTime value, string message ) //throws
		{
			if( false == DateTimeIsValid( value ) )
				throw new ApplicationException( message );			

			return true;
		}

		public static bool NumberIsValid( double value, string message ) //throws
		{
			if( false == NumberIsValid( value ) )
				throw new ApplicationException( message );			

			return true;
		}
		
		public static bool NumberIsValid( decimal value, string message ) //throws
		{
			if( false == NumberIsValid( value ) )
				throw new ApplicationException( message );			

			return true;
		}
		
		public static bool NumberIsValid( ulong value, string message ) //throws
		{
			if( false == NumberIsValid( value ) )
				throw new ApplicationException( message );			

			return true;
		}
		
		public static bool NumberIsValid( ushort value, string message ) //throws
		{
			if( false == NumberIsValid( value ) )
				throw new ApplicationException( message );			

			return true;
		}

		public static bool NumberIsValid( uint value, string message ) //throws
		{
			if( false == NumberIsValid( value ) )
				throw new ApplicationException( message );			

			return true;
		}

		public static bool NumberIsValid( long value, string message ) //throws
		{
			if( false == NumberIsValid( value ) )
				throw new ApplicationException( message );			

			return true;
		}
		
		public static bool NumberIsValid( short value, string message ) //throws
		{
			if( false == NumberIsValid( value ) )
				throw new ApplicationException( message );			

			return true;
		}

		public static bool NumberIsValid( int value, string message ) //throws
		{
			if( false == NumberIsValid( value ) )
				throw new ApplicationException( message );			

			return true;
		}

		public static bool DateTimeIsValid( DateTime value )
		{
			return value != DateTime.MinValue;
		}

		public static bool NumberIsValid( double value )
		{
			return value != double.MinValue;
		}
		
		public static bool NumberIsValid( decimal value )
		{
			return value != decimal.MinValue;
		}
		
		public static bool NumberIsValid( ulong value )
		{
			return value != ulong.MinValue;
		}
		
		public static bool NumberIsValid( ushort value )
		{
			return value != ushort.MinValue;
		}

		public static bool NumberIsValid( uint value )
		{
			return value != uint.MinValue;
		}

		public static bool NumberIsValid( long value )
		{
			return value != long.MinValue;
		}
		
		public static bool NumberIsValid( short value )
		{
			return value != short.MinValue;
		}

		public static bool NumberIsValid( int value )
		{
			return value != int.MinValue;
		}
		
		public static object ObjectIsNotNull( object obj, string message ) //throws
		{
			if( null == obj )
				throw new ApplicationException( message );

			return obj;
		}
		
		public static bool StringIsNotEmpty( string s, string message ) //throws
		{
			if( false == StringIsNotEmpty( s ) )
				throw new ApplicationException( message );			

			return true;
		}

		public static bool StringIsNotEmpty( string s )
		{
			return s != null && s.Length > 0;
		}
		
		public static bool IsNotNull( object obj )
		{
			return null != obj;
		}

		public static bool IsNotNull( object obj, string message ) //throws
		{
			if( false == IsNotNull(obj) )
				throw new ApplicationException( message );			

			return true;
		}
		
		public static bool ArrayIsNotNull( object array )
		{
			System.Array _array = array as System.Array;
						
			return null != _array && _array.Length > 0;
		}

		public static bool ArrayIsNotNull( object array, string message ) //throws
		{
			if( false == ArrayIsNotNull( array ) )
				throw new ApplicationException( message );
			return true;
		}
	}
}
