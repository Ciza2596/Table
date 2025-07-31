using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	[Serializable]
	public abstract class Array2D
	{
		[SerializeField]
		protected Vector2Int _size;
		public virtual Vector2Int Size => _size;
		public virtual bool IsReadOnly { get; set; }
		public abstract int Length { get; }
		
		public abstract void Resize(Vector2Int newSize);
		public virtual void Resize(int x, int y) => Resize(new Vector2Int(x, y));
	}

	[Serializable]
	public class Array2D<T> : Array2D
	{
		[SerializeField, HideInInspector]
		protected T[] _data;
		public override int Length => _data.Length;

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		public Array2D(int x, int y) : this(new Vector2Int(x, y)) { }

		[Preserve]
		public Array2D(Vector2Int size)
		{
			_size = size;
			_data = new T[size.x * size.y];
		}

		[Preserve]
		public Array2D(T[,] matrix)
		{
			_size = new Vector2Int(matrix.GetLength(1), matrix.GetLength(0));
			_data = matrix.Cast<T>().ToArray();
		}

		[Preserve]
		public Array2D(T[] array, Vector2Int size)
		{
			_size = size;
			_data = array;
		}

		public virtual T this[int x, int y]
		{
			get => _data[(_size.x * y) + x];
			set => _data[(_size.x * y) + x] = value;
		}

		public virtual T this[Vector2Int pos]
		{
			get => this[pos.x, pos.y];
			set => this[pos.x, pos.y] = value;
		}

		public virtual Array2D<TCast> Cast<TCast>() => new(_data.Cast<TCast>().ToArray(), _size);

		public override void Resize(Vector2Int newSize)
		{
			if (IsReadOnly || (newSize.x == _size.x && newSize.y == _size.y))
				return;

			var newData = new T[newSize.x * newSize.y];

			if (_data?.Length > 0)
			{
				var newWidth = Mathf.Min(Size.x, newSize.x);
				var newHeight = Mathf.Min(Size.y, newSize.y);

				for (int y = 0; y < newHeight; y++)
				{
					for (int x = 0; x < newWidth; x++)
					{
						var oldIndex = y * _size.x + x;
						var newIndex = y * newSize.x + x;
						newData[newIndex] = _data[oldIndex];
					}
				}
			}
			
			_data = newData;
			_size = newSize;
		}

		public virtual void AddRow(T[] x)
		{
			if (IsReadOnly || x == null)
				return;

			var arrayLength = Length;
			Resize(_size.x, _size.y + 1);

			for (int i = 0; i < x.Length; i++)
				_data[arrayLength + i] = x[i];
		}

		public virtual void AddColumn(T[] y)
		{
			if (IsReadOnly || y == null)
				return;

			Resize(_size.x + 1, _size.y);

			for (int i = 0; i < y.Length; i++)
				_data[_size.x * (i + 1)] = y[i];
		}
		
		public virtual T[] ToArray() => _data;
		
		public virtual T[,] ToMatrix()
		{
			var matrix = new T[_size.x, _size.y];
			
			for (int i = 0; i < _data.Length; i++)
				matrix[i % _size.x, i / _size.x] = _data[i];

			return matrix;
		}
	}
}