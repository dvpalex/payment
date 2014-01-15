using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using SuperPag.Framework;
using SuperPag.Framework.Helper;

namespace SuperPag.Framework.Web.WebControls
{
	///	TODO: Comentário
	[Designer(typeof(ButtonDesigner)),
	ToolboxData("<{0}:EventButton runat=server></{0}:EventButton>")]
	public class EventButton : System.Web.UI.WebControls.Button, IEventControl
	{
		#region Propriedades e Eventos

		private string _eventName;
		private string _msgValueToCompare;
		private Type _eventType;
		private string _msgSource = "";
		private string _msgSourceKey = "";
		private string _msgVisibleField = "";
		private string _msgCompareField = "";
		object _message;
		MessageControlBuilder _builder;
		
		//TODO: Rever
		//Propriedade para verificar se faz comparação com minvalue ou o valor passado em MsgValueToCompare 
		private MsgValidationTypeEnum _listMsgValidationType = MsgValidationTypeEnum.None;

		//TODO: Rever
		//Propriedade para verificar como devera ser comparado os dados
		private MsgCompareOperatorEnum _listMsgCompareOperator = MsgCompareOperatorEnum.Equal;

		//TODO: Rever
		//Tipo de dado a ser comparado
		private MsgCompareDataTypesEnum _listMsgCompareDataTypesEnum = MsgCompareDataTypesEnum.Integer;


		//Message Source
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgSource
		{get{return _msgSource;}set{_msgSource = value;}}

		[Bindable(true), Category("Event Info"), DefaultValue("")]
		public string EventName
		{get{return _eventName;}set{_eventName = value;}}

		[Bindable(true), Category("Event Bind"), DefaultValue("")]
		public Type EventType
		{get{return _eventType;}set{_eventType = value;}}

		//Visible Field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgVisibleField
		{get{return _msgVisibleField;}set{_msgVisibleField = value;}}

		//Visible Field
		[Bindable(true), Category("Message Compare"), DefaultValue("")]
		public string MsgCompareField
		{get{return _msgCompareField;}set{_msgCompareField = value;}}

		//Compara visible field
		[Bindable(true), Category("Message Bind"), DefaultValue("")]
		public string MsgValueToCompare
		{get{return _msgValueToCompare;}set{_msgValueToCompare = value;}}

		//TODO: Comentário
		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind AfterMessageBind;

		[Bindable(true), Category("Message Event"), DefaultValue("")]
		public event MessageEventHandler.MessageDataBind BeforeMessageBind;

		//TODO: Rever
		//Qual o tipo de verificação a ser usado
		[Bindable(true), Category("Message Compare"), DefaultValue("")]
		public MsgValidationTypeEnum MsgValidationType
		{
			get{return _listMsgValidationType;}
			set
			{
				_listMsgValidationType = value;
			}
		}

		//TODO: Rever
		//Qual o tipo de verificação a ser usado
		[Bindable(true), Category("Message Compare"), DefaultValue("")]
		public MsgCompareOperatorEnum MsgCompareOperator
		{
			get{return _listMsgCompareOperator;}
			set
			{
				_listMsgCompareOperator = value;
			}
		}

		//TODO: Rever
		//Tipos válidos para comparação
		[Bindable(true), Category("Message Compare"), DefaultValue("")]
		public MsgCompareDataTypesEnum MsgCompareDataType
		{
			get{return _listMsgCompareDataTypesEnum;}
			set
			{
				_listMsgCompareDataTypesEnum = value;
			}
		}

		//TODO: Comentário
		protected override void OnInit(System.EventArgs e)
		{
			base.OnInit(e);
		}

		//TODO: Comentário
		public void OnAfterMessageBind(object message)
		{
			if(AfterMessageBind != null)
			{
				AfterMessageBind(this, message);
			}
		}

		//TODO: Comentário
		public void OnBeforeMessageBind(object message)
		{
			if(BeforeMessageBind != null)
			{
				BeforeMessageBind(this, message);
			}
		}
		
		#endregion Propriedades e Eventos

		#region Message Init

		public void MessageInit()
		{
			if(this.Page is MessagePage)
			{
				MessagePage page = (MessagePage)this.Page;

				if(_msgSource != String.Empty && _msgSource != null)
				{			
					if(_msgSourceKey != "")
					{
						
						_message = page.Messages.Get(_msgSourceKey);
					} 
					else
					{
						_message = page.Messages.Get(_msgSource);
					}
				}
			}
			else
			{
				throw new ApplicationException("The MsgButton must be placed inside a MessagePage ");
			}
			_builder = new MessageControlBuilder();
		}

		#endregion Message Init

		public void Show()
		{
			this.Visible = true;
		}
		
		public string GetEventName()
		{
			return _eventName;
		}

		public void Hide()
		{
			this.Visible = false;
		}

		public void MessageBind()
		{
			OnBeforeMessageBind(_message);

			if (_message != null) 
			{
				this.Visible = ValidationField( (Message)_message );
			}
			OnAfterMessageBind(_message);
		}

		//TODO: Comentário						
		public void MessageUnBind(){}

		//TODO: Rever
		internal bool ValidationField( Message _message ) 
		{			
			//Verifica se mostra o botão pela verificação de comparação com uma propriedade
			if ( this.MsgCompareField != String.Empty && this.MsgCompareField != null ) 
			{
				object propValue = _builder.GetObjectProperty(_message, this.MsgCompareField, false, false);
				if ( (propValue is Enum) )
					propValue = (int)propValue;

				if ( this.MsgValueToCompare != null && this.MsgValueToCompare != "" && this.MsgValidationType == MsgValidationTypeEnum.ValueToCompare  )
				{
					Tristate t = HelperCompareValues.CompareTwoValues( this.MsgCompareDataType, this.MsgCompareOperator, propValue , this.MsgValueToCompare );
					if ( t != null ) return (bool)t;
				}
				else if ( this.MsgValidationType == MsgValidationTypeEnum.MinValue  )
				{
					Tristate t = HelperCompareValues.CompareToMinValue( this.MsgCompareDataType, this.MsgCompareOperator, propValue);
					if ( t != null ) return (bool)t;
				}
			}


			//Verifica se mostra o botão pela verificação do campo visível
			if ( this.MsgVisibleField != null && this.MsgVisibleField != "" &&
				this.MsgValueToCompare != null && this.MsgValueToCompare != "") 
			{
				object propValue = _builder.GetObjectProperty(_message, this.MsgVisibleField, false, false);
				if ( (propValue is Enum) )
					propValue = (int)propValue;

				Tristate t = HelperCompareValues.CompareTwoValues( this.MsgCompareDataType, this.MsgCompareOperator, propValue , this.MsgValueToCompare );
				
				if ( t != null ) return (bool)t;
			}

			return this.Visible;
		}

		//é usado pelo repeater
		public void MessageDataSource(object message) 
		{
			_builder= new MessageControlBuilder();
			_message = message;
		}
	}

	//TODO: Comentário
	public class ButtonDesigner : System.Web.UI.Design.ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			string designTimeHtml = base.GetDesignTimeHtml ();
			EventButton button = (EventButton)this.Component;
			
			string _value = "value=\"{" + button.EventName + "}\"";

			designTimeHtml =
				designTimeHtml.Insert(designTimeHtml.Length - 2, _value);

			return designTimeHtml;
		}
	}
}
