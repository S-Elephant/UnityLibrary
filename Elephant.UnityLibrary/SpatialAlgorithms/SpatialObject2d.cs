using Elephant.UnityLibrary.SpatialAlgorithms.Interfaces;
using UnityEngine;

namespace Elephant.UnityLibrary.SpatialAlgorithms
{
	/// <inheritdoc/>
	public class SpatialObject2d : ISpatialObject2d
	{
		/// <inheritdoc/>
		public Vector2 Position { get; set; }

		/// <inheritdoc/>
		public Vector2Int GridPos { get; set; }

		/// <inheritdoc/>
		public float Radius { get; set; }

		/// <summary>
		/// Contructor.
		/// </summary>
		public SpatialObject2d(Vector2 position, float radius)
		{
			Position = position;
			Radius = radius;
		}
	}
}
