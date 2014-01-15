using System;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using SuperPag.Framework;
using SuperPag.Framework.Helper;


namespace SuperPag.Framework.Web.WebControls
{
	/// <summary>
	///	TODO: Comentário
	/// </summary>	
	[DefaultEvent("MessageEvent"),
	ToolboxData("<{0}:MsgDataGrid runat=server></{0}:MsgDataGrid>")]
	public class MsgDataGrid : System.Web.UI.WebControls.DataGrid, MessageControl
	{
		#region Propriedades e Eventos

		private string _msgSource;
		private string _msgSourceKey = "";
		private string _msgSourceField = "";		
		private MessageCollection _messageArray; //private System.Array _messageArray;
		private static MessageControlBuilder _builder = new MessageControlBuilder();
		private bool _ShowGoToPage;

		//ShowGoToPage
		[Bindable(true), Category("Appearance"), DefaultValue(false)]
		public bool ShowGoToPage
		{get{return _ShowGoToPage;}set{_ShowGoToPage = value;}}

		//Message Source
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSource
		{
			get { return _msgSource; }
			set { _msgSource = value; }
		}
        
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceField
		{get{return _msgSourceField;}set{_msgSourceField = value;}}

		//Message Source Key
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceKey
		{get{return _msgSourceKey;}set{_msgSourceKey = value;}}

		//TODO: Comentário
		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind AfterMessageBind;

		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind BeforeMessageBind;

		//TODO: Codigo duplicado
		private void UnBindDropDownListColumn(int iColumnIndex, MessageCollection messageArray)
		{
			DropDownListColumn dropDownListColumn = (DropDownListColumn)this.Columns[iColumnIndex];
					
			for(int j = 0; j < this.Items.Count;j++)
			{

				DataGridItem item = (DataGridItem)this.Items[j];
				object _message = messageArray[j];

				string _selectedValue =
					((MsgDropDownList)item.Cells[iColumnIndex].Controls[0]).SelectedValue;

				Helper.SetUndBindPropetyValue(_message, dropDownListColumn.SelectedValueField, _selectedValue);
			}	
		}

		//TODO: Comentário
		private void UnBindCheckBoxColumn(int iColumnIndex, MessageCollection messageArray)
		{			
			CheckBoxColumn checkBoxColumn = (CheckBoxColumn)this.Columns[iColumnIndex];
					
			for(int j = 0; j < this.Items.Count;j++)
			{
				DataGridItem item = (DataGridItem)this.Items[j];

				BindingFlags _fl = BindingFlags.Instance | BindingFlags.Public;

				object _message = messageArray[j];
				PropertyInfo[] properties = 
					_message.GetType().GetProperties( _fl );

				if( null != properties )
				{
					//TODO: Codigo duplicado
					foreach(PropertyInfo p in properties)
					{
						if(p.Name == checkBoxColumn.CheckedField)
						{
							if(p.PropertyType == typeof(Tristate))
							{
								Tristate _checked =
									((CheckBox)item.Cells[iColumnIndex].Controls[0]).Checked;

								p.SetValue(_message, _checked, null);
							} 
							else if(p.PropertyType == typeof(System.Boolean))
							{
								p.SetValue(_message, Convert.ToBoolean(((CheckBox)item.Cells[iColumnIndex].Controls[0]).Checked), null);
							}
						}
					}
				}
			}			
		}

		//TODO: Comentário
		public delegate void MsgDataGridMessageEventHandler(
			object sender, 
			string eventName,
			Message message);

		//TODO: Comentário
		//Message Source
		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MsgDataGridMessageEventHandler MessageEvent;
	
		//TODO: Comentário
		public void OnMessageEvent(string eventName, Message message)
		{
			if(MessageEvent != null)
			{
				MessageEvent(this, eventName, message);
			}
		}

		
		//subo o evento
		public void OnAfterMessageBind(object message)
		{
			if(AfterMessageBind != null)
			{
				AfterMessageBind(this, message);
			}
			if(this.PageCount > 1)
			{
				CreatePager();				
			}
		}


		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState (savedState);
			CreatePager();
		}
		
