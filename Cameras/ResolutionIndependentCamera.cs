#nullable enable
using UnityEngine;

namespace Elephant.UnityLibrary.Cameras
{
	/// <summary>
	/// You can use Unity's camera with an orthographic projection and the orthographic size of the camera defines how
	/// much content is visible vertically, and it doesn't change with the resolution by default. However, aspect ratios
	/// can change the visible content horizontally, so you might want to adjust for that by (for example) using this
	/// script.
	/// </summary>
	public class ResolutionIndependentCamera : MonoBehaviour
	{
		/// <summary>
		/// The aspect ratio you've primarily designed for.
		/// The most common mobile aspect is 16:9.
		/// </summary>
		public float DesignAspect = 16f / 9f;

		/// <summary>
		/// Assign the your camera here. If you leave it null then it will look for a camera on its gameobject instead.
		/// </summary>
		[SerializeField] private Camera? _camera = null;

		/// <summary>
		/// Awake.
		/// </summary>
		private void Awake()
		{
			if (_camera == null)
				RetrieveAndSetCamera();

			if (_camera != null)
				_camera.orthographic = true; // Required.
		}

		/// <summary>
		/// Start.
		/// </summary>
		private void Start()
		{
			if (_camera == null)
				return;

			// Calculate the current aspect ratio of the screen.
			float currentAspect = (float)Screen.width / Screen.height;

			// Determine the scale ratio between the current aspect ratio and the design aspect ratio.
			float scaleHeight = currentAspect / DesignAspect;

			// If the scaleHeight is less than 1, it means that the current aspect ratio is taller/narrower than the design aspect ratio.
			if (scaleHeight < 1.0f)
			{
				// Adjust the orthographic size based on the scaleHeight so that the horizontal view remains consistent.
				float orthographicSize = _camera.orthographicSize / scaleHeight;
				_camera.orthographicSize = orthographicSize;
			}
		}

		/// <summary>
		/// Retrieve the camera and set it.
		/// </summary>
		public virtual void RetrieveAndSetCamera()
		{
			_camera = GetComponentInChildren<Camera>();

			if (_camera == null)
				Debug.LogError($"No {nameof(Camera)} set and unable to find a {nameof(Camera)} on this {gameObject.name} {nameof(GameObject)} nor any of its children. The camera won't be resolution independent.");
		}
	}
}