#nullable enable

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elephant.UnityLibrary.EditorOnly
{
	/// <summary>
	/// Ensures that the Unity Editor always starts playing from a designated start scene,
	/// and returns to it after the play mode ends.
	/// </summary>
	/// <remarks>[InitializeOnLoad] is applied and initializes this class as soon as the project is loaded.</remarks>
	[InitializeOnLoad]
	public class PlayFromStartupScene
	{
		/// <summary>
		/// Prefix menu item path for all menu items of this script.
		/// </summary>
		private const string PlayFromStartSceneMenuItemPrefix = SettingsEditor.MenuPrefix + "Play from start scene/";

		/// <summary>
		/// Toggle menu item path.
		/// </summary>
		private const string MenuItemToggle = PlayFromStartSceneMenuItemPrefix + "Is enabled";

		/// <summary>
		/// "Set start scene" menu item path.
		/// </summary>
		private const string MenuItemSetStartScene = PlayFromStartSceneMenuItemPrefix + "Set start scene";

		/// <summary>
		/// <see cref="_isEnabled"/> <see cref="EditorPrefs"/> key.
		/// </summary>
		private const string PrefIsEnabled = "Elephant_PlayFromStartScene_IsEnabled";

		/// <summary>
		/// <see cref="StartScenePath"/> <see cref="EditorPrefs"/> key.
		/// </summary>
		private const string PrefStartScene = "Elephant_PlayFromStartScene_StartScenePath";

		/// <summary>
		/// <see cref="EditorScene"/> <see cref="EditorPrefs"/> key.
		/// </summary>
		/// <remarks>
		/// I'm using this instead of a private static string because my version of Unity
		/// currently contains a bug regarding the static.
		/// </remarks>
		private const string PrefEditorScene = "Elephant_PlayFromStartScene_EditorScenePath";

		/// <summary>
		/// Path to the start scene. Update this path according to your start scene location in the Assets folder.
		/// </summary>
		private static string? StartScenePath => EditorPrefs.GetString(PrefStartScene, null);

		/// <summary>
		/// Determines if this script is enabled when switching PlayModes.
		/// </summary>
		private static bool _isEnabled = false;

		/// <summary>
		/// Path to the last active scene before entering play mode.
		/// </summary>
		/// <remarks>
		/// Uses <see cref="EditorPrefs"/> instead of a private static string because my version of Unity
		/// currently contains a bug regarding the static.
		/// </remarks>
		private static string? EditorScene
		{
			get => EditorPrefs.GetString(PrefEditorScene, null);
			set => EditorPrefs.SetString(PrefEditorScene, value);
		}

		/// <summary>
		/// Static constructor to subscribe to the <see cref="EditorApplication.playModeStateChanged"/> event.
		/// </summary>
		static PlayFromStartupScene()
		{
			// Update checked state.
			_isEnabled = EditorPrefs.GetBool(PrefIsEnabled, false);
			Menu.SetChecked(MenuItemToggle, _isEnabled);

			// Hook.
			EditorApplication.playModeStateChanged += OnPlayModeChanged;
		}

		/// <summary>
		/// Deconstructor.
		/// </summary>
		~PlayFromStartupScene()
		{
			try
			{
				// Unhook.
				EditorApplication.playModeStateChanged -= OnPlayModeChanged;
			}
			catch
			{
				// Do nothing.
			}
		}

		#region Menu items

		/// <summary>
		/// Toggles <see cref="_isEnabled"/> and the related menu item. Result is saved into <see cref="EditorPrefs"/>.
		/// </summary>
		[MenuItem(MenuItemToggle)]
		private static void ToggleEnable()
		{
			_isEnabled = !_isEnabled;
			EditorPrefs.SetBool(PrefIsEnabled, _isEnabled);
			Menu.SetChecked(MenuItemToggle, _isEnabled);
		}

		/// <summary>
		/// Menu item for selecting the starting scene.
		/// </summary>
		[MenuItem(MenuItemSetStartScene)]
		private static void SetStartScene()
		{
			// Open a dialog to choose the scene.
			string scene = EditorUtility.OpenFilePanel("Select start scene", "Assets", "unity");
			if (!string.IsNullOrEmpty(scene))
			{
				// Convert the full path into a relative path and store it in EditorPrefs.
				EditorPrefs.SetString(PrefStartScene, scene.Replace(Application.dataPath, "Assets"));
			}
		}

		#endregion

		/// <summary>
		/// Called when the play mode state changes. This method handles the switching of scenes
		/// when entering play mode and when returning to edit mode.
		/// </summary>
		/// <param name="state">The current state of the play mode change.</param>
		private static void OnPlayModeChanged(PlayModeStateChange state)
		{
			if (!_isEnabled)
				return;

			switch (state)
			{
				case PlayModeStateChange.ExitingEditMode:
					// Save the current scene path before playing.
					EditorScene = SceneManager.GetActiveScene().path;

					// Save the current scene before playing.
					EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

					// If not already in the start scene, switch to it.
					if (SceneManager.GetActiveScene().path != StartScenePath)
						EditorSceneManager.OpenScene(StartScenePath);
					break;

				case PlayModeStateChange.EnteredEditMode:
					if (!string.IsNullOrEmpty(EditorScene))
					{
						if (SceneManager.GetActiveScene().path != EditorScene)
						{
							// Return to the scene that was last active before entering play mode.
							EditorSceneManager.OpenScene(EditorScene);
						}
					}
					else
					{
						Debug.LogWarning("Unable to return to the original editor scene because it's either null or empty or no longer exists.");
					}

					break;
			}
		}
	}
}