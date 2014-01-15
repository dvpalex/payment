using System;
using SuperPag.Framework.Data.Components;

namespace SuperPag.Framework.Data.Components.Data
{
	/// <summary>
	/// Exception disparada pelo framework data para indicar 
	/// que ocorreu erro na exclusão de algum registro - erro de foreingKey
	/// </summary>
	public class DeleteConstraintException : SuperPagFrameworkException {
		public readonly string fkEntityName;
		public readonly string pkEntityName;
		public readonly string fieldName;

#if SQL
        public DeleteConstraintException(IDataObject dataObject, System.Data.SqlClient.SqlException ex)
#elif ORACLE
        public DeleteConstraintException(IDataObject dataObject, System.Data.OracleClient.OracleException ex)
#else
        public DeleteConstraintException(IDataObject dataObject, System.Data.SqlClient.SqlException ex)
#endif
            : base(null, ex)
        {
			string[] arrayOfText = ex.Message.Split('\'');
			this.fkEntityName = arrayOfText[5];
			this.fieldName = arrayOfText[7];
			string fkName = arrayOfText[1];
			if (fkName.IndexOf('_') == -1) {
				throw new ApplicationException("Mensagem de erro SQL não está no formato esperado pela DeleteConstraintException");
			} else {
				this.pkEntityName = fkName.Split('_')[2];
			}
		}
	}

	/// <summary>
	/// Exception disparada pelo framework data para indicar
	/// que ocorreu erro na inclusão / atualização de algum registro - unique violation
	/// </summary>
	public class DuplicatedKeyException : SuperPagFrameworkException {
		public readonly string EntityName;
		public readonly string uniqueKey;
		public readonly string[] keys;

#if SQL
        public DuplicatedKeyException(IDataObject dataObject, System.Data.SqlClient.SqlException ex)
#elif ORACLE
        public DuplicatedKeyException(IDataObject dataObject, System.Data.OracleClient.OracleException ex)
#else
        public DuplicatedKeyException(IDataObject dataObject, System.Data.SqlClient.SqlException ex)
#endif
            : base(null, ex) 
		{
			string[] arrayOfText = ex.Message.Split('\'');

#if SQL
			switch (ex.Number) 
#elif ORACLE
            switch (ex.Code)
#else
			switch (ex.Number) 
#endif
			{
				case 2627:
					this.uniqueKey = arrayOfText[1];
					this.EntityName = arrayOfText[3];
					break;
				case 2601: 
					this.uniqueKey = arrayOfText[3];
					this.EntityName = arrayOfText[1];
					break;
				default:
					throw new ApplicationException("Cadastrar código de erro da SqlException");
			}

			keys = dataObject.GetFieldsFromIndex(this.uniqueKey, this.EntityName);
		}
	}
}
