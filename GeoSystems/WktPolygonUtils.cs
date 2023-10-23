using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// WKT (=Well Known Text) polygon utilities.
	/// </summary>
	public static class WktPolygonUtils
	{
		/// <summary>
		/// Calculate bounds of a multi-polygon its points.
		/// </summary>
		/// <param name="points">List representing multi-polygons, where each multi-polygon is a list of polygons and each polygon consists of an exterior ring and zero or more interior rings (holes).</param>
		/// <returns>Minimum bounds and maximum bounds.</returns>
		public static (Vector2 MinBounds, Vector2 MaxBounds) PointsToBounds(List<List<List<Vector2>>> points)
		{
			if (!points.Any())
			{
				Debug.LogError("Polygon is empty. Returning zero's instead");
				return (Vector2.zero, Vector2.zero);
			}

			// Initialize AABB boundaries to extreme values.
			Vector2 minBounds = new(float.MaxValue, float.MaxValue);
			Vector2 maxBounds = new(float.MinValue, float.MinValue);

			foreach (var polygon in points)
			{
				foreach (var exteriorRing in polygon)
				{
					foreach (var point in exteriorRing)
					{
						// Update AABB boundaries using the polygon points.
						minBounds.x = Mathf.Min(minBounds.x, point.x);
						minBounds.y = Mathf.Min(minBounds.y, point.y);
						maxBounds.x = Mathf.Max(maxBounds.x, point.x);
						maxBounds.y = Mathf.Max(maxBounds.y, point.y);
					}
				}
			}

			return (minBounds, maxBounds);
		}

		/// <summary>
		/// Calculate center vector from min- and max bounds.
		/// </summary>
		/// <param name="minBounds">The lowest <see cref="Vector2"/> corner of the AABB.</param>
		/// <param name="maxBounds">The highest <see cref="Vector2"/> corner of the AABB.</param>
		/// <returns>Center position of the AABB that is formed by the two <paramref name="minBounds"/> and <paramref name="maxBounds"/>.</returns>
		public static Vector2 CenterFromBounds(Vector2 minBounds, Vector2 maxBounds)
		{
			return (minBounds + maxBounds) / 2f;
		}
	}
}