		private void CreatePager()
		{
			if ( !this.AllowPaging ) return;

			if ( this.PageCount > 0 && this.Items.Count > 0 )
			{
				int intTamanho = 0;

				HtmlTable table = new HtmlTable();
			
				table.ID = "tbDataGridFooter";
				table.CellSpacing = 0;
				table.Attributes.Add("class", "tbPagerSUM");
				table.Attributes.Add("style", "border-collapse:collapse;");

				HtmlTableRow tr = new HtmlTableRow();
				
				//Cell´s
				HtmlTableCell cellFirst = new HtmlTableCell();
				HtmlTableCell cellPrevious = new HtmlTableCell();

				#region Button´s First and Previous

				if( this.CurrentPageIndex != 0 )
				{
					ImageButton imgFirst = new ImageButton();
					imgFirst.CausesValidation = false;
					imgFirst.ImageUrl = "~/App_Themes/default/images/Seta_Primeiro.gif";
					imgFirst.Click += new ImageClickEventHandler(imgFirst_Click);
					cellFirst.Controls.Add(imgFirst);
				
					ImageButton imgPrevious = new ImageButton();
					imgPrevious.CausesValidation = false;
					imgPrevious.Click += new ImageClickEventHandler(imgPrevious_Click);
                    imgPrevious.ImageUrl = "~/App_Themes/default/images/Seta_Anterior.gif";
					cellPrevious.Controls.Add(imgPrevious);

					intTamanho += 40;
				}
				else
				{
					Image imgFirst = new Image();
                    imgFirst.ImageUrl = "~/App_Themes/default/images/Seta_Primeiro.gif";
					imgFirst.Style.Add("filter", "progid:DXImageTransform.Microsoft.BasicImage(Rotation=0,Mirror=0,Invert=0,XRay=0,Grayscale=1,Opacity=0.80)");
					cellFirst.Controls.Add(imgFirst);

					Image imgPrevious = new Image();
                    imgPrevious.ImageUrl = "~/App_Themes/default/images/Seta_Anterior.gif";
					imgPrevious.Style.Add("filter", "progid:DXImageTransform.Microsoft.BasicImage(Rotation=0,Mirror=0,Invert=0,XRay=0,Grayscale=1,Opacity=0.80)");
					cellPrevious.Controls.Add(imgPrevious);
				}

				//Set width and align of cell
				cellFirst.Width = "20";
				cellFirst.Align = "center";

				cellPrevious.Width = "20";
				cellPrevious.Align = "center";

				tr.Cells.Add(cellFirst);
				tr.Cells.Add(cellPrevious);

				#endregion

				#region Button´s Next and Last

				HtmlTableCell cellNext = new HtmlTableCell();
				HtmlTableCell cellLast = new HtmlTableCell();

				if(this.CurrentPageIndex < this.PageCount - 1)
				{
					ImageButton imgNext = new ImageButton();
					imgNext.CausesValidation = false;
                    imgNext.ImageUrl = "~/App_Themes/default/images/Seta_Proximo.gif";
					imgNext.Click += new ImageClickEventHandler(imgNext_Click);
					cellNext.Controls.Add(imgNext);

					ImageButton imgLast = new ImageButton();
					imgLast.CausesValidation = false;
					imgLast.Click += new ImageClickEventHandler(imgLast_Click);
                    imgLast.ImageUrl = "~/App_Themes/default/images/Seta_Ultimo.gif";
					cellLast.Controls.Add(imgLast);

					intTamanho += 40;
				}
				else
				{
					Image imgNext = new Image();
                    imgNext.ImageUrl = "~/App_Themes/default/images/Seta_Proximo.gif";
					imgNext.Style.Add("filter", "progid:DXImageTransform.Microsoft.BasicImage(Rotation=0,Mirror=0,Invert=0,XRay=0,Grayscale=1,Opacity=0.80)");
					cellNext.Controls.Add( imgNext );

					Image imgLast = new Image();
                    imgLast.ImageUrl = "~/App_Themes/default/images/Seta_Ultimo.gif";
					imgLast.Style.Add("filter", "progid:DXImageTransform.Microsoft.BasicImage(Rotation=0,Mirror=0,Invert=0,XRay=0,Grayscale=1,Opacity=0.80)");
					cellLast.Controls.Add( imgLast );
				}
				
				//Set width and align of cell
				cellNext.Width = "20";
				cellNext.Align = "center";

				cellLast.Width = "20";
				cellLast.Align = "center";
				
				tr.Cells.Add(cellNext);
				tr.Cells.Add(cellLast);

				table.Width = "100%";

				#endregion

				#region Page Number´s

				HtmlTableCell tcDeAte = new HtmlTableCell();

				int currentePage = this.CurrentPageIndex + 1;
				tcDeAte.InnerText = String.Format("{0} de {1}", currentePage, this.PageCount);
				//tcDeAte.Attributes.Add("class", "tdSubTituloSNZ");
				tcDeAte.Align = "Right";
				tcDeAte.Width = "90%";
				tr.Cells.Add(tcDeAte);

				#endregion

				#region ShowGoToPage 

				if ( _ShowGoToPage  ) 
				{
					HtmlTableCell tcGoTo = new HtmlTableCell();
					tcGoTo.Align = "Right";

					MsgTextBox txtGoTo = new MsgTextBox();
					txtGoTo.ID = "txtGoTo";
					txtGoTo.Width = System.Web.UI.WebControls.Unit.Pixel(30);
					txtGoTo.MaxLength = 3;
					txtGoTo.InputMandatory = true;
					txtGoTo.CustomFormat = CustomFormatHelper.CustomFormatEnum.NumInteiro;
					txtGoTo.InputFilter = MsgCustomValidators.FilterEnum.NumInteiro;
					txtGoTo.CssChildsControl = "spanValidationLST";

					ImageButton imgGoTo = new ImageButton();
					imgGoTo.ID = "imgGoTo";
					imgGoTo.CausesValidation = true;
					imgGoTo.ImageUrl = "~/Recursos/Imagens/Seta_Proximo.gif";
					imgGoTo.Click += new ImageClickEventHandler(imgGoTo_Click);
					imgGoTo.ImageAlign = ImageAlign.AbsMiddle;
					//TODO: fazer multi lang
					imgGoTo.ToolTip = "Ir para";

					RangeValidator rvGoTo = new RangeValidator();
					rvGoTo.ControlToValidate = "txtGoTo";
					rvGoTo.MinimumValue = 1.ToString();
					rvGoTo.MaximumValue = this.PageCount.ToString();
					rvGoTo.ErrorMessage = "(*)";
					rvGoTo.Type = ValidationDataType.Integer;
					rvGoTo.Display = ValidatorDisplay.Dynamic;
					rvGoTo.CssClass = "spanValidationLST";

					Literal litEspaco = new Literal();
					litEspaco.Text = "&nbsp" ;

					tcGoTo.Controls.Add(txtGoTo);
					tcGoTo.Controls.Add(rvGoTo);
					tcGoTo.Controls.Add(litEspaco);
					tcGoTo.Controls.Add(imgGoTo);
					
					tr.Cells.Add(tcGoTo);
				}

				#endregion
				
				table.Rows.Add(tr);
			
				//Só coloca a table de paginação se existir páginas
				this.Controls.Add(table);
			}
		}

		private void imgNext_Click(object sender, ImageClickEventArgs args)
		{
			this.CurrentPageIndex += 1;
			this.MessageInit();
			this.MessageBind();
		}

		private void imgLast_Click(object sender, ImageClickEventArgs args)
		{
			this.CurrentPageIndex = this.PageCount - 1;
			this.MessageInit();
			this.MessageBind();
		}

		private void imgPrevious_Click(object sender, ImageClickEventArgs args)
		{
			this.CurrentPageIndex -= 1;
			this.MessageInit();
			this.MessageBind();
		}

		private void imgFirst_Click(object sender, ImageClickEventArgs args)
		{
			this.CurrentPageIndex = 0;
			this.MessageInit();
			this.MessageBind();
		}

