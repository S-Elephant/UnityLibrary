using Elephant.UnityLibrary.GeoSystems.Interfaces;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Represents a geometric object that is linear in nature, such as a line or a series of connected lines.
	/// This abstract class provides a base for implementing linear geometry objects, including functionality for calculating and accessing their axis-aligned bounding box (AABB).
	/// </summary>
	public abstract class Lineal : Geometry, ILineal
	{
		/// <summary>
		/// Center position of this <see cref="Surface"/>.
		/// </summary>
		private Vector2 _center;

		/// <summary>
		/// Centroid (= the weighted center) of this <see cref="Surface"/>.
		/// </summary>
		private Vector2 _centroid;

		/// <summary>
		/// Center position of this <see cref="Surface"/>.
		/// </summary>
		public Vector2 Center
		{
			get
			{
				if (IsDirty)
					Recalculate();
				return _center;
			}
			protected set => _center = value;
		}

		/// <summary>
		/// Centroid (= the weighted center) of this <see cref="Surface"/>.
		/// </summary>
		public Vector2 Centroid
		{
			get
			{
				if (IsDirty)
					Recalculate();
				return _centroid;
			}
			protected set => _centroid = value;
		}

		/// <summary>
		/// AABB.
		/// </summary>
		private Rect _aabb;

		/// <inheritdoc/>
		public Rect Aabb
		{
			get
			{
				if (IsDirty)
					Recalculate();
				return _aabb;
			}
			protected set => _aabb = value;
		}

		/// <summary>
		/// Calculate the AABB.
		/// </summary>
		protected abstract Rect CalculateAabb();

		/// <inheritdoc/>
		public override void Recalculate()
		{
			Aabb = CalculateAabb();
			Center = CalculateCenter();
			Centroid = CalculateCenter();

			base.Recalculate();
			InvokeOnRecalculated();
		}

		/// <summary>
		/// Calculate the center and return it.
		/// </summary>
		protected abstract Vector2 CalculateCenter();

		/// <summary>
		/// Calculate the centroid and return it. This is the weighted center.
		/// </summary>
		protected abstract Vector2 CalculateCentroid();

		/// <summary>
		/// Rotate around <see cref="Center"/>.
		/// </summary>
		/// <param name="clockwiseAngleInDegrees">By how many degrees (clockwise) to rotate. Use a negative value to rotate counter-clockwise.</param>
		public virtual void RotateAroundCenter(float clockwiseAngleInDegrees)
		{
			RotateAroundPivot(clockwiseAngleInDegrees, Center);
		}

		/// <summary>
		/// Rotate around <see cref="Centroid"/>.
		/// </summary>
		/// <param name="clockwiseAngleInDegrees">By how many degrees (clockwise) to rotate. Use a negative value to rotate counter-clockwise.</param>
		public virtual void RotateAroundCentroid(float clockwiseAngleInDegrees)
		{
			RotateAroundPivot(clockwiseAngleInDegrees, Centroid);
		}

		/// <summary>
		/// Rotate around <see cref="Center"/>.
		/// </summary>
		/// <param name="clockwiseAngleInRadians">By how many radians (clockwise) to rotate. Use a negative value to rotate counter-clockwise.</param>
		public virtual void RotateAroundCenterUsingRad(float clockwiseAngleInRadians)
		{
			RotateAroundPivotUsingRad(clockwiseAngleInRadians, Center);
		}

		/// <summary>
		/// Rotate around <see cref="Centroid"/>.
		/// </summary>
		/// <param name="clockwiseAngleInRadians">By how many radians (clockwise) to rotate. Use a negative value to rotate counter-clockwise.</param>
		public virtual void RotateAroundCentroidUsingRad(float clockwiseAngleInRadians)
		{
			RotateAroundPivotUsingRad(clockwiseAngleInRadians, Centroid);
		}

		/// <summary>
		/// Rotate around <paramref name="pivot"/>.
		/// </summary>
		/// <param name="clockwiseAngleInDegrees">By how many degrees (clockwise) to rotate. Use a negative value to rotate counter-clockwise.</param>
		/// <param name="pivot">Vertex or point to rotate around.</param>
		public virtual void RotateAroundPivot(float clockwiseAngleInDegrees, Vector2 pivot)
		{
			float angleInRadians = clockwiseAngleInDegrees * -Mathf.Deg2Rad; // Minus sign because positive degrees = clockwise, but positive radians = counter-clockwise.
			RotateAroundPivotUsingRad(angleInRadians, pivot);
		}

		/// <summary>
		/// Rotate around <paramref name="pivot"/>.
		/// </summary>
		/// <param name="clockwiseAngleInRadians">By how many radians (clockwise) to rotate. Use a negative value to rotate counter-clockwise.</param>
		/// <param name="pivot">Vertex or point to rotate around.</param>
		public abstract void RotateAroundPivotUsingRad(float clockwiseAngleInRadians, Vector2 pivot);
	}
}
