using System;
using UnityEngine;

namespace Elephant.UnityLibrary.ThirdParty.OpenStreetMap
{
	/// <summary>
	/// Convert between GPS and OSM (=Open Street Map) tile coordinates. For RDS conversions see MathRd.cs.
	/// Latitude comes before longitude.
	/// Version 1.00.
	/// </summary>
	/// <remarks>Floats are usually to imprecise while decimals are overkill so doubles are used where possible.</remarks>
	public static class CoordinateConverterUtilities
	{
		/// <summary>
		/// The Earth's radius in meters (mean value, as the Earth is an oblate spheroid).
		/// </summary>
		public const double EarthRadius = 6378137d;

		/// <summary>
		/// Half the circumference (=omtrek in Dutch) of the Earth.
		/// </summary>
		public const double HalfEarthCircumference = Math.PI * EarthRadius;

		/// <summary>
		/// Convert a GPS coordinate into an Open Street Maps tile coordinate.
		/// </summary>
		public static (int TileX, int TileY) ToTileCoordinate(int zoom, double latitude, double longitude)
		{
			// Convert the longitude to a tile X coordinate.
			// The formula takes the longitude, adjusts it into a 0-360 degree range (by adding 180),
			// then scales it by the number of tiles at the given zoom level (which is 2^zoom).
			int tileX = (int)Math.Floor((longitude + 180) / 360 * Math.Pow(2, zoom));

			// Convert the latitude to a tile Y coordinate.
			// This formula involves some trigonometry to transform from a latitude in degrees to a
			// Mercator-projected Y coordinate, then scales it by the number of tiles at the given zoom level.
			// See also: https://en.wikipedia.org/wiki/Mercator_projection.
			int tileY = (int)Math.Floor((1 - Math.Log(Math.Tan(latitude * Math.PI / 180) + 1 / Math.Cos(latitude * Math.PI / 180)) / Math.PI) / 2 * Math.Pow(2, zoom));

			return (tileX, tileY);
		}

		/// <summary>
		/// Convert a GPS coordinate into an Open Street Maps tile coordinate.
		/// </summary>
		public static (int TileX, int TileY) ToTileCoordinate(int zoom, Vector2 gpsCoordinate)
		{
			return ToTileCoordinate(zoom, gpsCoordinate.x, gpsCoordinate.y);
		}

		/// <summary>
		/// Convert Open Street Map tile coordinate ==> GPS coordinate.
		/// Latitude is the first parameter.
		/// </summary>
		public static (double Latitude, double Longitude) TileToGps(int zoom, int tileX, int tileY)
		{
			// Calculate the width of a single tile at the given zoom level in meters.
			// The total width (2*HalfEarthCircumference) is divided by the number of tiles, which is 2^zoom.
			double tileWidth = 2d * HalfEarthCircumference / (1 << zoom);
			// The height of a tile is the same as the width because the tiles in Open Street Map are square.
			double tileHeight = 2d * HalfEarthCircumference / (1 << zoom);

			// Calculate the horizontal distance in meters from the left-most edge of the left-most tile to the center of the given tileX.
			double centerX = (tileX + 0.5d) * tileWidth - HalfEarthCircumference;
			// Calculate the vertical distance in meters from the top-most edge of the top-most tile to the center of the given tileY.
			double centerY = HalfEarthCircumference - (tileY + 0.5d) * tileHeight;

			// Convert the centerX value to degrees to get the longitude.
			// The formula derives from the relationship between arc length and angle in a circle (arc length = angle * radius).
			double longitude = centerX / EarthRadius * 180d / Math.PI;
			// Convert the centerY value to degrees to get the latitude.
			// The formula involves some trigonometry and is used to transform Mercator-projected Y coordinate back to latitude.
			double latitude = (2d * Math.Atan(Math.Exp(centerY / EarthRadius)) - Math.PI / 2d) * 180d / Math.PI;

			return (latitude, longitude);
		}

		/// <summary>
		/// Convert Open Street Map tile coordinate ==> GPS coordinate.
		/// x = latitude, y = longitude.
		/// </summary>
		public static Vector2 TileToGpsAsVector2(int zoom, int tileX, int tileY)
		{
			var gpsCoordinate = TileToGps(zoom, tileX, tileY);
			return new Vector2((float)gpsCoordinate.Latitude, (float)gpsCoordinate.Longitude);
		}
	}
}
