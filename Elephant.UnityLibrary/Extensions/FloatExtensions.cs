using Elephant.UnityLibrary.Maths;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// <see cref="float"/> extensions.
	/// </summary>
	public static class FloatExtensions
	{
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