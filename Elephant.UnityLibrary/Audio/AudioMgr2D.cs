using System.Collections.Generic;
using UnityEngine;

namespace Elephant.UnityLibrary.Audio
{
	/// <summary>
	/// <para>Manages audio playback for sound effects and music within a 2D game.
	/// This includes playing, stopping,and toggling sound effects and music tracks.
	/// Uses the singleton pattern.</para>
	///
	/// <para>Usage:
	/// Create a new GameObject in your first scene, attach this script to it and
	/// drag and drop your SFX and music clips into this. Assign the AudioSources and
	/// configure them as needed.
	/// Then later to play something: AudioManager2D.Instance.PlaySfx("Jump")</para>
	/// </summary>
	public class AudioMgr2D : MonoBehaviour
	{
		/// <summary>
		/// Singleton instance of AudioManager2D to ensure only one instance is active.
		/// </summary>
		public static AudioMgr2D Instance { get; private set; }

		/// <summary>
		/// Component for playing sound effects.
		/// </summary>
		[SerializeField] protected AudioSource _sfxSource;

		/// <summary>
		/// Component for playing music.
		/// </summary>
		[SerializeField] protected AudioSource _musicSource;

		/// <summary>
		/// List of sound effect AudioClips available to play.
		/// </summary>
		[SerializeField] protected List<AudioClip> _sfxClips;

		/// <summary>
		/// List of music AudioClips available to play.
		/// </summary>
		[SerializeField] protected List<AudioClip> _musicClips;

		/// <summary>
		/// Library mapping sound effect names to their respective AudioClip.
		/// </summary>
		protected Dictionary<string, AudioClip> _sfxLibrary = new Dictionary<string, AudioClip>();

		/// <summary>
		/// Library mapping music track names to their respective AudioClip.
		/// </summary>
		protected Dictionary<string, AudioClip> _musicLibrary = new Dictionary<string, AudioClip>();

		/// <summary>
		/// Flag to enable or disable sound effects.
		/// </summary>
		public bool IsSfxEnabled { get; protected set; } = true;

		/// <summary>
		/// Flag to enable or disable music playback.
		/// </summary>
		public bool IsMusicEnabled { get; protected set; } = true;

		/// <summary>
		/// Sfx volume, between 0.0f and 1.0f.
		/// </summary>
		public float SfxVolume
		{
			get => _sfxSource.volume;
			set => _sfxSource.volume = value;
		}

		/// <summary>
		/// Music volume, between 0.0f and 1.0f.
		/// </summary>
		public float MusicVolume
		{
			get => _musicSource.volume;
			set => _musicSource.volume = value;
		}

		/// <summary>
		/// Initializes this AudioManager2D instance and prepares the sound effect and music libraries.
		/// Ensures that this object persists between scene loads.
		/// </summary>
		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
				InitializeSelf();
			}
			else
			{
				// Allow no more than 1 instance.
				Destroy(gameObject);
			}
		}

		/// <summary>
		/// Initializes this AudioManager2D instance and prepares the sound effect and music libraries.
		/// </summary>
		private void InitializeSelf()
		{
			// Initialize the sound effects library.
			foreach (var clip in _sfxClips)
				_sfxLibrary[clip.name] = clip;

			// Initialize the music library.
			foreach (var clip in _musicClips)
				_musicLibrary[clip.name] = clip;
		}

		/// <summary>
		/// Optional initialization helper.
		/// </summary>
		public void InitializeSettings(bool isSfxEnabled, float sfxVolume, bool isMusicEnabled, float musicVolume)
		{
			IsSfxEnabled = isSfxEnabled;
			SfxVolume = sfxVolume;
			IsMusicEnabled = isMusicEnabled;
			SfxVolume = sfxVolume;
		}

		/// <summary>
		/// Plays a sound effect identified by name if sound effects are enabled.
		/// </summary>
		/// <param name="sfxKey">Name of the sound effect to play.</param>
		public void PlaySfx(string sfxKey)
		{
			// Note: PlayOneShotallows us to play a clip once, and it can overlap with other
			// clips played by PlayOneShot on the same AudioSource.
			if (IsSfxEnabled)
			{
				if (_sfxLibrary.TryGetValue(sfxKey, out AudioClip clip))
				{
					_sfxSource.PlayOneShot(clip);
				}
				else
				{
					Debug.LogWarning($"Attempted to play an unknown SFX clip, key: {sfxKey}. Aborting.");
				}
			}
		}

		/// <summary>
		/// Plays a music track identified by name if music is enabled. If the music track is already playing, it will not restart.
		/// </summary>
		/// <param name="musicKey">Name of the music track to play.</param>
		public void PlayMusic(string musicKey)
		{
			if (IsMusicEnabled)
			{
				if (_musicLibrary.TryGetValue(musicKey, out AudioClip clip))
				{
					if (_musicSource.clip != clip)
					{
						_musicSource.clip = clip;
						_musicSource.Play();
					}
				}
				else
				{
					Debug.LogWarning($"Attempted to play an unknown music clip, key: {musicKey}. Aborting");
				}
			}
		}

		/// <summary>
		/// Enable or disable SFX. Optionally also stop all running SFX if it's being disabled.
		/// </summary>
		/// <param name="isSfxEnabled">True to enable sound effects, false to disable them.</param>
		/// <param name="stopRunning">True to stop any running SFX if <paramref name="isSfxEnabled"/> is false.</param>
		public void SetSfxEnabled(bool isSfxEnabled, bool stopRunning = true)
		{
			IsSfxEnabled = isSfxEnabled;

			if (isSfxEnabled && stopRunning)
				_sfxSource.Stop();
		}

		/// <summary>
		/// Enable or disable music. Optionally also stop any running music if it's being disabled.
		/// </summary>
		/// <param name="isMusicEnabled">True to music sound effects, false to disable it.</param>
		/// <param name="stopRunning">True to stop the music if <paramref name="isMusicEnabled"/> is false.</param>
		public void SetMusicEnabled(bool isMusicEnabled, bool stopRunning = true)
		{
			IsMusicEnabled = isMusicEnabled;

			if (!isMusicEnabled && stopRunning)
			{
				_musicSource.Stop();
				_musicSource.clip = null;
			}
		}
	}
}