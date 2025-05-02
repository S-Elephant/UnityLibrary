namespace Elephant.UnityLibrary.Audio
{
	/// <summary>
	/// Audio configuration container.
	/// </summary>
	[System.Serializable]
	public struct AudioConfig
	{
		/// <summary>Master volume.</summary>
		public float MasterVolume;

		/// <summary>SFX volume.</summary>
		public float SfxVolume;

		/// <summary>Determines if SFX should be enabled.</summary>
		public bool IsSfxEnabled;

		/// <summary>Music volume.</summary>
		public float MusicVolume;

		/// <summary>Determines if music should be enabled.</summary>
		public bool IsMusicEnabled;

		/// <summary>Constructor.</summary>
		public AudioConfig(float masterVolume, float sfxVolume, bool isSfxEnabled, float musicVolume, bool isMusicEnabled)
		{
			MasterVolume = masterVolume;
			SfxVolume = sfxVolume;
			IsSfxEnabled = isSfxEnabled;
			MusicVolume = musicVolume;
			IsMusicEnabled = isMusicEnabled;
		}
	}
}