using Elephant.UnityLibrary.Maths;
using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// <see cref="float"/> extensions.
	/// </summary>
	public static class FloatExtensions
	{
		/// <summary>
		/// Determines if two float values are approximately equal within the specified <paramref name="tolerance"/>.
		/// </summary>
		/// <param name="value">First float value to compare.</param>
		/// <param name="other">Second float value to compare.</param>
		/// <param name="tolerance">Maximum allowed difference (inclusive) between the values (default: <see cref="MathConstants.SafeGameTolerance"/>).</param>
		/// <returns><c>true</c> if the absolute difference between the values is less than or equal to the tolerance; otherwise, false.</returns>
		public static bool AreRoughlyEqual(this float value, float other, float tolerance = MathConstants.SafeGameTolerance)
		{
			return Mathf.Abs(value - other) <= tolerance;
		}

		/// <summary>
		/// Determines if two float values are approximately unequal within the specified <paramref name="tolerance"/>.
		/// </summary>
		/// <param name="value">First float value to compare.</param>
		/// <param name="other">Second float value to compare.</param>
		/// <param name="tolerance">Minimum allowed difference (inclusive) between the values (default: <see cref="MathConstants.SafeGameTolerance"/>).</param>
		/// <returns><c>true</c> if the absolute difference between the values is greater than the tolerance; otherwise, false.</returns>
		public static bool AreRoughlyUnequal(this float value, float other, float tolerance = MathConstants.SafeGameTolerance)
		{
			return Mathf.Abs(value - other) >= tolerance;
		}

		/// <summary>
		/// Returns true if <paramref name="value"/> is roughly zero.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// <param name="tolerance">Precision tolerance value.</param>
		/// <returns><c>true</c> if <paramref name="value"/> is roughly zero.</returns>
		/// <remarks>Doesn't check for <see cref="float.NaN"/>.</remarks>
		public static bool IsRoughlyZero(this float value, float tolerance = MathConstants.SafeGameTolerance)
		{
			return value <= tolerance && value >= -tolerance;
		}

		/// <summary>
		/// Returns true if <paramref name="value"/> is NOT roughly zero.
		/// </summary>
		/// <param name="value">Value to check.</param>
		/// /// <param name="tolerance">Precision tolerance value.</param>
		/// <returns><c>true</c> if <paramref name="value"/> is NOT roughly zero.</returns>
		/// <remarks>Doesn't check for <see cref="float.NaN"/>.</remarks>
		public static bool IsNotRoughlyZero(this float value, float tolerance = MathConstants.SafeGameTolerance)
		{
			return value < -tolerance || value > tolerance;
		}
	}
}