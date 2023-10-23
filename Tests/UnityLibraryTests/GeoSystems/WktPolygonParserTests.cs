using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems
{
	/// <summary>
	/// <see cref="WktPolygonParser"/> tests.
	/// </summary>
	public class WktPolygonParserTests
	{
		/// <summary>
		/// Simple <see cref="WktPolygonParser.ParseWKT"/> test.
		/// </summary>
		[Fact]
		public void SimplePolygonParse()
		{
			// Arrange.
			List<Vector2> rings = new()
			{
				new(30, 10),
				new(40, 40),
				new(40, 40),
				new(20, 20),
				new(10, 10),
				new(30, 10),
			};
			List<List<List<Vector2>>> expected = new() { new List<List<Vector2>> { rings } };

			// Act.
			List<List<List<Vector2>>> result = WktPolygonParser.ParseWKT("POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))");

			// Assert.
			Assert.Single(result);
			Assert.Single(result[0]);
			Assert.Equal(5, result[0][0].Count);
			for (int i = 0; i < expected.Count; i++)
				Assert.Equal(expected[0][0][i], result[0][0][i]);
		}
	}
}