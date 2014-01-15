using System;
using System.Reflection;

namespace SuperPag.Framework.Business
{
	
	public interface IPreProcessing
	{
		//M�todo que processa os erros, retorna true / false indicando se deve subir a exception
		bool PreProcessing(MethodInfo method, params object[] parametersValues);
	
		//M�todo de p�s processamento, retorna true / false indicando se o m�todo deve ou n�o ser processado
		object PosProcessing(MethodInfo method, object returnValue, params object[] parametersValues);
		
		//M�todo que processa os erros, retorna true / false indicando se deve subir a exception
		bool ProcessingError( Exception e);		
	}
}
