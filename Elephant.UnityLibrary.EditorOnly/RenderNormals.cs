#nullable enable

using UnityEditor;
using UnityEngine;

namespace Elephant.UnityLibrary.EditorOnly
{
	/// <summary>
	/// Draws normals in the Unity editor for debugging purposes, such as analyzing shadow-related issues.
	/// </summary>
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(Renderer))]
	public class RenderNormals : MonoBehaviour
	{
		/// <summary>
		/// The maximum number of normals to draw. This is a performance optimization to prevent the editor
		/// from becoming sluggish when working with complex meshes that have a large number of normals.
		/// Adding a value of say 1000 means that 1000 face- and 1000 vertex normals may be rendered simultaneously.
		/// </summary>
		[Min(1)] [SerializeField] private int _maxNormalsToRender = 5000;

		/// <summary>
		/// Configuration for drawing face normals. This includes settings such as the color of the normals
		/// and whether they should be drawn. Face normals are perpendicular to the mesh's surfaces and are useful
		/// for visualizing the orientation of each face or triangle in the mesh.
		/// </summary>
		public RenderNormalsData Face = new(new Color32(255, 0, 0, 255), true);

		/// <summary>
		/// Configuration for drawing vertex normals. Similar to face normals, this includes settings such as the color
		/// and whether they should be drawn. Vertex normals are important for lighting calculations and can help in
		/// visualizing how light would interact with the mesh.
		/// </summary>
		public RenderNormalsData Vertex = new(new Color32(255, 0, 255, 255), false);

		#region Unity events

		/// <summary>
		/// Called when the object is selected in the editor. Updates wire frame visibility and renders
		/// normals if applicable.
		/// </summary>
		private void OnDrawGizmosSelected()
        {
            RenderAll(true);
        }

        /// <summary>
        /// Called when the object is NOT selected in the editor. Renders normals if applicable.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!Selection.Contains(gameObject))
                RenderAll(false);
        }

		#endregion

		/// <summary>
		/// Renders the normals for the mesh if the corresponding settings are enabled.
		/// This method handles both face and vertex normals. The rendering of normals
		/// is contingent on whether the object is selected and if the number of normals
		/// to draw does not exceed the specified maximum.
		/// </summary>
		/// <param name="isSelected">Whether the object is selected.</param>
		protected virtual void RenderAll(bool isSelected)
		{
			MeshFilter? meshFilter = GetComponent<MeshFilter>();

			if (meshFilter == null)
			{
				Debug.LogError($"A valid {nameof(MeshFilter)} is required and must be attached to the same {nameof(GameObject)} as the {nameof(RenderNormals)} is attached to.");
				return;
			}

			Mesh mesh = meshFilter.sharedMesh;
			int[] triangles = mesh.triangles;
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;

			// Render face normals.
			if (Face.IsRenderingAllowed(isSelected))
				RenderFaceNormals(triangles, vertices);

			// Render vertex normals.
			if (Vertex.IsRenderingAllowed(isSelected))
				RenderVertexNormals(vertices, normals);
		}

		/// <summary>
		/// Draws the face normals of the mesh if the number of normals to draw does not exceed the specified maximum.
		/// </summary>
		/// <param name="triangles">Triangle indices of the mesh.</param>
		/// <param name="vertices">Vertex positions of the mesh.</param>
		protected virtual void RenderFaceNormals(int[] triangles, Vector3[] vertices)
		{
			// Counter for drawn normals.
			int drawnNormals = 0;

			// Iterate through the triangle indices in steps of 3, as each triangle is defined by three vertices.
			for (int i = 0; i < triangles.Length; i += 3)
			{
				// Check if the number of normals drawn has reached the maximum limit (_maxNormalsToDraw).
				// If so, exit the loop to avoid drawing any more normals.
				if (drawnNormals >= _maxNormalsToRender)
					break;

				// Calculate the world position of each vertex of the current triangle.
				Vector3 v0 = transform.TransformPoint(vertices[triangles[i]]);
				Vector3 v1 = transform.TransformPoint(vertices[triangles[i + 1]]);
				Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 2]]);

				// Compute the center point of the triangle by averaging the positions of its vertices.
				Vector3 center = (v0 + v1 + v2) / 3;

				// Calculate the normal of the triangle. This is done by computing the cross product of two sides
				// of the triangle and normalizing the result to ensure it has a length of 1.
				Vector3 direction = Vector3.Cross(v1 - v0, v2 - v0).normalized;

				// Draw the normal at the center of the triangle. The Render method visualizes the normal as a line
				// (or possibly an arrow) starting from the center of the triangle and extending in the direction
				// of the normal.
				Render(Face, center, direction);

				// Increment the count of drawn normals.
				drawnNormals++;
			}
		}

		/// <summary>
		/// Draws the vertex normals of the mesh if the number of normals to draw does not exceed the specified maximum.
		/// </summary>
		/// <param name="vertices">Vertex positions of the mesh.</param>
		/// <param name="normals">Normal vectors of the mesh's vertices.</param>
		protected virtual void RenderVertexNormals(Vector3[] vertices, Vector3[] normals)
		{
			// Counter for drawn normals.
			int drawnNormals = 0;

			// Iterate through the triangle indices in steps of 3, as each triangle is defined by three vertices.
			for (int i = 0; i < vertices.Length; i++)
			{
				// Check if the number of normals drawn has reached the maximum limit (_maxNormalsToDraw).
				// If so, exit the loop to avoid drawing any more normals.
				if (drawnNormals >= _maxNormalsToRender)
					break;

				// Render the vertex normals of the mesh.
				Render(Vertex, transform.TransformPoint(vertices[i]), transform.TransformVector(normals[i]));

				// Increment the count of drawn normals.
				drawnNormals++;
			}
		}

		/// <summary>
		/// Render a normal as an arrow from a given point in a specified direction.
		/// </summary>
		/// <param name="renderNormalsData"><see cref="RenderNormalsData"/>.</param>
		/// <param name="origin">The starting point of the normal.</param>
		/// <param name="direction">The direction of the normal.</param>
		protected virtual void Render(RenderNormalsData renderNormalsData, Vector3 origin, Vector3 direction)
		{
			// Only render if it's pointing in the direction of the camera.
			if (!IsPointingTowardsCameraView(direction))
				return;

			// Render wire sphere at the base if applicable.
			if (renderNormalsData.RenderNormalBaseWireSphereSize)
			{
				Gizmos.color = renderNormalsData.NormalBaseWireSphereColor;
				Gizmos.DrawWireSphere(origin, renderNormalsData.NormalBaseWireSphereSize);
			}

			// Render lines with arrowheads.
			RenderNormalLine(renderNormalsData, origin, direction);
			RenderNormalArrowhead(renderNormalsData, origin, direction);
		}

		/// <summary>
		/// Render the line.
		/// </summary>
		private void RenderNormalLine(RenderNormalsData renderNormalsData, Vector3 origin, Vector3 direction)
		{
			Gizmos.color = renderNormalsData.NormalColor;
			Vector3 to = origin + (direction * renderNormalsData.NormalLength);
			Gizmos.DrawLine(origin, to);
		}

		/// <summary>
		/// Render just the arrow head of a line.
		/// </summary>
		private void RenderNormalArrowhead(RenderNormalsData renderNormalsData, Vector3 origin, Vector3 direction)
		{
			Vector3 to = origin + (direction * renderNormalsData.NormalLength);
			Gizmos.DrawRay(to, Quaternion.Euler(0, 0, 45) * -direction * renderNormalsData.NormalLength * 0.25f);
			Gizmos.DrawRay(to, Quaternion.Euler(0, 0, -45) * -direction * renderNormalsData.NormalLength * 0.25f);
		}

		/// <summary>
		/// Determine if the vector <paramref name="direction"/> is pointing towards the camera's view.
		/// </summary>
		private bool IsPointingTowardsCameraView(Vector3 direction)
		{
			return Camera.current.transform.InverseTransformDirection(direction).z < 0f;
		}
	}
}