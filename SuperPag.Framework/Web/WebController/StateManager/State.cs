using System;
using System.Configuration;
using System.Collections;

namespace SuperPag.Framework.Web.WebController
{
	/// <summary>
	/// </summary>
	public class StateManager
	{
		// Define o local onde os itens devem ser persistidos
		// default = ViewState
//		private static PersistLocations _persistLocation = 
//			PersistLocations.ViewState; 
//		private enum PersistLocations
//		{
//			ViewState = 1,
//			SessionState = 2,
//			EFinancialState = 3
//		}

		//Itens
		protected virtual System.Web.SessionState.HttpSessionState Items
		{get{return System.Web.HttpContext.Current.Session;}}

		/// <summary>
		/// Construtor estático, inicializa o local de persistência
		/// dos itens
		/// </summary>
		static StateManager()
		{
//			_persistLocation = PersistLocations.SessionState;
		}

		/// <summary>
		/// Adiciona um item
		/// </summary>
		protected virtual void Add(string key, object _value)
		{
			System.Web.HttpContext.Current.Session.Add(key, _value);
		}

		/// <summary>
		/// Adiciona um item ou se já existir, atualiza seu valor
		/// </summary>
		protected virtual void AddOrUpdate(string key, object _value)
		{
			//			if(_items.Conte(key))
			//			{
			//				_items[key] = _value;				
			//			} 
			//			else
			//			{
			Add(key, _value);
			//			}			
		}
		
		/// <summary>
		/// Obtem o valor de um item
		/// </summary>
		public virtual object GetValue(string key)
		{
			return System.Web.HttpContext.Current.Session[key];
		}

	}
}
