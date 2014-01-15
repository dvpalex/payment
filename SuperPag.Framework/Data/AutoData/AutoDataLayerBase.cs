using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Collections;
using SuperPag.Framework.Data.Components.Data;
using SuperPag.Framework.Data.Components.Data.Objects.Helpers;
using SuperPag.Framework.Data.Components.Data.Objects.SqlServer.Helpers;
using SuperPag.Framework.Data.Components.Data.Objects.SqlServer;
using SuperPag.Framework.Data.Components.AutoDataLayer.MethodBuilders;

namespace SuperPag.Framework.Data.Components.AutoDataLayer 
{
	public abstract class DataLayerBase : DataObject
	{

		public static string ResolveFilter( string Field, Filter Filter, string parameterName ) {
			switch( Filter ) {
				case Filter.Equal:
					return String.Format( "{0} = {1}", Field, parameterName );
				case Filter.NotEqual:
					return String.Format( "NOT( {0} = {1})", Field, parameterName );
				case Filter.LessThan:
					return String.Format( "{0} < {1}", Field, parameterName );
				case Filter.GreaterThan:
					return String.Format( "{0} > {1}", Field, parameterName );					
				case Filter.LessOrEqual:
					return String.Format( "{0} <= {1}", Field, parameterName );					
				case Filter.GreaterOrEqual:
					return String.Format( "{0} >= {1}", Field, parameterName );					
				case Filter.Like:
				case Filter.LikeLeft:
				case Filter.LikeRight:
				case Filter.LikeLeftRight:
					return String.Format( "{0} LIKE {1}", Field, parameterName );
				case Filter.NotLike:
					return String.Format( "NOT( {0} LIKE {1} )", Field, parameterName );					
				case Filter.IsNull:
					return String.Format("{0} is null", Field);
				case Filter.NotIsNull:
					return String.Format("{0} is not null", Field);
				default:
					throw new ApplicationException("FilterEnum não suportado: " + Enum.GetName(typeof(Filter), Filter));
			}
		}

		public static string ResolveLink( Link Link ) {
			switch( Link ) {
				case Link.And:
					return "AND";
				case Link.Or:
					return "OR";
				default:
					throw new ApplicationException("LinkEnum não suportado: " + Enum.GetName(typeof(Link), Link));
			}
		}

		protected class Where {
			internal ArrayList listOfParameters = new ArrayList();
			internal ArrayList listOfWhereDatas = new ArrayList();

			public void Append(Block block, Link link) {
				listOfWhereDatas.Add(new WhereData(block, link));
			}

			public void Append(string field, Filter filter, object value, Link link) 
			{
				if ( filter == Filter.LikeLeft )
				{
					value = "%" + value ;
				} 
				else if ( filter == Filter.LikeRight )
				{
					value = value + "%" ;
				}
				else if ( filter == Filter.LikeLeftRight )
				{
					value = "%" +  value + "%" ;
				}


				if (filter != Filter.IsNull && filter != Filter.NotIsNull) 
				{
#if SQL
					string paramName = string.Format("@P{0}", listOfParameters.Count);
#elif ORACLE
                    string paramName = string.Format(":P{0}", listOfParameters.Count);
#else
					string paramName = string.Format("@P{0}", listOfParameters.Count);
#endif
                    IDbDataParameter param = null;

					if (null == value) {
#if SQL
						param = new SqlParameter(paramName, null);
#elif ORACLE
						param = new OracleParameter(paramName, null);
#else
                        param = new SqlParameter(paramName, null);
#endif
					} else {
#if SQL
                        param = new SqlParameter(paramName, Helpers.CSharpToSqlDBType(value.GetType()));
						param.Value = value;
#elif ORACLE
                        param = new OracleParameter(paramName, Helpers.CSharpToOracleType(value.GetType()));
                        param.Value = (value.GetType().Equals(typeof(Guid)) ? value.ToString() : value);
#else
                        param = new SqlParameter(paramName, Helpers.CSharpToSqlDBType(value.GetType()));
						param.Value = value;
#endif
                    }

					listOfParameters.Add(param);
					listOfWhereDatas.Add(new WhereData(field, filter, paramName, link));
				} 
				else {
					listOfWhereDatas.Add(new WhereData(field, filter, null, link));
				}
			}

#if SQL
			public SqlParameter[] GetParameters() {
				return (SqlParameter[])listOfParameters.ToArray(typeof(SqlParameter));
			}
#elif ORACLE
			public OracleParameter[] GetParameters() {
                return (OracleParameter[])listOfParameters.ToArray(typeof(OracleParameter));
			}
#else
			public SqlParameter[] GetParameters() {
				return (SqlParameter[])listOfParameters.ToArray(typeof(SqlParameter));
			}
#endif

