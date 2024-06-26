using Elephant.UnityLibrary.GeoSystems.Wkts;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.WktPolygonParserTests
{
	/// <summary>
	/// <see cref="WktPolygonParser.ParseWkt"/> tests.
	/// </summary>
	public class ParseTests
	{
		private const string Polygon1 = "POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))";
		private const string MultiPolygon1 = "MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))";
		private const string MultiPolygon2 = "MULTIPOLYGON(((3 1, 50 100, 3 1)), ((1 500, 10 10, 50 75)))";

		private static List<List<List<Vector2>>> CreatePolygon1Geometry() => new()
		{
			new()
			{
				new List<Vector2> { new(30, 10), new(40, 40), new(20, 40), new(10, 20), new(30, 10) },
			},
		};

		private static List<List<List<Vector2>>> CreateMultiPolygon1Geometry() => new()
		{
			// First polygon.
			new()
			{
				new List<Vector2> { new(30, 20), new(45, 40), new(10, 40), new(30, 20) },
			},
			// Second polygon.
			new()
			{
				new List<Vector2> { new(15, 5), new(40, 10), new(10, 20), new(5, 10), new(15, 5) },
			},
		};

		private static List<List<List<Vector2>>> CreateMultiPolygon2Geometry() => new()
		{
			// First polygon.
			new()
			{
				new List<Vector2> { new(3, 1), new(50, 100), new(3, 1) },
			},
			// Second polygon.
			new()
			{
				new List<Vector2> { new(1, 500), new(10, 10), new(50, 75) },
			},
		};

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ParseWkt"/> test.
		/// </summary>
		[Fact]
		public void SimplePolygonParse()
		{
			// Act.
			List<List<List<Vector2>>> result = WktPolygonParser.ParseWkt(Polygon1);

			// Assert.
			List<List<List<Vector2>>> expected = CreatePolygon1Geometry();
			Assert.Equal(expected, result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ParseWkt"/> test.
		/// </summary>
		[Fact]
		public void ParseWktReturnsCorrectMultiPolygon1()
		{
			// Act.
			List<List<List<Vector2>>> result = WktPolygonParser.ParseWkt(MultiPolygon1);

			// Assert.
			List<List<List<Vector2>>> expected = CreateMultiPolygon1Geometry();
			Assert.Equal(expected, result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ParseWkt"/> test.
		/// </summary>
		[Fact]
		public void ParseWktReturnsCorrectMultiPolygon2()
		{
			// Act.
			List<List<List<Vector2>>> result = WktPolygonParser.ParseWkt(MultiPolygon2);

			// Assert.
			List<List<List<Vector2>>> expected = CreateMultiPolygon2Geometry();
			Assert.Equal(expected, result);
		}

		/// <summary>
		/// 'Empty' WKT strings must result in empty geometry.
		/// </summary>
		[Theory]
		[InlineData(WktPolygonParser.EmptyPoint)]
		[InlineData(WktPolygonParser.EmptyLineString)]
		[InlineData(WktPolygonParser.EmptyMultiPoint)]
		[InlineData(WktPolygonParser.EmptyPolygon)]
		[InlineData(WktPolygonParser.EmptyMultiLineString)]
		[InlineData(WktPolygonParser.EmptyMultiPolygon)]
		[InlineData(WktPolygonParser.EmptyGeometryCollection)]
		public void ParseWktReturnsEmptyIfEmpty(string wktString)
		{
			// Act.
			List<List<List<Vector2>>> result = WktPolygonParser.ParseWkt(wktString);

			// Assert.
			List<List<List<Vector2>>> expected = new();
			Assert.Equal(expected, result);
		}

		/// <summary>
		/// Multi-polygon should be parsed, resulting in exactly 2 polygons.
		/// </summary>
		[Fact]
		public void IsMultiPolygonParsedCorrectly()
		{
			// Arrange.
			const string wktString = "MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)), ((20 35, 10 30, 10 10, 30 5, 45 20, 20 35), (30 20, 20 15, 20 25, 30 20)))";

			// Act.
			List<List<List<Vector2>>> result = WktPolygonParser.ParseWkt(wktString);

			// Assert.
			Assert.Equal(2, result.Count); // 2 polygons.
		}

		/// <summary>
		/// Polygon should be parsed, resulting in exactly 1 polygon with 2 rings.
		/// </summary>
		[Fact]
		public void IsPolygonWithHoleParsedCorrectly()
		{
			// Arrange.
			const string wktString = "POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10), (20 30, 35 35, 30 20, 20 30))";

			// Act.
			List<List<List<Vector2>>> result = WktPolygonParser.ParseWkt(wktString);

			// Assert.
			Assert.Single(result); // 1 polygon.
			Assert.Equal(2, result[0].Count); // 2 rings.
		}
	}
}