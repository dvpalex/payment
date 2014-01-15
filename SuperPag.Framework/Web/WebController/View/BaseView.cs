using System;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SuperPag.Framework;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebController
{
	/// <summary>
	/// Classe abstrata responsável pela apresentação de uma visão
	/// </summary>
	public abstract class BaseView : Page
	{
		private MessageState _messages = MessageState.SoleInstance();
		private bool _clearMemory = true;
		private bool _allowRepost = false;
		private HtmlForm _htmlForm;

		
		CommandStackManager _stackMngr = new CommandStackManager();

		protected CommandStackManager CommandStack
		{get { return _stackMngr;}}

	
		protected bool AllowRepost
		{get { return _allowRepost;} set { _allowRepost = value;}}

		/// <summary>
		/// Se a página for uma popup adicionadas na página
		/// </summary>
		public bool ClearBeforePersitMemory
		{get{return _clearMemory;} set{_clearMemory = value;}}

		/// <summary>
		/// Mensagens adicionadas na página
		/// </summary>
		public virtual MessageState Messages
		{get{return _messages;}}

		public HtmlForm Form
		{get{return _htmlForm;}}
		
		/// <summary>
		/// Salva todas as mensagens que estão no contexto para o 
		/// StateServer
		/// </summary>
		protected virtual void SaveContext()
		{
			_messages.PersistMemory();
		}

		/// <summary>
		/// Obtem todas as mensagens peristidas no StateServer e
		/// atualiza o Contexto
		/// </summary>
		protected virtual void RefillContext()
		{
			_messages.RefillMemory();
		}

		protected virtual bool HandleError(out Exception exception)
		{
			ErrorHandler error = new ErrorHandler();
			exception = Server.GetLastError();

			if(!(exception is FWCException))
			{
				error.Handle(exception);

				FWCWebException webException = new FWCWebException(exception, "");
				exception = webException;
			} 
			
			return true;
		}

		protected virtual bool CheckDirectAccess()
		{
			return true;
		}

		protected override void OnError(EventArgs e)
		{			
			Exception exception;
			if(HandleError(out exception))
			{
				throw exception;
			} 
			else
			{
				base.OnError(e);
			}
		}

		/// <summary>
		/// On Init
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			if(CheckDirectAccess())
			{
				if(Request.HttpMethod.ToUpper() == "GET" && Request.RawUrl.ToLower().IndexOf(".do") == -1)
				{
					throw new DirectAccessException();
				}
			}

			base.OnInit (e);

			foreach(Control c in this.Controls)
			{
				if(c is HtmlForm) { _htmlForm = (HtmlForm)c; break; }
			}
			
			if(Page.IsPostBack)
			{
				_messages.RefillMemory();
			}

		
		}

		public string URL;
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			URL = this.Page.ResolveUrl ( "~" );

			if(! Page.IsPostBack)
			{
				string postId = Guid.NewGuid().ToString();
				ViewState["__POSTID"] = postId;
			}
		}

		private void SavePost()
		{
			string postId = ViewState["__POSTID"].ToString();
			ArrayList postStack;

			if(Session["__POSTSTACK"] is string[])
			{
				postStack = new ArrayList((string[])Session["__POSTSTACK"]);
			} 
			else
			{
				postStack = new ArrayList();
			}
			postStack.Add(postId);
			while(postStack.Count > 25)
			{
				postStack.RemoveAt(0);
			}
			Session["__POSTSTACK"] = postStack.ToArray(typeof(String));
		}


		protected virtual bool ValidatePost()
		{
			if(_allowRepost) { return true; }

			ArrayList postStack;

			if(Session["__POSTSTACK"] is string[])
			{
				postStack = new ArrayList((string[])Session["__POSTSTACK"]);
				return postStack.IndexOf(ViewState["__POSTID"]) == -1;				
			} 
			else
			{
				return true;
			}
		}
 
		protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
		{
			if(ValidatePost())			
			{
                SavePost();
				string postId = Guid.NewGuid().ToString();
				ViewState["__POSTID"] = postId;
				base.RaisePostBackEvent (sourceControl, eventArgument);
			} 
			else
			{
				CommandStackManager csMngr = new CommandStackManager();
				BaseCommand b = csMngr.GetLastCommand();
				if(b != null) 
				{
					b.Execute();
				} 
				else
				{
					throw new NavigationFlowException(Request.Url.AbsoluteUri);
				}
			}						
		}
		
		/// <summary>
		/// On Unload
		/// </summary>
		/// <param name="e"></param>
		protected override void OnUnload(EventArgs e)
		{
            //Limpa as mensagens
            if (_clearMemory) _messages.ClearMemory();

            //Persiste as mensagens
            _messages.PersistMemory();

            base.OnUnload(e);
		}

		//TODO: Obsoleto
		protected Message GetMessage(string key)
		{return (Message)_messages.Get(key);}

		//TODO: Obsoleto
		protected MessageCollection GetMessageArray(string key)
		{return (MessageCollection)_messages.GetArray(key);}

		//TODO: Obsoleto
		public Message GetMessage(Type message)
		{return (Message)_messages.Get(message);}

		//TODO: Obsoleto
		protected MessageCollection GetMessageArray(Type message)
		{return (MessageCollection)_messages.GetArray(message);}

		//TODO: Obsoleto
		protected object GetValue(string key)
		{return (object)_messages.GetPropValue(key);}

		/// <summary>
		/// Obtem a mensagem selecionada por um dropdown list
		/// </summary>
		public Message GetSelectedDropDownListMessage(Type message)
		{ return (Message)_messages.GetSelectedDropDownListMessage(message); }

		/// <summary>
		/// Obtem a mensagem selecionada por um dropdown list
		/// </summary>
		public Message GetSelectedDropDownListMessage(string ListItemSource, string key)
		{ return (Message)_messages.GetSelectedDropDownListMessage( ListItemSource, key ); }

		//TODO: Obsoleto
		protected void AddEnumeration(
			EnumKeyText[] enumItems, 
			Type enumType, 
			string key)
		{
			_messages.Add(enumItems, enumType, key);
		}

		//TODO: Obsoleto
		protected void AddEnumeration(EnumListBuilderBase listBuilder)
		{
			_messages.Add(listBuilder);
		}

		//TODO: Obsoleto
		protected void AddEnumeration(EnumListBuilderBase listBuilder, string key)
		{
			_messages.Add(listBuilder, key);
		}


		//TODO: Obsoleto
		protected void AddMessage(Message message, string key)
		{
			_messages.Add(message, key);		
		}
		
		//TODO: Obsoleto
		protected void AddMessage(Message message)
		{
			_messages.Add(message);			
		}
		
		//TODO: Obsoleto
		protected void AddMessage(MessageCollection cmessage)
		{
			_messages.Add(cmessage);
		}

		//TODO: Obsoleto
		protected void AddMessage(MessageCollection cmessage, string key)
		{
			_messages.Add(cmessage, key);
		}

		//TODO: Comentário
		protected void AddValue(object value, string key)
		{
			_messages.AddPropValue(value, key);
		}

		//TODO: Comentário
		protected void ClearDropDownMessage( string ItemSource, string key )
		{
			_messages.ClearDropDownMessage( ItemSource, key );
		}

		/// <summary>
		/// Cria e insere um script de alert na página com o texto informado
		/// </summary>
		/// <param name="Id">Id do script</param>
		/// <param name="Text">Texto a ser exibido no alert</param>
		protected void ShowAlert(string Id, string Text) 
		{
			string script = "<script language='JavaScript'>\r\n";
			script += "\talert('" + Text.Replace("'", "\\'").Replace("\r", "\\r").Replace("\n","\\n") + "');\r\n";
			script += "</script>\r\n";

			this.RegisterStartupScript(Id, script);
		}

	}		
}
