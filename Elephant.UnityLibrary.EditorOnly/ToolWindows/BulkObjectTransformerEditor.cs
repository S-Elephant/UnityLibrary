using UnityEditor;
using UnityEngine;

namespace Elephant.UnityLibrary.EditorOnly.ToolWindows
{
	/// <summary>
	/// Apply bulk tranfsormation operations of all selected objects.
	/// </summary>
	internal class BulkObjectTransformerEditor : EditorWindow
	{
		/// <summary>
		/// Menu item path prefix.
		/// </summary>
		private const string MenuItemPrefix = SettingsEditor.MenuPrefix + "/Windows/";

		/// <summary>
		/// Menu window caption.
		/// </summary>
		private const string MenuItemTitle = "Bulk Object Transformer";

		/// <summary>
		/// Offset to move the selected objects by.
		/// </summary>
		private Vector3 _moveOffset = Vector3.zero;

		/// <summary>
		/// Offset to rotate the selected objects by (in Euler angles).
		/// </summary>
		private Vector3 _rotationOffset = Vector3.zero;

		/// <summary>
		/// Offset to scale the selected objects by.
		/// </summary>
		private Vector3 _scaleOffset = Vector3.zero;

		/// <summary>
		/// Multiplier value to scale the selected objects by.
		/// </summary>
		private Vector3 _scaleMultiplier = Vector3.one;

		/// <summary>
		/// Opens this tool window.
		/// </summary>
		[MenuItem(MenuItemPrefix + MenuItemTitle)]
		private static void ShowWindow()
		{
			GetWindow<BulkObjectTransformerEditor>(MenuItemTitle);
		}

		/// <summary>
		/// Displays the actual GUI window.
		/// </summary>
		private void OnGUI()
		{
			// Display the number of selected objects.
			GUILayout.Label($"Selected Objects: {Selection.gameObjects.Length}", EditorStyles.boldLabel);

			// Input for moving objects.
			_moveOffset = EditorGUILayout.Vector3Field("Move Offset", _moveOffset);

			// Input for rotating objects.
			_rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset (Euler)", _rotationOffset);

			// Input for scaling objects.
			_scaleOffset = EditorGUILayout.Vector3Field("Scale Offset (is applied before multiplier)", _scaleOffset);

			// Input for scaling objects. A scale of 1 will do nothing.
			_scaleMultiplier = EditorGUILayout.Vector3Field("Scale Multiplier", _scaleMultiplier);

			// Add blank space.
			GUILayout.Space(12);

			// Apply button.
			if (GUILayout.Button("Apply Transformation(s)"))
				ApplyTransformations();

			// Reset button.
			if (GUILayout.Button("Reset Input Values"))
				ResetFields();
		}

		/// <summary>
		/// Applies the transformation offsets and multipliers to the selected objects.
		/// </summary>
		private void ApplyTransformations()
		{
			foreach (GameObject obj in Selection.gameObjects)
			{
				// Apply move offset.
				obj.transform.position += _moveOffset;

				// Apply rotation offset (Euler angles).
				obj.transform.rotation *= Quaternion.Euler(_rotationOffset);

				// Apply scale offset.
				obj.transform.localScale += _scaleOffset;

				// Apply scale multiplier.
				Vector3 currentScale = obj.transform.localScale;
				obj.transform.localScale = new Vector3(
					currentScale.x * _scaleMultiplier.x,
					currentScale.y * _scaleMultiplier.y,
					currentScale.z * _scaleMultiplier.z);
			}
		}

		/// <summary>
		/// Resets the transformation fields to their default values.
		/// </summary>
		private void ResetFields()
		{
			_moveOffset = Vector3.zero;
			_rotationOffset = Vector3.zero;
			_scaleOffset = Vector3.zero;
			_scaleMultiplier = Vector3.one;

			// This repaint fixes the problem where an input field was not reset when the user was editing while pressing the reset-button.
			EditorGUI.FocusTextInControl(null);
		}
	}
}
