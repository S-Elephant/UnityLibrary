using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// Miscellaneous extensions.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Returns the orthographic camera bounds in world space.
		/// </summary>
		public static Bounds OrthographicBounds(this Camera camera)
		{
			float screenAspect = (float)Screen.width / (float)Screen.height;
			float cameraHeight = camera.orthographicSize * 2;
			Bounds bounds = new Bounds(
				camera.transform.position,
				new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
			return bounds;
		}

		/// <summary>
		/// Searches recursively for a child transform with the specified name within the descendants of the given parent transform.
		/// </summary>
		/// <param name="parent">Parent transform from which to start the search.</param>
		/// <param name="name">Name of the child transform to search for.</param>
		/// <returns>First Transform found that matches the specified name. If no transform is found, returns null.</returns>
		/// <remarks>
		/// Performs a depth-first search to find the first child transform that matches the given name.
		/// If the child transform is not found directly under the given parent, the search continues recursively among all descendants.
		/// </remarks>
		public static Transform FindRecursively(this Transform parent, string name)
		{
			foreach (Transform child in parent)
			{
				if (child.name == name)
				{
					return child;
				}

				Transform result = child.FindRecursively(name);
				if (result != null)
				{
					return result;
				}
			}

			return null;
		}

		/// <summary>
		/// Sets the layer of the game object and all its children recursively.
		/// </summary>
		/// <param name="gameObject"><see cref="GameObject"/> and all of its children to set the layers for. Cannot be null.</param>
		/// <param name="targetLayer">
		/// Target layer to set (between 0 to 31, inclusive). This is the same integer value as the one shown in Unity's
		/// inspector "Tags and Layers" or through "Project Settings... > Tags and Layers"
		/// </param>
		public static void SetLayerRecursively(this GameObject gameObject, int targetLayer)
		{
			gameObject.layer = targetLayer;

			foreach (Transform child in gameObject.transform)
				SetLayerRecursively(child.gameObject, targetLayer);
		}
	}
}
