using System;
using System.Globalization;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Formats large numbers into a more human-readable format
	/// (or shorter format) using customizable suffixes like:
	/// 'k', 'm', 'b', and 't'.
	/// Values are truncated, not rounded. Supports negative numbers.
	/// </summary>
	/// <remarks>Treats negative zero input (-0) as zero (0).</remarks>
	public class NumberFormatter
	{
		/// <summary>Customizable unit suffixes for thousands.</summary>
		public string UnitThousands { get; set; } = "k";

		/// <summary>Customizable unit suffixes for millions.</summary>
		public string UnitMillions { get; set; } = "m";

		/// <summary>Customizable unit suffixes for billions.</summary>
		public string UnitBillions { get; set; } = "b";

		/// <summary>Customizable unit suffixes for trillions.</summary>
		public string UnitTrillions { get; set; } = "t";

		/// <summary>Customizable unit suffixes for quadrillions.</summary>
		public string UnitQuadrillions { get; set; } = "q";

		/// <summary>
		/// Formats the given score into a more human-readable format.
		/// </summary>
		/// <param name="value">Value to format.</param>
		/// <param name="minValueRequired">Minimum value required for formatting to be applied. Defaults to 10000.</param>
		/// <param name="maxDecimals">
		/// Maximum number of decimal places to include in the formatted output.
		/// Set to 0 for no decimals. Values less than 0 are treated as 0. Defaults to 1.
		/// </param>
		/// <returns>Value in a formatted or unformatted format.</returns>
		public string Format(int value, int minValueRequired = 10000, int maxDecimals = 1)
		{
			return Format((long)value, (long)minValueRequired, maxDecimals);
		}

		/// <summary>
		/// Formats the given score into a more human-readable format.
		/// </summary>
		/// <param name="value">Value to format.</param>
		/// <param name="minValueRequired">Minimum value required for formatting to be applied. Defaults to 10000.</param>
		/// <param name="maxDecimals">
		/// Maximum number of decimal places to include in the formatted output.
		/// Set to 0 for no decimals. Values less than 0 are treated as 0. Defaults to 1.
		/// </param>
		/// <returns>Value in a formatted or unformatted format.</returns>
		public string Format(long value, long minValueRequired = 10000, int maxDecimals = 1)
		{
			if (maxDecimals < 0)
				maxDecimals = 0;

			long absoluteScore = Math.Abs(value);
			string sign = value < 0 ? "-" : "";

			if (absoluteScore < minValueRequired || absoluteScore < 1000)
			{
				return value.ToString(CultureInfo.InvariantCulture);
			}
			else if (absoluteScore < 1_000_000)
			{
				return sign + FormatUnit(absoluteScore, 1000, UnitThousands, maxDecimals);
			}
			else if (absoluteScore < 1_000_000_000)
			{
				return sign + FormatUnit(absoluteScore, 1_000_000, UnitMillions, maxDecimals);
			}
			else if (absoluteScore < 1_000_000_000_000)
			{
				return sign + FormatUnit(absoluteScore, 1_000_000_000, UnitBillions, maxDecimals);
			}
			else if (absoluteScore < 1_000_000_000_000_000)
			{
				return sign + FormatUnit(absoluteScore, 1_000_000_000_000, UnitTrillions, maxDecimals);
			}
			else
			{
				// Quadrillions and above.
				return sign + FormatUnit(absoluteScore, 1_000_000_000_000_000, UnitQuadrillions, maxDecimals);
			}
		}

		/// <summary>
		/// Formats the given value by dividing it by the specified divisor, applies precision rules,
		/// clamps the result to avoid rounding up at unit boundaries, and appends the provided unit suffix.
		/// Truncates instead of rounding values.
		/// </summary>
		/// <param name="absoluteValue">Absolute value to format (e.g., 1234567).</param>
		/// <param name="divisor">
		/// Number by which to divide <paramref name="absoluteValue"/> to
		/// scale it to the appropriate unit (e.g., 1,000 for thousands, 1,000,000 for millions).</param>
		/// <param name="unit">The unit suffix to append to the formatted number (e.g., "k", "m", "b", "t").
		/// </param>
		/// <param name="maxDecimals">
		/// Maximum number of decimal places to include in the formatted output.
		/// Set to 0 for no decimals.
		/// </param>
		/// <returns>
		/// String representing the formatted value with the appropriate number of decimal places and the
		/// specified unit suffix.
		/// </returns>
		/// <example>1,230,000 with a divisor of 1,000,000 and unit "m" becomes: "1.23m".</example>
		private string FormatUnit(long absoluteValue, long divisor, string unit, int maxDecimals)
		{
			// Convert the absolute value to the scaled unit (e.g., thousands, millions).
			decimal value = (decimal)absoluteValue / divisor;

			// Prevent values like 999.999k from rounding up to 1000k (which would be 1m).
			// Clamp the value to 999 to ensure it never reaches the next unit.
			if (value >= 1000)
				value = 999;

			// Ensure maxDecimals is within a reasonable range (0 to 15).
			if (maxDecimals < 0)
				maxDecimals = 0;
			if (maxDecimals > 15)
				maxDecimals = 15;

			// Calculate the factor for truncating decimals (e.g., 10, 100, 1000).
			decimal factor = (decimal)Math.Pow(10, maxDecimals);

			// Truncate the value to the specified number of decimal places (no rounding).
			value = Math.Truncate(value * factor) / factor;

			// Build the numeric format string (e.g., "0", "0.#", "0.##").
			string format = maxDecimals <= 0 ? "0" : $"0.{new string('#', maxDecimals)}";

			// Format the value as a string with the correct number of decimals and append the unit suffix.
			return $"{value.ToString(format, CultureInfo.InvariantCulture)}{unit}";
		}
	}
}
