#nullable enable
using UnityEngine;

namespace Elephant.UnityLibrary.Cameras
{
	/// <summary>
	/// Adjusts the orthographic size of the camera to fit certain bounds, ensuring that the content remains consistent
	/// across varying screen resolutions and aspect ratios. The various methods provided allow you to fit the camera
	/// to specified bounds, or to the entirety of a predefined screen size in Unity units.
	/// For more info see: https://www.youtube.com/watch?v=3xXlnSetHPM
	/// </summary>
	/// <example>
	/// Attach this script to your Camera gameobject and call <see cref="HorizontalFit"/> in the Update.
	/// Now try resizing your game window.
	/// Note that usually you may call it once from the Start() instead of from the Update().
	/// </example>
	[RequireComponent(typeof(Camera))]
	public class ScaleCameraWithScreenResolution : MonoBehaviour
	{
		/// <summary>
		/// Assign the your camera here. If you leave it null then it will look for a camera on its gameobject instead.
		/// </summary>
		[SerializeField] private Camera? _camera = null;

		/// <summary>
		/// The desired size of the screen in Unity world units.
		///
		/// This represents the reference size based on which the camera's orthographic size will be adjusted.
		/// For instance, if the value is set to (9, 15):
		/// - The width represents 9 units in the Unity world from the left edge to the right edge of the camera's view.
		/// - The height represents 15 units in the Unity world from the bottom edge to the top edge of the camera's view.
		///
		/// Adjusting this value will determine how much of the game's world is visible on the screen. If the actual screen's
		/// aspect ratio is different from this ratio, the camera's orthographic size will be adjusted to fit the content
		/// based on the chosen fitting method (either horizontally or vertically).
		/// </summary>
		[SerializeField] private Vector2Int _screenSizeInUnityUnits = new(9, 15);

		/// <summary>
		/// The bounds for <see cref="FitToBounds()"/>.
		/// Note that bounds are twice as large compared to your normal measurements. Therefore, if you want to display
		/// an area of 16x9 (16 on the x, 9 on the y) then you should use bounds (8f, 4.5f, 0f).
		/// </summary>
		public Bounds TargetBounds = new(Vector3.zero, new Vector3(8f, 4.5f, 0f));

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
		/// Retrieve the camera and set it.
		/// </summary>
		public virtual void RetrieveAndSetCamera()
		{
			_camera = GetComponentInChildren<Camera>();

			if (_camera == null)
				Debug.LogError($"No {nameof(Camera)} set and unable to find a {nameof(Camera)} on this {gameObject.name} {nameof(GameObject)} nor any of its children. The camera won't be fit to certain dimensions.");
		}

		/// <summary>
		/// Adjusts the camera's orthographic size to fit the provided target bounds.
		/// Will try to fit the height of the target bounds into the screen and adjusts
		/// for width if the screen's aspect ratio is narrower than the target bounds.
		/// </summary>
		/// <param name="targetBounds">The bounds you want the camera to encompass.</param>
		public virtual void FitToBounds(Bounds targetBounds)
		{
			if (_camera == null)
				return;

			float screenRatio = Screen.width / (float)Screen.height;
			float targetRatio = targetBounds.size.x / targetBounds.size.y;

			if (screenRatio >= targetRatio)
			{
				_camera.orthographicSize = targetBounds.size.y / 2;
			}
			else
			{
				float differenceInSize = targetRatio / screenRatio;
				_camera.orthographicSize = targetBounds.size.y / 2 * differenceInSize;
			}
		}

		/// <summary>
		/// Adjusts the camera's orthographic size to fit the provided target bounds.
		/// Will try to fit the height of the target bounds into the screen and adjusts
		/// for width if the screen's aspect ratio is narrower than the target bounds.
		/// Uses <see cref="TargetBounds"/> as the target bounds.
		/// </summary>
		public virtual void FitToBounds()
		{
			FitToBounds(TargetBounds);
		}

		/// <summary>
		/// Adjusts the camera's orthographic size to fit the entire screen size.
		/// </summary>
		public virtual void FitEntireScreen()
		{
			FitEntireScreen(new Bounds(Vector3.zero, new Vector3(_screenSizeInUnityUnits.x, _screenSizeInUnityUnits.y)));
		}

		/// <summary>
		/// Adjusts the camera's orthographic size to fit the entire screen size.
		/// </summary>
		public virtual void HorizontalFit()
		{
			if (_camera == null)
				return;

			_camera.orthographicSize = (float)_screenSizeInUnityUnits.x * Screen.height / Screen.width * 0.5f;
		}

		/// <summary>
		/// Adjusts the camera's orthographic size to fit content vertically based on the predefined screen height.
		/// </summary>
		public virtual void VerticalFit()
		{
			VerticalFit(new Bounds(Vector3.zero, new Vector3(_screenSizeInUnityUnits.x, _screenSizeInUnityUnits.y)));
		}

		private void VerticalFit(Bounds bounds)
		{
			if (_camera == null)
				return;

			/* Why the 0.5 multiplication explanation:
			 * 1. Unity's orthographic size is actually half the height of the screen in world units. So, if your orthographic size is 5,
			 * the full height from the bottom to the top of the camera's view will be 10.
			 * 2. The ScreenSizeInUnityUnits.x represents the desired width in Unity units.
			 * 3. The term Screen.height / Screen.width gives you the aspect ratio of the screen.
			 * 4. Multiplying ScreenSizeInUnityUnits.x by the aspect ratio provides the full height in Unity units to achieve the desired width.
			 * 5. Since the orthographic size is half the height, we multiply by 0.5f to get the correct orthographic size.
			 */

			_camera.orthographicSize = bounds.size.x * Screen.height / Screen.width * 0.5f; // The 0.5f multiplier essentially converts the full width into a half width, which is how Unity's orthographic size is defined.
		}

		private void FitEntireScreen(Bounds bounds)
		{
			if (_camera == null)
				return;

			float screenRatio = Screen.width / (float)Screen.height;
			float targetRatio = bounds.size.x / bounds.size.y;

			if (screenRatio >= targetRatio)
				_camera.orthographicSize = bounds.size.y / 2;
			else
			{
				float differenceInSize = targetRatio / screenRatio;
				_camera.orthographicSize = bounds.size.y / 2 * differenceInSize;
			}
		}
	}
}
