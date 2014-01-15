using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SuperPag.Framework.Business
{
	public abstract class BusinessObject : IPreProcessing
	{
		//TODO: hardcoded log:
		Diagnostics.Log _log = new Diagnostics.Log();

		//Método que processa os erros, retorna true / false indicando se deve subir a exception
		public virtual bool PreProcessing(MethodInfo method, params object[] parametersValues)
		{
			_log.PreProcessing ( method, parametersValues );
			return true;
		}

		//Método de pós processamento, retorna true / false indicando se o método deve ou não ser processado
		public virtual object PosProcessing(MethodInfo method, object returnValue, params object[] parametersValues)
		{
			_log.PosProcessing ( method, returnValue, parametersValues );
			return returnValue;
		}

		//Método que processa os erros, retorna true / false indicando se deve subir a exception
		public virtual bool ProcessingError( Exception e)
		{
			_log.ProcessingError ( e );
			return true;
		}

		public static BusinessObject Create( Type type )
		{
			if ( ! type.IsSubclassOf ( typeof ( BusinessObject ) ) )
				throw new ArgumentException ( "O tipo informado deve herdar a classe base 'BaseBusinessObject'" , "type" );

			//cria o object 
			return (BusinessObject)BusinessObjectFactory.Build ( type );
		}

		protected BusinessObject () { }		
	}
}
