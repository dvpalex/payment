using System;
using System.Web;
using System.Collections;
using System.Security.Principal;
using SuperPag.Framework;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebController
{
	/// <summary>
	/// Summary description for Command.
	/// </summary>
	public abstract class BaseCommand
	{
		private Hashtable _parameters = new Hashtable();
		private ViewInfo _viewInfo;
		private string _ID;
		private Exception _error = null;
	
			//TODO: Comentário
		public Exception Error
		{get {return _error;} set{ _error = value; }}
		
		//TODO: Comentario
		protected abstract ViewInfo OnExecute();

		//TODO: Comentario
		public HttpContext Context
		{get {return HttpContext.Current;}}

        public IPrincipal User
        {
            get { return HttpContext.Current.User; }
        }

		//TODO: Comentario
		public Hashtable Parameters
		{get {return _parameters;}}

		//TODO: Comentario
		public ViewInfo View
		{get {return _viewInfo;}}

			//TODO: Comentário
		public string ID
		{get {return _ID;} set { _ID = value; }}

	
		public BaseCommand()
		{
		}

		//TODO: Comentario
		public void SetParams(Hashtable _parameters)
		{
			this._parameters = _parameters;
		}

		//TODO: Comentario
		public void Execute()
		{	
				CommandTrap ct = new CommandTrap();
				ct.PreProcessing(this);

				ViewInfo v = OnExecute();

				_viewInfo = v;

				ct.PostProcessing(this);

				if(v != null)
				{
                    Context.Items.Add("PageTitle", v.Title);
					Context.Server.Transfer(v.Url);                    
				} 			
		}

		//TODO: Comentario
		MessageState _messageState = MessageState.SoleInstance();
		protected MessageState Messages
		{get{return _messageState;}set{_messageState = value;}}

		//TODO: Comentario
		CommandStackManager _stackManager = new CommandStackManager();
		public CommandStackManager StackManager
		{get{return _stackManager;}set{_stackManager = value;}}

		//TODO: Comentario
		private bool _useStack = true;
		public bool UseStack
		{get{return _useStack;}set{_useStack = value;}}

		//TODO: Obsoleto
		protected void AddEnumeration(
			EnumKeyText[] enumItems, 
			Type enumType, 
			string key)
		{
			_messageState.Add(enumItems, enumType, key);
		}

		//TODO: Obsoleto
		protected void AddEnumeration(EnumListBuilderBase listBuilder)
		{
			_messageState.Add(listBuilder);
		}

		//TODO: Obsoleto
		protected void AddEnumeration(EnumListBuilderBase listBuilder, string key)
		{
			_messageState.Add(listBuilder, key);
		}

		//TODO: Obsoleto
		protected void AddMessage(Message message, string key)
		{
			_messageState.Add(message, key);		
		}

		//TODO: Obsoleto
		protected void AddMessage(Message message)
		{
			_messageState.Add(message);			
		}
		
		//TODO: Obsoleto
		protected void AddMessage(MessageCollection cmessage)
		{
			_messageState.Add(cmessage);
		}

		//TODO: Obsoleto
		protected void AddMessage(MessageCollection cmessage, string key)
		{
			_messageState.Add(cmessage, key);
		}

		//TODO: Comentario
		Hashtable _valuesQueue = new Hashtable();
		
		//Add values
		protected void AddValue( object value, string key )
		{
			_messageState.AddPropValue( value, key );
		}
	}
}


