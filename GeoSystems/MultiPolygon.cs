#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Elephant.UnityLibrary.Extensions;
using Elephant.UnityLibrary.GeoSystems.Interfaces;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// A collection of zero or more polygons.
	/// </summary>
	public class MultiPolygon : Surface, IMultiPolygonal
	{
		/// <summary>
		/// All <see cref="Polygon"/> that belong to this <see cref="MultiPolygon"/>.
		/// </summary>
		public List<Polygon> Polygons { get; set; } = new List<Polygon>();

		/// <summary>
		/// Adds a new <see cref="Polygon"/> to this <see cref="MultiPolygon"/>.
		/// </summary>
		public virtual void AddPolygon(Polygon polygonToAdd)
		{
			Polygons.Add(polygonToAdd);
			IsDirty = true;
		}

		/// <summary>
		/// Returns true if this <see cref="MultiPolygon"/> contains no <see cref="Polygon"/>s.
		/// </summary>
		public bool IsEmpty => Polygons.Count == 0;

		/// <summary>
		/// Draw all polygons inside this multi-polygon using gizmos.
		/// </summary>
		/// <param name="offset">Render offset to apply onto every vertex of this polygon.</param>
		public virtual void DrawGizmos(Vector2 offset, Color? exteriorRingColor = null, Color? interiorRingColor = null,
			Color? aabbColor = null, Color? centerColor = null, Color? centroidColor = null)
		{
			foreach (Polygon polygon in Polygons)
				polygon.DrawGizmos(offset, exteriorRingColor, interiorRingColor, aabbColor, centerColor, centroidColor);
		}

		/// <summary>
		/// Returns a new <see cref="MultiPolygon"/> from a valid multi-polygon WKT string.
		/// </summary>
		public static MultiPolygon FromWktString(string wktString)
		{
			List<string> polygonWktStrings = WktPolygonParser.SplitMultiPolygonIntoPolygons(wktString);

			MultiPolygon result = new();
			foreach (string polygonWktString in polygonWktStrings)
				result.AddPolygon(Polygon.FromWktString(polygonWktString));

			return result;
		}

		/// <inheritdoc/>
		protected override Rect CalculateAabb()
		{
			IEnumerable<Rect> rects = Polygons.Select(polygon => polygon.Aabb);
			return rects.Combine();
		}

		/// <inheritdoc/>
		public override void Recalculate()
		{
			foreach (Polygon polygon in Polygons)
				polygon.Recalculate();

			base.Recalculate();
		}

		/// <inheritdoc/>
		protected override Vector2 CalculateCenter()
		{
			// Ensure there are polygons to avoid division by zero.
			int count = Polygons.Count;
			if (count == 0)
				return Vector2.zero;

			// Sum up all the x and y coordinates.
			float totalX = 0;
			float totalY = 0;
			foreach (Polygon polygon in Polygons)
			{
				totalX += polygon.Center.x;
				totalY += polygon.Center.y;
			}

			// Calculate the average for each coordinate to find the overall center.
			return new Vector2(totalX / count, totalY / count);
		}

		/// <inheritdoc/>
		protected override Vector2 CalculateCentroid()
		{
			float totalArea = 0;
			Vector2 weightedCentroidSum = Vector2.zero;

			foreach (Polygon polygon in Polygons)
			{
				Vector2 centroid = polygon.Centroid;
				float area = polygon.SurfaceArea;

				if (area == 0) continue; // Skip polygons with no area

				weightedCentroidSum += centroid * area;
				totalArea += area;
			}

			if (totalArea == 0)
			{
				throw new InvalidOperationException("Total area of polygons is zero. Cannot calculate centroid.");
			}

			return weightedCentroidSum / totalArea;
		}

		/// <inheritdoc/>
		protected override float CalculateSurfaceArea()
		{
			float surfaceArea = 0f;
			foreach (Polygon polygon in Polygons)
				surfaceArea += polygon.SurfaceArea;

			return surfaceArea;
		}

		/// <inheritdoc/>
		public override object Clone()
		{
			MultiPolygon result = new();
			foreach (Polygon polygon in Polygons)
				result.Polygons.Add(polygon.DeepCloneTyped());

			return result;
		}

		/// <summary>
		/// Deep clone this typed.
		/// </summary>
		public virtual MultiPolygon DeepCloneTyped()
		{
			return (MultiPolygon)Clone();
		}
	}
}
