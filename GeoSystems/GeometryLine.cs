using System;
using System.Diagnostics;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Represents a line in geometric space using 2D positions.
	/// </summary>
	[DebuggerDisplay("{Start} --> {end}")]
	public class GeometryLine : Lineal, IDisposable
	{
		/// <inheritdoc/>
		public override GeometryType GeometryType => GeometryType.Line;

		/// <summary>
		/// First or start or source point.
		/// </summary>
		public GeometryVertex _start;

		/// <summary>
		/// First or start or source point.
		/// </summary>
		public GeometryVertex Start
		{
			get => _start;
			set
			{
				_start.RemoveParent(this);
				_start = value;
				_start.AddParent(this);
				InvokeOnPropertyChanged();
			}
		}

		/// <summary>
		/// Second or end or destination point.
		/// </summary>
		public GeometryVertex _end;

		/// <summary>
		/// Second or end or destination point.
		/// </summary>
		public GeometryVertex End
		{
			get => _end;
			set
			{
				_end.RemoveParent(this);
				_end = value;
				_end.AddParent(this);
				InvokeOnPropertyChanged();
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public GeometryLine()
		{
			_start = new();
			_start.AddParent(this);

			_end = new();
			_end.AddParent(this);
		}

		/// <summary>
		/// Constructor with initializers.
		/// </summary>
		public GeometryLine(Vector2 start, Vector2 end)
		{
			_start = new(start);
			_start.AddParent(this);
			
			_end = new(end);
			_end.AddParent(this);
		}

		/// <summary>
		/// Constructor with initializers.
		/// </summary>
		public GeometryLine(GeometryVertex start, GeometryVertex end)
		{
			_start = start;
			_start.AddParent(this);

			_end = end;
			_end.AddParent(this);
		}

		/// <summary>
		/// Check if the line is empty (both points are the same).
		/// </summary>
		public bool IsEmpty => Start.Position == End.Position;

		/// <summary>
		/// Returns true if this is a valid line. A line is considered valid when
		/// its points have different coordinates.
		/// </summary>
		public bool IsValid() => Start.Position != End.Position; // Ensures the line has non-zero length

		/// <summary>
		/// Check if another line is equal by comparing start and end points.
		/// </summary>
		public bool Equals(GeometryLine other)
		{
			return Start.Position == other.Start.Position && End.Position == other.End.Position;
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			return obj is GeometryLine line && Equals(line);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			// Combine the hash codes of the start and end points
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				hash = hash * 23 + Start.Position.GetHashCode();
				hash = hash * 23 + End.Position.GetHashCode();
				return hash;
			}
		}

		/// <inheritdoc/>
		protected override Rect CalculateAabb()
		{
			// Determine min and max x and y values.
			float minX = Mathf.Min(Start.Position.x, End.Position.x);
			float minY = Mathf.Min(Start.Position.y, End.Position.y);
			float maxX = Mathf.Max(Start.Position.x, End.Position.x);
			float maxY = Mathf.Max(Start.Position.y, End.Position.y);

			// Calculate width and height.
			float width = maxX - minX;
			float height = maxY - minY;

			return new Rect(minX, minY, width, height);
		}

		/// <inheritdoc/>
		public override void Recalculate()
		{
			Start.Recalculate();
			End.Recalculate();

			base.Recalculate();
		}

		/// <inheritdoc/>
		public override object Clone()
		{
			GeometryLine result = new((GeometryVertex)Start.DeepCloneTyped(), (GeometryVertex)End.DeepCloneTyped());
			result.Aabb = Aabb;

			return result;
		}

		/// <summary>
		/// Deep clone this typed.
		/// </summary>
		public virtual GeometryLine DeepCloneTyped()
		{
			return (GeometryLine)Clone();
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			try
			{
				_start.RemoveParent(this);
			}
			catch
			{
				// Do nothing.
			}

			try
			{
				_end.RemoveParent(this);
			}
			catch
			{
				// Do nothing.
			}
		}
	}
}
