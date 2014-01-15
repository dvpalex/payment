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
//    internal class ConfigurationSettingsSqlData : IConfigurationSettings
//    {
//        public void SaveSetting(string key, string _value)
//        {
////			SqlCommand sqlCommand = new SqlCommand();
////			sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
////			sqlCommand.CommandText = "USP_UPDATE_ConfigurationSettings";
////
////			sqlCommand.Parameters.Add("@dsKey", key);
////			sqlCommand.Parameters.Add("@dsValue", _value);
////
////			try
////			{
////				sqlCommand.Connection = new SqlConnection(ConfigurationReader.GetConnectionFX());
////				sqlCommand.Connection.Open();
////				sqlCommand.ExecuteNonQuery();
////			} 
////			finally
////			{
////				sqlCommand.Connection.Close();
////			}
//        }

//        public NameValueCollection GetSettings()
//        {
//            SqlCommand sqlCommand = new SqlCommand();
//            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
//            sqlCommand.CommandText = "USP_LIST_ConfigurationSettings";

//            NameValueCollection result = new NameValueCollection(5);

//            try
//            {
//                sqlCommand.Connection = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionFramework"]);

//                sqlCommand.Connection.Open();

//                SqlDataReader reader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
//                while(reader.Read())
//                {
//                    result.Add(reader["dsKey"].ToString(), reader["dsValue"].ToString());
//                }
//            } 
//            finally
//            {
//                sqlCommand.Connection.Close();
//            }

//            string connectionFx =
//                System.Configuration.ConfigurationSettings.AppSettings["ConnectionFramework"];
//            result["ConnectionFX"] = connectionFx;

//            return result;
//        }
//    }
//}
