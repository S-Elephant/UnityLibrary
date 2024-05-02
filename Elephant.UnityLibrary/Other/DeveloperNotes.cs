using UnityEngine;

namespace Elephant.UnityLibrary
{
	/// <summary>
	/// Is used for adding developer notes in the Unity Inspector.
	/// </summary>
	/// <remarks>Is intended for editor-use only and will self-destruct in non-editor builds in the Awake().</remarks>
	public class DeveloperNotes : MonoBehaviour
	{
#if UNITY_EDITOR
		/// <summary>
		/// Developer notes.
		/// </summary>
		[TextArea] public string Notes = string.Empty;
#else
		/// <summary>
		/// Awake.
		/// </summary>
		private void Awake()
		{
			Destroy(this);
		}
#endif
	}
}