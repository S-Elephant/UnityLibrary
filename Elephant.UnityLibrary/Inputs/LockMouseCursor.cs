using UnityEngine;

namespace Elephant.UnityLibrary.Inputs
{
	/// <summary>
	/// Add this script to one of your gameobjects in your scene to confine or lock the cursor.
	/// Version 1.00
	/// </summary>
	public class LockMouseCursor : MonoBehaviour
	{
		/// <inheritdoc cref="CursorLockMode"/>
		public CursorLockMode CursorLockMode = CursorLockMode.Confined;

		/// <summary>
		/// Start.
		/// </summary>
		private void Start()
		{
			Cursor.lockState = CursorLockMode;
		}
	}
}
