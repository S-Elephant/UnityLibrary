using Elephant.UnityLibrary.Extensions;
using UnityEngine;

namespace UnityLibraryTests.Extensions
{
	/// <summary>
	/// <see cref="Vectors"/> tests.
	/// </summary>
	public class VectorsExtensionsTests
	{
		/// <summary>
		/// <see cref="Vectors.ToVector3(Vector2, float)"/> tests.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(0f, 0f, 0f, 0f, 0f, 0f)]
		[InlineData(0f, -10f, 0f, 0f, -10f, 0f)]
		[InlineData(0f, -10f, 1000f, 0f, -10f, 1000f)]
		[InlineData(float.MinValue, float.MinValue, 0f, float.MinValue, float.MinValue, 0f)]
		[InlineData(float.MaxValue, float.MaxValue, 0f, float.MaxValue, float.MaxValue, 0f)]
		public void V2ToVector3ReturnsExpected(float x, float y, float optionalZ, float expectedX, float expectedY, float expectedZ)
		{
			// Arrange.
			Vector2 v2 = new(x, y);

			// Act.
			Vector3 convertedValue = v2.ToVector3(optionalZ);

			// Assert.
			Vector3 expected = new(expectedX, expectedY, expectedZ);
			Assert.Equal(expected, convertedValue);
		}

		/// <summary>
		/// <see cref="Vectors.ToVector4(Vector2, float, float)"/> tests.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f)]
		[InlineData(0f, -10f, 0f, 0f, 0f, -10f, 0f, 0f)]
		[InlineData(0f, -10f, 1000f, 0f, 0f, -10f, 1000f, 0f)]
		[InlineData(0f, -10f, 1000f, 150f, 0f, -10f, 1000f, 150f)]
		[InlineData(float.MinValue, float.MinValue, 0f, 0f, float.MinValue, float.MinValue, 0f, 0f)]
		[InlineData(float.MaxValue, float.MaxValue, 0f, 0f, float.MaxValue, float.MaxValue, 0f, 0f)]
		public void V2ToVector4ReturnsExpected(float x, float y, float optionalZ, float optionalW, float expectedX, float expectedY, float expectedZ, float expectedW)
		{
			// Arrange.
			Vector2 v2 = new(x, y);

			// Act.
			Vector4 convertedValue = v2.ToVector4(optionalZ, optionalW);

			// Assert.
			Vector4 expected = new(expectedX, expectedY, expectedZ, expectedW);
			Assert.Equal(expected, convertedValue);
		}

		/// <summary>
		/// <see cref="Vectors.ToVector2(Vector3)"/> tests.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(0f, 0f, 0f, 0f, 0f)]
		[InlineData(0f, -10f, 0f, 0f, -10f)]
		[InlineData(0f, -10f, 1000f, 0f, -10f)]
		[InlineData(float.MinValue, float.MinValue, 0f, float.MinValue, float.MinValue)]
		[InlineData(float.MaxValue, float.MaxValue, 0f, float.MaxValue, float.MaxValue)]
		public void V3ToVector2ReturnsExpected(float x, float y, float z, float expectedX, float expectedY)
		{
			// Arrange.
			Vector3 v3 = new(x, y, z);

			// Act.
			Vector3 convertedValue = v3.ToVector2();

			// Assert.
			Vector3 expected = new(expectedX, expectedY, 0f);
			Assert.Equal(expected, convertedValue);
		}

		/// <summary>
		/// <see cref="Vectors.ToVector4(Vector3, float)"/> tests.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f)]
		[InlineData(0f, -10f, 0f, 0f, 0f, -10f, 0f, 0f)]
		[InlineData(0f, -10f, 1000f, 0f, 0f, -10f, 1000f, 0f)]
		[InlineData(0f, -10f, 1000f, 150f, 0f, -10f, 1000f, 150f)]
		[InlineData(float.MinValue, float.MinValue, 0f, 0f, float.MinValue, float.MinValue, 0f, 0f)]
		[InlineData(float.MaxValue, float.MaxValue, 0f, 0f, float.MaxValue, float.MaxValue, 0f, 0f)]
		public void V3ToVector4ReturnsExpected(float x, float y, float z, float optionalW, float expectedX, float expectedY, float expectedZ, float expectedW)
		{
			// Arrange.
			Vector3 v3 = new(x, y, z);

			// Act.
			Vector4 convertedValue = v3.ToVector4(optionalW);

			// Assert.
			Vector4 expected = new(expectedX, expectedY, expectedZ, expectedW);
			Assert.Equal(expected, convertedValue);
		}

		/// <summary>
		/// <see cref="Vectors.ToVector2(Vector4)"/> tests.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(0f, 0f, 0f, 0f, 0f, 0f)]
		[InlineData(0f, -10f, 0f, 0f, -0f, -10f)]
		[InlineData(0f, -10f, 1000f, 0f, 0f, -10f)]
		[InlineData(float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue)]
		[InlineData(float.MaxValue, float.MaxValue, float.MinValue, float.MaxValue, float.MaxValue, float.MaxValue)]
		public void V4ToVector2ReturnsExpected(float x, float y, float z, float w, float expectedX, float expectedY)
		{
			// Arrange.
			Vector4 v4 = new(x, y, z, w);

			// Act.
			Vector2 convertedValue = v4.ToVector2();

			// Assert.
			Vector2 expected = new(expectedX, expectedY);
			Assert.Equal(expected, convertedValue);
		}

		/// <summary>
		/// <see cref="Vectors.ToVector3(Vector4)"/> tests.
		/// </summary>
		[Theory]
		[SpeedVeryFast, UnitTest]
		[InlineData(0f, 0f, 0f, 0f, 0f, 0f, 0f)]
		[InlineData(0f, -10f, 0f, 0f, 0f, -10f, 0f)]
		[InlineData(0f, -10f, 1000f, 0f, 0f, -10f, 1000f)]
		[InlineData(0f, -10f, 1000f, 150f, 0f, -10f, 1000f)]
		[InlineData(float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue)]
		[InlineData(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue)]
		public void V4ToVector3ReturnsExpected(float x, float y, float z, float w, float expectedX, float expectedY, float expectedZ)
		{
			// Arrange.
			Vector4 v4 = new(x, y, z, w);

			// Act.
			Vector3 convertedValue = v4.ToVector3();

			// Assert.
			Vector3 expected = new(expectedX, expectedY, expectedZ);
			Assert.Equal(expected, convertedValue);
		}
	}
}
