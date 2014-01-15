using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.OracleClient;
using Microsoft.ApplicationBlocks.Data;
using System.Threading;
using SuperPag.Framework.Data.Components;

namespace SuperPag.Framework.Data.Components.Data
{
    public interface IDataObject
    {
        //string GetEntityName();
        string[] GetFieldsFromIndex(string indexName, string tableName);
    }
}


namespace SuperPag.Framework.Data.Components.Data.Objects.SqlServer
{

    #region "pager"
    internal class Pager
    {
        public const int MAXPAGESIZE = 2000;

        /* true se tiver mais páginas */
        /* false se eof */
        public static bool GetPage(ref object[] objset, int size, int pagesize)
        {
            object[] _objset = objset;

            if (_objset == null || _objset.Length < 1 || _objset.Length < size - pagesize)
            {
                objset = null;

                return false;
            }

            int rowcount = _objset.Length;

            if (rowcount > pagesize)
            {
                object[] newobjset = new object[pagesize];

                for (int idx = size - pagesize, len = size, objidx = 0; idx < len; ++idx, ++objidx)
                    newobjset[objidx] = _objset[idx];

                objset = newobjset;

                return true; //mais registros
            }
            else
            {
                object[] newobjset = new object[rowcount];

                for (int idx = size - pagesize, len = rowcount, objidx = 0; idx < len; ++idx, ++objidx)
                    newobjset[objidx] = _objset[idx];

                objset = newobjset;

                return false; //EOF
            }
        }

        private static int InternalRelativePage(int rowcount, int pagesize, bool overflow)
        {
            int _rowcount = rowcount - (overflow ? 1 : 0);
            int resultado = _rowcount / pagesize;
            int resto = _rowcount % pagesize;

            if (resto > 0)
                resultado++;

            return resultado;
        }

        /* return true == overflow = existe mais dados no banco */
        /* return false == não existe mais dados no banco, vc esta na ultima pagina realmente */
        public static bool GetLastPage(ref object[] objset, out int relative_page, int pagesize)
        {
            object[] _objset = objset;

            if (_objset == null || _objset.Length < 1)
            {
                objset = null;
                relative_page = 0;

                return false;
            }

            int rowcount = _objset.Length;

            bool _overflow = false;

            if (rowcount > MAXPAGESIZE)
            {
                _overflow = true;
            }

            if (rowcount > pagesize)
            {
                relative_page = InternalRelativePage(rowcount, pagesize, _overflow);

                int new_pagesize = 0;

                if (_overflow)
                    new_pagesize = pagesize;
                else
                    new_pagesize = rowcount - ((relative_page - 1) * pagesize);

                object[] newobjset = new object[new_pagesize];

                for (int idx = (rowcount - new_pagesize - (_overflow ? 1 : 0)), objidx = 0; objidx < new_pagesize; ++idx, ++objidx)
                    newobjset[objidx] = _objset[idx];

                objset = newobjset;

                return _overflow;
            }
            else
            {
                objset = _objset;

                relative_page = 1;
            }

            return _overflow;
        }

        public static int CheckInternalPage(int variable, string variableName, /*string sqlStatement,*/ int size)
        {
            if (variable < 0)
            {
                throw new ArgumentOutOfRangeException(variableName, variable, "Cannot be lesser than the 1");
            }

            if (variable > MAXPAGESIZE)
            {
                throw new ArgumentOutOfRangeException(variableName, variable, String.Format("Cannot be greater than the maxsize = {0}", MAXPAGESIZE));
            }

            return size + 1;
        }
    }
    #endregion

    //TODO: Implementar a ICacheable
    public abstract class DataObject : IDataObject
    {
        //TODO: remover esse construtor e alterar a factory
        public DataObject()
        {
        }

        //		protected override bool PreProcessing(string senderObject, string methodName, params object[] parametersValues)
        //		{
        //			return true;
        //		}
        //
        //		protected override object PosProcessing(string senderObject, string methodName, object returnValue, params object[] parametersValues)
        //		{	
        //			return returnValue;
        //		}

        //Método que processo os erros, retorna true / false indicando se deve subir a exception
        //		protected override bool ProcessingError(ref Exception e)
        //		{
        //			return true;
        //		}

        //perfmon
        //tracing
        private string currentConnectionString;

        public string GetEntityName()
        {
            //TODO: depois que tudo for alterado para o novo BUIL da dataFactory, pegar pela BaseType
            string[] arrayOfNames = this.ToString().Split('.');

            //Retorna o último nome (nome da classe) removendo o SqlServer
#if SQL
            string entityName = arrayOfNames[arrayOfNames.Length - 1].Replace("SqlServer", "");
#elif ORACLE
            string entityName = arrayOfNames[arrayOfNames.Length - 1].Replace("Oracle", "");
#else
            string entityName = arrayOfNames[arrayOfNames.Length - 1].Replace("SqlServer", "");
#endif
            //Verifica se é um DAL_ (classe emitida pelo framework)
            if (entityName.StartsWith("DAL_"))
            {
                entityName = entityName.Remove(0, 4);
            }

            return entityName;
        }

