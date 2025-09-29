using UnityEngine;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Provides <see cref="Gizmos"/> utility methods.
	/// </summary>
	public static class GizmosUtils
	{
		/// <summary>
		/// Renders a 2D rectangle using <see cref="Gizmos"/>.
		/// Does not render invalid <paramref name="rect"/>s.
		/// </summary>
		/// <param name="rect"><see cref="Rect"/> to draw in world coordinates.</param>
		/// <param name="color">Color to use for drawing the rectangle outline. The original <see cref="Gizmos.color"/> is restored after rendering the <paramref name="rect"/>.</param>
		/// <param name="z">Z-coordinate to draw the <paramref name="rect"/> onto. Defaults to 0f.</param>
		/// <remarks>
		/// This method is intended for use in OnDrawGizmos() or OnDrawGizmosSelected() methods.
		/// The rectangle is drawn at Z = 0 in world space. For 3D positioning, adjust the Rect values accordingly.
		/// The rectangle is drawn as an outline using four separate line segments.
		/// </remarks>
		public static void DrawRectangle2D(Rect rect, Color color, float z = 0f)
		{
			// Don't draw an invalid rectangle.
			if (rect.width <= float.Epsilon || rect.height <= float.Epsilon)
				return;

			Color originalColor = Gizmos.color;
			Gizmos.color = color;

			// Calculate corners.
			Vector3 bottomLeft = new(rect.x, rect.y, z);
			Vector3 bottomRight = new(rect.x + rect.width, rect.y, z);
			Vector3 topRight = new(rect.x + rect.width, rect.y + rect.height, z);
			Vector3 topLeft = new(rect.x, rect.y + rect.height, z);

			// Draw outline.
			Gizmos.DrawLine(bottomLeft, bottomRight);
			Gizmos.DrawLine(bottomRight, topRight);
			Gizmos.DrawLine(topRight, topLeft);
			Gizmos.DrawLine(topLeft, bottomLeft);

			Gizmos.color = originalColor;
		}

		/// <inheritdoc cref="DrawRectangle2D(Rect, Color, float)"/>
		public static void DrawRectangle2D(Rect? rect, Color color, float z = 0f)
		{
			// Don't draw null.
			if (rect == null)
				return;

			DrawRectangle2D(rect.Value, color, z);
		}
	}
}
