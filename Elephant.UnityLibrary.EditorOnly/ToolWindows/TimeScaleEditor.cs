using UnityEditor;
using UnityEngine;

namespace Elephant.UnityLibrary.EditorOnly.ToolWindows
{
	/// <summary>
	/// Custom Editor window to control the time scale during Play Mode.
	/// </summary>
	public class TimeScaleEditor : EditorWindow
	{
		/// <summary>
		/// Menu item path prefix.
		/// </summary>
		private const string MenuItemPrefix = SettingsEditor.MenuPrefix + "/Windows/";

		/// <summary>
		/// Current time scale value.
		/// </summary>
		private float _timeScale = 1f; // Default time scale.

		/// <summary>
		/// Display the Time Scale Editor window.
		/// </summary>
		[MenuItem(MenuItemPrefix + "Time Scale Editor")]
		public static void ShowWindow()
		{
			GetWindow<TimeScaleEditor>("Time Scale Editor");
		}

		/// <summary>
		/// Render the GUI for the Time Scale Editor window.
		/// </summary>
		private void OnGUI()
		{
			// Sync _timeScale (because it could have been changed by some other code).
			if (Application.isPlaying)
				_timeScale = Time.timeScale;

			GUILayout.Label("Time Scale Controller", EditorStyles.boldLabel);

			// Slider to adjust the time scale.
			_timeScale = EditorGUILayout.Slider("Time Scale", _timeScale, 0f, 10f);

			if (Application.isPlaying)
				Time.timeScale = _timeScale;
			else
				EditorGUILayout.HelpBox("Time scale changes are only applied in Play Mode.", MessageType.Warning);

			// Reset button.
			if (GUILayout.Button("Reset Time Scale"))
			{
				_timeScale = 1f;
				if (Application.isPlaying)
					Time.timeScale = 1f;
			}
		}
	}
}