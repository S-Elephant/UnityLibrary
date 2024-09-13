#nullable enable

using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// <see cref="Transform"/> extensions.
	/// </summary>
	public static class TransformExtensions
	{
		/// <summary>
		/// Sets the GameObject of <paramref name="transform"/> to inactive and then destroys it.
		/// Destroy() does not immediately remove the object; it schedules the
		/// object to be destroyed at the end of the current frame.
		/// If you want the object to be removed instantly, you can use DestroyImmediate(),
		/// though this is generally discouraged unless absolutely necessary.
		/// This extension method Deactivates the object and then schedules it to be destroyed.
		/// </summary>
		/// <param name="transform">Transform to deactivate and destroy.</param>
		/// <param name="t">Optional delay in seconds before destruction.</param>
		public static void DeactivateAndDestroy(this Transform transform, float t = 0f)
		{
			transform.gameObject.SetActive(false);
			Object.Destroy(transform.gameObject, t);
		}

		/// <summary>
		/// Destroys all children of the specified <paramref name="parent"/>.
		/// The parent itself remains untouched.
		/// </summary>
		/// <param name="parent">Parent Transform whose children will be destroyed.</param>
		/// <param name="t">Optional delay in seconds before destruction.</param>
		public static void DeactivateAndDestroyAllChildren(this Transform parent, float t = 0f)
		{
			for (int i = parent.childCount - 1; i >= 0; i--)
				parent.GetChild(i).gameObject.DeactivateAndDestroy(t);
		}

		/// <summary>
		/// Destroys all children of the specified <paramref name="parent"/>.
		/// The parent itself remains untouched.
		/// </summary>
		/// <param name="parent">Parent Transform whose children will be destroyed.</param>
		/// <param name="t">Optional delay in seconds before destruction.</param>
		public static void DestroyAllChildren(this Transform parent, float t = 0f)
		{
			for (int i = parent.childCount - 1; i >= 0; i--)
				GameObject.Destroy(parent.GetChild(i), t);
		}
	}
}
