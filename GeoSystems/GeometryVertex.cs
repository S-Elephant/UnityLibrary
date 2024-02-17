using Elephant.UnityLibrary.GeoSystems.Interfaces;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Represents a vertex in geometric space with a 2D position. This class extends <see cref="Geometry"/>
	/// and implements the <see cref="IVertex"/> interface.
	/// </summary>
	public class GeometryVertex : Geometry, IVertex
	{
		/// <inheritdoc/>
		public Vector2 Position { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public GeometryVertex()
		{
		}

		/// <summary>
		/// Constructor with initializer.
		/// </summary>
		public GeometryVertex(Vector2 position)
		{
			Position = position;
		}

		/// <summary>
		/// Constructor with initializers.
		/// </summary>
		public GeometryVertex(float x, float y)
		{
			Position = new Vector2(x, y);
		}

		public bool Equals(GeometryVertex other)
		{
			return this.Position == other.Position;
		}

		/// <inheritdoc/>
		public override bool Equals(object obj)
		{
			return obj is GeometryVertex vertex && Equals(vertex);
		}

		/// <inheritdoc/>
		public override int GetHashCode()
		{
			return Position.GetHashCode();
		}

		/// <inheritdoc/>
		public override object Clone()
		{
			return new GeometryVertex(Position);
		}

		/// <summary>
		/// Deep clone this typed.
		/// </summary>
		public virtual GeometryVertex DeepCloneTyped()
		{
			return new GeometryVertex(Position);
		}
	}
}
