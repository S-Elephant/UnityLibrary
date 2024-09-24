#nullable enable

using System.Collections.Generic;
using System.Linq;
using Elephant.UnityLibrary.SpatialAlgorithms.Interfaces;
using UnityEngine;

namespace Elephant.UnityLibrary.SpatialAlgorithms
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public class SpatialHash2D : ISpatialHash2D
	{
		/// <summary>
		/// Size of each cell in this grid.
		/// </summary>
		private float _cellSize;

		/// <summary>
		/// Dictionary mapping grid positions to lists of <see cref="ISpatialObject2d"/>s within those positions.
		/// </summary>
		private Dictionary<Vector2Int, List<ISpatialObject2d>> _grid;

		/// <summary>
		/// Constructor.
		/// </summary>
		public SpatialHash2D(float cellSize)
		{
			_cellSize = cellSize;
			_grid = new Dictionary<Vector2Int, List<ISpatialObject2d>>();
		}

		/// <inheritdoc/>
		public void AddObject(ISpatialObject2d obj)
		{
			Vector2Int minCell = WorldToGrid(obj.Position - Vector2.one * obj.Radius);
			Vector2Int maxCell = WorldToGrid(obj.Position + Vector2.one * obj.Radius);

			for (int x = minCell.x; x <= maxCell.x; x++)
			{
				for (int y = minCell.y; y <= maxCell.y; y++)
				{
					Vector2Int cellPos = new Vector2Int(x, y);
					if (!_grid.TryGetValue(cellPos, out List<ISpatialObject2d> cellObjects))
					{
						cellObjects = new List<ISpatialObject2d>();
						_grid[cellPos] = cellObjects;
					}

					cellObjects.Add(obj);
					obj.GridPos = cellPos;
				}
			}
		}

		/// <inheritdoc/>
		public ISpatialObject2d? FindClosestNeighbor(ISpatialObject2d current, List<ISpatialObject2d>? excludeObjects = null)
		{
			if (excludeObjects == null)
				excludeObjects = new();

			float closestDistance = float.MaxValue;
			ISpatialObject2d? closestNeighbor = null;

			Vector2Int minCell = WorldToGrid(current.Position - Vector2.one * current.Radius);
			Vector2Int maxCell = WorldToGrid(current.Position + Vector2.one * current.Radius);

			for (int x = minCell.x - 1; x <= maxCell.x + 1; x++) // Check neighboring cells as well.
			{
				for (int y = minCell.y - 1; y <= maxCell.y + 1; y++)
				{
					Vector2Int cellPos = new Vector2Int(x, y);
					if (_grid.ContainsKey(cellPos))
					{
						foreach (ISpatialObject2d obj in _grid[cellPos])
						{
							if (excludeObjects.Contains(obj) || obj == current) continue;

							float distance = (obj.Position - current.Position).sqrMagnitude;
							if (distance < closestDistance)
							{
								closestDistance = distance;
								closestNeighbor = obj;
							}
						}
					}
				}
			}

			return closestNeighbor;
		}

		/// <inheritdoc/>
		public List<ISpatialObject2d> FindObjectsInNeighbors(Vector2Int targetCell, bool includeTargetCell = false)
		{
			List<ISpatialObject2d> foundObjects = new();

			for (int x = targetCell.x - 1; x <= targetCell.x + 1; x++)
			{
				for (int y = targetCell.y - 1; y <= targetCell.y + 1; y++)
				{
					Vector2Int cellPos = new Vector2Int(x, y);
					if (_grid.ContainsKey(cellPos))
					{
						// Include objects from the current cell if specified.
						if (includeTargetCell || cellPos != targetCell)
							foundObjects.AddRange(_grid[cellPos]);
					}
				}
			}

			return foundObjects;
		}

		/// <summary>
		/// Finds the closest neighbor that does not overlap a line if connected to the current object, excluding objects in the exclusion list.
		/// </summary>
		/// <param name="current">Current object for which to find the closest non-overlapping neighbor.</param>
		/// <param name="excludeObjects">List of objects to exclude from the search.</param>
		/// <param name="lineThickness">Thickness (radius) of the line to be checked.</param>
		/// <returns>Closest non-overlapping neighbor object or null if none found.</returns>
		public ISpatialObject2d? FindClosestNonOverlappingNeighbor(ISpatialObject2d current, List<ISpatialObject2d> excludeObjects, float lineThickness)
		{
			float closestDistance = float.MaxValue;
			ISpatialObject2d? closestNeighbor = null;

			Vector2Int minCell = WorldToGrid(current.Position - Vector2.one * current.Radius);
			Vector2Int maxCell = WorldToGrid(current.Position + Vector2.one * current.Radius);

			for (int x = minCell.x - 1; x <= maxCell.x + 1; x++) // Check neighboring cells as well.
			{
				for (int y = minCell.y - 1; y <= maxCell.y + 1; y++)
				{
					Vector2Int cellPos = new Vector2Int(x, y);
					if (_grid.ContainsKey(cellPos))
					{
						foreach (ISpatialObject2d obj in _grid[cellPos])
						{
							if (excludeObjects.Contains(obj) || obj == current) continue;

							// Check if line between current and this object overlaps with other objects.
							if (!LineOverlapsObjects(current.Position, obj.Position, lineThickness, excludeObjects))
							{
								float distance = (obj.Position - current.Position).sqrMagnitude;
								if (distance < closestDistance)
								{
									closestDistance = distance;
									closestNeighbor = obj;
								}
							}
						}
					}
				}
			}

			return closestNeighbor;
		}

		/// <summary>
		/// Checks if a capsule defined by a start and end point overlaps with a circle.
		/// </summary>
		/// <param name="start">Start point of the capsule.</param>
		/// <param name="end">End point of the capsule.</param>
		/// <param name="radius">Radius of the capsule.</param>
		/// <param name="circlePos">Center position of the circle.</param>
		/// <param name="circleRadius">Radius of the circle.</param>
		/// <returns>True if the capsule overlaps with the circle; otherwise, false.</returns>
		private bool IsCapsuleOverlappingCircle(Vector2 start, Vector2 end, float radius, Vector2 circlePos, float circleRadius)
		{
			// Calculate the squared distance from the circle's position to the closest point on the line segment (capsule).
			float distSquared = DistancePointToLineSegmentSquared(circlePos, start, end);

			// Calculate the combined radius of the capsule and the circle.
			float combinedRadius = radius + circleRadius;

			// Check if the distance squared is less than or equal to the squared combined radius.
			// If true, the capsule overlaps with the circle.
			return distSquared <= combinedRadius * combinedRadius;
		}

		/// <summary>
		/// Calculates the squared distance from a point to a line segment.
		/// </summary>
		/// <param name="point">Point to measure the distance from.</param>
		/// <param name="lineStart">Start point of the line segment.</param>
		/// <param name="lineEnd">End point of the line segment.</param>
		/// <returns>Squared distance from the point to the closest point on the line segment.</returns>
		private float DistancePointToLineSegmentSquared(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
		{
			// Vector that points from lineStart to lineEnd.
			Vector2 lineVector = lineEnd - lineStart;
			Vector2 pointToLineStart = point - lineStart;

			// t represents a parameter that indicates the position along the line segment defined by lineStart and lineEnd.
			float t = Vector2.Dot(pointToLineStart, lineVector) / lineVector.sqrMagnitude;
			// Clamp t to [0, 1] to stay on the line segment.
			t = Mathf.Clamp01(t);

			// Calculate the closest point on the line segment from lineStart to lineEnd.
			// The parameter t represents the interpolation factor between 0 (lineStart) and 1 (lineEnd).
			Vector2 closestPoint = lineStart + t * lineVector; // Closest point on the line segment.

			return (point - closestPoint).sqrMagnitude;
		}

		/// <inheritdoc/>
		public bool LineOverlapsObjects(Vector2 start, Vector2 end, float thickness, List<ISpatialObject2d>? excludeObjects = null)
		{
			if (excludeObjects == null)
				excludeObjects = new();

			Vector2Int minCell = WorldToGrid(Vector2.Min(start, end) - Vector2.one * thickness);
			Vector2Int maxCell = WorldToGrid(Vector2.Max(start, end) + Vector2.one * thickness);

			// Iterate through all relevant cells.
			for (int x = minCell.x; x <= maxCell.x; x++)
			{
				for (int y = minCell.y; y <= maxCell.y; y++)
				{
					Vector2Int cellPos = new Vector2Int(x, y);
					if (_grid.ContainsKey(cellPos))
					{
						foreach (ISpatialObject2d obj in _grid[cellPos])
						{
							// Skip excluded objects.
							if (excludeObjects.Contains(obj)) continue;

							// Check if object overlaps the line (as a capsule).
							if (IsCapsuleOverlappingCircle(start, end, thickness, obj.Position, obj.Radius))
							{
								// Overlap found.
								return true;
							}
						}
					}
				}
			}

			// No overlaps found.
			return false;
		}

		/// <inheritdoc/>
		public bool OverlapsAnyObject(Vector2 position, float radius)
		{
			Vector2Int minCell = WorldToGrid(position - Vector2.one * radius);
			Vector2Int maxCell = WorldToGrid(position + Vector2.one * radius);

			for (int x = minCell.x; x <= maxCell.x; x++)
			{
				for (int y = minCell.y; y <= maxCell.y; y++)
				{
					Vector2Int cellPos = new(x, y);
					if (_grid.ContainsKey(cellPos))
					{
						foreach (ISpatialObject2d obj in _grid[cellPos])
						{
							// Check for overlap.
							float combinedRadius = radius + obj.Radius;
							if ((position - obj.Position).sqrMagnitude <= combinedRadius * combinedRadius)
							{
								// Overlap found.
								return true;
							}
						}
					}
				}
			}

			// No overlaps found.
			return false;
		}

		/// <inheritdoc/>
		public void RemoveObject(ISpatialObject2d obj)
		{
			// Convert to a list to avoid modifying the collection during iteration.
			foreach (var kvp in _grid.ToList())
			{
				if (kvp.Value.Remove(obj))
				{
					// If the list is empty after removal, remove the cell from the grid.
					if (kvp.Value.Count == 0)
						_grid.Remove(kvp.Key);

					// Early return after successfully removing the object.
					return;
				}
			}
		}

		/// <inheritdoc/>
		public Vector2Int WorldToGrid(Vector2 position)
		{
			int x = Mathf.FloorToInt(position.x / _cellSize);
			int y = Mathf.FloorToInt(position.y / _cellSize);

			return new Vector2Int(x, y);
		}
	}
}
