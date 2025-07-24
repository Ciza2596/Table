using UnityEngine;

namespace CizaTable
{
	public static class ArrayExtension
	{
		public static Array2D<T> ToArray2D<T>(this T[,] array) => 
			new Array2D<T>(array);
		
		public static Array2D<T> ToArray2D<T>(this T[] array, Vector2Int size) =>
			new Array2D<T>(array, size);
	}
}