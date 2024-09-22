using UnityEngine;

namespace Elephant.UnityLibrary.SpatialAlgorithms.Interfaces
{
	/// <summary>
	/// Represents an object in 2D space.
	/// </summary>
	public interface ISpatialObject2d
	{
		/// <summary>
		/// Object world position.
		/// </summary>
		Vector2 Position { get; set; }

		/// <summary>
		/// Position/Index in the <see cref="SpatialHash2D"/>.
		/// </summary>
		Vector2Int GridPos { get; set; }

		/// <summary>
		/// Object radius, used for overlap calculation.
		/// </summary>
		float Radius { get; set; }
	}
}
