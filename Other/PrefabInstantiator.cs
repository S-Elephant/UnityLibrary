#nullable enable
using UnityEngine;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Prefab utilities.
	/// </summary>
	public static class PrefabInstantiator
	{
		/// <summary>
		/// Instantiates a new prefab and returns the component on it (it must be located on the root component).
		/// </summary>
		/// <typeparam name="TComponent">Component to retrieve on the root of the instantiated prefab.</typeparam>
		/// <param name="original">Prefab reference to instantiate.</param>
		/// <param name="parent">Parent <see cref="Transform"/> to spawn the new prefab onto.</param>
		/// <returns>Found component.</returns>
		public static TComponent Instantiate<TComponent>(GameObject original, Transform parent)
			where TComponent : Component
		{
			GameObject instance = Object.Instantiate(original, parent);
			return instance.GetComponent<TComponent>();
		}

		/// <summary>
		/// Instantiates a new prefab and returns the component on it (it must be located on the root component).
		/// </summary>
		/// <typeparam name="TComponent">Component to retrieve on the root of the instantiated prefab.</typeparam>
		/// <param name="original">Prefab reference to instantiate.</param>
		/// <returns>Found component.</returns>
		public static TComponent Instantiate<TComponent>(GameObject original)
			where TComponent : Component
		{
			GameObject instance = Object.Instantiate(original);
			return instance.GetComponent<TComponent>();
		}

		/// <summary>
		/// Instantiates a new prefab and returns the component on it (it must be located on the root component).
		/// </summary>
		/// <typeparam name="TComponent">Component to retrieve on the root of the instantiated prefab.</typeparam>
		/// <param name="original">Prefab reference to instantiate.</param>
		/// <param name="parent">Parent <see cref="Transform"/> to spawn the new prefab onto.</param>
		/// <param name="worldPositionStays">When you assign a parent <see cref="Transform"/>, pass true to position the new object directly in world space. Pass false to set the Object’s position relative to its new parent.</param>
		/// <param name="position">Optional position of new prefab after it has been assigned a parent.</param>
		/// <param name="rotation">Optional rotation of new prefab after it has been assigned a parent.</param>
		/// <returns>Found component.</returns>
		public static TComponent Instantiate<TComponent>(GameObject original, Transform parent, bool worldPositionStays, Vector3? position = null, Quaternion? rotation = null)
			where TComponent : Component
		{
			GameObject instance = Object.Instantiate(original, parent, worldPositionStays);

			if (position != null)
				instance.transform.position = (Vector3)position;
			if (rotation != null)
				instance.transform.rotation = (Quaternion)rotation;

			return instance.GetComponent<TComponent>();
		}

		/// <summary>
		/// Instantiates a new prefab and returns the component on it (it must be located on the root component).
		/// </summary>
		/// <typeparam name="TComponent">Component to retrieve on the root of the instantiated prefab.</typeparam>
		/// <param name="original">Prefab reference to instantiate.</param>
		/// <param name="position">Optional position of new prefab.</param>
		/// <param name="rotation">Optional rotation of new prefab.</param>
		/// <returns>Found component.</returns>
		public static TComponent Instantiate<TComponent>(GameObject original, Vector3 position, Quaternion? rotation = null)
			where TComponent : Component
		{
			GameObject instance = Object.Instantiate(original);

			instance.transform.position = position;
			if (rotation != null)
				instance.transform.rotation = (Quaternion)rotation;

			return instance.GetComponent<TComponent>();
		}

		/// <summary>
		/// Instantiates a new prefab and returns the component on it or on one of its children.
		/// </summary>
		/// <typeparam name="TComponent">Component to retrieve on the root of the instantiated prefab or on one of its children.</typeparam>
		/// <param name="original">Prefab reference to instantiate.</param>
		/// <param name="parent">Parent <see cref="Transform"/> to spawn the new prefab onto.</param>
		/// <returns>Found component.</returns>
		public static TComponent InstantiateGetInChildren<TComponent>(GameObject original, Transform parent)
			where TComponent : Component
		{
			GameObject instance = Object.Instantiate(original, parent);
			return instance.GetComponentInChildren<TComponent>();
		}

		/// <summary>
		/// Instantiates a new prefab and returns the component on it or on one of its children.
		/// </summary>
		/// <typeparam name="TComponent">Component to retrieve on the root of the instantiated prefab or on one of its children.</typeparam>
		/// <param name="original">Prefab reference to instantiate.</param>
		/// <returns>Found component.</returns>
		public static TComponent InstantiateGetInChildren<TComponent>(GameObject original)
			where TComponent : Component
		{
			GameObject instance = Object.Instantiate(original);
			return instance.GetComponentInChildren<TComponent>();
		}

		/// <summary>
		/// Instantiates a new prefab and returns the component on it or on one of its children.
		/// </summary>
		/// <typeparam name="TComponent">Component to retrieve on the root of the instantiated prefab or on one of its children.</typeparam>
		/// <param name="original">Prefab reference to instantiate.</param>
		/// <param name="parent">Parent <see cref="Transform"/> to spawn the new prefab onto.</param>
		/// <param name="worldPositionStays">When you assign a parent <see cref="Transform"/>, pass true to position the new object directly in world space. Pass false to set the Object’s position relative to its new parent.</param>
		/// <param name="position">Optional position of new prefab after it has been assigned a parent.</param>
		/// <param name="rotation">Optional rotation of new prefab after it has been assigned a parent.</param>
		/// <returns>Found component.</returns>
		public static TComponent InstantiateGetInChildren<TComponent>(GameObject original, Transform parent, bool worldPositionStays, Vector3? position = null, Quaternion? rotation = null)
			where TComponent : Component
		{
			GameObject instance = Object.Instantiate(original, parent, worldPositionStays);

			if (position != null)
				instance.transform.position = (Vector3)position;
			if (rotation != null)
				instance.transform.rotation = (Quaternion)rotation;

			return instance.GetComponentInChildren<TComponent>();
		}

		/// <summary>
		/// Instantiates a new prefab and returns the component on it or on one of its children.
		/// </summary>
		/// <typeparam name="TComponent">Component to retrieve on the root of the instantiated prefab or on one of its children.</typeparam>
		/// <param name="original">Prefab reference to instantiate.</param>
		/// <param name="position">Optional position of new prefab.</param>
		/// <param name="rotation">Optional rotation of new prefab.</param>
		/// <returns>Found component.</returns>
		public static TComponent InstantiateGetInChildren<TComponent>(GameObject original, Vector3 position, Quaternion? rotation = null)
			where TComponent : Component
		{
			GameObject instance = Object.Instantiate(original);

			instance.transform.position = position;
			if (rotation != null)
				instance.transform.rotation = (Quaternion)rotation;

			return instance.GetComponentInChildren<TComponent>();
		}
	}
}
