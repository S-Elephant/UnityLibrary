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

		/// <summary>
		/// Finds the first child Transform by name.
		/// </summary>
		/// <param name="parent">Parent Transform.</param>
		/// <param name="childName">Name of the child GameObject to find.</param>
		/// <param name="findRecursive">Optional: whether to search recursively through all children.</param>
		/// <param name="isCaseSensitive">Optional: whether the name search should be case sensitive.</param>
		/// <param name="includeInactive">Optional: whether to include inactive Transforms in the search.</param>
		/// <returns>First child Transform if found or null if not found.</returns>
		public static Transform? FirstChildByName(this Transform parent, string childName, bool findRecursive = false, bool isCaseSensitive = true, bool includeInactive = true)
		{
			if (childName == string.Empty)
				return null;

			foreach (Transform child in parent)
			{
				if (child.gameObject.activeSelf || includeInactive)
				{
					if ((isCaseSensitive && child.name == childName) ||
						(!isCaseSensitive && child.name.Equals(childName, System.StringComparison.OrdinalIgnoreCase)))
					{
						return child;
					}

					if (findRecursive)
					{
						Transform? foundChild = FirstChildByName(child, childName, true, isCaseSensitive, includeInactive);
						if (foundChild != null)
							return foundChild;
					}
				}
			}

			// No match found.
			return null;
		}
	}
}
