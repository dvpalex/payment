using System;
using System.Text;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Drawing.Design;
using ComponentArt.Web.UI;
using SuperPag.Framework;

namespace SuperPag.Framework.Web.WebControls
{

	[ DefaultEvent("MessageEvent") ]
	[Designer(typeof(TreeViewDesigner))]
	[ToolboxData("<{0}:MsgTreeView runat=server></{0}:MsgTreeView>")]
	public class MsgTreeView : System.Web.UI.WebControls.WebControl, MessageControl
	{

		#region MessageControl Members

		private string _msgSource;
		private string _baseTreeView;
		private string _msgSourceKey = "";
		private string _msgTextField = "";
		private string _msgChildField = "";
		private string _msgSourceField = "";
		private string _msgRaiseEventField = "";
		private string _msgIDField = "";
		private string _msgIsFolderField = "";
		private MessageCollection _messageArray; //private System.Array _messageArray;
		private static MessageControlBuilder _builder = new MessageControlBuilder();

			
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgIsFolderField
		{
			get { return _msgIsFolderField; }
			set { _msgIsFolderField = value; }
		}

		//Message Source
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSource
		{
			get { return _msgSource; }
			set { _msgSource = value; }
		}

		

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgRaiseEventField
		{
			get { return _msgRaiseEventField; }
			set { _msgRaiseEventField = value; }
		}

		//Message Source
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgIDField
		{
			get { return _msgIDField; }
			set { _msgIDField = value; }
		}

		//Message Source
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgChildField
		{
			get { return _msgChildField; }
			set { _msgChildField = value; }
		}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgTextField
		{
			get { return _msgTextField; }
			set { _msgTextField = value; }
		}

		//Message Source
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string BaseTreeView
		{
			get { return _baseTreeView; }
			set { _baseTreeView = value; }
		}

		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceField
		{get{return _msgSourceField;}set{_msgSourceField = value;}}

		//Message Source Key
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSourceKey
		{get{return _msgSourceKey;}set{_msgSourceKey = value;}}

		public delegate void MsgTreeViewMessageEventHandler(
			object sender, 
			Message message);

		//Message Source
		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MsgTreeViewMessageEventHandler MessageEvent;

		ComponentArt.Web.UI.TreeView _treeView;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

            ////TODO: corrigir
            //_treeView = new ComponentArt.Web.UI.TreeView(); //  (ComponentArt.Web.UI.TreeView)this.Controls[0].Controls[3].Controls[7].FindControl(this.BaseTreeView);
            //_treeView.AutoPostBackOnSelect = true;
            //_treeView.NodeSelected +=new ComponentArt.Web.UI.TreeView.NodeSelectedEventHandler(treeView_NodeSelected);
		}

        protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

            if (this.Page.Master != null)
                _treeView = (ComponentArt.Web.UI.TreeView)this.Page.Master.Controls[3].Controls[5].FindControl(this.BaseTreeView);
            else
                _treeView = (ComponentArt.Web.UI.TreeView)this.Page.FindControl(this.BaseTreeView);
            _treeView.AutoPostBackOnSelect = true;
            _treeView.NodeSelected += new ComponentArt.Web.UI.TreeView.NodeSelectedEventHandler(treeView_NodeSelected);

			FieldInfo f = typeof ( ComponentArt.Web.UI.WebControl ).GetField ( "licenseDiceResult" , BindingFlags.Instance | BindingFlags.NonPublic );
			f.SetValue ( _treeView, Enum.Parse (  f.FieldType , "Licensed" )  );

			MessageInit();

			MessageCollection mc = this._messageArray;

