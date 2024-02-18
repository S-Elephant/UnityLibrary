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
			GeometryLine line = new (new Vector2(10, 10), new Vector2(50, 50));
		
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
	}
}
