using System;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes {
	#region DefaultDataTableNameAttribute
	/// <summary>
	/// Define o nome da tabela padr�o para essa DataMessage
	/// </summary>
	/// <remarks>USO: DataMessages</remarks>
	[AttributeUsage(AttributeTargets.Class)]
	public class DefaultDataTableNameAttribute : Attribute {
		public readonly string _dataTableName = null;

		public DefaultDataTableNameAttribute(string dataTableName) {
			//Valida se o dataTableName � v�lido
			if (dataTableName == null || dataTableName.Length == 0) throw new AutoDataLayerBuildException("Atributo [DefaultDataTableName] deve possuir um nome de Tabela v�lido.");
			this._dataTableName = dataTableName;
		}
	}
	#endregion

	#region DataReferenceAttribute
	/// <summary>
	/// Define a refer�ncia para a tabela e/ou campo
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class DataReferenceAttribute : Attribute {
		internal Type _messageType;
		internal string _field;

		public DataReferenceAttribute(Type messageType)
			: this(messageType, null) {}

		public DataReferenceAttribute(string field)
			: this(null, field) {}

		public DataReferenceAttribute(Type messageType, string field) {
			//Verifica se o tipo � uma DataMessage v�lida
			if (messageType != null && !messageType.IsSubclassOf(typeof(DataMessageBase))) throw new AutoDataLayerBuildException("Atributo [DataReference], parametro [messageType] deve apontar para uma DataMessage derivada do tipo DataMessageBase.");

			this._messageType = messageType;
			this._field = field;
		}
	}
	#endregion

	#region DataRelationAttribute
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class DataRelationAttribute : DataInterfaceAttributes.DataRelationAttribute {
		public DataRelationAttribute(string leftField, Type rightMessageType, string rightField, Join join) 
			: base(null, leftField, rightMessageType, rightField, join) {}

		public DataRelationAttribute(Type leftMessageType, string leftField, Type rightMessageType, string rightField, Join join)
			: base(leftMessageType, leftField, rightMessageType, rightField, join) {}
	}
	#endregion

	#region AggregationAttribute
	[AttributeUsage(AttributeTargets.Field)]
	public class AggregationAttribute : DataInterfaceAttributes.AggregationAttribute {
		public AggregationAttribute(AggregationType aggregationType) 
			: base (null, aggregationType) {}
	}
	#endregion

	#region PrimaryKeyAttribute
	[AttributeUsage(AttributeTargets.Field)]
	public class PrimaryKeyAttribute : Attribute {
		public readonly bool _isGUID;

		public PrimaryKeyAttribute ()
			: this(false) {}

		public PrimaryKeyAttribute (bool isGUID) {
			this._isGUID = isGUID;
		}
	}
	#endregion

	#region IdentityAttribute
	[AttributeUsage(AttributeTargets.Field)]
	public class IdentityAttribute : Attribute {}
	#endregion

	#region ExcludeFromQueryAttribute
	
	[AttributeUsage(AttributeTargets.Field)]
	public class ExcludeFromQueryAttribute : Attribute {}

	#endregion
	

}
