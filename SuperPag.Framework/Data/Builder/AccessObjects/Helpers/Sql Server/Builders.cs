using System;
using System.Collections;
using System.Text;

namespace SuperPag.Framework.Data.Components.Data.Objects.SqlServer.Helpers.Builders {
	public enum Join {
		Inner,
		Left,
		Right,
		Full,
		Cross
	}

	public enum Filter {
		Equal,
		NotEqual,
		LessThan,
		GreaterThan,
		LessOrEqual,
		GreaterOrEqual,
		Like,
		NotLike,
		Between,
		NotBetween,
		IsNull,
		NotIsNull
	}

	public enum Link {	
		And,
		Or
	}

	public enum Block {
		Begin,
		End
	}

	public sealed class FieldData {
		public FieldData( string field, string alias ) {
			Field = field;
			Alias = alias;
		}

		public string Field;
		public string Alias;
	}

	public sealed class Field {		
		ArrayList list = new ArrayList();

		public void Set( string field, string alias ) {
			list.Add( new FieldData( field, alias ) );
		}

		public void Set( string field ) {
			Set( field, null );
		}

		public void BulkSet( string field, params string[] fields ) {
			Set( field );

			foreach( string f in fields ) {
				Set( f );
			}
		}

		public FieldData[] Data {
			get{ return (FieldData[])list.ToArray( typeof( FieldData ) ); }
		}
	}		

	public sealed class TableData {
		public TableData( string table, string alias ) {
			Table = table;
			Alias = alias;
		}

		public string Table;
		public string Alias;
	}
	
	public sealed class Table {
		ArrayList list = new ArrayList();

		public void Set( string table, string alias ) {
			list.Add( new TableData( table, alias ) );
		}

		public void Set( string table ) {
			Set( table, null );
		}

		public TableData[] Data {
			get{ return (TableData[])list.ToArray( typeof( TableData ) ); }
		}
	}

	public sealed class RelationData {
		public RelationData( string table_left, Join join, string table_right, string on_fields ) {
			TableLeft = table_left; 
			Join = join; 
			TableRight = table_right;
			OnFields = on_fields;
		}

		public string TableLeft; 
		public Join Join; 
		public string TableRight;
		public string OnFields;
	}
	
	public sealed class Relation {
		ArrayList list = new ArrayList();

		public void Set( string table_left, Join join, string table_right, string on_fields ) {
			list.Add( new RelationData( table_left, join, table_right, on_fields ) );
		}

		public void Set( Join join, string table_right, string on_fields ) {
			Set( null, join, table_right, on_fields );
		}

		public RelationData[] Data {
			get{ return (RelationData[])list.ToArray( typeof( RelationData ) ); }
		}
	}

	public sealed class OrderByData {
		public OrderByData( string field_or_alias, bool descending ) {
			FieldOrAlias = field_or_alias;
			Descending = descending;
		}
		
		public string FieldOrAlias; 
		public bool Descending;
	}
	
	public sealed class OrderBy {
		ArrayList list = new ArrayList();

		public void Set( string field_or_alias, bool descending ) {
			list.Add( new OrderByData( field_or_alias, descending ) );
		}

		public void Set( string field_or_alias ) {
			Set( field_or_alias, false );
		}

		public OrderByData[] Data {
			get{ return (OrderByData[])list.ToArray( typeof( OrderByData ) ); }
		}
	}

	public sealed class GroupByData {
		public GroupByData( string field_or_alias) {
			FieldOrAlias = field_or_alias;
		}
		
		public string FieldOrAlias; 
	}
	
	public sealed class GroupBy {
		ArrayList list = new ArrayList();

		public void Set( string field_or_alias) {
			list.Add( new GroupByData( field_or_alias ) );
		}

		public GroupByData[] Data {
			get{ return (GroupByData[])list.ToArray( typeof( GroupByData ) ); }
		}
	}

	public sealed class WhereData {
		internal bool isBlock;

		public WhereData(Block block, Link link) {
			this.isBlock = true;
			this.Block = block;
			this.Link = link;
		}

