using Elephant.UnityLibrary.Securities;
using Elephant.UnityLibrary.Securities.Interfaces;

namespace UnityLibraryTests.Securities
{
	/// <summary>
	/// <see cref="NumericObfuscator"/> tests.
	/// </summary>
	public class NumericObfuscatorTests
	{
		/// <summary>
		/// Tests that obfuscating and deobfuscating various decimal values returns the original value.
		/// </summary>
		[Theory]
		[InlineData(98765.4321)]
		[InlineData(-12345.6789)]
		[InlineData(0)]
		[InlineData(1.2345)]
		[InlineData(-0.0001)]
		public void ObfuscateAndDeobfuscateDecimal_ReturnsOriginalValue(decimal original)
		{
			// Arrange.
			INumericObfuscator obfuscator = new NumericObfuscator(10);

			// Act.
			decimal obfuscated = obfuscator.Obfuscate(original);
			decimal deobfuscated = obfuscator.DeObfuscate(obfuscated);

			// Assert.
			Assert.Equal(original, deobfuscated);
		}

		/// <summary>
		/// Tests that obfuscating and deobfuscating a double returns the original value.
		/// </summary>
		[Theory]
		[InlineData(98765.4321)]
		[InlineData(-12345.6789)]
		[InlineData(0)]
		[InlineData(1.2345)]
		[InlineData(-0.1)]
		public void ObfuscateAndDeobfuscateDouble_ReturnsOriginalValue(double original)
		{
			// Arrange.
			INumericObfuscator obfuscator = new NumericObfuscator(12345);

			// Act.
			double obfuscated = obfuscator.Obfuscate(original);
			double deobfuscated = obfuscator.DeObfuscate(obfuscated);

			// Assert.
			Assert.Equal(original, deobfuscated, precision: 2);
		}

		/// <summary>
		/// Tests that obfuscating and deobfuscating a float returns the original value.
		/// </summary>
		[Theory]
		[InlineData(12345.6789f)]
		[InlineData(-1234.5678f)]
		[InlineData(0f)]
		[InlineData(1.23f)]
		[InlineData(-0.1f)]
		public void ObfuscateAndDeobfuscateFloat_ReturnsOriginalValue(float original)
		{
			// Arrange.
			INumericObfuscator obfuscator = new NumericObfuscator(12340);

			// Act.
			float obfuscated = obfuscator.Obfuscate(original);
			float deobfuscated = obfuscator.DeObfuscate(obfuscated);

			// Assert.
			Assert.Equal(original, deobfuscated, precision: 2);
		}

		/// <summary>
		/// Tests that obfuscating and deobfuscating an int returns the original value.
		/// </summary>
		[Theory]
		[InlineData(123456789)]
		[InlineData(-98765432)]
		[InlineData(0)]
		[InlineData(100)]
		[InlineData(-1)]
		public void ObfuscateAndDeobfuscateInt_ReturnsOriginalValue(int original)
		{
			// Arrange.
			INumericObfuscator obfuscator = new NumericObfuscator(54021);

			// Act.
			int obfuscated = obfuscator.Obfuscate(original);
			int deobfuscated = obfuscator.DeObfuscate(obfuscated);

			// Assert.
			Assert.Equal(original, deobfuscated);
		}

		/// <summary>
		/// Tests that obfuscating and deobfuscating a long returns the original value.
		/// </summary>
		[Theory]
		[InlineData(9876543210123)]
		[InlineData(-9876543210123)]
		[InlineData(0)]
		[InlineData(1000000000000)]
		[InlineData(-1)]
		public void ObfuscateAndDeobfuscateLong_ReturnsOriginalValue(long original)
		{
			// Arrange.
			INumericObfuscator obfuscator = new NumericObfuscator(-120056);

			// Act.
			long obfuscated = obfuscator.Obfuscate(original);
			long deobfuscated = obfuscator.DeObfuscate(obfuscated);

			// Assert.
			Assert.Equal(original, deobfuscated);
		}
	}
}