using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using SuperPag.Framework;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls
{
	/// <summary>
	///	TODO: Comentário
	/// </summary>	
	[
	Designer(typeof(DropDownListDesigner)),
	ToolboxData("<{0}:MsgDropDownList runat=server></{0}:MsgDropDownList>")]
	public class MsgDropDownList : System.Web.UI.WebControls.DropDownList, MessageControl
	{
		#region Propriedades e Eventos

		private string _msgListItemsSource;
		private string _msgSource;
		private string _msgListItemTextField;
		private string _formatString;
		private string _msgListItemValueField;
		private string _msgSelectedValueField;
		private string _msgEnabledField;
		private string _msgVisibleField;
		private string _msgListItemsSourceKey = "";
		private string _msgSourceKey = "";
		private bool _unbindEntireMessage = false;
		private bool _inputMandatory;
        private bool _firstBlank = true;
        private string _alternateSelectedValue = "";
		private string _msgSourceField = "";
		private CustomFormatHelper.CustomFormatEnum _customFormat = CustomFormatHelper.CustomFormatEnum.None;
		private bool _sort;
		private Hashtable hsSort;
		private string _DefaultButton = "";
		RequiredFieldValidator _req;

		Message _message;
		object _messageListItems;
		static MessageControlBuilder _builder = new MessageControlBuilder();

		
		private ListSourceType _listSourceType = ListSourceType.Enumeration;
		public enum ListSourceType 
		{
			Enumeration = 1,
			Message = 2
		}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public ListSourceType MsgListSourceType
		{get{return _listSourceType;}set{_listSourceType = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSource
		{get{return _msgSource;}set{_msgSource = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSelectedValueField
		{get{return _msgSelectedValueField;}set{_msgSelectedValueField = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgEnabledField
		{get{return _msgEnabledField;}set{_msgEnabledField = value;}}
		
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgVisibleField
		{get{return _msgVisibleField;}set{_msgVisibleField = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgListItemsSource
		{get{return _msgListItemsSource;}set{_msgListItemsSource = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgListItemTextField
		{get{return _msgListItemTextField;}set{_msgListItemTextField = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgListItemValueField
		{get{return _msgListItemValueField;}set{_msgListItemValueField = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public bool UnBindEntireMessage
		{get{return _unbindEntireMessage;}set{_unbindEntireMessage = value;}}
		
		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public string FormatString
		{get{return _formatString;}set{_formatString = value;}}

		//Message Source Key
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceKey
		{get{return _msgSourceKey;}set{_msgSourceKey = value;}}

		//Message Source Key
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgListItemsSourceKey
		{get{return _msgListItemsSourceKey;}set{_msgListItemsSourceKey = value;}}

		//Input Mandatory
		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public bool InputMandatory
		{get{return _inputMandatory;}set{_inputMandatory = value;}}

        //First Blank
        [Bindable(true), Category("Message Format"), DefaultValue("")]
        public bool FirstBlank
        { get { return _firstBlank; } set { _firstBlank = value; } }

		//Alternate SelectedValue
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string AlternateSelectedValue 
		{get{return _alternateSelectedValue;}set{_alternateSelectedValue = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceField
		{get{return _msgSourceField;}set{_msgSourceField = value;}}

		//TODO: Comentário
		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind AfterMessageBind;

		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind BeforeMessageBind;

		//Custom Format
		[Bindable(true), Category("Message Format"), DefaultValue("")]
		public CustomFormatHelper.CustomFormatEnum CustomFormat 
		{get{return _customFormat;} set {_customFormat = value;}}

		//Custom Format
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public bool Sort 
		{get{return _sort;} set {_sort = value;}}

		//Define qual o botão será acionado quando teclar o ENTER
		[Bindable(true), Category("Action Button"), DefaultValue(""),
		Editor(typeof(ListControlsUIEditor), 
			typeof(System.Drawing.Design.UITypeEditor))]
		public string DefaultButton
		{get{return _DefaultButton;}set{_DefaultButton = value;}}

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

		public void InitMessage()
		{
			MessageInit();
		}

		public void MessageInit()
		{
			if(this.Page is MessagePage)
			{
				MessagePage page = (MessagePage)this.Page;

				//Message source
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

				//Message List Item Source
				if(_msgListItemsSource != String.Empty && _msgListItemsSource != null)
				{
					switch(_listSourceType)
					{
						case ListSourceType.Message:
							if(_msgListItemsSourceKey != "")
							{
								_messageListItems = page.Messages.GetArray(_msgListItemsSourceKey);
							} 
							else
							{
								_messageListItems = page.Messages.GetArray(_msgListItemsSource);
							}

							//Limpo mensagem do contexto para nao haver conflito com informações de outras telas
							if ( _messageListItems is MessageCollection )
							{
								Type t = _builder.GetTypeElementMessage( (MessageCollection)_messageListItems );
								((MessagePage)this.Page).Messages.ClearDropDownMessage( t );
							}
							else 
							{
								((MessagePage)this.Page).Messages.ClearDropDownMessage( _msgListItemsSource, _msgSourceKey );
							}

							break;
		                        
						case ListSourceType.Enumeration:
							if(_msgListItemsSourceKey != "")
							{
								_messageListItems = page.Messages.GetEnum("Enum" + _msgListItemsSourceKey);
							} 
							else
							{
								_messageListItems = page.Messages.GetEnum("Enum" + _msgListItemsSource);
							}
							break;
					}
				}	
			}			
		}

		#endregion Message Init


		//TODO: Comentário
		protected override ControlCollection CreateControlCollection()
		{
			return new ControlCollection(this);
		}

		//TODO: Comentário
		protected override void CreateChildControls()
		{
			if(_inputMandatory && this.Enabled)
			{
				_req = new RequiredFieldValidator();
				_req.ControlToValidate = this.ID;
				_req.EnableClientScript = true;
				_req.ErrorMessage = "&nbsp;(*)";
				_req.Display = ValidatorDisplay.Dynamic;
				Controls.Add(_req);
			}
		}

		//Data Bind
		public override void DataBind()
		{	
			base.DataBind();
			MessageBind();					
		}
	
		//Message Bind
		public void MessageBind()
		{
			OnBeforeMessageBind(_message);
			
			if(_messageListItems != null)
			{
				BindItens();
			}
			
			if(_message != null)
			{
				BindSelectedValue();
			} 
			else if (_alternateSelectedValue != null && _alternateSelectedValue != "") 
			{
				this.SelectedValue = _alternateSelectedValue;
			}

			OnAfterMessageBind(_message);
		}
		

		//TODO: Comentário
		private void BindItens()
		{
			this.Items.Clear();

            //Adiciona um item em branco no combo
            if (_firstBlank)
    			this.Items.Add(new ListItem("", ""));
			
			ListItem[] _listItem = null;

			switch(_listSourceType)
			{
				case ListSourceType.Message:
					_listItem = GetItems( _msgListItemTextField, _msgListItemValueField, (MessageCollection)_messageListItems );
					break;
				case ListSourceType.Enumeration:
					_listItem = GetItems( "Text", "Key", (System.Array)_messageListItems );
					break;
			}

			if ( _listItem != null )
			{
				if ( !_sort )
					this.Items.AddRange( _listItem );
				else 
				{
					Array.Sort(_listItem, new OrdenaListaComparer());
					this.Items.AddRange( _listItem );
				}
			}
		}

		//TODO: Comentário
		private ListItem[] GetItems(string text, string _value, MessageCollection array)
		{
			if ( array == null || array.Count == 0 ) return null;

			ListItem[] _listItem = new ListItem[array.Count];
			
			if ( _sort ) hsSort = new Hashtable();

			for( int i=0; i<array.Count; i++)
			{
				object _object = array[i];

				//Verifica se o objeto do array não é nulo
				if (_object != null) 
				{
					string itemText = "";
				
					string itemValue = 
						_builder.GetPropertyValue(_object, _value, _formatString, false);

					switch(_customFormat)
					{
						case CustomFormatHelper.CustomFormatEnum.None:
						{
							itemText = 
								_builder.GetPropertyValue(_object, text, _formatString, true);
							break;
						}
						default: 
						{
							string propertyValue = 
								_builder.GetPropertyValue(_object, text, "{0}", true );

							if ( propertyValue != null && propertyValue != "" ) 
							{
								CustomFormatHelper formatString = new CustomFormatHelper( _customFormat, true );
								itemText = formatString.GetCurrentFormatString( propertyValue );
							} 
							else
								itemText = propertyValue;

							break;
						}
					}
				
					_listItem[i] = new ListItem(itemText, itemValue);

					if ( _sort ) hsSort.Add( itemValue, i);
				}
			}

			return _listItem;
		}


		private ListItem[] GetItems(string text, string _value, System.Array array)
		{
			if ( array == null || array.Length == 0 ) return null;

			ListItem[] _listItem = new ListItem[array.Length];
			
			if ( _sort ) hsSort = new Hashtable();

			for( int i=0; i<array.Length; i++)
			{
				object _object = array.GetValue(i);

				//Verifica se o objeto do array não é nulo
				if (_object != null) 
				{
					string itemText = "";
				
					string itemValue = 
						_builder.GetPropertyValue(_object, _value, _formatString, false);

					switch(_customFormat)
					{
						case CustomFormatHelper.CustomFormatEnum.None:
						{
							itemText = 
								_builder.GetPropertyValue(_object, text, _formatString, true);
							break;
						}
						default: 
						{
							string propertyValue = 
								_builder.GetPropertyValue(_object, text, "{0}", true );

							if ( propertyValue != null && propertyValue != "" ) 
							{
								CustomFormatHelper formatString = new CustomFormatHelper( _customFormat, true );
								itemText = formatString.GetCurrentFormatString( propertyValue );
							} 
							else
								itemText = propertyValue;

							break;
						}
					}
				
					_listItem[i] = new ListItem(itemText, itemValue);

					if ( _sort ) hsSort.Add( itemValue, i);
				}
			}

			return _listItem;
		}



		//TODO: Comentário
		private void BindSelectedValue()
		{
			string _value;

			//Se for para fazer o unbind na mensagem inteira
			if(this.UnBindEntireMessage == true)
			{
				Message m = (Message)_builder.GetObjectProperty(_message, _msgSelectedValueField, false, false);
				_value = _builder.GetPropertyValue(m, this.MsgListItemValueField);
			}
			else
			{
				//Selected Value
				_value = 
					_builder.GetPropertyValue(_message, _msgSelectedValueField, _formatString, false);
			}
			
			ListItem item = base.Items.FindByValue(_value);
			if(item != null)
			{
				base.ClearSelection();
				item.Selected = true;
			}
		}

		private Message _selectedMessage;
		public Message SelectedMessage
		{
			get { return _selectedMessage; } 
		}

		//TODO: Comentário
		public void UnBindSelectedMessage()
		{
			//Seta o objeto selecionado no contexto
			if ( _listSourceType == ListSourceType.Message && _messageListItems != null && _messageListItems is MessageCollection)
			{
				bool clearMessage = true;

				if ( !_sort) 
				{
					int diff = this.Items.Count - ((MessageCollection)_messageListItems).Count;
					if ( this.SelectedIndex - diff > -1 ) 
					{
						object item = ((MessageCollection)_messageListItems)[ this.SelectedIndex - diff ];
						_selectedMessage = (Message)item;
						if ( _msgSourceKey != "") 
						{
							((MessagePage)this.Page).Messages.AddSelectedDropDownListMessage((Message)item, this._msgListItemsSource, this._msgSourceKey);
						} 
						else 
						{
							((MessagePage)this.Page).Messages.AddSelectedDropDownListMessage((Message)item);
						}
						clearMessage = false;
					}						
				} 
				else if ( hsSort != null && hsSort.ContainsKey( this.SelectedValue ) ) 
				{					
					object item = ((MessageCollection)_messageListItems)[ (int)hsSort[this.SelectedValue] ];
					_selectedMessage = (Message)item;
					if ( _msgSourceKey != "") 
					{
						((MessagePage)this.Page).Messages.AddSelectedDropDownListMessage((Message)item, this._msgListItemsSource, this._msgSourceKey );
					} 
					else 
					{
						((MessagePage)this.Page).Messages.AddSelectedDropDownListMessage((Message)item);
					}
					clearMessage = false;
				}
				
				if ( clearMessage ) 
				{
					if ( _msgSourceKey != "") 
					{
						((MessagePage)this.Page).Messages.ClearDropDownMessage( this._msgListItemsSource, this._msgSourceKey );
					} 
					else 
					{
						((MessagePage)this.Page).Messages.ClearDropDownMessage( _builder.GetTypeElementMessage( (MessageCollection)_messageListItems ) );
					}
				}
			} 
		}


		//TODO: Comentário
		public void MessageUnBind()
		{			
			if (!this.Visible ||
				this.SelectedItem == null || 
				this.SelectedItem.Value == null ||
				(this.InputMandatory && this.SelectedItem.Value == "") )
				return;
			
			this.UnBindSelectedMessage();			

			if(this._listSourceType == ListSourceType.Enumeration) 
			{
				if(this.SelectedItem.Value == "") { this.SelectedItem.Value = int.MinValue.ToString(); }
			}

			if(_unbindEntireMessage)
			{
				Helper.SetUndBindPropetyValue( _message, this.MsgSelectedValueField, _selectedMessage);
			}
			else
			{
				Helper.SetUndBindPropetyValue( _message, this.MsgSelectedValueField, this.SelectedItem.Value );
			}
		}
		
		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState (savedState);
			if ( _sort && ViewState["__HashIndices"] is Hashtable ) 
			{
				hsSort = (Hashtable)ViewState["__HashIndices"];
			}
			if ( ViewState["__InputMandatory"] != null ) 
			{
				this.InputMandatory = (bool)ViewState["__InputMandatory"];
			}
		}

		protected override object SaveViewState()
		{
			if ( _sort ) 
			{
				ViewState["__HashIndices"] = hsSort;
			}
			ViewState["__InputMandatory"] = this.InputMandatory;
			return base.SaveViewState ();
		}

		//TODO: Comentário
		protected override void Render(HtmlTextWriter writer)
		{
			if ( Ensure.StringIsNotEmpty( this.DefaultButton ) )
				this.Attributes.Add("onkeydown", 
					"fnTrapKD( " + this.DefaultButton + ",event);" );

			base.Render (writer);
			if(_req != null)
			{
				_req.RenderControl(writer);
			}
		}

		//é usado pelo repeater
		public void MessageDataSource(object message) 
		{
			if ( message is MessageCollection )
				_messageListItems = (MessageCollection)message;
			else if ( message is Message )
				_message = (Message)message;
		}

		//ordena lista
		internal class OrdenaListaComparer: IComparer
		{
			public int Compare(object x, object y)
			{
				if (x is ListItem && y is ListItem)
				{
					string TextX = ((ListItem) x).Text;
					string TextY = ((ListItem) y).Text;
					return TextX.CompareTo(TextY);				
				}
				
				throw new ArgumentException("array inválido");
			}
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			if ( this.AutoPostBack ) 
			{
				this.MessageInit();
				this.UnBindSelectedMessage();
			}

			base.OnSelectedIndexChanged (e);
		}

	}

	//TODO: Comentário
	public class DropDownListDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			string designTimeHtml = base.GetDesignTimeHtml ();
			MsgDropDownList ddl = (MsgDropDownList)this.Component;
			
			designTimeHtml += "{" + ddl.MsgSelectedValueField + "}";

			int position = designTimeHtml.LastIndexOf("</select>");

			
			designTimeHtml = designTimeHtml.Insert(position, "<option>{" + ddl.MsgListItemsSource + "}</option>");
						

			if(ddl.InputMandatory)
			{
				return designTimeHtml + "&nbsp;(*)";
			}
			else
			{
				return designTimeHtml + "";
			}			
		}
	}
}
