using System;

namespace SuperPag.Framework.Web.WebControls
{
	public enum DataTypeBindEnum
	{
		Message = 1,
		Value = 2
	}

	/// <summary>
	/// TODO: Coment�rio
	/// </summary>
	public interface MessageControl
	{
		/// TODO: Coment�rio
		void MessageInit();

		/// TODO: Coment�rio
		void MessageBind();	
	
		/// TODO: Coment�rio
		void MessageUnBind();
	
		//TODO: Coment�rio
		event MessageEventHandler.MessageDataBind AfterMessageBind;

		//TODO: Coment�rio
		event MessageEventHandler.MessageDataBind BeforeMessageBind;

		//TODO: Coment�rio
		void OnAfterMessageBind(object message);

		//TODO: Coment�rio
		void OnBeforeMessageBind(object message);

		//TODO: Usado para o repeater
		void MessageDataSource(object message);
	}

	public sealed class MessageEventHandler
	{
		//TODO: Coment�rio
		public delegate void MessageDataBind(object sender, object message);

		//TODO: Coment�rio
		public delegate void MessagePageDataBind(object sender);

		//TODO: Coment�rio
		public delegate void MessagePageDataUnBind( object sender, ref bool valid );
	}

	public class MessageReadOnly : Attribute
	{
	}
}
