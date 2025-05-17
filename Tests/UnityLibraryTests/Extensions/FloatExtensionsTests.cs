using Elephant.UnityLibrary.Extensions;
using Elephant.UnityLibrary.Maths;

namespace UnityLibraryTests.Extensions
{
	/// <summary>
	/// <see cref="FloatExtensions"/> tests.
	/// </summary>
	public class FloatExtensionsTests
	{
		/// <summary>
		/// Tests <see cref="FloatExtensions.IsRoughlyZero"/> with values just above/below <see cref="MathConstants.SafeGameTolerance"/>.
		/// </summary>
		[Theory]
		[InlineData(0f, true)]
		[InlineData(0.001f + MathConstants.SafeGameTolerance, false)]
		[InlineData(100f, false)]
		[InlineData(-100f, false)]
		[InlineData(float.MinValue, false)]
		[InlineData(float.MaxValue, false)]
		public void IsRoughlyZero_WithDefaultTolerance(float value, bool expected)
		{
			// Assert.
			Assert.Equal(expected, value.IsRoughlyZero());
		}

		/// <summary>
		/// Tests <see cref="FloatExtensions.IsNotRoughlyZero"/> with values just above/below <see cref="MathConstants.SafeGameTolerance"/>.
		/// </summary>
		[Theory]
		[InlineData(0f, false)]
		[InlineData(MathConstants.SafeGameTolerance + 0.001f, true)]
		[InlineData(100f, true)]
		[InlineData(-100f, true)]
		[InlineData(float.MinValue, true)]
		[InlineData(float.MaxValue, true)]
		public void IsNotRoughlyZero_WithDefaultTolerance(float value, bool expected)
		{
			// Assert.
			Assert.Equal(expected, value.IsNotRoughlyZero());
		}

		/// <summary>
		/// Tests custom epsilon values for <see cref="FloatExtensions.IsRoughlyZero"/>.
		/// </summary>
		[Theory]
		[InlineData(0f, MathConstants.SafeGameTolerance, true)]
		[InlineData(0.01f, 0.01f + MathConstants.SafeGameTolerance, true)]
		[InlineData(1f, 1f + MathConstants.SafeGameTolerance, true)]
		[InlineData(1000f, 1000f + MathConstants.SafeGameTolerance, true)]
		[InlineData(float.MaxValue - 1f, float.MaxValue, true)]
		[InlineData(0.02f, 0.01f, false)]
		[InlineData(0.01f, 0.02f, true)]
		public void IsRoughlyZero_RespectsCustomTolerance(float value, float tolerance, bool expected)
		{
			// Assert.
			Assert.Equal(expected, value.IsRoughlyZero(tolerance));
		}

		/// <summary>
		/// Tests custom epsilon values for <see cref="FloatExtensions.IsNotRoughlyZero"/>.
		/// </summary>
		[Theory]
		[InlineData(0.01f, 0.01f + MathConstants.SafeGameTolerance, false)]
		[InlineData(0.009f, 0.01f, false)]
		[InlineData(0.011f, 0.01f, true)]
		public void IsNotRoughlyZero_RespectsCustomTolerance(float value, float tolerance, bool expected)
		{
			// Assert.
			Assert.Equal(expected, value.IsNotRoughlyZero(tolerance));
		}
	}
}
