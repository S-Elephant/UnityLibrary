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
	}
}
