#nullable enable

using UnityEngine;
using UnityEngine.UIElements;

namespace Elephant.UnityLibrary.Diagnostics
{
	/// <summary>
	/// FPS counter that can optionally update an UI Toolkit label.
	/// </summary>
	public class FpsCounter : MonoBehaviour
	{
		/// <summary>
		/// <p>Optional FPS display.</p>
		/// <p>If set, it's text will be automatically updated if <see cref="IsEnabled"/> is true.</p>
		/// </summary>
		[Tooltip("Optional FPS display. If set, it's text will be automatically updated (assuming IsEnabled is true).")]
		public UnityEngine.UIElements.Label? UiToolkitFpsDisplay = null;

		/// <summary>
		/// Optional <see cref="UnityEngine.UIElements.Label"/> name as named in your UI document.
		/// Leave empty if you don't want to use this. Is case-sensitive.
		/// </summary>
		[Tooltip("Optional UnityEngine.UIElements.Label name as named in your UI document. Leave empty if you don't want to use this. Is case-sensitive.")]
		public string UiToolkitFpsLabelName = string.Empty;

		/// <summary>
		/// Delta time.
		/// </summary>
		private float _deltaTime = 0.0f;

		/// <summary>
		/// Current FPS value or 0 if not yet set or disabled.
		/// </summary>
		[Tooltip("Current FPS value or 0 if not yet set or disabled.")]
		public int FpsValue { get; private set; } = 0;

		/// <summary>
		/// Will only update the fps value if this is set to true.
		/// </summary>
		[Tooltip("Will only update the fps value if this is set to true.")]
		public bool IsEnabled = true;

		/// <summary>
		/// If true, automatically destroys this script instance when it's not running in the Unity Editor.
		/// </summary>
		[Tooltip("If true, automatically destroys this script instance when it's not running in the Unity Editor.")]
		public bool DestroyIfNotUnityEditor = false;

		/// <summary>
		/// Format string used to display the FPS. Use {0} as the placeholder for the FPS value.
		/// </summary>
		[Tooltip("Format string used to display the FPS. Use {0} as the placeholder for the FPS value.")]
		public string FpsDisplayFormat = "FPS: {0}";

		/// <summary>
		/// Awake.
		/// </summary>
		private void Awake()
		{
#if !UNITY_EDITOR
			if (DestroyIfNotUnityEditor)
				Destroy(this);
#endif

			if (UiToolkitFpsDisplay == null && UiToolkitFpsLabelName != string.Empty)
			{
				// Attempt to get it by name.
				VisualElement root = GetComponent<UIDocument>().rootVisualElement;
				UiToolkitFpsDisplay = root.Q<UnityEngine.UIElements.Label>(UiToolkitFpsLabelName);
			}
		}

		/// <summary>
		/// Update.
		/// </summary>
		private void Update()
		{
			if (!IsEnabled)
			{
				FpsValue = 0;
				return;
			}

			_deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
			float fps = 1.0f / _deltaTime;
			FpsValue = (int)Mathf.Ceil(fps);

			if (UiToolkitFpsDisplay != null)
				UiToolkitFpsDisplay.text = string.Format(FpsDisplayFormat, FpsValue);
		}
	}
}
