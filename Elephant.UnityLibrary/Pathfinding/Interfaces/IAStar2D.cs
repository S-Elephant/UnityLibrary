using System.Collections.Generic;
using UnityEngine;

namespace Elephant.UnityLibrary.Pathfinding.Interfaces
{
	/// <summary>
	/// Interface for a 2D A* pathfinding algorithm.
	/// </summary>
	public interface IAStar2D
	{
		/// <summary>
		/// Finds a path from the start position to the destination.
		/// </summary>
		/// <param name="grid">Grid of <see cref="IGridCell2D"/>s used for pathfinding.</param>
		/// <param name="gridWidth">Grid width in <see cref="IGridCell2D"/>s.</param>
		/// <param name="gridHeight">Grid height in <see cref="IGridCell2D"/>s.</param>
		/// <param name="startPosition">Starting position of the pathfinding.</param>
		/// <param name="destination">Target position to reach.</param>
		/// <param name="ignoreStartBlocked">Whether to ignore if the start position is blocked.</param>
		/// <param name="ignoreDestinationBlocked">Whether to ignore if the destination is blocked.</param>
		/// <returns>List of positions representing the path from the start to the destination.</returns>
		List<Vector2Int> FindPath(List<List<IGridCell2D>> grid, int gridWidth, int gridHeight, Vector2Int startPosition, Vector2Int destination, bool ignoreStartBlocked = true, bool ignoreDestinationBlocked = false);
	}
}
