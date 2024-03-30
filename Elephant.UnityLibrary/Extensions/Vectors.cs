using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// Vector extension methods.
	/// </summary>
	public static class Vectors
	{
		/// <summary>
		/// Calculates the normalized direction from one vector to another.
		/// </summary>
		/// <param name="from">Starting point of the direction vector.</param>
		/// <param name="to">Endpoint of the direction vector.</param>
		/// <returns>Normalized direction vector pointing from the 'from' vector to the 'to' vector.</returns>
		public static Vector2 DirectionTo(this Vector2 from, Vector2 to)
		{
			return (to - from).normalized;
		}

		/// <summary>
		/// Calculates the normalized direction from one vector to another.
		/// </summary>
		/// <param name="from">Starting point of the direction vector.</param>
		/// <param name="to">Endpoint of the direction vector.</param>
		/// <returns>Normalized direction vector pointing from the 'from' vector to the 'to' vector.</returns>
		public static Vector3 DirectionTo(this Vector3 from, Vector3 to)
		{
			return (to - from).normalized;
		}

		/// <summary>
		/// Convert <paramref name="vector3"/> into a <see cref="Vector2"/>.
		/// The <see cref="Vector3.z"/> value is discarded.
		/// </summary>
		public static Vector2 ToVector2(this Vector3 vector3)
		{
			return new Vector2(vector3.x, vector3.y);
		}

		/// <summary>
		/// Convert <paramref name="vector4"/> into a <see cref="Vector2"/>.
		/// The <see cref="Vector4.z"/> and <see cref="Vector4.w"/> values are discarded.
		/// </summary>
		public static Vector2 ToVector2(this Vector4 vector4)
		{
			return new Vector2(vector4.x, vector4.y);
		}

		/// <summary>
		/// Convert <paramref name="vector2"/> into a <see cref="Vector3"/> with an optional <see cref="Vector3.z"/> parameter.
		/// </summary>
		public static Vector3 ToVector3(this Vector2 vector2, float z = 0f)
		{
			return new Vector3(vector2.x, vector2.y, z);
		}

		/// <summary>
		/// Convert <paramref name="vector4"/> into a <see cref="Vector2"/>.
		/// The <see cref="Vector4.w"/> value is discarded.
		/// </summary>
		public static Vector3 ToVector3(this Vector4 vector4)
		{
			return new Vector3(vector4.x, vector4.y, vector4.z);
		}

		/// <summary>
		/// Convert <paramref name="vector2"/> into a <see cref="Vector4"/> with optional
		/// <see cref="Vector4.z"/> and <see cref="Vector4.w"/> parameters.
		/// </summary>
		public static Vector4 ToVector4(this Vector2 vector2, float z = 0f, float w = 0f)
		{
			return new Vector4(vector2.x, vector2.y, z, w);
		}

		/// <summary>
		/// Convert <paramref name="vector3"/> into a <see cref="Vector4"/> with optional <see cref="Vector4.w"/> parameter.
		/// </summary>
		public static Vector4 ToVector4(this Vector3 vector3, float w = 0f)
		{
			return new Vector4(vector3.x, vector3.y, vector3.z, w);
		}
	}
}
