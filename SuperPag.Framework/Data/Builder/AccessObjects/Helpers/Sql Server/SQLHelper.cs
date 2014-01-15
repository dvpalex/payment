//===============================================================================
// Microsoft Data Access Application Block for .NET
// http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp
//
// SQLHelper.cs
//
// This file contains the implementations of the SqlHelper and SqlHelperParameterCache
// classes.
//
// For more information see the Data Access Application Block Implementation Overview. 
// 
//===============================================================================
// Copyright (C) 2000-2001 Microsoft Corporation
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//==============================================================================

using System;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Collections;

namespace Microsoft.ApplicationBlocks.Data
{
	/// <summary>
	/// The SqlHelper class is intended to encapsulate high performance, scalable best practices for 
	/// common uses of SqlClient.
	/// </summary>
	public sealed class SqlHelper
	{
		#region private utility methods & constructors

		//Since this class provides only static methods, make the default constructor private to prevent 
		//instances from being created with "new SqlHelper()".
		private SqlHelper() {}

		/// <summary>
		/// This method is used to attach array of SqlParameters to a SqlCommand.
		/// 
		/// This method will assign a value of DbNull to any parameter with a direction of
		/// InputOutput and a value of null.  
		/// 
		/// This behavior will prevent default values from being used, but
		/// this will be the less common case than an intended pure output parameter (derived as InputOutput)
		/// where the user provided no input value.
		/// </summary>
		/// <param name="command">The command to which the parameters will be added</param>
		/// <param name="commandParameters">an array of SqlParameters tho be added to command</param>
#if SQL
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
#elif ORACLE
        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
#else
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
#endif
		{
#if SQL
            foreach (SqlParameter p in commandParameters)
#elif ORACLE
            foreach (OracleParameter p in commandParameters)
#else
            foreach (SqlParameter p in commandParameters)
#endif
            {
				//check for derived output value with no value assigned
				if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
				{
					p.Value = DBNull.Value;
				}
				
				command.Parameters.Add(p);
			}
		}

		/// <summary>
		/// This method assigns an array of values to an array of SqlParameters.
		/// </summary>
		/// <param name="commandParameters">array of SqlParameters to be assigned values</param>
		/// <param name="parameterValues">array of objects holding the values to be assigned</param>
#if SQL
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
#elif ORACLE
        private static void AssignParameterValues(OracleParameter[] commandParameters, object[] parameterValues)
#else
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
#endif
		{
			if ((commandParameters == null) || (parameterValues == null)) 
			{
				//do nothing if we get no data
				return;
			}

			// we must have the same number of values as we pave parameters to put them in
			if (commandParameters.Length != parameterValues.Length)
			{
				throw new ArgumentException("Parameter count does not match Parameter Value count.");
			}

			//iterate through the SqlParameters, assigning the values from the corresponding position in the 
			//value array
			for (int i = 0, j = commandParameters.Length; i < j; i++)
			{
				commandParameters[i].Value = parameterValues[i];
			}
		}

		/// <summary>
		/// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
		/// to the provided command.
		/// </summary>
		/// <param name="command">the SqlCommand to be prepared</param>
		/// <param name="connection">a valid SqlConnection, on which to execute this command</param>
		/// <param name="transaction">a valid SqlTransaction, or 'null'</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
#if SQL
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
#elif ORACLE
        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters)
#else
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
#endif
		{
			//associate the connection with the command
			command.Connection = connection;

			//set the command text (stored procedure name or SQL statement)
			command.CommandText = commandText;

			//if we were provided a transaction, assign it.
			if (transaction != null)
			{
				command.Transaction = transaction;
			}

			//set the command type
			command.CommandType = commandType;

			//attach the command parameters if they are provided
			if (commandParameters != null)
			{
				AttachParameters(command, commandParameters);
			}

			return;
		}


		#endregion private utility methods & constructors

