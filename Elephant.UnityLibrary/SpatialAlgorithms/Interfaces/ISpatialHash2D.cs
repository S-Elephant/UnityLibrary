using System.Collections.Generic;
using UnityEngine;

namespace Elephant.UnityLibrary.SpatialAlgorithms.Interfaces
{
	/// <summary>
	/// Efficient way for managing many objects in a 2D space, especially when objects are dynamically added or removed.
	/// </summary>
	public interface ISpatialHash2D
	{
		/// <summary>
		/// Adds a spatial object to the grid, determining which grid cells it occupies based on its position and radius.
		/// This method also automatically assigns the <see cref="ISpatialObject2d.GridPos"/> based on its <see cref="ISpatialObject2d.Position"/>.
		/// </summary>
		/// <param name="obj">Spatial object to add to the grid.</param>
		void AddObject(ISpatialObject2d obj);

		/// <summary>
		/// Finds the closest neighbor to the given object, excluding objects in the exclusion list.
		/// </summary>
		/// <param name="current">Current object for which to find the closest neighbor.</param>
		/// <param name="excludeObjects">List of objects to exclude from the search.</param>
		/// <returns>Closest neighbor object or null if none found.</returns>
		ISpatialObject2d FindClosestNeighbor(ISpatialObject2d current, List<ISpatialObject2d> excludeObjects = null);

		/// <summary>
		/// Finds the closest neighbor that does not overlap a line if connected to the current object, excluding objects in the exclusion list.
		/// </summary>
		/// <param name="current">Current object for which to find the closest non-overlapping neighbor.</param>
		/// <param name="excludeObjects">List of objects to exclude from the search.</param>
		/// <param name="lineThickness">Thickness (radius) of the line to be checked.</param>
		/// <returns>Closest non-overlapping neighbor object or null if none found.</returns>
		ISpatialObject2d FindClosestNonOverlappingNeighbor(ISpatialObject2d current, List<ISpatialObject2d> excludeObjects, float lineThickness);

		/// <summary>
		/// Finds all objects in the neighboring cells around the specified cell.
		/// </summary>
		/// <param name="targetCell">Cell position to check neighboring cells around.</param>
		/// <param name="includeTargetCell">Whether to include objects from the current cell.</param>
		/// <returns>List of objects found in the neighboring cells.</returns>
		List<ISpatialObject2d> FindObjectsInNeighbors(Vector2Int targetCell, bool includeTargetCell = false);

		/// <summary>
		/// Checks if the line between start and end (with a given thickness) overlaps any objects, excluding the given list.
		/// </summary>
		/// <param name="start">Start position of the line.</param>
		/// <param name="end">End position of the line.</param>
		/// <param name="thickness">Thickness (radius) of the line.</param>
		/// <param name="excludeObjects">List of objects to exclude from the check.</param>
		/// <returns>True if there is an overlap, otherwise false.</returns>
		bool LineOverlapsObjects(Vector2 start, Vector2 end, float thickness, List<ISpatialObject2d> excludeObjects = null);

		/// <summary>
		/// Removes a <see cref="ISpatialObject2d"/> from this grid, adjusting the relevant grid cells based on its position and radius.
		/// </summary>
		/// <param name="obj"><see cref="ISpatialObject2d"/> to remove from this grid.</param>
		void RemoveObject(ISpatialObject2d obj);

		/// <summary>
		/// Convert a world position into a grid position/index.
		/// </summary>
		Vector2Int WorldToGrid(Vector2 position);
	}
}