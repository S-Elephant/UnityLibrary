using UnityEngine;

namespace Elephant.UnityLibrary.Extensions
{
	/// <summary>
	/// <see cref="Color"/> extensions.
	/// </summary>
	public static class ColorExtensions
	{
		/// <summary>
		/// Returns a new color with the specified lerped alpha transparency.
		/// </summary>
		/// <param name="color">Base color to adjust.</param>
		/// <param name="alpha">Target alpha value (0 = fully transparent, 1 = fully opaque). Clamped to 0-1.</param>
		/// <returns>Color with the new alpha and original RGB values.</returns>
		public static Color Alpha(this Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha));
		}

		/// <summary>
		/// Returns a new color with the specified lerped alpha transparency.
		/// </summary>
		/// <param name="color">Base color to adjust.</param>
		/// <param name="colorAlpha">Target alpha value (taken from the <paramref name="colorAlpha"/> its alpha value) (0 = fully transparent, 1 = fully opaque). Clamped to 0-1.</param>
		/// <returns>Color with the new alpha and original RGB values.</returns>
		public static Color Alpha(this Color color, Color colorAlpha)
		{
			return new Color(color.r, color.g, color.b, Mathf.Clamp01(colorAlpha.a));
		}

		/// <summary>
		/// Returns a darker version of the color by blending it with black
		/// while preserving the original alpha value.
		/// </summary>
		/// <param name="color">Original color.</param>
		/// <param name="blendFactor">Blending amount (0 = original, 1 = completely black).</param>
		/// <returns>Darkened color with its alpga preserved.</returns>
		public static Color Darken(this Color color, float blendFactor)
		{
			blendFactor = Mathf.Clamp01(blendFactor);
			return Color.Lerp(color, Color.black, blendFactor);
		}

		/// <summary>
		/// Returns a new color with the specified hue while preserving saturation, brightness, and alpha.
		/// </summary>
		/// <param name="color">Base color to adjust.</param>
		/// <param name="hue">Target hue value (0 to 1, where 0 and 1 are red, 0.33 is green, 0.66 is blue).</param>
		/// <returns>Color with the new hue and original alpha.</returns>
		public static Color Hue(this Color color, float hue)
		{
			Color.RGBToHSV(color, out float h, out float s, out float v);
			return Color.HSVToRGB(hue, s, v).Alpha(color.a);
		}

		/// <summary>
		/// Returns a lighter version of the color by blending it with white
		/// while preserving the original alpha value.
		/// </summary>
		/// <param name="color">Original color.</param>
		/// <param name="blendFactor">Blending amount (0 = original, 1 = completely white).</param>
		/// <returns>Lightened color with its alpha preserved.</returns>
		public static Color Lighten(this Color color, float blendFactor)
		{
			blendFactor = Mathf.Clamp01(blendFactor);
			Color result = Color.Lerp(color, Color.white, blendFactor);
			result.a = color.a;

			return result;
		}

		/// <summary>
		/// Returns a new color with adjusted saturation while preserving hue, brightness, and alpha.
		/// </summary>
		/// <param name="color">Base color to adjust.</param>
		/// <param name="saturation">Target saturation (0 = grayscale, 1 = fully saturated). Clamped to 0-1.</param>
		/// <returns>Color with the new saturation and original alpha.</returns>
		public static Color Saturate(this Color color, float saturation)
		{
			Color.RGBToHSV(color, out float h, out float s, out float v);
			return Color.HSVToRGB(h, Mathf.Clamp01(saturation), v).Alpha(color.a);
		}
	}
}
