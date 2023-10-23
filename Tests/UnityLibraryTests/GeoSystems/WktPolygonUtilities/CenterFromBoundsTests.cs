using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.WktPolygonUtilities
{
	/// <summary>
	/// <see cref="WktPolygonUtils.CenterFromBounds"/> tests.
	/// </summary>
	public class CenterFromBoundsTests
	{
		/// <summary>
		/// Simple <see cref="WktPolygonUtils.CenterFromBounds"/> test.
		/// </summary>
		[Fact]
		public void Returns0()
		{
			// Arrange.
			Vector2 minimumBounds = new(-100, -100);
			Vector2 maximumBounds = new(100, 100);

			// Act.
			Vector2 result = WktPolygonUtils.CenterFromBounds(minimumBounds, maximumBounds);

			// Assert.
			Assert.Equal(Vector2.zero, result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonUtils.CenterFromBounds"/> test.
		/// </summary>
		[Fact]
		public void Returns100()
		{
			// Arrange.
			Vector2 minimumBounds = new(100, 100);
			Vector2 maximumBounds = new(100, 100);

			// Act.
			Vector2 result = WktPolygonUtils.CenterFromBounds(minimumBounds, maximumBounds);

			// Assert.
			Assert.Equal(new Vector2(100, 100), result);
		}

				/// <summary>
		/// Simple <see cref="WktPolygonUtils.CenterFromBounds"/> test.
		/// </summary>
		[Fact]
		public void ReturnsAlso100()
		{
			// Arrange.
			Vector2 minimumBounds = new(0, 0);
			Vector2 maximumBounds = new(200, 300);

			// Act.
			Vector2 result = WktPolygonUtils.CenterFromBounds(minimumBounds, maximumBounds);

			// Assert.
			Assert.Equal(new Vector2(100, 150), result);
		}

		/// <summary>
		/// Simple <see cref="WktPolygonUtils.CenterFromBounds"/> test.
		/// </summary>
		[Fact]
		public void ReturnsNegative50()
		{
			// Arrange.
			Vector2 minimumBounds = new(-300, -300);
			Vector2 maximumBounds = new(200, 200);

			// Act.
			Vector2 result = WktPolygonUtils.CenterFromBounds(minimumBounds, maximumBounds);

			// Assert.
			Assert.Equal(new Vector2(-50, -50), result);
		}
	}
}