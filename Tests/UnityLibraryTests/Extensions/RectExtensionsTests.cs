using Elephant.UnityLibrary.Extensions;
using UnityEngine;

namespace UnityLibraryTests.Extensions
{
	/// <summary>
	/// <see cref="RectExtensions"/> tests.
	/// </summary>
	public class RectExtensionsTests
	{
		/// <summary>
		/// <see cref="RectExtensions.Combine"/> tests.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(new float[] { 0, 0, 10, 10, 20, 20, 5, 5 }, 0, 0, 25, 25)] // Combines two overlapping rectangles.
		[InlineData(new float[] { -10, -10, 20, 20, 15, 15, 10, 10 }, -10, -10, 35, 35)] // Combines a rectangle and another one partially overlapping it.
		[InlineData(new float[] { }, 0, 0, 0, 0)] // Tests an empty array.
		[InlineData(new float[] { 5, 5, 5, 5 }, 5, 5, 5, 5)] // Single rectangle.
		public void Combine_ReturnsCorrectCombinedRect(float[] rectData, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
		{
			// Arrange.
			List<Rect> rects = new();
			for (int i = 0; i < rectData.Length; i += 4)
				rects.Add(new Rect(rectData[i], rectData[i + 1], rectData[i + 2], rectData[i + 3]));

			// Act.
			Rect combinedRect = rects.Combine();

			// Assert.
			Assert.Equal(expectedX, combinedRect.x, float.Epsilon);
			Assert.Equal(expectedY, combinedRect.y, float.Epsilon);
			Assert.Equal(expectedWidth, combinedRect.width, float.Epsilon);
			Assert.Equal(expectedHeight, combinedRect.height, float.Epsilon);
		}
	}
}
