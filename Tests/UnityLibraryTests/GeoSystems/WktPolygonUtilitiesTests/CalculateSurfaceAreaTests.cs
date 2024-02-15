using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.WktPolygonUtilitiesTests
{
	/// <summary>
	/// <see cref="WktPolygonUtils.NormalizeDegrees"/> tests.
	/// </summary>
	public class CalculateSurfaceAreaTests
	{
		/// <summary>
		/// <see cref="WktPolygonUtils.CalculateSurfaceArea"/> tests.
		/// </summary>
		[Theory]
		[InlineData("POLYGON((200000 500000, 200000 510000, 210000 510000, 210000 500000, 200000 500000))", 100000000)]
		[InlineData("POLYGON((220000 500000, 220000 505000, 225000 505000, 225000 500000, 220000 500000))", 25000000)] // 5x5 km square.
		[InlineData("POLYGON((230000 500000, 230000 510000, 250000 510000, 250000 500000, 230000 500000))", 200000000)] // 20x10 km square.
		[InlineData("POLYGON((240000 500000, 240000 502000, 242000 502000, 242000 500000, 240000 500000))", 4000000)] // 2x2 km square.
		[InlineData("POLYGON((245000 500000, 245000 515000, 246000 515000, 246000 500000, 245000 500000))", 15000000)] // 1x15 km square.
		[InlineData("MULTIPOLYGON(((240000 500000, 240000 502000, 242000 502000, 242000 500000, 240000 500000), (240500 500500, 241500 500500, 241500 501500, 240500 501500, 240500 500500)))", 3000000)] // 2x2 km square with a 1x1 km hole.
		public void CalculateSurfaceAreaReturnsExpected(string wktString, float expectedSurfaceAreaInMeters)
		{
			// Arrange.
			List<List<List<Vector2>>> geometry = WktPolygonParser.ParseWkt(wktString);

			// Act.
			float actualSurfaceAreaInMeters = WktPolygonUtils.CalculateSurfaceArea(geometry);

			// Assert.
			Assert.Equal(expectedSurfaceAreaInMeters, actualSurfaceAreaInMeters, 10000f);
		}
	}
}