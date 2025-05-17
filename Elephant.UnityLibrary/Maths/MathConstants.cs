namespace Elephant.UnityLibrary.Maths
{
	/// <summary>
	/// (Unity) Game specific mathematic constants that don't fit anywhere else.
	/// </summary>
	public static class MathConstants
	{
		/// <summary>
		/// Tolerance value for floating point comparisons that is considered safe for games.
		/// </summary>
		/// <remarks><see cref="float.Epsilon"/> is NOT safe for use in games ebcause it's too small.</remarks>
		public const float SafeGameTolerance = 1e-6f;
	}
}
