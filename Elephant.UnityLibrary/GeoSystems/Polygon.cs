#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Elephant.UnityLibrary.Extensions;
using Elephant.UnityLibrary.GeoSystems.Interfaces;
using Elephant.UnityLibrary.GeoSystems.Wkts;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Note that duplicate vertices are not allowed because they would create a self-intersecting boundary.
	/// </summary>
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	[Serializable]
	public class Polygon : Surface, IPolygonal, IDisposable
	{
		/// <inheritdoc/>
		public override GeometryType GeometryType => GeometryType.Polygon;

		/// <summary>
		/// Is used for the DebuggerDisplay only.
		/// </summary>
		public string DebuggerDisplay => $"ExteriorRing: {ExteriorRing.DebuggerDisplay}  InteriorRings: {string.Join(", ", InteriorRings.Select(InteriorRing => InteriorRing.DebuggerDisplay))}";

		/// <summary>
		/// Can be only 1 ring.
		/// </summary>
		public Ring ExteriorRing { get; set; } = new();

		/// <summary>
		/// A.k.a. holes. Can be 0 or more rings.
		/// </summary>
		[SerializeField]
		private readonly ObservableCollection<Ring> _interiorRings = new();

		/// <summary>
		/// A.k.a. holes. Can be 0 or more rings.
		/// </summary>
		public ObservableCollection<Ring> InteriorRings => _interiorRings;

		/// <summary>
		/// Returns true if this <see cref="Polygon"/> is empty.
		/// It's considered empty if it has an empty exterior ring.
		/// </summary>
		public bool IsEmpty => ExteriorRing.IsEmpty;

		/// <summary>
		/// Constructor.
		/// </summary>
		public Polygon()
		{
			_interiorRings.CollectionChanged += InteriorRingsOnCollectionChanged;
		}

		/// <summary>
		/// Constructor with initializers.
		/// </summary>
		public Polygon(Ring exteriorRing, List<Ring> interiorRings)
		{
			ExteriorRing = exteriorRing;

			_interiorRings = new(interiorRings);
			_interiorRings.CollectionChanged += InteriorRingsOnCollectionChanged;
		}

		private void InteriorRingsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			MarkAsDirty();
		}

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
			ObservableCollection<GeometryLine> allLines = ExteriorRing.Lines;
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

		/// <summary>
		/// Assign a new <see cref="ExteriorRing"/>.
		/// </summary>
		public void SetExteriorRing(Ring ring)
		{
			ExteriorRing = ring;
			MarkAsDirty();
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
			InvokeOnRecalculated();
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
		/// <param name="exteriorRingColor">Optional color to use for drawing the exterior ring of the polygon. If null, a default color is used.</param>
		/// <param name="interiorRingColor">Optional color to use for drawing the interior rings (holes) of the polygon. If null, a default color is used.</param>
		/// <param name="aabbColor">Optional color to use for drawing the axis-aligned bounding box of the polygon. If null, a default color is used.</param>
		/// <param name="centerColor">Optional color to use for marking the center of the polygon. If null, a default color is used.</param>
		/// <param name="centroidColor">Optional color to use for marking the centroid of the polygon. If null, a default color is used.</param>
		/// <remarks>
		/// This method uses Unity's Gizmos to visually debug the polygon by drawing its exterior and interior rings, 
		/// axis-aligned bounding box, center, and centroid with the specified colors. This is useful for visual debugging in the Unity Editor.
		/// </remarks>
		public void DrawGizmos(Vector2 offset, Color? exteriorRingColor = null, Color? interiorRingColor = null,
			Color? aabbColor = null, Color? centerColor = null, Color? centroidColor = null)
		{
			DrawAabbGizmo(offset, aabbColor);
		
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

			// Draw center.
			Gizmos.color = centerColor ?? Color.gray;
			Vector3 centerPosition = new(Center.x + offset.x, Center.y + offset.y, 0);
			Gizmos.DrawSphere(centerPosition, GizmosSphereSize);

			// Draw centroid.
			Gizmos.color = centroidColor ?? Color.cyan;
			Vector3 centroidPosition = new(Centroid.x + offset.x, Centroid.y + offset.y, 0);
			Gizmos.DrawSphere(centroidPosition, GizmosSphereSize);
		}

		/// <summary>
		/// Create a new <see cref="Polygon"/> from a valid polygon WKT string.
		/// </summary>
		public static Polygon FromWktString(string wktString)
		{
			Polygon polygon = new();

			// Assuming the input string is "POLYGON ((...))" or "POLYGON ((...),(...))"
			// Remove the POLYGON keyword and trim leading/trailing spaces and parentheses.
			string polygonData = WktPolygonParser.SanitizeWktString(wktString).Substring("POLYGON".Length).Trim('(', ')');

			// Split the data into exterior and interior rings
			string[] rings = polygonData.Split(new[] { "), (" }, StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < rings.Length; i++)
			{
				string ringString = rings[i];
				Ring ring = new();

				// Split the ring into coordinates.
				string[] coords = ringString.Trim().Split(',');

				// Previous point to connect lines.
				GeometryVertex? prevVertex = null;

				foreach (string coord in coords)
				{
					string[] parts = coord.Trim().Split(' ');
					if (parts.Length == 2 && float.TryParse(parts[0], out float x) && float.TryParse(parts[1], out float y))
					{
						GeometryVertex vertex = new() { Position = new Vector2(x, y) };

						if (prevVertex != null)
						{
							// Create a line from the previous vertex to the current vertex.
							GeometryLine line = new() { Start = prevVertex, End = vertex };
							ring.Lines.Add(line);
						}
						prevVertex = vertex;
					}
				}

				// Close the ring by connecting the last vertex to the first one, if not already connected.
				if (ring.Lines.Count > 0 && ring.Lines[0].Start != prevVertex)
				{
					GeometryLine closingLine = new() { Start = prevVertex, End = ring.Lines[0].Start };
					ring.Lines.Add(closingLine);
				}

				// Assign the ring to the polygon.
				if (i == 0) // The first ring is the exterior ring.
					polygon.ExteriorRing = ring;
				else // Subsequent rings are interior rings (holes).
					polygon.InteriorRings.Add(ring);
			}

			return polygon;
		}

		/// <summary>
		/// Parses a string representing a series of points into a list of Vector2 objects.
		/// </summary>
		/// <param name="ringString">The input string containing point coordinates, separated by commas and spaces. 
		/// Example format: "40 40, 20 45, 45 30, 40 40".</param>
		/// <returns>List of Vector2 objects representing the points parsed from the input string.</returns>
		/// <remarks>
		/// This method expects the input string to be formatted as a sequence of x and y coordinates 
		/// separated by spaces and commas. Each pair of x and y coordinates represents a point.
		/// </remarks>
		/// <example>
		/// <![CDATA[
		/// string input = "40 40, 20 45, 45 30, 40 40";
		/// List<Vector2> points = ParsePoints(input);
		/// foreach (Vector2 point in points)
		/// {
		///     Debug.Log(point);
		/// }
		/// ]]>
		/// </example>
		private static List<Vector2> ParsePoints(string ringString)
		{
			List<Vector2> points = new();

			// Split the input string by commas and spaces to extract individual coordinate strings.
			// StringSplitOptions.RemoveEmptyEntries removes any empty entries resulting from consecutive delimiters.
			string[] coords = ringString.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

			// Loop through the array of coordinate strings.
			// Increment by 2 each time since points are represented by pairs of coordinates (x and y).
			for (int i = 0; i < coords.Length; i += 2)
			{
				// Parse the x and y coordinates from the string to float.
				// Assumes the format is correct and every x has a corresponding y.
				float x = float.Parse(coords[i]);
				float y = float.Parse(coords[i + 1]);

				// Create a new Vector2 object from the parsed x and y values and add it to the list of points.
				points.Add(new Vector2(x, y));
			}

			// Return the list of parsed points.
			return points;
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

		/// <inheritdoc/>
		public void Dispose()
		{
			_interiorRings.CollectionChanged -= InteriorRingsOnCollectionChanged;
		}
	}
}
