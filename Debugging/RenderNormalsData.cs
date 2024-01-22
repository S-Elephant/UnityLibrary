using UnityEngine;

namespace Elephant.UnityLibrary.Debugging
{
    /// <summary>
    /// Represents settings and functionality for drawing face normals and face vertices.
    /// </summary>
    [System.Serializable]
    public class RenderNormalsData
    {
        /// <summary>
        /// Specifies the type of drawing for the normals.
        /// </summary>
        [SerializeField] private RenderConditionType _renderCondition;

        /// <summary>
        /// Length of the normals when drawn. Defaults to 0.4.
        /// Greater values require more performance when rendered.
        /// </summary>
        [Range(0.01f, 5f)] [SerializeField] public float NormalLength = 0.4f;

        /// <summary>
        /// Color used to draw the normals. This color is customizable via the Inspector.
        /// </summary>
        [SerializeField] public Color NormalColor;

        /// <summary>
        /// Base color used for rendering normals. Defaults to a specific orange color (255, 133, 0, 255).
        /// This color is used as a fallback or default when no other color is specified.
        /// </summary>
        [SerializeField] public Color NormalBaseWireSphereColor = new Color32(255, 128, 128, 255);

        /// <summary>
        /// Base wire sphere size at the start of each normal.
        /// </summary>
        [SerializeField] public float NormalBaseWireSphereSize = 0.015f;

        /// <summary>
        /// Determines if the  wire sphere at the start of each normal should be rendered.
        /// </summary>
        public bool RenderNormalBaseWireSphereSize = true;

        /// <summary>
        /// Determines when normals should be drawn.
        /// </summary>
        private enum RenderConditionType
        {
            /// <summary>
            /// Never render.
            /// </summary>
            Never = 0,

            /// <summary>
            /// Render only when selected.
            /// </summary>
            WhenSelected = 1,

            /// <summary>
            /// Always render.
            /// </summary>
            Always = 2,
        }

        /// <summary>
        /// Constructor with initializers.
        /// </summary>
        /// <param name="normalColor">The color of the normal.</param>
        /// <param name="render">Whether the normal should be rendered.</param>
        public RenderNormalsData(Color normalColor, bool render)
        {
            NormalColor = normalColor;
            _renderCondition = render ? RenderConditionType.WhenSelected : RenderConditionType.Never;
        }

        /// <summary>
        /// Determines if the normal can be drawn based on the current selection state.
        /// </summary>
        /// <param name="isSelected">Whether the object is selected.</param>
        /// <returns>true if the normal can be drawn; otherwise, false.</returns>
        public bool IsRenderingAllowed(bool isSelected)
        {
            return _renderCondition == RenderConditionType.Always ||
                   (_renderCondition == RenderConditionType.WhenSelected && isSelected);
        }
    }
}