        public string[] GetFieldsFromIndex(string indexName, string tableName)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append("declare @indID smallint ");
            query.Append("declare @i smallint ");
            query.Append("declare @indexName varchar(100) ");
            query.Append("declare @keys varchar(100) ");
            query.Append("select @indexName = '', @keys = '', @i = 1 ");
            query.Append("select @indID = indid from sysindexes where name = '{0}' ");
            query.Append("while (@indexName is not null) begin ");
            query.Append("select @indexName = index_col('{1}', @indID, @i) ");
            query.Append("if (@indexName is not null) begin ");
            query.Append("if (@i > 1) set @keys = @keys + ',' ");
            query.Append("set @keys = @keys + @indexName ");
            query.Append("end ");
            query.Append("set @i = @i + 1 ");
            query.Append("end select @keys as indexKeys ");

            string commandText = string.Format(query.ToString(), indexName, tableName);

            //Cria e abre a conexão 
#if SQL
            SqlConnection sqlConn = new SqlConnection(this.currentConnectionString);
#elif ORACLE
            OracleConnection sqlConn = new OracleConnection(this.currentConnectionString);
#else
            SqlConnection sqlConn = new SqlConnection(this.currentConnectionString);
#endif

            try
            {
                sqlConn.Open();

                //Cria e executa o comando
#if SQL
                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConn);
				SqlDataReader dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
#elif ORACLE
                OracleCommand sqlCommand = new OracleCommand(commandText, sqlConn);
                OracleDataReader dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
#else
                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConn);
                SqlDataReader dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
#endif

                //Verifica se obteve algum resultado
                if (dataReader.Read())
                {
                    string[] result = dataReader.GetString(0).Split(',');
                    dataReader.Close();
                    return result;
                }
                return null;
            }
            finally
            {
                sqlConn.Close();

            }
        }

        const int ROWCOUNT = 2000;

#if SQL
        protected int Execute(string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters) 
#elif ORACLE
        protected int Execute(string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.OracleClient.OracleParameter[] parameters)
#else
        protected int Execute(string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters)
#endif
        {
            this.currentConnectionString = connectionString;

            try
            {
                return SqlHelper.ExecuteNonQuery(connectionString, commandType, sqlStatement, parameters);
            }
#if SQL
            catch (SqlException ex) 
#elif ORACLE
            catch (OracleException ex)
#else
            catch (SqlException ex)
#endif
            {
                throw (HandleSqlExceptions(ex));
            }
        }

#if SQL
        private Exception HandleSqlExceptions(SqlException ex) 
#elif ORACLE
        private Exception HandleSqlExceptions(OracleException ex)
#else
        private Exception HandleSqlExceptions(SqlException ex)
#endif
        {
            //Verifica se é erro de conflito na deleção de registros pela constraint
            //Como o sql utiliza o mesmo código de erro para inser e delete constraint exception,
            //devemos verificar a string
#if SQL
			if (ex.Number == 547 && ex.Message.Trim().ToUpper().StartsWith("DELETE")) 
#elif ORACLE
            if (ex.Code == 547 && ex.Message.Trim().ToUpper().StartsWith("DELETE"))
#else
            if (ex.Number == 547 && ex.Message.Trim().ToUpper().StartsWith("DELETE"))
#endif
            {
                return new DeleteConstraintException(this, ex);
            }
#if SQL
            else if (ex.Number == 2627 || ex.Number == 2601) 
#elif ORACLE
            else if (ex.Code == 2627 || ex.Code == 2601)
#else
            else if (ex.Number == 2627 || ex.Number == 2601)
#endif
            { //Verifica se é erro de chave duplicada
                return new DuplicatedKeyException(this, ex);
            }
            else
            {
                return ex;
            }
        }

        //////		protected int Execute( out string newId, 
        //////			string connectionString, System.Data.CommandType commandType, string sqlStatement , 
        //////			params System.Data.SqlClient.SqlParameter[] parameters) 
        //////		{
        //////			SqlParameter newIdParameter = new SqlParameter( "@NEWID", SqlDbType.UniqueIdentifier );
        //////			newIdParameter.Direction = ParameterDirection.Output;
        //////			this.currentConnectionString = connectionString;
        //////			
        //////			ArrayList list;
        //////			
        //////			if( parameters != null )
        //////				list = new ArrayList( parameters );
        //////			else
        //////				list = new ArrayList( 1 );
        //////
        //////			list.Add( newIdParameter );
        //////
        //////			int rowsAffected = 0;
        //////
        //////			try 
        //////			{
        //////				rowsAffected = SqlHelper.ExecuteNonQuery(connectionString, commandType, sqlStatement, (SqlParameter[]) list.ToArray( typeof( SqlParameter ) ) );
        //////			} 
        //////			catch (SqlException ex) 
        //////			{
        //////				throw(HandleSqlExceptions(ex));
        //////			}
        //////			
        //////			newId = newIdParameter.Value is System.DBNull ? null : (string)newIdParameter.Value;
        //////
        //////			return rowsAffected;
        //////		}

