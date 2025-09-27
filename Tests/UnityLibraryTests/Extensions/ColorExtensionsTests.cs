using Elephant.UnityLibrary.Extensions;
using UnityEngine;

namespace UnityLibraryTests.Extensions
{
	/// <summary>
	/// Tests for color extension methods that modify HSV channels
	/// while preserving alpha.
	/// </summary>
	public class ColorExtensionsTests
	{
		/// <summary>
		/// Saturate test case data.
		/// </summary>
		public static TheoryData<Color, float, Color> SaturateTestCases => new()
		{
			{ new Color(0.5f, 0.5f, 0.5f, 0.3f), 0f, new Color(0.5f, 0.5f, 0.5f, 0.3f) },
			{ new Color(1, 0.5f, 0.5f, 0.7f), 1f, new Color(1, 0, 0, 0.7f) },
		};

		/// <summary>
		/// Verify that <see cref="ColorExtensions.Saturate(Color, float)"/>
		/// modifies saturation while preserving alpha.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[MemberData(nameof(SaturateTestCases))]
		public void Saturate_ModifiesSaturation_PreservesAlpha(Color input, float saturation, Color expected)
		{
			// Act.
			Color result = input.Saturate(saturation);

			// Assert.
			Assert.Equal(expected.r, result.r, 1);
			Assert.Equal(expected.g, result.g, 1);
			Assert.Equal(expected.b, result.b, 1);
			Assert.Equal(input.a, result.a, 1);
		}

		/// <summary>
		/// Verify that <see cref="ColorExtensions.Alpha(Color, float)"/> modifies
		/// only the alpha channel.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void Alpha_ModifiesOnlyAlpha()
		{
			// Arrange.
			Color color = new(1, 0, 0, 0.5f);

			// Act.
			Color result = color.Alpha(0.8f);

			// Assert.
			Assert.Equal(color.r, result.r, 1);
			Assert.Equal(color.g, result.g, 1);
			Assert.Equal(color.b, result.b, 1);
			Assert.Equal(0.8f, result.a, 1);
		}

		/// <summary>
		/// Verify that <see cref="ColorExtensions.Lighten(Color, float)"/> increases
		/// the brightness while preserving the alpha value.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void Brighten_IncreasesValue_PreservesAlpha()
		{
			// Arrange.
			Color darkRed = new(0.5f, 0, 0, 0.3f);

			// Act.
			Color result = darkRed.Lighten(0.5f);

			// Assert.
			Assert.Equal(0.75f, result.r, 1); // (0.5 + 1.0)/2 = 0.75
			Assert.Equal(0.5f, result.g, 1); // (0 + 1.0)/2 = 0.5
			Assert.Equal(0.5f, result.b, 1); // (0 + 1.0)/2 = 0.5
			Assert.Equal(0.3f, result.a, 1);
		}
	}
}
