#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Geometric data structure that represents multiple polygons as a single entity.
	/// </summary>
	public class MultiPolygon : ICloneable
	{
		/// <summary>
		/// Geometry as lists. May contain polygons, rings and points.
		/// </summary>
		public List<List<List<Vector2>>> Geometry = new ();

		/// <summary>
		/// All rings of all polygons in this <see cref="Geometry"/>.
		/// </summary>
		public List<List<Vector2>> Rings => Geometry.SelectMany(polygon => polygon).ToList();

		/// <summary>
		/// All points of all rings of all polygons in this <see cref="Geometry"/>.
		/// </summary>
		public List<Vector2> Points => Geometry.SelectMany(polygon => polygon).SelectMany(ring => ring).ToList();

		/// <summary>
		/// Constructor.
		/// </summary>
		public MultiPolygon()
		{
		}

		/// <summary>
		/// Constructor using a geometry initializer.
		/// </summary>
		public MultiPolygon(List<List<List<Vector2>>> geometry)
		{
			Geometry = geometry;
		}

		/// <summary>
		/// Constructor using a Well-known text string initializer.
		/// </summary>
		public MultiPolygon(string wktString)
		{
			Geometry = WktPolygonParser.ParseWkt(wktString);
		}

		/// <inheritdoc/>
		public object Clone()
		{
			// Create a new instance of MultiPolygon
			MultiPolygon clone = new();

			// Deep copy of the Geometry property
			clone.Geometry = new List<List<List<Vector2>>>();
			foreach (List<List<Vector2>> polygon in Geometry)
			{
				List<List<Vector2>> newPoly = new();
				foreach (List<Vector2> ring in polygon)
				{
					List<Vector2> newRing = new();
					foreach (Vector2 point in ring)
						newRing.Add(point);
					newPoly.Add(newRing);
				}
				clone.Geometry.Add(newPoly);
			}

			return clone;
		}

		/// <summary>
		/// Creates a new <see cref="MultiPolygon"/> that is a copy of the current instance.
		/// </summary>
		/// <returns>New <see cref="MultiPolygon"/> that is a copy of this instance.</returns>
		public MultiPolygon CloneAsMultiPolygon()
		{
			return (MultiPolygon)Clone();
		}

		/// <summary>
		/// Return a WKT string based on this <see cref="MultiPolygon"/>.
		/// </summary>
		public string ToWktString()
		{
			return WktPolygonParser.ToWktString(Geometry);
		}

		/// <summary>
		/// Returns true if <see cref="MultiPolygon.Geometry"/> of both <see cref="MultiPolygon"/>s are equal.
		/// </summary>
		public bool EqualsOtherGeometry(MultiPolygon? multiPolygon)
		{
			if (multiPolygon == null)
				return false;

			return EqualsOtherGeometry(multiPolygon.Geometry);
		}

		/// <summary>
		/// Returns true if both geometries are equal.
		/// </summary>
		public bool EqualsOtherGeometry(List<List<List<Vector2>>>? otherGeometry)
		{
			// Check if the other geometry is null or that the count of polygons are not the same.
			if (otherGeometry is null || Geometry.Count != otherGeometry.Count)
				return false;

			// Compare each polygon.
			for (int i = 0; i < Geometry.Count; i++)
			{
				List<List<Vector2>> polygon = Geometry[i];
				List<List<Vector2>> otherPolygon = otherGeometry[i];

				// Check if the count of rings is the same.
				if (polygon.Count != otherPolygon.Count)
					return false;

				// Compare each ring.
				for (int j = 0; j < polygon.Count; j++)
				{
					List<Vector2> ring = polygon[j];
					List<Vector2> otherRing = otherPolygon[j];

					// Check if the count of points is the same.
					if (ring.Count != otherRing.Count)
					{
						return false;
					}

					// Compare each point.
					for (int k = 0; k < ring.Count; k++)
					{
						if (!ring[k].Equals(otherRing[k]))
							return false;
					}
				}
			}

			// If all checks passed, the geometries are equal
			return true;
		}
	}
}
