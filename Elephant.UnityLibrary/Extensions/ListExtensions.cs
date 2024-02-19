#nullable enable
using System.Collections.Generic;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// <see cref="IList{T}"/> extensions.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Shuffle the <paramref name="list"/>. Modifies the <paramref name="list"/>
		/// </summary>
		public static void Shuffle<T>(this IList<T> list)
		{
			int cnt = list.Count;
			while (cnt > 1)
			{
				cnt--;
				int k = UnityEngine.Random.Range(0, cnt - 1);
				T value = list[k];
				list[k] = list[cnt];
				list[cnt] = value;
			}
		}

		/// <summary>
		/// Add <paramref name="itemToAdd"/> only if it doesn't already exist in <paramref name="list"/>.
		/// </summary>
		public static void AddIfNotExists<T>(this List<T> list, T itemToAdd)
		{
			if (!list.Contains(itemToAdd))
				list.Add(itemToAdd);
		}

		/// <summary>
		/// Add the item to the <paramref name="list"/> unless it already exists in that list in which case it will remove it instead.
		/// </summary>
		/// <returns>Altered <paramref name="list"/>.</returns>
		public static IList<TSource> AddOrRemoveIfExists<TSource>(this IList<TSource> list, TSource item)
		{
			if (list.Contains(item))
				list.Remove(item);
			else
				list.Add(item);

			return list;
		}

		/// <summary>
		/// Add the item to the <paramref name="list"/> unless it already exists in that list in which case it will remove it instead.
		/// If <paramref name="list"/> is null then nothing happens.
		/// </summary>
		/// <returns>Altered <paramref name="list"/>.</returns>
		public static IList<TSource>? AddOrRemoveIfExistsNullable<TSource>(this IList<TSource>? list, TSource item)
		{
			if (list == null)
				return null;

			if (list.Contains(item))
				list.Remove(item);
			else
				list.Add(item);

			return list;
		}
	}
}