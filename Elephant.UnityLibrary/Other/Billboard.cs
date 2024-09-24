using UnityEngine;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// A billboard that always faces the camera and supports sprites.
	/// </summary>
	/// <remarks>
	/// Unity's <see cref="BillboardRenderer"/> does not support sprites but
	/// this one does if you attach a <see cref="SpriteRenderer"/>.
	/// </remarks>
	public class Billboard : MonoBehaviour
	{
		/// <summary>
		/// Camera to face.
		/// </summary>
		public Camera TargetCamera;

		/// <summary>
		/// Update.
		/// </summary>
		private void Update()
		{
			LookAtCamera();
		}

		/// <summary>
		/// Face the camera.
		/// </summary>
		protected void LookAtCamera()
		{
			transform.LookAt(TargetCamera.transform);
		}
	}
}
