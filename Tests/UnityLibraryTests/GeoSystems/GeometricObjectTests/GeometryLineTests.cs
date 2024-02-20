using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.GeometricObjectTests
{
	/// <summary>
	/// <see cref="GeometryLine"/> tests.
	/// </summary>
	public class GeometryLineTests
	{
		/// <summary>
		/// <see cref="GeometryLine.Recalculate"/> should mark it as non-dirty.
		/// </summary>
		[Fact]
		public void RecalculateMarksAsNonDirty()
		{
			// Arrange.
			GeometryLine line = new(new Vector2(10, 10), new Vector2(50, 50));

			// Act.
			line.Recalculate();

			// Assert.
			Assert.False(line.IsDirty);
		}

		/// <summary>
		/// Assigning a new point should mark it as dirty.
		/// </summary>
		[Fact]
		public void AssigningNewPointMarksAsDirty()
		{
			// Arrange.
			GeometryLine line = new(new Vector2(10, 10), new Vector2(50, 50));
			line.Recalculate();

			// Act.
			line.Start = new(10, 10);

			// Assert.
			Assert.True(line.IsDirty);
		}

		/// <summary>
		/// Changing an existing point should mark it as dirty.
		/// </summary>
		[Fact]
		public void UpdatingExistingPointMarksAsDirty()
		{
			// Arrange.
			GeometryLine line = new(new Vector2(10, 10), new Vector2(50, 50));
			line.Recalculate();

			// Act.
			line.Start.Position = new Vector2(10, 11);

			// Assert.
			Assert.True(line.IsDirty); // this fails because the vertex is dirty but parents are not yet updated...
		}

		/// <summary>
		/// Changing an existing point to the same value should not mark it as dirty.
		/// </summary>
		[Fact]
		public void UpdatingExistingPointWithSameValueMarksAsDirty()
		{
			// Arrange.
			GeometryLine line = new(new Vector2(10, 10), new Vector2(50, 50));
			line.Recalculate();

			// Act.
			line.Start.Position = new Vector2(10, 10);

			// Assert.
			Assert.False(line.IsDirty);
		}

		/// <summary>
		/// Changing an existing point to the same value should not mark it as dirty.
		/// </summary>
		[Fact]
		public void ParentIsRemovedAfterParentDispose()
		{
			// Arrange.
			GeometryVertex start = new();
			GeometryVertex end = new();
			GeometryLine line = new(start, end);
			line.Recalculate();

			// Act.
			line.Dispose();

			// Assert.
			Assert.Empty(start.ParentsAsReadonly());
			Assert.Empty(end.ParentsAsReadonly());
		}

		/// <summary>
		/// Test <see cref="GeometryLine.CalculateCenter"/> tests.
		/// </summary>
		[Theory]
		[InlineData(-10, 0, 10, 0, 0, 0)]
		[InlineData(-10, 0, 0, 0, -5, 0)]
		[InlineData(100, 100, 0, -50, 50, 25)]
		public void CenterIsCalculatedCorrectly(float startX, float startY, float endX, float endY, float expectedCenterX, float expectedCenterY)
		{
			// Arrange.
			GeometryLine line = new(new Vector2(startX, startY), new Vector2(endX, endY));

			// Act.
			Vector2 center = line.Center;

			// Assert.
			Assert.Equal(expectedCenterX, center.x, 0.001f);
			Assert.Equal(expectedCenterY, center.y, 0.001f);
		}

		/// <summary>
		/// Translate test using <see cref="Space.Self"/>.
		/// </summary>
		[Fact]
		public void SimpleTranslationSelf()
		{
			// Arrange.
			GeometryLine line = new(new Vector2(10f, 10f), new Vector2(100f, 10f));
			Vector2 translation = new(0f, 10f);

			// Act.
			line.Translate(translation);

			// Assert.
			Assert.Equal(new GeometryLine(new Vector2(10f, 20f), new Vector2(100f, 20f)), line);
		}

		/// <summary>
		/// Translate test using <see cref="Space.World"/>.
		/// </summary>
		[Fact]
		public void SimpleTranslationWorld()
		{
			// Arrange.
			GeometryLine line = new(new Vector2(0f, 10f), new Vector2(100f, 10f));
			Vector2 translation = new(0f, 10f);

			// Act.
			line.Translate(translation, Space.World);

			// Assert.
			Assert.Equal(new GeometryLine(new Vector2(-50f, 10f), new Vector2(50f, 10f)), line);
		}

		/// <summary>
		/// Translate test using <see cref="Space.World"/>.
		/// </summary>
		[Fact]
		public void SimpleTranslationWorld2()
		{
			// Arrange.
			GeometryLine line = new(new Vector2(0f, 100f), new Vector2(100f, 10f));
			Vector2 translation = new(0f, 10f);

			// Act.
			line.Translate(translation, Space.World);

			// Assert.
			Assert.Equal(new GeometryLine(new Vector2(-50f, 55f), new Vector2(50f, -35f)), line);
		}
	}
}
