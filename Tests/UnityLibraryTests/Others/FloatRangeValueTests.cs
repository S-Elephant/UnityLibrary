using Elephant.UnityLibrary.Extensions;
using Elephant.UnityLibrary.Maths;
using Elephant.UnityLibrary.Other;

namespace UnityLibraryTests.Others
{
	/// <summary>
	/// <see cref="FloatRangeValue"/> tests.
	/// </summary>
	public class FloatRangeValueTests
	{
		/// <summary>
		/// <see cref="FloatRangeValue.Value"/> tests.
		/// </summary>
		[Theory]
		[InlineData(0f, 0f, 0f, 0f)]
		[InlineData(-1f, 0f, 0f, 0f)]
		[InlineData(1f, 0f, 0f, 0f)]
		[InlineData(-1f, -2f, 0f, -1f)]
		[InlineData(-2f, -2f, 0f, -2f)]
		[InlineData(-5f, -2f, 10f, -2f)]
		[InlineData(10f, -2f, 10f, 10f)]
		[InlineData(15f, -2f, 10f, 10f)]
		[InlineData(float.MinValue, -2f, 10f, -2f)]
		[InlineData(float.MaxValue, -2f, 10f, 10f)]
		public void ValueTests(float value, float min, float max, float expected)
		{
			// Arrange.
			FloatRangeValue x = new(value, min, max);

			// Assert.
			Assert.Equal(expected, x.Value, MathConstants.SafeGameTolerance);
		}

		/// <summary>
		/// <see cref="FloatRangeValue.IsMaxValue"/> tests.
		/// </summary>
		[Theory]
		[InlineData(0f, 0f, 0f, true)]
		[InlineData(1000f, 1000f, 1000f, true)]
		[InlineData(float.MinValue, float.MinValue, float.MinValue, true)]
		[InlineData(float.MaxValue, float.MaxValue, float.MaxValue, true)]
		[InlineData(-1f, 0f, 0f, true)]
		[InlineData(1f, 0f, 0f, true)]
		[InlineData(-1f, -2f, 0f, false)]
		[InlineData(-2f, -2f, 0f, false)]
		[InlineData(-5f, -2f, 10f, false)]
		[InlineData(10f, -2f, 10f, true)]
		[InlineData(15f, -2f, 10f, true)]
		[InlineData(float.MinValue, -2f, 10f, false)]
		[InlineData(float.MaxValue, -2f, 10f, true)]
		public void IsMaxValueTests(float value, float min, float max, bool expectedIsMaxValue)
		{
			// Arrange.
			FloatRangeValue x = new(value, min, max);

			// Act.
			bool isMaxValue = x.IsMaxValue;

			// Assert.
			Assert.Equal(expectedIsMaxValue, isMaxValue);
		}

		/// <summary>
		/// <see cref="FloatRangeValue.IsMinValue"/> tests.
		/// </summary>
		[Theory]
		[InlineData(0f, 0f, 0f, true)]
		[InlineData(1000f, 1000f, 1000f, true)]
		[InlineData(float.MinValue, float.MinValue, float.MinValue, true)]
		[InlineData(float.MaxValue, float.MaxValue, float.MaxValue, true)]
		[InlineData(-1f, 0f, 0f, true)]
		[InlineData(1f, 0f, 0f, true)]
		[InlineData(-1f, -2f, 0f, false)]
		[InlineData(-2f, -2f, 0f, true)]
		[InlineData(-5f, -2f, 10f, true)]
		[InlineData(10f, -2f, 10f, false)]
		[InlineData(15f, -2f, 10f, false)]
		[InlineData(float.MinValue, -2f, 10f, true)]
		[InlineData(float.MaxValue, -2f, 10f, false)]
		public void IsMinValueTests(float value, float min, float max, bool expectedIsMinValue)
		{
			// Arrange.
			FloatRangeValue x = new(value, min, max);

			// Act.
			bool isMinValue = x.IsMinValue;

			// Assert.
			Assert.Equal(expectedIsMinValue, isMinValue);
		}

		/// <summary>
		/// <see cref="FloatRangeValue.SetToMinValue"/> tests.
		/// </summary>
		[Theory]
		[InlineData(0f, 0f, 0f, 0f)]
		[InlineData(1000f, 1000f, 1000f, 1000f)]
		[InlineData(-1000f, -1000f, -1000f, -1000f)]
		[InlineData(-1f, 0f, 0f, 0f)]
		[InlineData(1f, 0f, 0f, 0f)]
		[InlineData(-1f, -2f, 0f, -2f)]
		[InlineData(-2f, -2f, 0f, -2f)]
		[InlineData(-5f, -2f, 10f, -2f)]
		[InlineData(10f, -2f, 10f, -2f)]
		[InlineData(15f, -2f, 10f, -2f)]
		[InlineData(float.MinValue, -2f, 10f, -2f)]
		[InlineData(float.MaxValue, -2f, 10f, -2f)]
		public void SetToMinValueTests(float value, float min, float max, float expected)
		{
			// Arrange.
			FloatRangeValue x = new(value, min, max);

			// Act.
			x.SetToMinValue();

			// Assert.
			Assert.Equal(expected, x.Value);
		}

		/// <summary>
		/// <see cref="FloatRangeValue.SetToMaxValue"/> tests.
		/// </summary>
		[Theory]
		[InlineData(0f, 0f, 0f, 0f)]
		[InlineData(1000f, 1000f, 1000f, 1000f)]
		[InlineData(-1000f, -1000f, -1000f, -1000f)]
		[InlineData(-1f, 0f, 0f, 0f)]
		[InlineData(1f, 0f, 0f, 0f)]
		[InlineData(-1f, -2f, 0f, 0f)]
		[InlineData(-2f, -2f, 0f, 0f)]
		[InlineData(-5f, -2f, 10f, 10f)]
		[InlineData(10f, -2f, 10f, 10f)]
		[InlineData(15f, -2f, 10f, 10f)]
		[InlineData(float.MinValue, -2f, 10f, 10f)]
		[InlineData(float.MaxValue, -2f, 10f, 10f)]
		public void SetToMaxValueTests(float value, float min, float max, float expected)
		{
			// Arrange.
			FloatRangeValue x = new(value, min, max);

			// Act.
			x.SetToMaxValue();

			// Assert.
			Assert.Equal(expected, x.Value);
		}
	}
}
