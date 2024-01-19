using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.WktPolygonParserTests
{
	/// <summary>
	/// <see cref="WktPolygonParser.ToWktString"/> tests.
	/// </summary>
	public class ToWktStringTests
	{
		private const string Polygon1 = "POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))";
		private const string Polygon1AsMultiPolygon = "MULTIPOLYGON(((30 10, 40 40, 20 40, 10 20, 30 10)))";
		private const string MultiPolygon1 = "MULTIPOLYGON(((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))";
		private const string MultiPolygon2 = "MULTIPOLYGON(((3 1, 50 100, 3 1)), ((1 500, 10 10, 50 75, 1 500)))";
		private const string MultiPolygon3 = "MULTIPOLYGON(((3 1, 50 100, 3 1)), ((1 500, 10 10, 50 75, 1 500)), ((10 5000, 100 100, 500 750, 10 5000)))";

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

		private static List<List<List<Vector2>>> CreateMultiPolygon3Geometry() => new()
		{
			new()
			{
				new List<Vector2> { new(3, 1), new(50, 100), new(3, 1) },
				new List<Vector2> { new(1, 500), new(10, 10), new(50, 75) },
				new List<Vector2> { new(10, 5000), new(100, 100), new(500, 750) }
			}
		};

		private static List<List<List<Vector2>>> CreateMultiPolygon4InvalidGeometry() => new()
		{
			new()
			{
				new List<Vector2> { new(3, 1), new(50, 100) }, // First and last point are not the same.
				new List<Vector2> { new(1, 500), new(10, 10), new(50, 75) },
				new List<Vector2> { new(10, 5000), new(100, 100), new(500, 750) }
			}
		};

		private static List<List<List<Vector2>>> CreateMultiPolygon5InvalidGeometry() => new()
		{
			new(), // Ring has no points.
		};

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ToWktString"/> test.
		/// </summary>
		[Fact]
		public void SimplePolygonParse()
		{
			// Act.
			string result = WktPolygonParser.ToWktString(CreatePolygon1Geometry());

			// Assert.
			Assert.Equal(Polygon1, result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ToWktString"/> test that expects a multi-polygon WKT string returned.
		/// </summary>
		[Fact]
		public void SimplePolygonAsMultiParse()
		{
			// Act.
			string result = WktPolygonParser.ToWktString(CreatePolygon1Geometry(), true);

			// Assert.
			Assert.Equal(Polygon1AsMultiPolygon, result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ToWktString"/> test for <see cref="MultiPolygon1"/>.
		/// </summary>
		[Fact]
		public void ParseWktReturnsCorrectMultiPolygon1()
		{
			// Act.
			string result = WktPolygonParser.ToWktString(CreateMultiPolygon1Geometry());

			// Assert.
			Assert.Equal(MultiPolygon1, result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ToWktString"/> test for <see cref="MultiPolygon2"/>.
		/// </summary>
		[Fact]
		public void ParseWktReturnsCorrectMultiPolygon2()
		{
			// Act.
			string result = WktPolygonParser.ToWktString(CreateMultiPolygon2Geometry());

			// Assert.
			Assert.Equal(MultiPolygon2, result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ToWktString"/> test for <see cref="MultiPolygon3"/>.
		/// </summary>
		[Fact]
		public void ParseWktReturnsCorrectMultiPolygon3()
		{
			// Act.
			string result = WktPolygonParser.ToWktString(CreateMultiPolygon3Geometry());

			// Assert.
			Assert.Equal(MultiPolygon3, result);
		}

		/// <summary>
		/// <see cref="WktPolygonParser.ToWktString"/> test using invalid input.
		/// </summary>
		[Fact]
		public void ParseWktReturnsEmptyIfInvalidInput1()
		{
			// Act.
			string result = WktPolygonParser.ToWktString(CreateMultiPolygon4InvalidGeometry(), defaultValue: WktPolygonParser.EmptyPoint);

			// Assert.
			Assert.Equal(WktPolygonParser.EmptyPoint, result);
		}

		/// <summary>
		/// <see cref="WktPolygonParser.ToWktString"/> test using another invalid input.
		/// </summary>
		[Fact]
		public void ParseWktReturnsEmptyIfInvalidInput2()
		{
			// Act.
			string result = WktPolygonParser.ToWktString(CreateMultiPolygon5InvalidGeometry(), defaultValue: WktPolygonParser.EmptyPolygon);

			// Assert.
			Assert.Equal(WktPolygonParser.EmptyPolygon, result);
		}
	}
}