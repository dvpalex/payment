using System;
using System.Configuration;
using System.Collections;
using System.Reflection;

namespace SuperPag.Framework.Configuration
{
	/// <summary>
	/// Provides a base class for customized configuration setcion in AppConfig file
	/// </summary>
	public abstract class BaseConfigurationSettings : IConfigurationSectionHandler
	{
		#region reader

		/// <summary>
		/// Get a methodInfo reference to specified method name in declaring type.
		/// </summary>
		/// <param name="declaringType">Type that declare the method</param>
		/// <param name="methodName">The method to get the methodInfo reference</param>
		/// <returns>MethodInfo</returns>
		public static MethodInfo GetReconfigureMethod(Type declaringType, string methodName)
		{
			return declaringType.GetMethod( methodName , BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
		}

		/// <summary>
		/// Provides a way to initialize derived class, reading the configuration file and attaching to the file system watcher
		/// </summary>
		/// <param name="sectionName">The name of section to read</param>
		/// <param name="reconfigureMethod">The method info that reconfigure the object. Will be call by the file system watcher on configuration file changes</param>
		/// <returns>BaseConfigurationSettings</returns>
		public static BaseConfigurationSettings Initalize( string sectionName ) //, MethodInfo reconfigureMethod 
		{
			BaseConfigurationSettings settings =  Configure ( sectionName );			
		
			// ConfigurationWatcher.AttachStaticMethod( reconfigureMethod );

			// ConfigurationWatcher.Start();

			return settings;
		}

		/// <summary>
		/// Configure the class and read configuration file
		/// </summary>
		/// <param name="sectionName">The name of section to read</param>
		/// <returns>BaseConfigurationSettings</returns>
		public static BaseConfigurationSettings Configure( string sectionName )
		{
			return ((BaseConfigurationSettings)ConfigurationSettings.GetConfig( sectionName ));
		}

		#endregion	

		#region create

		/// <summary>
		/// Member of IConfigurationSectionHandler. Used to parse the XML of the configuration section in derived classes
		/// </summary>
		public abstract object Create(object parent, object configContext, System.Xml.XmlNode section);

		#endregion create
	}
}
