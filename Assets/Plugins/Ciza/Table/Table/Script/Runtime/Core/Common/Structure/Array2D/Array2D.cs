using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	[Serializable]
	public abstract class Array2D
	{
		// PUBLIC VARIABLE: ---------------------------------------------------------------------

		public abstract Vector2Int Size { get; }
		public abstract bool IsReadOnly { get; }
		public abstract int Length { get; }

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		protected Array2D() { }

		// PUBLIC METHOD: ----------------------------------------------------------------------

		public virtual void Resize(int x, int y) => Resize(new Vector2Int(x, y));
		public abstract void Resize(Vector2Int newSize);

		public abstract void SetIsReadonly(bool isReadOnly);
	}

	[Serializable]
	public class Array2D<T> : Array2D
	{
		// VARIABLE: -----------------------------------------------------------------------------

		[SerializeField]
		protected Vector2Int _size;

		[SerializeField]
		protected bool _isReadOnly;

		[SerializeField, HideInInspector]
		protected T[] _data;


		// PUBLIC VARIABLE: ---------------------------------------------------------------------

		public override Vector2Int Size => _size;
		public override bool IsReadOnly => _isReadOnly;
		public override int Length => _data.Length;

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		public Array2D(int x, int y) : this(new Vector2Int(x, y), false) { }

		[Preserve]
		public Array2D(int x, int y, bool isReadOnly) : this(new Vector2Int(x, y), isReadOnly) { }

		[Preserve]
		public Array2D(Vector2Int size) : this(size, false) { }

		[Preserve]
		public Array2D(Vector2Int size, bool isReadOnly)
		{
			_size = size;
			_isReadOnly = isReadOnly;
			_data = new T[size.x * size.y];
		}

		[Preserve]
		public Array2D(T[,] matrix) : this(false, matrix) { }

		[Preserve]
		public Array2D(bool isReadOnly, T[,] matrix)
		{
			_size = new Vector2Int(matrix.GetLength(1), matrix.GetLength(0));
			_isReadOnly = isReadOnly;
			_data = matrix.Cast<T>().ToArray();
		}

		[Preserve]
		public Array2D() : this(Vector2Int.zero, Array.Empty<T>()) { }

		[Preserve]
		public Array2D(Vector2Int size, T[] array) : this(size, false, array) { }

		[Preserve]
		public Array2D(Vector2Int size, bool isReadOnly, T[] array)
		{
			_size = size;
			_isReadOnly = isReadOnly;
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

		// PUBLIC METHOD: ----------------------------------------------------------------------

		public virtual Array2D<TCast> Cast<TCast>() => new Array2D<TCast>(_size, _isReadOnly, _data.Cast<TCast>().ToArray());

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

		public override void SetIsReadonly(bool isReadOnly) =>
			_isReadOnly = isReadOnly;

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