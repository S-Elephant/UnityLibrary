using UnityEngine;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// World texts.
	/// </summary>
	public static class WorldTexts
	{
		/// <summary>
		/// Default world text sorting order.
		/// </summary>
		public const int DefaultSortingOrder = 5000;

		/// <summary>
		/// Create Text in the World.
		/// </summary>
		public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, int fontSize = 40, float characterSize = 0.03f, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = DefaultSortingOrder)
		{
			if (color == null) color = Color.white;
			return CreateWorldText(parent, text, localPosition, fontSize, characterSize, (Color)color, textAnchor, textAlignment, sortingOrder);
		}

		/// <summary>
		/// Create Text in the World.
		/// </summary>
		public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, float characterSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
		{
			GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
			Transform transform = gameObject.transform;
			transform.SetParent(parent, false);
			transform.localPosition = localPosition;
			TextMesh textMesh = gameObject.GetComponent<TextMesh>();
			textMesh.anchor = textAnchor;
			textMesh.alignment = textAlignment;
			textMesh.text = text;
			textMesh.fontSize = fontSize;
			textMesh.characterSize = characterSize;
			textMesh.color = color;
			textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
			return textMesh;
		}
	}
}
