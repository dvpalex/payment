using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace SuperPag.Framework.Web.WebControls
{
	/// <summary>
	/// Summary description for CheckBox.
	/// </summary>
	///	TODO: Comentário
	[Designer(typeof(CheckBoxDesigner)),
	ToolboxData("<{0}:MsgCheckBox runat=server></{0}:MsgCheckBox>")]
	public class MsgCheckBox : System.Web.UI.WebControls.CheckBox, MessageControl
	{
		private string _msgSource;
		private string _msgSourceKey = "";
		private string _msgCheckField;
		private string _msgEnabledField;
		private string _msgVisibleField;
		object _message;
		MessageControlBuilder _builder;
		
		//TODO: Comentário
		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind AfterMessageBind;

		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind BeforeMessageBind;

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

		public void MessageInit()

		{
			if(this.Page is MessagePage)
			{
				MessagePage page = (MessagePage)this.Page;

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
			else
			{
				throw new ApplicationException("The MsgButton must be placed inside a MessagePage ");
			}
			_builder = new MessageControlBuilder();
		}

		public void MessageBind()
		{
			OnBeforeMessageBind(_message);
			
			if(_message != null && _msgCheckField != null && this.Visible)
			{
				//Text
				this.Checked = 
					Convert.ToBoolean( _builder.GetPropertyValue(_message, _msgCheckField, typeof(bool)) );
			}

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
			Helper.SetUndBindPropetyValue( _message, this.MsgCheckField, this.Checked.ToString() );
		}

		//TODO: Comentário
		protected override void Render(HtmlTextWriter writer)
		{
			base.Render (writer);
		}

		//é usado pelo repeater
		public void MessageDataSource(object message) 
		{
			_builder= new MessageControlBuilder();
			_message = message;
		}
	}

	//TODO: Comentário
	public class CheckBoxDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			string designTimeHtml = base.GetDesignTimeHtml ();
			MsgCheckBox checkbox = (MsgCheckBox)this.Component;
			
			return designTimeHtml + "&nbsp;{" + checkbox.MsgCheckField + "}";
		}
	}
}
