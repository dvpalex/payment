using System;

namespace SuperPag.Framework.Data.Components
{
	public class SuperPagFrameworkException : Exception {
		public SuperPagFrameworkException() : base () {}
		public SuperPagFrameworkException(string Message) : base (Message) {}
		public SuperPagFrameworkException(string Message, Exception InnerException) : base (Message, InnerException) {}
	}

	/// <summary>
	/// Business Exception (disparada pelo business framework
	/// </summary>
	public class BusinessException : SuperPagFrameworkException {
		public BusinessException(string Message, Exception InnerException) : base (Message, InnerException) {}
	}

	/// <summary>
	/// Exception utilizada para identificar erros a chamadas a outros módulos através do MethodInvoker
	/// </summary>
	public class MethodInvokerException : Exception {
		public MethodInvokerException(string Message) : base (Message) {}
		public MethodInvokerException(string Message, Exception InnerException) : base (Message, InnerException) {}
	}

	/// <summary>
	/// Summary description for Exceptions.
	/// </summary>
	public class ConstraintViolationException  : SuperPagFrameworkException 
	{
		public string _field = "";

		public string Field
		{
			get
			{
				return this._field;
			}
		}

		public ConstraintViolationException(string message, string _field) : base ( message )
		{
		}

		public ConstraintViolationException(string message) : base ( message )
		{
		}
	}
}
