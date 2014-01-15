using System;
using System.Web;
using System.Collections;



namespace  SuperPag.Framework.Web.WebController
{

    /// <summary>
	/// TODO: Comentario
	/// </summary>
	public class CommandStackManager
	{
		
		// TODO: Comentario
		[Serializable]public class CommandInfo
		{
			public Hashtable Parameters;
			public string Name;
			public string AssemblyName;
			public string FullName;
			public string View;
			public long Time;
			public string id;
		}

//		public void MarkCommand(BaseCommand command, string key)
//		{
//			CommandStack cs;
//
//			if(HttpContext.Current.Session["commandstack"] != null)
//			{
//				cs = (CommandStack)HttpContext.Current.Session["CommandStack"];
//			}
//			else 
//			{
//				cs = new CommandStack();
//			}
//
//			cs.Add(command.Name, commandParameters, viewName, key);
//
//			HttpContext.Current.Session["CommandStack"] = cs;
//		}

		//TODO: Comentário
		public bool LastCommandIs(string commandName)
		{
			if(HttpContext.Current.Session["CommandStack"] != null)
			{
				CommandStack cs = (CommandStack) HttpContext.Current.Session["CommandStack"];
				CommandStackManager.CommandInfo c = (CommandStackManager.CommandInfo)cs.Stack[cs.Stack.Count - 1];
				return String.Compare(c.Name, commandName, true) == 0;
			}	
			else
			{
				return false;
			}
		}

		public BaseCommand GetLastCommand()
		{
			CommandStack cs;

			if(HttpContext.Current.Session["commandstack"] != null)
			{
				cs = (CommandStack)HttpContext.Current.Session["CommandStack"];
				if(cs.Stack.Count > 0)
				{
					//TODO: implementar no Factory
					CommandInfo c = (CommandInfo)cs.Stack[cs.Stack.Count-1];

					CommandFactory factory = new CommandFactory();
					BaseCommand command = factory.Make(c.AssemblyName, c.FullName);
					command.SetParams(c.Parameters);
					command.ID = c.id;

					return command;
				}		
			}
			return null;
		}


		public CommandInfo GetLastCommandInfo()
		{
			CommandStack cs;

			if(HttpContext.Current.Session["commandstack"] != null)
			{
				cs = (CommandStack)HttpContext.Current.Session["CommandStack"];
				if(cs.Stack.Count > 0)
				{
					CommandInfo c = (CommandInfo)cs.Stack[cs.Stack.Count-1];
					return c;
				}		
			}
			return null;
		}

        public CommandInfo GetCommandById(string commandId, bool unPile)
        {
            CommandStack cs;

            if (HttpContext.Current.Session["commandstack"] != null)
            {
                cs = (CommandStack)HttpContext.Current.Session["CommandStack"];

                string currentView =
                    ((CommandStackManager.CommandInfo)cs.Stack[cs.Stack.Count - 1]).View.ToLower();

                //todo: verificar command stack e stack manager
                for (int i = cs.Stack.Count - 1; i >= 0; i--)
                {
                    CommandInfo c = (CommandInfo)cs.Stack[i];
                    if (c.View.ToLower() != currentView)
                    {
                        if (c.id == commandId)
                        {
                            if (unPile)
                            {
                                //remove os items posteriores e o proprio comando requisitado
                                for (int j = (cs.Stack.Count - 1); j >= i; j--)
                                {
                                    cs.Stack.RemoveAt(j);
                                }
                            }
                            return c;
                        }
                    }
                }
            }
            return null;
        }

		public CommandInfo GetCommand(string commandId, bool unPile)
		{
			CommandStack cs;

			if(HttpContext.Current.Session["commandstack"] != null)
			{
				cs = (CommandStack)HttpContext.Current.Session["CommandStack"];

				string currentView = 
					((CommandStackManager.CommandInfo)cs.Stack[cs.Stack.Count - 1]).View.ToLower();

				//todo: verificar command stack e stack manager
				for(int i = cs.Stack.Count - 1; i >= 0 ; i --)
				{
					CommandInfo c = (CommandInfo)cs.Stack[i];
					if(c.View.ToLower() != currentView)
					{
						if(c.Name == commandId)
						{
							if(unPile)
							{
								//remove os items posteriores e o proprio comando requisitado
								for(int j = (cs.Stack.Count-1) ; j >= i; j --)
								{
									cs.Stack.RemoveAt(j);
								}
							}
							return c;
						}
					}
				}
			}
			return null;
		}

