#nullable enable

using UnityEngine;

namespace Elephant.UnityLibrary.Maths
{
	/// <summary>
	/// Provides utility methods for geometric calculations involving rectangles.
	/// </summary>
	public static class RectGeometry
	{
		/// <summary>
		/// Calculates the maximum number of <paramref name="rectSize"/> of a given size that can fit inside the specified <paramref name="containerRect"/> without overlapping and without rotating.
		/// </summary>
		/// <param name="containerRect">Dimensions of the container rectangle.</param>
		/// <param name="rectSize">Dimensions of the square to fit inside the container.</param>
		/// <returns>Maximum number of squares that can fit inside the container.</returns>
		public static int CalcMaxRectsInRect(Rect containerRect, Rect rectSize)
		{
			if (rectSize.width <= 0 || rectSize.height <= 0)
				return -1; // Square dimensions must be greater than zero but got {rectSize.width}x{rectSize.height}. Returning -1.

			int itemsX = Mathf.FloorToInt(containerRect.width / rectSize.width);
			int itemsY = Mathf.FloorToInt(containerRect.height / rectSize.height);

			return itemsX * itemsY;
		}
	}
}