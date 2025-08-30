using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace CizaTable.Editor
{
	public static class TypeUtils
	{
		public enum SortKinds
		{
			ByType = 0,
			ByTitle = 1,
			ByCategory = 2
		}

		private static readonly Comparison<Type>[] COMPARISONS = { CompareByType, CompareByTitle, CompareByCategory };

		private static readonly char[] SEPARATOR = { '.' };

		// PUBLIC METHOD: ----------------------------------------------------------------------


		#region Type Name

		public static string GetName(Type type) =>
			GetName(type.ToString());

		public static string GetName(string type)
		{
			var split = type.Split(SEPARATOR);
			return split.Length > 0 ? TextUtils.Humanize(split[^1]) : string.Empty;
		}

		#endregion

		#region Get Title

		public static string GetTitle(Type type, string[] forbiddenNames = null)
		{
			if (type == null)
				return "(none)";
			var title = type.GetCustomAttributes<TitleAttribute>().FirstOrDefault();
			var titleName = title != null && !string.IsNullOrEmpty(title.Title) ? title.Title : GetName(type);

			if (forbiddenNames == null) return titleName;
			if (string.IsNullOrEmpty(titleName)) return titleName;

			var number = 1;
			var complete = titleName;

			while (forbiddenNames.Contains(complete))
			{
				complete = $"{titleName} ({number})";
				number += 1;
			}

			return complete;
		}

		#endregion

		#region Check

		public static bool CheckIsListImp(Type type) =>
			typeof(IList).IsAssignableFrom(type);

		public static bool CheckIsUnityObjSubclass(Type type) =>
			type.IsSubclassOf(typeof(Object));

		public static bool CheckIsString(Type type) =>
			type == typeof(string);

		public static bool CheckIsClassWithoutStringOrUnityObjSubclass(Type type) =>
			type.IsClass && !CheckIsString(type) && !CheckIsUnityObjSubclass(type);

		#endregion

		public static object CreateInstance(Type type, params object[] args)
		{
			if (CheckIsUnityObjSubclass(type))
				return null;

			if (CheckIsString(type))
				return string.Empty;

			if (type.IsArray)
				return Array.CreateInstance(GetElementTypes(type)[0], (args.Length == 1 && args[0] is int length) ? length : 0);

			if (CheckIsListImp(type))
			{
				var listType = typeof(List<>).MakeGenericType(GetElementTypes(type)[0]);
				return (IList)Activator.CreateInstance(listType);
			}

			if (!type.IsValueType && (type.IsAbstract || type.IsInterface || type.GetConstructor(Type.EmptyTypes) == null))
				throw new InvalidOperationException($"Type {type.Name} cant created by activator,");
			return Activator.CreateInstance(type, args);
		}

		public static Type[] GetElementTypes(Type type)
		{
			var types = GetSelfAndBaseTypes(type);
			var allGenericTypes = new List<Type>();
			foreach (var childType in types)
			{
				if (childType.IsArray)
					allGenericTypes.Add(childType.GetElementType());
				else
				{
					var genericTypes = childType.GetGenericArguments();
					if (genericTypes.Length > 0)
						allGenericTypes.AddRange(genericTypes);
				}
			}

			return allGenericTypes.ToArray();
		}

		#region BaseTypes

		public static Type[] GetSelfAndBaseTypes(Type type) =>
			GetBaseAndSelfTypes(type, true);

		public static Type[] GetBaseAndSelfTypes(Type type) =>
			GetBaseAndSelfTypes(type, false);

		private static Type[] GetBaseAndSelfTypes(Type type, bool isReverse)
		{
			var types = new List<Type>();
			types.AddRange(GetBaseTypes(type, true));
			types.Add(type);
			if (isReverse)
				types.Reverse();
			return types.ToArray();
		}

		public static Type[] GetBaseTypes(Type type, bool isReverse = false)
		{
			var types = new List<Type>();
			var baseType = type.BaseType;
			while (baseType != null)
			{
				types.Add(baseType);
				baseType = baseType.BaseType;
			}

			if (isReverse)
				types.Reverse();
			return types.ToArray();
		}

		#endregion

		#region RelativeTypes

		public static Type[] GetSortedRelativeTypes(Type type, SortKinds sortKind = SortKinds.ByTitle)
		{
			var types = GetRelativeTypes(type);
			Array.Sort(types, COMPARISONS[(int)sortKind]);
			return types;
		}

		public static Type[] GetRelativeTypes(Type type, bool isIncludeAbstract = false)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var types = new List<Type>();
			foreach (var assembly in assemblies)
				foreach (var assemblyType in assembly.GetTypes())
				{
					if (!isIncludeAbstract && (assemblyType.IsAbstract || assemblyType.IsInterface)) continue;
					if (type.IsAssignableFrom(assemblyType)) types.Add(assemblyType);
				}

			return types.ToArray();
		}

		#endregion

		public static Trie<Type> GetTypesTree(Type baseType)
		{
			var trie = Trie<Type>.Create();
			foreach (var type in GetRelativeTypes(baseType))
			{
				var category = type.GetCustomAttributes<CategoryAttribute>(true).FirstOrDefault();
				var paths = category?.Path ?? Array.Empty<string>();
				var name = category != null && string.IsNullOrEmpty(category.Name) ? category.Name : TextUtils.Humanize(type.ToString());

				var subTrie = trie;
				foreach (var section in paths)
				{
					if (!subTrie.Children.TryGetValue(section, out Trie<Type> child))
						child = subTrie.AddChild(new Trie<Type>(section, null));
					subTrie = child;
				}

				subTrie.AddChild(new Trie<Type>(name, type));
			}

			return trie;
		}

		// PRIVATE METHOD: --------------------------------------------------------------------- 

		private static int CompareByType(Type a, Type b) =>
			string.CompareOrdinal(a.ToString(), b.ToString());

		private static int CompareByTitle(Type a, Type b)
		{
			var attrA = a.GetCustomAttributes<TitleAttribute>(true).FirstOrDefault();
			var attrB = b.GetCustomAttributes<TitleAttribute>(true).FirstOrDefault();

			var orderComparison = Nullable.Compare(attrA?.Order, attrB?.Order);
			if (orderComparison != 0)
				return orderComparison;

			return string.CompareOrdinal(attrA?.Title, attrB?.Title);
		}

		private static int CompareByCategory(Type a, Type b)
		{
			var attrA = a.GetCustomAttributes<CategoryAttribute>(true).FirstOrDefault();
			var attrB = b.GetCustomAttributes<CategoryAttribute>(true).FirstOrDefault();

			var orderComparison = Nullable.Compare(attrA?.Order, attrB?.Order);
			if (orderComparison != 0)
				return orderComparison;

			return string.CompareOrdinal(attrA?.ToString(), attrB?.ToString());
		}
	}
}