using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;

namespace SuperPag.Framework.Data.Mapping
{
#if SQL
    internal class SqlMessageAdapter
#elif ORACLE
    internal class OracleMessageAdapter
#else
    internal class SqlMessageAdapter
#endif
	{
#if SQL
        public static System.Array Fill(SqlCommand command, Type dataMessage)
#elif ORACLE
        public static System.Array Fill(OracleCommand command, Type dataMessage)
#else
        public static System.Array Fill(SqlCommand command, Type dataMessage)
#endif
		{
#if SQL
            SqlDataReader reader = null;
#elif ORACLE
            OracleDataReader reader = null;
#else
            SqlDataReader reader = null;
#endif

            try
			{
				command.Connection.Open();
				
				reader = command.ExecuteReader( CommandBehavior.CloseConnection );
			
				DataMapper mapper = new DataMapper( dataMessage , reader );

				return mapper.Do();
			} 
			finally 
			{
				if ( reader != null ) reader.Close();
			}

		}
	}
	
}
