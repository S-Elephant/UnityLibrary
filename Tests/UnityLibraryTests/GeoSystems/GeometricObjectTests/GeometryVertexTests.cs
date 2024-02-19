using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.GeometricObjectTests
{
	/// <summary>
	/// <see cref="GeometryVertex"/> tests.
	/// </summary>
	public class GeometryVertexTests
	{
		///// <summary>
		///// <see cref="GeometryLine.Recalculate"/> should mark it as non-dirty.
		///// </summary>
		//[Theory]
		//[InlineData(0f, 0f, "")]
		//public void ToJsonReturnsExpected(float positionX, float positionY, string expectedJson)
		//{
		//	// Arrange.
		//	GeometryVertex vertex = new(positionX, positionY);
		
		//	// Act.
		//	string json = vertex.ToJson();

		//	// Assert.
		//	Assert.Equal(@"{""_position"":{""x"":1.0,""y"":2.0}}", json);
		//}

		///// <summary>
		///// Assigning a new point should mark it as dirty.
		///// </summary>
		//[Fact]
		//public void AssigningNewPointMarksAsDirty()
		//{
		//	// Arrange.
		//	GeometryLine line = new(new Vector2(10, 10), new Vector2(50, 50));
		//	line.Recalculate();

		//	// Act.
		//	line.Start = new(10, 10);

		//	// Assert.
		//	Assert.True(line.IsDirty);
		//}
	}
}
