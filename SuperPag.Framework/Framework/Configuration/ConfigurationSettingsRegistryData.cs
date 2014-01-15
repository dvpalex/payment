//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Collections;
//using System.Collections.Specialized;

//namespace SuperPag.Framework.Configuration
//{
//    /// <summary>
//    /// Summary description for ConfigurationSettingsSqlData.
//    /// </summary>
//    internal class ConfigurationSettingsRegistryData : IConfigurationSettings
//    {
//        public void SaveSetting(string key, string _value)
//        {
//            Security.RegistryData reg = new Common.Security.RegistryData(true);
//            reg.WriteEntry(key, _value);
//        }

//        public NameValueCollection GetSettings()
//        {
//            Common.Security.RegistryData reg = new Common.Security.RegistryData(false);

//            string[] keys = reg.GetKeys();

//            NameValueCollection result = new NameValueCollection(5);
			
//            foreach(string k in keys)
//            {
//                result.Add(k, reg.GetEntry(k));
//            }

//            return result;
//        }
//    }
//}
