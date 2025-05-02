using System;
using System.Collections.Generic;
using Elephant.UnityLibrary.Pathfinding.Interfaces;
using UnityEngine;

namespace Elephant.UnityLibrary.Pathfinding
{
	/// <summary>
	/// A Star (a.k.a. A*) 2D pathfinding.
	/// </summary>
	public class AStar2D : IAStar2D
	{
		/// <summary>
		/// Delegate for heuristic calculation.
		/// </summary>
		public Func<Vector2Int, Vector2Int, int> Heuristic { get; set; } = ManhattanHeuristic;

		/// <summary>
		/// Direction offsets.
		/// </summary>
		private static readonly Vector2Int[] DirectionOffsets =
		{
			new(0, 1),  // Up.
			new(0, -1), // Down.
			new(1, 0),  // Right.
			new(-1, 0), // Left.
		};

		/// <inheritdoc/>
		public List<Vector2Int> FindPath(List<List<IGridCell2D>> grid, int gridWidth, int gridHeight, Vector2Int start, Vector2Int destination, bool ignoreStartBlocked = true, bool ignoreDestinationBlocked = false)
		{
			// Early return if the destination is not free and we are not ignoring destination blockages.
			if (!ignoreDestinationBlocked && !grid[destination.x][destination.y].IsFree())
				return new List<Vector2Int>();

			// Create open and closed lists.
			List<Node> openList = new();
			HashSet<Node> closedList = new();

			// Add the start node (player's position) to the open list.
			Node startNode = new(start, null, 0, Heuristic(start, destination));
			openList.Add(startNode);

			// Loop until we find the path or run out of options.
			while (openList.Count > 0)
			{
				// Get the node with the lowest F cost.
				Node currentNode = GetLowestFCostNode(openList);

				// If we reached the destination, reconstruct the path.
				if (currentNode.Position == destination)
					return ReconstructPath(currentNode);

				// Move the current node from open to closed list.
				openList.Remove(currentNode);
				closedList.Add(currentNode);

				// Get the neighbors of the current node.
				List<Node> neighbors = GetNeighbors(currentNode, grid, gridWidth, gridHeight, destination, ignoreStartBlocked);

				foreach (Node neighbor in neighbors)
				{
					// If the neighbor is in the closed list, skip it.
					if (closedList.Contains(neighbor))
						continue;

					// If the neighbor is not in the open list, add it.
					if (!openList.Contains(neighbor))
					{
						openList.Add(neighbor);
					}
					else
					{
						// Check if this path to neighbor is shorter.
						Node existingNeighbor = openList.Find(n => n.Position == neighbor.Position);
						if (existingNeighbor.GCost > neighbor.GCost)
						{
							existingNeighbor.Parent = currentNode;
							existingNeighbor.GCost = neighbor.GCost;
							existingNeighbor.FCost = neighbor.FCost;
						}
					}
				}
			}

			// If we exit the loop, no path was found.
			return new List<Vector2Int>();
		}

		/// <summary>
		/// Retrieves the neighboring nodes of the given current node within the grid.
		/// </summary>
		/// <param name="currentNode">Node whose neighbors are to be found.</param>
		/// <param name="grid">Grid containing all the grid cells.</param>
		/// <param name="width">Width of the grid.</param>
		/// <param name="height">Height of the grid.</param>
		/// <param name="destination">Destination coordinates for pathfinding purposes.</param>
		/// <param name="ignorePlayerPositionBlocked">If true, will ignore the fact that the player position itself may be blocked.</param>
		/// <returns>List of neighboring nodes that can be traversed.</returns>
		private List<Node> GetNeighbors(Node currentNode, List<List<IGridCell2D>> grid, int width, int height, Vector2Int destination, bool ignorePlayerPositionBlocked)
		{
			List<Node> neighbors = new();
			foreach (Vector2Int direction in DirectionOffsets)
			{
				Vector2Int neighborPosition = currentNode.Position + direction;

				// Check if neighbor position is within grid bounds.
				if (neighborPosition.x >= 0 && neighborPosition.x < width && neighborPosition.y >= 0 && neighborPosition.y < height)
				{
					// Ensure the neighbor is free or the destination itself.
					if ((grid[neighborPosition.x][neighborPosition.y].IsFree() || neighborPosition == destination)
						|| (ignorePlayerPositionBlocked && neighborPosition == currentNode.Position))
					{
						int newGCost = currentNode.GCost + grid[neighborPosition.x][neighborPosition.y].MovementCost();
						int hCost = Heuristic(neighborPosition, destination);
						Node neighborNode = new(neighborPosition, currentNode, newGCost, newGCost + hCost);

						neighbors.Add(neighborNode);
					}
				}
			}

			return neighbors;
		}

		/// <summary>
		/// Default Manhattan heuristic.
		/// </summary>
		private static int ManhattanHeuristic(Vector2Int start, Vector2Int end)
		{
			return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
		}

		/// <summary>
		/// Finds the node with the lowest F cost in the given open list.
		/// </summary>
		/// <param name="openList">List of nodes to search.</param>
		/// <returns>Node with the lowest F cost.</returns>
		private static Node GetLowestFCostNode(List<Node> openList)
		{
			Node lowestFCostNode = openList[0];
			foreach (var node in openList)
			{
				if (node.FCost < lowestFCostNode.FCost)
					lowestFCostNode = node;
			}
			return lowestFCostNode;
		}

		/// <summary>
		/// Reconstructs the path from the end node to the start node by following parent nodes.
		/// </summary>
		/// <param name="endNode">Node representing the end of the path.</param>
		/// <returns>List of positions representing the path from start to destination.</returns>
		private static List<Vector2Int> ReconstructPath(Node endNode)
		{
			List<Vector2Int> path = new();
			Node currentNode = endNode;

			while (currentNode != null)
			{
				path.Add(currentNode.Position);
				currentNode = currentNode.Parent;
			}

			// Reverse the path to get it from start to destination.
			path.Reverse();

			return path;
		}

		/// <summary>
		/// Represents a node in the grid for pathfinding purposes.
		/// </summary>
		private class Node
		{
			/// <summary>
			/// Position of the node in the grid.
			/// </summary>
			public Vector2Int Position;

			/// <summary>
			/// Parent node from which this node was reached.
			/// </summary>
			public Node Parent;

			/// <summary>
			/// Cost from the start node to this node.
			/// </summary>
			public int GCost;

			/// <summary>
			/// Total cost (GCost + Heuristic) for this node.
			/// </summary>
			public int FCost;

			/// <summary>
			/// Constructor.
			/// </summary>
			public Node(Vector2Int position, Node parent, int gCost, int fCost)
			{
				Position = position;
				Parent = parent;
				GCost = gCost;
				FCost = fCost;
			}

			/// <summary>
			/// Override Equals and GetHashCode to use Node in HashSet
			/// </summary>
			public override bool Equals(object obj)
			{
				if (obj == null || !(obj is Node))
					return false;
				return Position == ((Node)obj).Position;
			}

			/// <summary>
			/// Serves as the default hash function.
			/// </summary>
			/// <returns>Hash code for the current node.</returns>
			public override int GetHashCode()
			{
				return Position.GetHashCode();
			}
		}
	}
}
