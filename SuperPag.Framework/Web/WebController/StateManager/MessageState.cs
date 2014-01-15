using System;
using System.Collections;
using SuperPag.Framework;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebController
{
	/// <summary>
	/// Classe que gerencia o estado das mensagens trafegadas
	/// entre command, events e views.
	/// </summary>
	public class MessageState : StateManager
	{		
		private  System.Web.HttpContext _context = 
			System.Web.HttpContext.Current;

		private Hashtable _messages = new Hashtable();
		private Hashtable _values = new Hashtable();

		/// <summary>
		/// Mensagens adicionadas na página
		/// </summary>
		protected virtual Hashtable Messages
		{get{return _messages;}}

		/// <summary>
		/// Valores adicionados na página
		/// </summary>
		protected virtual Hashtable Values
		{get{return _values;}}

		public static MessageState SoleInstance()
		{
			if(System.Web.HttpContext.Current == null) 
				return new MessageState();

			Hashtable messages;
			if(System.Web.HttpContext.Current.Items.Contains("__MESSAGES"))
			{
				messages = (Hashtable)System.Web.HttpContext.Current.Items["__MESSAGES"];
			} 
			else
			{
				messages = new Hashtable();
				System.Web.HttpContext.Current.Items.Add("__MESSAGES", messages);
			}

			Hashtable values;
			if(System.Web.HttpContext.Current.Items.Contains("__VALUES"))
			{
				values = (Hashtable)System.Web.HttpContext.Current.Items["__VALUES"];
			} 
			else
			{
				values = new Hashtable();
				System.Web.HttpContext.Current.Items.Add("__VALUES", values);
			}

			return new MessageState(messages, values);
		}

		private MessageState(Hashtable messages, Hashtable values)
		{
			this._messages = messages;
			this._values = values;
		}

		private MessageState()
		{
			this._messages = new Hashtable();
			this._values = new Hashtable();
		}

		public void Clear()
		{
			this.Messages.Clear();
			this.Values.Clear();
			this.ClearMemory();
		}

		/// <summary>
		/// Salva todas as mensagens que estão no contexto para o 
		/// StateServer
		/// </summary>
		public void PersistMemory()
		{
			if(_messages != null)
			{
				IDictionaryEnumerator enumerator = 
					(IDictionaryEnumerator)_messages.GetEnumerator();

				while(enumerator.MoveNext()) 
				{ 
					base.Add("__M_" + enumerator.Key.ToString(), enumerator.Value);
				}
			}
			if(_values != null)
			{
				IDictionaryEnumerator enumerator = 
					(IDictionaryEnumerator)_values.GetEnumerator();

				while(enumerator.MoveNext()) 
				{ 
					base.Add("__V_" + enumerator.Key.ToString(), enumerator.Value);
				}
			}
		}

		/// <summary>
		/// Limpa todas as mensagens do StateServer
		/// </summary>
		public void ClearMemory()
		{
			if(Items != null)
			{
				ArrayList itemsKey = new ArrayList();

				IEnumerator e = Items.GetEnumerator();
				while(e.MoveNext())
				{
					string key = e.Current.ToString();
					if(key.StartsWith("__M_") || key.StartsWith("__V_"))
					{
						itemsKey.Add(key);
					}
				}

				foreach(string key in itemsKey)
				{
					Items.Remove(key);
				}
			}
		}

		/// <summary>
		/// Obtem todas as mensagens peristidas no StateServer e
		/// atualiza o Contexto
		/// </summary>
		public void RefillMemory()
		{
			if(Items != null)
			{
				ArrayList itemsKey = new ArrayList();

				IEnumerator e = Items.GetEnumerator();
				while(e.MoveNext())
				{
					string key = e.Current.ToString();
					if( key.StartsWith("__M_") )
					{
						_messages[key.Substring(4, key.Length - 4)] = Items[key];
						itemsKey.Add(key);
					} 
					else if (  key.StartsWith("__V_") ) 
					{
						_values[key.Substring(4, key.Length - 4)] = Items[key];
						itemsKey.Add(key);
					}
				}

				foreach(string key in itemsKey)
				{
					Items.Remove(key);
				}

			}
		}

		#region Memory State Manager

		/// <summary>
		/// Adiciona uma enumeração no contexto
		/// </summary>
		public void Add(
			EnumKeyText[] enumItems, 
			Type enumType, 
			string key)
		{
			if(enumItems != null && enumItems.Length > 0)
				_messages["Enum" + key] = enumItems;
		}
		
		/// <summary>
		/// Adiciona uma enumeração no contexto
		/// </summary>

		/// <summary>
		/// Adiciona uma enumeração no contexto
		/// </summary>
		public void Add(EnumListBuilderBase listBuilder)
		{
			EnumKeyText[] _enum = listBuilder.GetList();
			if(_enum != null && _enum.Length > 0)
				_messages["Enum" + listBuilder.TypeName] = _enum;
		}

		/// <summary>
		/// Adiciona uma enumeração no contexto
		/// </summary>
		public void Add(EnumListBuilderBase listBuilder, string key)
		{
			EnumKeyText[] _enum = listBuilder.GetList();
			if(_enum != null && _enum.Length > 0)
				_messages["Enum" + key] = _enum;
		}

		/// <summary>
		///Adiciona uma mensagem no contexto
		/// </summary>
		public void Add(Message message, string key)
		{
			if(message != null)
				_messages[key] = message;
		}

		/// <summary>
		/// Adiciona uma mensagem no contexto
		/// </summary>
		public void Add(Message message)
		{
			if(message != null)
				_messages[GetBaseTypeName(message.GetType())] = message;
		}
		
		/// <summary>
		/// Adiciona um array de mensagens no contexto
		/// </summary>
		public void Add(MessageCollection cmessage)
		{
			if(cmessage != null)
				//RAFAEL:
				//_messages["ArrayOf" + GetBaseTypeName(cmessage.GetType().GetElementType())] = cmessage;
				_messages[ GetBaseTypeName(cmessage.GetType()) ] = cmessage;
			}

		/// <summary>
		/// Adiciona um array de mensagens no contexto
		/// </summary>
		public void Add(MessageCollection cmessage, string key)
		{
			if( cmessage != null )
				_messages[key] = cmessage;	
		}

		/// <summary>
		///Adiciona um valor no contexto
		/// </summary>
		public void AddPropValue( object value, string key )
		{
			_values[key] = value;
		}

		/// <summary>
		///Adiciona um valor no contexto
		/// </summary>
		public void AddSelectedDropDownListMessage( Message message )
		{
			if(message != null)
				_messages["___DROPDOWN_" + GetBaseTypeName(message.GetType())] = message;
		}

		/// <summary>
		///Adiciona um valor no contexto
		/// </summary>
		public void AddSelectedDropDownListMessage( Message message, string ItemSource, string key )
		{
			if(message != null)
				_messages["___DROPDOWN_" + ItemSource + "_" + key] = message;
		}

		/// <summary>
		///Limpa um valor no contexto
		/// </summary>
		internal void ClearDropDownMessage( Type type )
		{
			_messages["___DROPDOWN_" + GetBaseTypeName(type)] = null;
		}
		internal void ClearDropDownMessage( string ItemSource, string key )
		{
			_messages["___DROPDOWN_" + ItemSource + "_" + key] = null;
		}

		/// <summary>
		///Adiciona uma mensagem no contexto
		/// </summary>
		public System.Array GetEnum(string key)
		{{return (System.Array)_messages[key];}}


		/// <summary>
		/// Obtem uma mensagem do contexto pela chave
		/// </summary>
		public Message Get(string key)
		{return (Message)_messages[key];}

		/// <summary>
		/// Obtem um array de mensagens do contexto pela chave
		/// </summary>
		//RAFAEL:
		//public Message[] GetArray(string key)
		//{return (Message[])_messages[key];}
		public MessageCollection GetArray(string key)
		{return (MessageCollection)_messages[key];}

		/// <summary>
		/// Obtem uma mensagem do contexto pelo tipo
		/// </summary>
		public Message Get(Type message)
		{return (Message)_messages[GetBaseTypeName(message)];}

		/// <summary>
		/// Obtem um array de mensagens do contexto pelo tipo
		/// </summary>
		// RAFAEL:
		//public Message[] GetArray(Type message)
		//{return (Message[])_messages["ArrayOf" + GetBaseTypeName(message)];}
		public MessageCollection GetArray(Type cmessage)
		{return (MessageCollection)_messages[GetBaseTypeName(cmessage)];}

		//Obtenho a classe base de um tipo
		private string GetBaseTypeName(Type type)
		{
			//RAFAEL: VERIFICAR
			string typeName = type.Name;
			while(type.Name != "Message" && type.Name != "MessageCollection" )
			{
				typeName = type.Name;
				type = type.BaseType;
			}
			return typeName;
		}

		/// <summary>
		/// Obtem a mensagem selecionada por um dropdown list
		/// </summary>
		public Message GetSelectedDropDownListMessage(Type message)
		{return (Message)_messages["___DROPDOWN_" + GetBaseTypeName(message)];}

		/// <summary>
		/// Obtem a mensagem selecionada por um dropdown list
		/// </summary>
		public Message GetSelectedDropDownListMessage( string ListItemSource, string key )
		{return (Message)_messages["___DROPDOWN_" + ListItemSource + "_" + key];}

		/// <summary>
		/// Obtem um valor do contexto pela chave
		/// </summary>
		public object GetPropValue(string key)
		{return (object)_values[key];}

		#endregion
	}	
}
