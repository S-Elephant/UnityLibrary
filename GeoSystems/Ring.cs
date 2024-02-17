using System;
using System.Collections.Generic;
using System.Linq;
using Elephant.UnityLibrary.Extensions;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Polygon ring.
	/// </summary>
	public class Ring : Lineal, ICloneable
	{
		/// <summary>
		/// All lines that make this ring.
		/// </summary>
		public List<GeometryLine> Lines { get; set; } = new();

		/// <summary>
		/// Returns true if this <see cref="Ring"/ > is empty.
		/// </summary>
		public bool IsEmpty => !Lines.Any();

		/// <summary>
		/// Returns true if this ring is closed (meaning that the start and endpoint are the same).
		/// </summary>
		public bool IsClosed() => Lines.Count > 2 && Lines[0].Start == Lines.Last().End;

		/// <summary>
		/// Check if the ring is valid which checks if the ring is not empty,
		/// has the minimum number of lines (at least 3),
		/// and is closed (excluding self-intersection checks).
		/// </summary>
		public bool IsValid => !IsEmpty && IsClosed();

		/// <summary>
		/// Constructor.
		/// </summary>
		public Ring()
		{
		}

		/// <summary>
		/// Constructor with lines.
		/// </summary>
		public Ring(List<GeometryLine> lines)
		{
			Lines = lines;
		}

		/// <inheritdoc/>
		protected override Rect CalculateAabb()
		{
			IEnumerable<Rect> lineAabbs = Lines.Select(line => line.Aabb);

			return lineAabbs.Combine();
		}

		/// <inheritdoc/>
		public override void Recalculate()
		{
			foreach (GeometryLine line in Lines)
				line.Recalculate();

			base.Recalculate();
		}

		/// <inheritdoc/>
		public override object Clone()
		{
			Ring result = new();
			result.Aabb = Aabb;
			foreach (GeometryLine line in Lines)
				result.Lines.Add(line.DeepCloneTyped());

			return result;
		}

		/// <summary>
		/// Deep clone.
		/// </summary>
		public virtual Ring DeepCloneTyped()
		{
			return (Ring)Clone();
		}
	}
}
