using System;
using System.Collections;

namespace SuperPag.Framework.Helper
{
	public class ArrayHelper 
	{
		//não retirar o atributo
		[Obsolete("Função será removida")]
		public static System.Array Concat( Array old_array, object new_value ) 
		{
			return Concat( old_array, new object[]{ new_value } ); 
		}

//		//não retirar o atributo
//		[Obsolete("não ficar redimensionando o array!!! - Crie o ArrayList, preencha e depois no final converta.")]
//		public static System.Array Concat( int[] old_array, int new_value ) {
//			object[] arrayOfOld = (object[])new ArrayList(old_array).ToArray(typeof(object));
//			return Concat( arrayOfOld, new object[]{ new_value } );
//		}

		/// <summary>
		/// Concatena 2 arrays. Não deve ser utilizado em loops.
		/// </summary>
		/// <param name="old_array">Array 1</param>
		/// <param name="new_values_array">Array 2</param>
		/// <returns>Array contendo o conteúdo dos arrays 1 e 2</returns>
		[Obsolete("Função será removida")]
		public static System.Array Concat( Array old_array, Array new_values_array ) 
		{
			if (old_array == null && new_values_array == null) 
			{
				return null;
			}
			else if (old_array == null) 
			{
				return new_values_array;
			} 
			else if (new_values_array == null) 
			{
				return old_array;
			} 
			else 
			{
				ArrayList _list = new ArrayList();
				if (old_array.Length > 0) _list.AddRange(old_array);
				if (new_values_array.Length > 0) _list.AddRange(new_values_array);

				return _list.ToArray(old_array.GetType().GetElementType());
			}
		}
	}
}