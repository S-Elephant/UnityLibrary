#nullable enable
using UnityEngine;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// A way to start coroutines from non-MonoBehaviour classes in Unity.
	/// </summary>
	/// <example>CoroutineStarter.Instance.StartCoroutine(MyCoroutine)</example>
	public class CoroutineStarter : MonoBehaviour
	{
		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static CoroutineStarter? _instance = null;

		/// <summary>
		/// Public access singleton instance.
		/// </summary>
		public static CoroutineStarter Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject go = new("CoroutineStarter");
					_instance = go.AddComponent<CoroutineStarter>();
				}
				return _instance;
			}
		}
	}
}
