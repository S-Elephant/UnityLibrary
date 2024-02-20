using Elephant.UnityLibrary.GeoSystems.Wkts;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.WktPolygonUtilitiesTests
{
	/// <summary>
	/// <see cref="WktPolygonUtils.Translate(string,Vector2)"/> and it's overload tests.
	/// </summary>
	public class TranslateTests
	{
		private const string MultiPolygon1 = "MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))";
		private const string MultiPolygon2 = "MULTIPOLYGON(((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))";

		/// <summary>
		/// Simple <see cref="WktPolygonUtils.Translate(string,Vector2)"/> test.
		/// </summary>
		[Theory]
		[InlineData(MultiPolygon1)]
		[InlineData(MultiPolygon2)]
		public void Translate0ReturnsOriginal(string wktString)
		{
			// Act.
			string result = WktPolygonUtils.Translate(wktString, Vector2.zero);

			// Assert.
			Assert.Equal(wktString, result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonUtils.Translate(string,Vector2)"/> test.
		/// </summary>
		[Theory]
		[InlineData(MultiPolygon1, 10, 20, "MULTIPOLYGON(((40 40, 55 60, 20 60, 40 40))), (((25 25, 50 30, 20 40, 15 30, 25 25)))")]
		[InlineData(MultiPolygon2, -5, 7, "MULTIPOLYGON(((25 27, 40 47, 5 47, 25 27))), (((10 12, 35 17, 5 27, 0 17, 10 12)))")]
		public void TranslateReturnsAsExpected(string wktString, float translationX, float translationY,  string expected)
		{
			// Act.
			string result = WktPolygonUtils.Translate(wktString, new Vector2(translationX, translationY));

			// Assert.
			Assert.Equal(expected, result);
		}
	}
}