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
			Vector2 v2 = new Vector2(x, y);
			Vector3 convertedValue = v2.ToVector3(optionalZ);

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
			Vector2 v2 = new Vector2(x, y);
			Vector4 convertedValue = v2.ToVector4(optionalZ, optionalW);

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
			Vector3 v3 = new Vector3(x, y, z);
			Vector3 convertedValue = v3.ToVector2();

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
			Vector3 v3 = new Vector3(x, y, z);
			Vector4 convertedValue = v3.ToVector4(optionalW);

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
			Vector4 v4 = new Vector4(x, y, z, w);
			Vector2 convertedValue = v4.ToVector2();

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
			Vector4 v4 = new Vector4(x, y, z, w);
			Vector3 convertedValue = v4.ToVector3();

			Vector3 expected = new(expectedX, expectedY, expectedZ);
			Assert.Equal(expected, convertedValue);
		}
	}
}