		private void imgGoTo_Click(object sender, ImageClickEventArgs args)
		{
			HtmlTable tbDataGridFooter = (HtmlTable)this.FindControl( "tbDataGridFooter" );
			MsgTextBox txtGoTo = (MsgTextBox)tbDataGridFooter.Rows[0].FindControl("txtGoTo");
			this.CurrentPageIndex = Convert.ToInt32( txtGoTo.Text ) - 1;
			this.MessageInit();
			this.MessageBind();
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

		#region MessageInit
		public void MessageInit()
		{
			if(_messageArray != null) return;
			object messageArray;
			if(this.Page is MessagePage)
			{
				MessagePage page = (MessagePage)this.Page;

				if(_msgSourceKey == "")
				{
					if(_msgSourceField == "")
					{
						messageArray = page.Messages.GetArray(_msgSource);
					} 
					else
					{
						object source = page.Messages.Get(_msgSource);
						messageArray = _builder.GetObjectProperty( source, _msgSourceField, false, false );
					}
				}
				else
				{
					if(_msgSourceField == "")
					{
						messageArray = page.Messages.GetArray(_msgSourceKey);
					} 
					else
					{
						object source = page.Messages.Get(_msgSourceKey);
						messageArray = DataBinder.GetPropertyValue(source, _msgSourceField);
					}
				}
			} 
			else
			{
				throw new ApplicationException("The MsgDataGrid must be placed inside a MessagePage ");
			}

			//RAFAEL:
			//if(messageArray != null && !(messageArray is System.Array))
			if(messageArray != null && !(messageArray is MessageCollection))
			{
				throw new ApplicationException("The MessageSource must be 'System.Array' type.");
			}

			//this._messageArray = (System.Array)messageArray;
			this._messageArray = (MessageCollection)messageArray;
		}
		#endregion MessageInit

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			IEnumerator eNumerator = this.Columns.GetEnumerator();
			while(eNumerator.MoveNext())
			{
				if(eNumerator.Current is PopUpColumn)
				{
					if(Page is MessagePage)
					{
						((MessagePage)Page).RegisterPopUpScript();
						((MessagePage)Page).RegisterClientScriptBlock("TitlePopUp", "<script>window.parent.document.title = document.title</script>");
					}
					((MessagePage)Page).ClearBeforePersitMemory = false;
				}
			}			
		}
	

		//TODO: Comentário
		public void MessageBind()
		{
			OnBeforeMessageBind(_messageArray);
			
			if(_messageArray != null)
			{
				base.DataSource = _messageArray;
				base.DataBind();
			} 
			OnAfterMessageBind(_messageArray);
		}

		//TODO: Comentário
		public void MessageUnBind()
		{
			for(int i = 0; i < this.Columns.Count;i++)
			{
				if(this.Columns[i] is CheckBoxColumn)
				{
					UnBindCheckBoxColumn(i, _messageArray);
				} 
				else if(this.Columns[i] is TextBoxColumn)
				{
					UnBindTextBoxColumn(i, _messageArray);
				}
				else if(this.Columns[i] is DropDownListColumn)
				{
					UnBindDropDownListColumn(i, _messageArray);
				}
			}
		}

		//TODO: Comentátio
		protected override void OnItemCommand(DataGridCommandEventArgs e)
		{
			base.OnItemCommand(e);
			
			MessageInit();
			
            //TODO: criar metodo específico para sort 
            //Rafael Gibrail - 08/junho/2006: Fiz o if abaixo pois estou usando o Sort do grid converncional e com clica no header não existe item selecionado
            if (e.Item.ItemIndex >= 0)
            {
                object item = _messageArray[this.CurrentPageIndex * this.PageSize + e.Item.ItemIndex];

                ((MessagePage)this.Page).Messages.Add((Message)item);

                OnMessageEvent(e.CommandName, (Message)item);
            }

			RaiseBubbleEvent(this, e);
		}

		protected override void OnItemDataBound(DataGridItemEventArgs e)
		{
			IEnumerator cols = this.Columns.GetEnumerator();

			bool printPreview = false;
			if(Context != null)
			{
				if(Context.Items["__PRINT"] is bool)
				{
					printPreview = (bool)Context.Items["__PRINT"];
				}
			}
			

			int i = -1;
			while(cols.MoveNext())
			{
				i++;

				if(cols.Current is MessageColumn)
				{
					((MessageColumn)cols.Current).MessageBind(e, _builder);
				} 

				if(cols.Current is NoPrintTemplateColumn)
				{
					if(printPreview)
					{
						e.Item.Cells[i].Attributes.Add("class", "Invisible");
					}
				}
				else if(cols.Current is EventColumn)
				{
					if(printPreview)
					{
						e.Item.Cells[i].Style.Add("display", "none");
					}
					((EventColumn)cols.Current).MessageBind(e, _builder);
				} 
				else if(cols.Current is CheckBoxColumn)
				{
					((CheckBoxColumn)cols.Current).MessageBind(e, _builder);
				}
				else if(cols.Current is DropDownListColumn)
				{
					((DropDownListColumn)cols.Current).MessageBind(e, _builder);
				}
				else if(cols.Current is TextBoxColumn)
				{
					((TextBoxColumn)cols.Current).MessageBind(e, _builder);
				}
			}
			base.OnItemDataBound (e);
		}

		//é usado pelo repeater
		public void MessageDataSource(object message) 
		{
			if(_msgSourceField  != "")
			{
				object source = 
					DataBinder.GetPropertyValue(message, _msgSourceField);
				_messageArray = (MessageCollection)source;
			} 
			else
			{
				_messageArray = (MessageCollection)message;
			}
		}

		//TODO: Comentário
		private void UnBindTextBoxColumn(
			int iColumnIndex, 
			MessageCollection messageArray)
		{
			TextBoxColumn textBoxColumn = (TextBoxColumn)this.Columns[iColumnIndex];
					
			for(int j = 0; j < this.Items.Count;j++)
			{
				DataGridItem item = (DataGridItem)this.Items[j];

				object _message = messageArray[j];

				string _text =
					((TextBox)item.Cells[iColumnIndex].Controls[0]).Text;

				Helper.SetUndBindPropetyValue(_message, textBoxColumn.Field, _text);
			}		
		}

