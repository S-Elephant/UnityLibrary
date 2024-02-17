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
		/// AABB.
		/// </summary>
		private Rect _aabb;

		/// <inheritdoc/>
		public Rect Aabb
		{
			protected set => _aabb = value;
			get
			{
				if (IsDirty)
					Recalculate();
				return _aabb;
			}
		}

		/// <summary>
		/// Calculate the AABB.
		/// </summary>
		protected abstract Rect CalculateAabb();

		/// <inheritdoc/>
		public override void Recalculate()
		{
			Aabb = CalculateAabb();

			base.Recalculate();
		}
	}
}
