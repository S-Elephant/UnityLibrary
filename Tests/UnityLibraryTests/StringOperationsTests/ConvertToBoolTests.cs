using Elephant.UnityLibrary.Common;

namespace UnityLibraryTests.StringOperationsTests
{
	/// <summary>
	/// <see cref="StringOperations.ConvertToBool(string?)"/> tests.
	/// </summary>
	public class ConvertToBoolTests
	{
		/// <summary>
		/// <see cref="StringOperations.ConvertToBool(string?)"/> tests.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData("true", true)]
		[InlineData("True", true)]
		[InlineData("false", false)]
		[InlineData("False", false)]
		[InlineData("1", true)]
		[InlineData("0", false)]
		[InlineData("", false)]
		[InlineData(null, false)]
		[InlineData("Invalid bool value", false)]
		[InlineData("bool", false)]
		[InlineData("&%#", false)]
		public void ConvertToBoolReturnsExpected(string? value, bool expectedResult)
		{
			// Act.
			bool result = StringOperations.ConvertToBool(value);

			// Assert.
			Assert.Equal(expectedResult, result);
		}
	}
}
