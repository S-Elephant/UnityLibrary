using UnityEngine;

namespace Elephant.UnityLibrary.ThirdParty.OpenStreetMap
{
	/// <inheritdoc cref="ICoordinateConverter"/>
	public class CoordinateConverter : ICoordinateConverter
	{
		/// <inheritdoc/>
		public virtual (int TileX, int TileY) ToTileCoordinate(int zoom, double latitude, double longitude)
		{
			return CoordinateConverterUtilities.ToTileCoordinate(zoom, latitude, longitude);
		}

		/// <inheritdoc/>
		public virtual (int TileX, int TileY) ToTileCoordinate(int zoom, Vector2 gpsCoordinate)
		{
			return CoordinateConverterUtilities.ToTileCoordinate(zoom, gpsCoordinate.x, gpsCoordinate.y);
		}

		/// <inheritdoc/>
		public virtual (double Latitude, double Longitude) TileToGps(int zoom, int tileX, int tileY)
		{
			return CoordinateConverterUtilities.TileToGps(zoom, tileX, tileY);
		}

		/// <inheritdoc/>
		public virtual Vector2 TileToGpsAsVector2(int zoom, int tileX, int tileY)
		{
			return CoordinateConverterUtilities.TileToGpsAsVector2(zoom, tileX, tileY);
		}
	}
}
