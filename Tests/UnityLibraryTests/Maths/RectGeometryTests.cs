using Elephant.UnityLibrary.Maths;
using UnityEngine;

namespace UnityLibraryTests.Maths
{
	/// <summary>
	/// <see cref="RectGeometry.CalcMaxRectsInRect(UnityEngine.Rect, UnityEngine.Rect)"/> tests.
	/// </summary>
	public class RectGeometryTests
	{
		/// <summary>
		/// Tests that the calculation returns the correct count when squares fit perfectly into the container (no partial tiles).
		/// </summary>
		[Fact]
		public void CalcMaxSquaresInRect_FitsPerfectly_ReturnsCorrectCount()
		{
			// Arrange.
			Rect container = new(0, 0, 10, 10);
			Rect square = new(0, 0, 2, 2);

			// Act.
			int result = RectGeometry.CalcMaxRectsInRect(container, square);

			// Assert.
			Assert.Equal(25, result); // (10/2) * (10/2) = 25.
		}

		/// <summary>
		/// Tests that the calculation correctly floors partial tiles when squares don't fit perfectly into the container.
		/// </summary>
		[Fact]
		public void CalcMaxSquaresInRect_PartialFit_ReturnsFlooredCount()
		{
			// Arrange.
			Rect container = new(0, 0, 10, 10);
			Rect square = new(0, 0, 3, 3);

			// Act.
			int result = RectGeometry.CalcMaxRectsInRect(container, square);

			// Assert.
			Assert.Equal(9, result); // (10/3=3.33→3) * (10/3=3.33→3) = 9.
		}

		/// <summary>
		/// Tests that the calculation works correctly with rectangular (non-square) containers.
		/// </summary>
		[Fact]
		public void CalcMaxSquaresInRect_RectangleContainer_ReturnsCorrectCount()
		{
			// Arrange.
			Rect container = new(0, 0, 10, 5);
			Rect square = new(0, 0, 2, 2);

			// Act.
			int result = RectGeometry.CalcMaxRectsInRect(container, square);

			// Assert.
			Assert.Equal(10, result); // (10/2=5) * (5/2=2.5→2) = 10.
		}

		/// <summary>
		/// Tests that an error value (-1) is returned when attempting to calculate with a zero-sized rectangle.
		/// </summary>
		[Fact]
		public void CalcMaxSquaresInRect_ZeroRectSize_ReturnsErrorValue()
		{
			// Arrange.
			Rect container = new(0, 0, 10, 10);
			Rect invalidSquare = new(0, 0, 0, 0);

			// Act.
			int result = RectGeometry.CalcMaxRectsInRect(container, invalidSquare);

			// Assert.
			Assert.Equal(-1, result); // Expects an error value.
		}

		/// <summary>
		/// Tests that an error value (-1) is returned when attempting to calculate with a negative-sized rectangle.
		/// </summary>
		[Fact]
		public void CalcMaxSquaresInRect_NegativeRectSize_ReturnsErrorValue()
		{
			// Arrange.
			Rect container = new(0, 0, 10, 10);
			Rect invalidSquare = new(0, 0, -1, 1);

			// Act.
			int result = RectGeometry.CalcMaxRectsInRect(container, invalidSquare);

			// Assert.
			Assert.Equal(-1, result); // Expects an error value.
		}
	}
}