using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Configuration;
using SuperPag.Framework.Configuration;

namespace SuperPag.Framework.Web.Configuration
{
	/// <summary>
	/// Customized configuration setcion in AppConfig file
	/// </summary>
	internal class ConfigurationSettings : BaseConfigurationSettings
	{		
		const string CONFIG_PATH = "superPagFrameworkWebSettings";

		#region Static

		private static ConfigurationSettings _settings;		
		public static ConfigurationSettings AppSettings { get { return _settings; } }

		static ConfigurationSettings()
		{
			_settings = (ConfigurationSettings)BaseConfigurationSettings.Initalize( CONFIG_PATH );
		}

		static void ReConfigure()
		{
			_settings = (ConfigurationSettings)BaseConfigurationSettings.Configure( CONFIG_PATH );
		}

		#endregion Static

		#region IConfigurationSectionHandler Members

		public override object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			GetCommandsMap( section );

			return this;
		}

		private void GetCommandsMap(System.Xml.XmlNode section)
		{
			XmlNode ndlst = section.SelectSingleNode( "commandsMap" );

			_commandMaps = new Hashtable();

			if (ndlst != null)
			{
				foreach( XmlNode nd in ndlst.ChildNodes )
				{
					_commandMaps.Add( nd.Attributes[ "extension" ].Value.ToLower(), nd.Attributes[ "commandAssembly" ].Value );
				}
			}
		}
	
		#endregion

		#region fields

		private Hashtable _commandMaps;
		
		public Hashtable CommandMaps { get { return _commandMaps; } }

		#endregion

	}
}

