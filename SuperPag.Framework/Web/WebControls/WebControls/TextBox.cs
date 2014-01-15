using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Drawing.Design;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls
{

	#region Classe para criar o range validator
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class RangeValidatorMsgTextBox 
	{
		private string _msgRangeValidatorMinimumValue = "";
		private string _msgRangeValidatorMaximumValue = "";
		private string _msgRangeValidatorMinimumField = "";
		private string _msgRangeValidatorMaximumField = "";
		private string _msgRangeValidatorErrorMessage = "";
		private bool _enabled = false;
		private ValidationDataType _type;
		private string _msgSource = "";

		private ValorDefaultMinMaxEnum _valorDefaultMax = ValorDefaultMinMaxEnum.None;
		private ValorDefaultMinMaxEnum _valorDefaultMin = ValorDefaultMinMaxEnum.None;

		public enum ValorDefaultMinMaxEnum 
		{
			DecimalMinValue = 1,
			DecimalMaxValue = 2,
			IntegerMinValue = 3,
			IntegerMaxValue = 4,
			Zero = 5,
			None = 6,
			PreDefined = 7
		}

		public RangeValidatorMsgTextBox() {}

		//Message Source
		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public string MsgRangeValidatorMinimumValue
		{
			get{return _msgRangeValidatorMinimumValue;}
			set{_msgRangeValidatorMinimumValue = value;}
		}

		//Message Source
		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public string MsgRangeValidatorMaximumValue
		{
			get{return _msgRangeValidatorMaximumValue;}
			set{_msgRangeValidatorMaximumValue = value;}			
		}
		
		//Message Source
		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public string MsgRangeValidatorMinimumField
		{
			get{return _msgRangeValidatorMinimumField;}
			set{_msgRangeValidatorMinimumField = value;}
		}
		
		//Message Source
		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public string MsgRangeValidatorMaximumField
		{
			get{return _msgRangeValidatorMaximumField;}
			set{_msgRangeValidatorMaximumField = value;}
		}
		
		//Message Source
		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public bool Enabled
		{
			get{return _enabled;}
			set{_enabled = value;}
		}

		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public ValidationDataType Type
		{
			get{return _type;}
			set{_type = value;}
		}

		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public string MsgSource
		{
			get{return _msgSource;}
			set{_msgSource = value;}
		}

		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public ValorDefaultMinMaxEnum MsgDefaultMaxValue
		{
			get{return _valorDefaultMax;}
			set{_valorDefaultMax = value;}
		}
		
		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public ValorDefaultMinMaxEnum MsgDefaultMinValue
		{
			get{return _valorDefaultMin;}
			set{_valorDefaultMin = value;}
		}

		[Bindable(true), DefaultValue(""), NotifyParentProperty(true), 
		RefreshProperties(RefreshProperties.Repaint)] 
		public string RangeValidatorErrorMessage
		{
			get 
			{
				return this._msgRangeValidatorErrorMessage;
			}
			set 
			{
				this._msgRangeValidatorErrorMessage = value;
			}
		}

		internal object valueMinTemp;
		internal object valueMaxTemp;
	}
	#endregion

	/// <summary>
	///	TODO: Comentário
	/// </summary>	
	[
	Designer(typeof(TextBoxDesigner)),
	ToolboxData("<{0}:MsgTextBox runat=server></{0}:MsgTextBox>")]
	public class MsgTextBox : System.Web.UI.WebControls.TextBox, MessageControl
	{
		#region Propriedades e Eventos

		private string _msgSource;
		private string _msgSourceKey = "";
		private string _msgTextField;
		private string _msgEnabledField;
		private string _msgVisibleField;
		private string _formatString;
		private string _inputValidationErrorMessage;
		private MsgCustomValidators.FilterEnum _inputFilter;
		private string _inputValidation;
		private bool _hidden;
		private bool _inputMandatory;
		
		private object _message;
		private object _messageRangeValidator;
		
		private MessageControlBuilder _builder;
		private RegularExpressionValidator _regexp;
		private RegularExpressionValidator _regexpInputFilter;
		private RequiredFieldValidator _req;
		private CompareValidator _compare;
		private CustomValidator _custom;		
		private BaseValidator _rangeval;
		private RangeValidator _range;

		private DataTypeBindEnum _dataTypeBind = DataTypeBindEnum.Message;
		private string _valueKey = "";

		private RangeValidatorMsgTextBox _rangeValidatorMsgTextBox = new RangeValidatorMsgTextBox();
		private CustomFormatHelper.CustomFormatEnum _customFormat = CustomFormatHelper.CustomFormatEnum.None;

		private string _DefaultButton = "";

		//Css of child´s control
		private string _CssChildsControl = "";

		//Message Source
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSource
		{get{return _msgSource;}set{_msgSource = value;}}

		//Text Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgTextField
		{get{return _msgTextField;}set{_msgTextField = value;}}
		
		//Enabled Field
		[Bindable(true), Category("Message Bind"), DefaultValue(""), MessageReadOnly()]
		public string MsgEnabledField
		{get{return _msgEnabledField;}set{_msgEnabledField = value;}}

		//Visible Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgVisibleField
		{get{return _msgVisibleField;}set{_msgVisibleField = value;}}

		//Format String
		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public string FormatString
		{get{return _formatString;}set{_formatString = value;}}

		//Input Filter
		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public MsgCustomValidators.FilterEnum InputFilter 
		{get{return _inputFilter;} set{_inputFilter = value;}}

		//Custom Filter
		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public CustomFormatHelper.CustomFormatEnum CustomFormat 
		{get{return _customFormat;} set {_customFormat = value;}}

		//Message Source Key
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceKey
		{get{return _msgSourceKey;}set{_msgSourceKey = value;}}

		//TODO: Comentário
		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind AfterMessageBind;

		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind BeforeMessageBind;

		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public string InputValidation
		{get{return _inputValidation;}set{_inputValidation = value;}}

		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public string InputValidationErrorMessage
		{get{return _inputValidationErrorMessage;}set{_inputValidationErrorMessage = value;}}

		//Input Mandatory
		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public bool InputMandatory
		{get{return _inputMandatory;}set{_inputMandatory = value;}}
		
		//Input hidden
		[Bindable(true), Category("Behavior"), DefaultValue("")]
		public bool Hidden
		{get{return _hidden;}set{_hidden = value;}}

		//Classe para range validator
		//PersistenceMode.InnerProperty: Specifies that the property persists in
		//the ASP.NET server control as a nested tag. 
		//DesignerSerializationVisibility.Content: Specifies that a visual
		//designer serializes the contents of this property instead of the 
		//property itself. 
		[Bindable(true), DefaultValue(""), Category("Message Range Validator"), 
		PersistenceMode(PersistenceMode.Attribute),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public RangeValidatorMsgTextBox MsgRangeValidator
		{
			get{return _rangeValidatorMsgTextBox;}
			set{_rangeValidatorMsgTextBox = value;}
		}

		//Tipo de bind do controle
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public DataTypeBindEnum DataTypeBind
		{get{return _dataTypeBind;}set{_dataTypeBind = value;}}

		//Chave para pegar a propriedade
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string BindValueKey
		{get{return _valueKey;}set{_valueKey = value;}}

		//Define qual o botão será acionado quando teclar o ENTER
		[Bindable(true), Category("Action Button"), DefaultValue(""),
		Editor(typeof(ListControlsUIEditor), 
		typeof(System.Drawing.Design.UITypeEditor))]
		public string DefaultButton
		{get{return _DefaultButton;}set{_DefaultButton = value;}}

		//Css dos controles filhos
		[Bindable(true), Category("Controls Style"), DefaultValue("")]
		public string CssChildsControl
		{get{return _CssChildsControl;}set{_CssChildsControl = value;}}

		#endregion Propriedades e Eventos

		public void MessageInit()
		{
			if(this.Page is MessagePage)
			{
				MessagePage page = (MessagePage)this.Page;

				if ( _dataTypeBind == DataTypeBindEnum.Message ) 
				{
					if(_msgSource != String.Empty && _msgSource != null)
					{			
						if(_msgSourceKey != "")
						{
						
							_message = page.Messages.Get(_msgSourceKey);
						} 
						else
						{
							_message = page.Messages.Get(_msgSource);
						}
					}
				}
				else if ( _dataTypeBind == DataTypeBindEnum.Value && _valueKey != null )
				{
					_message = page.Messages.GetPropValue(_valueKey);
				}

				if(this.MsgRangeValidator.MsgSource != String.Empty && this.MsgRangeValidator.MsgSource != null)
				{			
					_messageRangeValidator = page.Messages.Get(this.MsgRangeValidator.MsgSource);
				}
			}
			else
			{
				throw new ApplicationException("The MsgTextBox must be placed inside a MessagePage ");
			}
			_builder = new MessageControlBuilder();
		}
		//TODO: Comentário
		protected override void OnInit(System.EventArgs e)
		{
			base.OnInit(e);
		}

		protected override void CreateChildControls()
		{
			if(_inputMandatory && this.Enabled)
			{
				_req = new RequiredFieldValidator();
				_req.ControlToValidate = this.ID;
				_req.EnableClientScript = true;
				_req.Display = ValidatorDisplay.Dynamic;
				_req.ErrorMessage = "&nbsp;(*)";
				_req.Display = ValidatorDisplay.Dynamic;
				
				//Set CssClass of child
				if ( _CssChildsControl != null && _CssChildsControl != string.Empty ) _req.CssClass = _CssChildsControl;

				Controls.Add(_req);
			}

			if ( _inputFilter != MsgCustomValidators.FilterEnum.None 
				&& _inputFilter != MsgCustomValidators.FilterEnum.Texto ) 
			{
				switch(_inputFilter)
				{
					case MsgCustomValidators.FilterEnum.Data:
						this.MaxLength = 11; //em vez de 10, somo +1 para nao dar problema no jscrip da mask
						break;
					case MsgCustomValidators.FilterEnum.DataHora:
						this.MaxLength = 20; //em vez de 19, somo +1 para nao dar problema no jscrip da mask
						break;
					case MsgCustomValidators.FilterEnum.Valor:
					case MsgCustomValidators.FilterEnum.NumDecimal_1casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_1casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_2casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_2casa:
						this.MaxLength = 15; //em vez de 14, somo +1 para nao dar problema no jscrip da mask
						break;
					case MsgCustomValidators.FilterEnum.NumDecimal_3casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_4casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_5casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_6casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_7casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_8casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_3casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_4casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_5casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_6casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_7casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_8casa:
						this.MaxLength = 19; //em vez de 19, somo +1 para nao dar problema no jscrip da mask
						break;
					case MsgCustomValidators.FilterEnum.NumInteiro:
						if(this.MaxLength == 0)
							this.MaxLength = 10; //em vez de 10, somo +1 para nao dar problema no jscrip da mask
						break;
					case MsgCustomValidators.FilterEnum.CPF:
						this.MaxLength = 15; //em vez de 14, somo +1 para nao dar problema no jscrip da mask
						break;
					case MsgCustomValidators.FilterEnum.CNPJ:
						this.MaxLength = 19; //em vez de 19, somo +1 para nao dar problema no jscrip da mask
						break;
					
					case MsgCustomValidators.FilterEnum.HorarioHM:
						this.MaxLength = 6; //Len(HH:mm ) + 1 para nao dar problema no jscrip da mask
						break;
					
					case MsgCustomValidators.FilterEnum.HorarioHMS:
						this.MaxLength = 9;//Len(HH:mm:ss ) + 1 para nao dar problema no jscrip da mask
						break;
					case MsgCustomValidators.FilterEnum.MesAno:
						this.MaxLength = 7;//Len(MM:yyyy ) + 1 para nao dar problema no jscrip da mask
						break;
				}

				MsgCustomValidators _msgCustomValidator = new MsgCustomValidators( _inputFilter );
				
				if ( _inputFilter == MsgCustomValidators.FilterEnum.NumInteiro ) 
				{
					BaseValidator _ocompare = _msgCustomValidator.CreateCustomValidator( this );
					_compare = (CompareValidator)_ocompare;
					Controls.Add(_compare);

					_range = new RangeValidator ();
					_range.ControlToValidate = this.ID;
					_range.EnableClientScript = true;
					_range.ErrorMessage = "(Valor inválido)";
					_range.MinimumValue = int.MinValue.ToString();
					_range.MaximumValue = int.MaxValue.ToString();
					_range.Type = ValidationDataType.Integer;
					_range.Display = ValidatorDisplay.Dynamic;

					//Set CssClass of child
					if ( _CssChildsControl != null && _CssChildsControl != string.Empty ) _range.CssClass = _CssChildsControl;

					Controls.Add(_range);
				} 
				else 
				{
					BaseValidator _oregexpInputFilter = _msgCustomValidator.CreateCustomValidator( this );
					_regexpInputFilter = (RegularExpressionValidator)_oregexpInputFilter;
					Controls.Add(_regexpInputFilter);

					if (MsgCustomValidators.CheckType( _inputFilter ) == TipoFormatacao.Data ) 
					{
						_compare = new CompareValidator();
						_compare.ControlToValidate = this.ID;
						_compare.EnableClientScript = true;
						_compare.ErrorMessage = "&nbsp;Data inválida&nbsp;";
						_compare.Operator = ValidationCompareOperator.DataTypeCheck;
						_compare.Type = ValidationDataType.Date;
						_compare.Display = ValidatorDisplay.Dynamic;
						
						//Set CssClass of child
						if ( _CssChildsControl != null && _CssChildsControl != string.Empty ) _compare.CssClass = _CssChildsControl;

						_range = new RangeValidator ();
						_range.ControlToValidate = this.ID;
						_range.EnableClientScript = true;
						_range.ErrorMessage = "&nbsp;Data informada fora da faixa permitida&nbsp;";
						_range.MinimumValue = "1900-01-01";
						_range.MaximumValue = "2099-12-31";
						_range.Type = ValidationDataType.Date;
						_range.Display = ValidatorDisplay.Dynamic;
						
						//Set CssClass of child
						if ( _CssChildsControl != null && _CssChildsControl != string.Empty ) _range.CssClass = _CssChildsControl;

						Controls.Add(_range);
						Controls.Add(_compare);
					}
					else if ( MsgCustomValidators.CheckType( _inputFilter ) == TipoFormatacao.Valor ) 
					{
						_range = new RangeValidator ();
						_range.ControlToValidate = this.ID;
						_range.EnableClientScript = true;
						_range.ErrorMessage = "(*)";
						_range.MinimumValue = decimal.MinValue.ToString();
						_range.MaximumValue = decimal.MaxValue.ToString();
						_range.Type = ValidationDataType.Currency;
						_range.Display = ValidatorDisplay.Dynamic;

						//Set CssClass of child
						if ( _CssChildsControl != null && _CssChildsControl != string.Empty ) _range.CssClass = _CssChildsControl;

						Controls.Add(_range);
					}
					else if ( MsgCustomValidators.CheckType( _inputFilter ) == TipoFormatacao.Horario ) 
					{
						_custom = new CustomValidator();
						_custom.ControlToValidate = this.ID;
						_custom.EnableClientScript = true;
						if ( _inputFilter == MsgCustomValidators.FilterEnum.HorarioHM ) 
						{
							_custom.ErrorMessage = "(formato hh:mm)";
							_custom.ClientValidationFunction = "validHHMM";
						}
						else if ( _inputFilter == MsgCustomValidators.FilterEnum.HorarioHMS ) 
						{
							_custom.ErrorMessage = "(formato hh:mm:ss)";
							_custom.ClientValidationFunction = "validHHMMSS";
						}
						
						_custom.Display = ValidatorDisplay.Dynamic;

						//Set CssClass of child
						if ( _CssChildsControl != null && _CssChildsControl != string.Empty ) _custom.CssClass = _CssChildsControl;

						Controls.Add(_custom);
					}
					else if ( MsgCustomValidators.CheckType( _inputFilter ) == TipoFormatacao.MesAno ) 
					{
						_custom = new CustomValidator();
						_custom.ControlToValidate = this.ID;
						_custom.EnableClientScript = true;
						_custom.ErrorMessage = "(formato mm/aaaa)";
						_custom.ClientValidationFunction = "validMMYYYY";
						_custom.Display = ValidatorDisplay.Dynamic;
	
						//Set CssClass of child
						if ( _CssChildsControl != null && _CssChildsControl != string.Empty ) _custom.CssClass = _CssChildsControl;

						Controls.Add(_custom);
					}
				}
			}

			if(_inputValidation != string.Empty && _inputValidation != null)
			{
				_regexp = new RegularExpressionValidator();
				_regexp.ControlToValidate = this.ID;
				_regexp.EnableClientScript = true;
				_regexp.ErrorMessage = _inputValidationErrorMessage;
				_regexp.ValidationExpression = _inputValidation;
				_regexp.Display = ValidatorDisplay.Dynamic;

				//Set CssClass of child
				if ( _CssChildsControl != null && _CssChildsControl != string.Empty ) _regexp.CssClass = _CssChildsControl;

				Controls.Add(_regexp);
			}

			if ( this.MsgRangeValidator.Enabled && this.Enabled && !this.ReadOnly ) 
			{
				_rangeval = MsgTextBoxHelper.CreateRangeValidator( this );

				//Set CssClass of child
				if ( _CssChildsControl != null && _CssChildsControl != string.Empty ) _rangeval.CssClass = _CssChildsControl;

				Controls.Add(_rangeval);
			}
		}

		//Message Bind
		public void MessageBind()
		{
			OnBeforeMessageBind(_message);

			if(_message != null && _dataTypeBind == DataTypeBindEnum.Message)
			{
				switch(_customFormat)
				{
					case CustomFormatHelper.CustomFormatEnum.None:
					{
						this.Text = 
							_builder.GetPropertyValue(_message, _msgTextField, _formatString, true);
						break;
					}
					default: 
					{
						string propertyValue = 
							_builder.GetPropertyValue(_message, _msgTextField, "{0}", true );

						if ( propertyValue != null && propertyValue != "" ) 
						{
							CustomFormatHelper formatString = new CustomFormatHelper( _customFormat );
							this.Text = formatString.GetCurrentFormatString( propertyValue );
						} 
						else
							this.Text = "";

						break;
					}
				}
			}
			else if ( _message != null && this.BindValueKey != null && _dataTypeBind == DataTypeBindEnum.Value ) 
			{
				switch(_customFormat)
				{
					case CustomFormatHelper.CustomFormatEnum.None:
					{
						this.Text = 
							_builder.ConvertToString(_message, _formatString);
						break;
					}
					default: 
					{
						string propertyValue = _builder.ConvertToString(_message );

						if ( propertyValue != null && propertyValue != "" ) 
						{
							CustomFormatHelper formatString = new CustomFormatHelper( _customFormat, true );
							this.Text = formatString.GetCurrentFormatString( propertyValue );
						} 
						else
							this.Text = "";

						break;
					}
				}
			}

			//Verifica os valores para range
			if ( this.MsgRangeValidator.Enabled )
				MsgTextBoxHelper.SetRangeValues( this, _messageRangeValidator, _builder );

			OnAfterMessageBind(_message);
		}

		//subo o evento
		public void OnAfterMessageBind(object message)
		{
			if(AfterMessageBind != null)
			{
				AfterMessageBind(this, message);
			}
		}

		//subo o evento
		public void OnBeforeMessageBind(object message)
		{
			if(BeforeMessageBind != null)
			{
				BeforeMessageBind(this, message);
			}
		}

		//TODO: Comentário
		public void MessageUnBind()
		{
			if(_message != null && _dataTypeBind == DataTypeBindEnum.Message)
			{
				Helper.SetUndBindPropetyValue( _message, this );
			}	
		}

		//TODO: Comentário
		protected override void Render(HtmlTextWriter writer)
		{
			MsgTextBoxHelper.RegisterStartupScriptOfInputFilter( this, Page.Form.Name );
            
			base.Render (writer);

			if(_req != null)
			{
				_req.RenderControl(writer);
			}
			if(_compare != null)
			{
				_compare.RenderControl(writer);
			}
			if(_range != null)
			{
				_range.RenderControl(writer);
			}
			if(_regexp != null)
			{
				_regexp.RenderControl(writer);
			}
			if(_regexpInputFilter != null)
			{
				_regexpInputFilter.RenderControl(writer);
			}
			if(_rangeval != null)
			{
				_rangeval.RenderControl(writer);
			}
			if ( _custom != null ) 
			{
				_custom.RenderControl(writer);
			}			
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if ( (Site == null || !Site.DesignMode) && Hidden)
			{
				writer.AddAttribute("type", "hidden");
			} 
			base.AddAttributesToRender (writer);
		}

		//é usado pelo repeater
		public void MessageDataSource(object message) 
		{
			_builder = new MessageControlBuilder();
			_message = message;
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState (savedState);
			if ( ViewState["__TextBoxRangeValidator"] is object[] ) 
			{
				object[] property = (object[])ViewState["__TextBoxRangeValidator"];
				this.MsgRangeValidator.valueMinTemp = property[0];
				this.MsgRangeValidator.valueMaxTemp = property[1];
				this.MsgRangeValidator.Type = (ValidationDataType)property[2];
				this.MsgRangeValidator.MsgRangeValidatorMaximumField = (string)property[3];
				this.MsgRangeValidator.MsgRangeValidatorMinimumField = (string)property[4];
				this.MsgRangeValidator.Enabled = true;
			}

			this.InputMandatory = (bool)this.ViewState["InputMandatory"];
		}

		protected override object SaveViewState()
		{
			if ( this.MsgRangeValidator.Enabled ) 
			{
				object[] values = new object[] { this.MsgRangeValidator.valueMinTemp , this.MsgRangeValidator.valueMaxTemp, this.MsgRangeValidator.Type, this.MsgRangeValidator.MsgRangeValidatorMaximumField, this.MsgRangeValidator.MsgRangeValidatorMinimumField };
				ViewState["__TextBoxRangeValidator"] = values;
			} 
			else  
			{
				ViewState["__TextBoxRangeValidator"] = null;
			}

			this.ViewState["InputMandatory"] = this.InputMandatory;

			return base.SaveViewState ();
		}
	}

	//TODO: Comentário
	public class TextBoxDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			string designTimeHtml = base.GetDesignTimeHtml ();
			MsgTextBox textBox = (MsgTextBox)this.Component;
			
			if(textBox.TextMode == TextBoxMode.MultiLine)
			{
				string _value = "{" + textBox.MsgTextField + "}";
				designTimeHtml =
					designTimeHtml.Insert(designTimeHtml.Length - 11, _value);
			}
			else
			{
				string _value = "value=\"{" + textBox.MsgTextField + "}\"";
				designTimeHtml =
					designTimeHtml.Insert(designTimeHtml.Length - 2, _value);
			}

			if(textBox.InputValidation != null && 
				textBox.InputValidation != string.Empty)
			{
				designTimeHtml += "&nbsp;(" + textBox.InputValidation + ")";
			}
			
			if(textBox.InputMandatory)
			{
				designTimeHtml += "&nbsp;(*)";
			}

			if ( textBox.Hidden )
				designTimeHtml += "&nbsp;{hidden}";

			return designTimeHtml;
		}
	}
	
	#region MsgTextBoxHelper
	internal class MsgTextBoxHelper
	{
		//Retorna valores predefinidos caso seja passado int.minvalue, datetime.min , decimal.min e double.minvalue 
		#region CheckValueRangeValidator
		public static object CheckValueRangeValidator( MsgTextBox control, object valor, int MaxOuMin ) 
		{		
			if (
				(
				control.MsgRangeValidator.Type == ValidationDataType.Currency && 
				valor is decimal &&
				Convert.ToDecimal( valor ) == decimal.MinValue 
				)
				||
				( 
				control.MsgRangeValidator.Type == ValidationDataType.Integer && 
				valor is Int32 &&
				Convert.ToInt32( valor ) == int.MinValue
				)
				||
				( 
				control.MsgRangeValidator.Type == ValidationDataType.Double && 
				valor is double &&
				Convert.ToDouble( valor ) == double.MinValue
				)
				||
				( 
				control.MsgRangeValidator.Type == ValidationDataType.Date && 
				valor is DateTime &&
				Convert.ToDateTime( valor ) == DateTime.MinValue
				)
				|| valor == null
				) 
			{
				if ( MaxOuMin == 1 ) //Valores do range maximo 
				{
					switch( control.MsgRangeValidator.MsgDefaultMaxValue ) 
					{
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.Zero:
							return 0;
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.PreDefined:
							if ( control.MsgRangeValidator.MsgRangeValidatorMaximumValue != string.Empty )
								return GetNullIfMinValue( control.MsgRangeValidator.Type, control.MsgRangeValidator.MsgRangeValidatorMaximumValue );
							break;
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.DecimalMaxValue:
							return null;
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.IntegerMaxValue:
							return null;
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.None:
							return null;
					}
				} 
				else 
				{
					switch( control.MsgRangeValidator.MsgDefaultMinValue ) 
					{
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.Zero:
							return 0;
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.PreDefined:
							if ( control.MsgRangeValidator.MsgRangeValidatorMinimumValue != string.Empty )
								return GetNullIfMinValue( control.MsgRangeValidator.Type, control.MsgRangeValidator.MsgRangeValidatorMinimumValue );
							break;
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.DecimalMinValue:
							return null;
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.IntegerMinValue:
							return null;
						case RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.None:
							return null;
					}
				}
			}

			return valor;
		}
		#endregion

		#region Registra o script pra criação do objeto de filtro
		public static void RegisterStartupScriptOfInputFilter( MsgTextBox control, string formName ) 
		{
			//Caso não é visível ou é read only
			if ( !control.Visible || control.ReadOnly ) return;

			string objectMaskName = "o" + control.ClientID + "Mask";
			string concatOnKeyDown = "";

			if ( Ensure.StringIsNotEmpty( control.DefaultButton ) ) 
			{
				concatOnKeyDown = "fnTrapKD( document." + formName + "." + control.DefaultButton + ",event);";
			}

			if ( control.InputFilter != MsgCustomValidators.FilterEnum.None ) 
			{
				#region Coloco o script para filtrar as teclas pelo tipo do formato
				//Coloco o script com os tamanhos dos pixels de cada fonte
				switch( control.InputFilter ) 
				{
					case MsgCustomValidators.FilterEnum.Data : 
					{
						control.Page.RegisterStartupScript("script_DataInput"+objectMaskName, 
							"<script> " + objectMaskName + " = null;" + objectMaskName + "= new MaskDateTime(\"dd/mm/yyyy\", \"" + concatOnKeyDown + "\" );" +
							objectMaskName + ".attach(document." + formName + "." + control.ClientID + ");</script>");
						break;
					}
					case MsgCustomValidators.FilterEnum.Valor : 
					case MsgCustomValidators.FilterEnum.NumDecimal_1casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_2casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_3casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_4casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_5casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_6casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_7casa:
					case MsgCustomValidators.FilterEnum.NumDecimal_8casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_1casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_2casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_3casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_4casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_5casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_6casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_7casa:
					case MsgCustomValidators.FilterEnum.PercDecimal_8casa:
					{
						int digits = MsgCustomValidators.CheckDigits( control.InputFilter );
						string nroDecimalAfterNumber = (new string('0', digits));
					
						control.Page.RegisterStartupScript("script_DataInput"+objectMaskName, 
							"<script> " + objectMaskName + " = null;" + objectMaskName + "= new MaskDecimal(\"###.###," + nroDecimalAfterNumber + "\", \"" + concatOnKeyDown + "\" );" +
							objectMaskName + ".attach(document." + formName + "." + control.ClientID + ");</script>");
						break;
					}
					case MsgCustomValidators.FilterEnum.CPF:
					{
						control.Page.RegisterStartupScript("script_DataInput"+objectMaskName, 
							"<script> " + objectMaskName + " = null;" + objectMaskName + "= new MaskGeneric(\"###.###.###-##\", \"" + concatOnKeyDown + "\" );" +
							objectMaskName + ".attach(document." + formName + "." + control.ClientID + ");</script>");
						break;
					}
					case MsgCustomValidators.FilterEnum.CNPJ:
					{
						control.Page.RegisterStartupScript("script_DataInput"+objectMaskName, 
							"<script> " + objectMaskName + " = null;" + objectMaskName + "= new MaskGeneric(\"##.###.###/####-##\", \"" + concatOnKeyDown + "\" );" +
							objectMaskName + ".attach(document." + formName + "." + control.ClientID + ");</script>");
						break;
					}
					case MsgCustomValidators.FilterEnum.CEP:
					{
						control.Page.RegisterStartupScript("script_DataInput"+objectMaskName, 
							"<script> " + objectMaskName + " = null;" + objectMaskName + "= new MaskGeneric(\"#####-###\", \"" + concatOnKeyDown + "\" );" +
							objectMaskName + ".attach(document." + formName + "." + control.ClientID + ");</script>");
						break;
					}
					case MsgCustomValidators.FilterEnum.HorarioHMS:
					{
						control.Page.RegisterStartupScript("script_DataInput"+objectMaskName, 
							"<script> " + objectMaskName + " = null;" + objectMaskName + "= new MaskGeneric(\"##:##:##\", \"" + concatOnKeyDown + "\" );" +
							objectMaskName + ".attach(document." + formName + "." + control.ClientID + ");</script>");
						break;
					}
					case MsgCustomValidators.FilterEnum.HorarioHM:
					{
						control.Page.RegisterStartupScript("script_DataInput"+objectMaskName, 
							"<script> " + objectMaskName + " = null;" + objectMaskName + "= new MaskGeneric(\"##:##\", \"" + concatOnKeyDown + "\" );" +
							objectMaskName + ".attach(document." + formName + "." + control.ClientID + ");</script>");
						break;
					}
					case MsgCustomValidators.FilterEnum.NumInteiro:
					{
						control.Page.RegisterStartupScript("script_DataInput"+objectMaskName, 
							"<script> " + objectMaskName + " = null;" + objectMaskName + "= new MaskNumeric(\"[0-9]\", \"" + concatOnKeyDown + "\" );" +
							objectMaskName + ".attach(document." + formName + "." + control.ClientID + ");</script>");
						break;
					}
					case MsgCustomValidators.FilterEnum.MesAno:
					{
						control.Page.RegisterStartupScript("script_DataInput"+objectMaskName, 
							"<script> " + objectMaskName + " = null;" + objectMaskName + "= new MaskGeneric(\"##/####\", \"" + concatOnKeyDown + "\" );" +
							objectMaskName + ".attach(document." + formName + "." + control.ClientID + ");</script>");
						break;
					}
				}
				#endregion
			} 
			else if (concatOnKeyDown != "") 
			{
				control.Attributes.Add("onkeydown", concatOnKeyDown );
			}
		}
		#endregion

		public static void SetRangeValues ( MsgTextBox control, object _messageRangeValidator, MessageControlBuilder _builder ) 
		{
			#region Coloca Range Validator
			if ( control.MsgRangeValidator.Enabled ) 
			{
				if ( _messageRangeValidator != null ) 
				{
					if ( 
						control.MsgRangeValidator.MsgRangeValidatorMaximumField != string.Empty && 
						control.MsgRangeValidator.MsgRangeValidatorMaximumField != null) 
					{
						control.MsgRangeValidator.valueMaxTemp = 
							_builder.GetPropertyValue(_messageRangeValidator, control.MsgRangeValidator.MsgRangeValidatorMaximumField, true, typeof(decimal));
					} 

					//Valores default caso o valor da propriedade seja nulo
					control.MsgRangeValidator.valueMaxTemp = 
						CheckValueRangeValidator( control, control.MsgRangeValidator.valueMaxTemp, 1 );
				
					if ( 
						control.MsgRangeValidator.MsgRangeValidatorMinimumField != string.Empty && 
						control.MsgRangeValidator.MsgRangeValidatorMinimumField != null) 
					{
						control.MsgRangeValidator.valueMinTemp = 
							_builder.GetPropertyValue(_messageRangeValidator, control.MsgRangeValidator.MsgRangeValidatorMinimumField, true, typeof(decimal));
					} 

					//Valores default caso o valor da propriedade seja nulo
					control.MsgRangeValidator.valueMinTemp = 
						CheckValueRangeValidator( control, control.MsgRangeValidator.valueMinTemp, 2 );
				} 
				else 
				{
					if ( control.MsgRangeValidator.valueMaxTemp == null &&
						control.MsgRangeValidator.MsgDefaultMaxValue == RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.PreDefined &&
						control.MsgRangeValidator.MsgRangeValidatorMaximumValue != "" )
						control.MsgRangeValidator.valueMaxTemp = GetRealValueFromText ( control.MsgRangeValidator , control.MsgRangeValidator.MsgRangeValidatorMaximumValue );

					if ( control.MsgRangeValidator.valueMinTemp == null &&
						control.MsgRangeValidator.MsgDefaultMinValue == RangeValidatorMsgTextBox.ValorDefaultMinMaxEnum.PreDefined &&
						control.MsgRangeValidator.MsgRangeValidatorMinimumValue != "" )
						control.MsgRangeValidator.valueMinTemp =  GetRealValueFromText ( control.MsgRangeValidator, control.MsgRangeValidator.MsgRangeValidatorMinimumValue ) ;

					//Valores default caso o valor da propriedade seja nulo
					control.MsgRangeValidator.valueMaxTemp = 
						CheckValueRangeValidator( control, control.MsgRangeValidator.valueMaxTemp, 1 );

					//Valores default caso o valor da propriedade seja nulo
					control.MsgRangeValidator.valueMinTemp = 
						CheckValueRangeValidator( control, control.MsgRangeValidator.valueMinTemp, 2 );
				}
			}
			#endregion
		}

		
		private static object GetRealValueFromText ( RangeValidatorMsgTextBox range, string value )
		{
			if ( range.Type == ValidationDataType.Currency ) { return Convert.ToDecimal( value ) ; }
			else if ( range.Type == ValidationDataType.Date ) {  return Convert.ToDateTime( value ) ; }
			else if ( range.Type == ValidationDataType.Double ) { return Convert.ToDouble( value ) ; }
			else if ( range.Type == ValidationDataType.Integer ) { return Convert.ToInt32( value ) ; }
			else
				return value;
		}

		private static object GetNullIfMinValue( ValidationDataType type, object value ) 
		{
			if ( type == ValidationDataType.Currency ) { return Convert.ToDecimal( value ) == decimal.MinValue ? (object)null: (object)Convert.ToDecimal( value ); }
			else if ( type == ValidationDataType.Date ) {  return Convert.ToDateTime( value ) == DateTime.MinValue ? (object)null : (object)Convert.ToDateTime( value ) ; }
			else if ( type == ValidationDataType.Double ) { return Convert.ToDouble( value ) == double.MinValue ? (object)null : (object)Convert.ToDouble( value )  ; }
			else if ( type == ValidationDataType.Integer ) { return Convert.ToInt32( value ) == int.MinValue ? (object)null : (object)Convert.ToInt32( value ) ; }
			else
				return value.ToString() == string.Empty ? null: value;
		}
	
		public static BaseValidator CreateRangeValidator( MsgTextBox control ) 
		{
			#region Cria um objeto de verificação de Range
			bool ComparaMenorQue = false;
			bool ComparaMaiorQue = false;
			bool ComparaIntervalo = false;

			if ( control.MsgRangeValidator.valueMinTemp == null &&
				 control.MsgRangeValidator.valueMaxTemp == null ) 
				throw new Exception("Problemas para gerar a comparação. Os valores min e max são nulos!");

			ComparaMenorQue = ( control.MsgRangeValidator.valueMinTemp == null &&
								control.MsgRangeValidator.valueMaxTemp != null );

			ComparaMaiorQue = ( control.MsgRangeValidator.valueMinTemp != null &&
				control.MsgRangeValidator.valueMaxTemp == null );

			ComparaIntervalo = ( control.MsgRangeValidator.valueMinTemp != null &&
				control.MsgRangeValidator.valueMaxTemp != null );

			string MaximumValue = "";
			string MinimumValue = "";
			string ErrorMessageValue = "";

			if ( control.MsgRangeValidator.Type == ValidationDataType.Currency ) 
			{
				int digits = MsgCustomValidators.CheckDigits( control.InputFilter );

				if ( control.MsgRangeValidator.valueMaxTemp != null )
					MaximumValue = CustomFormatHelper.FormatCurrency( control.MsgRangeValidator.valueMaxTemp.ToString(), digits, false );

				if ( control.MsgRangeValidator.valueMinTemp != null )
					MinimumValue = CustomFormatHelper.FormatCurrency( control.MsgRangeValidator.valueMinTemp.ToString(), digits, false );
				
				if ( MinimumValue == "" && MaximumValue != "" )
					ErrorMessageValue = "&nbsp;Digite valores menores que " + CustomFormatHelper.FormatCurrency( control.MsgRangeValidator.valueMaxTemp.ToString(), digits, true );
				else if ( MaximumValue == "" && MinimumValue != "" )
					ErrorMessageValue = "&nbsp;Digite valores maiores que " + CustomFormatHelper.FormatCurrency( control.MsgRangeValidator.valueMinTemp.ToString(), digits, true );
				else if ( MaximumValue != MinimumValue )
					ErrorMessageValue = "&nbsp;Digite os valores entre " + CustomFormatHelper.FormatCurrency( control.MsgRangeValidator.valueMinTemp.ToString(), digits, true ) + " e " + 
						CustomFormatHelper.FormatCurrency( control.MsgRangeValidator.valueMaxTemp.ToString(), digits, true );
				else
					ErrorMessageValue = "&nbsp;Valor permitido: " + CustomFormatHelper.FormatCurrency( control.MsgRangeValidator.valueMinTemp.ToString(), digits, true );
			} 
			else if ( control.MsgRangeValidator.Type == ValidationDataType.Double ) 
			{
				int digits = MsgCustomValidators.CheckDigits( control.InputFilter );

				if ( control.MsgRangeValidator.valueMaxTemp != null )
					MaximumValue = CustomFormatHelper.FormatNumber( control.MsgRangeValidator.valueMaxTemp.ToString(), digits );

				if ( control.MsgRangeValidator.valueMinTemp != null )
					MinimumValue = CustomFormatHelper.FormatNumber( control.MsgRangeValidator.valueMinTemp.ToString(), digits );
				
				if ( MinimumValue == "" && MaximumValue != "" )
					ErrorMessageValue = "&nbsp;Digite valores menores que " + MaximumValue;
				else if ( MaximumValue == "" && MinimumValue != "" )
					ErrorMessageValue = "&nbsp;Digite valores maiores que " + MinimumValue;
				else if ( MaximumValue != MinimumValue )
					ErrorMessageValue = "&nbsp;Digite os valores entre " + MinimumValue + " e " + MaximumValue;
				else
					ErrorMessageValue = "&nbsp;Valor permitido: " + MinimumValue;
			}
			else if ( control.MsgRangeValidator.Type == ValidationDataType.Date ) 
			{
				if ( control.MsgRangeValidator.valueMaxTemp != null )
					MaximumValue = CustomFormatHelper.FormatData( control.MsgRangeValidator.valueMaxTemp.ToString() );

				if ( control.MsgRangeValidator.valueMinTemp != null )
					MinimumValue = CustomFormatHelper.FormatData( control.MsgRangeValidator.valueMinTemp.ToString() );

				if ( MinimumValue == "" && MaximumValue != "" )
					ErrorMessageValue = "&nbsp;Digite datas menores que " + MaximumValue;
				else if ( MaximumValue == "" && MinimumValue != "" )
					ErrorMessageValue = "&nbsp;Digite datas maiores que " + MinimumValue;
				else if ( MaximumValue != MinimumValue )
					ErrorMessageValue = "&nbsp;Digite as datas entre " + MinimumValue + " e " + MaximumValue;
				else
					ErrorMessageValue = "&nbsp;Data permitida: " + MinimumValue;
			}
			else if ( control.MsgRangeValidator.Type == ValidationDataType.Integer ) 
			{
				if ( control.MsgRangeValidator.valueMaxTemp != null )
					MaximumValue = Convert.ToInt32( control.MsgRangeValidator.valueMaxTemp ).ToString();

				if ( control.MsgRangeValidator.valueMinTemp != null )
					MinimumValue = Convert.ToInt32( control.MsgRangeValidator.valueMinTemp ).ToString();

				if ( MinimumValue == "" && MaximumValue != "" )
					ErrorMessageValue = "&nbsp;Digite valores menores que " + MaximumValue;
				else if ( MaximumValue == "" && MinimumValue != "" )
					ErrorMessageValue = "&nbsp;Digite valores maiores que " + MinimumValue;
				else if ( MaximumValue != MinimumValue )
					ErrorMessageValue = "&nbsp;Digite os valores entre " + MinimumValue + " e " + MaximumValue;
				else
					ErrorMessageValue = "&nbsp;Valor permitido: " + MinimumValue;
			}

			if ( control.MsgRangeValidator.RangeValidatorErrorMessage != "" )
				ErrorMessageValue = control.MsgRangeValidator.RangeValidatorErrorMessage;

			BaseValidator baseValidator = null;

			if ( MaximumValue == "" && MinimumValue == "" ) return null;

			if ( (MaximumValue == MinimumValue) || ComparaMenorQue || ComparaMaiorQue ) 
			{
				baseValidator = new CompareValidator();

				if ( MaximumValue == MinimumValue ) 
				{
					((CompareValidator)baseValidator).ValueToCompare = MaximumValue;
					((CompareValidator)baseValidator).Operator = ValidationCompareOperator.Equal;
				}
				else if ( ComparaMenorQue )
				{
					((CompareValidator)baseValidator).ValueToCompare = MaximumValue;
					((CompareValidator)baseValidator).Operator = ValidationCompareOperator.LessThanEqual;
				}
				else if ( ComparaMaiorQue )
				{
					((CompareValidator)baseValidator).ValueToCompare = MinimumValue;
					((CompareValidator)baseValidator).Operator = ValidationCompareOperator.GreaterThanEqual;
				}
				((CompareValidator)baseValidator).Type = control.MsgRangeValidator.Type;
			} 
			else
			{
				baseValidator = new RangeValidator();
				
				((RangeValidator)baseValidator).MaximumValue = MaximumValue;
				((RangeValidator)baseValidator).MinimumValue = MinimumValue;
				((RangeValidator)baseValidator).Type = control.MsgRangeValidator.Type;
			}

			baseValidator.EnableClientScript = true;
			baseValidator.ControlToValidate = control.ID;
			baseValidator.Display = ValidatorDisplay.Dynamic;
			baseValidator.ErrorMessage = ErrorMessageValue;
			
			return baseValidator;

			#endregion
		}
	}
	#endregion
}
