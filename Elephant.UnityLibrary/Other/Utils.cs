using System;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Version 1.00
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// Returns true of <paramref name="value"/> is an even number.
		/// </summary>
		public static bool IsEven(int value)
		{
			return value % 2 == 0;
		}

		/// <summary>
		/// Returns true of <paramref name="value"/> is an even number.
		/// </summary>
		public static bool IsEven(float value)
		{
			return value % 2 == 0;
		}

		/// <summary>
		/// Returns true of <paramref name="value"/> is an odd number.
		/// </summary>
		public static bool IsOdd(int value)
		{
			return value % 2 != 0;
		}

		/// <summary>
		/// Returns true of <paramref name="value"/> is an odd number.
		/// </summary>
		public static bool IsOdd(float value)
		{
			return value % 2 != 0;
		}

		/// <summary>
		/// Calculates the distance between 2 points.
		/// </summary>
		public static float Distance(float x1, float y1, float x2, float y2)
		{
			return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
		}
	}
}
