using UnityEngine;

namespace Elephant.UnityLibrary.ThirdParty.OpenStreetMap
{
	/// <summary>
	/// Convert between GPS, Unity and OSM (=Open Street Map) tile coordinates. For RDS conversions see MathRd.cs.
	/// Latitude comes before longitude.
	/// Version 1.00.
	/// </summary>
	/// <remarks>Floats are usually to imprecise while decimals are overkill so doubles are used where possible.</remarks>
	public interface ICoordinateConverter
	{
		/// <summary>
		/// Convert a GPS coordinate into an Open Street Maps tile coordinate.
		/// </summary>
		(int TileX, int TileY) ToTileCoordinate(int zoom, double latitude, double longitude);

		/// <summary>
		/// Convert a GPS coordinate into an Open Street Maps tile coordinate.
		/// </summary>
		(int TileX, int TileY) ToTileCoordinate(int zoom, Vector2 gpsCoordinate);

		/// <summary>
		/// Convert Open Street Map tile coordinate ==> GPS coordinate.
		/// Latitude is the first parameter.
		/// </summary>
		(double Latitude, double Longitude) TileToGps(int zoom, int tileX, int tileY);

		/// <summary>
		/// Convert Open Street Map tile coordinate ==> GPS coordinate.
		/// x = latitude, y = longitude.
		/// </summary>
		Vector2 TileToGpsAsVector2(int zoom, int tileX, int tileY);
	}
}
