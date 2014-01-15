using System;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace SuperPag.Framework.Business.Diagnostics
{
	public class Log : IPreProcessing
	{
		private MethodInfo _methodInfo;
		private object[] _params;
		
		public void WriteExceptionEntry ( Exception ex )
		{
			Trace.AutoFlush = true;

			try
			{
				lock ( this )
				{
					string date = FormatDate();
					string threadId = FormatThreadId();

					Trace.WriteLine ( "##################" );
					Trace.WriteLine ( FormatEntry ( date, threadId ) );
					Trace.WriteLine ( string.Format ( "{0} : {1} : {2} : {3}", date, threadId, ex.GetType().Name , ex.Message  ) );
					Trace.WriteLine ( string.Format ( "{0} : {1} : {2} ", date, threadId, ex.StackTrace ) );					
				}
			} 
			catch ( Exception logEx )
			{
				EventLog log = new EventLog("SuperPag");
				log.Source = "SuperPag";
				log.WriteEntry ( string.Format ( "{0}{1}", logEx.Message , logEx.StackTrace ) );
				log.WriteEntry ( string.Format ( "{0}{1}", ex.Message , ex.StackTrace ) );								
				Trace.WriteLine ( "logerror check event viewer" );				
			}
		}

		private string FormatEntry ( string date, string threadId )
		{
			// "01/01/2005 12:12:23 : nome-método : [mOrder=<xml>] ";
			string parameters = FormatParameters();
			string methodName = FormatMethodName();

			string message = string.Format ( "{0} : {1} : {2} : {3} ", date, threadId, methodName, parameters );
			return message;
			
		}

		private string FormatThreadId ()
		{
			return AppDomain.GetCurrentThreadId().ToString();
		}


		private string FormatMethodName ()
		{
			return string.Format ( "{0}.{1}", this._methodInfo.DeclaringType.Name , this._methodInfo.Name ) ;
		}

		private string FormatDate ()
		{
			return DateTime.Now.ToString ( "dd/MM/yy HH:mm:ss" );
		}
		private string FormatParameters ()
		{
			ParameterInfo[] arrP = this._methodInfo.GetParameters();

			StringBuilder sb = new StringBuilder();
			sb.Append ( "[" );
			int i = 0;
			foreach ( ParameterInfo p in arrP )
			{
				if ( p.ParameterType.IsSubclassOf ( typeof ( Message ) ) )
				{
					if ( _params [ i ] != null )
						sb.AppendFormat( " (\"{0}={1}\") ", p.Name , ((Message)_params [ i ]).ToXml() );
					else
						sb.AppendFormat( " (\"{0}={1}\") ", p.Name , "<null>" );	
				} 
				else 
				{
					sb.AppendFormat( " (\"{0}={1}\") ", p.Name , _params [ i ] );	
				}

				i ++;
			}
			sb.Append ( "]" );
			return sb.ToString();			
		}

		#region IProcessing Members

		public bool PreProcessing(MethodInfo method, params object[] parametersValues)
		{			
			this._params = parametersValues;
			this._methodInfo = method;
			return false;
		}


		public object PosProcessing(System.Reflection.MethodInfo method, object returnValue, params object[] parametersValues)
		{
			return null;
		}

		public bool ProcessingError( Exception e)
		{
			this.WriteExceptionEntry ( e );
			return false;
		}

		#endregion
	}
}
