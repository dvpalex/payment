using System;
using System.Reflection;

namespace SuperPag.Framework.Business
{
	
	public interface IPreProcessing
	{
		//Método que processa os erros, retorna true / false indicando se deve subir a exception
		bool PreProcessing(MethodInfo method, params object[] parametersValues);
	
		//Método de pós processamento, retorna true / false indicando se o método deve ou não ser processado
		object PosProcessing(MethodInfo method, object returnValue, params object[] parametersValues);
		
		//Método que processa os erros, retorna true / false indicando se deve subir a exception
		bool ProcessingError( Exception e);		
	}
}
