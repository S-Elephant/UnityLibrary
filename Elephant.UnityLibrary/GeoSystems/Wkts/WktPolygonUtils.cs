﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems.Wkts
{
	/// <summary>
	/// WKT (=Well Known Text) polygon utilities.
	/// Note that WKT exterior rings MUST be counter-clockwise and interior rings must be clockwise.
	/// For more info see: https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry
	/// </summary>
	public static class WktPolygonUtils
	{
		/// <summary>
		/// Calculate bounds of a multi-polygon its points.
		/// </summary>
		/// <param name="geometry">List representing multi-polygons, where each multi-polygon is a list of polygons and each polygon consists of an exterior ring and zero or more interior rings (holes).</param>
		/// <returns>Minimum bounds and maximum bounds.</returns>
		public static (Vector2 MinBounds, Vector2 MaxBounds) PointsToBounds(List<List<List<Vector2>>> geometry)
		{
			if (!geometry.Any())
			{
				Debug.LogError("Polygon is empty. Returning zero's instead");
				return (Vector2.zero, Vector2.zero);
			}

			// Initialize AABB boundaries to extreme values.
			Vector2 minBounds = new(float.MaxValue, float.MaxValue);
			Vector2 maxBounds = new(float.MinValue, float.MinValue);

			foreach (List<List<Vector2>> polygon in geometry)
			{
				foreach (List<Vector2> exteriorRing in polygon)
				{
					foreach (Vector2 point in exteriorRing)
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

		/// <summary>
		/// Rotates a complex geometry consisting of multiple polygons.
		/// </summary>
		/// <param name="geometry">Complex geometry to rotate, represented as a nested list of Vector2 points.</param>
		/// <param name="degrees">Angle in degrees to rotate the geometry. Positive for counterclockwise rotation, negative for clockwise.</param>
		/// <param name="origin">Point of origin around which the geometry is rotated.</param>
		/// <returns>New geometry data which is the rotated version of the original.</returns>
		public static List<List<List<Vector2>>> RotatePoints(List<List<List<Vector2>>> geometry, float degrees, Vector2 origin)
		{
			return RotateGeometry(geometry, degrees, origin);
		}

		/// <summary>
		/// Rotates a geometry represented by a WKT (Well-Known Text) string.
		/// </summary>
		/// <param name="wktString">WKT string representing the geometry to rotate.</param>
		/// <param name="degrees">Angle in degrees to rotate the geometry. Positive for counterclockwise rotation, negative for clockwise.</param>
		/// <param name="origin">Point of origin around which the geometry is rotated.</param>
		/// <returns>New geometry data which is the rotated version of the original.</returns>
		public static List<List<List<Vector2>>> RotateWktStringAsPoints(string wktString, float degrees, Vector2 origin)
		{
			List<List<List<Vector2>>> geometry = WktPolygonParser.ParseWkt(wktString);

			return RotateGeometry(geometry, degrees, origin);
		}

		/// <summary>
		/// Rotates a geometry represented by a WKT string and returns the result as a WKT string.
		/// </summary>
		/// <param name="wktString">WKT string representing the geometry to rotate.</param>
		/// <param name="degrees">Angle in degrees to rotate the geometry. Positive for counterclockwise rotation, negative for clockwise.</param>
		/// <param name="origin">Point of origin around which the geometry is rotated.</param>
		/// <returns>Rotated geometry represented as a WKT string.</returns>
		public static string RotateWktString(string wktString, float degrees, Vector2 origin)
		{
			// Apply rotation.
			List<List<List<Vector2>>> rotatedGeometry = RotateWktStringAsPoints(wktString, degrees, origin);

			// Convert back to WKT string and return.
			return WktPolygonParser.ToWktString(rotatedGeometry);
		}

		/// <summary>
		/// Normalizes the degrees to be within 0 to 359.99999.
		/// </summary>
		/// <param name="degrees">Degrees to be normalized.</param>
		/// <returns>Degrees within the range of 0 - 359.99999..</returns>
		/// <example>-5 will return 355 and 370 returns 10.</example>
		public static float NormalizeDegrees(float degrees)
		{
			degrees %= 360;
			if (degrees < 0)
				degrees += 360;

			return degrees;
		}

		/// <summary>
		/// Helper method to perform the actual rotation of geometry.
		/// </summary>
		/// <param name="originalGeometry">Original geometry as a nested list of Vector2 points.</param>
		/// <param name="degrees">Angle in degrees to rotate the geometry. Positive for counterclockwise rotation, negative for clockwise.</param>
		/// <param name="origin">Point of origin around which the geometry is rotated.</param>
		/// <returns>Rotated geometry as a nested list of <see cref="Vector2"/> points.</returns>
		private static List<List<List<Vector2>>> RotateGeometry(List<List<List<Vector2>>> originalGeometry, float degrees, Vector2 origin)
		{
			// Normalize the degrees to be within 0 to 359.99999..
			degrees = NormalizeDegrees(degrees);

			// Converts the normalized degrees to radians.
			float radians = degrees * Mathf.Deg2Rad;

			// Initializes a new list to store the rotated geometry.
			List<List<List<Vector2>>> rotatedGeometry = new();

			foreach (List<List<Vector2>> polygon in originalGeometry)
			{
				List<List<Vector2>> rotatedPolygon = new();
				foreach (List<Vector2> ring in polygon)
				{
					List<Vector2> rotatedRing = new();
					foreach (Vector2 point in ring)
					{
						// Translate point to origin.
						Vector2 translatedPoint = new(point.x - origin.x, point.y - origin.y);

						// Rotate point.
						Vector2 rotatedPoint = new(
							(translatedPoint.x * Mathf.Cos(radians)) - (translatedPoint.y * Mathf.Sin(radians)),
							(translatedPoint.x * Mathf.Sin(radians)) + (translatedPoint.y * Mathf.Cos(radians)));

						// Translate point back.
						rotatedPoint = new Vector2(rotatedPoint.x + origin.x, rotatedPoint.y + origin.y);

						// Adds rotated point to the rotated ring.
						rotatedRing.Add(rotatedPoint);
					}

					// Adds rotated ring to the rotated polygon
					rotatedPolygon.Add(rotatedRing);
				}

				// Adds rotated polygon to the rotated geometry.
				rotatedGeometry.Add(rotatedPolygon);
			}

			return rotatedGeometry;
		}

		/// <summary>
		/// Translate the <paramref name="wktString"/> by <paramref name="translation"/>.
		/// </summary>
		public static string Translate(string wktString, Vector2 translation)
		{
			if (translation == Vector2.zero)
				return wktString;

			List<List<List<Vector2>>> geometry = WktPolygonParser.ParseWkt(wktString);
			List<List<List<Vector2>>> translatedGeometry = Translate(geometry, translation);

			return WktPolygonParser.ToWktString(translatedGeometry);
		}

		/// <summary>
		/// Translate the <paramref name="geometry"/> by <paramref name="translation"/>.
		/// </summary>
		public static List<List<List<Vector2>>> Translate(List<List<List<Vector2>>> geometry, Vector2 translation)
		{
			if (translation == Vector2.zero)
				return geometry;

			List<List<List<Vector2>>> translatedGeometry = new();

			// Apply translation to each point in each polygon.
			foreach (List<List<Vector2>> polygon in geometry)
			{
				List<List<Vector2>> translatedPolygon = new();

				foreach (List<Vector2> ring in polygon)
				{
					List<Vector2> translatedRing = new();

					foreach (Vector2 point in ring)
						translatedRing.Add(point + translation);

					// Add the translated ring to the translated polygon.
					translatedPolygon.Add(translatedRing);
				}

				// Add the translated polygon to the translated geometry.
				translatedGeometry.Add(translatedPolygon);
			}

			return translatedGeometry;
		}

		/// <summary>
		/// Calculates the area of a polygonal ring using the Shoelace formula.
		/// </summary>
		/// <param name="ring">List of Vector2 points that define the polygonal ring. The ring should be closed,
		/// meaning the first and last points should be the same.</param>
		/// <returns>Calculated area of the ring. The sign of the area can indicate the orientation
		/// of the ring (positive for counter-clockwise, negative for clockwise).
		/// Returns 0f if the ring is empty.</returns>
		/// <remarks>
		/// WARNING: accuracy may be off by about 10 square km when performing tests. Use this only for rough estimates.
		/// Shoelace formula: https://en.wikipedia.org/wiki/Shoelace_formula
		/// </remarks>
		public static float CalculateRingArea(List<Vector2> ring)
		{
			float area = 0f;
			for (int i = 0; i < ring.Count; i++)
			{
				// This line calculates the index of the next vertex in the ring, wrapping around to 0 when i reaches the last index.
				// The modulus operator (%) ensures that when iterating through the ring vertices, the calculation loops back to the
				// starting vertex after reaching the end, thereby closing the loop of the polygon.
				int j = (i + 1) % ring.Count;

				// This line contributes to calculating the area of a polygonal ring using the shoelace formula(also known as Gauss's area formula).
				// The formula sums over the edges of the polygon, multiplying the x-coordinate of the current vertex by the y-coordinate of the next vertex,
				// and subtracting the product of the y-coordinate of the current vertex by the x-coordinate of the next vertex.
				//
				// Explanation of the components:
				// - ring[i].x * ring[j].y: Multiplies the x-coordinate of the current vertex (i) by the y-coordinate of the next vertex (j).
				// - ring[j].x * ring[i].y: Multiplies the x-coordinate of the next vertex (j) by the y-coordinate of the current vertex (i).
				// - The difference between these two products is then added to the 'area' accumulator.
				//
				// This approach effectively calculates the signed area of the polygon. The sign of the result (positive or negative)
				// indicates the winding direction of the vertices:
				// - A positive area implies that the vertices are ordered counter-clockwise.
				// - A negative area implies that the vertices are ordered clockwise.
				//
				// The final area is divided by 2.0f at the end of the loop to complete the calculation as per the shoelace formula.
				area += (ring[i].x * ring[j].y) - (ring[j].x * ring[i].y);
			}
			return area / 2.0f; // Direct calculation, sign indicates orientation.
		}

		/// <summary>
		/// Calculates the total surface area of a multipolygon, accounting for both exterior and interior rings (holes).
		/// </summary>
		/// <param name="multipolygon">List representing a multipolygon. Each element in the list is a polygon,
		/// which is itself a list of rings. The first ring in each polygon is the exterior ring, and any subsequent
		/// rings are considered interior rings (holes).</param>
		/// <returns>Total surface area of the multipolygon. Interior ring areas are subtracted from their
		/// respective exterior ring area, and the total area is always returned as a positive value.
		/// Returns 0f if the multipolygon is empty.</returns>
		/// <remarks>
		/// For each polygon (the first list), the first item in the second list is the exterior ring of the polygon, which defines the outer boundary.
		/// Any subsequent items in the second list are interior rings(holes) within that polygon.
		///
		/// WARNING: accuracy may be off by about 10 square km when performing tests and does not correctly calculate holes. Use this only for rough estimates.
		/// </remarks>
		public static float CalculateSurfaceArea(List<List<List<Vector2>>> multipolygon)
		{
			float totalArea = 0f;

			// Iterate over each polygon within the multipolygon. Each polygon is represented as a list of rings,
			// where the first ring is the exterior boundary, and any subsequent rings are interior boundaries (holes).
			foreach (List<List<Vector2>> polygon in multipolygon)
			{
				// Initialize a variable to hold the area of the current polygon. This variable will accumulate the
				// area of the exterior ring and subtract the areas of any interior rings.
				float polygonArea = 0f;

				// Iterate over each ring within the current polygon.
				for (int i = 0; i < polygon.Count; i++)
				{
					// Calculate the area of the current ring using the CalculateRingArea method.
					float ringArea = CalculateRingArea(polygon[i]);

					// If this is the first ring (i == 0), it's the exterior ring. Add its area to the polygon's total area.
					if (i == 0)
					{
						polygonArea += ringArea;
					}
					else
					{
						// For all subsequent rings (interior rings/holes), subtract their area from the polygon's total area.
						// Use Math.Abs to ensure the subtraction regardless of the calculated ring area's sign.
						// This is necessary because the interior ring should always reduce the polygon's total area,
						// even if the ring area calculation results in a negative value.
						polygonArea -= Math.Abs(ringArea);
					}
				}

				// Add the calculated area of the current polygon to the total area of the multipolygon.
				totalArea += polygonArea;
			}

			// Return the absolute value of the total area calculated for the multipolygon.
			// This ensures the returned surface area is always positive, as area measurements should not be negative.
			return Math.Abs(totalArea);
		}
	}
}
