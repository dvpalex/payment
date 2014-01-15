using System;

namespace SuperPag.Framework
{
	public class NumericString
	{
		string Value_;

		public NumericString( string value )
		{
			Value_ = value;
		}

		public string Value
		{
			get
			{
				return Value_;
			}
		}

		public override string ToString()
		{
			return Value_;
		}

			
		public static implicit operator int( NumericString w )
		{
			return Int32.Parse( w.Value );
		}

		public static implicit operator NumericString(string value)
		{
			return new NumericString( value );
		}

		public static implicit operator string(NumericString w)
		{
			return w.Value;
		}
	}
}
