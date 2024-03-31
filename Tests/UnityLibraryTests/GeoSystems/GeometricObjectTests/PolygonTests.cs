using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.GeometricObjectTests
{
	/// <summary>
	/// <see cref="Polygon"/> tests.
	/// </summary>
	public class PolygonTests
	{
		/// <summary>
		/// Test <see cref="Polygon.AllVertices"/>.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void AllVerticesReturnsCorrectly()
		{
			// Arrange.
			Polygon polygon = Polygon.FromWktString("POLYGON ((10 0, 0 -10, -10 0, 0 10, 10 0))");

			// Act.
			List<GeometryVertex> allVertices = polygon.AllVertices();

			// Assert.
			Assert.Contains(allVertices, x => x.Position == new Vector2(10, 0));
			Assert.Contains(allVertices, x => x.Position == new Vector2(0, -10));
			Assert.Contains(allVertices, x => x.Position == new Vector2(-10, 0));
			Assert.Contains(allVertices, x => x.Position == new Vector2(0, 10));
		}

		/// <summary>
		/// Test calculate center.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void CalculateCenterWorks()
		{
			// Arrange.
			Polygon polygon = Polygon.FromWktString("POLYGON ((10 0, 0 -10, -10 0, 0 10, 10 0))");

			// Act.
			Vector2 center = polygon.Center;

			// Assert.
			Assert.Equal(Vector2.zero, center);
		}

		/// <summary>
		/// Test calculate center.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void TranslateWorldToZero()
		{
			// Arrange.
			Polygon polygon = Polygon.FromWktString("POLYGON ((10 0, 0 -10, -10 0, 0 10, 10 0))");

			// Act.
			polygon.Translate(Vector2.zero, Space.World);
			Vector2 center = polygon.Center;

			// Assert.
			Assert.Equal(Vector2.zero, center);
		}

		/// <summary>
		/// Test calculate center.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void TranslateWorldToZeroTwice()
		{
			// Arrange.
			Polygon polygon = Polygon.FromWktString("POLYGON ((10 0, 0 -10, -10 0, 0 10, 10 0))");

			// Act.
			polygon.Translate(Vector2.zero, Space.World);
			polygon.Translate(Vector2.zero, Space.World);
			Vector2 center = polygon.Center;

			// Assert.
			Assert.Equal(Vector2.zero, center);
		}

		/// <summary>
		/// Test calculate center.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void TranslateWorldToTen()
		{
			// Arrange.
			Polygon polygon = Polygon.FromWktString("POLYGON ((10 0, 0 -10, -10 0, 0 10, 10 0))");

			// Act.
			polygon.Translate(new Vector2(10f, 10f), Space.World);
			Vector2 center = polygon.Center;

			// Assert.
			Assert.Equal(new Vector2(10f, 10f), center);
		}

		/// <summary>
		/// Test calculate center.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void TranslateWorldToTenTwice()
		{
			// Arrange.
			Polygon polygon = Polygon.FromWktString("POLYGON ((10 0, 0 -10, -10 0, 0 10, 10 0))");

			// Act.
			polygon.Translate(new Vector2(10f, 10f), Space.World);
			polygon.Translate(new Vector2(10f, 10f), Space.World);
			Vector2 center = polygon.Center;

			// Assert.
			Assert.Equal(new Vector2(10f, 10f), center);
		}

		/// <summary>
		/// Test calculate center.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void TranslateWorldThreeTimes()
		{
			// Arrange.
			Polygon polygon = Polygon.FromWktString("POLYGON ((10 0, 0 -10, -10 0, 0 10, 10 0))");

			// Act.
			polygon.Translate(new Vector2(10f, -10f), Space.World);
			polygon.Translate(new Vector2(100f, -50f), Space.World);
			polygon.Translate(new Vector2(10f, 10f), Space.World);
			Vector2 center = polygon.Center;

			// Assert.
			Assert.Equal(new Vector2(10f, 10f), center);
		}
	}
}
