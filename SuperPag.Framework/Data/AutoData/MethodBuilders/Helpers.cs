using System;
using System.Data;
using System.CodeDom;
using System.Collections;
using System.Reflection;
using System.Data.OracleClient;
using SuperPag.Framework.Data.Components;
using SuperPag.Framework.Data.Components.AutoDataLayer.DataInterfaceAttributes;
using DMAtt = SuperPag.Framework.Data.Components.AutoDataLayer.DataMessageAttributes;

namespace SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders {
	internal class CodeHelper {
		public static CodeStatement MethodInvoke(string varName, string methodName, params object[] parameters) {
			CodeMethodInvokeExpression codeMethodInvokeExp = new CodeMethodInvokeExpression(
				new CodeVariableReferenceExpression(varName), methodName
				);

			foreach (string param in parameters) {
				codeMethodInvokeExp.Parameters.Add(new CodeSnippetExpression( param ));
			}

			return new CodeExpressionStatement(codeMethodInvokeExp);
		}
	}

	internal class Helpers {
		#region Conversão de tipos CSharp / SqlDBType
		public static string GetNullValue(Type valueType) {
			if (valueType.IsClass) return "null";
			else if (valueType == typeof(long)) return "long.MinValue";
			else if (valueType == typeof(int)) return "int.MinValue";
			else if (valueType == typeof(byte)) return "byte.MinValue";
			else if (valueType == typeof(short)) return "short.MinValue";
			else if (valueType == typeof(string)) return "null";
			else if (valueType == typeof(bool)) return "false";
			else if (valueType == typeof(Tristate)) return "null";
			else if (valueType == typeof(DateTime)) return "DateTime.MinValue";
			else if (valueType == typeof(decimal)) return "decimal.MinValue";
			else if (valueType == typeof(byte[])) return "null";			 
			else if (valueType == typeof(System.Guid)) return "Guid.Empty";
			else throw new AutoDataLayerBuildException(string.Format("GetNullValue - Tipo não suportado [{0}]", valueType.FullName));			
		}

		public static string GetConvertTo(Type type, string value) {
			string result = null;
			if (type == typeof(int)) result = "Convert.ToInt32({0})";
			else if (type == typeof(long)) result =  "Convert.ToInt64({0})";
			else if (type == typeof(short)) result =  "Convert.ToInt16({0})";			
			else if (type == typeof(byte)) result =  "Convert.ToByte({0})";
			else if (type == typeof(string)) result =  "Convert.ToString({0})";
			else if (type == typeof(bool)) result =  "Convert.ToBoolean({0})";
			else if (type == typeof(Tristate)) result =  "(Tristate)Convert.ToBoolean({0})";
			else if (type == typeof(DateTime)) result =  "Convert.ToDateTime({0})";
			else if (type == typeof(decimal)) result =  "Convert.ToDecimal({0})";
			else if (type == typeof(byte[])) result =  "(byte[]){0}";
#if SQL
			else if (type == typeof(System.Guid)) result =  "(Guid){0}";
#elif ORACLE
            else if (type == typeof(System.Guid)) result = "(Guid)new Guid((string){0})";
#else
			else if (type == typeof(System.Guid)) result =  "(Guid){0}";
#endif
            else throw new AutoDataLayerBuildException(string.Format("GetConvertTo - Tipo não suportado [{0}]", type.FullName));
			
			return string.Format(result, value);
		}

		public static SqlDbType CSharpToSqlDBType(Type csharpType) {
			if (csharpType == typeof(int)) return SqlDbType.Int;
			else if (csharpType == typeof(long)) return SqlDbType.BigInt;
			else if (csharpType == typeof(short)) return SqlDbType.SmallInt;
			else if (csharpType == typeof(byte)) return SqlDbType.TinyInt;
			else if (csharpType == typeof(string)) return SqlDbType.VarChar;
			else if (csharpType == typeof(bool)) return SqlDbType.Bit;
			else if (csharpType == typeof(Tristate)) return SqlDbType.Bit;
			else if (csharpType == typeof(DateTime)) return SqlDbType.DateTime;
			else if (csharpType == typeof(decimal)) return SqlDbType.Decimal;
			else if (csharpType == typeof(byte[])) return SqlDbType.Image;
			else if (csharpType == typeof(System.Guid)) return SqlDbType.UniqueIdentifier;
			else throw new AutoDataLayerBuildException(string.Format("CSharpToSqlDBType - Tipo não suportado [{0}]", csharpType.FullName));
		}
        
        public static OracleType CSharpToOracleType(Type csharpType)
        {
            if (csharpType == typeof(int)) return OracleType.Int32;
            else if (csharpType == typeof(long)) return OracleType.Number;
            else if (csharpType == typeof(short)) return OracleType.Int16;
            else if (csharpType == typeof(byte)) return OracleType.Byte;
            else if (csharpType == typeof(string)) return OracleType.VarChar;
            else if (csharpType == typeof(bool)) return OracleType.Number;
            else if (csharpType == typeof(Tristate)) return OracleType.Number;
            else if (csharpType == typeof(DateTime)) return OracleType.DateTime;
            else if (csharpType == typeof(decimal)) return OracleType.Number;
            else if (csharpType == typeof(byte[])) return OracleType.Blob;
            else if (csharpType == typeof(System.Guid)) return OracleType.VarChar;
            else throw new AutoDataLayerBuildException(string.Format("CSharpToOracleType - Tipo não suportado [{0}]", csharpType.FullName));
        }
        #endregion	

