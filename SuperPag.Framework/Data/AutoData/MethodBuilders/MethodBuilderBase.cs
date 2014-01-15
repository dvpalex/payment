using System;
using System.CodeDom;
using System.Collections;
using System.Reflection;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders {
	internal abstract class MethodBuilderBase {
		protected CodeMemberMethod _methodImpl;
		protected MethodInfo _methodInfo;
		protected Type _defaultMessageType = null;
		protected Type _returnType = null;
		protected bool _isReturnArray = false;

		public MethodBuilderBase(CodeMemberMethod methodImpl, MethodInfo methodInfo) {
			this._methodImpl = methodImpl;
			this._methodInfo = methodInfo;
		}

		public Type[] Build( ArrayList listOfUsedTypes ) {
			try {
				//Obtem o defaultMessagaType e dispara um erro caso não consiga
				DefaultDataMessageAttribute defaultMessageAttribute = (DefaultDataMessageAttribute)Attribute.GetCustomAttribute(_methodInfo.DeclaringType, typeof(DefaultDataMessageAttribute), true);
				if (defaultMessageAttribute != null && defaultMessageAttribute._dataMessageType != null) {
					this._defaultMessageType = defaultMessageAttribute._dataMessageType;
				} else {
					throw new AutoDataLayerBuildException("O atributo [DefaultDataMessage] não foi definido para a interface.");
				}

				//Obtem o retorno do método
				this._returnType = _methodInfo.ReturnType;
				//Verifica se o retorno é um array
				if (this._returnType.IsArray) {
					this._isReturnArray = true;
					this._returnType = this._returnType.GetElementType();
				}

				//Verifica se o retorno é uma dataMessage
				if (this._returnType.IsSubclassOf(typeof(DataMessageBase))) {
					//Define a default datamessage como o tipo da DataMessage especificada no retorno
					this._defaultMessageType = this._returnType;
				}
				
				foreach ( Type t in _defaultMessageType.GetInterfaces() )
				{
					listOfUsedTypes.Add ( t );
				}

				return this.BuildMethod();
			} catch (AutoDataLayerBuildException ex) {
				throw new AutoDataLayerBuildException(string.Format("Erro na geração do método: {0}.{1}; Verifique o erro: {2}", _methodInfo.DeclaringType.FullName, _methodInfo.Name, ex.Message), ex);
			} catch (Exception ex) {
				throw new ApplicationException(string.Format("Erro inexperado na geração do método: {0}.{1}; Erro ocorrido: {2}", _methodInfo.DeclaringType.FullName, _methodInfo.Name, ex.Message), ex);
			}
		}

		protected abstract Type[] BuildMethod( );
	}
}
