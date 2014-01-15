using System;

namespace SuperPag.Framework.Data.Components.Data.Objects.Helpers
{
	using System;
	using System.Collections;
	
	internal class Pager
	{
		public const int MAXPAGESIZE = 2000;
		
		/* true se tiver mais páginas */
		/* false se eof */
		public static bool GetPage( ref object[] objset, int size, int pagesize )
		{
			object[] _objset = objset;
		
			if( _objset == null || _objset.Length < 1 || _objset.Length < size - pagesize )
			{
				objset = null;
				
				return false;
			}					
			
			int rowcount = _objset.Length;

			if( rowcount > pagesize )
			{
				object[] newobjset = new object[ pagesize ];
		
				for( int idx = size - pagesize, len = size, objidx = 0; idx < len; ++idx, ++objidx ) 
					newobjset[ objidx ] = _objset[ idx ];

				objset = newobjset;

				return true; //mais registros
			}
			else
			{
				object[] newobjset = new object[ rowcount ];

				for( int idx = size - pagesize, len = rowcount, objidx = 0; idx < len; ++idx, ++objidx ) 
					newobjset[ objidx ] = _objset[ idx ];

				objset = newobjset;

				return false; //EOF
			}			
		}

		private static int InternalRelativePage( int rowcount, int pagesize, bool overflow )
		{
			int _rowcount = rowcount - ( overflow ? 1 : 0 );
			int resultado = _rowcount / pagesize;
			int resto = _rowcount % pagesize;

			if( resto > 0 )
				resultado++;

			return resultado;				
		}

		/* return true == overflow = existe mais dados no banco */
		/* return false == não existe mais dados no banco, vc esta na ultima pagina realmente */
		public static bool GetLastPage( ref object[] objset, out int relative_page, int pagesize )
		{
			object[] _objset = objset;

			if( _objset == null || _objset.Length < 1 )
			{
				objset = null;
				relative_page = 0;
				
				return false;
			}		
		
			int rowcount = _objset.Length;

			bool _overflow = false;
		
			if( rowcount > MAXPAGESIZE )
			{
				_overflow = true;
			}

			if( rowcount > pagesize )
			{
				relative_page = InternalRelativePage( rowcount, pagesize, _overflow );

				int new_pagesize = 0;
			
				if( _overflow )
					new_pagesize = pagesize;
				else
					new_pagesize = rowcount - ( ( relative_page - 1 ) * pagesize );
			
				object[] newobjset = new object[ new_pagesize ];

				for( int idx = ( rowcount - new_pagesize - ( _overflow ? 1 : 0 ) ), objidx = 0; objidx < new_pagesize; ++idx, ++objidx ) 
					newobjset[ objidx ] = _objset[ idx ];

				objset = newobjset;				

				return _overflow; 				
			}
			else
			{
				objset = _objset;

				relative_page = 1;								
			}

			return _overflow;
		}

		public static int CheckInternalPage( int variable, string variableName, /*string sqlStatement,*/ int size )
		{
			if( variable < 0 )
			{
				throw new ArgumentOutOfRangeException( variableName, variable, "Cannot be lesser than the 1" );
			}

			if( variable > MAXPAGESIZE )
			{	
				throw new ArgumentOutOfRangeException( variableName, variable, String.Format( "Cannot be greater than the maxsize = {0}", MAXPAGESIZE ) );				
			}

			return size + 1;			
		}
	}
}
