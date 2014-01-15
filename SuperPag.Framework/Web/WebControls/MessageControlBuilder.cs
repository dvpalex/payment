using System;
using System.Web.UI;
using System.Resources;
using System.Reflection;
using System.Web.UI.WebControls;
using SuperPag.Framework;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls 
{
	/// <summary>
	/// TODO: Comentário
	/// </summary>
	public class MessageControlBuilder {

		// TODO: Comentário
		internal string GetPropertyValue(object _value, string property) {			
			return ConvertToString(GetObjectProperty(_value, property, false, false), "");
		}

		// TODO: Comentário
		internal string GetPropertyValue(object _value, string property, bool parseEnums, bool returnEnumName) {
			return ConvertToString(GetObjectProperty(_value, property, parseEnums, returnEnumName), "");
		}

		// TODO: Comentário
		internal string GetPropertyValue(object _value, string property, bool parseEnums) {
			return ConvertToString(GetObjectProperty(_value, property, parseEnums, false), "");
		}

		// TODO: Comentário
		internal string GetPropertyValue(object _value, string property, string format, bool parseEnums) {
			return ConvertToString(GetObjectProperty(_value, property, parseEnums, false), format);
		}

		// TODO: Comentário
		internal object GetPropertyValue(object _value, string property, bool parseEnums, Type castValue) {
			return GetObjectProperty(_value, property, parseEnums, false);
		}

		// TODO: Comentário
		internal object GetPropertyValue(object _value, string property, Type castValue) {
			return GetObjectProperty(_value, property, true, false);
		}

		// TODO: Comentário
		internal object GetObjectProperty(object _value, string property, bool parseEnums, bool returnEnumName) {
			if(property == null || _value == null){return string.Empty;}
			
			if(property.IndexOf('.') != -1) {
				string[] _properties = property.Split('.');
				for(int i = 0; i < _properties.Length; i++) {
					string p = _properties[i];
					if(_value != null) {
						PropertyInfo propertyInfo = GetPropertyInfo(_value, p);
						if ( propertyInfo != null )
							_value = ResolveEnum(propertyInfo, _value, parseEnums, false);
						else
							return null;
					} 
					else {
						break;
					}					
				}
				return _value;
			}
			else {	
				PropertyInfo propertyInfo = GetPropertyInfo(_value, property);
				return  ResolveEnum(propertyInfo, _value, parseEnums, returnEnumName);
			}
		}

		//TODO: Comentário
		private object ResolveEnum(PropertyInfo propertyInfo, object _value, bool parse, bool returnEnumName) {
			if(propertyInfo != null) {
				if(propertyInfo.PropertyType.BaseType == typeof(System.Enum)) {
					if(parse && !returnEnumName) {
						Type enumType = 
							propertyInfo.PropertyType;

						Assembly asm = propertyInfo.PropertyType.Assembly;
						
						//Todo: resolver por reflection
						string _namespace = enumType.Namespace;
						
						ResourceManager r = new ResourceManager(_namespace + ".EnumTranslate", asm);
					
						int enumValue = 
							Convert.ToInt32(propertyInfo.GetValue(_value, null));
					
						string enumText =
							EnumTranslateBase.GetTranslateValue(enumType,enumValue, r);

						return enumText;
					} else if(returnEnumName) {
						return Enum.GetName(propertyInfo.PropertyType, propertyInfo.GetValue(_value, null));
					} else {
						return Convert.ToInt32(propertyInfo.GetValue(_value, null));
					}
				}	
				else {
					return propertyInfo.GetValue(_value, null);
				}
			}
			else {
				return "";
			}			
		}

		private PropertyInfo GetPropertyInfo(object _value, string property) {
			PropertyInfo propertyInfo = _value.GetType().GetProperty(property);
			return propertyInfo;
		}

		internal Type GetTypeElementMessage( MessageCollection message ) { 
			Type typeArray = message.GetType();
			if ( typeArray.IsSubclassOf( typeof( MessageCollection ) ) ) {
				return ((CollectionOfAttribute)typeArray.GetCustomAttributes( typeof ( CollectionOfAttribute ), true )[0]).CollectionType ;
			}
			else
				return null;
		}

		// TODO: Comentário
		internal string ConvertToString(object _value) {
			return string.Format("{0}", PreConverterToString(_value));
		}

		// TODO: Comentário
		internal string ConvertToString(object _value, string format) {
			if(format != null && format != string.Empty) { 
				return string.Format(format, PreConverterToString(_value));
			} 
			else {
				return string.Format("{0}", PreConverterToString(_value));
			}
		}

		// TODO: Comentário
		private object PreConverterToString(
			object _value) {
			if(_value is string) {
				return (string)_value;
			} 
			else if(_value is int) {
				if((int)_value != int.MinValue) {
					return Convert.ToString(_value);
				} 
				else {
					return string.Empty;
				}
			}
			else if(_value is Int64) {
				if((Int64)_value != Int64.MinValue) {
					return Convert.ToString(_value);
				} 
				else {
					return string.Empty;
				}
			}
			else if(_value is byte) 
			{
				if((byte)_value != byte.MinValue) 
				{
					return Convert.ToString(_value);
				} 
				else 
				{
					return string.Empty;
				}
			}
			else if(_value is short) 
			{
				if((short)_value != short.MinValue) 
				{
					return Convert.ToString(_value);
				} 
				else 
				{
					return string.Empty;
				}
			}
			else if(_value is DateTime) 
			{
				if((DateTime)_value != DateTime.MinValue) {
					return _value;
				} 
				else {
					return string.Empty;
				}
			} 
			else if(_value == null) {
				return string.Empty;
			}
			else if(_value is decimal) {
				if((decimal)_value != decimal.MinValue) {
					return _value;
				} 
				else {
					return string.Empty;
				}
			}
			else if(_value is System.Enum) {
				return Convert.ToInt32(_value);
			}
			else if (_value is Tristate) {
				if ((Tristate)_value) {
					return "Sim";
				} else {
					return "Não";
				}

				//return Convert.ToBoolean(_value);				
			}
            else if (_value is Guid)
            {
                if ((Guid)_value != Guid.Empty)
                {
                    return _value;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
				throw new Exception("Não suportado");
			}

		}
	}
}
