#nullable enable

using UnityEngine;

namespace Elephant.UnityLibrary.Uguis
{
	/// <summary>
	/// Restricts the maximum width and height of a UI element.
	/// </summary>
	public class SizeLimiter : MonoBehaviour
	{
		/// <summary>
		/// Maximum UI element width in pixels.
		/// If the width exceeds this value and is greater than or equal to 0, it will be restricted to this maximum width.
		/// </summary>
		public float MaxWidth = -1f;

		/// <summary>
		/// Maximum UI element height.
		/// If the height exceeds this value and is greater than or equal to 0, it will be restricted to this maximum height.
		/// </summary>
		public float MaxHeight = -1f;

		/// <summary>
		/// If true, limits the size in the Awake.
		/// </summary>
		[SerializeField] protected bool LimitSizeInAwake = false;

		/// <summary>
		/// If true, limits the size in the Start.
		/// </summary>
		public bool LimitSizeInStart = true;

		/// <summary>
		/// If true, limits the size in the Update.
		/// </summary>
		public bool LimitSizeInUpdate = true;

		/// <summary>
		/// RectTransform component of the UI element that may have its size restricted.
		/// </summary>
		protected RectTransform? RectTransformToLimit = null;

		/// <summary>
		/// Awake.
		/// </summary>
		private void Awake()
		{
			if (LimitSizeInAwake)
				LimitSize();
		}

		/// <summary>
		/// Start.
		/// </summary>
		private void Start()
		{
			if (LimitSizeInStart)
				LimitSize();
		}

		/// <summary>
		/// Update.
		/// </summary>
		private void Update()
		{
			if (LimitSizeInUpdate)
				LimitSize();
		}

		/// <summary>
		/// Restricts the size of the UI element based on the maxWidth and maxHeight values.
		/// </summary>
		public void LimitSize()
		{
			if (RectTransformToLimit == null)
			{
				RectTransformToLimit = GetComponent<RectTransform>();
				if (RectTransformToLimit == null)
					return;
			}

			// Restrict width.
			if (MaxWidth > 0 && RectTransformToLimit.rect.width > MaxWidth)
				RectTransformToLimit.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxWidth);

			// Restrict height
			if (MaxHeight >= 0 && RectTransformToLimit.rect.height > MaxHeight)
				RectTransformToLimit.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxHeight);
		}
	}
}