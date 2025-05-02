namespace Elephant.UnityLibrary.Audio
{
	/// <summary>
	/// Defines the behavior for looping audio tracks.
	/// </summary>
	public enum PlaybackLoopModeType
	{
		/// <summary>
		/// Play the current track once, without looping
		/// </summary>
		NoLoop,

		/// <summary>
		/// Loop the current track indefinitely or until stopped.
		/// </summary>
		LoopSingle,

		/// <summary>
		/// Loop randomly through all tracks.
		/// </summary>
		LoopAllRandom,
	}
}
