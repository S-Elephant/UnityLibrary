using UnityEngine;

namespace Elephant.UnityLibrary.Core
{
	/// <summary>
	/// Inherit from this base class to create a singleton.
	/// Example: <![CDATA[public class MyClassName : Singleton<MyClassName> {}]]>
	/// This script will not prevent non singleton constructors from being used in your derived classes. To prevent this, add a protected constructor to each derived class.
	/// When Unity quits it destroys objects in a random order and this can create issues for singletons. So we prevent access to the singleton instance when the application quits to prevent problems.
	/// </summary>
	public class SingletonNonPersistent<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Check to see if we're about to be destroyed.
		private static bool _shuttingDown = false;

		private static readonly object Lock = new object();

		/// <summary>
		/// Access singleton instance through this propriety.
		/// </summary>
		private static T _Instance;

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
					if (_Instance == null)
					{
						// Search for existing instance.
						_Instance = (T)FindObjectOfType(typeof(T));

						// Create new instance if one doesn't already exist.
						if (_Instance == null)
						{
							// Need to create a new GameObject to attach the singleton to.
							var singletonObject = new GameObject();
							_Instance = singletonObject.AddComponent<T>();
							singletonObject.name = typeof(T).ToString() + "_Singleton";
						}
					}

					return _Instance;
				}
			}
		}

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
	}
}
