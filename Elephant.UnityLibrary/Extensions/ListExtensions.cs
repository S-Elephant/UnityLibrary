﻿#nullable enable
using System;
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
		[Obsolete("Use AddUnique() instead.")]
		public static void AddIfNotExists<T>(this List<T> list, T itemToAdd)
		{
			if (!list.Contains(itemToAdd))
				list.Add(itemToAdd);
		}

		/// <summary>
		/// Add <paramref name="itemToAdd"/> only if it doesn't already exist in <paramref name="list"/>.
		/// </summary>
		public static void AddUnique<T>(this List<T> list, T itemToAdd)
		{
			if (!list.Contains(itemToAdd))
				list.Add(itemToAdd);
		}

		/// <summary>
		/// Add a range of elements to the list if they don't already exist, using a <see cref="HashSet{T}"/> for efficiency.
		/// </summary>
		/// <typeparam name="T">Type of elements in the list.</typeparam>
		/// <param name="list">List to which elements are added.</param>
		/// <param name="items">Collection of items to be added.</param>
		public static void AddRangeUnique<T>(this List<T> list, IEnumerable<T> items)
		{
			HashSet<T> existingItems = new(list); // Use HashSet for O(1) lookup.

			foreach (T item in items)
			{
				if (existingItems.Add(item))
					list.Add(item);
			}
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