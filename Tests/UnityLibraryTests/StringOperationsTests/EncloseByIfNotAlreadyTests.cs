using Elephant.UnityLibrary.Common;

namespace UnityLibraryTests.StringOperationsTests
{
	/// <summary>
	/// <see cref="StringOperations.EncloseByIfNotAlready"/> tests.
	/// </summary>
	public class EncloseByIfNotAlreadyTests
	{
		/// <summary>
		/// <see cref="StringOperations.EncloseByIfNotAlready"/> should enclose using double quotes.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void EncloseInDoubleQuotes()
		{
			// Arrange.
			const string originalString = "This is a sentence that must be enclosed in double quotes.";

			// Act.
			string enclosedValue = StringOperations.EncloseByIfNotAlready(originalString, '"');

			// Assert.
			Assert.Equal($"\"{originalString}\"", enclosedValue);
		}

		/// <summary>
		/// <see cref="StringOperations.EncloseByIfNotAlready"/> should NOT enclose using double quotes because it already was.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void EncloseInDoubleQuotesShouldDoNothing()
		{
			// Arrange.
			const string originalString = "\"This is a sentence that must be enclosed in double quotes.\"";

			// Act.
			string enclosedValue = StringOperations.EncloseByIfNotAlready(originalString, '"');

			// Assert.
			Assert.Equal(originalString, enclosedValue);
		}

		/// <summary>
		/// <see cref="StringOperations.EncloseByIfNotAlready"/> should enclose using 'A'.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void EncloseByA()
		{
			// Arrange.
			const string originalString = "This is a sentence that must be enclosed in double quotes.";

			// Act.
			string enclosedValue = StringOperations.EncloseByIfNotAlready(originalString, 'A');

			// Assert.
			Assert.Equal($"A{originalString}A", enclosedValue);
		}
	}
}
