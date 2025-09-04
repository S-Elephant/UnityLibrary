#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// <see cref="IEnumerable{T}"/> extensions.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Thread-local random number generator.
		/// Each thread gets its own instance to avoid contention and improve performance.
		/// </summary>
		private static readonly ThreadLocal<Random> _threadRandom = new(() => new Random());

		/// <summary>
		/// Return random element(s).
		/// </summary>
		public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> source, int count)
		{
			return source.Shuffle().Take(count);
		}

		/// <summary>
		/// Return random element.
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if <paramref name="source"/> is empty.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
		public static T GetRandom<T>(this IEnumerable<T> source)
		{
			return source.Shuffle().Take(1).Single();
		}

		/// <summary>
		/// Shuffle the elements. Modifies the <paramref name="source"/>.
		/// </summary>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.OrderBy(_ => _threadRandom.Value.Next());
		}

		/// <summary>
		/// Shuffle the elements using a seed. Modifies the <paramref name="source"/>.
		/// </summary>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, int seed)
		{
			Random random = new(seed);
			return source.OrderBy(_ => random.Next());
		}

		/// <summary>
		/// Determines if the <paramref name="source"/> is empty.
		/// </summary>
		/// <returns>True if empty.</returns>
		public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
		{
			return !source.Any();
		}

		/// <summary>
		/// Return true if ALL of <paramref name="values"/> are contained in <paramref name="source"/>.
		/// Returns true if <paramref name="values"/> is empty.
		/// </summary>
		public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> values)
		{
			return values.All(value => source.Contains(value));
		}

		/// <summary>
		/// Return true if NONE of <paramref name="values"/> are contained in <paramref name="source"/>.
		/// Returns true if either <paramref name="source"/> or <paramref name="values"/> is empty.
		/// </summary>
		public static bool ContainsNone<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> values)
		{
			TSource[] sourceAsArray = source.ToArray();
			TSource[] valuesAsArray = values.ToArray();

			if (!sourceAsArray.Any() || !valuesAsArray.Any())
				return true;

			return !sourceAsArray.Intersect(valuesAsArray).Any(); // Note: LINQ Intersect returns the common elements from both collections.
		}

		/// <summary>
		/// Determines if the <paramref name="source"/> is empty.
		/// </summary>
		/// <returns>True if empty.</returns>
		public static bool None<TSource>(this IEnumerable<TSource> source)
		{
			return !source.Any();
		}

		/// <summary>
		/// Determines whether any element of a sequence does not satisfy a condition. This is the same as !source.Any(..).
		/// </summary>
		/// <typeparam name="TSource">An <see cref="IEnumerable{TSource}"/> whose elements to apply the predicate to.</typeparam>
		/// <param name="source">Source.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>True if the source sequence is empty or none of its elements passes the test in the specified predicate; otherwise, false.</returns>
		public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			return !source.Any(predicate);
		}

		/// <summary>
		/// Return true if the first item of <paramref name="source"/> equals <paramref name="itemToCompare"/>.
		/// </summary>
		/// <typeparam name="TSource">An <see cref="IEnumerable{TSource}"/> whose elements to apply the predicate to.</typeparam>
		/// <param name="source">Source.</param>
		/// <param name="itemToCompare">Item to compare with the last item.</param>
		/// <returns>True if the source sequence is empty or none of its elements passes the test in the specified predicate; otherwise, false.</returns>
		public static bool IsFirst<TSource>(this IEnumerable<TSource> source, TSource itemToCompare)
		{
			return EqualityComparer<TSource>.Default.Equals(source.First(), itemToCompare);
		}

		/// <summary>
		/// Return true if the last item of <paramref name="source"/> equals <paramref name="itemToCompare"/>.
		/// </summary>
		/// <typeparam name="TSource">An <see cref="IEnumerable{TSource}"/> whose elements to apply the predicate to.</typeparam>
		/// <param name="source">Source.</param>
		/// <param name="itemToCompare">Item to compare with the last item.</param>
		/// <returns>True if the source sequence is empty or none of its elements passes the test in the specified predicate; otherwise, false.</returns>
		public static bool IsLast<TSource>(this IEnumerable<TSource> source, TSource itemToCompare)
		{
			return EqualityComparer<TSource>.Default.Equals(source.Last(), itemToCompare);
		}

		/// <summary>
		/// Return true if EVERY item in <paramref name="source"/> is unique or if it is empty or null.
		/// If every item must always be unique then consider using a <see cref="HashSet{T}"/>.
		/// </summary>
		/// <typeparam name="TSource">IEnumerable type.</typeparam>
		/// <param name="source">Source list.</param>
		/// <returns>True if EVERY item in <paramref name="source"/> is unique or if it is empty or null.</returns>
		public static bool AreAllItemsUnique<TSource>(this IEnumerable<TSource>? source)
		{
			if (source == null)
				return true;

			TSource[] sourceAsArray = source.ToArray();

			return sourceAsArray.Distinct().Count() == sourceAsArray.Length;
		}
	}
}