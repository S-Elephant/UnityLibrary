using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.GeometricObjectTests
{
	/// <summary>
	/// <see cref="GeometryVertexTests"/> tests.
	/// </summary>
	public class GeometryVertexTests
	{
		/// <summary>
		/// RotateAroundPivot tests.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(0, 0, 0, 0, 90, 0, 0)] // Rotating a point around itself should do nothing.
		[InlineData(0, 1, 10, 50, 0, 0, 1)] // Rotating by 0 should do nothing.
		[InlineData(0, 1, 50, 10, 360, 0, 1)] // Rotating by 360 should do nothing.
		[InlineData(0, 1, 0, 0, 90, 1, 0)] // (0,1) clockwise --> (1,0). From top to right.
		[InlineData(1, 0, 0, 0, 90, 0, -1)] // (1,0) clockwise --> (0,-1). From right to bottom.
		[InlineData(0, -1, 0, 0, 90, -1, 0)] // (0,-1) clockwise --> (1,0). From bottom to left.
		[InlineData(-1, 0, 0, 0, 90, 0, 1)] // (-1,0) clockwise --> (0,1). From left to top.
		public void RotatePointTests(float pointX, float pointY, float pivotX, float pivotY, float angleInDegrees, float expectedX, float expectedY)
		{
			// Arrange.
			Vector2 point = new(pointX, pointY);
			Vector2 pivot = new(pivotX, pivotY);
			float angleInRadians = angleInDegrees * -Mathf.Deg2Rad; // Minus sign because positive degrees = clockwise, but positive radians = counter-clockwise.

			// Act.
			Vector2 result = GeometryVertex.RotateAroundPivot(point, angleInRadians, pivot);

			// Assert.
			Assert.Equal(result.x, expectedX, 0.0001f);
			Assert.Equal(result.y, expectedY, 0.0001f);
		}

		/// <summary>
		/// Translate tests using <see cref="Space.Self"/>.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(0, 0, 0, 0, 0, 0)]
		[InlineData(0, 0, 1, 1, 1, 1)]
		[InlineData(0, 0, float.MinValue, float.MaxValue, float.MinValue, float.MaxValue)]
		[InlineData(100, -100, 500, 500, 600, 400)]
		[InlineData(100, -100, -500, -500, -400, -600)]
		public void TranslationSelfTests(float pointX, float pointY, float translationX, float translationY, float expectedX, float expectedY)
		{
			// Arrange.
			Vector2 position = new(pointX, pointY);
			GeometryVertex vertex = new(position);
			Vector2 translation = new(translationX, translationY);

			// Act.
			vertex.Translate(translation);

			// Assert.
			Assert.Equal(new Vector2(expectedX, expectedY), vertex.Position);
		}

		/// <summary>
		/// Translate tests using <see cref="Space.World"/>.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(0, 0, 0, 0, 0, 0)]
		[InlineData(1, 1, 0, 0, 0, 0)]
		[InlineData(0, 0, 1, 1, 1, 1)]
		[InlineData(500, float.MinValue, float.MinValue, float.MaxValue, float.MinValue, float.MaxValue)]
		[InlineData(100, -100, 500, 500, 500, 500)]
		[InlineData(100, -100, -500, -500, -500, -500)]
		public void TranslationWorldTests(float pointX, float pointY, float translationX, float translationY, float expectedX, float expectedY)
		{
			// Arrange.
			Vector2 position = new(pointX, pointY);
			GeometryVertex vertex = new(position);
			Vector2 translation = new(translationX, translationY);

			// Act.
			vertex.Translate(translation, Space.World);

			// Assert.
			Assert.Equal(new Vector2(expectedX, expectedY), vertex.Position);
		}
	}
}
