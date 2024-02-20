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
using Debug = UnityEngine.Debug;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// A collection of zero or more polygons.
	/// </summary>
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	[Serializable]
	public class MultiPolygon : Surface, IMultiPolygonal, IDisposable
	{
		/// <inheritdoc/>
		public override GeometryType GeometryType => GeometryType.MultiPolygon;

		/// <summary>
		/// Is used for the DebuggerDisplay only.
		/// </summary>
		public string DebuggerDisplay => $"Polygons: {string.Join(", ", Polygons.Select(polygon => polygon.DebuggerDisplay))}";

		/// <summary>
		/// All <see cref="Polygon"/> that belong to this <see cref="MultiPolygon"/>.
		/// </summary>
		[SerializeField]
		private readonly ObservableCollection<Polygon> _polygons = new();

		/// <summary>
		/// All <see cref="Polygon"/> that belong to this <see cref="MultiPolygon"/>.
		/// </summary>
		public ObservableCollection<Polygon> Polygons => _polygons;

		/// <summary>
		/// Constructor.
		/// </summary>
		public MultiPolygon()
		{
			_polygons.CollectionChanged += PolygonsOnCollectionChanged;
		}

		/// <summary>
		/// Constructor with initializers.
		/// </summary>
		public MultiPolygon(List<Polygon> polygons)
		{
			_polygons = new(polygons);
			_polygons.CollectionChanged += PolygonsOnCollectionChanged;
		}

		private void PolygonsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			MarkAsDirty();
		}

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
		/// <param name="exteriorRingColor">Optional color to use for drawing the exterior ring of the polygon. If null, a default color is used.</param>
		/// <param name="interiorRingColor">Optional color to use for drawing the interior rings (holes) of the polygon. If null, a default color is used.</param>
		/// <param name="aabbColor">Optional color to use for drawing the axis-aligned bounding box of the polygon. If null, a default color is used.</param>
		/// <param name="centerColor">Optional color to use for marking the center of the polygon. If null, a default color is used.</param>
		/// <param name="centroidColor">Optional color to use for marking the centroid of the polygon. If null, a default color is used.</param>
		/// <remarks>
		/// This method uses Unity's Gizmos to visually debug the polygon by drawing its exterior and interior rings,
		/// axis-aligned bounding box, center, and centroid with the specified colors. This is useful for visual debugging in the Unity Editor.
		/// </remarks>
		public virtual void DrawGizmos(Vector2 offset, Color? exteriorRingColor = null, Color? interiorRingColor = null,
			Color? aabbColor = null, Color? centerColor = null, Color? centroidColor = null)
		{
			DrawAabbGizmo(offset, aabbColor);

			foreach (Polygon polygon in Polygons)
				polygon.DrawGizmos(offset, exteriorRingColor, interiorRingColor, aabbColor, centerColor, centroidColor);

			// Draw multi-polygon center.
			Gizmos.color = centerColor ?? Color.gray;
			Vector3 centerPosition = new(Center.x + offset.x, Center.y + offset.y, 0);
			Gizmos.DrawSphere(centerPosition, GizmosSphereSize);

			// Draw multi-polygon centroid.
			Gizmos.color = centroidColor ?? Color.cyan;
			Vector3 centroidPosition = new(Centroid.x + offset.x, Centroid.y + offset.y, 0);
			Gizmos.DrawSphere(centroidPosition, GizmosSphereSize);
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
			InvokeOnRecalculated();
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
		public override void RotateAroundPivotUsingRad(float clockwiseAngleInRadians, Vector2 pivot)
		{
			foreach (Polygon polygon in Polygons)
				polygon.RotateAroundPivotUsingRad(clockwiseAngleInRadians, pivot);
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
				// Total area of polygons is zero (or there are no polygons). Cannot calculate centroid. Centroid will be set to 0,0.
				return Vector2.zero;
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
		public override void Translate(Vector2 translation, Space space = Space.Self)
		{
			switch (space)
			{
				case Space.World:
					Vector2 multiPolygonCenter = CalculateCenter();

					// Determine the offset needed to move the current center to the new center (translation vector) minus offset.
					Vector2 effectiveTranslation = translation - multiPolygonCenter;

					// Apply this translation to each polygon in the MultiPolygon.
					foreach (Polygon polygon in Polygons)
					{
						foreach (GeometryLine line in polygon.ExteriorRing.Lines)
						{
							line.Start.Position += effectiveTranslation;
							line.End.Position += effectiveTranslation;
						}
						foreach (Ring ring in polygon.InteriorRings)
						{
							foreach (GeometryLine line in ring.Lines)
							{
								line.Start.Position += effectiveTranslation;
								line.End.Position += effectiveTranslation;
							}
						}
					}
					break;
				case Space.Self:
					foreach (Polygon polygon in Polygons)
						polygon.Translate(translation, space);
					break;
				default:
					Debug.LogError($"$Missing case-statement. Got {space}. No translation applied.");
					return;
			}
		}

		/// <inheritdoc/>
		public override List<GeometryVertex> AllVertices()
		{
			List<GeometryVertex> allVertices = new();
			foreach (Polygon polygon in Polygons)
				allVertices.AddRange(polygon.AllVertices());

			return allVertices;
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

		/// <inheritdoc/>
		public void Dispose()
		{
			_polygons.CollectionChanged -= PolygonsOnCollectionChanged;
		}
	}
}
