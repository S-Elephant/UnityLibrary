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
		/// <param name="isClockwise">If set to true, the rotation is clockwise; otherwise, it is counterclockwise.</param>
		/// <param name="origin">Point of origin around which the geometry is rotated.</param>
		/// <returns>New geometry data which is the rotated version of the original.</returns>
		public static List<List<List<Vector2>>> RotatePoints(List<List<List<Vector2>>> geometry, float degrees, bool isClockwise, Vector2 origin)
		{
			return RotateGeometry(geometry, degrees, isClockwise, origin);
		}

		/// <summary>
		/// Rotates a geometry represented by a WKT (Well-Known Text) string.
		/// </summary>
		/// <param name="wktString">WKT string representing the geometry to rotate.</param>
		/// <param name="degrees">Angle in degrees to rotate the geometry. Positive for counterclockwise rotation, negative for clockwise.</param>
		/// <param name="isClockwise">If set to true, the rotation is clockwise; otherwise, it is counterclockwise.</param>
		/// <param name="origin">Point of origin around which the geometry is rotated.</param>
		/// <returns>New geometry data which is the rotated version of the original.</returns>
		public static List<List<List<Vector2>>> RotateWktStringAsPoints(string wktString, float degrees, bool isClockwise, Vector2 origin)
		{
			List<List<List<Vector2>>> geometry = WktPolygonParser.ParseWkt(wktString);

			return RotateGeometry(geometry, degrees, isClockwise, origin);
		}

		/// <summary>
		/// Rotates a geometry represented by a WKT string and returns the result as a WKT string.
		/// </summary>
		/// <param name="wktString">WKT string representing the geometry to rotate.</param>
		/// <param name="degrees">Angle in degrees to rotate the geometry. Positive for counterclockwise rotation, negative for clockwise.</param>
		/// <param name="isClockwise">If set to true, the rotation is clockwise; otherwise, it is counterclockwise.</param>
		/// <param name="origin">Point of origin around which the geometry is rotated.</param>
		/// <returns>Rotated geometry represented as a WKT string.</returns>
		public static string RotateWktString(string wktString, float degrees, bool isClockwise, Vector2 origin)
		{
			// Apply rotation.
			List<List<List<Vector2>>> rotatedGeometry = RotateWktStringAsPoints(wktString, degrees, isClockwise, origin);

			// Convert back to WKT string and return.
			return WktPolygonParser.ToWktString(rotatedGeometry);
		}

		/// <summary>
		/// Normalizes the degrees to be within 0 to 359.99999..
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
		/// <param name="isClockwise">If set to true, the rotation is clockwise; otherwise, it is counterclockwise.</param>
		/// <param name="origin">Point of origin around which the geometry is rotated.</param>
		/// <returns>Rotated geometry as a nested list of <see cref="Vector2"/> points.</returns>
		private static List<List<List<Vector2>>> RotateGeometry(List<List<List<Vector2>>> originalGeometry, float degrees, bool isClockwise, Vector2 origin)
		{
			// Normalize the degrees to be within 0 to 359.99999..
			degrees = NormalizeDegrees(degrees);

			// Converts the normalized degrees to radians.
			float radians = degrees * Mathf.Deg2Rad;
			if (isClockwise)
				radians = -radians; // Invert radians for clockwise rotation.

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
	}
}
