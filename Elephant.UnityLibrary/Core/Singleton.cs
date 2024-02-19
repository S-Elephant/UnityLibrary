using UnityEngine;

namespace Elephant.UnityLibrary.Core
{
	/// <summary>
	/// Inherit from this base class to create a singleton.
	/// Example: public class MyClassName : Singleton<MyClassName> {}
	/// This script will not prevent non singleton constructors from being used in your derived classes. To prevent this, add a protected constructor to each derived class.
	/// When Unity quits it destroys objects in a random order and this can create issues for singletons. So we prevent access to the singleton instance when the application quits to prevent problems.
	/// </summary>
	public class Singleton<T> : MonoBehaviour
		where T : MonoBehaviour
	{
		// Check to see if we're about to be destroyed.
		private static bool _shuttingDown = false;

		private static readonly object Lock = new();

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		private static T _instance;

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (_shuttingDown)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
					return null;
				}

				lock (Lock)
				{
					if (_instance == null)
					{
						// Search for existing instance.
						_instance = (T)FindObjectOfType(typeof(T));

						// Create new instance if one doesn't already exist.
						if (_instance == null)
						{
							// Need to create a new GameObject to attach the singleton to.
							GameObject singletonObject = new GameObject();
							_instance = singletonObject.AddComponent<T>();
							singletonObject.name = typeof(T).ToString() + "_Singleton";

							// Make instance persistent.
							DontDestroyOnLoad(singletonObject);
						}
					}

					return _instance;
				}
			}
		}

#pragma warning disable S2696

		/// <summary>
		/// Application quit.
		/// </summary>
		private void OnApplicationQuit()
		{
			_shuttingDown = true;
		}

		/// <summary>
		/// Destroy.
		/// </summary>
		private void OnDestroy()
		{
			_shuttingDown = true;
		}

#pragma warning restore S2696
	}
}
