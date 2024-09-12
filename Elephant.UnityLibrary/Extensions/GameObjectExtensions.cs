using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// Destroy() does not immediately remove the object; it schedules the
	/// object to be destroyed at the end of the current frame.
	/// If you want the object to be removed instantly, you can use DestroyImmediate(),
	/// though this is generally discouraged unless absolutely necessary.
	/// This extension method Deactivates the object and then schedules it to be destroyed.
	/// </summary>
	public static class GameObjectExtensions
	{
		/// <summary>
		/// Sets <paramref name="gameObject"/> to inactive and then destroys it.
		/// </summary>
		/// <param name="gameObject">GameObject to deactivate and destroy.</param>
		/// <param name="t">Optional delay in seconds before destruction.</param>
		public static void DeactivateAndDestroy(this GameObject gameObject, float t = 0f)
		{
			gameObject.SetActive(false);
			Object.Destroy(gameObject, t);
		}
	}
}