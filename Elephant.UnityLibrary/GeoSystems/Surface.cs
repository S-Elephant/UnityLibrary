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
		/// Surface area.
		/// </summary>
		private float _surfaceArea;

		/// <summary>
		/// Gizmos sphere size for debug usage only.
		/// </summary>
		[NonSerialized]
		public float GizmosSphereSize = 0.5f;

		/// <summary>
		/// Surface area.
		/// </summary>
		public float SurfaceArea
		{
			get
			{
				if (IsDirty)
					Recalculate();
				return _surfaceArea;
			}
			protected set => _surfaceArea = value;
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
