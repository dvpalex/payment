using System;
using System.Web;
using System.Collections;
using SuperPag.Framework;

namespace SuperPag.Framework.Web.WebController
{
	/// <summary>
	/// TODO: Comentário
	/// </summary>
	public abstract class BaseEvent
	{
		//TODO: Comentario
		MessageState _messages = MessageState.SoleInstance();
		protected MessageState Messages
		{get{return _messages;}set{_messages = value;}}

        private Hashtable _parameters = new Hashtable();
		private BaseCommand _command;

		public BaseEvent()
		{
		}
			

		//TODO: Comentario
		public BaseCommand MakeCommand(Type commandType)
		{
			CommandFactory factory = new CommandFactory();
			return factory.Make(commandType);
		}

		//TODO: Comentario
		public HttpContext Context
		{get {  return HttpContext.Current;}}

		//TODO: Obsoleto
		protected Message GetMessage(string key)
		{return (Message)_messages.Get(key);}

		//TODO: Obsoleto
		protected MessageCollection GetMessageArray(string key)
		{return (MessageCollection)_messages.GetArray(key);}

		//TODO: Obsoleto
		protected Message GetMessage(Type message)
		{return (Message)_messages.Get(message);}

		//TODO: Obsoleto
		protected MessageCollection GetMessageArray(Type message)
		{return (MessageCollection)_messages.GetArray(message);}

		/// <summary>
		/// Obtem a mensagem selecionada por um dropdown list
		/// </summary>
		public Message GetSelectedDropDownListMessage(Type message)
		{ return (Message)_messages.GetSelectedDropDownListMessage(message); }

		/// <summary>
		/// Obtem a mensagem selecionada por um dropdown list
		/// </summary>
		public Message GetSelectedDropDownListMessage(string ListItemSource, string key)
		{ return (Message)_messages.GetSelectedDropDownListMessage(ListItemSource, key); }

		//Pega valor adicionado
		protected object GetValue( string key )
		{return _messages.GetPropValue( key ); }

		CommandStackManager _stackMngr = new CommandStackManager();

		protected CommandStackManager CommandStack
		{get { return _stackMngr;}}


		//TODO: Comentario
		protected abstract BaseCommand OnExecute();

		//TODO: Comentario
		public BaseCommand Command
		{get {return _command;}}

		//TODO: Comentario
		public Hashtable Parameters
		{get{ return _parameters; }}

		//TODO: Comentário
		private Exception _error;
		public Exception Error
		{get {return _error;} set{ _error = value; }}
		
		//TODO: Comentario
		public void Execute(Hashtable parameters)
		{
			this._parameters = parameters;

			EventTrap et = new EventTrap();
			et.PreProcessing(this);
			
			BaseCommand command = null;
			try
			{
				command = OnExecute();
			} 			
			catch(Exception ex)
			{
				//todo: coxa
				if(ex is SuperPag.Framework.Data.Components.Data.DeleteConstraintException ||
                  ex is SuperPag.Framework.Data.Components.Data.DuplicatedKeyException)
				{
					this.Error = ex;
					HttpContext.Current.Items["__DELETE_ERROR"] = ex;
				}
                else
                  throw ex;


			}
			
			_command = command;

			et.PostProcessing(this);

			if(this.Error == null)
			{
				if(command != null)
				{
					command.Execute();
				}
			}
		}

		//TODO: Obsoleto
		public BaseCommand LastView()
		{			
			return _stackMngr.LastView();
		}
		
	}
}
