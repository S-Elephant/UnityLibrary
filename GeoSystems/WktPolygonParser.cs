#nullable enable

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems
{
	/// <summary>
	/// WKT (=Well Known Text) parser for Unity that supports multi-polygons and those with holes.
	/// </summary>
	public static class WktPolygonParser
	{
		private const string PolygonKey = "POLYGON";
		private const string MultiPolygonKey = "MULTIPOLYGON";

		/// <summary>
		/// Parse a WKT string representing either a POLYGON or a MULTIPOLYGON.
		/// </summary>
		/// <param name="wkt">WKT string to be parsed.</param>
		/// <returns>List representing multi-polygons, where each multi-polygon is a list of polygons and each polygon consists of an exterior ring and zero or more interior rings (holes).</returns>
		public static List<List<List<Vector2>>> ParseWKT(string wkt)
		{
			List<List<List<Vector2>>> multiPolygon = new();

			// Check if the input WKT represents a polygon.
			if (wkt.StartsWith(PolygonKey))
			{
				// Parse the Polygon and add it to the MultiPolygon list.
				List<List<Vector2>>? polygon = ParsePolygon(wkt);
				if (polygon != null)
					multiPolygon.Add(polygon);
			}
			// Check if the input WKT represents a multi-polygon.
			else if (wkt.StartsWith(MultiPolygonKey))
			{
				// Split the input into individual multi-polygons and iterate over them
				foreach (string? polyStr in SplitMultiPolygon(wkt))
				{
					// Parse each MultiPolygon and add it to the multi-polygon list.
					List<List<Vector2>>? polygon = ParsePolygon(polyStr);
					if (polygon != null)
						multiPolygon.Add(polygon);
				}
			}

			return multiPolygon;
		}

		/// <summary>
		/// Split the multi-polygon WKT string into individual polygon strings.
		/// </summary>
		/// <param name="wkt">Multi-polygon WKT string to be split.</param>
		/// <returns>List of WKT strings, each representing a single polygon.</returns>
		private static List<string> SplitMultiPolygon(string wkt)
		{
			List<string> extractedPolygonStrings = new();
			// Use a regular expression to find and extract polygon strings from the input WKT.
			MatchCollection matches = Regex.Matches(wkt, PolygonKey + @"\s*\(([^()]*(?:\(.*?\))?[^()]*)\)");
			foreach (Match match in matches)
				extractedPolygonStrings.Add(PolygonKey + match.Groups[1].Value);

			return extractedPolygonStrings;
		}

		/// <summary>
		/// Parses a POLYGON WKT string into its exterior and interior rings.
		/// </summary>
		/// <param name="wkt">The POLYGON WKT string to be parsed.</param>
		/// <returns>A list representing a polygon, consisting of an exterior ring and zero or more interior rings. Returns null if there no rings.</returns>
		private static List<List<Vector2>>? ParsePolygon(string wkt)
		{
			List<List<Vector2>> rings = new();

			// Use a regular expression to find and extract ring representations from the input string.
			MatchCollection matches = Regex.Matches(wkt, @"\(([^()]*(?:\(.*?\))?[^()]*)\)");

			// Iterate over the matches and parse each ring.
			foreach (Match match in matches)
			{
				// Attempt to parse the ring and add it to the list if it's valid.
				List<Vector2>? ring = ParseRing(match.Groups[1].Value);
				if (ring != null)
					rings.Add(ring);
			}

			return rings.Count > 0 ? rings : null;
		}

		/// <summary>
		/// Parses a ring (either exterior or interior) string into its vertices.
		/// </summary>
		/// <param name="ringStr">The ring string to be parsed.</param>
		/// <returns>List of <see cref="Vector2"/>, representing the vertices of the ring. Returns null if there are no rings.</returns>
		/// <remarks>The first and the last coordinate should be the same for a polygon to be considered a valid WKT polygon.</remarks>
		private static List<Vector2>? ParseRing(string ringStr)
		{
			List<Vector2> ring = new();

			// Use a regular expression to find and extract numeric coordinates from the input string.
			MatchCollection matches = Regex.Matches(ringStr, @"(\-?\d+(\.\d+)?)\s(\-?\d+(\.\d+)?)\s?");

			// Iterate over the matches and parse each coordinate pair.
			foreach (Match match in matches)
			{
				float x = float.Parse(match.Groups[1].Value);
				float y = float.Parse(match.Groups[3].Value);
				ring.Add(new Vector2(x, y));
			}

			return ring.Count > 0 ? ring : null;
		}
	}
}
