using Elephant.UnityLibrary.GeoSystems;
using Elephant.UnityLibrary.GeoSystems.Wkts;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.WktPolygonParserTests
{
	/// <summary>
	/// <see cref="WktPolygonParser.IsMultiPolygon"/> tests.
	/// </summary>
	public class IsMultiPolygonTests
	{
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
		public void IfPolygonReturnsFalse()
		{
			// Arrange.
			List<List<List<Vector2>>> geometry = CreatePolygon1Geometry();

			// Act.
			bool result = WktPolygonParser.IsMultiPolygon(geometry);

			// Assert.
			Assert.False(result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ParseWkt"/> test.
		/// </summary>
		[Fact]
		public void IfMultiPolygonReturnsTrue()
		{
			// Arrange.
			List<List<List<Vector2>>> geometry = CreateMultiPolygon1Geometry();

			// Act.
			bool result = WktPolygonParser.IsMultiPolygon(geometry);

			// Assert.
			Assert.True(result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonParser.ParseWkt"/> test.
		/// </summary>
		[Fact]
		public void IfMultiPolygonReturnsTrue2()
		{
			// Arrange.
			List<List<List<Vector2>>> geometry = CreateMultiPolygon2Geometry();

			// Act.
			bool result = WktPolygonParser.IsMultiPolygon(geometry);

			// Assert.
			Assert.True(result);
		}
	}
}