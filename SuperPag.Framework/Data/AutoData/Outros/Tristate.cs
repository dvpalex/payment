using System;
using System.Xml.Serialization;

namespace SuperPag.Framework.Data.Components {

	[Serializable]
	public class Tristate : IConvertible, IXmlSerializable{
		private bool _value;

		public Tristate() {
		}

		public Tristate(bool value) {
			_value = value;
		}

		public static implicit operator Tristate (bool boolValue) {
			return new Tristate(boolValue);
		}

		public static implicit operator bool (Tristate tristate) {
			if (tristate != null) {
				return tristate._value;
			} else {
				return false;
			}
		}

		public static implicit operator Tristate (int intValue) {
			return new Tristate( intValue != 0 );
		}

		public static implicit operator int (Tristate tristate) {
			return tristate._value ? 1 : 0;
		}

		public static explicit operator Tristate(string strValue) {
			return new Tristate( strValue == "1" || strValue.ToLower() == "true");
		}

		public static explicit operator string (Tristate tristate) {
			if (tristate != null) {
				return tristate._value ? "true" : "false";
			} else {
				return "false";
			}
		}

		public override string ToString() {
			return (string)this;
		}

		public static bool operator == ( Tristate tristateL, Tristate tristateR) {
			if ( Tristate.ReferenceEquals( tristateL , null ) && Tristate.ReferenceEquals( tristateR , null ) )
				return true;
			else if ( Tristate.ReferenceEquals( tristateL , null ) || Tristate.ReferenceEquals( tristateR , null ) )
				return false;
			else
				return tristateL._value == tristateR._value;
		}

		public static bool operator != ( Tristate tristateL, Tristate tristateR) {
			if ( Tristate.ReferenceEquals( tristateL , null ) && Tristate.ReferenceEquals( tristateR , null ) )
				return false;
			else if ( Tristate.ReferenceEquals( tristateL , null ) || Tristate.ReferenceEquals( tristateR , null ) )
				return true;
			else
				return tristateL._value != tristateR._value;
		}

		public override bool Equals(object obj) {
			return base.Equals (obj);
		}

		public override int GetHashCode() {
			return base.GetHashCode ();
		}

		public static bool operator == ( Tristate tristate, bool boolValue) {
			return tristate._value == boolValue;
		}

		public static bool operator != ( Tristate tristate, bool boolValue) {
			return tristate._value != boolValue;
		}

		#region IConvertible Members

		ulong IConvertible.ToUInt64(IFormatProvider provider) {
			return Convert.ToUInt64(_value ? 1 : 0);
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider) {
			return Convert.ToSByte(_value ? 1 : 0);
		}

		double IConvertible.ToDouble(IFormatProvider provider) {
			return Convert.ToDouble(_value ? 1 : 0);
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider) {
			return Convert.ToDateTime(_value ? 1 : 0);
		}

		float IConvertible.ToSingle(IFormatProvider provider) {
			return Convert.ToSingle(_value ? 1 : 0);
		}

		bool IConvertible.ToBoolean(IFormatProvider provider) {
			return _value;
		}

		int IConvertible.ToInt32(IFormatProvider provider) {
			return Convert.ToInt32(_value ? 1 : 0);
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider) {
			return Convert.ToUInt16(_value ? 1 : 0);
		}

		short IConvertible.ToInt16(IFormatProvider provider) {
			return Convert.ToInt16(_value ? 1 : 0);
		}

		string IConvertible.ToString(IFormatProvider provider) {
			return (string)this;
		}

		byte IConvertible.ToByte(IFormatProvider provider) {
			return Convert.ToByte(_value ? 1 : 0);
		}

		char IConvertible.ToChar(IFormatProvider provider) {
			return _value ? '1' : '0';
		}

		long IConvertible.ToInt64(IFormatProvider provider) {
			return Convert.ToInt64(_value ? 1 : 0);
		}

		System.TypeCode IConvertible.GetTypeCode() {
			return TypeCode.Boolean;
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider) {
			return Convert.ToDecimal(_value ? 1 : 0);
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider) {
			return Convert.ChangeType(_value, conversionType);
		}

		uint IConvertible.ToUInt32(IFormatProvider provider) {
			return Convert.ToUInt32(_value ? 1 : 0);
		}

		#endregion

		#region IXmlSerializable Members

		public void WriteXml(System.Xml.XmlWriter writer) {
			writer.WriteString(_value.ToString());
		}

		public System.Xml.Schema.XmlSchema GetSchema() {
			return null;
		}

		public void ReadXml(System.Xml.XmlReader reader) {
			this._value = bool.Parse(reader.ReadString());
		}

		#endregion
	}
}