			if ( _treeView.SelectedNode != null )
			{
				Message message = FindByID ( mc , _treeView.SelectedNode.ID  );

				if ( message != null )
				{
					((MessagePage)this.Page).Messages.Add(message);
				} 
				else
				{
					if ( mc.Count > 0 )
					{
						((MessagePage)this.Page).Messages.Add ( (Message)Activator.CreateInstance ( mc.GetType() ) ) ;
					}
				}

				OnMessageEvent ( message );
			} 
			else
			{
				if ( mc != null && mc.Count > 0 )
				{
					((MessagePage)this.Page).Messages.Add ( (Message)Activator.CreateInstance ( mc [ 0 ].GetType() ) ) ;
				}
			}
		}


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

		public event SuperPag.Framework.Web.WebControls.MessageEventHandler.MessageDataBind AfterMessageBind;

		public event SuperPag.Framework.Web.WebControls.MessageEventHandler.MessageDataBind BeforeMessageBind;

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

		#endregion

		
		public void OnMessageEvent( Message message )
		{
			if(MessageEvent != null)
			{
				MessageEvent(this, message);
			}
		}

		private Message FindByID ( MessageCollection mc , string nodeId )
		{
			Message message = (Message)mc.FindFirstByValue ( this._msgIDField , nodeId);
			if ( message == null )
			{
				foreach ( Message m in mc )
				{
					MessageCollection subMessageColl = 
						(MessageCollection)DataBinder.GetPropertyValue ( m, this._msgChildField );

					message = FindByID ( subMessageColl , nodeId);
					if ( message != null )
					{
						return message;
					}
				}

				return null;
			} 
			else
			{
				return message;
			}
		}

		private void treeView_NodeSelected(object sender, TreeViewNodeEventArgs e)
		{
			MessageInit();
			
			MessageCollection mc = this._messageArray;

			Message message = FindByID ( mc , e.Node.ID  );

			((MessagePage)this.Page).Messages.Add(message);

			OnMessageEvent ( message );
		}

		public void MessageBind()
		{
            ////TODO: corrigir
            if (this.Page.Master != null)
                _treeView = (ComponentArt.Web.UI.TreeView)this.Page.Master.Controls[3].Controls[5].FindControl(this.BaseTreeView);
            else
                _treeView = (ComponentArt.Web.UI.TreeView)this.Page.FindControl(this.BaseTreeView);
            _treeView.AutoPostBackOnSelect = true;
            _treeView.NodeSelected += new ComponentArt.Web.UI.TreeView.NodeSelectedEventHandler(treeView_NodeSelected);

			OnBeforeMessageBind( _messageArray );
			
			CreateNodes ( _treeView );
				
			_treeView.DataBind();

			OnAfterMessageBind( _messageArray );

//            this.Controls.Add(_treeView);

		}

		public void CreateChildren( TreeViewNodeCollection nodes, MessageCollection mc )
		{
			if ( mc != null )
			{
				foreach ( Message message in mc ) 
				{
					TreeViewNode tvNode = new TreeViewNode ( ) ;
					tvNode.Text = DataBinder.GetPropertyValue ( message, this._msgTextField ).ToString() ;
			
					tvNode.ID = DataBinder.GetPropertyValue ( message, this._msgIDField ).ToString() ;

					if ( this._msgRaiseEventField != "" && this._msgRaiseEventField != null )
					{
						tvNode.AutoPostBackOnSelect = 
							Convert.ToBoolean ( DataBinder.GetPropertyValue ( message, this._msgRaiseEventField ) ) ;
					} 
					else
					{
						tvNode.AutoPostBackOnSelect = false;
					}

					//TODO: hardcoded
					if ( ! Convert.ToBoolean ( DataBinder.GetPropertyValue ( message, this._msgIsFolderField ) )  ) 
					{
						tvNode.ImageUrl = "~/App_Themes/default/images/word.gif";
					} 
					else	
					{
                        tvNode.ImageUrl = "~/App_Themes/default/images/folder.gif";
					}
				
					MessageCollection children = 
						(MessageCollection)DataBinder.GetPropertyValue ( message, this._msgChildField );

					if ( children != null )
						CreateChildren ( tvNode.Nodes, children );

					nodes.Add ( tvNode );
			
				}
			}
		}
		public void CreateNodes ( ComponentArt.Web.UI.TreeView treeView )
		{
			MessageCollection mc = (MessageCollection) this._messageArray; 

			CreateChildren( treeView.Nodes, mc );
		}

		public void MessageUnBind()
		{
			// TODO:  Add MsgTreeView.MessageUnBind implementation
		}

	}

	public class TreeViewDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			string designTimeHtml = base.GetDesignTimeHtml ();
			MsgTreeView treeView = (MsgTreeView)this.Component;
			
		
			return "Message tree view for <b>" + treeView.BaseTreeView + "</b>{" +  treeView.MsgSource + "}";
		}
	}
}
