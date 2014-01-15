using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace SuperPag.Framework.Web.WebControls {
	[Designer(typeof(RadioButtonDesigner)),
	ToolboxData("<{0}:MsgRadioButton runat=server></{0}:MsgRadioButton>")]
	public class MsgRadioButton : System.Web.UI.WebControls.RadioButton, MessageControl {
		private string _msgSource;
		private string _msgSourceKey = "";
		private string _msgSelectedField;
		private string _msgSelectedFieldValue;
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
		public string MsgSource {
		 get{return _msgSource;}set{_msgSource = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSelectedField {
		 get{return _msgSelectedField;}set{_msgSelectedField = value;}}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSelectedFieldValue {
			get{return _msgSelectedFieldValue;}set{_msgSelectedFieldValue = value;}
		}
		
		//Enabled Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgEnabledField {
		 get{return _msgEnabledField;}set{_msgEnabledField = value;}}

		//Visible Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgVisibleField {
		 get{return _msgVisibleField;}set{_msgVisibleField = value;}}

		//Message Source Key
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceKey {
		 get{return _msgSourceKey;}set{_msgSourceKey = value;}}

		public void MessageInit() {
			if(this.Page is MessagePage) {
				MessagePage page = (MessagePage)this.Page;

				if(_msgSource != String.Empty && _msgSource != null) {			
					if(_msgSourceKey != "") {
						
						_message = page.Messages.Get(_msgSourceKey);
					} 
					else {
						_message = page.Messages.Get(_msgSource);
					}
				}
			}
			else {
				throw new ApplicationException("The MsgButton must be placed inside a MessagePage ");
			}
			_builder = new MessageControlBuilder();
		}

		public void MessageBind() {
			OnBeforeMessageBind(_message);
			
			if(_message != null && _msgSelectedField != null && _msgSelectedFieldValue != null && this.Visible) {
				
				object enumName = _builder.GetPropertyValue(_message, _msgSelectedField, false, true);
				
				//Text
				this.Checked = enumName.ToString().ToLower() == this._msgSelectedFieldValue.ToLower();
			}

			OnAfterMessageBind(_message);
		}

		//subo o evento
		public void OnAfterMessageBind(object message) {
			if(AfterMessageBind != null) {
				AfterMessageBind(this, message);
			}
		}

		//subo o evento
		public void OnBeforeMessageBind(object message) {
			if(BeforeMessageBind != null) {
				BeforeMessageBind(this, message);
			}
		}

		//TODO: Comentário
		public void MessageUnBind() {
			if (this.Checked) {
				Helper.SetUndBindPropetyValue( _message, this.MsgSelectedField, this.MsgSelectedFieldValue );
			}
		}

		//TODO: Comentário
		protected override void Render(HtmlTextWriter writer) {
			base.Render (writer);
		}

		//é usado pelo repeater
		public void MessageDataSource(object message) {
			_builder= new MessageControlBuilder();
			_message = message;
		}
	}

	//TODO: Comentário
	public class RadioButtonDesigner : System.Web.UI.Design.ControlDesigner {
		public override string GetDesignTimeHtml() {
			string designTimeHtml = base.GetDesignTimeHtml ();
			MsgRadioButton radioButton = (MsgRadioButton)this.Component;
			
			return designTimeHtml + "&nbsp;{" + radioButton.MsgSelectedField + "." + radioButton.MsgSelectedFieldValue + "}";
		}
	}
}