		public static bool SortAndCheckOrder(OrderedAttributeBase[] attributes) {
			Array.Sort(attributes);

			for(int i = 0; i < attributes.Length; i++) {
				if (attributes[i]._Order != i) {
					return false;
				}
			}

			return true;
		}

		public static bool CheckIsPrimaryGUID(FieldInfo fieldInfo) {
			//Tenta obter o atributo PrimaryKey
			DMAtt.PrimaryKeyAttribute primaryKey = (DMAtt.PrimaryKeyAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DMAtt.PrimaryKeyAttribute));
			return primaryKey == null ? false : primaryKey._isGUID;
		}

		public static bool CheckIsPrimaryKey(FieldInfo fieldInfo) {
			//Verifica se o atributo existe
			return Attribute.IsDefined(fieldInfo, typeof(DMAtt.PrimaryKeyAttribute));
		}

		public static bool CheckIsIdentity(FieldInfo fieldInfo) {
			//Verifica se o atributo existe
			return Attribute.IsDefined(fieldInfo, typeof(DMAtt.IdentityAttribute));
		}

		public static void UpdateHashUniques(Hashtable hashTable, DataRelationAttribute item) {
			//Verifica se o item informado não é nulo
			if (item != null && !hashTable.ContainsKey(item.GetHashCode())) {
				hashTable.Add(item.GetHashCode(), item);
			}
		}

		public static bool CheckIsValidMessage(Type type) {
			return type != null && type.IsSubclassOf(typeof(DataMessageBase));
		}

		public static bool CheckISValueType(Type type) {
			//Verifica se o tipo é um array
			Type typeToCheck = type.IsArray ? type.GetElementType() : type;

			//Verifica se é valueType, string ou Tristate
			if (typeToCheck.IsValueType || typeToCheck == typeof(string) || typeToCheck == typeof(Tristate)) {
				return true;
			} else {
				//Caso contrário, deve ser uma classe, então verifica se é uma DataMessage
				if (typeToCheck.IsSubclassOf(typeof(DataMessageBase))) {
					return false;
				}
			}
			//TODO: verificar se consigo detalhar melhor a ocorrência do tipo inválido
			//Caso chegou não é um tipo válido
			throw new ApplicationException("Tipo inválido: " + type.FullName);
		}

		public static string GetDataTableName(Type dataMessageType) {
			DMAtt.DefaultDataTableNameAttribute dataTable = (DMAtt.DefaultDataTableNameAttribute)Attribute.GetCustomAttribute(dataMessageType, typeof(DMAtt.DefaultDataTableNameAttribute));
			//Verifica se encontrou o Atribute
			if (dataTable != null) {
				return dataTable._dataTableName;
			} else {
				throw new ApplicationException("Atributo [DefaultDataTableNameAttribute] não encontrado para o tipo " + dataMessageType.FullName);
			}
		}

		public static AggregationAttribute CheckForAggregation(ResolvedFieldInfo resolvedField, Type defaultMessageType, MethodInfo methodInfo) {
			//Verifica se existe algum atributo de agregação definido no método
			AggregationAttribute[] arrAggregationAtt = (AggregationAttribute[])Attribute.GetCustomAttributes(methodInfo, typeof(AggregationAttribute), true);
			if (arrAggregationAtt != null && arrAggregationAtt.Length > 0) {
				//Verifica se o atributo de aggregation é para esse field
				for (int i = 0; i < arrAggregationAtt.Length; i++) {
					ResolvedFieldInfo aggResolvedField = arrAggregationAtt[i]._fieldInfo.GetResolvedFieldInfo(defaultMessageType);
					if (aggResolvedField.DataFieldName == resolvedField.DataFieldName) {
						return arrAggregationAtt[i];
					}
				}
			}

			//Se não achou no método, pesquisa no field
			return (AggregationAttribute)Attribute.GetCustomAttribute(resolvedField.FieldInfo, typeof(AggregationAttribute), true);
		}

		public static string ResolveJoinType(Join join) {
			switch(join) {
				case Join.Left: return "LEFT";
				case Join.Inner: return "INNER";
				case Join.Right: return "RIGHT";
				default:
					throw new ApplicationException("Join não suportado " + Enum.GetName(typeof(Join), join));
			}
		}

		public static string ResolveSortOrderEnum(SortOrder sortOrder) {
			switch(sortOrder) {
				case SortOrder.ASC: return "ASC";
				case SortOrder.DESC: return "DESC";
				default:
					throw new ApplicationException("SortOrder não suportado " + Enum.GetName(typeof(SortOrder), sortOrder));
			}
		}

		public static string ResolveAggregation(AggregationType aggregation, string field) {
			switch (aggregation) {
				case AggregationType.Avg:
					return string.Format("AVG({0})", field);
				case AggregationType.Count:
					return string.Format("COUNT({0})", field);
				case AggregationType.Max:
					return string.Format("MAX({0})", field);
				case AggregationType.Min:
					return string.Format("MIN({0})", field);
				case AggregationType.Sum:
					return string.Format("SUM({0})", field);
				default:
					throw new ApplicationException("Aggregation não suportado: " + Enum.GetName(typeof(AggregationType), aggregation));
			}
		}
	}
}