#if SQL
        protected int Execute(out int newId, string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters) 
#elif ORACLE
        protected int Execute(out int newId, string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.OracleClient.OracleParameter[] parameters)
#else
        protected int Execute(out int newId, string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters)
#endif
        {
#if SQL
            SqlParameter newIdParameter = new SqlParameter("@NEWID", SqlDbType.Int);
#elif ORACLE
            OracleParameter newIdParameter = new OracleParameter("@NEWID", OracleType.Int16);
#else
            SqlParameter newIdParameter = new SqlParameter("@NEWID", SqlDbType.Int);
#endif
            newIdParameter.Direction = ParameterDirection.Output;
            this.currentConnectionString = connectionString;

            ArrayList list;

            if (parameters != null)
                list = new ArrayList(parameters);
            else
                list = new ArrayList(1);

            list.Add(newIdParameter);

            int rowsAffected = 0;

            try
            {
#if SQL
                rowsAffected = SqlHelper.ExecuteNonQuery(connectionString, commandType, sqlStatement, (SqlParameter[])list.ToArray(typeof(SqlParameter)));
#elif ORACLE
                rowsAffected = SqlHelper.ExecuteNonQuery(connectionString, commandType, sqlStatement, (OracleParameter[])list.ToArray(typeof(OracleParameter)));
#else
                rowsAffected = SqlHelper.ExecuteNonQuery(connectionString, commandType, sqlStatement, (SqlParameter[])list.ToArray(typeof(SqlParameter)));
#endif
            }
#if SQL
            catch (SqlException ex) 
#elif ORACLE
            catch (OracleException ex)
#else
            catch (SqlException ex)
#endif
            {
                throw (HandleSqlExceptions(ex));
            }

            newId = newIdParameter.Value is System.DBNull ? 0 : (int)newIdParameter.Value;

            return rowsAffected;
        }

#if SQL
        protected object ExecuteScalar(string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters) 
#elif ORACLE
        protected object ExecuteScalar(string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.OracleClient.OracleParameter[] parameters)
#else
        protected object ExecuteScalar(string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters)
#endif
        {
            this.currentConnectionString = connectionString;
            //sqlStatement = sqlStatement.Replace("Order.", "[Order].");
            try
            {
                return SqlHelper.ExecuteScalar(connectionString, commandType, sqlStatement, parameters);
            }
#if SQL
            catch (SqlException ex) 
#elif ORACLE
            catch (OracleException ex)
#else
            catch (SqlException ex)
#endif
            {
                throw (HandleSqlExceptions(ex));
            }
        }


#if SQL
        protected object[] ExecuteReader(string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters)
#elif ORACLE
        protected object[] ExecuteReader(string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.OracleClient.OracleParameter[] parameters)
#else
        protected object[] ExecuteReader(string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters)
#endif
        {
            this.currentConnectionString = connectionString;
            //int ini = sqlStatement.IndexOf("FROM", 0) + 1; ;
            //while (ini != -1)
            //{
            //    ini = sqlStatement.IndexOf("]", ini) + 1;
            //    if (ini != 0)
            //    {
            //        sqlStatement = sqlStatement.Insert(ini, " with(nolock)");
            //    }
            //    else
            //    {
            //        ini -= 1;
            //    }
            //}
           
            return ProcessReader(connectionString, commandType, sqlStatement, ROWCOUNT, parameters);
        }

#if SQL
        protected object[] ExecutePageReader(int pagesize, int page, out bool continue_paging, string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters)
#elif ORACLE
        protected object[] ExecutePageReader(int pagesize, int page, out bool continue_paging, string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.OracleClient.OracleParameter[] parameters)
#else
        protected object[] ExecutePageReader(int pagesize, int page, out bool continue_paging, string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters)
#endif
        {
            int size = pagesize * page;
            this.currentConnectionString = connectionString;

            int adjusted_size = Pager.CheckInternalPage(size, "pagesize * size", size);

            object[] objset = ProcessReader(connectionString, commandType, sqlStatement, adjusted_size, parameters);

            continue_paging = Pager.GetPage(ref objset, size, pagesize);

            return objset;
        }

