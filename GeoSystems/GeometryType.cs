namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// Determines the type of geometry.
	/// </summary>
	public enum GeometryType
	{
		/// <summary>
		/// None or unknown.
		/// </summary>
		None = 0,

		/// <summary>
		/// Vertex.
		/// </summary>
		Vertex,

		/// <summary>
		/// Line.
		/// </summary>
		Line,

		/// <summary>
		/// Ring.
		/// </summary>
		Ring,

		/// <summary>
		/// Polygon.
		/// </summary>
		Polygon,

		/// <summary>
		/// Multi-polygon.
		/// </summary>
		MultiPolygon,
	}
}
