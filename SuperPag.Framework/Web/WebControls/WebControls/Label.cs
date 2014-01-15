using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls
{
	/// <summary>
	///	TODO: Comentário
	/// </summary>	
	[
	Designer(typeof(LabelDesigner)),
	DefaultProperty("Text"),
	ToolboxData("<{0}:MsgLabel runat=server></{0}:MsgLabel>")]
	public class MsgLabel : System.Web.UI.WebControls.Label, MessageControl, IPostBackDataHandler
	{
		#region Propriedades e Eventos

		private string _msgSource;
		private string _msgSourceKey = "";
		private string _msgTextField;
		private string _msgVisibleField;
		private string _formatString;
		private CustomFormatHelper.CustomFormatEnum _customFormat = CustomFormatHelper.CustomFormatEnum.None;
		private bool _parseTextComplete = false;
		private DataTypeBindEnum _dataTypeBind = DataTypeBindEnum.Message;
		private string _valueKey = "";

		object _message;
		MessageControlBuilder _builder;
		
		//Message Source
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSource
		{get{return _msgSource;}set{_msgSource = value;}}

		//Text Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgTextField
		{get{return _msgTextField;}set{_msgTextField = value;}}
		
		//Visible Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgVisibleField
		{get{return _msgVisibleField;}set{_msgVisibleField = value;}}

		//Format String
		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public string FormatString
		{get{return _formatString;}set{_formatString = value;}}

		//Message Source Key
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceKey
		{get{return _msgSourceKey;}set{_msgSourceKey = value;}}

		//TODO: Comentário
		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind AfterMessageBind;

		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind BeforeMessageBind;

		//Custom Filter
		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public CustomFormatHelper.CustomFormatEnum CustomFormat 
		{
			get	
			{
				return _customFormat;
			} set 
			{
				_customFormat = value;
			}
		}

		private bool _msgParseText = false;
		//Verifica se faz bind da mensagem ou do texto
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public bool ParseText
		{get{return _msgParseText;}set{_msgParseText = value;}}

		//Tipo de bind do controle
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public DataTypeBindEnum DataTypeBind
		{get{return _dataTypeBind;}set{_dataTypeBind = value;}}

		//Chave para pegar a propriedade
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string BindValueKey
		{get{return _valueKey;}set{_valueKey = value;}}

		protected string HelperID
		{ get{return "__" + ClientID + "_State"; } }

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
		
		#endregion Propriedades e Eventos

		#region Message Init

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
			}
			else
			{
				throw new ApplicationException("The MsgButton must be placed inside a MessagePage ");

			}
			_builder = new MessageControlBuilder();
		}

		#endregion Message Init

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);

			if (!Page.IsPostBack)
			{
				//grava um campo hidden com o valor do text
				Page.RegisterHiddenField(HelperID, BasicFunctions.EncryptToBase64String(this.Text));
			}
		}

		//Message Bind
		public void MessageBind()
		{
			OnBeforeMessageBind(_message);
			
			if(_message != null && _dataTypeBind == DataTypeBindEnum.Message )
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
							CustomFormatHelper formatString = new CustomFormatHelper( _customFormat, true );
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
						string propertyValue = 
							_builder.ConvertToString(_message );

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

			OnAfterMessageBind(_message);
		}

		//TODO: Comentário
		public void MessageUnBind()
		{}

		//TODO: Comentário
		protected override void Render(HtmlTextWriter writer)
		{
			if ( !_parseTextComplete && this.ParseText && this.Text != string.Empty && this.Text != "" )
			{
				CustomFormatHelper formatString = new CustomFormatHelper( _customFormat, true );
				this.Text = formatString.GetCurrentFormatString( this.Text );
				_parseTextComplete = true;
			}

			base.Render (writer);		
		}
		
		//é usado pelo repeater
		public void MessageDataSource(object message) 
		{
			_builder = new MessageControlBuilder();
			_message = message;
		}

		#region IPostBackDataHandler Members

		public void RaisePostDataChangedEvent()
		{
			// TODO:  Add MsgLabel.RaisePostDataChangedEvent implementation
		}

		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
		{
			string value = postCollection[HelperID];
			if (value != null)
				//converte o campo hidden encriptado
				this.Text = BasicFunctions.DecryptFromBase64String( value );
			return false;
		}

		#endregion

		public override void DataBind()
		{
			//Verifico se está dentro de um container
			if ( this.NamingContainer != null &&  this.NamingContainer is System.Web.UI.WebControls.DataGridItem && this.MsgTextField != null && this.MsgTextField != "" ) 
			{
				DataGridItem dataItem = (System.Web.UI.WebControls.DataGridItem)this.NamingContainer;
				if ( dataItem.DataItem != null) 					
				{
					_builder = new MessageControlBuilder();
					_message = dataItem.DataItem;
					this.MessageBind();
				}
			}
			base.DataBind ();
		}

		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding (e);
			//Verifico se faço a formatação do texto sem bind
			if ( this.ParseText && this.Text != string.Empty && this.Text != "") 
			{
				CustomFormatHelper formatString = new CustomFormatHelper( _customFormat, true );
				this.Text = formatString.GetCurrentFormatString( this.Text );
				_parseTextComplete = true;
			}
		}
	}

	//TODO: Comentário
	public class LabelDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			string designTimeHtml = base.GetDesignTimeHtml ();
			MsgLabel label = (MsgLabel)this.Component;
			
			string _value = "{" + label.MsgTextField + "}";

			designTimeHtml =
				designTimeHtml.Insert(designTimeHtml.Length - 7, _value);

			return designTimeHtml;
		}
	}
}
