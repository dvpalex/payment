using System;

namespace SuperPag.Framework.Web.WebController
{
	public class FWCWebException : FWCException 
	{
		private string _homeUrl;
		private string _commandAssembly;
		private string _commandNamespace;
		private string _moduleAcronym;

		public string ModuleAcronym
		{get { return _moduleAcronym; }}

		public string HomeUrl
		{get { return _homeUrl; }}
		
		public string CommandAssembly
		{get { return _commandAssembly; }}
		
		public string CommandNamespace
		{get { return _commandNamespace; }}

		public FWCWebException(string message) : base (message)
		{
			FillProperties();
		}

		public FWCWebException(
			Exception innerException, string message) : base (message, innerException)
		{
			FillProperties();
		}

		private void FillProperties()
		{
			CommandStackManager csMngr = new CommandStackManager();
			BaseCommand b = csMngr.GetLastCommand();
			if(b == null) { return; }
			_moduleAcronym = ModuleConfigurationHelper.GetModuleAcronym(b.GetType().Assembly);
			_commandNamespace = b.GetType().FullName;
			_commandAssembly = b.GetType().Assembly.GetName().Name;
			_homeUrl = "~/" + _moduleAcronym;
		}
	}

	public class WebNavigationException : FWCWebException 
	{
		public WebNavigationException(string message) : base (message) {}
	}

	public class NotAuthorizedException : WebNavigationException
	{
		public NotAuthorizedException() : base ("") {}
	}

	public class NavigationFlowException : WebNavigationException 
	{
		public NavigationFlowException(
			string aspxUrl) :
			base ("Duplicated post in " + aspxUrl)
		{			
		}		
	}

	public class DirectAccessException : WebNavigationException 
	{
		public DirectAccessException() : base (""){}

	}

	
}
