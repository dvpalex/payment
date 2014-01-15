using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections;
using SuperPag.Framework;

namespace SuperPag.Framework.Web.WebControls
{
	public class RepeaterControlBuilder : ControlBuilder
	{
		public override Type GetChildControlType(String tagName, IDictionary
			attributes)
		{
			if (tagName.ToLower().EndsWith("label"))
				return typeof(Label);
			if (tagName.ToLower().EndsWith("dropdownlist"))
				return typeof(DropDownList);
			if (tagName.ToLower().EndsWith("repeater"))
				return typeof(Repeater);
			if (tagName.ToLower().EndsWith("datagrid"))
				return typeof(DataGrid);
			if (tagName.ToLower().EndsWith("button"))
				return typeof(Button);
			if (tagName.ToLower().EndsWith("textbox"))
				return typeof(TextBox);
			return null;
		}
	}

	/// <summary>
	/// Summary description for Repeater.
	/// </summary>
	///	TODO: Comentário
	[Designer(typeof(RepeaterDesigner)),
	ControlBuilderAttribute(typeof(RepeaterControlBuilder)),ParseChildren(true),
	ToolboxData("<{0}:MsgRepeater runat=server></{0}:MsgRepeater>"),
	DefaultEvent("MessageEvent")]
	public class MsgRepeater : System.Web.UI.WebControls.Repeater , MessageControl
	{
		#region Propriedades e Evento

		private string _msgSource;
		private string _msgSourceKey = "";
		private string _msgCheckField;
		private string _msgEnabledField;
		private string _msgVisibleField;
		object _messageArray;
		private string _msgSourceField = "";

		//Message Source
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSource
		{get{return _msgSource;}set{_msgSource = value;}}

		//Text Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgCheckField
		{get{return _msgCheckField;}set{_msgCheckField = value;}}
		
		//Enabled Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgEnabledField
		{get{return _msgEnabledField;}set{_msgEnabledField = value;}}

		//Visible Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgVisibleField
		{get{return _msgVisibleField;}set{_msgVisibleField = value;}}

		//Message Source Key
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceKey
		{get{return _msgSourceKey;}set{_msgSourceKey = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceField
		{get{return _msgSourceField;}set{_msgSourceField = value;}}

		public event MessageEventHandler.MessageDataBind AfterMessageBind;

		public event MessageEventHandler.MessageDataBind BeforeMessageBind;

		public void OnAfterMessageBind(object message)
		{
			if(AfterMessageBind != null)
			{
				AfterMessageBind(this, message);
			}
		}

		public void OnBeforeMessageBind(object message)
		{
			if(BeforeMessageBind != null)
			{
				BeforeMessageBind(this, message);
			}
		}

		#endregion Propriedades e Evento

		#region MessageInit
		public void MessageInit()
		{
			if(_messageArray != null) { return ; }
			object messageArray;

			if(this.Page is MessagePage)
			{
				MessagePage page = (MessagePage)this.Page;

				if(_msgSourceKey == "")
				{
					if(_msgSourceField == "")
					{
						messageArray = page.Messages.GetArray("ArrayOf" + _msgSource);
					} 
					else
					{
						if(_msgSource == "" || _msgSource == null) { return; }
						object source = page.Messages.Get(_msgSource);
						messageArray = DataBinder.GetPropertyValue(source, _msgSourceField);
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
						object source = page.Messages.GetArray(_msgSourceKey);
						messageArray = DataBinder.GetPropertyValue(source, _msgSourceField);
					}
				}
			} 
			else
			{
				throw new ApplicationException("The MsgRepeater must be placed inside a MessagePage ");
			}

			if ( messageArray != null) 
			{
				if(!(messageArray is System.Array))
				{
					throw new ApplicationException("The MessageSource must be 'System.Array' type.");
				}
			} else return;

			MessageControlBuilder _builder = new MessageControlBuilder();
			this._messageArray = (System.Array)messageArray;				
		}


		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			if(this.Page.IsPostBack)
			{
				MessageInit();
				foreach(RepeaterItem item in this.Controls) 
				{
					foreach(Control child in item.Controls)
					{
						if(child is MessageControl)
						{
							if(child is MsgRepeater)
							{
								((MessageControl)child).MessageDataSource(DataBinder.Eval(((System.Array)_messageArray).GetValue(item.ItemIndex), ((MsgRepeater)child).MsgSourceField));
							} 
							else
							{
								((MessageControl)child).MessageDataSource(((System.Array)_messageArray).GetValue(item.ItemIndex));
							}
						}
					}
				}
			}			
		}

		#endregion MessageInit

		protected override void OnItemDataBound(RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || 
				e.Item.ItemType == ListItemType.AlternatingItem)
			{
				foreach(Control child in e.Item.Controls)

				{
					if(child is MessageControl)
					{
						if(child is MsgRepeater)
						{
							((MessageControl)child).MessageDataSource(DataBinder.Eval(e.Item.DataItem, ((MsgRepeater)child).MsgSourceField));
							((MessageControl)child).MessageBind();
						} 
						else
						{
							((MessageControl)child).MessageDataSource(e.Item.DataItem);
							((MessageControl)child).MessageBind();
						}						
					}
				}
			}
			base.OnItemDataBound (e);
		}

		//TODO: Comentário
		public delegate void MsgRepeaterMessageEventHandler(
			object sender, 
			string eventName,
			Message message);

		//Message Source
		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MsgRepeaterMessageEventHandler MessageEvent;


		//TODO: Comentário
		public void OnMessageEvent(string eventName, Message message)
		{
			if(MessageEvent != null)
			{
				MessageEvent(this, eventName, message);
			}
		}

		protected override void OnItemCommand(RepeaterCommandEventArgs e)
		{			
			base.OnItemCommand(e);
			
			if(_messageArray == null)
			{
				MessageInit();
			}
			object item = ((System.Array)_messageArray).GetValue(e.Item.ItemIndex);

			((MessagePage)this.Page).Messages.Add((Message)item);

			OnMessageEvent(e.CommandName, (Message)item);

			RaiseBubbleEvent(this, e);
		}

		public void MessageBind()
		{
			//TODO: Eventos
			if(_messageArray != null)
			{
				this.DataSource = _messageArray;
				this.DataBind();
			}
		}

		//TODO: Comentário
		public void MessageUnBind()
		{
			foreach(RepeaterItem item in this.Controls) 
			{
				foreach(Control child in item.Controls)
				{
					if(child is MessageControl)
					{
						((MessageControl)child).MessageUnBind();
					}
				}
			}
		}

		//TODO: Comentário
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render (writer);
		}

		public void MessageDataSource(object message) 
		{
			_messageArray = (System.Array)message;
		}
	}

	//TODO: Comentário
	public class RepeaterDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			string designTimeHtml = base.GetDesignTimeHtml ();
			MsgRepeater Repeater = (MsgRepeater)this.Component;
			
			return designTimeHtml + "&nbsp;{" + Repeater.MsgCheckField + "}";
		}
	}
}