using System;
using System.Reflection;
using SuperPag.Framework.Data.Components.AutoDataLayer;
using SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes {
	public abstract class OrderedAttributeBase : Attribute, IComparable {
		internal int _Order;

		#region IComparable Members
		public int CompareTo(object obj) {
			OrderedAttributeBase toCompare = (OrderedAttributeBase)obj;
			return this._Order.CompareTo(toCompare._Order);
		}
		#endregion
	}

	#region DefaultDataMessageAttribute
	/// <summary>
	/// Define a DataMessage padrão para a interface
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface)]
	public class DefaultDataMessageAttribute : Attribute {
		internal Type _dataMessageType;

		public DefaultDataMessageAttribute(Type dataMessageType) {
			//Verifica se o tipo é uma DataMessage válida
			if (dataMessageType == null || !dataMessageType.IsSubclassOf(typeof(DataMessageBase))) throw new AutoDataLayerBuildException("Atributo [DefaultDataMessage], parametro [dataMessageType] deve apontar para uma DataMessage válida e derivada do tipo DataMessageBase.");

			this._dataMessageType = dataMessageType;
		}
	}
	#endregion

	#region PersistInMemoryAttribute
	/// <summary>
	/// Define que o dataLayer deve ficar gravado no cache de memória
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface)]
	public class PersistInMemoryAttribute : Attribute {}
	#endregion

	#region MethodTypeAttribute
	/// <summary>
	/// Define o tipo do método
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class MethodTypeAttribute : Attribute {
		internal MethodTypes _type;
		internal Type _customMethodClassType;
		internal string _customMethodName;
		internal string _procName;
		
		public MethodTypeAttribute(MethodTypes type) : this(type, null, null) {}

		public MethodTypeAttribute(MethodTypes type, string procName ) 
		{
			this._type = type;
			this._procName = procName;			
		}

		public MethodTypeAttribute(MethodTypes type, Type customMethodClassType, string customMethodName) {
            //Verifica se é custom method e custom method não foi definido
			if (type == MethodTypes.Custom && (customMethodClassType == null || customMethodName == null || customMethodName.Length == 0))
				throw new AutoDataLayerBuildException("O método custom deve especificar os dados do método estático a ser chamado.");

			if (type != MethodTypes.Custom && (customMethodClassType != null || customMethodName != null)) 
				throw new AutoDataLayerBuildException("O método custom só deve ser especificado para métodos do tipo MethodTypes.Custom.");
			
			this._type = type;
			this._customMethodClassType = customMethodClassType;
			this._customMethodName = customMethodName;
		}
	}
	#endregion

	#region OrderByAttribute
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class OrderByAttribute : OrderedAttributeBase {
		internal DataFieldInfo _fieldInfo = null;
		internal SortOrder _sortOrder;

		public OrderByAttribute (int order, string field, SortOrder sortOrder)
			: this(order, null, field, sortOrder) {}

		public OrderByAttribute (int order, Type messageType, string field, SortOrder sortOrder) {
			//Valida se o DataField é válido
			if (field == null || field.Length == 0) throw new AutoDataLayerBuildException("Atributo [OrderBy], parametro [field] deve possuir um nome de campo válido");
			//Verifica se o tipo é uma DataMessage válida
			if (messageType != null && !messageType.IsSubclassOf(typeof(DataMessageBase))) throw new AutoDataLayerBuildException("Atributo [OrderBy], parametro [messageType] deve apontar para uma DataMessage derivada do tipo DataMessageBase.");

			this._Order = order;
			this._fieldInfo = new DataFieldInfo(messageType, field);
			this._sortOrder = sortOrder;
		}
	}
	#endregion

	#region ReturnFieldAttribute
	
	[AttributeUsage(AttributeTargets.ReturnValue)]
	public class ReturnFieldAttribute : Attribute {
		internal DataFieldInfo _fieldInfo;

		public ReturnFieldAttribute(string field) 
			: this(null, field) {}

		public ReturnFieldAttribute(Type messageType, string field) {
			//Valida se o DataField é válido
			if (field == null || field.Length == 0) throw new AutoDataLayerBuildException("Atributo [ReturnField], parametro [field] deve possuir um nome de campo válido");
			//Verifica se o tipo é uma DataMessage válida
			if (messageType != null && !messageType.IsSubclassOf(typeof(DataMessageBase))) throw new AutoDataLayerBuildException("Atributo [ReturnField], parametro [messageType] deve apontar para uma DataMessage derivada do tipo DataMessageBase.");

			this._fieldInfo = new DataFieldInfo(messageType, field);
		}
	}
	#endregion

	#region DataRelationAttribute
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class DataRelationAttribute : Attribute {
		internal DataFieldInfo _leftFieldInfo = null;
		internal DataFieldInfo _rightFieldInfo = null;
		internal ResolvedFieldInfo _leftResolvedField = null;
		internal ResolvedFieldInfo _rightResolvedField = null;
		internal Join _join;

		public DataRelationAttribute(Type leftMessageType, string leftField, Type rightMessageType, string rightField, Join join) {
			//Valida o campo da esquerda
			if (leftField == null || leftField.Length == 0) throw new AutoDataLayerBuildException("Atributo [DataRelation], parametro [leftField] deve possuir um nome de campo válido");
			//Valida o campo da direita
			if (rightField == null || rightField.Length == 0) throw new AutoDataLayerBuildException("Atributo [DataRelation], parametro [rightField] deve possuir um nome de campo válido");
			//Valida a DataMessage da esquerda
			if (leftMessageType != null && !leftMessageType.IsSubclassOf(typeof(DataMessageBase))) throw new AutoDataLayerBuildException("Atributo [DataRelation], parametro [leftMessageType] pode ser nulo ou apontar para uma DataMessage derivada do tipo DataMessageBase.");
			//Valida a DataMessage da direita
			if (rightMessageType == null || !rightMessageType.IsSubclassOf(typeof(DataMessageBase))) throw new AutoDataLayerBuildException("Atributo [DataRelation], parametro [rightMessageType] deve apontar para uma DataMessage válida e derivada do tipo DataMessageBase.");
						
			this._leftFieldInfo = new DataFieldInfo(leftMessageType, leftField);
			this._rightFieldInfo = new DataFieldInfo(rightMessageType, rightField);
			this._join = join;
		}

		public void ResolveFields(Type defaultMessageType) {
			this._leftResolvedField = this._leftFieldInfo.GetResolvedFieldInfo(defaultMessageType);
			this._rightResolvedField = this._rightFieldInfo.GetResolvedFieldInfo(defaultMessageType);
		}

		public override int GetHashCode() {
			return (_leftResolvedField.DataTableName + _rightResolvedField.DataTableName).GetHashCode();
		}

	}
	#endregion

	#region GroupByAttribute
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class GroupByAttribute : OrderedAttributeBase {
		internal DataFieldInfo _fieldInfo;

		public GroupByAttribute(int order, string field) 
			: this(order, null, field) {}

		public GroupByAttribute(int order, Type messageType, string field) {
			//Valida se o DataField é válido
			if (field == null || field.Length == 0) throw new AutoDataLayerBuildException("Atributo [GroupBy], parametro [field] deve possuir um nome de campo válido");
			//Verifica se o tipo é uma DataMessage válida
			if (messageType != null && !messageType.IsSubclassOf(typeof(DataMessageBase))) throw new AutoDataLayerBuildException("Atributo [GroupBy], parametro [messageType] deve apontar para uma DataMessage derivada do tipo DataMessageBase.");

			this._Order = order;
			this._fieldInfo = new DataFieldInfo(messageType, field);
		}
	}
	#endregion

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class LinkAllAttribute : Attribute 
	{
		public Link Link ;

		public LinkAllAttribute( Link link ) 
		{
			this.Link = link;
		}
	}

	#region WhereAttribute
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class WhereAttribute : OrderedAttributeBase {
		internal string _parameterName = null;
		internal DataFieldInfo _fieldInfo = null;
		internal Filter _Filter;
		internal Link _Link;
		internal Block _Block;
		internal Type _typeOfParameter;
		internal bool isBlock;
		// internal bool isValue = false;
		
		public WhereAttribute(int Order, Block Block, Link Link) {
			this._Block = Block;
			this._Order = Order;
			this._Link = Link;
			this.isBlock = true;
		}

		public WhereAttribute(int Order, string parameterName, string field, Filter Filter, Link Link) 
			: this(Order, parameterName, null, field, Filter, Link) {}

//		public WhereAttribute(int Order, string field, object value, Filter Filter, Link Link) 
//		{
//			//Valida se o DataField é válido
//			if (field == null || field.Length == 0) throw new AutoDataLayerBuildException("Atributo [Where], parametro [field] deve possuir um nome de campo válido");
//			
//			this._Filter = Filter;
//			this._Link = Link;
//			this._Block = Block.None;
//			this._Order = Order;
//			this.isValue = true;
//			this.isBlock = false;
//		}

		public WhereAttribute(int Order, string parameterName, Type messageType, string field, Filter Filter, Link Link) {
			//Valida se o DataField é válido
			if (field == null || field.Length == 0) throw new AutoDataLayerBuildException("Atributo [Where], parametro [field] deve possuir um nome de campo válido");
			//Valida se o parameterName foi especificado ou o método não exige
			if ((parameterName == null || parameterName.Length == 0) && Filter != Filter.IsNull && Filter != Filter.NotIsNull)
				throw new AutoDataLayerBuildException("Atribute [Where] parametro [parameterName] só pode não ser especificado para filtros do tipo Filter.IsNull ou Filter.NotIsNull");
			//Verifica se o tipo é uma DataMessage válida
			if (messageType != null && !messageType.IsSubclassOf(typeof(DataMessageBase))) throw new AutoDataLayerBuildException("Atributo [Where], parametro [messageType] deve apontar para uma DataMessage derivada do tipo DataMessageBase.");

			this._fieldInfo = new DataFieldInfo(messageType, field);
			this._Filter = Filter;
			this._Link = Link;
			this._Block = Block.None;
			this._Order = Order;
			this._parameterName = parameterName;
			this.isBlock = false;
		}
	}
	#endregion

	#region AggregationAttribute
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class AggregationAttribute : Attribute {
		internal DataFieldInfo _fieldInfo;
		internal AggregationType _aggregationType;

		public AggregationAttribute(string field, AggregationType aggregationType) 
			: this(null, field, aggregationType) {}

		public AggregationAttribute(Type messageType, string field, AggregationType aggregationType) {
			//Valida se o DataField é válido
			if (field == null || field.Length == 0) throw new AutoDataLayerBuildException("Atributo [Aggregation], parametro [field] deve possuir um nome de campo válido");
			//Verifica se o tipo é uma DataMessage válida
			if (messageType != null && !messageType.IsSubclassOf(typeof(DataMessageBase))) throw new AutoDataLayerBuildException("Atributo [Aggregation], parametro [messageType] deve apontar para uma DataMessage derivada do tipo DataMessageBase.");

			this._fieldInfo = new DataFieldInfo(messageType, field);
			this._aggregationType = aggregationType;
		}
	}
	#endregion
}
