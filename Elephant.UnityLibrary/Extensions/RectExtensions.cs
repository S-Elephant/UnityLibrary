#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// <see cref="Rect"/> extension(s).
	/// </summary>
	public static class RectExtensions
	{
		/// <summary>
		/// Calculates the combined Axis-Aligned Bounding Box (AABB) from a collection
		/// of Rect objects. This method determines the minimum and maximum extents of
		/// the collection and returns a Rect that encompasses all the Rect objects within
		/// it. If the collection is null or empty, a Rect with zero position and size is
		/// returned.
		/// </summary>
		/// <param name="rects">Collection of Rect objects to combine. Can be null.</param>
		/// <returns><see cref="Rect"/> representing the combined AABB of the input
		/// Rect objects. If input is null then <see cref="Rect.zero"/> is returned.</returns>
		public static Rect Combine(this IEnumerable<Rect>? rects)
		{
			if (rects == null)
				return Rect.zero;

			Rect[] rectsAsArray = rects.ToArray();
			if (!rectsAsArray.Any())
				return Rect.zero;

			float minX = float.MaxValue, minY = float.MaxValue;
			float maxX = float.MinValue, maxY = float.MinValue;

			foreach (Rect rect in rectsAsArray)
			{
				minX = Mathf.Min(minX, rect.xMin);
				minY = Mathf.Min(minY, rect.yMin);
				maxX = Mathf.Max(maxX, rect.xMax);
				maxY = Mathf.Max(maxY, rect.yMax);
			}

			return new Rect(minX, minY, maxX - minX, maxY - minY);
		}
	}
}
