using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.WktPolygonParserTests
{
	/// <summary>
	/// <see cref="WktPolygonParser.ParsePolygon"/> tests.
	/// </summary>
	public class TranslateTests
	{
		private const string Polygon1 = "POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))";
		private const string MultiPolygon1 = "MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))";
		private const string MultiPolygon2 = "MULTIPOLYGON(((3 1, 50 100, 3 1)), ((1 500, 10 10, 50 75)))";

		private static List<List<List<Vector2>>> CreatePolygon1Geometry() => new()
		{
			new()
			{
				new List<Vector2> { new(30, 10), new(40, 40), new(20, 40), new(10, 20), new(30, 10) }
			}
		};

		private static List<List<List<Vector2>>> CreateMultiPolygon1Geometry() => new()
		{
			new()
			{
				new List<Vector2> { new(30, 20), new(45, 40), new(10, 40), new(30, 20) },
				new List<Vector2> { new(15, 5), new(40, 10), new(10, 20), new(5, 10), new(15, 5) }
			}
		};

		private static List<List<List<Vector2>>> CreateMultiPolygon2Geometry() => new()
		{
			new()
			{
				new List<Vector2> { new(3, 1), new(50, 100), new(3, 1) },
				new List<Vector2> { new(1, 500), new(10, 10), new(50, 75) }
			}
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
	}
}