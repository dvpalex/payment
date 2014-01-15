using System;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text.RegularExpressions;
using SuperPag.Framework;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls
{
	#region enums
	public enum TipoFormatacao 
	{
		Valor = 1,
		Data = 2,
		Numero = 3,
		Texto = 4,
		Inteiro = 5,
		Horario = 6,
		MesAno = 7
	}

	public enum MsgCompareDataTypesEnum
	{
		Integer,
		Boolean,
		Decimal,
		DateTime,
		Double
	}
	public enum MsgCompareOperatorEnum
	{
		GreaterThanEqual,
		GreaterThan,
		Equal,
		NotEqual,
		LessThan,
		LessThanEqual
	}

	public enum MsgValidationTypeEnum
	{
		None = 1,
		ValueToCompare = 2,
		MinValue = 3
	}
	#endregion

	public class ReturnField 
	{
		protected PropertyInfo _p;
		protected object _Message;
        
		public PropertyInfo Field 
		{
			get 
			{
				return this._p;
			}
			set 
			{
				this._p = value;
			}
		}

		public object Message 
		{
			get 
			{
				return this._Message;
			}
			set 
			{
				this._Message = value;
			}
		}
	}

	public class HelperCompareValues 
	{
		#region comparadores para verificar se o valor do item é igual a da mensagem
		public static bool CompareValues( MsgCompareOperatorEnum compare, int value1, int value2 ) 
		{
			switch( compare ) 
			{
				case( MsgCompareOperatorEnum.Equal ) :
				{
					return ( value1 == value2 );
				}
				case( MsgCompareOperatorEnum.NotEqual ) :
				{
					return ( value1 != value2 );
				}
				case( MsgCompareOperatorEnum.GreaterThan ) :
				{
					return ( value1 > value2 );
				}
				case( MsgCompareOperatorEnum.GreaterThanEqual ) :
				{
					return ( value1 >= value2 );
				}
				case( MsgCompareOperatorEnum.LessThan ) :
				{
					return ( value1 < value2 );
				}
				case( MsgCompareOperatorEnum.LessThanEqual ) :
				{
					return ( value1 <= value2 );
				}
			}
			return false;
		}

		public static bool CompareValues( MsgCompareOperatorEnum compare, DateTime value1, DateTime value2 ) 
		{
			switch( compare ) 
			{
				case( MsgCompareOperatorEnum.Equal ) :
				{
					return ( value1 == value2 );
				}
				case( MsgCompareOperatorEnum.NotEqual ) :
				{
					return ( value1 != value2 );
				}
				case( MsgCompareOperatorEnum.GreaterThan ) :
				{
					return ( value1 > value2 );
				}
				case( MsgCompareOperatorEnum.GreaterThanEqual ) :
				{
					return ( value1 >= value2 );
				}
				case( MsgCompareOperatorEnum.LessThan ) :
				{
					return ( value1 < value2 );
				}
				case( MsgCompareOperatorEnum.LessThanEqual ) :
				{
					return ( value1 <= value2 );
				}
			}
			return false;
		}

		public static bool CompareValues( MsgCompareOperatorEnum compare, double value1, double value2 ) 
		{
			switch( compare ) 
			{
				case( MsgCompareOperatorEnum.Equal ) :
				{
					return ( value1 == value2 );
				}
				case( MsgCompareOperatorEnum.NotEqual ) :
				{
					return ( value1 != value2 );
				}
				case( MsgCompareOperatorEnum.GreaterThan ) :
				{
					return ( value1 > value2 );
				}
				case( MsgCompareOperatorEnum.GreaterThanEqual ) :
				{
					return ( value1 >= value2 );
				}
				case( MsgCompareOperatorEnum.LessThan ) :
				{
					return ( value1 < value2 );
				}
				case( MsgCompareOperatorEnum.LessThanEqual ) :
				{
					return ( value1 <= value2 );
				}
			}
			return false;
		}

		public static bool CompareValues( MsgCompareOperatorEnum compare, decimal value1, decimal value2 ) 
		{
			switch( compare ) 
			{
				case( MsgCompareOperatorEnum.Equal ) :
				{
					return ( value1 == value2 );
				}
				case( MsgCompareOperatorEnum.NotEqual ) :
				{
					return ( value1 != value2 );
				}
				case( MsgCompareOperatorEnum.GreaterThan ) :
				{
					return ( value1 > value2 );
				}
				case( MsgCompareOperatorEnum.GreaterThanEqual ) :
				{
					return ( value1 >= value2 );
				}
				case( MsgCompareOperatorEnum.LessThan ) :
				{
					return ( value1 < value2 );
				}
				case( MsgCompareOperatorEnum.LessThanEqual ) :
				{
					return ( value1 <= value2 );
				}
			}
			return false;
		}

		public static bool CompareValues( MsgCompareOperatorEnum compare, bool value1, bool value2 ) 
		{
			switch( compare ) 
			{
				case( MsgCompareOperatorEnum.Equal ) :
				{
					return ( value1 == value2 );
				}
				case( MsgCompareOperatorEnum.NotEqual ) :
				{
					return ( value1 != value2 );
				}
			}
			return false;
		}

		#endregion

		#region Verifica validação de Enabled, Visible

		public static Tristate CompareTwoValues( MsgCompareDataTypesEnum dataType, MsgCompareOperatorEnum compareOperator, string valueObject, string valueToCompare ) 
		{
			return CompareTwoValues( dataType, compareOperator, (object)valueObject, (object)valueToCompare );
		}
		public static Tristate CompareTwoValues( MsgCompareDataTypesEnum dataType, MsgCompareOperatorEnum compareOperator, decimal valueObject, decimal valueToCompare ) 
		{
			return CompareTwoValues( dataType, compareOperator, (object)valueObject, (object)valueToCompare );
		}
		public static Tristate CompareTwoValues( MsgCompareDataTypesEnum dataType, MsgCompareOperatorEnum compareOperator, int valueObject, int valueToCompare ) 
		{
			return CompareTwoValues( dataType, compareOperator, (object)valueObject, (object)valueToCompare );
		}
		public static Tristate CompareTwoValues( MsgCompareDataTypesEnum dataType, MsgCompareOperatorEnum compareOperator, DateTime valueObject, DateTime valueToCompare ) 
		{
			return CompareTwoValues( dataType, compareOperator, (object)valueObject, (object)valueToCompare );
		}
		public static Tristate CompareTwoValues( MsgCompareDataTypesEnum dataType, MsgCompareOperatorEnum compareOperator, object valueObject, object valueToCompare ) 
		{
			if ( valueObject != null && dataType == MsgCompareDataTypesEnum.Integer ) 
			{
				if(valueObject is Int32)
				{
					int _valueProp = Convert.ToInt32( valueObject );
					int _valueToCompare =  Convert.ToInt32( valueToCompare );
					return CompareValues( compareOperator, _valueProp, _valueToCompare );
				} 
			}
			else if ( valueObject != null && dataType == MsgCompareDataTypesEnum.Decimal ) 
			{
				if(valueObject is decimal)
				{
					decimal _valueProp = Convert.ToDecimal( valueObject );
					decimal _valueToCompare =  Convert.ToDecimal( valueToCompare );
					return CompareValues( compareOperator, _valueProp, _valueToCompare );
				} 
			}
			else if ( valueObject != null && dataType == MsgCompareDataTypesEnum.DateTime ) 
			{
				if(valueObject is DateTime)
				{
					DateTime _valueProp = Convert.ToDateTime( valueObject );
					DateTime _valueToCompare = Convert.ToDateTime( valueToCompare );
					return CompareValues( compareOperator, _valueProp, _valueToCompare );
				} 
			}
			else if ( valueObject != null && dataType == MsgCompareDataTypesEnum.Double ) 
			{
				if(valueObject is double)
				{
					double _valueProp = Convert.ToDouble( valueObject );
					double _valueToCompare = Convert.ToDouble( valueToCompare );
					return CompareValues( compareOperator, _valueProp, _valueToCompare );
				} 
			}
			else if ( valueObject != null && dataType == MsgCompareDataTypesEnum.Boolean ) 
			{
				if(valueObject is Tristate || valueObject is bool)
				{
					bool _valueProp = Convert.ToBoolean( valueObject );
					bool _valueToCompare = Convert.ToBoolean( valueToCompare );
					return CompareValues( compareOperator, _valueProp, _valueToCompare );
				} 
			}

			return null;
		}

		public static Tristate CompareToMinValue( MsgCompareDataTypesEnum dataType, MsgCompareOperatorEnum compareOperator, object valueObject) 
		{
			if ( valueObject != null && dataType == MsgCompareDataTypesEnum.Integer ) 
			{
				if(valueObject is Int32)
				{
					int valueProp = (int)valueObject;
					if ( compareOperator == MsgCompareOperatorEnum.Equal )
						return valueProp == int.MinValue;
					else
						return valueProp != int.MinValue;

				}
			}
			else if ( valueObject != null && dataType == MsgCompareDataTypesEnum.Decimal ) 
			{
				if(valueObject is decimal)
				{
					decimal valueProp = (decimal)valueObject;
					if ( compareOperator == MsgCompareOperatorEnum.Equal )
						return valueProp == decimal.MinValue;
					else
						return valueProp != decimal.MinValue;
				}
			}
			else if ( valueObject != null && dataType == MsgCompareDataTypesEnum.DateTime ) 
			{
				if(valueObject is DateTime)
				{
					DateTime valueProp = (DateTime)valueObject;
					if ( compareOperator == MsgCompareOperatorEnum.Equal )
						return valueProp == DateTime.MinValue;
					else
						return valueProp != DateTime.MinValue;
				}
			}
			else if ( valueObject != null && dataType == MsgCompareDataTypesEnum.Double ) 
			{
				if(valueObject is Int32)
				{
					double valueProp = (double)valueObject;
					if ( compareOperator == MsgCompareOperatorEnum.Equal )
						return valueProp == double.MinValue;
					else
						return valueProp != double.MinValue;
				}
			}
			else if ( valueObject != null && dataType == MsgCompareDataTypesEnum.Boolean ) 
			{
				if(valueObject is Int32)
				{
					bool valueProp = Convert.ToBoolean( valueObject );
					if ( compareOperator == MsgCompareOperatorEnum.Equal )
						return valueProp == false;
					else
						return valueProp != false;
				}
			}

			return null;
		}

		#endregion

	}

	public class Helper
	{
		public Helper(){}

		#region funções apra UnBind de Mensagens
		public static void SetUndBindPropetyValue( object message, string propertyField, Message valueProperty ) 
		{
			ReturnField returnField = ReturnPropertyField( message,  propertyField );
			if (returnField != null)
			{
				PropertyInfo p = returnField.Field;
				object _seekMessage = returnField.Message;

				p.SetValue(_seekMessage, valueProperty, null);
			}
		}

		public static void SetUndBindPropetyValue( object message, MsgTextBox control) 
		{
			if ( control.InputFilter != MsgCustomValidators.FilterEnum.None ) 
			{
				string matchString = "";

				RegexOptions options = new RegexOptions();
				options |= RegexOptions.Singleline;

				switch( control.InputFilter ) 
				{
					case MsgCustomValidators.FilterEnum.CPF : 
					case MsgCustomValidators.FilterEnum.CNPJ : 
					{
						if (Regex.IsMatch(control.Text, "[0-9]", options)) 
						{
							Match mc = Regex.Match(control.Text, "[0-9]", options);
							while ( mc.Success ) 
							{				
								matchString += mc.Value;
								mc = mc.NextMatch();
							}
						}
						break;
					}
					case MsgCustomValidators.FilterEnum.NumDecimal_2casa : 
					case MsgCustomValidators.FilterEnum.NumDecimal_3casa : 
					case MsgCustomValidators.FilterEnum.NumDecimal_4casa : 
					case MsgCustomValidators.FilterEnum.NumDecimal_5casa : 
					case MsgCustomValidators.FilterEnum.NumDecimal_6casa : 
					case MsgCustomValidators.FilterEnum.NumDecimal_7casa : 
					case MsgCustomValidators.FilterEnum.NumDecimal_8casa : 
					case MsgCustomValidators.FilterEnum.Valor:
					case MsgCustomValidators.FilterEnum.NumInteiro: 
					{
						if (Regex.IsMatch(control.Text, "[0-9,]", options)) 
						{
							Match mc = Regex.Match(control.Text, "[0-9,]", options);
							while ( mc.Success ) 
							{				
								matchString += mc.Value;
								mc = mc.NextMatch();
							}
						}
						break;
					}
					case MsgCustomValidators.FilterEnum.Texto : 
					{
						matchString = control.Text;
						break;
					}
					case MsgCustomValidators.FilterEnum.Data : 
					{
						if (Regex.IsMatch(control.Text, "^(\\d{1,2})(/)(\\d{1,2})(/)(\\d{4})", options)) 
						{
							Match mc = Regex.Match(control.Text, "^(\\d{1,2})(/)(\\d{1,2})(/)(\\d{4})", options);
							while ( mc.Success ) 
							{				
								matchString += mc.Value;
								mc = mc.NextMatch();
							}
						}
						break;
					}
					case MsgCustomValidators.FilterEnum.DataHora : 
					{
						if (Regex.IsMatch(control.Text, "^(\\d{1,2})(/)(\\d{1,2})(/)(\\d{4})(\\ )(\\d{1,2})(:)(\\d{1,2})((\\:)(\\d{1,2}))?", options)) 
						{
							Match mc = Regex.Match(control.Text, "^(\\d{1,2})(/)(\\d{1,2})(/)(\\d{4})(\\ )(\\d{1,2})(:)(\\d{1,2})((\\:)(\\d{1,2}))?", options);
							while ( mc.Success ) 
							{				
								matchString += mc.Value;
								mc = mc.NextMatch();
							}
						}
						break;
					}
					case MsgCustomValidators.FilterEnum.HorarioHM : 
					{
						if (Regex.IsMatch(control.Text, "^(\\d{1,2})(:)(\\d{1,2})", options)) 
						{
							Match mc = Regex.Match(control.Text, "^(\\d{1,2})(:)(\\d{1,2})", options);
							while ( mc.Success ) 
							{				
								matchString += mc.Value;
								mc = mc.NextMatch();
							}

							matchString  = "1900-01-01 " + matchString;
						}
						break;
					}
					case MsgCustomValidators.FilterEnum.HorarioHMS : 
					{
						if (Regex.IsMatch(control.Text, "^(\\d{1,2})(:)(\\d{1,2})((\\:)(\\d{1,2}))?", options)) 
						{
							Match mc = Regex.Match(control.Text, "^(\\d{1,2})(:)(\\d{1,2})((\\:)(\\d{1,2}))?", options);
							while ( mc.Success ) 
							{				
								matchString += mc.Value;
								mc = mc.NextMatch();
							}
						}
						break;
					}
					case MsgCustomValidators.FilterEnum.MesAno : 
					{
						if (Regex.IsMatch(control.Text, "^(\\d{1,2})(/)(\\d{1,4})", options)) 
						{
							Match mc = Regex.Match(control.Text, "^(\\d{1,2})(/)(\\d{1,4})", options);
							matchString = mc.ToString();
						}
						break;
					}
					default :
					{
						matchString = control.Text;
						break;
					}
				}

				SetUndBindPropetyValue( message, control.MsgTextField, matchString );
			} 
			else 
			{
				SetUndBindPropetyValue( message, control.MsgTextField, control.Text );
			}
		}

		public static void SetUndBindPropetyValue( object message, string propertyField, string valueProperty ) 
		{
			ReturnField returnField = ReturnPropertyField( message,  propertyField );
			if (returnField != null)
			{
				PropertyInfo p = returnField.Field;
				object _seekMessage = returnField.Message;

				//TODO: Verificar todos os tipos
				if(p.PropertyType == typeof(System.Int32))
				{	
					if(valueProperty == string.Empty)
					{
						p.SetValue(_seekMessage, Int32.MinValue, null);
					} 
					else
					{
						p.SetValue(_seekMessage, Convert.ToInt32(valueProperty), null);
					}
				}
				else if(p.PropertyType == typeof(System.Byte))
				{	
					if(valueProperty == string.Empty)
					{
						p.SetValue(_seekMessage, byte.MinValue, null);
					} 
					else
					{
						p.SetValue(_seekMessage, Convert.ToByte(valueProperty), null);
					}
				}
				else if(p.PropertyType == typeof(System.Int16))
				{	
					if(valueProperty == string.Empty)
					{
						p.SetValue(_seekMessage, Int16.MinValue, null);
					} 
					else
					{
						p.SetValue(_seekMessage, Convert.ToInt16(valueProperty), null);
					}
				}
				else if(p.PropertyType == typeof(System.Int64))
				{	
					if(valueProperty == string.Empty)
					{
						p.SetValue(_seekMessage, Int64.MinValue, null);
					} 
					else
					{
						p.SetValue(_seekMessage, Convert.ToInt64(valueProperty), null);
					}
				}
				else if(p.PropertyType.BaseType == typeof(System.Enum))
				{
					if(valueProperty == string.Empty)
					{
						p.SetValue(_seekMessage, null, null);
					} 
					else 
					{
						p.SetValue(_seekMessage,
							Enum.Parse(p.PropertyType, valueProperty, true), null);
					}
				}
				else if(p.PropertyType == typeof(System.Decimal))
				{
					if(valueProperty == string.Empty)
					{							
						p.SetValue(_seekMessage, Decimal.MinValue, null);
					}
					else
					{
						p.SetValue(_seekMessage, Convert.ToDecimal(valueProperty), null);
					}
				}
				else if(p.PropertyType == typeof(System.DateTime))
				{
					if(valueProperty == string.Empty)
					{							
						p.SetValue(_seekMessage, DateTime.MinValue, null);
					}
					else
					{						
						p.SetValue(_seekMessage, Convert.ToDateTime(valueProperty), null);
					}
				}
				else if (p.PropertyType == typeof(SuperPag.Framework.Helper.Tristate)) 
				{
					if(valueProperty == string.Empty)
					{							
						p.SetValue(_seekMessage, null, null);
					} 
					else 
					{
						p.SetValue(_seekMessage, (SuperPag.Framework.Helper.Tristate)valueProperty, null);
					}
				} 
				else if (p.PropertyType == typeof(bool))
				{
					if(valueProperty == string.Empty)
					{							
						p.SetValue(_seekMessage, null, null);
					} 
					else 
					{ 
						p.SetValue(_seekMessage, Convert.ToBoolean( valueProperty ), null);
					}
				}
				else if (p.PropertyType == typeof(string))
				{	
					if(valueProperty == string.Empty)
					{
						p.SetValue(_seekMessage, null, null);
					} 
					else
					{
						p.SetValue(_seekMessage, valueProperty, null);
					}
				}
                else if (p.PropertyType == typeof(Guid))
                {
                    if (valueProperty == string.Empty)
                    {
                        p.SetValue(_seekMessage, null, null);
                    }
                    else
                    {
                        p.SetValue(_seekMessage, valueProperty, null);
                    }
                }

				else
					p.SetValue(_seekMessage, valueProperty, null);
			}
		}

		#endregion

		#region IsPropertyMinValue
		public static bool IsPropertyMinValue( object message, string propertyField ) 
		{
			ReturnField returnField = ReturnPropertyField( message, propertyField);

			if (returnField != null) 
			{
				PropertyInfo p = returnField.Field;
				
				object _value = p.GetValue( message, null );
				//TODO: Verificar todos os tipos
				if(p.PropertyType == typeof(System.Int32))
				{	
					return (int)_value == Int32.MinValue;
				}
				else if(p.PropertyType == typeof(System.Int64))
				{	
					return (System.Int64)_value == Int64.MinValue;
				}
				else if(p.PropertyType.BaseType == typeof(System.Enum))
				{
					return (int)_value == int.MinValue;
				}
				else if(p.PropertyType == typeof(System.Decimal))
				{
					return (System.Decimal)_value == Decimal.MinValue;
				}
				else if(p.PropertyType == typeof(System.DateTime))
				{
					return (System.DateTime)_value == DateTime.MinValue;
				}
				else if (p.PropertyType == typeof(SuperPag.Framework.Helper.Tristate)) 
				{
					return (SuperPag.Framework.Helper.Tristate)_value == null;
				} 
				else if (p.PropertyType == typeof(bool))
				{
					return (bool)_value == false;
				}
			}
			return false;
		}
		#endregion

		#region Retorna uma propriedade da mensagem
		public static ReturnField ReturnPropertyField( object message, string propertyField ) 
		{
			object _seekMessage = message;

			if(message == null) return null;
			if(propertyField == null) return null;

			string[] _properties = propertyField.Split('.');
			for(int i = 0; i < _properties.Length; i++)
			{
				string property = _properties[i];
				if(_seekMessage != null)
				{
					PropertyInfo p = GetPropertyInfo(_seekMessage, property);
					if(p != null)
					{
						if (i < (_properties.Length-1))
							_seekMessage = p.GetValue(_seekMessage, BindingFlags.Instance | BindingFlags.Public, null, null, null);
						else if (p != null)
						{
							ReturnField ret = new ReturnField();
							ret.Field = p;
							ret.Message = _seekMessage;
							return ret;
						}
					}					
				} 
				else 
					break;
			}

			return null;		
		}
		#endregion

		#region Retorna uma propriedade da mensagem
		private static PropertyInfo GetPropertyInfo(object _value, string property)
		{
			PropertyInfo propertyInfo = _value.GetType().GetProperty(property);
			return propertyInfo;
		}
		#endregion
	}

	
	public class CustomFormatHelper 
	{
		#region enums
		
		//Input Validation
		public enum CustomFormatEnum
		{
			None = 0,
			CPF = 1,
			CNPJ = 2,
			Texto = 3,
			Valor = 4,
			Data = 5,
			DataHora = 6,
			DataDMY = 7,
			DataMDY = 8,
			DataYMD = 9,
			NumInteiro = 10,
			NumDecimal_1casa = 28,
			NumDecimal_2casa = 11,
			NumDecimal_3casa = 12,
			NumDecimal_4casa = 13,
			NumDecimal_5casa = 14,
			NumDecimal_6casa = 15,
			NumDecimal_7casa = 16,
			NumDecimal_8casa = 17,
			PercDecimal_1casa = 29,
			PercDecimal_2casa = 18,
			PercDecimal_3casa = 19,
			PercDecimal_4casa = 20,
			PercDecimal_5casa = 21,
			PercDecimal_6casa = 22,
			PercDecimal_7casa = 23,
			PercDecimal_8casa = 24,
			HorarioHM = 25,
			HorarioHMS = 26,
			MesAno = 27
		}

		#endregion

		private static CustomFormatEnum _currentFormat = (CustomFormatEnum)int.MinValue;
		
		private static System.Globalization.CultureInfo _curCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;

		private static bool _ShowCurrencySymbol = false;

		public CustomFormatHelper( CustomFormatEnum format ) 
		{
			_currentFormat = format;
			_ShowCurrencySymbol = false;
		}

		public CustomFormatHelper( CustomFormatEnum format, bool ShowCurrencySymbol ) 
		{
			_currentFormat = format;
			_ShowCurrencySymbol = ShowCurrencySymbol;
		}

		#region Retorna o formato da string
		public string GetCurrentFormatString( string Value ) 
		{
			TipoFormatacao tipoParaFormato = (TipoFormatacao)int.MinValue;
			
			int digits  = 0;

			switch(_currentFormat)
			{
					//verificação de valores
				case CustomFormatEnum.Valor:
					digits = _curCultureInfo.NumberFormat.CurrencyDecimalDigits;
					tipoParaFormato = TipoFormatacao.Valor;
					break;
				case CustomFormatEnum.NumDecimal_1casa:
					digits = 1;
					tipoParaFormato = TipoFormatacao.Valor;
					break;
				case CustomFormatEnum.NumDecimal_2casa:
					digits = 2;
					tipoParaFormato = TipoFormatacao.Valor;
					break;
				case CustomFormatEnum.PercDecimal_1casa:
					digits = 1;
					tipoParaFormato = TipoFormatacao.Numero;
					break;
				case CustomFormatEnum.PercDecimal_2casa:
					digits = 2;
					tipoParaFormato = TipoFormatacao.Numero;
					break;
				case CustomFormatEnum.NumDecimal_3casa:
					digits = 3;
					tipoParaFormato = TipoFormatacao.Valor;
					break;
				case CustomFormatEnum.PercDecimal_3casa:
					digits = 3;
					tipoParaFormato = TipoFormatacao.Numero;
					break;
				case CustomFormatEnum.NumDecimal_4casa:
					digits = 4;
					tipoParaFormato = TipoFormatacao.Valor;
					break;
				case CustomFormatEnum.PercDecimal_4casa:
					digits = 4;
					tipoParaFormato = TipoFormatacao.Numero;
					break;
				case CustomFormatEnum.NumDecimal_5casa:
					digits = 5;
					tipoParaFormato = TipoFormatacao.Valor;
					break;
				case CustomFormatEnum.PercDecimal_5casa:
					digits = 5;
					tipoParaFormato = TipoFormatacao.Numero;
					break;
				case CustomFormatEnum.NumDecimal_6casa:
					digits = 6;
					tipoParaFormato = TipoFormatacao.Valor;
					break;
				case CustomFormatEnum.PercDecimal_6casa:
					digits = 6;
					tipoParaFormato = TipoFormatacao.Numero;
					break;
				case CustomFormatEnum.NumDecimal_7casa:
					digits = 7;
					tipoParaFormato = TipoFormatacao.Valor;
					break;
				case CustomFormatEnum.PercDecimal_7casa:
					digits = 7;
					tipoParaFormato = TipoFormatacao.Numero;
					break;
				case CustomFormatEnum.NumDecimal_8casa:
					digits = 8;
					tipoParaFormato = TipoFormatacao.Valor;
					break;
				case CustomFormatEnum.PercDecimal_8casa:
					digits = 8;
					tipoParaFormato = TipoFormatacao.Numero;
					break;				
				
				case CustomFormatEnum.Data:					
				case CustomFormatEnum.DataHora:
				case CustomFormatEnum.DataDMY:
				case CustomFormatEnum.DataMDY:
				case CustomFormatEnum.DataYMD:
					tipoParaFormato = TipoFormatacao.Data;
					break;

					//Verificação de textos
				case CustomFormatEnum.CPF:
					tipoParaFormato = TipoFormatacao.Texto;
					break;
				case CustomFormatEnum.CNPJ:
					tipoParaFormato = TipoFormatacao.Texto;
					break;
				case CustomFormatEnum.HorarioHM:
				case CustomFormatEnum.HorarioHMS:
					tipoParaFormato = TipoFormatacao.Horario;
					break;
				case CustomFormatEnum.MesAno:
					tipoParaFormato = TipoFormatacao.MesAno;
					break;
				default:
					tipoParaFormato = TipoFormatacao.Texto;
					break;

			}

			if ( (int)tipoParaFormato == int.MinValue )
				return Value;
			else 
			{
				switch( tipoParaFormato ) 
				{
					case TipoFormatacao.Data:
					{
						string formatData = "dd/MM/yyyy";

						switch( _currentFormat ) 
						{
							case CustomFormatEnum.Data:
								formatData = _curCultureInfo.DateTimeFormat.ShortDatePattern;
								return FormatData( formatData, Value );
							case CustomFormatEnum.DataHora:
								formatData = _curCultureInfo.DateTimeFormat.ShortDatePattern + " " + _curCultureInfo.DateTimeFormat.ShortTimePattern;
								return FormatData( formatData, Value );
							case CustomFormatEnum.DataDMY:
								return FormatData( formatData, Value );
							case CustomFormatEnum.DataMDY:
								formatData = "MM/dd/yyyy";
								return FormatData( formatData, Value );
							case CustomFormatEnum.DataYMD:
								formatData = "yyyy/MM/dd";
								return FormatData( formatData, Value );
						}
						break;
					}
					case TipoFormatacao.Numero:
						return FormatNumber( Value, digits );
					case TipoFormatacao.Valor:
						return FormatCurrency( Value, digits );
					case TipoFormatacao.Texto:
					{
						switch( _currentFormat ) 
						{
							case CustomFormatEnum.CPF: //tamanho de caracteres 11
								return FormatCPF ( Value );
							case CustomFormatEnum.CNPJ: //tamanho de caracteres 14
								return FormatCNPJ ( Value );
							default:
								return Value;
						}
					}
					case TipoFormatacao.Horario:
					{
						switch( _currentFormat ) 
						{
							case CustomFormatEnum.HorarioHM: 
								return FormatHoraHM ( Value );
							case CustomFormatEnum.HorarioHMS:
								return FormatHora ( Value );
							default:
								return Value;
						}
					}
					case TipoFormatacao.MesAno:
					{
						return FormatMesAno( Value );
					}
				}
			}

			return null;
		}
		#endregion

		#region Verifica quantos digitos decimais tem o formato
		public static int CheckDigits( CustomFormatEnum _format ) 
		{
			switch(_format)
			{
					//verificação de valores
				case CustomFormatEnum.Valor:
					return _curCultureInfo.NumberFormat.CurrencyDecimalDigits;
				case CustomFormatEnum.NumDecimal_1casa:
				case CustomFormatEnum.PercDecimal_1casa:
					return 1;
				case CustomFormatEnum.NumDecimal_2casa:
				case CustomFormatEnum.PercDecimal_2casa:
					return 2;
				case CustomFormatEnum.NumDecimal_3casa:
				case CustomFormatEnum.PercDecimal_3casa:
					return 3;
				case CustomFormatEnum.NumDecimal_4casa:
				case CustomFormatEnum.PercDecimal_4casa:
					return 4;
				case CustomFormatEnum.NumDecimal_5casa:
				case CustomFormatEnum.PercDecimal_5casa:
					return 5;
				case CustomFormatEnum.NumDecimal_6casa:
				case CustomFormatEnum.PercDecimal_6casa:
					return 6;
				case CustomFormatEnum.NumDecimal_7casa:
				case CustomFormatEnum.PercDecimal_7casa:
					return 7;
				case CustomFormatEnum.NumDecimal_8casa:
				case CustomFormatEnum.PercDecimal_8casa:
					return 8;
			}
			return 0;
		}
		#endregion

		#region funcoes auxiliares
		public static string FormatCPF( string CPF ) 
		{
			string cpfValue = CPF;
			if ( cpfValue.Length < 11 ) cpfValue = cpfValue.PadLeft( 11 - cpfValue.Length, '0');
			else if ( cpfValue.Length > 11 ) cpfValue = cpfValue.Substring(0, 11);

			return string.Format("{0:000'.'000'.'000'-'00}", Convert.ToDecimal( cpfValue ));
		}

		public static string FormatCNPJ( string CNPJ ) 
		{
			string cnpjValue = CNPJ;
			if ( cnpjValue.Length < 14 ) cnpjValue = cnpjValue.PadLeft( 14 - cnpjValue.Length, '0');
			else if ( cnpjValue.Length > 14 ) cnpjValue = cnpjValue.Substring(0, 14);

			return string.Format("{0:00'.'000'.'000'/'0000'-'00}", Convert.ToDecimal( cnpjValue ));
		}

		public static string FormatNumber( string valor ) 
		{
			return string.Format("{0:#,##0.00}", Convert.ToDecimal( valor ));
		}

		public static string FormatNumber( string valor, int digits ) 
		{
			string sformat = "{0:#,##0." + (new string('0', digits)) + "}";
			return string.Format(sformat, Convert.ToDecimal( valor ));
		}

		public string FormatCurrency( string valor, int digits ) 
		{
			string sformat = "{0:" + 
				(_ShowCurrencySymbol == true ? _curCultureInfo.NumberFormat.CurrencySymbol + " " : "" )
				+ "###,##0." + (new string('0', digits)) + "}";
			return string.Format(sformat, Convert.ToDecimal( valor ));
		}

		public static string FormatCurrency( string valor, int digits, bool showCurrencySymbol ) 
		{
			string sformat = "{0:" + 
				(showCurrencySymbol == true ? _curCultureInfo.NumberFormat.CurrencySymbol + " " : "" )
				+ "###,##0." + (new string('0', digits)) + "}";
			return string.Format(sformat, Convert.ToDecimal( valor ));
		}

		public static string FormatData( string valor ) 
		{
			string Formato = _curCultureInfo.DateTimeFormat.ShortDatePattern;
			return string.Format("{0:" + Formato + "}", Convert.ToDateTime( valor ));
		}

		public static string FormatDataHora( string valor ) 
		{
			string Formato = _curCultureInfo.DateTimeFormat.ShortDatePattern + " " +
				_curCultureInfo.DateTimeFormat.ShortTimePattern;
			return string.Format("{0:" + Formato + "}", Convert.ToDateTime( valor ));
		}
		
		public static string FormatData( string Formato, string valor ) 
		{
			return string.Format("{0:" + Formato + "}", Convert.ToDateTime( valor ));
		}

		public static string FormatHora( string valor ) 
		{
			string Formato = _curCultureInfo.DateTimeFormat.ShortTimePattern;
			return string.Format("{0:" + Formato + "}", Convert.ToDateTime( valor ));
		}

		public static string FormatHoraHM( string valor ) 
		{
			string Formato = "HH:mm";
			return string.Format("{0:" + Formato + "}", Convert.ToDateTime( valor ));
		}

		public static string FormatMesAno( string valor )
		{			
			return string.Format("{0:MM/yyyy}", Convert.ToDateTime( valor ) );
		}
		#endregion
	}

	
	public class MsgCustomValidators 
	{
		#region Enum
		//Input Validation
		public enum FilterEnum
		{
			None = 0,
			CPF = 1,
			CNPJ = 2,
			Texto = 3,
			Valor = 4,
			Data = 5,
			DataHora = 6,
			DataDMY = 7,
			DataMDY = 8,
			DataYMD = 9,
			NumInteiro = 10,
			NumDecimal_1casa = 28,
			NumDecimal_2casa = 11,
			NumDecimal_3casa = 12,
			NumDecimal_4casa = 13,
			NumDecimal_5casa = 14,
			NumDecimal_6casa = 15,
			NumDecimal_7casa = 16,
			NumDecimal_8casa = 17,
			PercDecimal_1casa = 29,
			PercDecimal_2casa = 18,
			PercDecimal_3casa = 19,
			PercDecimal_4casa = 20,
			PercDecimal_5casa = 21,
			PercDecimal_6casa = 22,
			PercDecimal_7casa = 23,
			PercDecimal_8casa = 24,
			HorarioHM = 25,
			HorarioHMS = 26,
			MesAno = 27,
			Email = 30,
			CEP = 31
		}

		#endregion

		private static FilterEnum _currentFilter = (FilterEnum)int.MinValue;
		
		private static System.Globalization.CultureInfo _curCultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;

		public MsgCustomValidators( FilterEnum filterType ) 
		{
			_currentFilter = filterType;
		}

		public BaseValidator CreateCustomValidator( WebControl controlRender )
		{
			string RegExpValidation = "";
			string MsgValidation = "";
			string decimalchar = _curCultureInfo.NumberFormat.CurrencyDecimalSeparator;
			string groupchar = _curCultureInfo.NumberFormat.CurrencyGroupSeparator;

			TipoFormatacao tipoFormato = CheckType( _currentFilter );
			int digits = CheckDigits( _currentFilter );

			if ( (int)tipoFormato == int.MinValue )
				return null;
			else 
			{
				switch( tipoFormato ) 
				{
					case TipoFormatacao.Data:
					{
						string formatData = _curCultureInfo.DateTimeFormat.ShortDatePattern;

						switch( _currentFilter ) 
						{
							case FilterEnum.Data:
								//separa os digitos
								RegExpValidation = GetRegExpForCultureDateTime( _curCultureInfo, _currentFilter );
								MsgValidation = "&nbsp;Data inválida (" + _curCultureInfo.DateTimeFormat.ShortDatePattern + ")";
								break;
							case FilterEnum.DataHora:
								RegExpValidation = GetRegExpForCultureDateTime( _curCultureInfo, _currentFilter );
								MsgValidation = "&nbsp;Data inválida (" + _curCultureInfo.DateTimeFormat.ShortDatePattern + " " + _curCultureInfo.DateTimeFormat.ShortTimePattern + ")";
								break;
							case FilterEnum.DataDMY:
								RegExpValidation = "^\\s*(\\d{1,2})([-./])(\\d{1,2})([-./])(\\d{4})\\s*$";
								MsgValidation = "&nbsp;Data inválida (dd/MM/aaaa)";
								break;
							case FilterEnum.DataMDY:
								RegExpValidation = "^\\s*(\\d{1,2})([-./])(\\d{1,2})([-./])(\\d{4})\\s*$";
								MsgValidation = "&nbsp;Data inválida (MM/dd/aaaa)";
								break;
							case FilterEnum.DataYMD:
								RegExpValidation = "^\\s*(\\d{4})([-./])(\\d{1,2})([-./])(\\d{1,2})\\s*$";
								MsgValidation = "&nbsp;Data inválida (aaaa/MM/dd)";
								break;
						}
						break;
					}
					case TipoFormatacao.Horario:
					{
						switch( _currentFilter ) 
						{
							case FilterEnum.HorarioHM:
								//separa os digitos
								RegExpValidation = "^\\s*(\\d{1,2})(:)(\\d{1,2})((\\:)(\\d{1,2}))?\\s*$";
								MsgValidation = "&nbsp;Horário inválido (hh:mm)";
								break;
							case FilterEnum.HorarioHMS:
								RegExpValidation = "^\\s*(\\d{1,2})(:)(\\d{1,2})(:)(\\d{1,2})\\s*$";
								MsgValidation = "&nbsp;Horário inválido (" + _curCultureInfo.DateTimeFormat.LongTimePattern + ")";
								break;
						}
						break;
					}
					case TipoFormatacao.Numero:
						RegExpValidation = "^\\s*([-\\+])?(((\\d+)(\\" + groupchar + "?))*)(\\d+)"
							+ ((digits > 0) ? "(\\" + decimalchar + "(\\d{1," + digits.ToString() + "}))?" : "")
							+ "\\s*$";
						MsgValidation = "&nbsp;Digite um número válido";
						break;
					case TipoFormatacao.Valor:
						RegExpValidation = "^\\s*([-\\+])?(((\\d+)\\" + groupchar + ")*)(\\d+)"
							+ ((digits > 0) ? "(\\" + decimalchar + "(\\d{1," + digits.ToString() + "}))?" : "")
							+ "\\s*$";
						MsgValidation = "&nbsp;Digite um valor válido";
						break;
					case TipoFormatacao.MesAno: //tamanho de caracteres 7
						RegExpValidation = "^(\\d{2})(/)(\\d{4})";
						MsgValidation = "&nbsp;Digite um Mês/Ano válido";
						break;								
					case TipoFormatacao.Texto:
					{
						switch( _currentFilter ) 
						{
							case FilterEnum.CPF: //tamanho de caracteres 11
								RegExpValidation = "^(\\d{3})(.)(\\d{3})(.)(\\d{3})(-)(\\d{2})";
								MsgValidation = "&nbsp;Digite um CPF válido";
								break;
							case FilterEnum.CNPJ: //tamanho de caracteres 14
								RegExpValidation = "^(\\d{2})(.)(\\d{3})(.)(\\d{3})(/)(\\d{4})(-)(\\d{2})";
								MsgValidation = "&nbsp;Digite um CNPJ válido";
								break;
							case FilterEnum.Email: 
								RegExpValidation = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
								MsgValidation = "&nbsp;Digite um e-mail válido";
								break;
							case FilterEnum.CEP: 
								RegExpValidation = "^(\\d{5})(-)(\\d{3})";
								MsgValidation = "&nbsp;Digite um CEP válido";
								break;
							default:
								RegExpValidation = "";
								MsgValidation = "";
								break;
						}
						break;
					}
					case TipoFormatacao.Inteiro:
						break;
				}

				if ( tipoFormato != TipoFormatacao.Inteiro ) 
				{
					System.Web.UI.WebControls.RegularExpressionValidator _regexpInputFilter = 
						new RegularExpressionValidator();
					_regexpInputFilter.ControlToValidate = controlRender.ID;
					_regexpInputFilter.EnableClientScript = true;
					_regexpInputFilter.ErrorMessage = MsgValidation;
					_regexpInputFilter.ValidationExpression = RegExpValidation;
					_regexpInputFilter.Display = ValidatorDisplay.Dynamic;
					return _regexpInputFilter;
				} 
				else 
				{
					System.Web.UI.WebControls.CompareValidator _compare = new CompareValidator();
					_compare.ControlToValidate = controlRender.ID;
					_compare.EnableClientScript = true;
					_compare.ErrorMessage = "&nbsp;Valor inválido&nbsp;";
					_compare.Operator = ValidationCompareOperator.DataTypeCheck;
					_compare.Type = ValidationDataType.Integer;
					_compare.Display = ValidatorDisplay.Dynamic;
					return _compare;
				}
			}

		}

		#region Verifica numero de casas decimais para um tipo de filtro
		public static int CheckDigits( FilterEnum filter ) 
		{
			switch(filter)
			{
				//verificação de valores
				case FilterEnum.Valor:
					return _curCultureInfo.NumberFormat.CurrencyDecimalDigits;
				case FilterEnum.NumDecimal_1casa:
				case FilterEnum.PercDecimal_1casa:
					return 1;
				case FilterEnum.NumDecimal_2casa:
				case FilterEnum.PercDecimal_2casa:
					return 2;
				case FilterEnum.NumDecimal_3casa:
				case FilterEnum.PercDecimal_3casa:
					return 3;
				case FilterEnum.NumDecimal_4casa:
				case FilterEnum.PercDecimal_4casa:
					return 4;
				case FilterEnum.NumDecimal_5casa:
				case FilterEnum.PercDecimal_5casa:
					return 5;
				case FilterEnum.NumDecimal_6casa:
				case FilterEnum.PercDecimal_6casa:
					return 6;
				case FilterEnum.NumDecimal_7casa:
				case FilterEnum.PercDecimal_7casa:
					return 7;
				case FilterEnum.NumDecimal_8casa:
				case FilterEnum.PercDecimal_8casa:
					return 8;
			}

			return 0;
		}
		#endregion

		#region Verifica o tipo de formatacao de um filtro
		public static TipoFormatacao CheckType( FilterEnum filter ) 
		{
			switch(filter)
			{
					//verificação de valores
				case FilterEnum.Valor:
				case FilterEnum.NumDecimal_1casa:
				case FilterEnum.NumDecimal_2casa:
				case FilterEnum.NumDecimal_3casa:
				case FilterEnum.NumDecimal_4casa:
				case FilterEnum.NumDecimal_5casa:
				case FilterEnum.NumDecimal_6casa:
				case FilterEnum.NumDecimal_7casa:
				case FilterEnum.NumDecimal_8casa:
					return TipoFormatacao.Valor;
				
				case FilterEnum.PercDecimal_1casa:
				case FilterEnum.PercDecimal_2casa:
				case FilterEnum.PercDecimal_3casa:
				case FilterEnum.PercDecimal_4casa:
				case FilterEnum.PercDecimal_5casa:
				case FilterEnum.PercDecimal_6casa:
				case FilterEnum.PercDecimal_7casa:
				case FilterEnum.PercDecimal_8casa:
					return TipoFormatacao.Numero;
				
				case FilterEnum.NumInteiro:
					return TipoFormatacao.Inteiro;

				case FilterEnum.Data:					
				case FilterEnum.DataHora:
				case FilterEnum.DataDMY:
				case FilterEnum.DataMDY:
				case FilterEnum.DataYMD:
					return TipoFormatacao.Data;
				
				case FilterEnum.HorarioHM:
				case FilterEnum.HorarioHMS:
					return TipoFormatacao.Horario;
				
				case FilterEnum.MesAno:
					return TipoFormatacao.MesAno;
				
				default:
					return TipoFormatacao.Texto;
			}	
		}

		#endregion

		#region Pega uma regular expression para a validação de uma data apartir da Cultura do Usuário

		private static string GetRegExpForCultureDateTime (
			System.Globalization.CultureInfo c,
			FilterEnum filter ) 
		{
			string[] splitData = c.DateTimeFormat.ShortDatePattern.Split(Convert.ToChar(c.DateTimeFormat.DateSeparator));
			string _regExpValidationForDate = "^\\s*";

			for (int i =0; i<splitData.Length; i++ ) 
			{
				if ( splitData[i].ToLower().StartsWith("d") || 
					splitData[i].ToLower().StartsWith("m") )
					_regExpValidationForDate += "(\\d{1,2})";
				else
					_regExpValidationForDate += "(\\d{4})";

				if ( (i+1) < splitData.Length )
					_regExpValidationForDate += "([-./])";
				else 
				{
					if ( filter != FilterEnum.DataHora )
						_regExpValidationForDate += "\\s*$";
				}
			}

			if ( filter == FilterEnum.DataHora )
			{
				//separador data com o horário
				_regExpValidationForDate += "(\\ )";

				//TODO : só vai funcionar com HH:mm ou HH:mm:ss

				string[] splitTime = c.DateTimeFormat.ShortTimePattern.Split(Convert.ToChar(c.DateTimeFormat.TimeSeparator));
				for(int i=0; i<splitTime.Length; i++ )
				{
					if (  splitData[i].ToLower().StartsWith("h")  ) 
					{
						_regExpValidationForDate += "(\\d{1,2})";
						if ( (i+1) < splitData.Length )
							_regExpValidationForDate += "(:)";
					}
					else if (  splitData[i].ToLower().StartsWith("m")  ) 
					{
						_regExpValidationForDate += "(\\d{1,2})";
						if ( (i+1) < splitData.Length ) //coloca para os segundos
							_regExpValidationForDate += "((\\:)(\\d{1,2}))?";
					}
				}
				_regExpValidationForDate += "\\s*$";
			}

			return _regExpValidationForDate;
		}
		#endregion
	}
}