#if SQL
        protected object[] ExecuteLastPageReader(int pagesize, out int relative_page, out bool page_overflow, string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters)
#elif ORACLE
        protected object[] ExecuteLastPageReader(int pagesize, out int relative_page, out bool page_overflow, string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.OracleClient.OracleParameter[] parameters)
#else
        protected object[] ExecuteLastPageReader(int pagesize, out int relative_page, out bool page_overflow, string connectionString, System.Data.CommandType commandType, string sqlStatement, params System.Data.SqlClient.SqlParameter[] parameters)
#endif
        {
            int adjusted_size = Pager.CheckInternalPage(pagesize, "pagesize", Pager.MAXPAGESIZE);
            this.currentConnectionString = connectionString;

            object[] objset = ProcessReader(connectionString, commandType, sqlStatement, adjusted_size, parameters);

            page_overflow = Pager.GetLastPage(ref objset, out relative_page, pagesize);

            return objset;
        }

        //		private string[] MetadataReader( System.Data.SqlClient.SqlDataReader reader )
        //		{
        //			DataTable table = reader.GetSchemaTable();
        //			DataRow[] rows = table.Select( null, null, DataViewRowState.CurrentRows );
        //
        //			string[] lookup = new string[ rows.Length ];
        //
        //			int index = 0;
        //			foreach( DataRow row in rows )
        //			{
        //				if( row[ 0 ] != null )
        //				{
        //					lookup[ index ] = row[ 0 ].ToString();
        //				}
        //				else
        //				{
        //					lookup[ index ] = index.ToString();
        //				}
        //
        //				index++;
        //			}
        //
        //			return lookup;
        //		}

#if SQL
        private object[] ProcessReader(string connectionString, System.Data.CommandType commandType, string sqlStatement, int rowcount, params System.Data.SqlClient.SqlParameter[] parameters)
#elif ORACLE
        private object[] ProcessReader(string connectionString, System.Data.CommandType commandType, string sqlStatement, int rowcount, params System.Data.OracleClient.OracleParameter[] parameters)
#else
        private object[] ProcessReader(string connectionString, System.Data.CommandType commandType, string sqlStatement, int rowcount, params System.Data.SqlClient.SqlParameter[] parameters)
#endif
        {
#if SQL
            System.Data.SqlClient.SqlParameter[] parameters_list = null;

			System.Data.SqlClient.SqlParameter psize = new System.Data.SqlClient.SqlParameter( "@P_ROWCOUNT", System.Data.SqlDbType.Int );
#elif ORACLE
            System.Data.OracleClient.OracleParameter[] parameters_list = null;

            System.Data.OracleClient.OracleParameter psize = new System.Data.OracleClient.OracleParameter("@P_ROWCOUNT", System.Data.OracleClient.OracleType.Int16);
#else
            System.Data.SqlClient.SqlParameter[] parameters_list = null;

            System.Data.SqlClient.SqlParameter psize = new System.Data.SqlClient.SqlParameter("@P_ROWCOUNT", System.Data.SqlDbType.Int);
#endif
            psize.Value = rowcount;

            ArrayList plist;

            if (parameters != null)
                plist = new ArrayList(parameters);
            else
                plist = new ArrayList(1);

            // plist.Add( psize );

#if SQL
            parameters_list = (System.Data.SqlClient.SqlParameter[])plist.ToArray(typeof(System.Data.SqlClient.SqlParameter));
			
			System.Data.SqlClient.SqlDataReader reader = SqlHelper.ExecuteReader( connectionString, commandType, sqlStatement, parameters_list );
#elif ORACLE
            parameters_list = (System.Data.OracleClient.OracleParameter[])plist.ToArray(typeof(System.Data.OracleClient.OracleParameter));

            System.Data.OracleClient.OracleDataReader reader = SqlHelper.ExecuteReader(connectionString, commandType, sqlStatement, parameters_list);
#else
            parameters_list = (System.Data.SqlClient.SqlParameter[])plist.ToArray(typeof(System.Data.SqlClient.SqlParameter));

            System.Data.SqlClient.SqlDataReader reader = SqlHelper.ExecuteReader(connectionString, commandType, sqlStatement, parameters_list);
#endif

            ArrayList list = new ArrayList();

            //string[] lookup_table = MetadataReader( reader );

            try
            {
                while (reader.Read())
                {
                    int len = reader.FieldCount;

                    Hashtable hash = new Hashtable(len);

                    for (int idx = 0; idx < len; idx++)
                    {
                        hash.Add(reader.GetName(idx), reader.GetValue(idx));
                        //hash.Add( lookup_table[idx], reader.GetValue( idx ) );
                    }

                    list.Add(hash);
                }
            }
            finally
            {
                reader.Close();
            }

            return list.ToArray();
        }
    }
}