		#region ExecuteNonQuery

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
#if SQL
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
#elif ORACLE
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
#else
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
#endif
		{
			//create & open a SqlConnection, and dispose of it after we are done.
#if SQL
            SqlConnection cn = new SqlConnection(connectionString);
#elif ORACLE
            OracleConnection cn = new OracleConnection(connectionString);
#else
            SqlConnection cn = new SqlConnection(connectionString);
#endif

            //create a command and prepare it for execution
#if SQL
            SqlCommand cmd = new SqlCommand();
#elif ORACLE
            OracleCommand cmd = new OracleCommand();
#else
            SqlCommand cmd = new SqlCommand();
#endif


            try
			{
				cn.Open();

#if SQL
                PrepareCommand(cmd, cn, (SqlTransaction)null, commandType, commandText, commandParameters);
#elif ORACLE
                PrepareCommand(cmd, cn, (OracleTransaction)null, commandType, commandText, commandParameters);
#else
                PrepareCommand(cmd, cn, (SqlTransaction)null, commandType, commandText, commandParameters);
#endif

                //finally, execute the command.
				return cmd.ExecuteNonQuery();
			} 
			finally
			{
				// detach the SqlParameters from the command object, so they can be used again.
				cmd.Parameters.Clear();

				cn.Close();
			}
			
		}

		#endregion ExecuteNonQuery
	
		#region ExecuteReader

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a SqlDataReader containing the resultset generated by the command</returns>
#if SQL
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
#elif ORACLE
        public static OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
#else
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
#endif
		{
			//create & open a SqlConnection
#if SQL
            SqlConnection cn = new SqlConnection(connectionString);
#elif ORACLE
            OracleConnection cn = new OracleConnection(connectionString);
#else
            SqlConnection cn = new SqlConnection(connectionString);
#endif

            try
			{
				cn.Open();

				//create a command and prepare it for execution
#if SQL
                SqlCommand cmd = new SqlCommand();
#elif ORACLE
                OracleCommand cmd = new OracleCommand();
#else
                SqlCommand cmd = new SqlCommand();
#endif
                PrepareCommand(cmd, cn, null, commandType, commandText, commandParameters);
			
				//create a reader
#if SQL
                SqlDataReader dr;
#elif ORACLE
                OracleDataReader dr;
#else
                SqlDataReader dr;
#endif

                // call ExecuteReader with the appropriate CommandBehavior
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			
				// detach the SqlParameters from the command object, so they can be used again.
				cmd.Parameters.Clear();
			
				return dr;
			}
			catch
			{
				//if we fail to return the SqlDatReader, we need to close the connection ourselves
				cn.Close();
				throw;
			}
		}

		#endregion ExecuteReader

		#region ExecuteScalar
	
		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
#if SQL
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
#elif ORACLE
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
#else
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
#endif
		{
			//create & open a SqlConnection, and dispose of it after we are done.
#if SQL
            SqlConnection cn = new SqlConnection(connectionString);
#elif ORACLE
            OracleConnection cn = new OracleConnection(connectionString);
#else
            SqlConnection cn = new SqlConnection(connectionString);
#endif

            object retval = null;

			//execute the command & return the results
			try
			{
				cn.Open();

				//create a command and prepare it for execution
#if SQL
                SqlCommand cmd = new SqlCommand();
				PrepareCommand(cmd, cn, (SqlTransaction)null, commandType, commandText, commandParameters);
#elif ORACLE
                OracleCommand cmd = new OracleCommand();
                PrepareCommand(cmd, cn, (OracleTransaction)null, commandType, commandText, commandParameters);
#else
                SqlCommand cmd = new SqlCommand();
				PrepareCommand(cmd, cn, (SqlTransaction)null, commandType, commandText, commandParameters);
#endif

                retval = cmd.ExecuteScalar();

				// detach the SqlParameters from the command object, so they can be used again.
				cmd.Parameters.Clear();
			} 
			finally
			{
				cn.Close();
			}
			
			return retval;
		}


		#endregion ExecuteScalar	

	}

	/// <summary>
	/// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
	/// ability to discover parameters for stored procedures at run-time.
	/// </summary>
	public sealed class SqlHelperParameterCache
	{
		#region private methods, variables, and constructors

		//Since this class provides only static methods, make the default constructor private to prevent 
		//instances from being created with "new SqlHelperParameterCache()".
		private SqlHelperParameterCache() {}

		private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// resolve at run time the appropriate set of SqlParameters for a stored procedure
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
		/// <returns></returns>
#if SQL
        private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
#elif ORACLE
        private static OracleParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
#else
        private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
#endif
		{
#if SQL
            SqlConnection cn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand(spName,cn);
#elif ORACLE
            OracleConnection cn = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand(spName, cn);
#else
            SqlConnection cn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand(spName,cn);
#endif

            try
			{
				cn.Open();
				cmd.CommandType = CommandType.StoredProcedure;

#if SQL
                SqlCommandBuilder.DeriveParameters(cmd);
#elif ORACLE
                OracleCommandBuilder.DeriveParameters(cmd);
#else
                SqlCommandBuilder.DeriveParameters(cmd);
#endif

                if (!includeReturnValueParameter) 
				{
					cmd.Parameters.RemoveAt(0);
				}

#if SQL
                SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count]; ;
#elif ORACLE
                OracleParameter[] discoveredParameters = new OracleParameter[cmd.Parameters.Count]; ;
#else
                SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count]; ;
#endif

                cmd.Parameters.CopyTo(discoveredParameters, 0);

				return discoveredParameters;
			} 
			finally
			{
				cn.Close();	
			}
		}

		//deep copy of cached SqlParameter array
