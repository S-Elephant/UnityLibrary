using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems
{
	/// <summary>
	/// <see cref="WktPolygonParser.ParsePolygon"/> tests.
	/// </summary>
	public class MultiPolygonTests
	{
		private const string MultiPolygon1 = "MULTIPOLYGON ((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5))";

		private static List<List<List<Vector2>>> CreateMultiPolygon1Geometry() => new()
		{
			// First polygon.
			new()
			{
				new List<Vector2> { new(30, 20), new(45, 40), new(10, 40), new(30, 20) }
			},
			// Second polygon.
			new()
			{
				new List<Vector2> { new(15, 5), new(40, 10), new(10, 20), new(5, 10), new(15, 5) }
			}
		};

		private static List<List<List<Vector2>>> CreateMultiPolygon2Geometry() => new()
		{
			//new()
			//{
			//	new List<Vector2> { new(3, 1), new(50, 100), new(3, 1) },
			//	new List<Vector2> { new(1, 500), new(10, 10), new(50, 75) }
			//}
			// First polygon.
			new()
			{
				new List<Vector2> { new(3, 1), new(50, 100), new(3, 1) }
			},
			// Second polygon.
			new()
			{
				new List<Vector2> { new(1, 500), new(10, 10), new(50, 75), new(1, 500) }
			}
		};

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
		public void EmptyWktStringResultsInEmptyGeometry(string wktString)
		{
			// Act.
			MultiPolygon multiPolygon = new(wktString);

			// Assert.
			List<List<List<Vector2>>> expected = new();
			Assert.Equal(expected, multiPolygon.Geometry);
		}

		/// <summary>
		/// Constructor using WKT string should result in expected geometry.
		/// </summary>
		[Fact]

		public void ParseWktReturnsEmptyIfEmpty()
		{
			// Act.
			MultiPolygon multiPolygon = new(MultiPolygon1);

			// Assert.
			List<List<List<Vector2>>> expected = CreateMultiPolygon1Geometry();
			Assert.Equal(expected, multiPolygon.Geometry);
		}

		/// <summary>
		/// Clone should return a new instance.
		/// </summary>
		[Fact]

		public void CloneShouldReturnNewInstance()
		{
			// Arrange.
			MultiPolygon multiPolygon = new(MultiPolygon1);

			// Act.
			MultiPolygon clonedMultiPolygon = multiPolygon.CloneAsMultiPolygon();

			clonedMultiPolygon.Geometry = new();

			// Assert.
			Assert.NotEqual(multiPolygon, clonedMultiPolygon);
			Assert.NotEqual(multiPolygon.Geometry, clonedMultiPolygon.Geometry);
		}

		/// <summary>
		/// <see cref="MultiPolygon.EqualsOtherGeometry"/> test.
		/// </summary>
		[Fact]
		public void EqualsOtherGeometryShouldReturnTrue()
		{
			// Arrange.
			MultiPolygon multiPolygon1 = new(CreateMultiPolygon1Geometry());
			MultiPolygon multiPolygon2 = new(CreateMultiPolygon1Geometry());

			// Act.
			bool equals = multiPolygon1.EqualsOtherGeometry(multiPolygon2.Geometry);

			// Assert.
			Assert.True(equals);
		}

		/// <summary>
		/// <see cref="MultiPolygon.EqualsOtherGeometry"/> test.
		/// </summary>
		[Fact]
		public void EqualsOtherGeometryShouldReturnFalseWhenNull()
		{
			// Arrange.
			MultiPolygon multiPolygon1 = new(CreateMultiPolygon1Geometry());

			// Act.
			bool equals = multiPolygon1.EqualsOtherGeometry((MultiPolygon)null);

			// Assert.
			Assert.False(equals);
		}

		/// <summary>
		/// <see cref="MultiPolygon.EqualsOtherGeometry"/> test.
		/// </summary>
		[Fact]
		public void EqualsOtherGeometryShouldReturnFalseWhenMismatch()
		{
			// Arrange.
			MultiPolygon multiPolygon1 = new(CreateMultiPolygon1Geometry());
			MultiPolygon multiPolygon2 = new(CreateMultiPolygon2Geometry());

			// Act.
			bool equals = multiPolygon1.EqualsOtherGeometry(multiPolygon2.Geometry);

			// Assert.
			Assert.False(equals);
		}
	}
}