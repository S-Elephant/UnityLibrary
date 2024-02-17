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
	/// Note that duplicate vertices are not allowed because they would create a self-intersecting boundary.
	/// </summary>
	public class Polygon : Surface, IPolygonal
	{
		/// <summary>
		/// Can be only 1 ring.
		/// </summary>
		public Ring ExteriorRing { get; set; } = new();

		/// <summary>
		/// A.k.a. holes. Can be 0 or more rings.
		/// </summary>
		public List<Ring> InteriorRings { get; set; } = new();

		/// <summary>
		/// Returns true if this <see cref="Polygon"/> is empty.
		/// It's considered empty if it has an empty exterior ring.
		/// </summary>
		public bool IsEmpty => ExteriorRing.IsEmpty;

		/// <summary>
		/// Returns true if all rings are valid using <see cref="Ring.IsValid"/>.
		/// </summary>
		public bool IsValid()
		{
			// Check if the exterior ring is valid:
			// It must be closed and contain at least 3 lines to form a polygon.
			if (!ExteriorRing.IsClosed() || ExteriorRing.Lines.Count < 3)
			{
				return false;
			}

			// Check each interior ring.
			foreach (Ring interiorRing in InteriorRings)
			{
				// Each interior ring must also be closed and contain at least 3 lines.
				if (!interiorRing.IsClosed() || interiorRing.Lines.Count < 3)
					return false;

				// Ideally, check for self-intersection in each interior ring.
				// Not implemented due to complexity.
			}

			// If all checks pass, the polygon is considered valid.
			return true;
		}

		/// <inheritdoc/>
		protected override Vector2 CalculateCenter()
		{
			List<Vector2> allPoints = ExteriorRing.Lines.SelectMany(line => new[] { line.Start.Position, line.End.Position }).ToList();
			float x = allPoints.Average(point => point.x);
			float y = allPoints.Average(point => point.y);
			return new Vector2(x, y);
		}

		/// <inheritdoc/>
		protected override Vector2 CalculateCentroid()
		{
			List<GeometryLine> allLines = ExteriorRing.Lines;
			if (allLines.Count < 3)
			{
				return Vector2.zero;
			}

			float accumulatedArea = 0.0f;
			float centerX = 0.0f;
			float centerY = 0.0f;

			for (int i = 0; i < allLines.Count; i++)
			{
				Vector2 currentPoint = allLines[i].Start.Position;
				Vector2 nextPoint = i + 1 < allLines.Count ? allLines[i + 1].Start.Position : allLines[0].Start.Position;
				float tempArea = currentPoint.x * nextPoint.y - nextPoint.x * currentPoint.y;
				accumulatedArea += tempArea;
				centerX += (currentPoint.x + nextPoint.x) * tempArea;
				centerY += (currentPoint.y + nextPoint.y) * tempArea;
			}

			if (accumulatedArea == 0)
				return Vector2.zero;

			accumulatedArea *= 0.5f;
			centerX /= (6.0f * accumulatedArea);
			centerY /= (6.0f * accumulatedArea);

			return new Vector2(centerX, centerY);
		}

		/// <inheritdoc/>
		protected override float CalculateSurfaceArea()
		{
			// Calculate the area of the exterior ring
			float area = CalculateRingArea(ExteriorRing);

			// Subtract the area of each interior ring
			foreach (Ring interiorRing in InteriorRings)
				area -= CalculateRingArea(interiorRing);

			return Math.Abs(area);
		}

		/// <summary>
		/// Calculates the signed area of a ring.
		/// </summary>
		/// <param name="ring">Ring for which to calculate the area.</param>
		/// <returns>Signed area of the ring.</returns>
		private float CalculateRingArea(Ring ring)
		{
			float area = 0f;
			List<Vector2> points = ring.Lines.SelectMany(line => new[] { line.Start.Position, line.End.Position }).Distinct().ToList();
			for (int i = 0; i < points.Count; i++)
			{
				Vector2 p1 = points[i];
				Vector2 p2 = points[(i + 1) % points.Count]; // Wrap around to the first point
				area += (p1.x * p2.y) - (p2.x * p1.y);
			}
			return area * 0.5f;
		}

		/// <inheritdoc/>
		public override void Recalculate()
		{
			ExteriorRing.Recalculate();
			foreach (Ring interiorRing in InteriorRings)
				interiorRing.Recalculate();

			base.Recalculate();
		}

		/// <summary>
		/// Draw this polygon using gizmos.
		/// </summary>
		/// <param name="position">The center position where the center of this polygon will be rendered.</param>
		public void DrawGizmosOnPosition(Vector2 position)
		{
			Vector2 centerOffset = position - Center; // Calculate offset to move polygon's center to the specified position
			DrawGizmos(centerOffset);
		}

		/// <summary>
		/// Draw this polygon using gizmos.
		/// </summary>
		/// <param name="offset">Render offset to apply onto every vertex of this polygon.</param>
		public void DrawGizmos(Vector2 offset, Color? exteriorRingColor = null, Color? interiorRingColor = null,
			Color? aabbColor = null, Color? centerColor = null, Color? centroidColor = null)
		{
			// Draw exterior ring.
			Gizmos.color = exteriorRingColor ?? Color.blue;
			for (int i = 0; i < ExteriorRing.Lines.Count; i++)
			{
				Vector2 start = ExteriorRing.Lines[i].Start.Position + offset;
				Vector2 end = i < ExteriorRing.Lines.Count - 1 ? ExteriorRing.Lines[i + 1].Start.Position + offset : ExteriorRing.Lines[0].Start.Position + offset;
				Gizmos.DrawLine(start, end);
			}

			// Draw interior rings (holes).
			Gizmos.color = interiorRingColor ?? Color.red;
			foreach (Ring ring in InteriorRings)
			{
				for (int i = 0; i < ring.Lines.Count; i++)
				{
					Vector2 start = ring.Lines[i].Start.Position + offset;
					Vector2 end = i < ring.Lines.Count - 1 ? ring.Lines[i + 1].Start.Position + offset : ring.Lines[0].Start.Position + offset;
					Gizmos.DrawLine(start, end);
				}
			}

			// Draw AABB.
			Gizmos.color = aabbColor ?? Color.green;
			// Top line
			Gizmos.DrawLine(new Vector2(Aabb.xMin, Aabb.yMin) + offset, new Vector2(Aabb.xMax, Aabb.yMin) + offset);
			// Bottom line
			Gizmos.DrawLine(new Vector2(Aabb.xMin, Aabb.yMax) + offset, new Vector2(Aabb.xMax, Aabb.yMax) + offset);
			// Left line
			Gizmos.DrawLine(new Vector2(Aabb.xMin, Aabb.yMin) + offset, new Vector2(Aabb.xMin, Aabb.yMax) + offset);
			// Right line
			Gizmos.DrawLine(new Vector2(Aabb.xMax, Aabb.yMin) + offset, new Vector2(Aabb.xMax, Aabb.yMax) + offset);

			// Draw Center
			Gizmos.color = centerColor ?? Color.gray;
			Vector3 centerPosition = new(Center.x + offset.x, Center.y + offset.y, 0);
			Gizmos.DrawSphere(centerPosition, GizmosSphereSize);

			// Draw Centroid
			Gizmos.color = centroidColor ?? Color.cyan;
			Vector3 centroidPosition = new(Centroid.x + offset.x, Centroid.y + offset.y, 0);
			Gizmos.DrawSphere(centroidPosition, GizmosSphereSize);
		}

		/// <summary>
		/// Create a new <see cref="Polygon"/> from a valid polygon WKT string.
		/// </summary>
		public static Polygon FromWktString(string wktString)
		{
			List<List<List<Vector2>>> geometryData = WktPolygonParser.ParseWkt(wktString);

			// Check if there's at least one polygon in the data.
			if (geometryData.Count == 0 || geometryData[0].Count == 0)
			{
				throw new ArgumentException("Invalid WKT string: No polygons found.");
			}

			// Create a new Polygon instance.
			Polygon polygon = new();

			// Only consider the first polygon (ignore potential multi-polygon structure).
			List<List<Vector2>> polygonData = geometryData[0];

			// The first list of Vector2 points is the exterior ring.
			List<Vector2> exteriorRingPoints = polygonData[0];
			Ring exteriorRing = new();

			// Convert the list of Vector2 points to GeometryLines for the exterior ring.
			for (int i = 0; i < exteriorRingPoints.Count; i++)
			{
				Vector2 start = exteriorRingPoints[i];
				Vector2 end = exteriorRingPoints[(i + 1) % exteriorRingPoints.Count]; // Ensure we loop back to the start for the last line.
				GeometryLine line = new(start, end);
				exteriorRing.Lines.Add(line);
			}

			polygon.ExteriorRing = exteriorRing;

			// Process any interior rings (holes).
			for (int i = 1; i < polygonData.Count; i++)
			{
				Ring interiorRing = new();
				List<Vector2> interiorRingPoints = polygonData[i];

				for (int j = 0; j < interiorRingPoints.Count; j++)
				{
					Vector2 start = interiorRingPoints[j];
					Vector2 end = interiorRingPoints[(j + 1) % interiorRingPoints.Count]; // Loop back to start for the last line.
					GeometryLine line = new(start, end);
					interiorRing.Lines.Add(line);
				}

				polygon.InteriorRings.Add(interiorRing);
			}

			// Since the polygon's data might have changed, mark it as needing recalculation of geometric properties.
			polygon.IsDirty = true;

			return polygon;
		}


		/// <inheritdoc/>
		protected override Rect CalculateAabb()
		{
			List<Rect> ringAabbs = InteriorRings.Select(ring => ring.Aabb).ToList();
			ringAabbs.Add(ExteriorRing.Aabb);

			return ringAabbs.Combine();
		}

		/// <inheritdoc/>
		public override object Clone()
		{
			Polygon result = new();

			result.Aabb = Aabb;
			result.Center = Center;
			result.Centroid = Centroid;

			result.ExteriorRing = ExteriorRing.DeepCloneTyped();
			foreach (Ring interiorRing in InteriorRings)
				result.InteriorRings.Add(interiorRing);

			return result;
		}

		/// <summary>
		/// Deep clone this typed.
		/// </summary>
		public virtual Polygon DeepCloneTyped()
		{
			return (Polygon)Clone();
		}
	}
}
