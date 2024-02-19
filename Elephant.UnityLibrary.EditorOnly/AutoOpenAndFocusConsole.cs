using System;
using System.Reflection;
using UnityEditor;

namespace Elephant.UnityLibrary.EditorOnly
{
	/// <summary>
	/// <para>Automatically show the console window when pressing play in the Editor.</para>
	/// <para>May be toggled through the menu near the top, Unity editor remembers this toggled state.</para>
	/// </summary>
	/// <remarks>This script MUST be placed in an Editor folder because it uses the UnityEditor namespace.</remarks>
	[InitializeOnLoad] // Ensure the script runs on editor load.
	public class AutoOpenAndFocusConsole
	{
		/// <summary>
		/// Menu configuration menu path.
		/// </summary>
		private const string MenuName = "Elephant/Auto focus console on play";

		/// <summary>
		/// Menu configuration editor preference key.
		/// </summary>
		private const string EditorPreferenceKeyIsEnabled = "AutoOpenAndFocusConsoleOnPlay";

		/// <summary>
		/// Menu configuration: whether is is enabled by default.
		/// </summary>
		private const bool IsEnabledByDefault = false;

		/// <summary>
		/// Constructor.
		/// </summary>
		static AutoOpenAndFocusConsole()
		{
			EditorApplication.playModeStateChanged += OnPlayModeChanged;
			EditorApplication.quitting += OnEditorQuitting;
		}

		/// <summary>
		/// Is called when <see cref="EditorApplication.playModeStateChanged"/> is invoked.
		/// </summary>
		private static void OnPlayModeChanged(PlayModeStateChange state)
		{
			// Only open when entering play mode.
			if (EditorPrefs.GetBool(EditorPreferenceKeyIsEnabled, IsEnabledByDefault) && state == PlayModeStateChange.EnteredPlayMode)
				ShowConsoleWindow();
		}

		/// <summary>
		/// Is called when the Unity Editor is shutting down.
		/// </summary>
		private static void OnEditorQuitting()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeChanged;
		}

		/// <summary>
		/// Menu for toggling the auto open and focus behaviour.
		/// </summary>
		[MenuItem(MenuName)]
		private static void ToggleAction()
		{
			bool currentState = EditorPrefs.GetBool(EditorPreferenceKeyIsEnabled, IsEnabledByDefault);
			EditorPrefs.SetBool(EditorPreferenceKeyIsEnabled, !currentState);
			Menu.SetChecked(MenuName, !currentState);
		}

		/// <summary>
		/// Show console window.
		/// </summary>
		private static void ShowConsoleWindow()
		{
			// Reflection to get internal Unity type for console window.
			Assembly assembly = Assembly.GetAssembly(typeof(EditorWindow));
			Type consoleWindowType = assembly.GetType("UnityEditor.ConsoleWindow");

			// Create a new console window if it's not already open and focus it.
			EditorWindow window = EditorWindow.GetWindow(consoleWindowType);
			window.Focus();
		}
	}
}