		public WhereData( string field_or_alias, Filter filter, object beginValue, object endValue, Link link ) {
			FieldOrAlias = field_or_alias;
			Filter = filter; 
			BeginValue = beginValue; 
			EndValue = endValue; 
			Link = link;
			this.isBlock = false;
		}

		public string FieldOrAlias;
		public Filter Filter; 
		public object BeginValue; 
		public object EndValue; 
		public Link Link;
		public Block Block;
	}
	
	public sealed class Where {
		ArrayList list = new ArrayList();
		
		public WhereData Set( string field_or_alias, Filter filter, object beginValue, object endValue, Link link ) {
			WhereData where = new WhereData( field_or_alias, filter, beginValue, endValue, link );
			list.Add( where );
			return where;
		}

		public WhereData Set( string field_or_alias, Filter filter, object beginValue, object endValue ) {
			return Set( field_or_alias, filter, beginValue, endValue, Link.And );
		}

		public WhereData Set( string field_or_alias, Filter filter, object value ) {
			return Set( field_or_alias, filter, value, null, Link.And );
		}

		public WhereData Set( string field_or_alias, Filter filter, object value, Link link ) {
			return Set( field_or_alias, filter, value, null, link );
		}

		public WhereData[] Data {
			get{ return (WhereData[])list.ToArray( typeof( WhereData ) ); }
		}

		public void Set(Block block) {
			Set(block, Link.And);
		}

		public void Set(Block block, Link link) {
			WhereData where = new WhereData(block, link);
			list.Add( where );
		}
	}

	public sealed class SelectCommand {
		private Field _Field;
		private Table _Table;
		private Relation _Relation;
		private OrderBy _OrderBy;
		private GroupBy _GroupBy;
		private Where _Where;

		public SelectCommand() {
			_Field = new Field();
			_Table = new Table() ;
			_Relation = new Relation();
			_OrderBy = new OrderBy();
			_Where = new Where();
			_GroupBy = new GroupBy();
		}

		private string ResolveJoin( Join join ) {
			switch( join ) {
				case Join.Inner:
					return "INNER JOIN";
				case Join.Full:
					return "FULL JOIN";
				case Join.Cross:
					return "CROSS JOIN";
				case Join.Left:
					return "LEFT JOIN";
				case Join.Right:
					return "RIGHT JOIN";
				default:
					return null;
			}
		}

		private string ResolveLink( Link link ) {
			switch( link ) {
				case Link.And:
					return "AND";
				case Link.Or:
					return "OR";
				default:
					return null;
			}
		}

