using System;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Geometry with a surface area
	/// </summary>
	public abstract class Surface : Lineal
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
		/// Surface area.
		/// </summary>
		private float _surfaceArea;

		/// <summary>
		/// Gizmos sphere size for debug usage only.
		/// </summary>
		[NonSerialized]
		public float GizmosSphereSize = 0.5f;

		/// <summary>
		/// Center position of this <see cref="Surface"/>.
		/// </summary>
		public Vector2 Center
		{
			protected set => _center = value;
			get
			{
				if (IsDirty)
					Recalculate();
				return _center;
			}
		}

		/// <summary>
		/// Centroid (= the weighted center) of this <see cref="Surface"/>.
		/// </summary>
		public Vector2 Centroid
		{
			protected set => _centroid = value;
			get
			{
				if (IsDirty)
					Recalculate();
				return _centroid;
			}
		}

		/// <summary>
		/// Surface area.
		/// </summary>
		public float SurfaceArea
		{
			protected set => _surfaceArea = value;
			get
			{
				if (IsDirty)
					Recalculate();
				return _surfaceArea;
			}
		}

		/// <inheritdoc/>
		public override void Recalculate()
		{
			Center = CalculateCenter();
			Centroid = CalculateCentroid();
			SurfaceArea = CalculateSurfaceArea();

			base.Recalculate();
		}

		/// <summary>
		/// Calculate the center point.
		/// </summary>
		protected abstract Vector2 CalculateCenter();

		/// <summary>
		/// Calculate the centroid. This is the weighted center.
		/// </summary>
		protected abstract Vector2 CalculateCentroid();

		/// <summary>
		/// Calculate the surface area.
		/// </summary>
		protected abstract float CalculateSurfaceArea();

		/// <summary>
		/// Render AABB gizmo.
		/// </summary>
		public void DrawAabbGizmo(Vector2 offset, Color? aabbColor = null)
		{
			Gizmos.color = aabbColor ?? Color.green;
			// Top line
			Gizmos.DrawLine(new Vector2(Aabb.xMin, Aabb.yMin) + offset, new Vector2(Aabb.xMax, Aabb.yMin) + offset);
			// Bottom line
			Gizmos.DrawLine(new Vector2(Aabb.xMin, Aabb.yMax) + offset, new Vector2(Aabb.xMax, Aabb.yMax) + offset);
			// Left line
			Gizmos.DrawLine(new Vector2(Aabb.xMin, Aabb.yMin) + offset, new Vector2(Aabb.xMin, Aabb.yMax) + offset);
			// Right line
			Gizmos.DrawLine(new Vector2(Aabb.xMax, Aabb.yMin) + offset, new Vector2(Aabb.xMax, Aabb.yMax) + offset);
		}
	}
}
