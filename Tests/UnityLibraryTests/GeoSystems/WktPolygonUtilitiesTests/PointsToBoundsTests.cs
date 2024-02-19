using Elephant.UnityLibrary.GeoSystems;
using Elephant.UnityLibrary.GeoSystems.Wkts;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.WktPolygonUtilitiesTests
{
	/// <summary>
	/// <see cref="WktPolygonUtils.CalculateRelativeBounds"/> tests.
	/// </summary>
	public class PointsToBounds
	{
		/// <summary>
		/// Simple <see cref="WktPolygonUtils.CalculateRelativeBounds"/> test.
		/// </summary>
		[Fact]
		public void Returns0And10()
		{
			// Arrange.
			List<Vector2> rings = new()
			{
				new(0, 0),
				new(10, 0),
				new(10, 10),
				new(0, 10),
				new(0, 0),
			};
			List<List<List<Vector2>>> points = new() { new List<List<Vector2>> { rings } };

			// Act.
			(Vector2 MinBounds, Vector2 MaxBounds) bounds = WktPolygonUtils.PointsToBounds(points);

			// Assert.
			Assert.Equal(Vector2.zero, bounds.MinBounds);
			Assert.Equal(new Vector2(10f, 10f), bounds.MaxBounds);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonUtils.CalculateRelativeBounds"/> test.
		/// </summary>
		[Fact]
		public void Returns0And200300()
		{
			// Arrange.
			List<Vector2> rings = new()
			{
				new(100, 300),
				new(200, 0),
				new(200, 100),
				new(0, 100),
				new(0, 0),
				new(100, 300),
			};
			List<List<List<Vector2>>> points = new() { new List<List<Vector2>> { rings } };

			// Act.
			(Vector2 MinBounds, Vector2 MaxBounds) bounds = WktPolygonUtils.PointsToBounds(points);

			// Assert.
			Assert.Equal(Vector2.zero, bounds.MinBounds);
			Assert.Equal(new Vector2(0f, 0f), bounds.MinBounds);
			Assert.Equal(new Vector2(200f, 300f), bounds.MaxBounds);
		}
	}
}