		private string ResolveFilter( string field, Filter filter, int idx ) {
#if SQL
			switch( filter ) {
				case Filter.Equal:
					return String.Format( "( {0} = {1}{2} )", field, "@P", idx );
				case Filter.NotEqual:
					return String.Format( "NOT( {0} = {1}{2} )", field, "@P", idx );
				case Filter.LessThan:
					return String.Format( "( {0} < {1}{2} )", field, "@P", idx );
				case Filter.GreaterThan:
					return String.Format( "( {0} > {1}{2} )", field, "@P", idx );					
				case Filter.LessOrEqual:
					return String.Format( "( {0} <= {1}{2} )", field, "@P", idx );					
				case Filter.GreaterOrEqual:
					return String.Format( "( {0} >= {1}{2} )", field, "@P", idx );					
				case Filter.Like:
					return String.Format( "( {0} LIKE {1}{2} )", field, "@P", idx );
				case Filter.NotLike:
					return String.Format( "NOT( {0} LIKE {1}{2} )", field, "@P", idx );					
				case Filter.Between:
					return String.Format( "( {0} BETWEEN {1}{2} AND {1}{2}_ )", field, "@P", idx );
				case Filter.NotBetween:
					return String.Format( "NOT( {0} BETWEEN {1}{2} AND {1}{2}_ )", field, "@P", idx );					
				case Filter.IsNull:
					return String.Format("( {0} is null )", field);
				case Filter.NotIsNull:
					return String.Format("( {0} is not null )", field);
				default:
					return null;
			}
#elif ORACLE
            switch (filter)
            {
                case Filter.Equal:
                    return String.Format("( {0} = {1}{2} )", field, ":P", idx);
                case Filter.NotEqual:
                    return String.Format("NOT( {0} = {1}{2} )", field, ":P", idx);
                case Filter.LessThan:
                    return String.Format("( {0} < {1}{2} )", field, ":P", idx);
                case Filter.GreaterThan:
                    return String.Format("( {0} > {1}{2} )", field, ":P", idx);
                case Filter.LessOrEqual:
                    return String.Format("( {0} <= {1}{2} )", field, ":P", idx);
                case Filter.GreaterOrEqual:
                    return String.Format("( {0} >= {1}{2} )", field, ":P", idx);
                case Filter.Like:
                    return String.Format("( {0} LIKE {1}{2} )", field, ":P", idx);
                case Filter.NotLike:
                    return String.Format("NOT( {0} LIKE {1}{2} )", field, ":P", idx);
                case Filter.Between:
                    return String.Format("( {0} BETWEEN {1}{2} AND {1}{2}_ )", field, ":P", idx);
                case Filter.NotBetween:
                    return String.Format("NOT( {0} BETWEEN {1}{2} AND {1}{2}_ )", field, ":P", idx);
                case Filter.IsNull:
                    return String.Format("( {0} is null )", field);
                case Filter.NotIsNull:
                    return String.Format("( {0} is not null )", field);
                default:
                    return null;
            }
#else
			switch( filter ) {
				case Filter.Equal:
					return String.Format( "( {0} = {1}{2} )", field, "@P", idx );
				case Filter.NotEqual:
					return String.Format( "NOT( {0} = {1}{2} )", field, "@P", idx );
				case Filter.LessThan:
					return String.Format( "( {0} < {1}{2} )", field, "@P", idx );
				case Filter.GreaterThan:
					return String.Format( "( {0} > {1}{2} )", field, "@P", idx );					
				case Filter.LessOrEqual:
					return String.Format( "( {0} <= {1}{2} )", field, "@P", idx );					
				case Filter.GreaterOrEqual:
					return String.Format( "( {0} >= {1}{2} )", field, "@P", idx );					
				case Filter.Like:
					return String.Format( "( {0} LIKE {1}{2} )", field, "@P", idx );
				case Filter.NotLike:
					return String.Format( "NOT( {0} LIKE {1}{2} )", field, "@P", idx );					
				case Filter.Between:
					return String.Format( "( {0} BETWEEN {1}{2} AND {1}{2}_ )", field, "@P", idx );
				case Filter.NotBetween:
					return String.Format( "NOT( {0} BETWEEN {1}{2} AND {1}{2}_ )", field, "@P", idx );					
				case Filter.IsNull:
					return String.Format("( {0} is null )", field);
				case Filter.NotIsNull:
					return String.Format("( {0} is not null )", field);
				default:
					return null;
			}
#endif
        }

		public override string ToString() {
			StringBuilder sb = new StringBuilder( "SET ROWCOUNT @P_ROWCOUNT; SELECT TOP 500 " );

			//FIELDS
			FieldData[] fieldData = Field.Data;
			for( int i = 0, l = fieldData.Length; i < l; i++ ) {
				FieldData fd = fieldData[ i ];
				
				if( fd.Alias != null )
					sb.AppendFormat( "{0} AS {1}", fd.Field, fd.Alias );				
				else
					sb.AppendFormat( "{0}", fd.Field );

				if( i < l - 1 )
					sb.Append( ", " );				
			}

			sb.Append( " FROM " );

			//TABLES
			TableData[] tableData = Table.Data;
			for( int i = 0, l = tableData.Length; i < l; i++ ) {
				TableData td = tableData[ i ];
				
				if( td.Alias != null )
					sb.AppendFormat( "{0} {1}", td.Table, td.Alias );				
				else
					sb.AppendFormat( "{0}", td.Table );

				if( i < l - 1 )
					sb.Append( ", " );				
			}

			//RELATIONS
			RelationData[] relationData = Relation.Data;

			if( relationData.Length > 0 && tableData.Length > 0 )
				sb.Append( ", " );

			for( int i = 0, l = relationData.Length; i < l; i++ ) {
				RelationData rd = relationData[ i ];
				
				if( rd.TableLeft != null )
					sb.AppendFormat( " {0} {3} {1} ON {2} ", rd.TableLeft, rd.TableRight, rd.OnFields, ResolveJoin( rd.Join ) );
				else
					sb.AppendFormat( " {2} {0} ON {1} ", rd.TableRight, rd.OnFields, ResolveJoin( rd.Join ) );				
			}

			//WHERE
			WhereData[] whereDataFull = Where.Data;
			ArrayList listOfWhereData = new ArrayList();
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

					listOfWhereData.Add(whereDataFull[i]);
				} else {
					whereValid = true;
					listOfWhereData.Add(whereDataFull[i]);
				}
			}

