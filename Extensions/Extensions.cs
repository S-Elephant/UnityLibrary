using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// Version 1.00
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Return random element.
		/// </summary>
		public static T GetRandom<T>(this IEnumerable<T> source)
		{
			return source.Shuffle().Take(1).Single();
		}


		/// <summary>
		/// Shuffle the <paramref name="list"/>.
		/// </summary>
		public static void Shuffle<T>(this IList<T> list)
		{
			int cnt = list.Count;
			while (cnt > 1)
			{
				cnt--;
				int k = UnityEngine.Random.Range(0, cnt - 1);
				T value = list[k];
				list[k] = list[cnt];
				list[cnt] = value;
			}
		}

		public static Vector3 DirectionTo(this Vector3 from, Vector3 to)
		{
			return (to - from).normalized;
		}

		/// <summary>
		/// Returns the orthographic camera bounds in world space.
		/// </summary>
		public static Bounds OrthographicBounds(this Camera camera)
		{
			float screenAspect = (float)Screen.width / (float)Screen.height;
			float cameraHeight = camera.orthographicSize * 2;
			Bounds bounds = new Bounds(
				camera.transform.position,
				new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
			return bounds;
		}

		public static Transform FindRecursively(this Transform parent, string name)
		{
			foreach (Transform child in parent)
			{
				if (child.name == name)
				{
					return child;
				}

				Transform result = child.FindRecursively(name);
				if (result != null)
				{
					return result;
				}
			}

			return null;
		}
	}
}
