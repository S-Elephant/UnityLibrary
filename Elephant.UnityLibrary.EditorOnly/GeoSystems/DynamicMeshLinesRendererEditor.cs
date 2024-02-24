using Elephant.UnityLibrary.GeoSystems.Renderers;
using UnityEditor;
using UnityEngine;

namespace Elephant.UnityLibrary.EditorOnly.GeoSystems
{
	/// <summary>
	/// Custom Unity Editor script for <see cref="DynamicMeshLinesRenderer"/>.
	/// </summary>
	[CustomEditor(typeof(DynamicMeshLinesRenderer))]
	public class DynamicMeshLinesRendererEditor : Editor
	{
		/// <inheritdoc/>
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			DynamicMeshLinesRenderer script = (DynamicMeshLinesRenderer)target;

			if (GUILayout.Button("Refresh"))
				script.Refresh();
		}
	}
}