			//Verifica se deve montar a cláusula where (se existe algo além de blocos)
			if (whereValid) {
				//Obtem o whereData somente com os elementos válidos
				WhereData[] whereData = (WhereData[]) listOfWhereData.ToArray(typeof(WhereData));

				//Adiciona a clausa where
				sb.Append( " WHERE " );

				int parameterIndex = 0;

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
						} else { //Block.End
							sb.Append (" ) ");

							//Verifica se não é o último wheredata
							if( i < l - 1 ) {
								//Verifica se o próximo não é um endblock
								if (!(whereData[i + 1].isBlock && whereData[i + 1].Block == Block.End)) {
									sb.AppendFormat( " {0} ", ResolveLink( wd.Link ) );
								}
							}
						}
					} else {
						sb.AppendFormat( " {0} ", ResolveFilter( wd.FieldOrAlias, wd.Filter, parameterIndex++ )  );
				
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

			//GROUP BY
			GroupByData[] groupbyData = GroupBy.Data;
			//Verifica se existe GroupBy
			if ( groupbyData.Length > 0 ) 
				sb.Append( " GROUP BY " );

			for ( int i = 0, l = groupbyData.Length; i < l; i++ ) {
				GroupByData gd = groupbyData[ i ];

				sb.AppendFormat( " {0} ", gd.FieldOrAlias);

				if( i < l - 1 )
					sb.Append( ", " );
			}

			//ORDER BY
			OrderByData[] orderbyData = OrderBy.Data;

			if( orderbyData.Length > 0 )
				sb.Append( " ORDER BY " );

			for( int i = 0, l = orderbyData.Length; i < l; i++ ) {
				OrderByData od = orderbyData[ i ];

				sb.AppendFormat( " {0} {1} ", od.FieldOrAlias, od.Descending ? "DESC" : "" );

				if( i < l - 1 )
					sb.Append( ", " );
			}

			sb.Append( "; SET ROWCOUNT 0");

			string result = sb.ToString();

			//			//Verifica e retira os "AND ()" do where
			//			while (result.IndexOf("AND ()") > -1) {
			//				result = result.Replace("AND ()", "");
			//			}
			//
			//			//Verifica e retira os "OR ()"
			//			while (result.IndexOf("OR ()") > -1) {
			//				result = result.Replace("OR ()", "");
			//			}
			//
			//			//Verifica e retira os "() AND" do where
			//			while (result.IndexOf("() AND") > -1) {
			//				result = result.Replace("() AND", "");
			//			}
			//
			//			//Verifica e retira os "() OR"
			//			while (result.IndexOf("() OR") > -1) {
			//				result = result.Replace("() OR", "");
			//			}
			//			
			//			//Verifica e retira os "()" do where
			//			while (result.IndexOf("()") > -1) {
			//				result = result.Replace("()", "");
			//			}

			return result;
		}

		public Field Field {
			get{ return _Field; }
		}
		

		public Table Table {
			get{ return _Table; }
		}

		public Relation Relation {
			get{ return _Relation; }
		}

		public OrderBy OrderBy {
			get{ return _OrderBy; }
		}

		public GroupBy GroupBy {
			get{ return _GroupBy; }
		}
		
		public Where Where {
			get{ return _Where; }
		}
	}

	public class Parameter {
		System.Collections.ArrayList _List = new System.Collections.ArrayList( 10 );
		int _Index = 0;

#if SQL
        public System.Data.SqlClient.SqlParameter[] Parameters
#elif ORACLE
        public System.Data.OracleClient.OracleParameter[] Parameters
#else
        public System.Data.SqlClient.SqlParameter[] Parameters
#endif
        {
			get {
				if( _List.Count < 1 )
					return null;
#if SQL
                return (System.Data.SqlClient.SqlParameter[])_List.ToArray(typeof(System.Data.SqlClient.SqlParameter));
#elif ORACLE
                return (System.Data.OracleClient.OracleParameter[])_List.ToArray(typeof(System.Data.OracleClient.OracleParameter));
#else
                return (System.Data.SqlClient.SqlParameter[])_List.ToArray(typeof(System.Data.SqlClient.SqlParameter));
#endif
            }
		}

#if SQL
        public void AddParameter(System.Data.SqlDbType dbType, WhereData wd)
#elif ORACLE
        public void AddParameter(System.Data.OracleClient.OracleType dbType, WhereData wd)
#else
        public void AddParameter(System.Data.SqlDbType dbType, WhereData wd)
#endif
        {
			this.AddParameter( dbType, 0, wd );
		}

#if SQL
        public void AddParameter(System.Data.SqlDbType dbType, int size, WhereData wd)
#elif ORACLE
        public void AddParameter(System.Data.OracleClient.OracleType dbType, int size, WhereData wd)
#else
        public void AddParameter(System.Data.SqlDbType dbType, int size, WhereData wd)
#endif
        {			
			bool fBetween = Filter.Between == wd.Filter || Filter.NotBetween == wd.Filter;
				
            System.Data.IDbDataParameter parameter;

			if( size > 0 )
#if SQL
                parameter = new System.Data.SqlClient.SqlParameter( String.Format( "@P{0}", _Index ), dbType, size ) ;
#elif ORACLE
                parameter = new System.Data.OracleClient.OracleParameter(String.Format(":P{0}", _Index), dbType, size);
#else
                parameter = new System.Data.SqlClient.SqlParameter(String.Format("@P{0}", _Index), dbType, size);
#endif
			else
#if SQL
				parameter = new System.Data.SqlClient.SqlParameter( String.Format( "@P{0}", _Index ), dbType ) ;
#elif ORACLE
                parameter = new System.Data.OracleClient.OracleParameter(String.Format(":P{0}", _Index), dbType);
#else
                parameter = new System.Data.SqlClient.SqlParameter(String.Format("@P{0}", _Index), dbType);
#endif

			parameter.Value = wd.BeginValue;

			_List.Add( parameter );

			if( fBetween ) {
                System.Data.IDbDataParameter parameter2;
				
				if( size > 0 )
#if SQL
                    parameter2 = new System.Data.SqlClient.SqlParameter(String.Format("@P{0}_", _Index), dbType, size);
#elif ORACLE
                    parameter2 = new System.Data.OracleClient.OracleParameter(String.Format(":P{0}_", _Index), dbType, size);
#else
                    parameter2 = new System.Data.SqlClient.SqlParameter(String.Format("@P{0}_", _Index), dbType, size);
#endif
				else
#if SQL
                    parameter2 = new System.Data.SqlClient.SqlParameter(String.Format("@P{0}_", _Index), dbType);
#elif ORACLE
                    parameter2 = new System.Data.OracleClient.OracleParameter(String.Format(":P{0}_", _Index), dbType);
#else
                    parameter2 = new System.Data.SqlClient.SqlParameter(String.Format("@P{0}_", _Index), dbType);
#endif

                    parameter2.Value = wd.EndValue;

				_List.Add( parameter2 );
			}
			
//			if(wd.Filter != Filter.IsNull)
//			{
				_Index++;
//			}			
		}
	}
}

