using System;

namespace SuperPag.Framework.Web.WebControls
{
	public enum DataTypeBindEnum
	{
		Message = 1,
		Value = 2
	}

	/// <summary>
	/// TODO: Comentário
	/// </summary>
	public interface MessageControl
	{
		/// TODO: Comentário
		void MessageInit();

		/// TODO: Comentário
		void MessageBind();	
	
		/// TODO: Comentário
		void MessageUnBind();
	
		//TODO: Comentário
		event MessageEventHandler.MessageDataBind AfterMessageBind;

		//TODO: Comentário
		event MessageEventHandler.MessageDataBind BeforeMessageBind;

		//TODO: Comentário
		void OnAfterMessageBind(object message);

		//TODO: Comentário
		void OnBeforeMessageBind(object message);

		//TODO: Usado para o repeater
		void MessageDataSource(object message);
	}

	public sealed class MessageEventHandler
	{
		//TODO: Comentário
		public delegate void MessageDataBind(object sender, object message);

		//TODO: Comentário
		public delegate void MessagePageDataBind(object sender);

		//TODO: Comentário
		public delegate void MessagePageDataUnBind( object sender, ref bool valid );
	}

	public class MessageReadOnly : Attribute
	{
	}
}
