//SC using System;
//using System.Data.SqlClient;
//using System.Collections;
//
//namespace SuperPag.Framework.Web.WebController
//{
//	/// <summary>
//	/// Summary description for SecurityData.
//	/// </summary>
//	internal class SecuritySqlData
//	{
//		public static Hashtable SoleInstance()
//		{
////			SqlCommand sqlCommand = new SqlCommand();
////			sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
////			sqlCommand.CommandText = "USP_LISTA_FUNCIONALIDADE";
////
////			Hashtable result;
////
////			try
////			{
////				sqlCommand.Connection = new SqlConnection(Configuration.ConfigurationReader.GetConnectionFX());
////
////				sqlCommand.Connection.Open();
////
////				SqlDataReader reader = sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
////				result = Read(reader);		
////			} 
////			finally
////			{
////				sqlCommand.Connection.Close();
////			}
//
//			return new Hashtable();
//		}
//
//		private static Hashtable Read(SqlDataReader reader)
//		{
//			Hashtable modules = new Hashtable(4);
//
//			string currentModule;
//			string lastModule = string.Empty;
//			while(reader.Read())
//			{
//				//obtem a hash do modulo
//				currentModule = reader.GetString(0);
//				if(lastModule != currentModule)
//				{
//					modules.Add(currentModule, new Hashtable(200));
//					lastModule = currentModule;
//				}
//
//				Hashtable commands;
//				commands = (Hashtable)modules[currentModule];
//
//				//verifica se a ação já existe
//				string action = reader.GetString(1);
//				action = action.ToLower();
//				if(!(commands[action] is System.Array))
//				{
//					commands[action] = new string[0];
//				}
//
//				//Coloca as funcionalidades
//				ArrayList functionalities = new ArrayList((string[])commands[action]);
//				functionalities.Add(reader.GetString(2));
//					
//				string[] funcs = (string[])functionalities.ToArray(typeof(System.String));
//				Array.Sort(funcs);
//				commands[action] = funcs;				
//			}
//
//			reader.Close();
//
//			return modules;
//		}
//
//	}
//}
