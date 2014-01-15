using System;
using System.Data.SqlClient;
using System.Data.OracleClient;
using Microsoft.ApplicationBlocks.Data;

namespace SuperPag.Framework.Data.Components.Data.Objects.SqlServer.Helpers {	
	public sealed class SqlCommandHelper {
#if SQL
        public static System.Data.SqlClient.SqlParameter SetParameter(System.Data.SqlClient.SqlParameter sqlParam, object oValue)
#elif ORACLE
        public static System.Data.OracleClient.OracleParameter SetParameter(System.Data.OracleClient.OracleParameter sqlParam, object oValue)
#else
        public static System.Data.SqlClient.SqlParameter SetParameter(System.Data.SqlClient.SqlParameter sqlParam, object oValue)
#endif
        {
			sqlParam.Value = oValue;

			return sqlParam;
		}		
	}

	public sealed class SqlCache {
#if SQL
        public static void SetCachedParameters(string id, string connectionString, SqlParameter[] sqlParams)
#elif ORACLE
        public static void SetCachedParameters(string id, string connectionString, OracleParameter[] sqlParams)
#else
        public static void SetCachedParameters(string id, string connectionString, SqlParameter[] sqlParams)
#endif
        {
			SqlHelperParameterCache.CacheParameterSet( connectionString, id, sqlParams );
		}

#if SQL
        public static SqlParameter[] GetCachedParameters(string id, string connectionString)
#elif ORACLE
        public static OracleParameter[] GetCachedParameters(string id, string connectionString)
#else
        public static SqlParameter[] GetCachedParameters(string id, string connectionString)
#endif
        {
			return SqlHelperParameterCache.GetCachedParameterSet( connectionString, id );			
		}
	}
}
