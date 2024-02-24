using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems.Renderers
{
	/// <summary>
	/// Renders a <see cref="GeoSystems.MultiPolygon"/>.
	/// </summary>
	public class MultiPolygonRenderer : MonoBehaviour
	{
		/// <summary>
		/// Multipolygon to render.
		/// </summary>
		public MultiPolygon MultiPolygon = new();

		/// <summary>
		/// Parent <see cref="GameObject"/> that contains all renderers.
		/// </summary>
		protected GameObject RenderersParent = null!;

		/// <summary>
		/// Polygon exterior ring colors.
		/// </summary>
		public Color ExteriorRingColor = Color.black;

		/// <summary>
		/// Polygon interior ring colors.
		/// </summary>
		public Color InteriorRingColor = Color.black;

		/// <summary>
		/// Start.
		/// </summary>
		private void Start()
		{
			RenderersParent = new GameObject("Renderers");
			RenderersParent.transform.SetParent(transform);

			Render();
		}

		/// <summary>
		/// Render the <see cref="MultiPolygon"/>.
		/// </summary>
		public virtual void Render()
		{
			foreach (Transform child in RenderersParent.transform)
				Destroy(child.gameObject);

			foreach (Polygon polygon in MultiPolygon.Polygons)
			{
				SpawnAndConfigureRenderer(polygon.ExteriorRing.AllVertices().Select(vertex => new Vector3(vertex.Position.x, vertex.Position.y, 0f)).ToList(), ExteriorRingColor);

				foreach (Ring interiorRing in polygon.InteriorRings)
					SpawnAndConfigureRenderer(interiorRing.AllVertices().Select(vertex => new Vector3(vertex.Position.x, vertex.Position.y, 0f)).ToList(), InteriorRingColor);
			}
		}

		/// <summary>
		/// Spawn and configure a <see cref="DynamicMeshLinesRenderer"/>.
		/// </summary>
		protected virtual void SpawnAndConfigureRenderer(List<Vector3> vertices, Color? color = null)
		{
			GameObject newRenderer = new("Renderer");
			newRenderer.transform.SetParent(RenderersParent.transform);

			DynamicMeshLinesRenderer dynamicMeshLinesRenderer = newRenderer.AddComponent<DynamicMeshLinesRenderer>();
			dynamicMeshLinesRenderer.Vertices = new List<Vector3>(vertices);
			if (color != null)
				dynamicMeshLinesRenderer.RenderLineMaterialColor = dynamicMeshLinesRenderer.RenderVertexMaterialColor = (Color)color;
			dynamicMeshLinesRenderer.Refresh();
		}

		/// <summary>
		/// Destroy all renderers.
		/// </summary>
		public void Clear()
		{
			foreach (GameObject geometryRenderer in RenderersParent.transform)
				Destroy(geometryRenderer);
		}
	}
}
