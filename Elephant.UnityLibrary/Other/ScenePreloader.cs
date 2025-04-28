#nullable enable

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// A generic class for preloading scenes to reduce user loading times. Example usage:
	/// ScenePreloader preloader = new ScenePreloader(this, "MySceneName");
	/// preloader.AllowSceneTransfer = true; // Set this whenever you're ready to switch scenes.
	/// </summary>
	public class ScenePreloader
	{
		/// <summary>
		/// If both the preloading is done and this variable is true, then the next scene will be loaded.
		/// </summary>
		public bool AllowSceneTransfer = false;

		/// <summary>Constructor.</summary>
		public ScenePreloader(MonoBehaviour anyMonoBehaviour, string sceneNameToLoad)
		{
			anyMonoBehaviour.StartCoroutine(LoadSceneCoroutine(sceneNameToLoad));
		}

		/// <summary>
		/// Load the specified scene asynchronously and waits until it is ready and the AllowSceneTransfer flag is true before activating it.
		/// </summary>
		/// <param name="sceneNameToLoad">Name of the scene to preload.</param>
		/// <returns>Coroutine enumerator.</returns>
		private IEnumerator LoadSceneCoroutine(string sceneNameToLoad)
		{
			yield return null;

			// Begin to load the Scene.
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneNameToLoad);

			// Don't let the Scene activate until allowSceneActivation is set to true.
			asyncOperation.allowSceneActivation = false;

			// Wait while the load is still in progress.
			while ((asyncOperation.progress < 0.9f) || !AllowSceneTransfer)
			{
				yield return null;
			}

			// Wait while the scene transfer is disallowed.
			while (!AllowSceneTransfer)
			{
				yield return null;
			}

			asyncOperation.allowSceneActivation = true;
		}
	}
}
