using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// <see cref="Texture2D"/> extensions.
	/// </summary>
	public static class Texture2DExtensions
	{
		/// <summary>
		/// Create and return a new <see cref="Sprite"/> from a <see cref="Texture2D"/>.
		/// </summary>
		public static Sprite ToSprite(this Texture2D texture2D, float pivotX = 0.5f, float pivotY = 0.5f, float pixelsPerUnit = 100f, uint extrude = 0)
		{
			return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(pivotX, pivotY), pixelsPerUnit, extrude);
		}
	}
}
