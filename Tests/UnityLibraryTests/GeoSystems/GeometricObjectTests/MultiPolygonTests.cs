using Elephant.UnityLibrary.GeoSystems;
using UnityEngine;

namespace UnityLibraryTests.GeoSystems.GeometricObjectTests
{
	/// <summary>
	/// <see cref="MultiPolygon"/> tests.
	/// </summary>
	public class MultiPolygonTests
	{
		/// <summary>
		/// Multi-polygon that consists of 2 simple valid non-overlapping polygons without any holes/interior-rings.
		/// Their images are located here: https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry.
		/// </summary>
		private const string PolygonWktString1 = "MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))";

		/// <summary>
		/// Multi-polygon that consists of 2 simple valid non-overlapping polygons.
		/// The first polygon is a simple polygon without any holes/interior-rings but the second polygon has 1 hole/interior-ring.
		/// Their images are located here: https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry
		/// </summary>
		private const string PolygonWktString2 = "MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)), ((20 35, 10 30, 10 10, 30 5, 45 20, 20 35), (30 20, 20 15, 20 25, 30 20)))";

		/// <summary>
		/// <see cref="MultiPolygon.Recalculate"/> should mark it as non-dirty.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void RecalculateMarksAsNonDirty()
		{
			// Arrange.
			MultiPolygon multiPolygon = MultiPolygon.FromWktString(PolygonWktString1);

			// Act.
			multiPolygon.Recalculate();

			// Assert.
			Assert.True(multiPolygon.IsClean);
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.IsClean));
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.ExteriorRing.IsClean && polygon.InteriorRings.All(interiorRing => interiorRing.IsClean)));
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.ExteriorRing.Lines.All(line => line.IsClean) && polygon.InteriorRings.All(interiorRing => interiorRing.Lines.All(line => line.IsClean))));
		}

		/// <summary>
		/// Assigning a new polygon should mark it as dirty.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void AddNewPolygonMarksAsDirty()
		{
			// Arrange.
			MultiPolygon multiPolygon = MultiPolygon.FromWktString(PolygonWktString1);

			// Act.
			multiPolygon.Polygons.Add(new Polygon());

			// Assert.
			Assert.True(multiPolygon.IsDirty);
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.IsDirty));
		}

		/// <summary>
		/// Assigning a new ring should mark it as dirty.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void AddNewRingMarksAsDirty()
		{
			// Arrange.
			MultiPolygon multiPolygon = MultiPolygon.FromWktString(PolygonWktString1);

			// Act.
			multiPolygon.Polygons[1].InteriorRings.Add(new Ring());

			// Assert.
			Assert.True(multiPolygon.IsDirty);
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.IsDirty));
			Assert.True(multiPolygon.Polygons[1].InteriorRings.Last().IsDirty);
		}

		/// <summary>
		/// Update exterior ring vertex should mark it as dirty.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void UpdateExteriorRingVertexMarksAsDirty()
		{
			// Arrange.
			MultiPolygon multiPolygon = MultiPolygon.FromWktString(PolygonWktString1);
			GeometryVertex vertex = multiPolygon.Polygons[0].ExteriorRing.Lines[0]._start;

			// Act.
			vertex.Position = new Vector2(29, 21);

			// Assert.
			Assert.True(multiPolygon.IsDirty);
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.IsDirty));
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.ExteriorRing.IsDirty && polygon.InteriorRings.All(interiorRing => interiorRing.IsDirty)));
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.ExteriorRing.Lines.All(line => line.IsDirty) && polygon.InteriorRings.All(interiorRing => interiorRing.Lines.All(line => line.IsDirty))));
			Assert.True(vertex.IsDirty);
		}

		/// <summary>
		/// Update interior ring vertex should mark it as dirty.
		/// </summary>
		[Fact]
		[SpeedVeryFast, UnitTest]
		public void UpdateInteriorRingVertexMarksAsDirty()
		{
			// Arrange.
			MultiPolygon multiPolygon = MultiPolygon.FromWktString(PolygonWktString2);
			GeometryVertex vertex = multiPolygon.Polygons[1].InteriorRings[0].Lines[0]._start;

			// Act.
			vertex.Position = new Vector2(29, 21);

			// Assert.
			Assert.True(multiPolygon.IsDirty);
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.IsDirty));
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.ExteriorRing.IsDirty && polygon.InteriorRings.All(interiorRing => interiorRing.IsDirty)));
			Assert.True(multiPolygon.Polygons.All(polygon => polygon.ExteriorRing.Lines.All(line => line.IsDirty) && polygon.InteriorRings.All(interiorRing => interiorRing.Lines.All(line => line.IsDirty))));
			Assert.True(vertex.IsDirty);
		}
	}
}
