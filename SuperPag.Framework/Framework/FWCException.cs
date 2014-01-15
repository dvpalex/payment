using System;

namespace SuperPag.Framework
{
	public class FWCException : Exception 
	{
		public FWCException() : base () {}
		public FWCException(string Message) : base (Message) {}
		public FWCException(string Message, Exception InnerException) : base (Message, InnerException) {}
	}

	/// <summary>
	/// Business Exception (disparada pelo business framework
	/// </summary>
	public class BusinessException : FWCException 
	{
		public BusinessException(string Message, Exception InnerException) : base (Message, InnerException) {}
	}

	/// <summary>
	/// Exception utilizada para identificar erros a chamadas a outros módulos através do MethodInvoker
	/// </summary>
	public class MethodInvokerException : Exception 
	{
		public MethodInvokerException(string Message) : base (Message) {}
		public MethodInvokerException(string Message, Exception InnerException) : base (Message, InnerException) {}
	}

	/// <summary>
	/// Summary description for Exceptions.
	/// </summary>
	public class ConstraintViolationException  : FWCException 
	{
		public string _field = "";

		public string Field
		{
			get { return this._field; }
		}

		public ConstraintViolationException(string message, string _field) : base ( message )
		{
		}

		public ConstraintViolationException(string message) : base ( message )
		{
		}
	}
}
