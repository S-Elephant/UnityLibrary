using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems.Renderers
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class GeometryFillRenderer : MonoBehaviour
	{
		/// <summary>
		/// Vertices that make up the fill area.
		/// </summary>
		public List<Vector3> Vertices = new();

		/// <summary>
		/// Mesh that is the fill area.
		/// </summary>
		protected Mesh FillMesh = null!;

		/// <summary>
		/// If true, <see cref="Refresh"/> is called upon <see cref="Start"/>.
		/// </summary>
		public bool RefreshOnStart = true;

		/// <summary>
		/// Default shader name for when no material is assigned.
		/// </summary>
		public string DefaultShaderName = "Standard";

		/// <summary>
		/// Color used for rendering filled area.
		/// </summary>
		public Color FillColor = Color.gray;

		/// <summary>
		/// If true, local space will be ignored and everything will be handled using world space only.
		/// </summary>
		/// <remarks>
		/// You may have to call <see cref="Refresh"/> when changing the position while this value is true
		/// and then setting it to false.
		/// </remarks>
		public bool UseWorldSpaceOnly = false;

		/// <summary>
		/// Determines if Gizmos should be drawn. Does nothing outside of the Unity Editor.
		/// </summary>
		[Header("Gizmos")] public bool DrawGizmos = true;

		/// <summary>
		/// Size of the Gizmo spheres for debugging purposes.
		/// Set to 0f to disable.
		/// </summary>
		public float GizmoSphereSize = 0.5f;

		/// <summary>
		/// Gizmo color.
		/// </summary>
		public Color GizmoColor = Color.magenta;

		/// <summary>
		/// Returns if awake was called.
		/// </summary>
		private bool IsAwakeCalled = false;

		/// <summary>
		/// Awake.
		/// </summary>
		private void Awake()
		{
			FillMesh = new();
			IsAwakeCalled = true;
		}

		/// <summary>
		/// Start.
		/// </summary>
		private void Start()
		{
			if (RefreshOnStart)
				Refresh();
		}

		/// <summary>
		/// On draw Gizmos.
		/// </summary>
		private void OnDrawGizmosSelected()
		{
			if (!DrawGizmos || Vertices == null || Vertices.Count < 3)
				return;

			// Optional: Set a custom color for the wireframe
			Gizmos.color = GizmoColor;

			List<Vector3> vertices = VerticesInWorldSpace().ToList();
			int vertexCount = Vertices.Count;
			for (int i = 0; i < vertexCount; i++)
			{
				Vector3 currentVertex = vertices[i];

				// Draw line between each pair of vertices if applicable.
				if (vertexCount > 1)
				{
					Vector3 nextVertex = vertices[(i + 1) % vertexCount]; // Loop back to the first vertex to close the shape.
					Gizmos.DrawLine(currentVertex, nextVertex);
				}

				// Draw vertex if applicable.
				if (GizmoSphereSize > 0f)
					Gizmos.DrawWireSphere(currentVertex, GizmoSphereSize);
			}
		}

		/// <summary>
		/// Returns a new list of vertices adjusted by the gameObject's transform position.
		/// </summary>
		/// <returns>The vertices in world space.</returns>
		public IEnumerable<Vector3> VerticesInWorldSpace()
		{
			if (UseWorldSpaceOnly)
				return Vertices;

			return Vertices.Select(vertex => transform.position + vertex);
		}

		/// <summary>
		/// Returns a new default material.
		/// </summary>
		protected virtual Material CreateDefaultMaterial()
		{
			return new Material(Shader.Find(DefaultShaderName));
		}

		/// <summary>
		/// Rerender/Redraw the filled area.
		/// </summary>
		public virtual void Refresh()
		{
			FillMesh = new Mesh();
			GetComponent<MeshFilter>().mesh = FillMesh;
			DrawMesh(VerticesInWorldSpace());
			var meshRenderer = GetComponent<MeshRenderer>();

			if (meshRenderer.material != null)
				Destroy(meshRenderer.material);
			meshRenderer.material = CreateDefaultMaterial();
			meshRenderer.material.color = FillColor;
		}

		/// <summary>
		/// Create and configure the mesh.
		/// </summary>
		protected virtual void DrawMesh(IEnumerable<Vector3> vertices)
		{
			FillMesh.Clear();

			// Assign vertices
			FillMesh.vertices = vertices.ToArray();

			// Create triangles. These are the indices into the vertices array.
			// For a triangle, you need to specify three indices.
			int[] triangles =
			{
				0, 1, 2 // This will draw one triangle
            };
			FillMesh.triangles = triangles;

			// Optionally, update the normals of the mesh.
			FillMesh.RecalculateNormals();
		}

		/// <summary>
		/// On validate. For debugging purposes only.
		/// </summary>
		private void OnValidate()
		{
			if (IsAwakeCalled && FillMesh && Application.isPlaying)
				DrawMesh(VerticesInWorldSpace());
		}
	}
}