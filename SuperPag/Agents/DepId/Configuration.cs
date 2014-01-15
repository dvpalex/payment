using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Configuration;
using SuperPag.Framework.Configuration;

namespace SuperPag.Agents.DepId.Configuration
{
	/// <summary>
	/// Customized configuration setcion in AppConfig file
	/// </summary>
	public class ConfigurationSettings : BaseConfigurationSettings
	{
        const string CONFIG_PATH = "retornoDepIdSettings";

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
			XmlNode ndlst = section.SelectSingleNode( "extensionMap" );

            _extensionMaps = new List<MExtensionMap>();

			if (ndlst != null)
			{
				foreach( XmlNode nd in ndlst.ChildNodes )
				{
                    MExtensionMap mExtensionMap = new MExtensionMap();
                    mExtensionMap.Extension = nd.Attributes["extension"].Value.ToLower();
                    mExtensionMap.CnabType = int.Parse(nd.Attributes["cnabType"].Value);
					_extensionMaps.Add(  mExtensionMap );
				}
			}
		}
	
		#endregion

		#region fields

		private List<MExtensionMap> _extensionMaps;

        public List<MExtensionMap> ExtensionMaps { get { return _extensionMaps; } }

		#endregion

	}


    [Serializable()]
    public class MExtensionMap
    {
        public MExtensionMap() { }

        private string _extension;
        private int _cnabType;

        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        public int CnabType
        {
            get { return _cnabType; }
            set { _cnabType = value; }
        }
    }  
}

