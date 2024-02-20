using System.Linq;
using Elephant.UnityLibrary.Other;
using UnityEditor;
using UnityEngine;

namespace Elephant.UnityLibrary.EditorOnly
{
	/// <summary>
	/// Custom <see cref="CoroutineStarter"/> editor for Unity's inspector.
	/// </summary>
	[CustomEditor(typeof(CoroutineStarter))]
	public class CoroutineStarterEditor : UnityEditor.Editor
	{
		/// <summary>
		/// The target script that is shown in the inspector.
		/// </summary>
		private CoroutineStarter _coroutineStarter = null!;

		/// <summary>
		/// This method is called when the object is loaded.
		/// </summary>
#pragma warning disable IDE0051 // Ignored because OnDestroy is a Unity event.
		private void OnEnable()
#pragma warning restore IDE0051
		{
			_coroutineStarter = (CoroutineStarter)target;
			_coroutineStarter.OnStartTrackingCoroutine += RepaintFromEvent;
			_coroutineStarter.OnStopTrackingCoroutine += RepaintFromEvent;
		}

		/// <summary>
		/// This method is called when the scriptable object goes out of scope.
		/// </summary>
#pragma warning disable IDE0051 // Ignored because OnDestroy is a Unity event.
		private void OnDisable()
#pragma warning restore IDE0051
		{
			_coroutineStarter.OnStartTrackingCoroutine -= RepaintFromEvent;
			_coroutineStarter.OnStopTrackingCoroutine -= RepaintFromEvent;
		}

		/// <inheritdoc/>
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.LabelField($"Total tracked coroutines: {CoroutineStarter.TotalTrackedCoroutinesStarted - CoroutineStarter.TotalTrackedCoroutinesStopped}");
			EditorGUILayout.LabelField($"Total tracked coroutines started: {CoroutineStarter.TotalTrackedCoroutinesStarted}");
			EditorGUILayout.LabelField($"Total tracked coroutines stopped/untracked: {CoroutineStarter.TotalTrackedCoroutinesStopped}");

			// Group by MainCategory, order by MainCategory, then by SubCategory, then by Coroutine name.
			var groupedData = _coroutineStarter.CoroutineData
				.GroupBy(kvp => kvp.Value.MainCategory)
				.OrderBy(group => group.Key) // Order by MainCategory.
				.ThenBy(group => group.Select(x => x.Value.SubCategory))
				.ThenBy(group => group.Select(x => x.Key)) // Order by coroutine name.
				.ToDictionary(g => g.Key, g => g.Select(kvp => kvp).ToList());

			if (GUILayout.Button("Kill all tracked"))
				_coroutineStarter.StopAllTrackedCoroutines();

			if (GUILayout.Button("Kill all globally"))
				_coroutineStarter.StopAllCoroutines();

			if (GUILayout.Button("Refresh this inspector window"))
				Repaint();

			foreach (var groupedDataKvp in groupedData)
			{
				EditorGUILayout.LabelField(groupedDataKvp.Key);
				foreach (var coroutineValueKvp in groupedDataKvp.Value)
				{
					EditorGUILayout.BeginHorizontal();
					GUI.enabled = false;
					EditorGUILayout.TextField($"{coroutineValueKvp.Value.CallerName} ▶ {coroutineValueKvp.Key}");
					GUI.enabled = true;
					if (GUILayout.Button("Kill"))
						_coroutineStarter.StopCoroutine(coroutineValueKvp.Value.Coroutine);
					EditorGUILayout.EndHorizontal();
				}
			}
		}

		/// <summary>
		/// Redraw any inspectors that shows this editor.
		/// </summary>
		private void RepaintFromEvent(object sender, CoroutineStarter.CoroutineValue e)
		{
			Repaint();
		}
	}
}