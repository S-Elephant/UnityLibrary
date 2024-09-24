using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// Vector extension methods.
	/// </summary>
	public static class Vectors
	{
		/// <summary>
		/// Convert <paramref name="vector3"/> 3D into the correct 2D <see cref="Vector2"/>.
		/// The <see cref="Vector3"/> y value is discarded.
		/// </summary>
		public static Vector2 To2d(this Vector3 vector3)
		{
			return new Vector2(vector3.x, vector3.z);
		}

		/// <summary>
		/// Convert <paramref name="vector3"/> 3D into the correct 2D <see cref="Vector2"/>.
		/// The <see cref="Vector3"/> y value is discarded.
		/// Returns null if input is null.
		/// </summary>
		public static Vector2? To2d(this Vector3? vector3)
		{
			if (vector3 == null)
				return null;

			return new Vector2(vector3.Value.x, vector3.Value.z);
		}

		/// <summary>
		/// Convert <paramref name="vector2"/> 2D into the correct 3D <see cref="Vector3"/>.
		/// The <see cref="Vector3"/> y value is set to 0f and the <paramref name="vector2"/> y value is set to the <see cref="Vector3"/> z value.
		/// </summary>
		public static Vector3 To3d(this Vector2 vector2)
		{
			return new Vector3(vector2.x, 0f, vector2.y);
		}

		/// <summary>
		/// Convert <paramref name="vector2"/> 2D into the correct 3D <see cref="Vector3"/>.
		/// The <see cref="Vector3"/> y value is set to 0f and the <paramref name="vector2"/> y value is set to the <see cref="Vector3"/> z value.
		/// Returns null if input is null.
		/// </summary>
		public static Vector3? To3d(this Vector2? vector2)
		{
			if (vector2 == null)
				return null;

			return new Vector3(vector2.Value.x, 0f, vector2.Value.y);
		}

		/// <summary>
		/// Returns the normalized direction from one vector to another.
		/// </summary>
		/// <param name="from">Starting point of the direction vector.</param>
		/// <param name="to">Endpoint of the direction vector.</param>
		/// <returns>Normalized direction vector pointing from the 'from' vector to the 'to' vector.</returns>
		public static Vector2 DirectionTo(this Vector2 from, Vector2 to)
		{
			return (to - from).normalized;
		}

		/// <summary>
		/// Returns the normalized direction from one vector to another.
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