        public BaseCommand GetBaseCommandById(string commandId, bool unPile)
        {
            CommandInfo c = this.GetCommandById(commandId, unPile);

            if (c != null)
            {
                CommandFactory factory = new CommandFactory();
                BaseCommand command = factory.Make(c.AssemblyName, c.FullName);
                command.SetParams(c.Parameters);
                command.ID = c.id;

                return command;
            }

            return null;
        }

		public BaseCommand GetBaseCommand(string commandId, bool unPile)
		{
			CommandInfo c = this.GetCommand( commandId, unPile );
			
			if ( c != null ) 
			{
				CommandFactory factory = new CommandFactory();
				BaseCommand command = factory.Make(c.AssemblyName, c.FullName);
				command.SetParams(c.Parameters);
				command.ID = c.id;

				return command;
			}

			return null;
		}

		public CommandInfo GetCommand(string commandName)
		{
			CommandStack cs;

			if(HttpContext.Current.Session["commandstack"] != null)
			{
				cs = (CommandStack)HttpContext.Current.Session["CommandStack"];
				//todo: verificar command stack e stack manager
				for(int i = cs.Stack.Count - 1; i >= 0 ; i --)
				{
					CommandInfo c = (CommandInfo)cs.Stack[i];
					if(c.Name == commandName)
					{
						return c;
					}
				}
			}
			return null;
		}

		public void AddCommand(
			string commandName,
			string fullName,
			string assemblyName,
			string commandId,
			Hashtable commandParameters,
			string viewName)
		{
			CommandStack cs;

			if(HttpContext.Current.Session["commandstack"] != null)
			{
				cs = (CommandStack)HttpContext.Current.Session["CommandStack"];
			}
			else 
			{
				cs = new CommandStack();
			}

			cs.Add(commandName, fullName, assemblyName, commandId, commandParameters, viewName);

			HttpContext.Current.Session["CommandStack"] = cs;
		}	
	
		//TODO: Comentário
		public BaseCommand LastView()
		{
			//Verifico se tem comando na pilha
			if(HttpContext.Current.Session["CommandStack"] != null)
			{
				//obtenho a pilha
				CommandStack commandStack = (CommandStack)
					HttpContext.Current.Session["CommandStack"];
				
				return commandStack.LastView();
			}
			else 
			{
				throw new Exception("Comando de destino não encontrado na pilha");
			}
		}
		
	}
		
	/// TODO: Comentario
	[Serializable()]
	public class CommandStack
	{
		// TODO: Comentario
		ArrayList _commands = new ArrayList();

		// TODO: Comentario
		public void Add(
			string commandName, 
			string fullName,
			string assemblyName,
			string commandId, 
			Hashtable parameters, 
			string viewName)
		{
			CommandStackManager.CommandInfo commandInfo 
				= new CommandStackManager.CommandInfo();
			commandInfo.Parameters = parameters;
			commandInfo.FullName = fullName;
			commandInfo.id = commandId;
			commandInfo.AssemblyName = assemblyName;
			commandInfo.Name = commandName;
			commandInfo.Time = DateTime.Now.Ticks ;
			commandInfo.View = viewName;
			_commands.Add(commandInfo);

			while(_commands.Count > 20)
			{
				_commands.RemoveAt(0);
			}
		}

		public BaseCommand LastView()
		{
			string currentView = ((CommandStackManager.CommandInfo)_commands[_commands.Count - 1]).View.ToLower();
			for(int i = _commands.Count - 1; i >= 0; i--)
			{
				CommandStackManager.CommandInfo commandInStack = (CommandStackManager.CommandInfo)_commands[i];
				if(commandInStack.View.ToLower() != currentView)
				{
					CommandFactory factory = new CommandFactory();
					BaseCommand command = factory.Make(commandInStack.AssemblyName, commandInStack.FullName);
					command.SetParams(commandInStack.Parameters);
					command.ID = commandInStack.id;

					_commands.RemoveAt(_commands.Count - 1);

					return command;
				}
			}
			return null;
		}

		// TODO: Comentario
		public ArrayList Stack
		{
			get
			{
				return _commands;
			}
		}

	}
}