            public override string ToString() {
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				WhereData[] whereDataFull = (WhereData[])listOfWhereDatas.ToArray(typeof(WhereData));
				ArrayList listOfValidWhereData = new ArrayList();
				bool whereValid = false;

				//Verifica se existe algo além de blocos vazios
				//e remove blocos vazios
				for( int i = 0, l = whereDataFull.Length; i < l; i++) {
					if (whereDataFull[i].isBlock) {
						//Verifica se é início de bloco e se não é o último wheredada
						if (i < l - 1 && whereDataFull[i].Block == Block.Begin) {
							//Verifica se o próximo não é fim de bloco
							if (whereDataFull[i + 1].Block == Block.End) {
								//Se é, quer dizer que o bloco está vazio e não iremos adicionálo
								//o próximo também será ignorado pois é o fechamento
								i++;
								continue;
							} 
						}

						listOfValidWhereData.Add(whereDataFull[i]);
					} 
					else {
						whereValid = true;
						listOfValidWhereData.Add(whereDataFull[i]);
					}
				}

				//Verifica se deve montar a cláusula where (se existe algo além de blocos)
				if (whereValid) {
					//Obtem o whereData somente com os elementos válidos
					WhereData[] whereData = (WhereData[]) listOfValidWhereData.ToArray(typeof(WhereData));

					//Adiciona a clausa where
					sb.Append( " WHERE " );

					for( int i = 0, l = whereData.Length; i < l; i++ ) {
						WhereData wd = whereData[ i ];
				
						//Verifica se é bloco
						if (wd.isBlock) {
							if (wd.Block == Block.Begin) {
								//Verifica se não é o último wheredata
								//if ( i < l - 1 ) {
								//Verifica se o próximo não é um endblock
								//if (!(whereData[i + 1].isBlock && whereData[i + 1].Block == Block.End)) {
								sb.Append (" ( ");
								//} else {
								//Pula o fechamento do bloco
								//	i++;
								//}																	  
								//}
							} 
							else { 
							  //Block.End
								sb.Append (" ) ");

								//Verifica se não é o último wheredata
								if( i < l - 1 ) {
									//Verifica se o próximo não é um endblock
									if (!(whereData[i + 1].isBlock && whereData[i + 1].Block == Block.End)) {
										sb.AppendFormat( " {0} ", ResolveLink( wd.Link ) );
									}
								}
							}
						} 
						else {
							sb.AppendFormat( " {0} ", ResolveFilter( wd.Field, wd.Filter, wd.ParamName )  );
				
							//Verifica se não é o último wheredata
							if( i < l - 1 ) {
								//Verifica se o próximo não é um endblock
								if (!(whereData[i + 1].isBlock && whereData[i + 1].Block == Block.End)) {
									sb.AppendFormat( " {0} ", ResolveLink( wd.Link ) );
								}
							}
						}
					}
				}

				return sb.ToString();
			}
		}

		internal class WhereData {
			public string Field;
			public Filter Filter; 
			public Link Link;
			public Block Block;
			internal bool isBlock;
			public string ParamName;

			public WhereData(Block block, Link link) {
				this.isBlock = true;
				this.Block = block;
				this.Link = link;
			}

			public WhereData( string field, Filter filter, string paramName, Link link ) {
				this.Field = field;
				this.Filter = filter; 
				this.Link = link;
				this.isBlock = false;
				this.ParamName = paramName;
			}
		}
	}
}
