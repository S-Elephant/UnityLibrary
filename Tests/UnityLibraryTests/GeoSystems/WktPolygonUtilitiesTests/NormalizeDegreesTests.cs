using Elephant.UnityLibrary.GeoSystems.Wkts;

namespace UnityLibraryTests.GeoSystems.WktPolygonUtilitiesTests
{
	/// <summary>
	/// <see cref="WktPolygonUtils.NormalizeDegrees"/> tests.
	/// </summary>
	public class NormalizeDegreesTests
	{
		/// <summary>
		/// Test various inputs and outputs.
		/// </summary>
		[Theory]
		[InlineData(-360f, 0f)]
		[InlineData(-180f, 180f)]
		[InlineData(-5f, 355f)]
		[InlineData(0f, 0f)]
		[InlineData(5f, 5f)]
		[InlineData(180f, 180f)]
		[InlineData(360f, 0f)]
		[InlineData(370f, 10f)]
		[InlineData(900f, 180f)]
		public void Returns0And10(float degrees, float expected)
		{
			// Act.
			float result = WktPolygonUtils.NormalizeDegrees(degrees);

			// Assert.
			Assert.Equal(expected, result, float.Epsilon);
		}
	}
}