﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Elephant.UnityLibrary.GeoSystems.Wkts
{
	/// <summary>
	/// WKT (=Well Known Text) parser for Unity that supports multi-polygons and those with holes.
	/// Note that WKT exterior rings MUST be counter-clockwise and interior rings must be clockwise.
	/// For more info see: https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry
	/// </summary>
	public static class WktPolygonParser
	{
		private const string PolygonKey = "POLYGON";
		private const string MultiPolygonKey = "MULTIPOLYGON";

#pragma warning disable SA1600 // Elements should be documented.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member.
		public const string EmptyPoint = "POINT EMPTY";
		public const string EmptyLineString = "LINESTRING EMPTY";
		public const string EmptyMultiPoint = "MULTIPOINT EMPTY";
		public const string EmptyPolygon = "POLYGON EMPTY";
		public const string EmptyMultiLineString = "MULTILINESTRING EMPTY";
		public const string EmptyMultiPolygon = "MULTIPOLYGON EMPTY";
		public const string EmptyGeometryCollection = "GEOMETRYCOLLECTION EMPTY";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member.
#pragma warning restore SA1600 // Elements should be documented.

		/// <summary>
		/// Parse a WKT string representing either a POLYGON or a MULTIPOLYGON.
		/// More info here: https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry.
		/// </summary>
		/// <param name="wkt">WKT string to be parsed.</param>
		/// <returns>List representing multi-polygons, where each multi-polygon is a list of polygons and each polygon consists of an exterior ring and zero or more interior rings (holes).</returns>
		public static List<List<List<Vector2>>> ParseWkt(string? wkt)
		{
			// Handle null and empty values.
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

			// Trim whitespace from the input string for cleaner processing.
			wkt = wkt!.Trim();
			List<List<List<Vector2>>> multiPolygon = new();

			// Check if the input WKT represents a single polygon.
			if (wkt.StartsWith(PolygonKey))
			{
				// Parse the Polygon and add it to the MultiPolygon list as a single polygon.
				List<List<Vector2>>? polygon = ParsePolygon(wkt);
				if (polygon != null)
					multiPolygon.Add(polygon);
				return multiPolygon;
			}

			// Determine if the input WKT represents a multi-polygon.
			if (wkt.StartsWith(MultiPolygonKey))
			{
				List<string> polygonsWkt = SplitMultiPolygonIntoPolygons(wkt);
				foreach (string polygonWkt in polygonsWkt)
				{
					List<List<Vector2>>? polygons = ParsePolygon(polygonWkt);
					if (polygons != null)
						multiPolygon.Add(polygons);
				}

				return multiPolygon;
			}

			// Unrecognized or not supported.
			return new List<List<List<Vector2>>>();
		}

		/// <summary>
		/// Splits a multi-polygon string into polygon strings.
		/// </summary>
		/// <param name="wktString">Valid MULTIPOLYGON or POLYGON WKT string.</param>
		public static List<string> SplitMultiPolygonIntoPolygons(string wktString)
		{
			List<string> polygons = new();

			// Check if the input is a multi-polygon.
			if (wktString.StartsWith(MultiPolygonKey))
			{
				// Remove 'MULTIPOLYGON' keyword and trim outer parentheses.
				string trimmed = wktString.Substring(MultiPolygonKey.Length).Trim().Trim('(', ')');

				// Split the trimmed content by ")), ((" which separates individual polygons
				string[] parts = trimmed.Split(new[] { ")), ((" }, StringSplitOptions.RemoveEmptyEntries);

				foreach (string part in parts)
				{
					string formattedPart = part;
					// Ensure correct formatting for polygons, especially handling inner rings correctly.
					if (!formattedPart.StartsWith("("))
						formattedPart = "(" + formattedPart;
					if (!formattedPart.EndsWith(")"))
						formattedPart = formattedPart + ")";
					polygons.Add($"{PolygonKey} ({formattedPart})");
				}
			}
			else if (wktString.StartsWith(PolygonKey))
			{
				// Pattern to match each polygon within the polygon.
				const string pattern = @"(\(\([^)]+\)\))";
				MatchCollection matches = Regex.Matches(wktString, pattern);

				foreach (Match match in matches)
					polygons.Add(($"{PolygonKey} " + match.Groups[1].Value).Replace("(((", "((".Replace(")))", "))")));
			}

			return polygons;
		}

		/// <summary>
		/// Parses a POLYGON WKT string into its exterior and interior rings.
		/// </summary>
		/// <param name="wkt">The POLYGON WKT string to be parsed.</param>
		/// <returns>A list representing a polygon, consisting of an exterior ring and zero or more interior rings. Returns null if there no rings.</returns>
		private static List<List<Vector2>>? ParsePolygon(string wkt)
		{
			List<List<Vector2>> rings = new();

			// Remove the POLYGON keyword and trim the remaining string.
			string polygonContent = SanitizeWktString(wkt).Replace($"{PolygonKey} ", "");

			// Trim the leading and trailing parentheses.
			if (!string.IsNullOrEmpty(polygonContent) && polygonContent.Length > 2)
				polygonContent = polygonContent.Substring(1, polygonContent.Length - 2);

			// Split the content by "), (" to separate rings.
			string[] ringContents = polygonContent.Split(new[] { "), (" }, StringSplitOptions.RemoveEmptyEntries);

			//int ringCount = 0;
			for (int index = 0; index < ringContents.Length; index++)
			{
				string ringContent = ringContents[index];
				if (!ringContent.StartsWith("("))
					ringContent = $"({ringContent}";
				if (!ringContent.EndsWith(")"))
					ringContent = $"{ringContent})";

				List<Vector2>? ring = ParseRing(ringContent.Trim());
				if (ring != null)
					rings.Add(ring);
			}

			return rings.Count > 0 ? rings : null;
		}

		/// <summary>
		/// Parses a ring (either exterior or interior) string into its vertices.
		/// </summary>
		/// <param name="ringStr">The ring string to be parsed. Valid example value: "(30 20, 45 40, 10 40, 30 20)"</param>
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
						Vector2 firstVertex = geometry[i][j][0];
						if (geometry[i][j][0] != geometry[i][j][geometry[i][j].Count - 1])
							sb.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}, ", firstVertex.x, firstVertex.y);
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

		/// <summary>
		/// Removes invalid characters and applies some other santiations to the <paramref name="wktString"/> and return it.
		/// </summary>
		public static string SanitizeWktString(string wktString)
		{
			return wktString.Replace("),(", "), (").Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
		}
	}
}
