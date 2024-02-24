#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elephant.UnityLibrary.GeoSystems.Renderers
{
	/// <summary>
	/// Triggered after one or more lines are clicked.
	/// DynamicMeshLines is this and the first list are the clicked vertices, the second list are
	/// the clicked lines. 
	/// </summary>
	[Serializable]
	public class OnGeometryClicked : UnityEvent<DynamicMeshLinesRenderer, List<GameObject>, List<GameObject>>
	{
	}

	/// <summary>
	/// Triggered after one or more lines are clicked.
	/// DynamicMeshLines is this and <![CDATA[List<GameObject>]]> are the GameObjects of the clicked lines.
	/// </summary>
	[Serializable]
	public class OnLineClicked : UnityEvent<DynamicMeshLinesRenderer, List<GameObject>>
	{
	}

	/// <summary>
	/// Triggered after on or more vertices are clicked.
	/// DynamicMeshLines is this and <![CDATA[List<GameObject>]]> are the GameObjects of the clicked vertices.
	/// </summary>
	[Serializable]
	public class OnVertexClicked : UnityEvent<DynamicMeshLinesRenderer, List<GameObject>>
	{
	}

	/// <summary>
	/// Renders dynamic mesh lines.
	/// </summary>
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class DynamicMeshLinesRenderer : MonoBehaviour
	{
		#region Unity Events

		/// <inheritdoc cref="Renderers.OnGeometryClicked"/>
		public OnGeometryClicked? OnGeometryClicked = null;

		/// <inheritdoc cref="Renderers.OnLineClicked"/>
		public OnLineClicked? OnLineClicked = null;

		/// <inheritdoc cref="Renderers.OnVertexClicked"/>
		public OnVertexClicked? OnVertexClicked = null;

		#endregion

		#region Properties and fields

		/// <summary>
		/// Geometry to render. Will do nothing if it's empty.
		/// <p>Coordinates are in local space.</p>
		/// </summary>
		public List<Vector3> Vertices = new();

		/// <summary>
		/// Geometry lines thickness. 
		/// </summary>
		public float lineThickness = 0.4f;

		/// <summary>
		/// Child objects that contain rendered line items.
		/// </summary>
		public List<GameObject> LineObjects = new();

		/// <summary>
		/// Child objects that contain rendered vertex items.
		/// </summary>
		public List<GameObject> VertexObjects = new();

		/// <summary>
		/// If true, <see cref="Refresh"/> is called upon <see cref="Start"/>.
		/// </summary>
		public bool RefreshOnStart = true;

		/// <summary>
		/// If true and if the start and end vertices are not the same, a line between the first
		/// and last vertices is rendered. 
		/// </summary>
		public bool CloseGeometries = true;

		/// <summary>
		/// Default shader name for when no material is assigned.
		/// </summary>
		public string DefaultShaderName = "Standard";

		/// <summary>
		/// If true, local space will be ignored and everything will be handled using world space only.
		/// </summary>
		/// <remarks>
		/// You may have to call <see cref="Refresh"/> when changing the position while this value is true
		/// and then setting it to false.
		/// </remarks>
		public bool UseWorldSpaceOnly = false;

		/// <summary>
		/// If true, geometry line colliders will be created.
		/// </summary>
		public bool CreateLineColliders = true;

		/// <summary>
		/// If true, geometry vertex colliders will be created.
		/// </summary>
		public bool CreateVertexColliders = true;

		/// <summary>
		/// Material to use for rendering lines. If null, a new default material will be created.
		/// </summary>
		[Header("Lines")] public Material? RenderLineMaterial = null;

		/// <summary>
		/// Color used for rendering lines.
		/// </summary>
		public Color RenderLineMaterialColor = Color.black;

		/// <summary>
		/// GameObject name of each line.
		/// </summary>
		public string LineObjectNamePrefix = "Line";

		/// <summary>
		/// Material to use for rendering vertices. If null, a new default material will be created.
		/// </summary>
		[Header("Vertices")] public Material? RenderVertexMaterial = null;

		/// <summary>
		/// Color used for rendering vertices.
		/// </summary>
		public Color RenderVertexMaterialColor = Color.black;

		/// <summary>
		/// GameObject name of all vertices.
		/// </summary>
		public string VertexObjectNamePrefix = "Vertex";

		/// <summary>
		/// If true, input will be processed.
		/// </summary>
		[Header("Input")] public bool ProcessInput = false;

		/// <summary>
		/// Camera used for processing mouse clicks. If null, the main camera will be used.
		/// </summary>
		public Camera? Camera = null;

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

		#endregion

		#region Unity Message Functions

		/// <summary>
		/// Awake.
		/// </summary>
		private void Awake()
		{
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
		/// Update.
		/// </summary>
		private void Update()
		{
			if (ProcessInput)
				ProcessMouseInput(Camera);
		}

		/// <summary>
		/// Process mouse input.
		/// </summary>
		protected virtual void ProcessMouseInput(Camera? inputCamera)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 rayOrigin;
				if (inputCamera == null)
					rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				else
					rayOrigin = inputCamera.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.zero);

				// Get clicked geometries.
				List<GameObject> clickedLines = GetClickedLines(hits);
				List<GameObject> clickedVertices = GetClickedVertices(hits);

				// Fire events if applicable.
				if (clickedLines.Any() || clickedVertices.Any())
				{
					OnGeometryClicked?.Invoke(this, clickedVertices, clickedLines);

					if (clickedLines.Any())
						OnLineClicked?.Invoke(this, clickedLines);

					if (clickedVertices.Any())
						OnVertexClicked?.Invoke(this, clickedVertices);
				}
			}
		}

		/// <summary>
		/// Draw Gizmos when selected.
		/// </summary>
		protected virtual void OnDrawGizmosSelected()
		{
			if (!DrawGizmos)
				return;

			Gizmos.color = GizmoColor;
			List<Vector3> vertices = VerticesInWorldSpace().ToList();

			int vertexCount = vertices.Count;
			for (int i = 0; i < vertexCount; i++)
			{
				Vector3 currentVertex = vertices[i];

				// Draw line between each pair of vertices if applicable.
				if (vertexCount > 1)
				{
					Vector3 nextVertex = vertices[(i + 1) % vertexCount]; // Loop back to the first vertex to close the shape.

					if (i == vertexCount - 1)
					{
						// Draw last line if applicable.
						if (IsClosingRequired())
							Gizmos.DrawLine(vertices[0], vertices[vertexCount - 1]);
					}
					else
					{
						// Draw normal line.
						Gizmos.DrawLine(currentVertex, nextVertex);
					}
				}

				// Draw vertex if applicable.
				if (GizmoSphereSize > 0f)
					Gizmos.DrawWireSphere(currentVertex, GizmoSphereSize);
			}
		}

		#endregion

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
		/// Rerender/Redraw this geometry.
		/// </summary>
		public virtual void Refresh()
		{
			DestroyAndClearGeometryObjects();

			CreateLines(VerticesInWorldSpace().ToList());

			foreach (Vector3 vertex in VerticesInWorldSpace())
				CreateVertices(vertex);
		}

		/// <summary>
		/// Destroy and clear all geometry objects used for rendering.
		/// </summary>
		protected virtual void DestroyAndClearGeometryObjects()
		{
			foreach (GameObject lineObject in LineObjects)
				Destroy(lineObject);
			LineObjects.Clear();

			foreach (GameObject vertexObject in VertexObjects)
				Destroy(vertexObject);
			VertexObjects.Clear();
		}

		/// <summary>
		/// Create line geometry. Will do nothing if there are less than 1 vertices.
		/// </summary>
		protected virtual void CreateLines(List<Vector3> vertices)
		{
			int verticesCount = vertices.Count;

			if (verticesCount < 2)
				return; // Ensure there are at least two vertices

			// Create a line between each pair of vertices.
			for (int i = 0; i < verticesCount - 1; i++)
				CreateLineMesh(vertices[i], vertices[i + 1]);

			// Draw closing line if applicable.
			if (verticesCount > 2)
			{
				Vector3 firstVertex = vertices[0];
				Vector3 lastVertex = vertices[verticesCount - 1];

				if (IsClosingRequired())
					CreateLineMesh(firstVertex, lastVertex);
			}
		}

		/// <summary>
		/// Returns true if an extra line must be drawn between the last and the first vertices.
		/// </summary>
		protected virtual bool IsClosingRequired()
		{
			Vector3 firstVertex = Vertices[0];
			Vector3 lastVertex = Vertices[Vertices.Count - 1];

			return firstVertex != lastVertex && CloseGeometries;
		}

		/// <summary>
		/// Returns the GameObjects of all clicked lines that belong to this renderer.
		/// </summary>
		private List<GameObject> GetClickedLines(RaycastHit2D[] hits)
		{
			List<GameObject> clickedLines = new();
			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider != null)
				{
					MeshRenderer? meshRenderer;
					DynamicMeshLinesRenderer? dynamicMeshLines;
					// Note: This is not an efficient way to handle these null values but it will do for now.
					try
					{
						meshRenderer = hit.collider.transform.parent.GetComponent<MeshRenderer>();
						dynamicMeshLines = hit.collider.transform.parent.parent.GetComponent<DynamicMeshLinesRenderer>();
					}
					catch (NullReferenceException)
					{
						continue;
					}

					if (dynamicMeshLines != null && dynamicMeshLines == this && meshRenderer != null)
						clickedLines.Add(meshRenderer.gameObject);
				}
			}

			return clickedLines;
		}

		/// <summary>
		/// Returns the GameObjects of all clicked vertices that belong to this renderer.
		/// </summary>
		private List<GameObject> GetClickedVertices(RaycastHit2D[] hits)
		{
			List<GameObject> clickedVertices = new();
			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider != null)
				{
					MeshRenderer? meshRenderer;
					DynamicMeshLinesRenderer? dynamicMeshLines;
					// Note: This is not an efficient way to handle these null values but it will do for now.
					try
					{
						meshRenderer = hit.collider.transform.GetComponent<MeshRenderer>();
						dynamicMeshLines = hit.collider.transform.parent.GetComponent<DynamicMeshLinesRenderer>();
					}
					catch (NullReferenceException)
					{
						continue;
					}

					if (dynamicMeshLines != null && dynamicMeshLines == this && meshRenderer != null)
						clickedVertices.Add(meshRenderer.gameObject);
				}
			}

			return clickedVertices;
		}

		/// <summary>
		/// On validate. For debugging purposes only.
		/// </summary>
		private void OnValidate()
		{
			if (IsAwakeCalled && Application.isPlaying)
				Refresh();
		}

		/// <summary>
		/// Returns a new default material.
		/// </summary>
		protected virtual Material CreateDefaultMaterial()
		{
			return new Material(Shader.Find(DefaultShaderName));
		}

		/// <summary>
		/// Return a name for a line <see cref="GameObject"/>.
		/// </summary>
		protected virtual string LineObjectName(Vector3 start, Vector3 end) => $"{LineObjectNamePrefix} {start:F2} => {end:F2}";

		/// <summary>
		/// Create and configure a new line mesh.
		/// </summary>
		/// <param name="start">Start position of the line.</param>
		/// <param name="end">End position of the line.</param>
		/// <remarks>
		/// This method generates a GameObject representing a line between two specified points.
		/// The line's appearance is customized by the material and color properties set on the MeshRenderer.
		/// If a collider is needed for the line, it can be added by enabling the <see cref="CreateLineColliders"/> flag.
		/// </remarks>
		protected virtual void CreateLineMesh(Vector3 start, Vector3 end)
		{
			GameObject lineObject = new(LineObjectName(start, end));
			LineObjects.Add(lineObject);
			lineObject.transform.parent = transform;

			// Add MeshRenderer and setup its material.
			MeshRenderer meshRenderer = lineObject.AddComponent<MeshRenderer>();
			if (RenderLineMaterial == null)
				meshRenderer.material = CreateDefaultMaterial();
			else
				meshRenderer.material = RenderLineMaterial;
			meshRenderer.material.color = RenderLineMaterialColor;

			MeshFilter meshFilter = lineObject.AddComponent<MeshFilter>();

			Mesh mesh = new();
			Vector3[] vertices = new Vector3[4];
			int[] triangles = new int[6];

			Vector3 direction = (end - start).normalized;
			// Calculate a perpendicular vector to the direction. This will help to set the line thickness.
			Vector3 cross = Vector3.Cross(direction, Vector3.forward) * (lineThickness / 2f);

			// Define vertices positions to form a rectangle (=thick line).
			vertices[0] = start + cross;
			vertices[1] = start - cross;
			vertices[2] = end + cross;
			vertices[3] = end - cross;

			// Define how vertices are connected to form two triangles making up the rectangle.
			// Note: Order is important to ensure normals face the correct direction.
			triangles[0] = 0;
			triangles[1] = 1;
			triangles[2] = 2;
			triangles[3] = 2;
			triangles[4] = 1;
			triangles[5] = 3;

			// Assign vertices and triangles to the mesh.
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			// Recalculate normals for the lighting calculations.
			mesh.RecalculateNormals();

			meshFilter.mesh = mesh;

			// If applicable, add a collider to the line.
			if (CreateLineColliders)
				AddLineCollider(lineObject.transform, start, end);
		}

		/// <summary>
		/// Add line collider.
		/// </summary>
		protected virtual void AddLineCollider(Transform parent, Vector3 start, Vector3 end)
		{
			GameObject colliderObject = new("LineCollider");
			colliderObject.transform.SetParent(parent, false); // Set parent without worldPositionStays.

			// Add a BoxCollider2D component to the line object.
			BoxCollider2D lineCollider = colliderObject.AddComponent<BoxCollider2D>();

			// Calculate line length and collider size.
			float lineLength = Vector3.Distance(start, end);
			lineCollider.size = new Vector2(lineLength + lineThickness, lineThickness);

			// Calculate midpoint in parent's local space.
			Vector3 midpoint = (start + end) * 0.5f;
			// Position colliderObject at midpoint relative to parent.
			colliderObject.transform.localPosition = midpoint;

			// Set the collider its offset to zero since the colliderObject is already positioned at the midpoint.
			lineCollider.offset = Vector2.zero;

			// Calculate the angle between the line's start and end points in degrees.
			float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
			// Apply rotation to the colliderObject.
			colliderObject.transform.localEulerAngles = new Vector3(0, 0, angle);
		}

		#region Vertices

		/// <summary>
		/// Create and configure vertex mesh.
		/// </summary>
		protected virtual Mesh CreateVertexMesh(float radius)
		{
			Mesh mesh = new();
			List<Vector3> vertices = new();
			List<int> triangles = new();

			// Add center point
			vertices.Add(Vector3.zero);
			int resolution = 20; // Increase for smoother circles
			for (int i = 0; i <= resolution; i++)
			{
				float angle = i * 2 * Mathf.PI / resolution;
				vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0));
			}

			for (int i = 1; i < resolution; i++)
			{
				// Reversed the winding order here
				triangles.Add(0);
				triangles.Add(i + 1);
				triangles.Add(i);
			}

			// Close the circle by connecting the last and first vertices on the perimeter
			triangles.Add(0);
			triangles.Add(1);
			triangles.Add(resolution);

			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles, 0);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds(); // Good practice to recalculate bounds
			return mesh;
		}

		/// <summary>
		/// Return a name for a vertex <see cref="GameObject"/>.
		/// </summary>
		protected virtual string VertexObjectName(Vector3 position) => $"{VertexObjectNamePrefix} {position:F2}";

		protected virtual void CreateVertices(Vector3 position)
		{
			GameObject vertexObject = new(VertexObjectName(position));
			VertexObjects.Add(vertexObject);
			vertexObject.transform.position = position;
			vertexObject.transform.parent = transform;

			// Add MeshRenderer and setup its material.
			MeshRenderer meshRenderer = vertexObject.AddComponent<MeshRenderer>();
			if (RenderVertexMaterial == null)
				meshRenderer.material = CreateDefaultMaterial();
			else
				meshRenderer.material = RenderVertexMaterial;
			meshRenderer.material.color = RenderVertexMaterialColor;


			MeshFilter meshFilter = vertexObject.AddComponent<MeshFilter>();
			meshFilter.mesh = CreateVertexMesh(lineThickness / 2f); // Circle radius must be half the line thickness.

			if (CreateVertexColliders)
				CreateVertexCollider(vertexObject);
		}

		protected virtual void CreateVertexCollider(GameObject parent)
		{
			CircleCollider2D vertexCollider = parent.AddComponent<CircleCollider2D>();
			vertexCollider.radius = lineThickness / 2f;
		}

		#endregion
	}
}