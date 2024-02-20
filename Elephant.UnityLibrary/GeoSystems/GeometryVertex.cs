using System;
using System.Collections.Generic;
using System.Diagnostics;
using Elephant.UnityLibrary.GeoSystems.Interfaces;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Represents a vertex in geometric space with a 2D position. This class extends <see cref="Geometry"/>
	/// and implements the <see cref="IVertex"/> interface.
	/// </summary>
	[DebuggerDisplay("({Position.x}, {Position.y})")]
	[Serializable]
	public class GeometryVertex : Geometry, IVertex
	{
		/// <inheritdoc/>
		public override GeometryType GeometryType => GeometryType.Vertex;

		/// <summary>
		/// Position.
		/// </summary>
		[SerializeField]
		private Vector2 _position = Vector2.zero;

		/// <inheritdoc/>
		public Vector2 Position
		{
			get => _position;
			set
			{
				if (Position == value)
					return;

				_position = value;
				IsDirty = true;
			}
		}

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

		/// <summary>
		/// Determines whether the specified <paramref name="other"/> is equal to the current <see cref="GeometryVertex"/>.
		/// </summary>
		public bool Equals(GeometryVertex other)
		{
			return Position == other.Position;
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
		public override void Recalculate()
		{
			base.Recalculate();
			InvokeOnRecalculated();
		}

		/// <summary>
		/// Rotates this <see cref="GeometryVertex"/> around a given pivot.
		/// </summary>
		/// <param name="angleInRadians">Angle of rotation in radians. Positive values for clockwise rotation, negative values for counter-clockwise.</param>
		/// <param name="pivot">Pivot point around which to rotate.</param>
		/// <returns>Rotated point as a new Vector2 instance.</returns>
		public void RotateAroundPivot(float angleInRadians, Vector2 pivot)
		{
			Position = RotateAroundPivot(Position, angleInRadians, pivot);
		}

		/// <summary>
		/// Rotates a point around a given pivot.
		/// </summary>
		/// <param name="point">Point to rotate.</param>
		/// <param name="angleInRadians">Angle of rotation in radians. Positive values for counter-clockwise rotation, negative values for clockwise rotation.</param>
		/// <param name="pivot">Pivot point around which to rotate.</param>
		/// <returns>Rotated point as a new Vector2 instance.</returns>
		public static Vector2 RotateAroundPivot(Vector2 point, float angleInRadians, Vector2 pivot)
		{
			// Rotation by 0 radians/degrees should do nothing.
			if (angleInRadians == 0)
				return point;

			// Translate point back to origin: pivot becomes (0,0).
			float translatedX = point.x - pivot.x;
			float translatedY = point.y - pivot.y;

			// Apply rotation.
			float rotatedX = translatedX * Mathf.Cos(angleInRadians) - translatedY * Mathf.Sin(angleInRadians);
			float rotatedY = translatedX * Mathf.Sin(angleInRadians) + translatedY * Mathf.Cos(angleInRadians);

			// Translate point back.
			Vector2 rotatedPoint = new(rotatedX + pivot.x, rotatedY + pivot.y);

			return rotatedPoint;
		}

		/// <inheritdoc/>
		public override void Translate(Vector2 translation, Space space = Space.Self)
		{
			switch (space)
			{
				case Space.Self when translation == Vector2.zero:
					return;
				case Space.Self:
					Position += translation;
					break;
				case Space.World:
					Position = translation;
					break;
				default:
					Debug.LogError($"$Missing case-statement. Got {space}. No translation applied.");
					return;
			}
		}

		/// <inheritdoc/>
		public override List<GeometryVertex> AllVertices()
		{
			return new List<GeometryVertex> { this };
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