#if SQL
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
#elif ORACLE
        private static OracleParameter[] CloneParameters(OracleParameter[] originalParameters)
#else
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
#endif
		{
#if SQL
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];
#elif ORACLE
            OracleParameter[] clonedParameters = new OracleParameter[originalParameters.Length];
#else
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];
#endif

            for (int i = 0, j = originalParameters.Length; i < j; i++)
			{
#if SQL
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
#elif ORACLE
                clonedParameters[i] = (OracleParameter)((ICloneable)originalParameters[i]).Clone();
#else
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
#endif
            }

			return clonedParameters;
		}

		#endregion private methods, variables, and constructors

		#region caching functions

		/// <summary>
		/// add parameter array to the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters to be cached</param>
#if SQL
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
#elif ORACLE
        public static void CacheParameterSet(string connectionString, string commandText, params OracleParameter[] commandParameters)
#else
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
#endif
		{
			string hashKey = connectionString + ":" + commandText;

			paramCache[hashKey] = commandParameters;
		}

		/// <summary>
		/// retrieve a parameter array from the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an array of SqlParamters</returns>
#if SQL
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
#elif ORACLE
        public static OracleParameter[] GetCachedParameterSet(string connectionString, string commandText)
#else
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
#endif
		{
			string hashKey = connectionString + ":" + commandText;

#if SQL
            SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];
#elif ORACLE
            OracleParameter[] cachedParameters = (OracleParameter[])paramCache[hashKey];
#else
            SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];
#endif

            if (cachedParameters == null)
			{			
				return null;
			}
			else
			{
				//Cloca os parameters
#if SQL
                SqlParameter[] clonedParameters = CloneParameters(cachedParameters);
#elif ORACLE
                OracleParameter[] clonedParameters = CloneParameters(cachedParameters);
#else
                SqlParameter[] clonedParameters = CloneParameters(cachedParameters);
#endif
                //Define o valor default para cada parameter
#if SQL
                foreach (SqlParameter parameter in clonedParameters)
#elif ORACLE
                foreach (OracleParameter parameter in clonedParameters)
#else
                foreach (SqlParameter parameter in clonedParameters)
#endif
                {
					parameter.Value = null;
				}

				return clonedParameters;
			}
		}

		#endregion caching functions

		#region Parameter Discovery Functions

		/// <summary>
		/// Retrieves the set of SqlParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an array of SqlParameters</returns>
#if SQL
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
#elif ORACLE
        public static OracleParameter[] GetSpParameterSet(string connectionString, string spName)
#else
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
#endif
		{
			return GetSpParameterSet(connectionString, spName, false);
		}

		/// <summary>
		/// Retrieves the set of SqlParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
		/// <returns>an array of SqlParameters</returns>
#if SQL
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
#elif ORACLE
        public static OracleParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
#else
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
#endif
		{
			string hashKey = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter":"");

#if SQL
            SqlParameter[] cachedParameters;
			
			cachedParameters = (SqlParameter[])paramCache[hashKey];
#elif ORACLE
            OracleParameter[] cachedParameters;

            cachedParameters = (OracleParameter[])paramCache[hashKey];
#else
            SqlParameter[] cachedParameters;
			
			cachedParameters = (SqlParameter[])paramCache[hashKey];
#endif

            if (cachedParameters == null)
			{
#if SQL
                cachedParameters = (SqlParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
#elif ORACLE
                cachedParameters = (OracleParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
#else
                cachedParameters = (SqlParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
#endif
            }
			
			return CloneParameters(cachedParameters);
		}

		#endregion Parameter Discovery Functions

	}
}