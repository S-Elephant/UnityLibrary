#nullable enable

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

		public const string EmptyPoint = "POINT EMPTY";
		public const string EmptyLineString = "LINESTRING EMPTY";
		public const string EmptyMultiPoint = "MULTIPOINT EMPTY";
		public const string EmptyPolygon = "POLYGON EMPTY";
		public const string EmptyMultiLineString = "MULTILINESTRING EMPTY";
		public const string EmptyMultiPolygon = "MULTIPOLYGON EMPTY";
		public const string EmptyGeometryCollection = "GEOMETRYCOLLECTION EMPTY";

		/// <summary>
		/// Parse a WKT string representing either a POLYGON or a MULTIPOLYGON.
		/// More info here: https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry.
		/// </summary>
		/// <param name="wkt">WKT string to be parsed.</param>
		/// <returns>List representing multi-polygons, where each multi-polygon is a list of polygons and each polygon consists of an exterior ring and zero or more interior rings (holes).</returns>
		public static List<List<List<Vector2>>> ParseWkt(string? wkt)
		{
			if (string.IsNullOrEmpty(wkt) ||
			    wkt == EmptyPoint ||
			    wkt == EmptyLineString ||
			    wkt == EmptyMultiPoint ||
			    wkt == EmptyPolygon ||
			    wkt == EmptyMultiLineString ||
			    wkt == EmptyMultiPolygon ||
			    wkt == EmptyGeometryCollection)
			{
				return new List<List<List<Vector2>>>();
			}

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
			List<string> extractedPolygonStrings = new List<string>();
			int openParenCount = 0;
			int start = 0;

			for (int i = 0; i < wkt.Length; i++)
			{
				if (wkt[i] == '(')
				{
					if (openParenCount == 0)
						start = i;
					openParenCount++;
				}
				else if (wkt[i] == ')')
				{
					openParenCount--;
					if (openParenCount == 0)
					{
						// Extract the polygon string and add the POLYGON keyword.
						extractedPolygonStrings.Add(PolygonKey + " " + wkt.Substring(start, i - start + 1));
					}
				}
			}

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

		/// <summary>
		/// Returns true if the <paramref name="geometry"/> is truly a multi-polygon (instead of a regular polygon).
		/// </summary>
		public static bool IsMultiPolygon(List<List<List<Vector2>>> geometry)
		{
			return geometry.Count > 1 || (geometry.Count == 1 && geometry[0].Count > 1);
		}

		/// <summary>
		/// Convert geometry into a POLYGON or MULTIPOLYGON WKT string.
		/// More info here: More info here: https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry.
		/// </summary>
		/// <param name="geometry">Polygon or multi-polygon to be converted into a WKT string.</param>
		/// <param name="forceAsMultiPolygon">If true, the string will always be a MULTIPOLYGON string.</param>
		/// <param name="defaultValue">Default value to return if <paramref name="geometry"/> is empty.</param>
		/// <returns>
		/// WKT string without a space between "MULTIPOLYGON" or "POLYGON" and the first '(' character.
		/// Returns <paramref name="defaultValue"/> if the geometry is empty, has any polygon w/o rings or has any ring with less than 3 points.
		/// </returns>
		public static string ToWktString(List<List<List<Vector2>>> geometry, bool forceAsMultiPolygon = false, string defaultValue = EmptyMultiPolygon)
		{
			if (!geometry.Any() || geometry.Any(polygon => !polygon.Any() || polygon.Any(ring => ring.Count < 3)))
			{
				// Return the default value if:
				// 1. The geometry collection is empty.
				// 2. Any polygon in the collection does not have at least one ring.
				// 3. Any ring in any polygon does not have at least three points.
				return defaultValue;
			}

			StringBuilder sb = new();

			bool isMultiPolygon = forceAsMultiPolygon || geometry.Count > 1 || (geometry.Count == 1 && geometry[0].Count > 1);

			sb.Append(isMultiPolygon ? "MULTIPOLYGON" : "POLYGON");

			for (int i = 0; i < geometry.Count; i++)
			{
				if (i > 0) // We're in a multi-polygon and this is not the first polygon, add a comma
					sb.Append(", ");

				if (isMultiPolygon)
					sb.Append("("); // Start of the polygon

				for (int j = 0; j < geometry[i].Count; j++)
				{
					if (j > 0) // This is not the first ring in the polygon, add a comma
						sb.Append(", ");

					sb.Append("(("); // Start of the ring

					foreach (Vector2 vertex in geometry[i][j])
						sb.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}, ", vertex.x, vertex.y);

					// If the first and last vertices are not equal (this is required for a valid WKT string
					// using polygons or multi-polygons) then duplicate the first item and add it to the last
					// position.
					if (geometry[i][j].Count > 0)
					{
						Vector2 firstVertext = geometry[i][j][0];
						if (geometry[i][j][0] != geometry[i][j][geometry[i][j].Count - 1])
							sb.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}, ", firstVertext.x, firstVertext.y);
					}

					sb.Remove(sb.Length - 2, 2); // Remove the trailing comma and space
					sb.Append("))"); // End of the ring
				}

				sb.Append(")"); // End of the (multi-)polygon
			}

			if (!isMultiPolygon)
				sb.Remove(sb.Length - 1, 1);

			return sb.ToString();
		}
	}
}
