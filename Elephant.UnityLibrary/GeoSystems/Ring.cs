using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Elephant.UnityLibrary.Extensions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Polygon ring.
	/// </summary>
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	[Serializable]
	public class Ring : Lineal, ICloneable, IDisposable
	{
		/// <inheritdoc/>
		public override GeometryType GeometryType => GeometryType.Ring;

		/// <summary>
		/// All lines that make this ring.
		/// </summary>
		[SerializeField]
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
		// ReSharper disable once PossibleUnintendedReferenceComparison
		public bool IsClosed() => Lines.Count > 2 && Lines[0].Start == Lines.Last().End;

		/// <summary>
		/// Returns true if this ring is open (meaning that the start and endpoint are not the same).
		/// </summary>
		public bool IsOpen() => !IsClosed();

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
		protected override Vector2 CalculateCenter()
		{
			if (_lines.Count == 0)
				return Vector2.zero;

			float sumX = 0, sumY = 0;
			int count = 0;

			foreach (GeometryLine line in _lines)
			{
				sumX += line.Start.Position.x + line.End.Position.x;
				sumY += line.Start.Position.y + line.End.Position.y;
				count += 2; // Adding two points per line.
			}

			return new Vector2(sumX / count, sumY / count);
		}

		/// <inheritdoc/>
		protected override Vector2 CalculateCentroid()
		{
			if (_lines.Count == 0)
				return Vector2.zero;

			double sumX = 0, sumY = 0, totalLength = 0;
			foreach (GeometryLine line in _lines)
			{
				// Calculate midpoint of the line.
				float midX = (line.Start.Position.x + line.End.Position.x) / 2;
				float midY = (line.Start.Position.y + line.End.Position.y) / 2;

				// Calculate length of the line.
				double length = Math.Sqrt(Math.Pow(line.End.Position.x - line.Start.Position.x, 2) + Math.Pow(line.End.Position.y - line.Start.Position.y, 2));

				// Add weighted midpoints.
				sumX += midX * length;
				sumY += midY * length;
				totalLength += length;
			}

			// Divide by total length to get the weighted average.
			return new Vector2((float)sumX / (float)totalLength, (float)sumY / (float)totalLength);
		}

		/// <inheritdoc/>
		public override void RotateAroundPivotUsingRad(float clockwiseAngleInRadians, Vector2 pivot)
		{
			List<GeometryVertex> vertices = AllVertices();

			foreach (GeometryVertex vertex in vertices)
				vertex.RotateAroundPivot(clockwiseAngleInRadians, pivot);
		}

		/// <inheritdoc/>
		public override void Translate(Vector2 translation, Space space = Space.Self)
		{
			switch (space)
			{
				case Space.World:
					// Determine the offset needed to move the current center to the new center (translation vector) minus offset.
					Vector2 effectiveTranslation = translation - CalculateCenter();

					// Apply this translation to each line in the ring.
					foreach (GeometryLine line in Lines)
					{
						// Translate each line by the calculated translation vector.
						line.Start.Position += effectiveTranslation;
						line.End.Position += effectiveTranslation;
					}
					break;
				case Space.Self:
					foreach (GeometryLine line in Lines)
						line.Translate(translation, space);
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

			int lastLineIndex = Lines.Count - 1;
			for (int index = 0; index < Lines.Count; index++)
			{
				GeometryLine line = Lines[index];

				if (index == 0)
				{
					allVertices.AddRange(line.AllVertices());
				}
				else
				{
					// Only add the last line end point if it's not the last line because the
					// endpoint of the last line is the start point of the first line.
					// This only applies if the ring is closed.
					if (IsOpen() || index != lastLineIndex)
						allVertices.Add(line.End);
				}
			}

			return allVertices;
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
