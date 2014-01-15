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

namespace SuperPag.Framework.Web.WebControls
{
	[ToolboxData("<{0}:MsgListBox runat=server></{0}:MsgListBox>")]
	public class MsgListBox : System.Web.UI.WebControls.ListBox, MessageControl
	{
		#region Propriedades e Eventos

		private string _msgSource;
		private string _msgSourceKey = "";
		private string _msgSourceField = "";	
		private string _selectedField = "";
		private MessageCollection _messageArray; //private System.Array _messageArray;
		private static MessageControlBuilder _builder = new MessageControlBuilder();

		public void MessageUnBind()
		{

		}
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
		
		//subo o evento
		public void OnAfterMessageBind(object message)
		{
			if(AfterMessageBind != null)
			{
				AfterMessageBind(this, message);
			}
		}


		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState (savedState);
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

		public void MessageBind()
		{
			OnBeforeMessageBind(_messageArray);
			
			if(_messageArray != null)
			{
				base.DataSource = _messageArray;
				base.DataBind();

//				//Seleciona os items
//				int i = 0;
//				foreach ( ListItem item in this.Items )
//				{
////					bool propery
////					Helper.ReturnPropertyField ( 
////					if ( _messageArray [ i ]
//				}
			} 
			OnAfterMessageBind(_messageArray);
		}

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

	
	}
}


