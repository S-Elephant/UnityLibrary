using Elephant.UnityLibrary.Other;

namespace UnityLibraryTests.Others
{
	/// <summary>
	/// <see cref="NumberFormatter"/> tests.
	/// </summary>
	public class NumberFormatterTests
	{
		/// <summary>
		/// Formatting of various positive long values format as expected.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(9999, "9999")]
		[InlineData(10000, "10k")]
		[InlineData(12345, "12.3k")]
		[InlineData(999999, "999.9k")]
		[InlineData(1_000_000, "1m")]
		[InlineData(12_345_678, "12.3m")]
		[InlineData(999_999_999, "999.9m")]
		[InlineData(1_000_000_000, "1b")]
		[InlineData(2_345_678_901, "2.3b")]
		[InlineData(999_999_999_999, "999.9b")]
		[InlineData(1_000_000_000_000, "1t")]
		[InlineData(1_234_567_890_123, "1.2t")]
		public void Format_PositiveLongs(long value, string expected)
		{
			// Act.
			string formattedValue = new NumberFormatter().Format(value);

			// Assert.
			Assert.Equal(expected, formattedValue);
		}

		/// <summary>
		/// Formatting of various negative long values format as expected.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(-9999, "-9999")]
		[InlineData(-10000, "-10k")]
		[InlineData(-12345, "-12.34k")]
		[InlineData(-999999, "-999.99k")]
		[InlineData(-1_000_000, "-1m")]
		[InlineData(-12_345_678, "-12.34m")]
		[InlineData(-999_999_999, "-999.99m")]
		[InlineData(-1_000_000_000, "-1b")]
		[InlineData(-2_345_678_901, "-2.34b")]
		[InlineData(-999_999_999_999, "-999.99b")]
		[InlineData(-1_000_000_000_000, "-1t")]
		[InlineData(-1_234_567_890_123, "-1.23t")]
		public void Format_NegativeLongs(long value, string expected)
		{
			// Act.
			string formattedValue = new NumberFormatter().Format(value, maxDecimals: 2);

			// Assert.
			Assert.Equal(expected, formattedValue);
		}

		/// <summary>
		/// Formatting with custom minimum value required returns expected.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(9999, 5000, "9.999k")]
		[InlineData(9999, 10000, "9999")]
		[InlineData(10000, 10001, "10000")]
		public void Format_CustomMinValueRequired(long value, long minValueRequired, string expected)
		{
			// Act.
			string formattedValue = new NumberFormatter().Format(value, minValueRequired, maxDecimals: 999);

			// Assert.
			Assert.Equal(expected, formattedValue);
		}

		/// <summary>
		/// Formatting with a maximum of 999 decimals returns expected.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(9999, "9999")]
		[InlineData(10000, "10k")]
		[InlineData(12345, "12.345k")]
		[InlineData(10500, "10.5k")]
		[InlineData(11000, "11k")]
		[InlineData(12001, "12.001k")]
		[InlineData(15000, "15k")]
		[InlineData(123456, "123.456k")]
		[InlineData(1234567, "1.234567m")]
		[InlineData(2345678, "2.345678m")]
		[InlineData(123456789, "123.456789m")]
		[InlineData(1234567890, "1.23456789b")]
		[InlineData(2345678901, "2.345678901b")]
		[InlineData(123456789012, "123.456789012b")]
		[InlineData(1234567890123, "1.234567890123t")]
		[InlineData(2345678901234, "2.345678901234t")]
		[InlineData(12345678901234, "12.345678901234t")]
		[InlineData(123456789012345, "123.456789012345t")]
		[InlineData(1234567890123456, "1.234567890123456q")]
		[InlineData(2345678901234567, "2.345678901234567q")]
		public void Format_TwoDecimals(long value,string expected)
		{
			// Act.
			string formattedValue = new NumberFormatter().Format(value, maxDecimals: 999);

			// Assert.
			Assert.Equal(expected, formattedValue);
		}

		/// <summary>
		/// Formatting of int values delegates to long formatting.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void Format_Int_DelegatesToLong()
		{
			// Act.
			string formattedValue = new NumberFormatter().Format(10000);

			// Assert.
			Assert.Equal("10k", formattedValue);
		}

		/// <summary>
		/// Formatting of various positive quadrillion values format as expected.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(999_999_999_999_999, "999.99t")]
		[InlineData(1_000_000_000_000_000, "1q")]
		[InlineData(1_234_567_890_123_456, "1.23q")]
		[InlineData(9_999_999_999_999_999, "9.99q")]
		public void Format_Quadrillion_PositiveLongs(long value, string expected)
		{
			// Act.
			string formattedValue = new NumberFormatter().Format(value, maxDecimals: 2);

			// Assert.
			Assert.Equal(expected, formattedValue);
		}

		/// <summary>
		/// Formatting of various negative quadrillion values format as expected.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(-999_999_999_999_999, "-999.99t")]
		[InlineData(-1_000_000_000_000_000, "-1q")]
		[InlineData(-1_234_567_890_123_456, "-1.23q")]
		[InlineData(-9_999_999_999_999_999, "-9.99q")]
		public void Format_Quadrillion_NegativeLongs(long value, string expected)
		{
			// Act.
			string formattedValue = new NumberFormatter().Format(value, maxDecimals: 2);

			// Assert.
			Assert.Equal(expected, formattedValue);
		}
	}
}
