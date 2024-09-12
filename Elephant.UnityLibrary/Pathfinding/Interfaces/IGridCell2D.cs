using UnityEngine;

namespace Elephant.UnityLibrary.Pathfinding
{
	/// <summary>
	/// Interface representing a cell in the grid for pathfinding.
	/// A higher x means more to the right/east and a higher Y means more upwards/north,
	/// a left-handed Y-up coordinate system.
	/// </summary>
	public interface IGridCell2D
	{
		/// <summary>
		/// Returns the movement cost for this cell.
		/// </summary>
		/// <remarks>This value is usually: 1.</remarks>
		int MovementCost();

		/// <summary>
		/// Checks if the cell is free (i.e., not blocked or occupied).
		/// </summary>
		/// <returns>True if the cell is free, false otherwise.</returns>
		bool IsFree();

		/// <summary>
		/// Gets the position of the cell in the grid.
		/// </summary>
		/// <returns>Position of the cell as a Vector2Int.</returns>
		Vector2Int GetPosition();

		/// <summary>
		/// Sets the cell as free or occupied.
		/// </summary>
		/// <param name="isFree">True if the cell should be free, false if occupied.</param>
		void SetFree(bool isFree);
	}
}