		protected override void PrepareControlHierarchy()
		{
			base.PrepareControlHierarchy();
		}
	}

	public class TextBoxColumn : System.Web.UI.WebControls.DataGridColumn 
	{
		private string _formatString = "";
		private bool _inputMandatory;
		private int _maxLength;
		private string _field = "";
		private MsgCustomValidators.FilterEnum _inputFilter;
		private string _enabledField = "";
		private string _visibleField = "";
		private bool _notEnabled;
		private bool _notVisible;
		
		private CustomFormatHelper.CustomFormatEnum _customFormatString = CustomFormatHelper.CustomFormatEnum.None;

		private ArrayList _list = new ArrayList();

		public bool InputMandatory
		{get{return _inputMandatory;}set{_inputMandatory = value;}}

		public string Field
		{get{return _field;}set{_field = value;}}


		public int MaxLength
		{get{return _maxLength;}set{_maxLength = value;}}

		public string FormatString
		{get{return _formatString;}set{_formatString = value;}}

		public string EnabledField
		{get{return _enabledField;}set{_enabledField = value;}}
		
		public string VisibleField
		{get{return _visibleField;}set{_visibleField = value;}}

		public bool NotEnabled
		{get{return _notEnabled;}set{_notEnabled = value;}}
		
		public bool NotVisible
		{get{return _notVisible;}set{_notVisible = value;}}

		//Input filter
		public MsgCustomValidators.FilterEnum InputFilter
		{get{return _inputFilter;}set{_inputFilter = value;}}

		//Custom Format String
		public CustomFormatHelper.CustomFormatEnum CustomFormat
		{get{return _customFormatString;}set{_customFormatString = value;}}

		//Message Bind
		internal void MessageBind(DataGridItemEventArgs e, MessageControlBuilder builder)
		{
			ListItemType itemType = e.Item.ItemType;
			if(itemType == ListItemType.AlternatingItem || itemType == ListItemType.Item)
			{
				int columnIndex = this.Owner.Columns.IndexOf((DataGridColumn)this);
	
				if ( (!base.DesignMode) )
				{
					object dataItem = e.Item.DataItem;
					if(e.Item.DataItem != null)
					{
						MsgTextBox textBox = new MsgTextBox();

						string text = "";
						if(_field != "")
						{						
							switch(_customFormatString)
							{
								case CustomFormatHelper.CustomFormatEnum.None:
								{
									text = 
										builder.GetPropertyValue(dataItem, _field, _formatString, true);
									textBox.Text = text;
									break;
								}
								default: 
								{
									string propertyValue = 
										builder.GetPropertyValue(dataItem, _field, "{0}", true );

									if ( propertyValue != null && propertyValue != "" ) 
									{
										CustomFormatHelper formatString = new CustomFormatHelper( _customFormatString );
										text = formatString.GetCurrentFormatString( propertyValue );
										textBox.Text = text;
									} 
									else
										textBox.Text = "";

									break;
								}
							}
						}

						bool _enabled = true;
						if(_enabledField != "")
						{
							_enabled =
								Convert.ToBoolean(builder.GetPropertyValue(dataItem, _enabledField, false));
							textBox.Enabled = _notEnabled ? ! _enabled :  _enabled;
						}

						bool _visible = true;
						if(_visibleField != "")
						{
							_visible =
								Convert.ToBoolean(builder.GetPropertyValue(dataItem, _visibleField, false));
							textBox.Visible = _notVisible ? _visible : ! _visible;
						}
						
						if(_inputFilter != MsgCustomValidators.FilterEnum.None)
							textBox.InputFilter = _inputFilter;

						if ( _customFormatString != CustomFormatHelper.CustomFormatEnum.None ) 
							textBox.CustomFormat = _customFormatString;

						textBox.FormatString = _formatString;
						textBox.MaxLength = _maxLength;
						textBox.InputMandatory = _inputMandatory;
						
						textBox.ID = "ctl_" + columnIndex + "_" + e.Item.ItemIndex.ToString();

						object[] property = {textBox.Text, textBox.Enabled, textBox.Visible, textBox.InputFilter, textBox.FormatString, textBox.MaxLength, textBox.InputMandatory, textBox.ID, textBox.CustomFormat};

						_list.Add(property);
						e.Item.Cells[columnIndex].Controls.Add(textBox);
					}
				} 
				else 
				{
					TextBox textBox = new TextBox();
					textBox.Text = "{" + _field + ":" + _customFormatString.ToString()  + "}";
					e.Item.Cells[columnIndex].Controls.Add(textBox);
				}				
			}
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState (savedState);
			_list = (ArrayList)ViewState["__TextBoxList"];

			for(int i = 0; i < this.Owner.Items.Count; i++)
			{
				DataGridItem item = (DataGridItem)this.Owner.Items[i];
				
				MsgTextBox textBox = new MsgTextBox();
				object[] property = (object[])_list[i];
				textBox.Text = (string)property[0];
				textBox.Enabled = (bool)property[1];
				textBox.Visible = (bool)property[2];
				textBox.InputFilter = (MsgCustomValidators.FilterEnum)property[3];
				textBox.FormatString = (string)property[4];
				textBox.MaxLength = (int)property[5];
				textBox.InputMandatory = (bool)property[6];
				textBox.ID = (string)property[7];
				textBox.CustomFormat = (CustomFormatHelper.CustomFormatEnum)property[8];
				item.Cells[this.Owner.Columns.IndexOf(this)].Controls.Add(textBox);
			}
		}


		protected override object SaveViewState()
		{			
			ViewState["__TextBoxList"] = _list;
			return base.SaveViewState ();
		}
	}

	
	//	TODO: Comentário
	public class DropDownListColumn : System.Web.UI.WebControls.DataGridColumn 
	{
		#region Properties
		private string _listItemsSource = "";
		private string _listItemTextField = "";
		private string _formatString = "";
		private string _listItemValueField = "";
		private string _selectedValueField = "";
		private string _enabledField = "";
		private string _visibleField = "";
		private bool _notVisible;
		private bool _notEnabled;
		private bool _inputMandatory;
		private SourceType _listSourceType = SourceType.Enumeration;
		private string _width;

		public enum SourceType 
		{
			Enumeration = 1,
			Message = 2,
			SubMessage = 3
		}

		public bool NotEnabled
		{get{return _notEnabled;}set{_notEnabled = value;}}
		public bool NotVisible
		{get{return _notVisible;}set{_notVisible = value;}}
		public SourceType ListSourceType
		{get{return _listSourceType;}set{_listSourceType = value;}}
		public string SelectedValueField
		{get{return _selectedValueField;}set{_selectedValueField = value;}}
		public string EnabledField
		{get{return _enabledField;}set{_enabledField = value;}}
		public string VisibleField
		{get{return _visibleField;}set{_visibleField = value;}}
		public string ListItemsSource
		{get{return _listItemsSource;}set{_listItemsSource = value;}}
		public string ListItemTextField
		{get{return _listItemTextField;}set{_listItemTextField = value;}}
		public string ListItemValueField
		{get{return _listItemValueField;}set{_listItemValueField = value;}}
		public string FormatString
		{get{return _formatString;}set{_formatString = value;}}
		public bool InputMandatory
		{get{return _inputMandatory;}set{_inputMandatory = value;}}
		/// <summary>
		/// Tamanho do Drop Down List (em pixels). Informar apenas o tamanho (ex: width="300")
		/// </summary>
		public string Width
		{get{return _width;}set{_width = value;}}


		#endregion Properties

		private ArrayList _list = new ArrayList();
		string[] _dropDownItensText;
		string[] _dropDownItensValue;

		//Message Bind
		internal void MessageBind(DataGridItemEventArgs e, MessageControlBuilder builder)
		{
			ListItemType itemType = e.Item.ItemType;
			if(itemType == ListItemType.AlternatingItem || itemType == ListItemType.Item)
			{
				int columnIndex = this.Owner.Columns.IndexOf((DataGridColumn)this);
	
				if ( (!base.DesignMode) )
				{
					object dataItem = e.Item.DataItem;
					if(e.Item.DataItem != null)
					{
						MsgDropDownList dropdown = new MsgDropDownList();

					
						if(_listSourceType == DropDownListColumn.SourceType.SubMessage)
						{
							object arr = System.Web.UI.DataBinder.GetPropertyValue(dataItem, _listItemsSource);
							dropdown.MessageDataSource(arr);
							dropdown.MsgListSourceType = MsgDropDownList.ListSourceType.Message;
						} 
						else 
						{
							dropdown.MsgListItemsSource = _listItemsSource;
							dropdown.MsgListSourceType = (MsgDropDownList.ListSourceType)_listSourceType;
						}

						dropdown.MsgListItemTextField = _listItemTextField;
						dropdown.FormatString = _formatString;
						dropdown.MsgListItemValueField = _listItemValueField;
						dropdown.InputMandatory = _inputMandatory;						

						if(_width != null && _width != "")
						{
							int iWidth = 0;
							try
							{
								iWidth = int.Parse(_width);
							}
							catch
							{
							}
							if(iWidth != 0)
								dropdown.Width =new Unit(iWidth, UnitType.Pixel);
						}

						e.Item.Cells[columnIndex].Controls.Add(dropdown);

						dropdown.MessageInit();
						dropdown.MessageBind();

						dropdown.ID = Guid.NewGuid().ToString();

						SaveListItemsValues(dropdown);

						string _selectedValue;
						if(_selectedValueField != "")
						{
							_selectedValue =
								builder.GetPropertyValue(dataItem, _selectedValueField, false);
							dropdown.SelectedValue = _selectedValue;
						}
						
						bool _enabled = true;
						if(_enabledField != "")
						{
							_enabled =
								Convert.ToBoolean(builder.GetPropertyValue(dataItem, _enabledField, false));
							dropdown.Enabled = _notEnabled ? ! _enabled :  _enabled;
						}

						bool _visible = true;
						if(_visibleField != "")
						{
							_visible =
								Convert.ToBoolean(builder.GetPropertyValue(dataItem, _visibleField, false));
							dropdown.Visible = _notVisible ? _visible : ! _visible;
						}
						
						object[] property = {dropdown.SelectedValue, dropdown.Enabled, dropdown.Visible, dropdown.ID};

						_list.Add(property);	
					}
				} 
				else 
				{
					MsgDropDownList msgMsgDropDownList = new MsgDropDownList();
					msgMsgDropDownList.MsgListItemsSource = _listItemsSource;
					msgMsgDropDownList.MsgSelectedValueField = _selectedValueField;
					e.Item.Cells[columnIndex].Controls.Add(msgMsgDropDownList);

				}				
			}
		}

		ArrayList _lstDropDowns = new ArrayList();

		private void SaveListItemsValues(MsgDropDownList ddl)
		{
			ArrayList _lstProperties = new ArrayList(2);

			_dropDownItensText = new string[ddl.Items.Count];
			_dropDownItensValue = new string[ddl.Items.Count];
			for(int i = 0; i < ddl.Items.Count;i++)
			{
				_dropDownItensValue[i] = ddl.Items[i].Value;
				_dropDownItensText[i] = ddl.Items[i].Text;
			}

			_lstProperties.Add(_dropDownItensText);
			_lstProperties.Add(_dropDownItensValue);

			_lstDropDowns.Add(_lstProperties);
		}

		private void SetListItemsValues(MsgDropDownList ddl)
		{						
			for(int i = 0; i < _dropDownItensText.Length;i++)
			{
				ddl.Items.Add(new ListItem(_dropDownItensText[i], _dropDownItensValue[i]));
			}			 
		}


		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState (savedState);
			_lstDropDowns = (ArrayList)ViewState["__DropDownListProperties"];
			//TODO: Colocar a lista dentro do _lstDropDowns
			_list = (ArrayList)ViewState["__DropDownList"];

			for(int i = 0; i < this.Owner.Items.Count; i++)
			{
				DataGridItem item = (DataGridItem)this.Owner.Items[i];
				
				MsgDropDownList ddl = new MsgDropDownList();

				//TODO: Tirar de variavel global e passar para o método
				
				_dropDownItensText = (string[])((ArrayList)_lstDropDowns[i])[0]; // (string[])ViewState["__DropDownListItensText"];
				_dropDownItensValue =(string[])((ArrayList)_lstDropDowns[i])[1]; //(string[])ViewState["__DropDownListItensValue"];

				SetListItemsValues(ddl);
				
				object[] property = (object[])_list[i];
				ddl.SelectedValue = (string)property[0];
				ddl.Enabled = (bool)property[1];
				ddl.Visible = (bool)property[2];
				ddl.ID = (string)property[3];
				item.Cells[this.Owner.Columns.IndexOf(this)].Controls.Add(ddl);
			}
		}

		protected override object SaveViewState()
		{			
			ViewState["__DropDownListProperties"] = _lstDropDowns;
			ViewState["__DropDownList"] = _list;
//			ViewState["__DropDownListItensText"] = _dropDownItensText;
//			ViewState["__DropDownListItensValue"] = _dropDownItensValue;
			return base.SaveViewState ();
		}

	}

	//	TODO: Comentário
	public class CheckBoxColumn : System.Web.UI.WebControls.DataGridColumn 
	{
		private string _checkedField = "";
		private string _enabledField = "";
		private string _enabledFalse = "";
		private string _visibleField = "";
		private bool _notChecked;
		private bool _notEnabled;
		private bool _notVisible;

		private string _id = "";

		private ArrayList _list = new ArrayList();

		public string ID
		{get{return _id;}set{_id = value;}}

		public string CheckedField
		{get{return _checkedField;}set{_checkedField = value;}}
		
		public string EnabledField
		{get{return _enabledField;}set{_enabledField = value;}}
		
		public string VisibleField
		{get{return _visibleField;}set{_visibleField = value;}}

		public string Enabled
		{get{return _enabledFalse;}set{_enabledFalse = value;}}

		public bool NotChecked
		{get{return _notChecked;}set{_notChecked = value;}}
		
		public bool NotEnabled
		{get{return _notEnabled;}set{_notEnabled = value;}}
		
		public bool NotVisible
		{get{return _notVisible;}set{_notVisible = value;}}

		//Message Bind
		internal void MessageBind(DataGridItemEventArgs e, MessageControlBuilder builder)
		{
			ListItemType itemType = e.Item.ItemType;
			if(itemType == ListItemType.AlternatingItem || itemType == ListItemType.Item)
			{
				int columnIndex = this.Owner.Columns.IndexOf((DataGridColumn)this);
	
				if ( (!base.DesignMode) )
				{
					object dataItem = e.Item.DataItem;
					if(e.Item.DataItem != null)
					{
						CheckBox checkBox = new CheckBox();

						bool _checked = false;
						if(_checkedField != "")
						{
							_checked =
								Convert.ToBoolean( builder.GetPropertyValue(dataItem, _checkedField, typeof(bool)) );

								//Convert.ToBoolean(builder.GetPropertyValue(dataItem, _checkedField, false));
							
							checkBox.Checked = _notChecked ? ! _checked :  _checked;
						}
						
						bool _enabled = true;
						if (_enabledFalse != "")
						{
							_enabled = Convert.ToBoolean(_enabledFalse);
							checkBox.Enabled = _enabled;
						}

						if(_enabledField != "")
						{
							_enabled =
								Convert.ToBoolean( builder.GetPropertyValue(dataItem, _enabledField, typeof(bool)) );

							checkBox.Enabled = _notEnabled ? ! _enabled :  _enabled;
						}

						bool _visible = true;
						if(_visibleField != "")
						{
							_visible =
								Convert.ToBoolean( builder.GetPropertyValue(dataItem, _visibleField, typeof(bool)) );
							checkBox.Visible = _notVisible ? _visible : ! _visible;
						}
						
						bool[] property = {checkBox.Checked, checkBox.Enabled, checkBox.Visible};

						_list.Add(property);
						e.Item.Cells[columnIndex].Controls.Add(checkBox);
					}
				} 
				else 
				{
					e.Item.Cells[columnIndex].Controls.Add(new CheckBox());
					Label label = new Label();
					label.Text = "{" + _checkedField + "}";
					e.Item.Cells[columnIndex].Controls.Add(label);
				}				
			}
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState (savedState);
			_list = (ArrayList)ViewState["__CheckBoxList"];

			for(int i = 0; i < this.Owner.Items.Count; i++)
			{
				DataGridItem item = (DataGridItem)this.Owner.Items[i];
				
				CheckBox checkBox = new CheckBox();
				bool[] property = (bool[])_list[i];
				checkBox.Checked = property[0];
				checkBox.Enabled = property[1];
				checkBox.Visible = property[2];
				item.Cells[this.Owner.Columns.IndexOf(this)].Controls.Add(checkBox);
			}
		}


		protected override object SaveViewState()
		{			
			ViewState["__CheckBoxList"] = _list;
			return base.SaveViewState ();
		}

	}
	
	/// <summary>
	///	TODO: Comentário
	/// </summary>	
	//[ParseChildrenAttribute(true, "Header")]
	public class MessageColumn : System.Web.UI.WebControls.TemplateColumn
	{
		private string _field;
		private string _formatString;
		private string _totalFormatString;
		private string _id;
		private int _MaxLength;
		private bool _totalizar;
		private CustomFormatHelper.CustomFormatEnum _customFormatString = CustomFormatHelper.CustomFormatEnum.None;
		private CustomFormatHelper.CustomFormatEnum _customTotalFormatString = CustomFormatHelper.CustomFormatEnum.None;
		private double _total;
		private string _helpAction = "";


		//Id
		public bool Totalizar
		{get{return _totalizar;}set{_totalizar = value;}}

		//HelpAction
		public string HelpAction
		{get{return _helpAction;}set{_helpAction = value;}}

		//Id
		public string id
		{get{return _id;}set{_id = value;}}

		//Field
		public string Field
		{get{return _field;}set{_field = value;}}
		
		public string TotalFormatString
		{get{return _totalFormatString;}set{_totalFormatString = value;}}
		
		//Format String
		public string FormatString
		{get{return _formatString;}set{_formatString = value;}}

		//Custom Format String
		public CustomFormatHelper.CustomFormatEnum CustomFormat
		{get{return _customFormatString;}set{_customFormatString = value;}}

		//Custom Total Format String
		public CustomFormatHelper.CustomFormatEnum CustomTotalFormat
		{get{return _customTotalFormatString;}set{_customTotalFormatString = value;}}

		//Max length da string
		public int MaxLength
		{get{return _MaxLength;}set{_MaxLength = value;}}

		//Message Bind
		protected internal virtual void MessageBind(DataGridItemEventArgs e, MessageControlBuilder builder)
		{
			ListItemType itemType = e.Item.ItemType;
			
			int columnIndex = this.Owner.Columns.IndexOf((DataGridColumn)this);

			if(itemType == ListItemType.AlternatingItem || itemType == ListItemType.Item)
			{				
				if ( (!base.DesignMode) )
				{
					if(e.Item.DataItem != null)
					{
						if(_totalizar)
						{
							object text = 
								builder.GetPropertyValue(e.Item.DataItem, _field, true, System.Type.GetType("System.Decimal") );
							if ( (text is Int32 || text is decimal || text is double) && text.ToString() != String.Empty && !Helper.IsPropertyMinValue( e.Item.DataItem, _field ) ) { _total += Convert.ToDouble(text); }
						}

						string textColumn = "";

						//Verifica se faz formatação customizada
						if ( _customFormatString == CustomFormatHelper.CustomFormatEnum.None )
							textColumn = 
								builder.GetPropertyValue(e.Item.DataItem, _field, _formatString, true);
						else 
						{
							string propertyValue = 
								builder.GetPropertyValue(e.Item.DataItem, _field, "{0}", true );

							if ( propertyValue != null && propertyValue != "" ) 
							{
								CustomFormatHelper formatString = new CustomFormatHelper( _customFormatString , true );
								textColumn = formatString.GetCurrentFormatString( propertyValue );
							} 
						}

						if (_MaxLength > 3)
						{
							e.Item.Cells[columnIndex].Text = 
								_MaxLength > 0 && textColumn.Length >= _MaxLength ? BasicFunctions.Mid( textColumn, _MaxLength-3 ) + "..." : textColumn  ;
						}
						else
							e.Item.Cells[columnIndex].Text = 
								_MaxLength > 0 && textColumn.Length >= _MaxLength ? BasicFunctions.Mid( textColumn, _MaxLength ) : textColumn  ;

					}
				
					if ( _helpAction != "" && _helpAction != null )
					{
						e.Item.Cells[columnIndex].Text = string.Format ( 
							"<a href=javascript:openHelp('{0}',{1}') class='xpto'>(?)</a>&nbsp;{2}",
							_helpAction,
							_helpAction,
							e.Item.Cells[columnIndex].Text);
					}
				} 
				else 
				{
					e.Item.Cells[columnIndex].Text = "{" + _field + ":" + _customFormatString.ToString() + ":" +
						( _MaxLength > 0 ? _MaxLength.ToString() : "") + "}";
				}	
			} 
			else if(itemType == ListItemType.Footer)
			{
				if(_totalizar)
				{
					if ( _customTotalFormatString == CustomFormatHelper.CustomFormatEnum.None )
						e.Item.Cells[columnIndex].Text = string.Format(_totalFormatString, _total); 
					else 
					{
						CustomFormatHelper formatString = new CustomFormatHelper( _customTotalFormatString , true );
						e.Item.Cells[columnIndex].Text = formatString.GetCurrentFormatString( _total.ToString("n") );
					}
				}
			}
		}
	}

	/// <summary>
	///	TODO: Comentário
	/// </summary>	
	public class NoPrintTemplateColumn : TemplateColumn 
	{

	}

	/// <summary>
	///	TODO: Comentário
	/// </summary>	
	public class PopUpColumn : MessageColumn 
	{
		private string _toolTip;

		//Tool Tip
		public string ToolTip
		{get{return _toolTip;}set{_toolTip = value;}}

		private string _text = string.Empty;
		private string _firstReturnValue = string.Empty;
		private string _secondReturnValue = string.Empty;
		private string _thirdReturnValue = string.Empty;

		//Text
		public string Text
		{get{return _text;}set{_text = value;}}

		//FirstReturnValue
		public string FirstReturnValue
		{get{return _firstReturnValue;}set{_firstReturnValue = value;}}

		//SecondReturnValue
		public string SecondReturnValue
		{get{return _secondReturnValue;}set{_secondReturnValue = value;}}
		
		//ThirdReturnValue
		public string ThirdReturnValue
		{get{return _thirdReturnValue;}set{_thirdReturnValue = value;}}

		protected internal override void MessageBind(
			DataGridItemEventArgs e, 
			MessageControlBuilder builder)
		{
			base.MessageBind(e, builder);

			ListItemType itemType = e.Item.ItemType;			
			int columnIndex = this.Owner.Columns.IndexOf((DataGridColumn)this);
			if(itemType == ListItemType.AlternatingItem || itemType == ListItemType.Item)
			{
				if ( (!base.DesignMode) )
				{
					if(e.Item.DataItem != null)
					{
						HyperLink h = new HyperLink();
						if(_text != "")
						{
							h.ImageUrl = _text;
						} 
						else 
						{
							h.ImageUrl = e.Item.Cells[columnIndex].Text;
						}
						h.ToolTip = _toolTip;
						

						StringBuilder sb = 
							new StringBuilder("javascript:ReturnDataGridValue('");
						if(_firstReturnValue != string.Empty)
						{
							string _value = builder.GetPropertyValue(
								e.Item.DataItem, 
								_firstReturnValue, 
								this.FormatString, 
								true);

							sb.Append(_value);
								
						}
						sb.Append("','");
						if(_secondReturnValue != string.Empty)
						{
							string _value = builder.GetPropertyValue(
								e.Item.DataItem, 
								_secondReturnValue, 
								this.FormatString, 
								true);

							sb.Append(_value);
						}
						sb.Append("','");
						if(_thirdReturnValue != string.Empty)
						{
							string _value = builder.GetPropertyValue(
								e.Item.DataItem, 
								_thirdReturnValue, 
								this.FormatString, 
								true);

							sb.Append(_value);
						}
						sb.Append("');");
						h.NavigateUrl = sb.ToString();
						h.ImageUrl = _text;

						e.Item.Cells[columnIndex].Controls.Add(h);
					}
				} 		
			} 
		}
	}
    
	/// <summary>
	///	TODO: Comentário
	/// </summary>	
	public class EventColumn : System.Web.UI.WebControls.DataGridColumn 
	{
		private string _event;
		private string _label;
		private string _toolTip;
		private string _id;		
		private EventButtonType _buttonType = EventButtonType.Button;
		private ITemplate _header;
		private string _msgTextField;
		private bool _causesValidation = false;
		private bool _totalizar;
		private double _total;
		private CustomFormatHelper.CustomFormatEnum _customFormatString = CustomFormatHelper.CustomFormatEnum.None;
		private CustomFormatHelper.CustomFormatEnum _customTotalFormatString = CustomFormatHelper.CustomFormatEnum.None;

		//MessageField
		public string MsgTextField
		{get{return _msgTextField;}set{_msgTextField = value;}}

		//Id
		public bool CausesValidation
		{get{return _causesValidation;}set{_causesValidation = value;}}

		//Id
		public string id
		{get{return _id;}set{_id = value;}}

		//Tool Tip
		public string ToolTip
		{get{return _toolTip;}set{_toolTip = value;}}

		//Header
		public ITemplate Header
		{get{return _header;}set{_header = value;}}

		//Event
		public string Event
		{get{return _event;}set{_event = value;}}

		//Label
		public string Label
		{get{return _label;}set{_label = value;}}

		//Type
		public enum EventButtonType
		{
			Link = 1,
			Button = 2,
			Image = 3
		}
		public EventButtonType ButtonType
		{get{return _buttonType;}set{_buttonType = value;}}

		//Totalizar
		public bool Totalizar
		{get{return _totalizar;}set{_totalizar = value;}}

		//Custom Format String
		public CustomFormatHelper.CustomFormatEnum CustomFormat
		{get{return _customFormatString;}set{_customFormatString = value;}}

		//Custom Total Format String
		public CustomFormatHelper.CustomFormatEnum CustomTotalFormat
		{get{return _customTotalFormatString;}set{_customTotalFormatString = value;}}

		// TODO: Comentário
		protected override void OnColumnChanged()
		{
			base.OnColumnChanged ();
		}

		// TODO: Comentário
		protected internal virtual void MessageBind(DataGridItemEventArgs e, MessageControlBuilder builder)
		{
			if(_msgTextField == "" || _msgTextField == null) { return ; }

			int columnIndex = this.Owner.Columns.IndexOf((DataGridColumn)this);

			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if ( (!base.DesignMode) )
				{
					if(e.Item.DataItem != null)
					{
						string propertyValue = 
							builder.GetPropertyValue(e.Item.DataItem, _msgTextField, "{0}", true );

						if ( propertyValue != null && propertyValue != "" ) 
						{
							if(_totalizar)
							{
								if ( BasicFunctions.IsNumeric( propertyValue ) &&
									!Helper.IsPropertyMinValue( e.Item.DataItem, _msgTextField ) ) 
								{ _total += Convert.ToDouble(propertyValue); }
							}

							//Verifica se faz formatação customizada
							if ( _customFormatString != CustomFormatHelper.CustomFormatEnum.None )
							{
								CustomFormatHelper formatString = new CustomFormatHelper( _customFormatString , true );
								propertyValue = formatString.GetCurrentFormatString( propertyValue );
							}

							Control c = e.Item.Cells[columnIndex].Controls[0];
							if(c is LinkButton)
							{
								((LinkButton)c).Text = propertyValue;
							} 
							else if(c is ImageButton)
							{
								((ImageButton)c).AlternateText = propertyValue;
								((ImageButton)c).ToolTip = propertyValue;
							} 
							else if(c is Button)
							{
								((Button)c).Text = propertyValue;
							} 
						} 
					}
				} 
				else
				{
					Control c = e.Item.Cells[columnIndex].Controls[0];
					if(c is LinkButton)
					{
						((LinkButton)c).Text = "{" + _msgTextField + "}";
					} 
					else if(c is ImageButton)
					{
						((ImageButton)c).AlternateText = "{" + _msgTextField + "}";
						((ImageButton)c).ToolTip = "{" + _msgTextField + "}";
					} 
					else if(c is Button)
					{
						((Button)c).Text = "{" + _msgTextField + "}";
					} 
				}
			}
			else if(e.Item.ItemType == ListItemType.Footer)
			{
				if(_totalizar)
				{
					if ( _customTotalFormatString == CustomFormatHelper.CustomFormatEnum.None )
						e.Item.Cells[columnIndex].Text = string.Format("{0}", _total); 
					else 
					{
						CustomFormatHelper formatString = new CustomFormatHelper( _customTotalFormatString , true );
						e.Item.Cells[columnIndex].Text = formatString.GetCurrentFormatString( _total.ToString("n") );
					}
				}
			}
		}
		
		//TODO: Comentário
		public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
		{			
			if(itemType == ListItemType.Header)
			{
				base.InitializeCell (cell, columnIndex, itemType);
				if(_header != null)
				{
					_header.InstantiateIn(cell);
				}				
			}          
			else 
			{
				switch(_buttonType)
				{
					case EventButtonType.Link:
					
						LinkButton l = new LinkButton();
						l.CommandName = _event;
						l.Text = _label;
						l.CausesValidation = this._causesValidation;
						cell.Controls.Add(l);						
						break;

					case EventButtonType.Image:
					
						ImageButton i = new ImageButton();
						i.CommandName = _event;
						i.ImageUrl = _label;
						i.ToolTip = ToolTip; 
						i.CausesValidation = this._causesValidation;
						cell.Controls.Add(i);
						break;

					case EventButtonType.Button:
						
						Button b = new Button();
						b.CommandName = _event;
						b.Text = _label;
						b.CausesValidation = this._causesValidation;
						cell.Controls.Add(b);						
						break;								
				}
				base.InitializeCell (cell, columnIndex, itemType);
			}
		}

	}
}


