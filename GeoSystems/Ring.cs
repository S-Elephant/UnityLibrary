using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Elephant.UnityLibrary.Extensions;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Polygon ring.
	/// </summary>
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class Ring : Lineal, ICloneable, IDisposable
	{
		/// <inheritdoc/>
		public override GeometryType GeometryType => GeometryType.Ring;

		/// <summary>
		/// All lines that make this ring.
		/// </summary>
		private readonly ObservableCollection<GeometryLine> _lines = new();

		/// <summary>
		/// All lines that make this ring.
		/// </summary>
		public ObservableCollection<GeometryLine> Lines => _lines;

		/// <summary>
		/// Is used for the DebuggerDisplay only.
		/// </summary>
		public string DebuggerDisplay => $"Lines: {string.Join(", ", _lines.Select(line => $"{line.Start.Position.x},{line.Start.Position.y} --> {line.End.Position.x},{line.End.Position.y}"))}";

		/// <summary>
		/// Returns true if this <see cref="Ring" /> is empty.
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
			Lines.CollectionChanged += LinesOnCollectionChanged;
		}

		/// <summary>
		/// Constructor with lines.
		/// </summary>
		public Ring(List<GeometryLine> lines)
		{
			_lines = new(lines);
			Lines.CollectionChanged += LinesOnCollectionChanged;
		}

		private void LinesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			MarkAsDirty();
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

		/// <inheritdoc/>
		public void Dispose()
		{
			Lines.CollectionChanged -= LinesOnCollectionChanged;
		}
	}
}
